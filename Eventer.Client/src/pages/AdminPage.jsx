import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";
import { jwtDecode } from "jwt-decode";
import "../css/AdminPage.css";

const AdminPage = () => {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [showPopup, setShowPopup] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);

    const navigate = useNavigate();

    const token = sessionStorage.getItem("accessToken");

    useEffect(() => {
        const role = token ? jwtDecode(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : null;
        if (role !== "Admin") {
            navigate("/about");
        }
    }, [navigate]);

    const fetchEvents = async (page) => {
        try {
            const response = await apiClient.get("/events", {
                params: {
                    page,
                },
            });
            setEvents(response.data.events);
            setTotalPages(response.data.totalPages);
            setLoading(false);
        } catch (err) {
            setError(err.response?.data?.message || "Ошибка при загрузке событий.");
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvents(page);
    }, [page]);

    const handlePrevious = () => {
        if (page > 1) setPage(page - 1);
    };

    const handleDeleteClick = (event) => {
        setSelectedEvent(event);
        setShowPopup(true);
    };

    const handleCancelDelete = () => {
        setShowPopup(false);
        setSelectedEvent(null);
    };

    const handleEditClick = (eventId) => {
        navigate(`/edit_event/${eventId}`);
    };

    const handleConfirmDelete = async () => {
        if (!selectedEvent) return;

        try {
            await apiClient.delete(`/events/${selectedEvent.id}`);

            const updatedEvents = events.filter((event) => event.id !== selectedEvent.id);

            if (updatedEvents.length > 0 || page === 1) {
                setEvents(updatedEvents);
            } else {
                setPage((prevPage) => Math.max(prevPage - 1, 1));
            }

            setShowPopup(false);
            setSelectedEvent(null);

            fetchEvents(page);
        } catch (err) {
            console.error("Ошибка при удалении события:", err.response?.data || err.message);
            setError("Не удалось удалить событие. Попробуйте снова.");
        }
    };

    const handleNext = () => {
        if (page < totalPages) setPage(page + 1);
    };

    const handleCreateEvent = () => {
        navigate("/create_event");
    };

    if (loading) return <p>Загрузка событий...</p>;
    if (error) return <p className="admin-error-message">{error}</p>;

    return (
        <div className="admin-page">
            <h1>Администрирование событий</h1>

            <button onClick={handleCreateEvent} className="create-event-button">
                Создать событие
            </button>

            {events.length === 0 ? (
                <p className="no-events-message">На данный момент событий нет.</p>
            ) : (
                <>
                    <div className="events-list">
                        {events.map((event) => (
                            <div key={event.id} className="event-card">
                                <h2>{event.title}</h2>
                                <p className="event-id">ID: {event.id}</p>
                                <div className="event-actions">
                                    <button
                                        className="edit-button"
                                        onClick={() => handleEditClick(event.id)}
                                    >
                                        Редактировать
                                    </button>
                                    <button
                                        className="delete-button"
                                        onClick={() => handleDeleteClick(event)}
                                    >
                                        Удалить
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                    <div className="pagination">
                        <button onClick={handlePrevious} disabled={page === 1}>
                            Назад
                        </button>
                        <span>Страница {page} из {totalPages}</span>
                        <button onClick={handleNext} disabled={page === totalPages}>
                            Вперед
                        </button>
                    </div>
                </>
            )}

            {showPopup && selectedEvent && (
                <div className="popup-overlay">
                    <div className="popup">
                        <h2>Вы точно хотите удалить событие?</h2>
                        <p>Название: <strong>{selectedEvent.title}</strong></p>
                        <p>ID: <strong>{selectedEvent.id}</strong></p>
                        <div className="popup-actions">
                            <button onClick={handleConfirmDelete} className="confirm-button">
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

export default AdminPage;
