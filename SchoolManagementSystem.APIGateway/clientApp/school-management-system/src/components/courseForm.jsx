import Form from "./common/form";
import Joi from "joi-browser";
import { useNavigate, useParams } from "react-router-dom";
import { useContext } from "react";
import CourseService from "../services/courseService";
import AppContext from "./context/appContext";

const CourseForm = () => {
  const navigate = useNavigate();
  const { specializes } = useContext(AppContext);
  const course = {
    name: "",
    specializeId: 0
  };
  const { id } = useParams();

  const inputs = [
    {
      path: "name",
      label: "Name",
      type: "text",
    },
    {
      path: "specializeId",
      label: "Specialize",
      type: "select",
      options: specializes.get(),
    },
  ];

  const schema = {
    id: Joi.optional(),
    name: Joi.string().trim().required().max(50).label("Name"),
    specializeId: Joi.number().integer().required().min(1).label("Specialize"),
  };

  const schemaMessages = {
    specializeId: {
      "number.min": '"Specialize" is not allowed to be empty',
    },
  };

  const getCourse = () => {
    if (!id) return Promise.resolve(course);
  
    return new Promise((resolve, reject) => {
      CourseService.getByIdAsync(id)
        .then(({ data: courseToEdit }) => resolve(courseToEdit))
        .catch((exception) => {
          if (exception.response && exception.response.status === 404) {
            reject("/not-found");
          } else {
            reject(exception);
          }
        });
    });
  };

  const setFormDataOnInitialRender = (setData) =>
  getCourse()
    .then((currentCourse) => setData(currentCourse))
    .catch((error) => {
      if (error === "/not-found")
        navigate("/not-found", { replace: true });
      setData({ name: "", specializeId: 0 });
    });

  const handleSubmit = async (course) => {
    const courseToSave = { ...course };
    const errors = {};
    try {
      if (!id) await CourseService.addCourseAsync(courseToSave);
      else await CourseService.updateCourseAsync(id, courseToSave);
      navigate("/courses", { replace: false });
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
      formTitle={id ? "Update course" : "Add course"}
      initialData={course}
      schema={schema}
      schemaMessages={schemaMessages}
      handleSubmit={handleSubmit}
      submitLabel={id ? "Update" : "Create"}
      setDataOnInitialRender={setFormDataOnInitialRender}
    />
  );
};

export default CourseForm;
