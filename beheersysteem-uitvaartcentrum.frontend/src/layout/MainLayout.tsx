import { Outlet } from "react-router-dom";
import Alert from "../components/structures/Alert";

function MainLayout() {
    return (
        <div className="bg-light">
            <main className="container bg-white shadow position-relative min-vh-100">
                <Alert />
                <Outlet />
            </main>
        </div>
    );
}

export default MainLayout;