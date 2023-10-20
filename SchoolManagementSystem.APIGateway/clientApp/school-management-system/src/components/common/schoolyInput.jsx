import { PropTypes } from "prop-types";
import { getDateInCLientZone } from "../../utils/helpers";

const SchoolyInput = ({
  label,
  name,
  onValueChange,
  error,
  placeHolder,
  ...rest
}) => {
  if (rest && rest.type === "date" && rest.value instanceof Date) {
    rest.value = getDateInCLientZone(rest.value);
  }

  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <input
        className="form-control"
        onChange={onValueChange}
        placeholder={placeHolder}
        name={name}
        {...rest}
      />
      {error && <div className="alert alert-danger">{error}</div>}
    </div>
  );
};

SchoolyInput.propTypes = {
  label: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  value: PropTypes.any.isRequired,
  onValueChange: PropTypes.func.isRequired,
  error: PropTypes.string,
  placeHolder: PropTypes.string,
};

export default SchoolyInput;
