import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";

import App from "./App";

import { AlertProvider } from "./providers/AlertProvider";
import { AuthProvider } from "./providers/AuthProvider";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
        <BrowserRouter>
            <AlertProvider>
                <AuthProvider>
                    <App />
                </AuthProvider>
            </AlertProvider>
        </BrowserRouter>
    </React.StrictMode>
);