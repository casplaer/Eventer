import React, { useEffect, useState } from "react";
import "../css/UserEventsPage.css";
import { useNavigate, Link } from "react-router-dom";
import apiClient from "../api/apiClient";
import { jwtDecode } from "jwt-decode";
import usersIcon from "../assets/multiple-users.png";

const UserEventsPage = () => {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUserEvents = async () => {
            try {
                const token = sessionStorage.getItem("accessToken");
                const userId = token
                    ? jwtDecode(token)["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
                    : null;

                if (!userId) {
                    setError("Не удалось определить пользователя. Войдите в систему.");
                    setLoading(false);
                    return;
                }

                const response = await apiClient.get("/events/your-events", {
                    params: {
                        UserId: userId,
                        Page: 1,
                    },
                });

                setEvents(response.data.events);
            } catch (err) {
                console.error("Ошибка при загрузке событий:", err.response?.data || err.message);
                setError("Не удалось загрузить события. Попробуйте позже.");
            } finally {
                setLoading(false);
            }
        };

        fetchUserEvents();
    }, []);

    const handleEventClick = (eventId) => {
        navigate(`/details/${eventId}`);
    };

    if (loading) return <p>Загрузка событий...</p>;
    if (error) return <p className="error-message">{error}</p>;

    return (
        <div className="user-events-page">
            <h1>События, на которые вы записаны</h1>
            {events.length === 0 ? (
                <div className="no-events-message">
                    <p>Вы пока не записаны ни на одно событие.</p>
                    <p>
                        Вы можете записаться на событие на этой{" "}
                        <Link to="/events" className="link-to-events">странице</Link>.
                    </p>
                </div>
            ) : (
                <div className="events-list">
                    {events.map((event) => (
                        <div
                            key={event.id}
                            className="event-card"
                            onClick={() => handleEventClick(event.id)}
                        >
                            <h2 className="event-title">{event.title}</h2>
                            <p className="event-date">
                                Дата: {event.startDate} {event.startTime}
                            </p>
                            <p className="event-venue">Место: {event.venue}</p>
                            <div className="event-meta">
                                <span className="event-registrations">
                                    Зарегистрировано: {event.currentRegistrations}/{event.maxParticipants}
                                </span>
                                <img src={usersIcon} alt="Users" className="users-icon" />
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default UserEventsPage;
