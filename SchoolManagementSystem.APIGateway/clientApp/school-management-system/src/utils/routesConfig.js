import { rolesEnum } from "./enums";
import Enroll from "../components/enroll";
import Courses from "../components/courses";
import Students from "../components/students";
import Attendance from "../components/attendance";
import RegisterCourses from "../components/registerCourses";
import StudentCourses from "../components/studentCourses";
import About from "../components/about";
import CourseForm from "../components/courseForm";
import Home from "../components/home";
import Register from "../components/register";
import Login from "../components/login";
import Logout from "../components/logout";
import Forbidden from "../components/forbidden";
import NotFound from "../components/notFound";

const routesConfig = (context) => [
  {
    component: <Courses />,
    path: "/courses",
    requireAuthorize: [rolesEnum.admin],
    label: "Courses",
    isInStartOfNavLinks: true,
  },
  {
    component: <Students />,
    path: "/students",
    requireAuthorize: [rolesEnum.admin],
    label: "Students",
    isInStartOfNavLinks: true,
  },
  {
    component: <Enroll />,
    path: "/enroll",
    requireAuthorize: [rolesEnum.noRole],
    label: "Enroll",
    isInStartOfNavLinks: true,
  },
  {
    component: <Attendance />,
    path: "/attendance",
    requireAuthorize: [rolesEnum.admin],
    label: "Attendance",
    isInStartOfNavLinks: true,
  },
  {
    component: <RegisterCourses />,
    path: "/courses/register",
    requireAuthorize: [rolesEnum.student],
    label: "Register courses",
    isVisible: !context.isCoursesRegistered.get() && context.isAccepted.get(),
    isInStartOfNavLinks: true,
  },
  {
    component: <StudentCourses />,
    path: "/courses/mine",
    label: "Courses",
    requireAuthorize: [rolesEnum.student],
    isVisible: context.isCoursesRegistered.get() && context.isAccepted.get(),
    isInStartOfNavLinks: true,
  },
  {
    component: <About />,
    path: "/about",
    label: "About",
    isInStartOfNavLinks: true,
  },
  {
    component: <CourseForm />,
    path: "/courses/new",
    requireAuthorize: [rolesEnum.admin],
  },
  {
    component: <CourseForm />,
    path: "/courses/:id",
    requireAuthorize: [rolesEnum.admin],
  },
  {
    component: <StudentCourses />,
    path: "/students/courses/:studentId",
    requireAuthorize: [rolesEnum.admin],
  },
  {
    component: <Home />,
    path: "/",
    label: context.user.get()?.email,
    handleClick: (e) => e.preventDefault(),
    isInEndOfNavLinks: true,
  },
  {
    component: <Register />,
    path: "/register",
    anonymousOnly: true,
    label: "Register",
    isInEndOfNavLinks: true,
  },
  {
    component: <Login />,
    path: "/login",
    anonymousOnly: true,
    label: "Login",
    isInEndOfNavLinks: true,
  },
  {
    component: <Logout />,
    path: "/logout",
    requireAuthorize: true,
    label: "Logout",
    isInEndOfNavLinks: true,
  },
  {
    component: <Forbidden />,
    path: "/forbidden",
  },

  {
    component: <NotFound />,
    path: "*",
  },
];

export default routesConfig;
