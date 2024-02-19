import { useContext } from "react";
import AccountsService from "../services/accountService";
import { setJwt } from "../utils/user";
import AppContext from "./context/appContext";
import { useNavigate } from "react-router-dom";
import { useEffectOnInitialRender } from "../hooks/useEffect";

const Logout = () => {
  const { user } = useContext(AppContext);
  const navigate = useNavigate();

  useEffectOnInitialRender(() => {
    AccountsService.logoutAsync()
      .catch(() => {})
      .finally(() => {
        setJwt();
        user.set(null, () => {
          navigate("/");
        });
      });
  });

  return <></>;
};

export default Logout;
