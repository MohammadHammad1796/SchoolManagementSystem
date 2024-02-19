import { useState } from "react";
import CourseService from "../services/courseService";
import { useParams } from "react-router-dom";
import { useEffectOnInitialRender } from "../hooks/useEffect";

const StudentCourses = () => {
  const { studentId } = useParams();
  const [courses, setCourses] = useState([]);

  const getCourses = () => {
    return new Promise((resolve, reject) => {
      if (studentId)
        CourseService.getStudentCoursesAsync(studentId)
          .then(({ data }) => resolve(data))
          .catch(reject);
      else
        CourseService.getMineAsync()
          .then(({ data }) => resolve(data))
          .catch(reject);
    });
  };

  useEffectOnInitialRender(() => {
    getCourses()
      .then((courses) => setCourses(courses))
      .catch(() => setCourses([]));
  });

  return (
    <div className="col-md-4">
      <h4 style={{ marginTop: "10px", marginBottom: "10px" }}>
        {studentId ? "Student courses" : "My courses"}
      </h4>
      <ul
        style={{
          padding: "10px",
        }}
        className="row"
      >
        {courses.map((course, i) => {
          return (
            <li className="form-check" key={course.id}>
              <label className="form-check-label">{course.name}</label>
              <hr />
            </li>
          );
        })}
      </ul>
    </div>
  );
};
export default StudentCourses;
