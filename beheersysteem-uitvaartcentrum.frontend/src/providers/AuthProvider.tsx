import { useState, ReactNode, useEffect, useCallback } from "react";
import { AuthContext, UserStatus } from "../contexts/AuthContext";
import { authService } from "../services/authService";
import { useNavigate } from "react-router-dom";

export function AuthProvider({ children }: { children: ReactNode }) {
    const navigate = useNavigate();
    const [user, setUser] = useState<UserStatus | null>(null);
    const [loading, setLoading] = useState(true);

    const refreshStatus = useCallback(async () => {
        setLoading(true);
        try {
            const data = await authService.status();

            setUser(data);
        } catch (error) {
            console.error(error);

            setUser(null);
        } finally {
            setLoading(false);
        }
    }, []);



    useEffect(() => {
        refreshStatus();
    }, [refreshStatus]);

    const hasPermission = useCallback((action: string) => {
        return user?.actions.includes(action) ?? false;
    }, [user]);


    const logOut = useCallback(async () => {
        await authService.logout();

        setUser(null);
        navigate("/login");
    }, []);

    return (
        <AuthContext.Provider value={{ 
            user, 
            setUser, 
            hasPermission, 
            loading, 
            setLoading, 
            refreshStatus, 
            logOut
        }}>
            {children}
        </AuthContext.Provider>
    );
}