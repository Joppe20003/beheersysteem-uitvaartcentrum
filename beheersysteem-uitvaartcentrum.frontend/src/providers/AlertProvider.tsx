import { useState, ReactNode } from "react";
import { AlertContext, Alert, AlertType } from "../contexts/AlertContext";

export function AlertProvider({ children }: { children: ReactNode }) {
    const [alert, setAlert] = useState<Alert | null>(null);

    const showAlert = (message: string, type: AlertType) => {
        setAlert({ message, type });
    };

    const clearAlert = () => setAlert(null);

    return (
        <AlertContext.Provider value= {{ alert, showAlert, clearAlert }}>
            { children }
        </AlertContext.Provider>
    );
}