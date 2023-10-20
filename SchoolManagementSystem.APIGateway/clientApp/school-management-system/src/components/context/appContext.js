import React from "react";

const AppContext = React.createContext();
AppContext.displayName = "appContext";

export const AppContextProvider = AppContext.Provider;

export default AppContext;
