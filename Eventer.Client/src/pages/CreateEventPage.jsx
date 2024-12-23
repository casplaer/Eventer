import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "../css/CreateEventPage.css";
import apiClient from "../api/apiClient";
import ImageUploader from '../components/ImageUploader';
import plus from "../assets/plus.png";

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
    const [images, setImages] = useState([]);

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
                const categoriesData = response.data.categories.$values;

                setCategories(categoriesData);

                if (categoriesData.length > 0) {
                    setFormData((prev) => ({
                        ...prev,
                        category: categoriesData[0].name,
                    }));
                }
            } catch (err) {
                const errorMessage = err.response?.data?.message || "Ошибка при получении категорий.";
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

    const handleImageUpload = (event) => {
        const files = Array.from(event.target.files);

        if (images.length + files.length > 3) {
            setError("Можно загрузить не более 3 изображений.");
            return;
        }

        setImages((prev) => [...prev, ...files]);
    };

    const handleRemoveImage = (index) => {
        const updatedImages = images.filter((_, i) => i !== index);
        setImages(updatedImages);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        const selectedCategory = categories.find(category => category.name === formData.category);
        formData.category = selectedCategory;

        const formDataToSend = new FormData();

        images.forEach((file, index) => {
            formDataToSend.append("images", file); 
        });

        formDataToSend.append("title", formData.title);
        formDataToSend.append("description", formData.description);
        formDataToSend.append("startDate", formData.date);
        formDataToSend.append("startTime", formData.time);
        formDataToSend.append("venue", formData.venue);
        formDataToSend.append("category.Id", formData.category.id);
        formDataToSend.append("category.Name", formData.category.name); 
        formDataToSend.append("category.Description", formData.category.description || "");
        formDataToSend.append("maxParticipants", formData.maxParticipants);

        for (let [key, value] of formDataToSend.entries()) {
            console.log(`${key}:`, value);
        }


        try {
            const response = await apiClient.post("/events", formDataToSend,
                {
                    headers:
                    {
                        "Content-Type": "multipart/form-data" 
                    }
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
            setImages([]);
        } catch (err) {
            const errorMessage = err.response?.data?.message || "Ошибка при создании события.";
            setError(errorMessage);
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
                <div style={{ display: "flex", gap: "10px" }}>
                    {images.map((image, index) => (
                        <div
                            key={index}
                            style={{
                                position: "relative",
                                width: "150px",
                                height: "150px",
                            }}
                        >
                            <img
                                src={URL.createObjectURL(image)}
                                alt={`Uploaded ${index + 1}`}
                                style={{
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                    borderRadius: "8px",
                                }}
                            />
                            <button
                                onClick={() => handleRemoveImage(index)}
                                style={{
                                    position: "absolute",
                                    top: "5px",
                                    right: "5px",
                                    background: "black",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "0%",
                                    width: "20px",
                                    height: "20px",
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    cursor: "pointer",
                                }}
                            >
                                ×
                            </button>
                        </div>
                    ))}
                    {images.length < 3 && (
                        <label
                            style={{
                                width: "150px",
                                height: "150px",
                                background: "lightgray",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                cursor: "pointer",
                                borderRadius: "4px",
                                fontSize: "35px",
                            }}
                        >
                            <img
                                src={plus}
                                alt="Add"
                                style={{ width: "25px", height: "25px" }}
                            />
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
