import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "../css/CreateEventPage.css";
import apiClient from "../api/apiClient";
import ImageUploader from '../components/ImageUploader'

const CreateEventPage = () => {
    const [formData, setFormData] = useState({
        title: "",
        description: "",
        date: "",
        time: "12:00",
        venue: "",
        category: "",
        maxParticipants: "",
        images: [],
    });
    const [categories, setCategories] = useState([]);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const navigate = useNavigate();
    const token = sessionStorage.getItem("accessToken");

    useEffect(() => {
        const role = token ? jwtDecode(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : null;
        if (role !== "Admin") {
            navigate("/about");
        }
    }, [navigate]);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await apiClient.get("/categories");
                const categoriesData = response.data.categories;

                setCategories(categoriesData);

                if (categoriesData.length > 0) {
                    setFormData((prev) => ({
                        ...prev,
                        category: categoriesData[0].name,
                    }));
                }
            } catch (err) {
                const errorMessage = err.response?.data?.message || "Ошибка при создании события.";
                setError(errorMessage);
            }
        };

        fetchCategories();

        const tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);

        const formattedDate = tomorrow.toISOString().split("T")[0];
        setFormData((prev) => ({
            ...prev,
            date: formattedDate,
        }));
    }, []);

    const handleBackToAdmin = () => {
        navigate("/admin");
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleImageChange = (e) => {
        const files = Array.from(e.target.files);
        if (formData.images.length + files.length > 3) {
            setError("Можно загрузить не более трех изображений.");
            return;
        }
        const newImages = files.map((file) => ({
            file,
            url: URL.createObjectURL(file),
        }));
        setFormData({ ...formData, images: [...formData.images, ...newImages] });
    };

    const handleRemoveImage = (index) => {
        const updatedImages = formData.images.filter((_, i) => i !== index);
        setFormData({ ...formData, images: updatedImages });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        const formDataToSend = new FormData();

        formDataToSend.append("title", formData.title);
        formDataToSend.append("description", formData.description);
        formDataToSend.append("startDate", formData.date);
        formDataToSend.append("startTime", formData.time);
        formDataToSend.append("venue", formData.venue);
        formDataToSend.append("category", formData.category);
        formDataToSend.append("maxParticipants", formData.maxParticipants);

        formData.images.forEach((image, index) => {
            formDataToSend.append(`images[${index}]`, image.file);
        });

        try {
            const response = await apiClient.post("/events", formDataToSend, {
                headers: { "Content-Type": "multipart/form-data" },
            });

            console.log("Событие успешно создано!", response.data);
            setSuccess("Событие успешно создано!");
            setFormData({
                title: "",
                description: "",
                date: formData.date,
                time: "12:00",
                venue: "",
                category: categories[0]?.name || "",
                maxParticipants: "",
                images: [],
            });
        } catch (err) {
            const errorResponse = err.response?.data;
            const firstError = errorResponse?.errors
                ? Object.values(errorResponse.errors)[0][0]
                : "Ошибка при создании события.";
            setError(firstError);
            console.error("Ошибка при создании события:", err.response?.data || err.message);
        }
    };

    return (
        <div className="create-event-page">
            <h1>Создать новое событие</h1>
            <form onSubmit={handleSubmit} className="create-event-form">
                <div className="form-group">
                    <label htmlFor="title">Название события</label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={formData.title}
                        onChange={handleInputChange}
                        placeholder="Введите название события"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="description">Описание события</label>
                    <textarea
                        id="description"
                        name="description"
                        value={formData.description}
                        onChange={handleInputChange}
                        placeholder="Введите описание события"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="date">Дата</label>
                    <input
                        type="date"
                        id="date"
                        name="date"
                        value={formData.date}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="time">Время</label>
                    <input
                        type="time"
                        id="time"
                        name="time"
                        value={formData.time}
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
                        placeholder="Введите место проведения"
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
                        placeholder="Введите максимум участников"
                        min="1"
                        required
                    />
                </div>
{/*                <div className="form-group">
                    <label htmlFor="images">Загрузите изображения (не более 3)</label>
                    <input
                        type="file"
                        id="images"
                        name="images"
                        multiple
                        accept="image/*"
                        onChange={handleImageChange}
                    />
                    <div className="image-previews">
                        {formData.images.map((image, index) => (
                            <div key={index} className="image-preview">
                                <img src={image.url} alt={`Preview ${index + 1}`} />
                                <button type="button" onClick={() => handleRemoveImage(index)}>
                                    ✕
                                </button>
                            </div>
                        ))}
                    </div>
                </div>*/}
                <ImageUploader />
                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}
                <button type="submit" className="create-event-button">Создать</button>
                <button onClick={handleBackToAdmin} className="back-to-admin-button">
                    Назад к Администрированию
                </button>
            </form>
        </div>
    );
};

export default CreateEventPage;
