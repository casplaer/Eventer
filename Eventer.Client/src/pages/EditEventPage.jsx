import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "../css/EditEventPage.css";
import apiClient from "../api/apiClient";

const EditEventPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        title: "",
        description: "",
        startDate: "",
        startTime: "",
        venue: "",
        category: "",
        maxParticipants: "",
    });
    const [categories, setCategories] = useState([]);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    const token = sessionStorage.getItem("accessToken");

    useEffect(() => {
        const role = token ? jwtDecode(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : null;
        if (role !== "Admin") {
            navigate("/about");
        }
    }, [navigate]);

    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const response = await apiClient.get(`/events/${id}`);
                const event = response.data;
                setFormData({
                    title: event.title,
                    description: event.description,
                    startDate: event.startDate,
                    startTime: event.startTime,
                    venue: event.venue,
                    category: event.category.name,
                    maxParticipants: event.maxParticipants,
                });
            } catch (err) {
                console.error("Ошибка при загрузке события:", err);
                setError("Не удалось загрузить данные события.");
            }
        };

        const fetchCategories = async () => {
            try {
                const response = await apiClient.get("/categories");
                setCategories(response.data.categories);
            } catch (err) {
                console.error("Ошибка при загрузке категорий:", err);
                setError("Не удалось загрузить категории.");
            }
        };

        fetchEvent();
        fetchCategories();
    }, [id]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        try {
            const selectedCategory = categories.find(
                (category) => category.name === formData.category
            );

            if (!selectedCategory) {
                setError("Категория не найдена. Пожалуйста, выберите корректную категорию.");
                return;
            }

            console.log(formData);

            await apiClient.put(`/events`, {
                id: id,
                title: formData.title,
                description: formData.description,
                startDate: formData.startDate,
                startTime: formData.startTime,
                venue: formData.venue,
                category: {
                    id: selectedCategory.id,
                    name: selectedCategory.name,
                    description: selectedCategory.description || null,
                },
                maxParticipants: parseInt(formData.maxParticipants, 10),
            });

            setSuccess("Событие успешно обновлено!");
            navigate("/admin");
        } catch (err) {
            console.error("Ошибка при обновлении события:", err);
            setError("Не удалось обновить событие. Попробуйте снова.");
        }
    };

    return (
        <div className="edit-event-page">
            <h1>Редактировать событие</h1>
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
            <form onSubmit={handleSubmit} className="edit-event-form">
                <div className="form-group">
                    <label htmlFor="title">Название события</label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={formData.title}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="description">Описание</label>
                    <textarea
                        id="description"
                        name="description"
                        value={formData.description}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="startDate">Дата</label>
                    <input
                        type="date"
                        id="startDate"
                        name="startDate"
                        value={formData.startDate}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="startTime">Время</label>
                    <input
                        type="time"
                        id="startTime"
                        name="startTime"
                        value={formData.startTime}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="venue">Место проведения</label>
                    <input
                        type="text"
                        id="venue"
                        name="venue"
                        value={formData.venue}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="category">Категория</label>
                    <select
                        id="category"
                        name="category"
                        value={formData.category}
                        onChange={handleInputChange}
                        required
                    >
                        <option value="">Выберите категорию</option>
                        {categories.map((category) => (
                            <option key={category.id} value={category.name}>
                                {category.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="maxParticipants">Максимум участников</label>
                    <input
                        type="number"
                        id="maxParticipants"
                        name="maxParticipants"
                        value={formData.maxParticipants}
                        onChange={handleInputChange}
                        min="1"
                        required
                    />
                </div>
                <button type="submit" className="save-button">Сохранить изменения</button>
            </form>
        </div>
    );
};
export default EditEventPage;