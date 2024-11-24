import React, { createContext, useState, useContext, useEffect } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children, navigate }) => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const accessToken = sessionStorage.getItem("accessToken");
        const storedUser = sessionStorage.getItem("user");

        if (accessToken && storedUser) {
            setUser(JSON.parse(storedUser));
        }
    }, []);

    const login = (userData) => {
        setUser(userData);
        sessionStorage.setItem("user", JSON.stringify(userData));
    };

    const logout = () => {
        setUser(null);
        sessionStorage.clear();
        navigate("/about");
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
