import { useContext, useEffect } from "react";
import ListGroup from "./common/listGroup";
import CourseService from "../services/courseService";
import { useState } from "react";
import AppContext from "./context/appContext";
import CoursesTable from "./coursesTable";
import { useNavigate } from "react-router-dom";
import { CourseContextProvider } from "./context/courseContext";

const Courses = () => {
  const { specializes } = useContext(AppContext);
  const navigate = useNavigate();

  const [courses, setCourses] = useState({});
  const [query, setQuery] = useState({
    paginate: {
      number: 1,
      size: 10,
    },
    sort: {
      sortBy: "name",
      isAscending: true,
    },
    filters: {
      specializeId: 0,
    },
  });
  const [lastUpdate, setLastUpdate] = useState(new Date());

  useEffect(() => {
    const populateData = async () => {
      let courses = await CourseService.getCoursesAsync(query);
      if (courses.data.length === 0 && query.paginate.number !== 1) {
        query.paginate.number = 1;
        courses = await CourseService.getCoursesAsync(query);
      }
      setCourses(courses.data);
    };

    try {
      populateData();
    } catch (_) {}
  }, [lastUpdate, query]);

  const specializeList = [{ id: 0, name: "All" }, ...specializes.get()];

  const handleSort = async (sort) => {
    const newQuery = { ...query };
    newQuery.sort = sort;
    setQuery(newQuery);
  };

  const handlePageChange = async (pageNumber) => {
    const newQuery = { ...query };
    newQuery.paginate.number = pageNumber;
    setQuery(newQuery);
  };

  const handleListGroupSelect = async ({ id }) => {
    const newQuery = { ...query };
    newQuery.filters.specializeId = id;
    newQuery.paginate.number = 1;
    setQuery(newQuery);
  };

  const { filters, sort } = query;
  return (
    <CourseContextProvider
      value={{
        setLastUpdate: (date) => setLastUpdate(date),
      }}
    >
      <div className="row">
        <div className="col-2">
          <h4>Specialize</h4>
          <ListGroup
            selectedItem={filters.specializeId}
            items={specializeList}
            onItemSelect={handleListGroupSelect}
            key="specializeId"
          />
        </div>
        <div className="col" style={{ overflowX: "scroll" }}>
          <button
            className="btn btn-primary"
            style={{ margin: "5px" }}
            onClick={() => navigate("/courses/new", { replace: false })}
          >
            New course
          </button>
          <p>
            {courses.total
              ? `Showing from ${
                  (query.paginate.number - 1) * query.paginate.size + 1
                } to ${
                  (query.paginate.number - 1) * query.paginate.size +
                  courses.data.length
                } from ${courses.total} courses`
              : "There are no courses yet."}
          </p>
          {courses.total > 0 && (
            <CoursesTable
              courses={courses.data}
              sort={sort}
              pageSize={query.paginate.size}
              currentPage={query.paginate.number}
              itemsCount={courses.total}
              onPageChange={handlePageChange}
              handleSort={handleSort}
            />
          )}
        </div>
      </div>
    </CourseContextProvider>
  );
};

export default Courses;
