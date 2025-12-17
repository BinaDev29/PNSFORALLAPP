import React, { createContext, useContext, useState, useEffect } from "react";

interface AuthContextType {
    isAuthenticated: boolean;
    login: (token: string) => void;
    logout: () => void;
    checkAuth: () => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem("pns_auth_token");
        setIsAuthenticated(!!token);
        setIsLoading(false);
    }, []);

    const login = (token: string) => {
        localStorage.setItem("pns_auth_token", token);
        setIsAuthenticated(true);
    };

    const logout = () => {
        localStorage.removeItem("pns_auth_token");
        setIsAuthenticated(false);
    };

    const checkAuth = () => {
        const token = localStorage.getItem("pns_auth_token");
        return !!token;
    }

    if (isLoading) {
        return null; // Or a loading spinner
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, login, logout, checkAuth }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
}
