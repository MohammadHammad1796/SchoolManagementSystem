import _ from "lodash";

const TableBody = ({ items, columns, identifier }) => {
  return (
    <tbody>
      {items.map((item) => (
        <tr key={item[identifier]}>
          {columns.map(
            (column) =>
              column.isVisible !== false && (
                <td
                  key={column.path}
                  title={
                    column.content === undefined ? _.get(item, column.path) : ""
                  }
                >
                  {column.content
                    ? column.content(item)
                    : _.get(item, column.path) || ""}
                </td>
              )
          )}
        </tr>
      ))}
    </tbody>
  );
};

export default TableBody;
