import { Link } from "react-router-dom";
import NavLink from "./common/navLink";
import { useContext } from "react";
import AppContext from "./context/appContext";
import { rolesEnum } from "../utils/enums";

const NavBar = () => {
  const context = useContext(AppContext);
  const user = context.user.get();

  const startRoutes = [
    { path: "/enroll", label: "Enroll", requireAuthorize: [rolesEnum.noRole] },
    {
      path: "/courses/register",
      label: "Register courses",
      requireAuthorize: [rolesEnum.student],
      isVisible: !context.isCoursesRegistered.get() && context.isAccepted.get(),
    },
    {
      path: "/courses/mine",
      label: "Courses",
      requireAuthorize: [rolesEnum.student],
      isVisible: context.isCoursesRegistered.get() && context.isAccepted.get(),
    },
    { path: "/courses", label: "Courses", requireAuthorize: [rolesEnum.admin] },
    { path: "/students", label: "Students", requireAuthorize: [rolesEnum.admin] },
    { path: "/attendance", label: "Attendance", requireAuthorize: [rolesEnum.admin] },
    { path: "/about", label: "About" },
  ];

  const endRoutes = [
    {
      path: "/",
      label: user?.email,
      requireAuthorize: true,
      handleClick: (e) => e.preventDefault(),
    },
    {
      path: "/logout",
      label: "Logout",
      requireAuthorize: true,
    },
    { path: "/register", label: "Register", anonymousOnly: true },
    { path: "/login", label: "Login", anonymousOnly: true },
  ];

  const renderLinks = (routes) => {
    return routes.map(
      ({
        path,
        label,
        requireAuthorize,
        anonymousOnly,
        handleClick,
        isVisible,
      }) => {
        let shouldDisplay = true;
        if (user) {
          if (
            anonymousOnly ||
            (Array.isArray(requireAuthorize) &&
              requireAuthorize.every((role) => !user?.roles?.includes(role)))
          )
            shouldDisplay = false;
        } else if (requireAuthorize) shouldDisplay = false;

        const linkProps = { path, label };
        if (handleClick) linkProps.onClick = handleClick;

        return (
          (isVisible === true || isVisible === undefined) &&
          shouldDisplay && (
            <li
              className="nav-item"
              style={{
                color: "#fff",
                fontSize: "15px",
                fontWeight: 700,
                fontFamily: "'Montserrat', sans-serif",
              }}
              key={path}
            >
              <NavLink {...linkProps} />
            </li>
          )
        );
      }
    );
  };

  return (
    <nav className="navbar navbar-expand-lg bg-light">
      <div className="container">
        <Link to="/" className="navbar-brand" style={{ color: "#fff" }}>
          Schooly
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav">{renderLinks(startRoutes)}</ul>
          <div className="navbar-nav" style={{ marginLeft: "auto" }}>
            <div className="nav-item text-nowrap" style={{ display: "flex" }}>
              {renderLinks(endRoutes)}
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default NavBar;
