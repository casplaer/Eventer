import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import "../css/RegisterPage.css";

const RegisterPage = () => {
    const [formData, setFormData] = useState({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
    });

    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            alert("Пароли не совпадают!");
            return;
        }

        try {
            await axios.post("https://localhost:7028/api/auth/register", {
                userName: formData.username,
                email: formData.email,
                password: formData.password,
                passwordConfirm: formData.confirmPassword,
            });

            navigate("/login");
        } catch (err) {
            console.log(err.response);
            const errorMessage = err.response?.data?.message || "Ошибка регистрации. Попробуйте снова.";
            setError(errorMessage);
        }
    };

    const togglePasswordVisibility = () => {
        setShowPassword((prev) => !prev);
    };

    return (
        <div className="register-page">
            <div className="register-container">
                <h1 className="register-title">Регистрация</h1>
                {error && <p className="error-message">{error}</p>}
                <form onSubmit={handleSubmit} className="register-form">
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
                        <label htmlFor="email">Email</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={formData.email}
                            onChange={handleInputChange}
                            placeholder="Введите email"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label for="password">Пароль</label>
                        <input
                            type={showPassword ? "text" : "password"}
                            id="password"
                            name="password"
                            placeholder="Введите пароль"
                            value={formData.password}
                            onChange={handleInputChange}
                            required
                        />
                        <span className="password-eye" onClick={togglePasswordVisibility}>
                            {showPassword ? "🙈" : "👁️"}
                        </span>
                    </div>

                    <div className="form-group">
                        <label htmlFor="confirmPassword">Подтвердите пароль</label>
                        <input
                            type="password"
                            id="confirmPassword"
                            name="confirmPassword"
                            value={formData.confirmPassword}
                            onChange={handleInputChange}
                            placeholder="Подтвердите пароль"
                            required
                        />
                    </div>
                    <button type="submit" className="register-button">Зарегистрироваться</button>
                </form>
            </div>
        </div>
    );
};

export default RegisterPage;
