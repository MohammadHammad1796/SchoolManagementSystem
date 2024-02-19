import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowUpLong as up } from "@fortawesome/free-solid-svg-icons";
import { faArrowDownLong as down } from "@fortawesome/free-solid-svg-icons";

const TableHeaderColumn = (props) => {
  let { identifier, label, onSort, isSortable, sort, isVisible } = props;
  isSortable = isSortable === undefined ? true : false;
  isVisible = isVisible === undefined ? true : false;

  const raiseSort = (currentSort, identifier) => {
    currentSort.isAscending =
      currentSort.sortBy === identifier
        ? !currentSort.isAscending
        : currentSort.isAscending;
    currentSort.sortBy = identifier;
    onSort(currentSort);
  };

  return (
    isVisible && (
      <th
        onClick={() => isSortable && raiseSort(sort, identifier)}
        title={label}
        style={{ userSelect: "none", cursor: "pointer" }}
      >
        {label}
        {sort.sortBy === identifier &&
          (sort.isAscending === true ? (
            <FontAwesomeIcon icon={up} style={{ float: "right" }} />
          ) : (
            <FontAwesomeIcon icon={down} style={{ float: "right" }} />
          ))}
      </th>
    )
  );
};

export default TableHeaderColumn;
