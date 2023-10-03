import { Container } from "semantic-ui-react";
import Login from "../../features/registration/Login";
import "./App.css";
import { Outlet, useLocation } from "react-router-dom";

function App() {
  const location = useLocation();
  return (
    <>
      {location.pathname === "/" ? (
        <Login />
      ) : (
        <>
          <Container style={{ marginTop: "7em" }}>
            <Outlet />
          </Container>
        </>
      )}
    </>
  );
}

export default App;
