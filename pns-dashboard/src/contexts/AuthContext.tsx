import React, { createContext, useContext, useState, useEffect } from "react";

interface User {
    id: string;
    userName: string;
    email: string;
    roles: string[];
}

interface AuthContextType {
    isAuthenticated: boolean;
    user: User | null;
    login: (token: string, userData: User) => void;
    logout: () => void;
    checkAuth: () => boolean;
    isAdmin: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem("pns_auth_token");
        const storedUser = localStorage.getItem("user");
        if (token && storedUser) {
            setIsAuthenticated(true);
            setUser(JSON.parse(storedUser));
        }
        setIsLoading(false);
    }, []);

    const login = (token: string, userData: User) => {
        localStorage.setItem("pns_auth_token", token);
        localStorage.setItem("user", JSON.stringify(userData));
        setIsAuthenticated(true);
        setUser(userData);
    };

    const logout = () => {
        localStorage.removeItem("pns_auth_token");
        localStorage.removeItem("user");
        setIsAuthenticated(false);
        setUser(null);
    };

    const checkAuth = () => {
        const token = localStorage.getItem("pns_auth_token");
        return !!token;
    }

    const isAdmin = user?.roles.includes("Admin") || false;

    if (isLoading) {
        return null; 
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, user, login, logout, checkAuth, isAdmin }}>
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
