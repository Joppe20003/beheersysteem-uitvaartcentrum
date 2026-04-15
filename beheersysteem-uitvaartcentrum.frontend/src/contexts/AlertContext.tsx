import { createContext } from "react";

export type AlertType = "success" | "danger" | "warning" | "info";

export interface Alert {
    message: string;
    type: AlertType;
}

export interface AlertContextType {
    alert: Alert | null;
    showAlert: (message: string, type: AlertType) => void;
    clearAlert: () => void;
}

export const AlertContext = createContext<AlertContextType | null>(null);