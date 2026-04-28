import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../services/authService';

interface UserStatus {
    isAuthenticated: boolean;
    username: string;
    actions: string[];
}

interface ProtectedProps {
    children: React.ReactNode;
}

const ProtectedComponent: React.FC<ProtectedProps> = ({ children }) => {
    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState<UserStatus | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const verifyAuth = async () => {
            try {
                const data = await authService.status();

                setUser(data);
            } catch (error: unknown) {
                const status = error.status || error.statusCode;

                if (status === 403) {
                    navigate(-1);
                } else {
                    navigate("/login");
                }
            } finally {
                setLoading(false);
            }
        };

        verifyAuth();
    }, [navigate]);

    if (loading) {
        return <div className="spinner">Controleren van toegang...</div>;
    }

    return user && user.isAuthenticated ? <>{children}</> : null;
};

export default ProtectedComponent;