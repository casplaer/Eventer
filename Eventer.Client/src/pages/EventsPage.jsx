import React, { useEffect, useState } from "react";
import { getEvents } from "../api/events";
import '../css/EventPage.css'

const EventsPage = () => {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Функция для загрузки данных
        const fetchEvents = async () => {
            try {
                const data = await getEvents(); // Вы можете передать параметры фильтра
                setEvents(data); 
            } catch (error) {
                console.error("Ошибка при загрузке событий:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchEvents();
    }, []);

    if (loading) {
        return <p>Загрузка...</p>;
    }

    return (
        <div>
            <h1>Список событий</h1>
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
