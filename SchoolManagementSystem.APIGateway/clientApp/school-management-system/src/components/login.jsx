import Form from "./common/form";
import Joi from "joi-browser";
import AccountsService from "../services/accountService";
import { getUser, setJwt } from "../utils/user";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import AppContext from "./context/appContext";

const Login = () => {
  const { user } = useContext(AppContext);
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
      placeHolder: "Enter password",
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
      const { data: jwt } = await AccountsService.loginAsync(data);
      setJwt(jwt);

      user.set(getUser(), () => {
        navigate(-1);
      });
      return;
    } catch (exception) {
      if (exception.response) {
        const { data, status } = exception.response;
        if (status === 400) {
          for (const [key, value] of Object.entries(data)) {
            errors[key] = value.join(", ");
          }
        } else if (status === 401) {
          let message = data.message;
          if (data.lockoutEnd) {
            const date = new Date(data.lockoutEnd);
            message = data.message.replace("{0}", date.toLocaleString());
          }
          errors.password = message || "Email or password incorrect";
        }
      }
    }
    return errors;
  };

  return (
    <Form
      inputs={inputs}
      formTitle={"Login"}
      initialData={data}
      schema={schema}
      handleSubmit={handleSubmit}
    />
  );
};

export default Login;
