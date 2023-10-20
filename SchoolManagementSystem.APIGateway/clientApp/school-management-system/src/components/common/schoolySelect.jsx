import { PropTypes } from "prop-types";
import Select from "react-select";

const SchoolySelect = ({ label, name, onSelectChange, error, ...rest }) => {
  const emptyOption = { label: "", value: 0 };
  rest.options = rest.options.map((option) => ({
    label: option.name,
    value: option.id,
  }));
  rest.options.splice(0, 0, ...[emptyOption]);
  rest.value = rest.options.find((o) => o.value === rest.value) || emptyOption;

  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <Select
        className="form-control"
        name={name}
        onChange={onSelectChange}
        {...rest}
      />
      {error && <div className="alert alert-danger">{error}</div>}
    </div>
  );
};

SchoolySelect.propTypes = {
  label: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  value: PropTypes.number.isRequired,
  onSelectChange: PropTypes.func.isRequired,
  error: PropTypes.string,
};

export default SchoolySelect;
