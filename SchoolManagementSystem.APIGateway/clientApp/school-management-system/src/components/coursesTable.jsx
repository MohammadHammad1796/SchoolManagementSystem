import DataTable from "./common/dataTable";
import React, { useContext } from "react";
import AppContext from "./context/appContext";
import CourseContext from "./context/courseContext";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import CourseService from "../services/courseService";

const CoursesTable = (props) => {
  const {
    courses,
    sort,
    pageSize,
    currentPage,
    itemsCount,
    onPageChange,
    handleSort,
  } = props;

  const { specializes } = useContext(AppContext);
  const { setLastUpdate } = useContext(CourseContext);
  const navigate = useNavigate();

  const columns = [
    {
      path: "id",
      isVisible: false,
    },
    {
      path: "name",
      label: "Name",
    },
    {
      path: "specializeId",
      label: "Specialize",
      isSortable: false,
      content: (course) => {
        const specializeList = specializes.get();
        const specialize = specializeList.find(
          (s) => s.id === course.specializeId
        );
        return <span>{specialize?.name || ""}</span>;
      },
    },
    {
      path: "actions",
      label: "Actions",
      isSortable: false,
      content: (course) => (
        <>
          <button
            className="btn btn-warning"
            style={{ margin: "5px" }}
            onClick={() =>
              navigate("/courses/" + course.id, { replace: false })
            }
          >
            Edit
          </button>
          <button
            className="btn btn-danger"
            style={{ margin: "5px" }}
            onClick={() => {
              CourseService.deleteAsync(course.id)
                .then(() => {
                  toast.success("Course deleted successfully.");
                  setLastUpdate(new Date());
                })
                .catch(() => {});
            }}
          >
            Delete
          </button>
        </>
      ),
    },
  ];

  return (
    <DataTable
      items={courses}
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

export default CoursesTable;
