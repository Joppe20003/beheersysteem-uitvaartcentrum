import { Route, Routes, Navigate } from "react-router-dom";

import MainLayout from "./layout/MainLayout";
import AuthLayout from "./layout/AuthLayout";

import NotFound from "./pages/others/NotFound";
import Overview from "./pages/dossiers/Overview";

import View from "./pages/dossiers/View";
import LoginView from "./pages/auth/LoginView";
import RegisterView from "./pages/auth/RegisterView";

import Create from "./pages/dossiers/Create";

function App() {
    return (
        <Routes>
            <Route element={<MainLayout />}>
                <Route path="/" element={<Navigate to="dossiers" replace />} />

                <Route path="dossiers">
                    <Route index element={<Overview />} />
                    <Route path="aanmaken" element={<Create />} />
                    <Route path="view/:id" element={<View />} />
                </Route>

                <Route path="*" element={<NotFound />} />
            </Route>
            <Route element={<AuthLayout />}>
                <Route path="/registreren" element={<RegisterView />} />
                <Route path="/login" element={<LoginView />} />
            </Route>
        </Routes>
    );
}

export default App;