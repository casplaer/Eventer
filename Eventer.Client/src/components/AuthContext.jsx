import React, { createContext, useState, useContext, useEffect } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children, navigate }) => {
    const [user, setUser] = useState(null);

    // ѕроверка наличи€ токена при загрузке приложени€
    useEffect(() => {
        const accessToken = sessionStorage.getItem("accessToken");
        const storedUser = sessionStorage.getItem("user");

        if (accessToken && storedUser) {
            setUser(JSON.parse(storedUser));
        }
    }, []);

    const login = (userData) => {
        setUser(userData);
        sessionStorage.setItem("user", JSON.stringify(userData)); // —охранение данных пользовател€
    };

    const logout = () => {
        setUser(null);
        sessionStorage.clear(); // ”даление всех данных из сессии
        navigate("/about");
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
