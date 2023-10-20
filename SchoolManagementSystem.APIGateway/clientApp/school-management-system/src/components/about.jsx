import "../assets/styles/about.css";

const About = () => {
  return (
    <>
      <h4>About us</h4>
      <div
        style={{
          width: "100%",
          height: "70vh",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          color: "#fff",
        }}
      >
        <div
          style={{
            backgroundColor: "rgb(72 27 111)",
            width: "50vw",
            height: "50vh",
            display: "flex",
            flexDirection: "row",
            borderRadius: "15px",
            padding: "15px",
            justifyContent: "space-around",
          }}
        >
          <p>
            <span>Mohammad Hammad</span>
            <hr />
            <span>Software Engineer</span>
            <hr />
            <span>web developer</span>
            <hr />
            <span>.Net developer</span>
            <hr />
            <span>React developer</span>
          </p>
          <p>
            <span>Ameer Abo Hammoud</span>
            <hr />
            <span>Competitive programmer</span>
            <hr />
            <span>Mobile developer</span>
            <hr />
            <span>.Net developer</span>
            <hr />
            <span>Flutter developer</span>
          </p>
        </div>
      </div>
    </>
  );
};

export default About;
