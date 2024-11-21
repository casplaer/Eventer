import React, { useState } from "react";
import "../css/Sidebar.css";

const Sidebar = () => {
    const [isOpen, setIsOpen] = useState(false);

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
                <ul className="sidebar-links">
                    <li><a href="#admin">Администрированиe</a></li>
                    <li><a href="#about">О проекте</a></li>
                    <li><a href="#events">Список событий</a></li>
                    <li><a href="#login">Войти</a></li>
                </ul>
            </div>
        </div>
    );
};

export default Sidebar;
