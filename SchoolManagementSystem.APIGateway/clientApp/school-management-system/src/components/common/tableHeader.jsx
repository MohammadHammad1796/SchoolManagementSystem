import TableHeaderColumn from "./tableHeaderColumn";

const TableHeader = (props) => {
  const { headers, onSort, currentSort } = props;
  return (
    <thead>
      <tr>
        {headers.map((header) => (
          <TableHeaderColumn
            identifier={header.path}
            label={header.label}
            onSort={onSort}
            isSortable={header.isSortable}
            isVisible={header.isVisible}
            sort={currentSort}
            key={header.path}
          />
        ))}
      </tr>
    </thead>
  );
};

export default TableHeader;
