import React, { useEffect, useState, useRef } from "react";
import "../css/EventDetailsPage.css";
import { useParams, useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";

const EventDetailsPage = () => {
    const { id } = useParams();
    const [isEnrolled, setIsEnrolled] = useState(false);
    const [event, setEvent] = useState(null);
    const [currentImageIndex, setCurrentImageIndex] = useState(0);
    const imageIntervalRef = useRef(null);
    const navigate = useNavigate();
    const [enrollmentId, setEnrollmentId] = useState();

    useEffect(() => {
        const fetchEventDetails = async () => {
            try {
                const response = await apiClient.get(`/events/${id}`);
                const eventData = response.data;

                if (eventData.images && eventData.images.length > 0) {
                    const cache = await caches.open("event-images-cache");
                    eventData.images.forEach(async (imageUrl) => {
                        const cachedResponse = await cache.match(imageUrl);
                        if (!cachedResponse) {
                            await cache.add(imageUrl);
                        }
                    });
                }
                setEvent(eventData);
            } catch (err) {
                console.error("Ошибка при загрузке события:", err);
            }
        };

        const checkEnrollment = async () => {
            try {
                const response = await apiClient.get(`/events/${id}/isEnrolled`);
                console.log(response.data);
                setIsEnrolled(response.data.isEnrolled);
                setEnrollmentId(response.data.id);
            } catch (err) {
                console.error("Ошибка при проверке записи:", err.response?.data || err.message);
            }
        };

        fetchEventDetails();
        checkEnrollment();
    }, [id]);

    const handleEnroll = async () => {
        navigate(`/enroll/${id}`);
    };

    const handleBackButton = () => {
        navigate("/events");
    };

    const handleEdit = () => {
        navigate(`/edit-enrollment/${enrollmentId}`);
    };

    useEffect(() => {
        if (event?.images?.length) {
            imageIntervalRef.current = setInterval(() => {
                handleNextImage();
            }, 10000);

            return () => {
                clearInterval(imageIntervalRef.current);
            };
        }
    }, [event, currentImageIndex]);

    const handleNextImage = () => {
        setCurrentImageIndex((prevIndex) =>
            (prevIndex + 1) % event.images.length
        );
    };

    const handlePreviousImage = () => {
        setCurrentImageIndex((prevIndex) =>
            (prevIndex - 1 + event.images.length) % event.images.length
        );
    };

    const handleDotClick = (index) => {
        setCurrentImageIndex(index);
        clearInterval(imageIntervalRef.current);
        imageIntervalRef.current = setInterval(() => {
            handleNextImage();
        }, 10000);
    };

    if (!event) return <p>Загрузка события...</p>;

    const isEventFull = event.maxParticipants - event.currentRegistrations <= 0;

    return (
        <div className="event-details-page">
            <h1 className="event-details-title">{event.title}</h1>
            {isEnrolled && (
                <p className="enrolled-message">
                    Вы уже зарегистрированы на это событие
                </p>
            )}
            <h2>Что Вас ждёт:</h2>
            <p className="event-description">{event.description}</p>

            {event.images && event.images.length > 0 && (
                <div className="image-carousel">
                    <div className="details-image-container">
                        <button
                            className="carousel-arrow carousel-arrow--prev"
                            onClick={handlePreviousImage}
                        >
                            ❮
                        </button>
                        <img
                            src={event.images[currentImageIndex]}
                            alt={`Event Image ${currentImageIndex + 1}`}
                            className="carousel-image"
                        />
                        <button
                            className="carousel-arrow carousel-arrow--next"
                            onClick={handleNextImage}
                        >
                            ❯
                        </button>
                        <div className="image-counter">
                            {currentImageIndex + 1}/{event.images.length}
                        </div>
                    </div>
                    <div className="dots-container">
                        {event.images.map((_, index) => (
                            <span
                                key={index}
                                className={`dot ${index === currentImageIndex ? "active" : ""
                                    }`}
                                onClick={() => handleDotClick(index)}
                            ></span>
                        ))}
                    </div>
                </div>
            )}
            <p className="remaining-seats">
                Осталось свободных мест:{" "}
                {event.maxParticipants - event.currentRegistrations}/
                {event.maxParticipants}
            </p>
            {isEventFull && (
                <p className="full-event-message">
                    На это событие не осталось свободных мест
                </p>
            )}
            <div className="button-container">
                {isEnrolled ? (
                    <button className="edit-button" onClick={handleEdit}>
                        Редактировать запись
                    </button>
                ) : !isEventFull ? (
                    <button className="register-button" onClick={handleEnroll}>
                        Записаться
                    </button>
                ) : null}
                <button className="back-button" onClick={handleBackButton}>
                    Назад
                </button>
            </div>

        </div>
    );
};

export default EventDetailsPage;
