import Form from "./common/form";
import Joi from "joi-browser";
import { getUser, setJwt } from "../utils/user";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import AppContext from "./context/appContext";
import AdmissionService from "../services/admissionService";
import AccountService from "../services/accountService";
import {
  getDateBeforeYears,
  getDateTimeInUTC,
  getFirstDateOfYear,
  getLastDateOfYear,
} from "../utils/helpers";

const Enroll = () => {
  const { user, specializes } = useContext(AppContext);
  const navigate = useNavigate();

  const inputs = [
    {
      path: "firstName",
      label: "First name",
      type: "text",
      placeHolder: "Enter your first name",
    },
    {
      path: "lastName",
      label: "Last name",
      type: "text",
      placeHolder: "Enter your last name",
    },
    {
      path: "fatherFirstName",
      label: "Father first name",
      type: "text",
      placeHolder: "Enter your father first name",
    },
    {
      path: "motherFullName",
      label: "Mother full name",
      type: "text",
      placeHolder: "Enter your mother full name",
    },
    {
      path: "isMale",
      label: "Gender",
      type: "radio",
    },
    {
      path: "dateOfBirth",
      label: "Date of birth",
      type: "date",
    },
    {
      path: "specializeId",
      label: "Specialize",
      type: "select",
      options: specializes.get(),
    },
    {
      path: "certificateDate",
      label: "Certificate date",
      type: "date",
    },
    {
      path: "certificateAverage",
      label: "Certificate average",
      type: "number",
    },
  ];

  const data = {
    firstName: "",
    lastName: "",
    fatherFirstName: "",
    motherFullName: "",
    isMale: true,
    dateOfBirth: getLastDateOfYear(getDateBeforeYears(17)),
    specializeId: 0,
    certificateDate: getLastDateOfYear(getDateBeforeYears(1)),
    certificateAverage: 0,
  };

  const schema = {
    firstName: Joi.string().trim().required().max(50).label("First name"),
    lastName: Joi.string().trim().required().max(50).label("Last name"),
    fatherFirstName: Joi.string()
      .trim()
      .required()
      .max(50)
      .label("Father first name"),
    motherFullName: Joi.string()
      .trim()
      .required()
      .max(100)
      .label("Mother full name"),
    isMale: Joi.boolean().required().label("Gender"),
    dateOfBirth: Joi.date()
      .required()
      .max(getFirstDateOfYear(getDateBeforeYears(16)))
      .min(getFirstDateOfYear(getDateBeforeYears(20)))
      .label("Date of birth"),
    specializeId: Joi.number().integer().required().min(1).label("Specialize"),
    certificateDate: Joi.date()
      .required()
      .max(getFirstDateOfYear(getDateBeforeYears(0)))
      .min(getFirstDateOfYear(getDateBeforeYears(4)))
      .label("Certificate date"),
    certificateAverage: Joi.number()
      .required()
      .min(150)
      .max(310)
      .label("Certificate average"),
  };

  const schemaMessages = {
    specializeId: {
      "number.min": '"Specialize" is not allowed to be empty',
    },
  };

  const handleSubmit = async (data) => {
    const resource = JSON.parse(JSON.stringify(data));
    resource.dateOfBirth = getDateTimeInUTC(resource.dateOfBirth);
    resource.certificateDate = getDateTimeInUTC(resource.certificateDate);
    const errors = {};
    try {
      await AdmissionService.enrollAsync(resource);
      const { data: jwt } = await AccountService.refreshAccessTokenAsync();
      setJwt(jwt);
      user.set(getUser());
      navigate("/");
      return;
    } catch (exception) {
      if (exception.response) {
        const { data, status } = exception.response;
        if (status === 400) {
          for (const [key, value] of Object.entries(data)) {
            errors[key] = value.join(", ");
          }
        }
      }
    }
    return errors;
  };

  return (
    <Form
      inputs={inputs}
      formTitle={"Enroll"}
      initialData={data}
      schema={schema}
      schemaMessages={schemaMessages}
      handleSubmit={handleSubmit}
    />
  );
};

export default Enroll;
