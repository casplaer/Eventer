import React from "react";
import "../css/AboutPage.css";

const AboutPage = () => {
    return (
        <div className="about-page">
            <header className="about-header">
                <h1 className="welcome-title">Добро пожаловать!</h1>
                <p className="subtitle">Тестовое задание для стажировки</p>
            </header>

            <section className="project-overview">
                <h2 className="section-title">Общая информация</h2>
                <p className="project-description">
                    Это приложение предназначено для регистрации и управления событиями. Оно разработано с использованием
                    следующих технологий:
                </p>
                <ul className="tech-list">
                    <li><strong>Клиентская часть:</strong> React</li>
                    <li><strong>Серверная часть:</strong> ASP.NET Core с архитектурным подходом "Чистая архитектура"</li>
                    <li><strong>Аутентификация и авторизация:</strong> JWT Access и Refresh токены</li>
                </ul>
            </section>

            <section className="features-section">
                <h2 className="section-title">Возможности приложения</h2>
                <div className="roles">
                    <div className="role">
                        <h3>Администратор</h3>
                        <ul className="role-capabilities">
                            <li>Редактирование информации о событии</li>
                            <li>Удаление события</li>
                            <li>Создание события</li>
                            <li>Просмотр списка участников события</li>
                        </ul>
                    </div>
                    <div className="role">
                        <h3>Обычный пользователь</h3>
                        <ul className="role-capabilities">
                            <li>Просмотр списка событий</li>
                            <li>Просмотр подробной информации о событии</li>
                            <li>Запись на событие</li>
                            <li>Редактирование записи на событие</li>
                            <li>Просмотр списка событий, на которые он записан</li>
                        </ul>
                    </div>
                </div>
            </section>

            <section className="test-users">
                <h2 className="section-title">Тестовые пользователи</h2>
                <p>
                    Для проверки функционала приложения доступны следующие тестовые учетные записи:
                </p>
                <ul className="test-users-list">
                    <li><strong>Администратор:</strong> TestAdmin / 123qwe</li>
                    <li><strong>Обычный пользователь:</strong> TestUser / 123qwe</li>
                </ul>
            </section>

            <section className="additional-info">
                <h2 className="section-title">Технические особенности</h2>
                <ul className="features-list">
                    <li>
                        <strong>Пагинация:</strong> реализована для списков событий.
                    </li>
                    <li>
                        <strong>Кэширование изображений:</strong> при просмотре подробной информации о событии изображения
                        сохраняются в кэш браузера.
                    </li>
                    <li>
                        <strong>Middleware для обработки исключений:</strong> обрабатывает ошибки глобально и возвращает
                        их в формате JSON.
                    </li>
                </ul>
                <p className="check-feature">
                    Вы можете проверить Middleware, отправив запрос на{" "}
                    <code>https://localhost:7028/api/events/throw</code> (или адрес вашего сервера).
                </p>
            </section>

            <section className="personal-note">
                <h2 className="section-title">Примечание от разработчика</h2>
                <p className="developer-note">
                    Я стремлюсь стать backend-разработчиком, поэтому визуальная часть может быть выполнена
                    не идеально. Тем не менее, я активно развиваюсь и стараюсь улучшать свои навыки!
                </p>
            </section>

            <footer className="about-footer">
                <h2>Контакты</h2>
                <p>
                    <strong>Разработчик:</strong> Сапроненко Вячеслав
                </p>
                <p>
                    <strong>Telegram:</strong>{" "}
                    <a href="https://t.me/ssscasp" target="_blank" rel="noopener noreferrer">
                        @ssscasp
                    </a>
                </p>
                <p>
                    <strong>Email:</strong>{" "}
                    <a href="mailto:tlpc2m@gmail.com">tlpc2m@gmail.com</a> /{" "}
                    <a href="mailto:dinsidemh@gmail.com">dinsidemh@gmail.com</a>
                </p>
                <p>
                    <strong>GitHub:</strong>{" "}
                    <a href="https://github.com/casplaer" target="_blank" rel="noopener noreferrer">
                        github.com/casplaer
                    </a>
                </p>
            </footer>
        </div>
    );
};

export default AboutPage;
