import React from "react";

const StudentContext = React.createContext();
StudentContext.displayName = "studentContext";

export const StudentContextProvider = StudentContext.Provider;

export default StudentContext;
