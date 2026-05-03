import { createContext } from "react";

export interface UserStatus {
    isAuthenticated: boolean;
    username: string;
    actions: string[];
    role: string;
    id: string;
}

export interface AuthContextType {
    user: UserStatus | null;
    setUser: (user: UserStatus | null) => void;
    hasPermission: (action: string) => boolean;
    logOut: () => Promise<void>;
    loading: boolean;
    setLoading: (loading: boolean) => void;
    refreshStatus: () => Promise<void>;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);