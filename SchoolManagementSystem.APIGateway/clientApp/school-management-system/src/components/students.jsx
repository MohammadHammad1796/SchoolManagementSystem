import { useContext, useEffect } from "react";
import ListGroup from "./common/listGroup";
import AdmissionService from "../services/admissionService";
import { useState } from "react";
import StudentsTable from "./studentsTable";
import AppContext from "./context/appContext";
import { StudentContextProvider } from "./context/studentContext";

const Students = () => {
  const { specializes } = useContext(AppContext);
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
      status: 0,
      specializeId: 0,
    },
  });
  const [lastUpdate, setLastUpdate] = useState(new Date());

  useEffect(() => {
    const populateData = async () => {
      let students = await AdmissionService.getStudentsAsync(query);
      if (students.data.length === 0 && query.paginate.number !== 1) {
        query.paginate.number = 1;
        students = await AdmissionService.getStudentsAsync(query);
      }
      setStudents(students.data);
    };

    try {
      populateData();
    } catch (_) {}
  }, [lastUpdate, query]);

  const statusList = [
    {
      id: 0,
      name: "All",
    },
    {
      id: "accepted",
      name: "Accepted",
    },
    {
      id: "rejected",
      name: "Rejected",
    },
    {
      id: "reviewed",
      name: "Reviewed",
    },
    {
      id: "notReviewed",
      name: "Not reviewed",
    },
  ];

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

  const handleListGroupSelect = async (selectName, { id }) => {
    const newQuery = { ...query };
    newQuery.filters[selectName] = id;
    newQuery.paginate.number = 1;
    setQuery(newQuery);
  };

  const { filters, sort } = query;
  return (
    <StudentContextProvider
      value={{
        setLastUpdate: (date) => setLastUpdate(date),
      }}
    >
      <div className="row">
        <div className="col-2">
          <h4>Status</h4>
          <ListGroup
            selectedItem={filters.status}
            items={statusList}
            onItemSelect={(item) => handleListGroupSelect("status", item)}
            key="status"
          />
          <hr />
          <h4>Specialize</h4>
          <ListGroup
            selectedItem={filters.specializeId}
            items={specializeList}
            onItemSelect={(item) => handleListGroupSelect("specializeId", item)}
            key="specializeId"
          />
        </div>
        <div className="col" style={{ overflowX: "scroll" }}>
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
            <StudentsTable
              students={students.data}
              sort={sort}
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

export default Students;
