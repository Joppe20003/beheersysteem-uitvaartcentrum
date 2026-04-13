import "bootstrap/dist/css/bootstrap.min.css";

import { Outlet } from "react-router-dom";
import Footer from "../components/structures/Footer";
import Header from "../components/structures/Header";

function MainLayout() {
    return (
        <div className="d-flex flex-column min-vh-100">
            <Header />
            <main className="flex-grow-1">
                <Outlet />
            </main>
            <Footer />
        </div>
    );
}

export default MainLayout;