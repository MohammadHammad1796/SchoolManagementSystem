import { useState } from "react";
import Joi from "joi-browser";
import SchoolyInput from "./schoolyInput";
import SchoolySelect from "./schoolySelect";
import SchoolyRadio from "./schoolyRadio";
import "../../assets/styles/form.css";
import { isFunctionAsync } from "../../utils/helpers";
import { useEffectOnInitialRender } from "../../hooks/useEffect";

const Form = ({
  formTitle,
  inputs,
  initialData,
  schema,
  schemaMessages,
  handleSubmit,
  submitLabel,
  setDataOnInitialRender,
}) => {
  const [data, setData] = useState(initialData);
  const [errors, setErrors] = useState({});

  useEffectOnInitialRender(() => {
    if (setDataOnInitialRender && typeof setDataOnInitialRender === "function")
      setDataOnInitialRender(setData);
  });

  const handleInputChange = ({ target: input }) => {
    const newData = { ...data };
    const newErrors = { ...errors };
    const errorMessage = validateProperty(input);
    if (errorMessage) newErrors[input.name] = errorMessage;
    else delete newErrors[input.name];

    newData[input.name] = input.value;

    setData(newData);
    setErrors(newErrors);
  };

  const handleSelectChange = (selectedOption, event, label) => {
    const { name } = event;
    const newData = { ...data };
    const newErrors = { ...errors };

    const errorMessage = validateProperty({
      name,
      value: selectedOption.value,
    });
    if (errorMessage) newErrors[name] = errorMessage;
    else delete newErrors[name];

    newData[name] = selectedOption.value;
    setData(newData);
    setErrors(newErrors);
  };

  const handleRadioChange = ({ target: input }) => {
    const newData = { ...data };
    const newErrors = { ...errors };

    let { name, value } = input;
    value = value === true || value === "true";
    const errorMessage = validateProperty({
      name,
      value,
    });
    if (errorMessage) newErrors[name] = errorMessage;
    else delete newErrors[name];

    newData[name] = value;
    setData(newData);
    setErrors(newErrors);
  };

  const validateProperty = ({ name, value }) => {
    const { error } = Joi.validate(value, schema[name]);
    if (!error) return "";

    if (
      schemaMessages &&
      schemaMessages[name] &&
      schemaMessages[name][error.details[0].type]
    )
      return schemaMessages[name][error.details[0].type];
    return error.details[0].message;
  };

  const validate = () => {
    const options = { abortEarly: false, allowUnknown: true };
    const { error } = Joi.validate(data, schema, options);
    if (!error) return null;

    const errors = {};
    for (const item of error.details) {
      if (
        schemaMessages &&
        schemaMessages[item.path] &&
        schemaMessages[item.path][item.type]
      )
        errors[item.path[0]] = schemaMessages[item.path][item.type];
      else errors[item.path[0]] = item.message;
    }
    return errors;
  };

  const doSubmit = async (event) => {
    event.preventDefault();

    const errors = validate();
    setErrors(errors || {});
    if (errors) return;

    let newErrors;

    if (typeof handleSubmit === "function") {
      if (isFunctionAsync(handleSubmit))
        newErrors = await handleSubmit(data);
    }

    if (newErrors && Object.keys(newErrors).length) setErrors(newErrors);
  };

  const renderSubmitButton = (label) => {
    return (
      <button type="submit" className="btn btn-primary" disabled={validate()}>
        {label}
      </button>
    );
  };

  const renderInput = ({ path, label, type, placeHolder }) => {
    return (
      <SchoolyInput
        key={path}
        label={label}
        name={path}
        type={type}
        value={data[path]}
        onValueChange={handleInputChange}
        error={errors[path]}
        placeHolder={placeHolder}
      />
    );
  };

  const renderRadio = ({ path, label }) => {
    return (
      <SchoolyRadio
        key={path}
        label={label}
        name={path}
        value={data[path]}
        onValueChange={handleRadioChange}
        error={errors[path]}
      />
    );
  };

  const renderSelect = ({ path, label, options }) => {
    return (
      <SchoolySelect
        key={path}
        label={label}
        name={path}
        options={options}
        value={data[path]}
        onSelectChange={(selectedOption, event) =>
          handleSelectChange(selectedOption, event, label)
        }
        error={errors[path]}
      />
    );
  };

  const renderFormGroup = ({ path, label, type, placeHolder, options }) => {
    if (type === "select") return renderSelect({ path, label, options });
    if (type === "radio") return renderRadio({ path, label });
    return renderInput({ path, label, type, placeHolder });
  };

  return (
    <div>
      <h1>{formTitle}</h1>
      <form onSubmit={doSubmit} className="col-md-6">
        {inputs.map((input) => renderFormGroup(input))}
        {renderSubmitButton(submitLabel || formTitle)}
      </form>
    </div>
  );
};

export default Form;
