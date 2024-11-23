import React, { useState } from "react";
import "../css/Sidebar.css";
import { Link } from "react-router-dom";
import { useAuth } from "../components/AuthContext";

const Sidebar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const { user, logout } = useAuth();

    const toggleSidebar = () => {
        setIsOpen(!isOpen);
    };

    return (
        <div className={`sidebar-container ${isOpen ? "open" : ""}`}>
            <div className="hamburger" onClick={toggleSidebar}>
                <div className="line"></div>
                <div className="line"></div>
                <div className="line"></div>
            </div>

            <div className="sidebar">
                {user && <p className="sidebar-greeting">Привет, {user.userName}!</p>}
                <ul className="sidebar-links">
                    <li><Link to="/admin">Администрированиe</Link></li>
                    <li><Link to="/about">О проекте</Link></li>
                    <li><Link to="/events">Список событий</Link></li>
                    {user ? (
                        <li>
                            <button onClick={logout} className="logout-button">Выйти</button>
                        </li>
                    ) : (
                        <li>
                            <Link to="/login">Войти</Link>
                        </li>
                    )}
                </ul>
            </div>
        </div>
    );
};

export default Sidebar;
