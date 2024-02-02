import Form from "./common/form";
import Joi from "joi-browser";
import AccountService from "../services/accountService";
import { useNavigate } from "react-router-dom";

const Register = () => {
  const navigate = useNavigate();

  const inputs = [
    {
      path: "email",
      label: "Email",
      type: "text",
      placeHolder: "Enter your email",
    },
    {
      path: "password",
      label: "Password",
      type: "password",
      placeHolder: "Enter password of 5 characters at least",
    },
  ];

  const data = {
    email: "",
    password: "",
  };

  const schema = {
    email: Joi.string().trim().required().email().label("Email"),
    password: Joi.string().trim().required().min(5).label("Password"),
  };

  const handleSubmit = async (data) => {
    const errors = {};
    try {
      await AccountService.registerAsync(data);
      navigate("/login");
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
      formTitle={"Register"}
      initialData={data}
      schema={schema}
      handleSubmit={handleSubmit}
    />
  );
};

export default Register;
