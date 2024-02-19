import { useContext, useState } from "react";
import CourseService from "../services/courseService";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import AppContext from "./context/appContext";
import { useEffectOnInitialRender } from "../hooks/useEffect";

const RegisterCourses = () => {
  const navigate = useNavigate();
  const [courses, setCourses] = useState([]);
  const [selectedCourses, setSelectedCourses] = useState([]);
  const [error, setError] = useState("");
  const { isCoursesRegistered } = useContext(AppContext);

  useEffectOnInitialRender(() => {
    CourseService.getForMySPecializeAsync()
      .then(({ data: courses }) => setCourses(courses))
      .catch(() => setCourses([]));
  });

  const handleSubmit = async () => {
    let errors = "";
    try {
      await CourseService.registerAsync(selectedCourses);
      toast.success("Courses registered successfully.");
      isCoursesRegistered.set(true);
      navigate("/", { replace: false });
      return;
    } catch (exception) {
      if (exception.response) {
        const { data, status } = exception.response;
        if (status === 400) {
          for (const [, value] of Object.entries(data)) {
            errors += value.join(", ");
          }
          setError(errors);
        }
      }
    }
  };

  const handleCheck = (id) => {
    let selected = [...selectedCourses];
    const courseIndex = selectedCourses.findIndex(
      (courseId) => courseId === id
    );
    if (courseIndex === -1) selected = [id, ...selected];
    else selected.splice(courseIndex, 1);

    setSelectedCourses(selected);
  };

  return (
    <>
      <h4 style={{ marginTop: "10px", marginBottom: "10px" }}>
        Courses registration
      </h4>
      <div
        style={{
          border: "1px solid black",
          borderRadius: "5px",
          padding: "10px",
        }}
        className="row"
      >
        {courses.map((course, i) => {
          return (
            <div className="form-check col-md-3" key={course.id}>
              <input
                className="form-check-input"
                type="checkbox"
                value={course.id}
                id={course.name + i}
                onChange={() => handleCheck(course.id)}
                checked={selectedCourses.includes(course.id)}
              />
              <label className="form-check-label" htmlFor={course.name + i}>
                {course.name}
              </label>
            </div>
          );
        })}
      </div>
      {error && <div className="alert alert-danger">{error}</div>}
      <button
        className="btn btn-primary"
        onClick={handleSubmit}
        style={{ marginTop: "5px", marginBottom: "5px" }}
        disabled={selectedCourses.length === 0}
      >
        Register
      </button>
    </>
  );
};

export default RegisterCourses;
