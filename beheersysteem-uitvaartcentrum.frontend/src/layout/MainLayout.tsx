import { Outlet } from "react-router-dom";
import { AlertProvider } from "../providers/AlertProvider"
import Alert from "../components/structures/Alert";

function MainLayout() {
    return (
        <AlertProvider>
            <div className="bg-light">
                <main className="container bg-white shadow position-relative min-vh-100">
                    <Alert />
                    <Outlet />
                </main>
            </div>
        </AlertProvider>
    );
}

export default MainLayout;