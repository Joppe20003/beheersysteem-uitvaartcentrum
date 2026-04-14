import { Outlet } from "react-router-dom";
import { AlertProvider } from "../context/AlertContext";
import Alert from "../components/structures/Alert";

function MainLayout() {
    return (
        <AlertProvider>
            <div className="bg-light min-vh-100">
                <div className="container bg-white shadow position-relative min-vh-100">
                    <div className="row">
                        <main>
                            <Alert />
                            <Outlet />
                        </main>
                    </div>
                </div>
            </div>
        </AlertProvider>
    );
}

export default MainLayout;