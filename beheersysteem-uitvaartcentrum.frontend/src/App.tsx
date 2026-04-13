import { Route, Routes } from "react-router-dom";

import MainLayout  from "./layout/MainLayout.tsx";
import NotFound from "./pages/others/NotFound.tsx";
import Overview from "./pages/dossiers/Overview.tsx";
import View from "./pages/dossiers/View.tsx";
import Create from "./pages/dossiers/Create.tsx";

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
