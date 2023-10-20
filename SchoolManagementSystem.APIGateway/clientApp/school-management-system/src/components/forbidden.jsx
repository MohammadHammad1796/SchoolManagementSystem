import background from "../assets/403.png";

const Forbidden = () => {
  return (
    <div
      style={{
        width: "100%",
        height: "70vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <img
        src={background}
        alt="You don't have privilege to access this page"
      />
    </div>
  );
};

export default Forbidden;
