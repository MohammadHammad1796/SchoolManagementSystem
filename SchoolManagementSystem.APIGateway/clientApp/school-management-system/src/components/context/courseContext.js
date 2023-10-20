import React from "react";

const CourseContext = React.createContext();
CourseContext.displayName = "courseContext";

export const CourseContextProvider = CourseContext.Provider;

export default CourseContext;
