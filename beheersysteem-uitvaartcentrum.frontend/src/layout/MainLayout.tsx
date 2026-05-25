import { Outlet } from "react-router-dom";
import Alert from "../components/structures/Alert";
import Header from "../components/shared/Header";

function MainLayout() {
    return (
        <div className="bg-light">
            <main className="container bg-white border-start border-end shadow position-relative min-vh-100">
                <header>
                    <Header />
                </header>
                <div>
                    <Alert />
                    <Outlet />
                </div>
            </main>
        </div>
    );
}

export default MainLayout;