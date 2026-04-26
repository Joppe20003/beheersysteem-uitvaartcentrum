import { Outlet } from "react-router-dom";
import Alert from "../components/structures/Alert";

function AuthLayout() {
    return (
        <main className="container-fluid bg-light min-vh-100 d-flex justify-content-center align-items-center">
            <Alert />
            <Outlet />
        </main>
    );
}

export default AuthLayout;