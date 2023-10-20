import DataTable from "./common/dataTable";
import React, { useContext } from "react";
import StudentContext from "./context/studentContext";
import { toast } from "react-toastify";
import AttendanceService from "../services/attendanceService";

const StudentsAttendanceTable = (props) => {
  const {
    students,
    sort,
    pageSize,
    currentPage,
    itemsCount,
    onPageChange,
    handleSort,
  } = props;

  const { reloadTable, date } = useContext(StudentContext);

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
      path: "isAttended",
      label: "Status",
      isSortable: false,
      content: (student) => {
        if (student.isAttended) return <span>Attend</span>;
        if (student.isAttended === false) return <span>Absence</span>;
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
            disabled={student.isAttended}
            style={{ margin: "5px", display: "block" }}
            onClick={async () => {
              try {
                await AttendanceService.saveAttendance({
                  studentId: student.id,
                  isAttended: true,
                  date: date.get(),
                });
                toast.success("Student attended successfully.");
                reloadTable();
              } catch (_) {}
            }}
          >
            Attend
          </button>
          <button
            className="btn btn-danger"
            disabled={student.isAttended === false}
            style={{ margin: "5px", display: "block" }}
            onClick={async () => {
              try {
                await AttendanceService.saveAttendance({
                  studentId: student.id,
                  isAttended: false,
                  date: date.get(),
                });
                toast.success("Student absence successfully.");
                reloadTable();
              } catch (_) {}
            }}
          >
            Absence
          </button>
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

export default StudentsAttendanceTable;
