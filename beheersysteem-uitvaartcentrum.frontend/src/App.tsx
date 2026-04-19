import { Route, Routes, Navigate } from "react-router-dom";

import MainLayout from "./layout/MainLayout";
import NotFound from "./pages/others/NotFound";
import Overview from "./pages/dossiers/Overview";
import View from "./pages/dossiers/View";
import Create from "./pages/dossiers/Create";

function App() {
    return (
        <Routes>
            <Route path="/" element={<MainLayout />}>
                <Route index element={<Navigate to="/dossiers" replace />} />

                <Route path="dossiers">
                    <Route index element={<Overview />} />
                    <Route path="aanmaken" element={<Create />} />
                    <Route path="view/:id" element={<View />} />
                </Route>

                <Route path="*" element={<NotFound />} />
            </Route>
        </Routes>
    );
}

export default App;