import { createContext, useContext, useState, ReactNode } from "react";

type AlertType = "success" | "danger" | "warning" | "info";

interface Alert {
    message: string;
    type: AlertType;
}

interface AlertContextType {
    alert: Alert | null;
    showAlert: (message: string, type: AlertType) => void;
    clearAlert: () => void;
}

const AlertContext = createContext<AlertContextType | null>(null);

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

export function useAlert() {
    const context = useContext(AlertContext);
    if (!context) throw new Error("useAlert must be used within AlertProvider");
    return context;
}