import { useContext, useEffect, useMemo, useState } from "react";
import AppContext from "./context/appContext";
import ListGroup from "./common/listGroup";
import { getDateInCLientZone, getDateTimeInUTC } from "./../utils/helpers";
import { StudentContextProvider } from "./context/studentContext";
import StudentsAttendanceTable from "./studentsAttendanceTable";
import AttendanceService from "../services/attendanceService";

const Attendance = () => {
  const { specializes } = useContext(AppContext);
  const specializeList = useMemo(() => specializes.get() || [], [specializes]);
  const [students, setStudents] = useState({});
  const [query, setQuery] = useState({
    paginate: {
      number: 1,
      size: 10,
    },
    sort: {
      sortBy: "firstName",
      isAscending: true,
    },
    filters: {
      date: getDateInCLientZone(new Date()),
      specializeId: 0,
    },
  });
  const [lastUpdate, setLastUpdate] = useState(new Date());

  useEffect(() => {
    if (!specializeList.length) return;

    if (!query.filters.specializeId) {
      query.filters.specializeId = specializeList[0].id;
      return setQuery(query);
    }

    const queryResource = JSON.parse(JSON.stringify(query));
    queryResource.filters.date = getDateTimeInUTC(queryResource.filters.date);
    AttendanceService.getStudentsAsync(queryResource)
      .then(({ data: students }) => {
        if (students.data.length || queryResource.paginate.number === 1)
          return setStudents(students);

        const newQuery = { ...query };
        newQuery.paginate.number = 1;
        setQuery(newQuery);
      })
      .catch(() => setStudents({}));
  }, [query, lastUpdate, specializes, specializeList]);

  const handleSpecializeSelect = ({ id }) => {
    const studentsQuery = { ...query };
    studentsQuery.filters.specializeId = id;
    setQuery(studentsQuery);
  };

  const handleDateChange = (event) => {
    const newQuery = { ...query };
    const date = event.target.value;
    newQuery.filters.date = date;
    setQuery(newQuery);
  };

  const handleSort = (sort) => {
    const newQuery = { ...query };
    newQuery.sort = sort;
    setQuery(newQuery);
  };

  const handlePageChange = (pageNumber) => {
    const newQuery = { ...query };
    newQuery.paginate.number = pageNumber;
    setQuery(newQuery);
  };

  return (
    <StudentContextProvider
      value={{
        reloadTable: () => setLastUpdate(new Date()),
        date: { get: () => getDateTimeInUTC(query.filters.date) },
      }}
    >
      <h4 style={{ marginTop: "10px", marginBottom: "10px" }}>Attendance</h4>
      <hr />
      <div className="row">
        <div className="col-2">
          <h4>Specialize</h4>
          <ListGroup
            selectedItem={query.filters.specializeId}
            items={specializeList}
            onItemSelect={handleSpecializeSelect}
            key="specializeId"
          />
        </div>
        <div className="col" style={{ overflowX: "scroll" }}>
          <h4>Date</h4>
          <input
            className="form-control mb-3"
            type="date"
            value={getDateInCLientZone(query.filters.date)}
            onChange={handleDateChange}
            name="date"
          />
          <hr />
          <p>
            {students.total
              ? `Showing from ${
                  (query.paginate.number - 1) * query.paginate.size + 1
                } to ${
                  (query.paginate.number - 1) * query.paginate.size +
                  students.data.length
                } from ${students.total} students`
              : "There are no students yet."}
          </p>
          {students.total > 0 && (
            <StudentsAttendanceTable
              students={students.data}
              sort={query.sort}
              pageSize={query.paginate.size}
              currentPage={query.paginate.number}
              itemsCount={students.total}
              onPageChange={handlePageChange}
              handleSort={handleSort}
            />
          )}
        </div>
      </div>
    </StudentContextProvider>
  );
};

export default Attendance;
