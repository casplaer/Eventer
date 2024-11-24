import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";
import "../css/EnrollPage.css";

const EnrollPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [event, setEvent] = useState(null);

    const user = JSON.parse(sessionStorage.getItem("user"));
    const userEmail = user?.email || ""; 

    const [formData, setFormData] = useState({
        eventId: id,
        name: "",
        surName: "",
        dateOfBirth: "",
        email: userEmail,
    });

    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [loading, setLoading] = useState(true);

    const getMinMaxDates = () => {
        const today = new Date();
        const minDate = new Date(today.getFullYear() - 70, today.getMonth(), today.getDate());
        const maxDate = new Date(today.getFullYear() - 12, today.getMonth(), today.getDate());

        return {
            min: minDate.toISOString().split("T")[0],
            max: maxDate.toISOString().split("T")[0],
        };
    };

    const { min, max } = getMinMaxDates();

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleBackButton = () => {
        navigate(`/details/${id}`);
    }

    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const response = await apiClient.get(`/events/${id}`);
                setEvent(response.data);
            } catch (err) {
                console.error("Ошибка при загрузке события:", err.response?.data || err.message);
                setError("Не удалось загрузить данные события.");
            } finally {
                setLoading(false);
            }
        };

        fetchEvent();
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        try {
            await apiClient.post("/events/enroll", formData);
            setSuccess("Вы успешно записались на событие!");
            setTimeout(() => {
                navigate("/events");
            }, 2000);
        } catch (err) {
            console.error("Ошибка при записи на событие:", err.response?.data || err.message);
            setError("Не удалось записаться на событие. Проверьте данные и попробуйте снова.");
        }
    };

    return (
        <div className="enroll-page">
            <h1>Запись на событие</h1>
            <h1>{event ? event.title : "Событие"}</h1>
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
            <form className="enroll-form" onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="name">Имя</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleInputChange}
                        placeholder="Введите ваше имя"
                        minLength = "4"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="surName">Фамилия</label>
                    <input
                        type="text"
                        id="surName"
                        minLength="3"
                        name="surName"
                        value={formData.surName}
                        onChange={handleInputChange}
                        placeholder="Введите вашу фамилию"
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
                        placeholder="Введите ваш email"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="dateOfBirth">Дата рождения</label>
                    <input
                        type="date"
                        id="dateOfBirth"
                        name="dateOfBirth"
                        value={formData.dateOfBirth}
                        onChange={handleInputChange}
                        min={min}
                        max={max}
                        required
                    />
                </div>
                <button type="submit" className="enroll-button">
                    Записаться
                </button>
                <button className="back-button" onClick={ handleBackButton }>
                    Назад
                </button>
            </form>
        </div>
    );
};

export default EnrollPage;
