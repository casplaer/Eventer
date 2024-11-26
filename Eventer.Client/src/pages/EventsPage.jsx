import React, { useEffect, useState } from "react";
import "../css/EventPage.css";
import apiClient from "../api/apiClient";
import usersIcon from "../assets/multiple-users.png";

const EventsPage = () => {
    const [events, setEvents] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1); 
    const [totalPages, setTotalPages] = useState(1); 

    const [filters, setFilters] = useState({
        title: "",
        date: "",
        venue: "",
        category: "",
    });

    const fetchEvents = async () => {
        try {
            const response = await apiClient.get("/events", {
                params: {
                    Title: filters.title || null,
                    Date: filters.date || null,
                    Venue: filters.venue || null,
                    CategoryId: filters.category?.id || null,
                    page,
                },
            });

            console.log(response.data);

            setEvents(response.data.events);
            setTotalPages(response.data.totalPages);
            setLoading(false);
        } catch (err) {
            if (err.response?.status === 401) {
                setError("Перенаправление...");
            } else {
                setError(err.response?.data?.message || "Ошибка при загрузке событий.");
            }
            setLoading(false);
        }
    };

    const fetchCategories = async () => {
        try {
            const response = await apiClient.get("/categories");
            setCategories(response.data.categories);
        } catch (err) {
            console.error("Ошибка при загрузке категорий:", err.response?.data?.message || err.message);
            setError("Не удалось загрузить категории.");
        }
    };

    useEffect(() => {
        fetchEvents();
        fetchCategories();
    }, [page]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        if (name === "category") {
            const selectedCategory = categories.find((category) => category.id === value);
            setFilters((prev) => ({ ...prev, category: selectedCategory || null }));
        } else {
            setFilters((prev) => ({ ...prev, [name]: value }));
        }
    };

    const handleFilterSubmit = (e) => {
        e.preventDefault();
        setPage(1);
        fetchEvents();
    };

    const handlePrevious = () => {
        if (page > 1) setPage(page - 1);
    };

    const handleNext = () => {
        if (page < totalPages) setPage(page + 1);
    };

    if (loading) return <p className="loading-text">Загрузка событий...</p>;
    if (error) return <p className="events-error-message">{error}</p>;

    return (
        <div>
            <h1>Список событий</h1>

            <form className="filter-form" onSubmit={handleFilterSubmit}>
                <div className="form-group">
                    <label htmlFor="title">Название</label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={filters.title}
                        onChange={handleInputChange}
                        placeholder="Введите название события"
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="date">Дата</label>
                    <input
                        type="date"
                        id="date"
                        name="date"
                        value={filters.date}
                        onChange={handleInputChange}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="venue">Место</label>
                    <input
                        type="text"
                        id="venue"
                        name="venue"
                        value={filters.venue}
                        onChange={handleInputChange}
                        placeholder="Введите место проведения"
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="category">Категория</label>
                    <select
                        id="category"
                        name="category"
                        value={filters.category?.id || ""}
                        onChange={handleInputChange}
                    >
                        <option value="">Все категории</option>
                        {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                                {category.name}
                            </option>
                        ))}
                    </select>
                </div>
                <button type="submit" className="filter-button">Применить фильтры</button>
            </form>

            {events.length === 0 ? (
                <p className="no-events-message">На данный момент нет подходящих событий.</p>
            ) : (
                <div className="events-list">
                    {events.map((event) => (
                        <div key={event.id} className="event-card">
                            <h2
                                className="event-title"
                                onClick={() => (window.location.href = `/details/${event.id}`)}
                            >
                                {event.title}
                            </h2>
                            <p className="event-address">Адрес: {event.venue}</p>
                            <div className="event-footer">
                                <span className="event-participants">
                                    {event.currentRegistrations}/{event.maxParticipants}
                                </span>
                                <img
                                    src={usersIcon}
                                    alt="Icon"
                                    className="event-icon"
                                />
                            </div>
                        </div>
                    ))}
                </div>)}

            <div className="pagination">
                <button onClick={handlePrevious} disabled={page === 1}>
                    Назад
                </button>
                <span>Страница {page} из {totalPages}</span>
                <button onClick={handleNext} disabled={page === totalPages}>
                    Вперед
                </button>
                    </div>
        </div>
    );
};

export default EventsPage;
