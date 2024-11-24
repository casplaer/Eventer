import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "../css/EditEventPage.css";
import apiClient from "../api/apiClient";
import plus from "../assets/plus.png";

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

    const [existingImages, setExistingImages] = useState([]); 
    const [removedImages, setRemovedImages] = useState([]); 
    const [newImages, setNewImages] = useState([]);

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

                console.log(event);

                setFormData({
                    title: event.title,
                    description: event.description,
                    startDate: event.startDate,
                    startTime: event.startTime,
                    venue: event.venue,
                    category: event.category.name,
                    maxParticipants: event.maxParticipants,
                });

                setExistingImages(event.images || []);

            } catch (err) {
                const errorMessage = err.response?.data?.message || "Неизвестная ошибка.";
                setError(errorMessage);
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

    const handleImageUpload = (event) => {
        const files = Array.from(event.target.files);

        if (existingImages.length + newImages.length + files.length > 3) {
            setError("Можно загрузить не более 3 изображений.");
            return;
        }

        setNewImages((prev) => [...prev, ...files]);
    };

    const handleRemoveExistingImage = (index) => {
        const removedImage = existingImages[index];
        setExistingImages((prev) => prev.filter((_, i) => i !== index));
        setRemovedImages((prev) => [...prev, removedImage]);
    };

    const handleRemoveNewImage = (index) => {
        setNewImages((prev) => prev.filter((_, i) => i !== index));
    };

    const handleBackToAdmin = () => {
        navigate("/admin");
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

            const formDataToSend = new FormData();
            formDataToSend.append("id", id);
            formDataToSend.append("title", formData.title);
            formDataToSend.append("description", formData.description);
            formDataToSend.append("startDate", formData.startDate);
            formDataToSend.append("startTime", formData.startTime);
            formDataToSend.append("venue", formData.venue);
            formDataToSend.append("category.id", selectedCategory.id);
            formDataToSend.append("category.name", selectedCategory.name);
            formDataToSend.append("maxParticipants", formData.maxParticipants);

            existingImages.forEach((url) => {
                formDataToSend.append("existingImages", url);
            });

            removedImages.forEach((url) => {
                formDataToSend.append("removedImages", url);
            });

            newImages.forEach((file) => {
                formDataToSend.append("images", file);
            });

            await apiClient.put(`/events`, formDataToSend, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
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
                <div className="form-group">
                    <label>Изображения:</label>
                    <div className="images-wrapper">
                        {existingImages.map((url, index) => (
                            <div key={index} className="image-container">
                                <img
                                    src={url}
                                    alt={`Existing ${index + 1}`}
                                />
                                <button
                                    type="button"
                                    onClick={() => handleRemoveExistingImage(index)}
                                >
                                    ×
                                </button>
                            </div>
                        ))}
                        {newImages.map((file, index) => (
                            <div key={`new-${index}`} className="image-container">
                                <img
                                    src={URL.createObjectURL(file)}
                                    alt={`New ${index + 1}`}
                                />
                                <button
                                    type="button"
                                    onClick={() => handleRemoveNewImage(index)}
                                >
                                    ×
                                </button>
                            </div>
                        ))}
                        {existingImages.length + newImages.length < 3 && (
                            <label className="image-container">
                                <img src={plus} alt="Add" className="add-new-image-button" />
                                <input
                                    type="file"
                                    accept="image/*"
                                    multiple
                                    onChange={handleImageUpload}
                                    style={{ display: "none" }}
                                />
                            </label>
                        )}
                    </div>
                </div>


                <button type="submit" className="save-button">Сохранить изменения</button>
                <button onClick={handleBackToAdmin} className="back-to-admin-button">
                    Назад к Администрированию
                </button>
            </form>
        </div>
    );
};

export default EditEventPage;
