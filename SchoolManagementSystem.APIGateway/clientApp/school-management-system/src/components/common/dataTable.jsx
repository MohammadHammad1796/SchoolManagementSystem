import TableHeader from "./tableHeader";
import TableBody from "./tableBody";
import Pagination from "./pagination";
import React from "react";

const DataTable = (props) => {
  const {
    items,
    columns,
    onSort,
    currentSort,
    identifier,
    pageSize,
    currentPage,
    itemsCount,
    onPageChange,
  } = props;
  return (
    <React.Fragment>
      <table className="table table-bordered table-hover">
        <TableHeader
          headers={columns}
          onSort={onSort}
          currentSort={currentSort}
        />
        <TableBody items={items} columns={columns} identifier={identifier} />
      </table>
      <Pagination
        pageSize={pageSize}
        currentPage={currentPage}
        itemsCount={itemsCount}
        onPageChange={onPageChange}
      />
    </React.Fragment>
  );
};

export default DataTable;
