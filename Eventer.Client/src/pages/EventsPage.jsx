import React, { useEffect, useState } from "react";
import '../css/EventPage.css'
import apiClient from "../api/apiClient";

const EventsPage = () => {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null); 

    const [filters, setFilters] = useState({
        title: "",
        date: "",
        venue: "",
        category: "",
    });

    const fetchEvents = async () => {
        console.log("Вызов fetchEvents");
        try {
            const response = await apiClient.get("/events", {
                params: {
                    Title: filters.title || null,
                    Date: filters.date || null,
                    Venue: filters.venue || null,
                    Category: filters.category || null,
                },
            });
            setEvents(response.data.events);
            setLoading(false);
        }
        catch (err) {
            if (error.response?.status === 401) {
                setError("Перенаправление...");
            }
            setError(err.response?.data?.message || "Ошибка при загрузке событий");
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvents();
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({ ...prev, [name]: value }));
    };

    const handleFilterSubmit = (e) => {
        e.preventDefault();
        fetchEvents();
    };

    if (loading) return <p>Загрузка событий...</p>;
    if (error) return <p className="error-message">{error}</p>;

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
                        value={filters.category}
                        onChange={handleInputChange}
                    >
                        <option value="">Все категории</option>
                        <option value="Music">Музыка</option>
                        <option value="Sport">Спорт</option>
                        <option value="Education">Образование</option>
                        <option value="Art">Искусство</option>
                    </select>
                </div>
                <button type="submit" className="filter-button">Применить фильтры</button>
            </form>

            <div className="events-grid">
                {events.map((event) => (
                    <div key={event.id} className="event-card">
                        <h2>{event.title}</h2>
                        <p>{event.description}</p>
                        <p>
                            Дата: {event.startDate} | Время: {event.startTime}
                        </p>
                        <p>Место: {event.venue}</p>
                        <p>Категория: {event.category.name}</p>
                        <p>Максимум участников: {event.maxParticipants}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default EventsPage;
