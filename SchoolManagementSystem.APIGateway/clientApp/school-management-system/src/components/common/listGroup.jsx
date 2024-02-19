const ListGroup = (props) => {
  const {
    selectedItem,
    items,
    onItemSelect,
    valueProperty,
    textProperty,
    ...rest
  } = props;

  const isSelectedItem = (item) => {
    return selectedItem === item[valueProperty];
  };

  return (
    <ul className="list-group" {...rest}>
      {items.map((item) => (
        <li
          className={`list-group-item ${isSelectedItem(item) && "active"}`}
          style={{ userSelect: "none", cursor: isSelectedItem(item) ? "auto" : "pointer" }}
          aria-current={isSelectedItem(item) && "true"}
          onClick={() => !isSelectedItem(item) && onItemSelect(item)}
          key={item[valueProperty]}
        >
          {item[textProperty]}
        </li>
      ))}
    </ul>
  );
};

ListGroup.defaultProps = {
  textProperty: "name",
  valueProperty: "id",
};

export default ListGroup;
