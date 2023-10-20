import { PropTypes } from "prop-types";
import "../../assets/styles/schoolyRadio.css";

const SchoolyRadio = ({ label, name, value, onValueChange, error }) => {
  const trueId = name + "1";
  const falseId = name + "0";

  return (
    <div className="form-group">
      <label htmlFor={name} className="radio-form-group-label">
        {label}
      </label>
      <div className="form-check form-radio">
        <input
          className="form-check-input"
          type="radio"
          name={name}
          id={trueId}
          onChange={onValueChange}
          value={true}
          checked={value === true}
        />
        <label className="form-check-label" htmlFor={trueId}>
          Male
        </label>
      </div>
      <div className="form-check form-radio">
        <input
          className="form-check-input"
          type="radio"
          name={name}
          id={falseId}
          onChange={onValueChange}
          value={false}
          checked={value === false}
        />
        <label className="form-check-label" htmlFor={falseId}>
          Female
        </label>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}
    </div>
  );
};

SchoolyRadio.propTypes = {
  label: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  value: PropTypes.bool.isRequired,
  onValueChange: PropTypes.func.isRequired,
  error: PropTypes.string,
};

export default SchoolyRadio;
