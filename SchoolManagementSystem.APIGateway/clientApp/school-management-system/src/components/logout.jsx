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
    const logout = async () => {
      try {
        await AccountsService.logoutAsync();
      } catch (_) {}

      setJwt();
      user.set(null, () => {
        navigate("/");
      });
    };
    logout();
  });

  return <></>;
};

export default Logout;
