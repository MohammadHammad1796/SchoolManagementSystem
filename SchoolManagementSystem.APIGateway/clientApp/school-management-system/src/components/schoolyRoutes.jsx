import { Routes, Route, Navigate } from "react-router-dom";
import SchoolyNavigate from "./common/schoolyNavigate";
import { useContext } from "react";
import AppContext from "./context/appContext";
import routesConfig from "../utils/routesConfig";

const SchoolyRoutes = () => {
  const context = useContext(AppContext);
  const user = context.user.get();
  const routes = routesConfig(context, user);

  return (
    <Routes>
      {routes.map(
        ({ path, component, requireAuthorize, anonymousOnly, isVisible }) => {
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
            (isVisible === true || isVisible === undefined) && (
              <Route path={path} element={element} key={path} />
            )
          );
        }
      )}
    </Routes>
  );
};

export default SchoolyRoutes;
