import React, { useState } from "react";
import { useAuth } from '../components/AuthContext'
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import "../css/LoginPage.css";

const LoginPage = () => {
    const [formData, setFormData] = useState({ username: "", password: "" });
    const [error, setError] = useState(null);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post("https://localhost:7028/api/auth/login", {
                userName: formData.username,
                password: formData.password,
            });

            const AccessToken = response.data["accessToken"];
            const RefreshToken = response.data["refreshToken"];
            const user = response.data["user"];

            sessionStorage.setItem("accessToken", AccessToken);
            sessionStorage.setItem("refreshToken", RefreshToken);

            login(user);

            navigate("/events");
        } catch (err) {
            const errorMessage = err.response?.data?.message || "Произошла ошибка при входе. Проверьте данные.";
            setError(errorMessage);
        }
    };


    return (
        <div className="login-page">
            <div className="login-container">
                <h1 className="login-title">Войти</h1>
                <form onSubmit={handleSubmit} className="login-form">
                    <div className="form-group">
                        <label htmlFor="username">Логин</label>
                        <input
                            type="text"
                            id="username"
                            name="username"
                            value={formData.username}
                            onChange={handleInputChange}
                            placeholder="Введите логин"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label>Пароль</label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleInputChange}
                            placeholder="Введите пароль"
                            required
                        />
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    <button type="submit" className="login-button">Войти</button>
                </form>
                <div className="login-footer">
                    <span>Нет аккаунта?</span>
                    <Link to="/register" className="register-button">
                        Зарегистрироваться
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;
