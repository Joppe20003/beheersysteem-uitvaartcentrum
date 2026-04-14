import "bootstrap/dist/css/bootstrap.min.css";

import { Route, Routes } from "react-router-dom";

import MainLayout  from "./layout/MainLayout";

import NotFound from "./pages/others/NotFound";
import Overview from "./pages/dossiers/Overview";
import View from "./pages/dossiers/View";
import Create from "./pages/dossiers/Create";

function App() {
    return (
        <Routes>
            <Route element={<MainLayout />} >
                <Route path="/" element={<Overview />} />
                <Route path="/dossiers" element={<Overview />} />
                <Route path="/dossiers/:id/view" element={<View />} />
                <Route path="/dossiers/aanmaken" element={<Create />} />
            </Route>
            <Route>
                <Route path="/*" element={<NotFound />} />
            </Route>
        </Routes>
    );
}

export default App
