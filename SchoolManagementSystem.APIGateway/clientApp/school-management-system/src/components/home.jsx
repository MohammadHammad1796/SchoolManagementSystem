import background from "../assets/home-background.jpg";
import { faArrowRightToBracket } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "../assets/styles/home.css";
import { Link } from "react-router-dom";

const Home = () => {
  return (
    <>
      <img src={background} style={{ width: "100%" }} alt="Background" />
      <ul className="row home-links" style={{ margin: 0, padding: 0 }}>
        <li className="col-md-6">
          <Link to="/register" style={{ background: "#632862" }}>
            <span className="title">Not a student</span>
            <span className="sub-text">Register now</span>
            <FontAwesomeIcon icon={faArrowRightToBracket} />
          </Link>
        </li>
        <li className="col-md-6">
          <Link
            to="/about"
            style={{
              background: "#77b6ea",
            }}
          >
            <span className="title">About us</span>
            <span className="sub-text">Schooly high school</span>
            <FontAwesomeIcon icon={faArrowRightToBracket} />
          </Link>
        </li>
      </ul>
    </>
  );
};

export default Home;
