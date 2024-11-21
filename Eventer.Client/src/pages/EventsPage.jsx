import React from "react";
import "../css/EventPage.css"; // Подключение файла стилей

const events = [
  {
    id: "1",
    title: "Тестовое событие 1",
    description: "Описание тестового события 1",
    startDate: "2024-11-20",
    startTime: "12:00",
    venue: "Место проведения 1",
    maxParticipants: 100,
    category: "Музыка",
  },
  {
    id: "2",
    title: "Тестовое событие 2",
    description: "Описание тестового события 2",
    startDate: "2024-11-21",
    startTime: "14:00",
    venue: "Место проведения 2",
    maxParticipants: 50,
    category: "Искусство",
  },
];

const EventsPage = () => {
  return (
    <div className="events-page">
      <h1 className="page-title">Список событий</h1>
      <div className="events-grid">
        {events.map((event) => (
          <div key={event.id} className="event-card">
            <h2 className="event-title">{event.title}</h2>
            <p className="event-description">{event.description}</p>
            <p className="event-details">
              <strong>Дата:</strong> {event.startDate} | <strong>Время:</strong>{" "}
              {event.startTime}
            </p>
            <p className="event-details">
              <strong>Место:</strong> {event.venue}
            </p>
            <p className="event-details">
              <strong>Категория:</strong> {event.category}
            </p>
            <p className="event-details">
              <strong>Максимум участников:</strong> {event.maxParticipants}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default EventsPage;
