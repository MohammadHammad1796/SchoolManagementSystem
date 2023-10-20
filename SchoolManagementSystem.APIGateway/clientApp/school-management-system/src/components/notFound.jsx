import background from "../assets/404.png";

const NotFound = () => {
  return (
    <div
      style={{
        width: "100%",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <img src={background} style={{ width: "400px" }} alt="Page not found" />
    </div>
  );
};

export default NotFound;
