import { RouteObject, createBrowserRouter } from "react-router-dom";
import Login from "../../features/registration/Login";
import App from "../layout/App";
import Dashboard from "../../features/dashboard/Dashboard";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {path: '/login', element: <Login />},
            {path: '/dashboard', element: <Dashboard />},
        ]
    }
]
export const router = createBrowserRouter(routes)