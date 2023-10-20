import { Link, useLocation } from "react-router-dom";

const NavLink = ({ path, label, onClick, style }) => {
  const pathname = useLocation().pathname;
  return (
    <Link
      to={path}
      className={`nav-link ${pathname === path && "active"}`}
      style={{ color: "#fff", ...style }}
      onClick={onClick ? onClick : undefined}
    >
      {label}
    </Link>
  );
};

export default NavLink;
