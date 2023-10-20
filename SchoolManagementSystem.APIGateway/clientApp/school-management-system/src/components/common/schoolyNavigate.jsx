import { Navigate, useLocation } from "react-router-dom";

const SchoolyNavigate = ({ path }) => {
  const { pathname: protectedPath } = useLocation();
  return <Navigate to={path} state={{ from: protectedPath }} />;
};

export default SchoolyNavigate;
