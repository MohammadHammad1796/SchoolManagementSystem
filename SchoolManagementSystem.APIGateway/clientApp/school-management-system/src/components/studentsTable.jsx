import DataTable from "./common/dataTable";
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import AppContext from "./context/appContext";
import AdmissionService from "../services/admissionService";
import StudentContext from "./context/studentContext";
import { toast } from "react-toastify";
import { getDateInCLientZone } from "../utils/helpers";

const StudentsTable = (props) => {
  const {
    students,
    sort,
    pageSize,
    currentPage,
    itemsCount,
    onPageChange,
    handleSort,
  } = props;

  const { specializes } = useContext(AppContext);
  const { setLastUpdate } = useContext(StudentContext);

  const columns = [
    {
      path: "id",
      isVisible: false,
    },
    {
      path: "firstName",
      label: "First name",
    },
    {
      path: "lastName",
      label: "Last name",
    },
    {
      path: "fatherFirstName",
      label: "Father first name",
    },
    {
      path: "motherFullName",
      label: "Mother full name",
    },
    {
      path: "isMale",
      label: "Gender",
      content: (student) => <span>{student.isMale ? "Male" : "Female"}</span>,
    },
    {
      path: "dateOfBirth",
      label: "Date of birth",
      content: (student) => (
        <span>{getDateInCLientZone(student.dateOfBirth)}</span>
      ),
    },
    {
      path: "specializeId",
      label: "Specialize",
      content: (student) => {
        const specializeList = specializes.get();
        const specialize = specializeList.find(
          (s) => s.id === student.specializeId
        );
        return <span>{specialize?.name || ""}</span>;
      },
    },
    {
      path: "certificateDate",
      label: "Certificate date",
      content: (student) => (
        <span>{getDateInCLientZone(student.certificateDate)}</span>
      ),
    },
    {
      path: "certificateAverage",
      label: "Certificate average",
    },
    {
      path: "status",
      label: "Status",
      content: (student) => {
        if (student.isAccepted) return <span>Accepted</span>;
        if (!student.isAccepted && student.reviewed)
          return <span>Rejected</span>;
        return <span>Not reviewed</span>;
      },
    },
    {
      path: "actions",
      label: "Actions",
      isSortable: false,
      content: (student) => (
        <>
          <button
            className="btn btn-primary"
            disabled={student.isAccepted}
            style={{ margin: "5px", display: "block" }}
            onClick={() => {
              AdmissionService.admitStudentAsync(student.id, true)
                .then(() => {
                  toast.success("Student accepted successfully.");
                  setLastUpdate(new Date());
                })
                .catch(() => {});
            }}
          >
            Accept
          </button>
          <button
            className="btn btn-danger"
            disabled={!student.isAccepted && student.reviewed}
            style={{ margin: "5px", display: "block" }}
            onClick={() => {
              AdmissionService.admitStudentAsync(student.id, false)
                .then(() => {
                  toast.success("Student rejected successfully.");
                  setLastUpdate(new Date());
                })
                .catch(() => {});
            }}
          >
            Reject
          </button>
          {student.isAccepted && (
            <Link
              to={`/students/courses/${student.id}`}
              style={{ margin: "5px", display: "block" }}
            >
              Courses
            </Link>
          )}
        </>
      ),
    },
  ];

  return (
    <DataTable
      items={students}
      columns={columns}
      onSort={handleSort}
      currentSort={sort}
      identifier={"id"}
      pageSize={pageSize}
      currentPage={currentPage}
      itemsCount={itemsCount}
      onPageChange={onPageChange}
    />
  );
};

export default StudentsTable;
