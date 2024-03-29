import React, { useEffect, useState } from "react";
import { ToastContainer, toast } from "react-toastify";
import { useStateWithCallbackLazy } from "use-state-with-callback";
import SchoolyRoutes from "./components/schoolyRoutes";
import NavBar from "./components/navbar";
import { AppContextProvider } from "./components/context/appContext";
import { getUser } from "./utils/user";
import "./App.css";
import "react-toastify/dist/ReactToastify.css";
import AdmissionService from "./services/admissionService";
import CourseService from "./services/courseService";
import NavLink from "./components/common/navLink";
import { rolesEnum } from "./utils/enums";
import { useEffectOnInitialRender } from "./hooks/useEffect";

function App() {
  const [user, setUser] = useStateWithCallbackLazy(getUser());
  const [specializes, setSpecializes] = useState([]);
  const [isCoursesRegistered, setIsCoursesRegistered] = useState(false);
  const [isAccepted, setIsAccepted] = useState(false);
  const [shouldRenderRoutes, setShouldRenderRoutes] = useState(false);

  useEffectOnInitialRender(() => {
    AdmissionService.getSpecializesAsync()
      .then(({ data }) => setSpecializes(data))
      .catch(() => setSpecializes([]));

    const errorMessage = localStorage.getItem("loadMessage");
    if (!errorMessage) return;

    toast.error(errorMessage);
    localStorage.removeItem("loadMessage");
  });

  useEffect(() => {
    let isAcceptedResult = false,
      isCoursesRegisteredResult = false,
      shouldRenderRoutesResult = true;
    const setStates = () => {
      setIsAccepted(isAcceptedResult);
      setIsCoursesRegistered(isCoursesRegisteredResult);
      setShouldRenderRoutes(shouldRenderRoutesResult);
    };

    if (!user || !user.roles.includes(rolesEnum.student)) return setStates();

    AdmissionService.isAcceptedAsync()
      .then(({ data: isAccepted }) => {
        if (!isAccepted) return setStates();

        isAcceptedResult = true;
        CourseService.isCoursesRegisteredAsync()
          .then(({ data: isRegistered }) => (isCoursesRegisteredResult = true))
          .catch(() => {})
          .finally(() => setStates());
      })
      .catch(() => setStates());
  }, [user]);

  return (
    <AppContextProvider
      value={{
        user: { get: () => user, set: (u, callback) => setUser(u, callback) },
        specializes: { get: () => specializes },
        isCoursesRegistered: {
          get: () => isCoursesRegistered,
          set: (isReg) => setIsCoursesRegistered(isReg),
        },
        isAccepted: { get: () => isAccepted },
      }}
    >
      <header
        style={{ position: "fixed", top: 0, right: 0, left: 0, zIndex: 10 }}
      >
        <NavBar />
      </header>
      <main
        role="main"
        className="container"
        style={{ marginBottom: "75px", marginTop: "60px" }}
      >
        <ToastContainer />
        {shouldRenderRoutes && <SchoolyRoutes />}
      </main>

      <footer
        style={{
          background: "#481b6f",
          height: "70px",
          position: "fixed",
          left: 0,
          right: 0,
          bottom: 0,
          zIndex: 10,
        }}
      >
        <div
          className="wrapper container"
          style={{
            paddingTop: "10px",
            textAlign: "left",
          }}
        >
          <p
            style={{
              margin: "5px 0",
              color: "#fff",
              float: "left",
              fontSize: "13px",
              marginLeft: "10px",
              letterSpacing: "0.002em",
              lineHeight: 1.528571429,
            }}
          >
            © 2023 Schooly High School&nbsp;&nbsp;·&nbsp;&nbsp; Legal
            Information&nbsp;&nbsp;·&nbsp;&nbsp;Syria, Damascus, Mazzeh
            <br />
            T: 0934 406 451&nbsp;&nbsp;·&nbsp;&nbsp;E:{" "}
            <a href="mailto:admin@schoolmanagementsystem.com">
              admin@schoolmanagementsystem.com
            </a>
          </p>

          <p
            id="credit"
            style={{
              color: "#fff",
              fontSize: "13px",
              marginLeft: "10px",
              letterSpacing: "0.002em",
              margin: "10px 4px 0 0",
              padding: 0,
              float: "right",
              lineHeight: 1.528571429,
            }}
          >
            School Website Powered <br />
            by{" "}
            <NavLink
              path="/"
              label="Mohammad Hammad"
              style={{ display: "inline" }}
            />
          </p>
        </div>
      </footer>
    </AppContextProvider>
  );
}

export default App;
