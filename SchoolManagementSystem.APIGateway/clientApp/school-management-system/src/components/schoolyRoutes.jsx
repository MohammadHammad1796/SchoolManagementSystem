import { Routes, Route, Navigate } from "react-router-dom";
import SchoolyNavigate from "./common/schoolyNavigate";
import Logout from "./logout";
import { useContext } from "react";
import AppContext from "./context/appContext";
import Login from "./login";
import Register from "./register";
import Home from "./home";
import About from "./about";
import Enroll from "./enroll";
import Students from "./students";
import Courses from "./courses";
import CourseForm from "./courseForm";
import RegisterCourses from "./registerCourses";
import StudentCourses from "./studentCourses";
import Forbidden from "./forbidden";
import NotFound from "./notFound";
import Attendance from "./attendance";

const SchoolyRoutes = () => {
  const context = useContext(AppContext);
  const routes = [
    {
      component: <Enroll />,
      path: "/enroll",
      requireAuthorize: ["norole"],
    },
    {
      component: <Students />,
      path: "/students",
      requireAuthorize: ["admin"],
    },
    {
      component: <Courses />,
      path: "/courses",
      requireAuthorize: ["admin"],
    },
    {
      component: <CourseForm />,
      path: "/courses/new",
      requireAuthorize: ["admin"],
    },
    {
      component: <CourseForm />,
      path: "/courses/:id",
      requireAuthorize: ["admin"],
    },
    {
      component: <RegisterCourses />,
      path: "/courses/register",
      requireAuthorize: ["student"],
      isAllowed: !context.isCoursesRegistered.get() && context.isAccepted.get(),
    },
    {
      component: <StudentCourses />,
      path: "/courses/mine",
      requireAuthorize: ["student"],
      isAllowed: context.isCoursesRegistered.get() && context.isAccepted.get(),
    },
    {
      component: <StudentCourses />,
      path: "/students/courses/:studentId",
      requireAuthorize: ["admin"],
    },
    {
      component: <Attendance />,
      path: "/attendance",
      requireAuthorize: ["admin"],
    },
    { component: <About />, path: "/about" },
    { component: <Register />, path: "/register", anonymousOnly: true },
    {
      component: <Login />,
      path: "/login",
      anonymousOnly: true,
    },
    {
      component: <Logout />,
      path: "/logout",
      requireAuthorize: true,
    },
    { component: <Forbidden />, path: "/forbidden" },
    { component: <Home />, path: "/" },
    { component: <NotFound />, path: "*" },
  ];

  const user = context.user.get();

  return (
    <Routes>
      {routes.map(
        ({ path, component, requireAuthorize, anonymousOnly, isAllowed }) => {
          let element = component;
          if (user) {
            if (anonymousOnly) element = <Navigate to="/" />;
            else if (
              Array.isArray(requireAuthorize) &&
              requireAuthorize.every((role) => !user?.roles?.includes(role))
            )
              element = <Navigate to="/forbidden" />;
          } else if (requireAuthorize && path !== "/logout")
            element = <SchoolyNavigate path="/login" />;

          return (
            (isAllowed === true || isAllowed === undefined) && (
              <Route path={path} element={element} key={path} />
            )
          );
        }
      )}
    </Routes>
  );
};

export default SchoolyRoutes;
