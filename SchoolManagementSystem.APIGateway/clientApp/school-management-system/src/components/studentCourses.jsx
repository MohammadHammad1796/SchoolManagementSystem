import { useEffect, useState } from "react";
import CourseService from "../services/courseService";
import { useParams } from "react-router-dom";

const StudentCourses = () => {
  const { studentId } = useParams();
  const [courses, setCourses] = useState([]);

  const getCourses = async () => {
    let data;
    if (studentId)
      ({ data } = await CourseService.getStudentCoursesAsync(studentId));
    else ({ data } = await CourseService.getMineAsync());
    return data;
  };

  const handleInitialRender = async () => {
    try {
      const courses = await getCourses();
      setCourses(courses);
    } catch (_) {}
  };

  useEffect(() => {
    handleInitialRender();
  }, []);

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
