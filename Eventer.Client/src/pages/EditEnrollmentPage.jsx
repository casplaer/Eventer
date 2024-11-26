import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";
import deleteIcon from '../assets/delete.png';
import "../css/EditEnrollmentPage.css";

const EditEnrollmentPage = () => {
    const { enrollmentId } = useParams();
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        enrollmentId: enrollmentId,
        name: "",
        surname: "",
        email: "",
        dateOfBirth: "",
    });

    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [loading, setLoading] = useState(true);
    const [eventId, setEventId] = useState();
    const [eventName, setEventName] = useState();
    const [showPopup, setShowPopup] = useState(false);

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

    useEffect(() => {
        const fetchEnrollment = async () => {
            try {
                const response = await apiClient.get(`/enrollments/${enrollmentId}`);

                const enrollment = response.data;

                setEventId(enrollment.eventId);

                const eventResponse = await apiClient.get(`/events/${enrollment.eventId}`);
                setEventName(eventResponse.data.title);

                setFormData({
                    enrollmentId: enrollment.enrollmentId,
                    name: enrollment.name,
                    email: enrollment.email,
                    surname: enrollment.surname,
                    dateOfBirth: enrollment.dateOfBirth,
                });
            } catch (err) {
                console.error("Ошибка при загрузке записи:", err.response?.data || err.message);
                setError(err.response?.data?.message || "Не удалось загрузить данные записи.");
            } finally {
                setLoading(false);
            }
        };

        fetchEnrollment();
    }, [enrollmentId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleBack = () => {
        navigate(`/details/${eventId}`);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        try {
            await apiClient.put(`/enrollments`, formData);
            setSuccess("Запись успешно обновлена!");
            setTimeout(() => navigate("/events"), 2000);
        } catch (err) {
            console.error("Ошибка при обновлении записи:", err.response?.data || err.message);
            setError("Не удалось обновить запись. Попробуйте снова.");
        }
    };

    const handleDelete = async () => {
        try {
            await apiClient.delete(`/enrollments/${enrollmentId}`);
            navigate(`/details/${eventId}`);
        } catch (err) {
            console.error("Ошибка при удалении записи:", err.response?.data || err.message);
            setError("Не удалось удалить запись. Попробуйте снова.");
        }
    };

    const handleConfirmDelete = () => {
        setShowPopup(true);
    };

    const handleCancelDelete = () => {
        setShowPopup(false);
    };

    return (
        <div className="edit-enrollment-page">
            <button className="enrollment-delete-button" onClick={handleConfirmDelete}>
                <img src={deleteIcon} alt="Удалить" className="delete-icon" />
            </button>
            <h1>Редактировать запись на событие</h1>
            <h1>{eventName}</h1>
            {loading && <p>Загрузка данных...</p>}
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
            {!loading && (
                <form className="edit-enrollment-form" onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="name">Имя</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={formData.name}
                            onChange={handleInputChange}
                            placeholder="Введите ваше имя"
                            minLength="4"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="surname">Фамилия</label>
                        <input
                            type="text"
                            id="surname"
                            name="surname"
                            value={formData.surname}
                            onChange={handleInputChange}
                            placeholder="Введите вашу фамилию"
                            minLength="3"
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
                            placeholder="Введите вашу почту"
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
                    <button type="submit" className="submit-button">
                        Сохранить изменения
                    </button>
                    <button type="button" onClick={handleBack} className="back-button">
                        Назад
                    </button>
                </form>
            )}
            {showPopup && (
                <div className="popup-overlay">
                    <div className="popup">
                        <h2>Вы точно хотите удалить запись на</h2>
                        <h2>{eventName}?</h2>
                        <div className="popup-actions">
                            <button onClick={handleDelete} className="confirm-button">
                                Да
                            </button>
                            <button onClick={handleCancelDelete} className="cancel-button">
                                Нет
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default EditEnrollmentPage;
