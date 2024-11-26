import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import apiClient from "../api/apiClient";
import "../css/EnrolledUsersPage.css";

const EnrolledUsersPage = () => {
    const { id } = useParams(); 
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchEnrolledUsers = async () => {
            try {
                const response = await apiClient.get(`/enrollments/users-enrolled/${id}`);
                console.log(response.data);
                setUsers(response.data);
            } catch (err) {
                console.error("Ошибка при загрузке пользователей:", err.response?.data || err.message);
                setError("Не удалось загрузить пользователей. Попробуйте позже.");
            } finally {
                setLoading(false);
            }
        };

        fetchEnrolledUsers();
    }, [id]);

    if (loading) return <p>Загрузка пользователей...</p>;
    if (error) return <p className="error-message">{error}</p>;

    return (
<div className="enrolled-users-page">
    <h1>Пользователи, зарегистрированные на событие</h1>
    {users.length === 0 ? (
    <p>На это событие еще никто не зарегистрировался.</p>
    ) : (
    <div className="users-list">
        {users.map((user) => (
        <div key={user.id} className="user-card">
            <h3>{user.name} {user.surname}</h3>
            <p>Email: {user.email}</p>
            <p>Дата рождения: {user.dateOfBirth}</p>
        </div>
        ))}
    </div>
    )}
</div>
    );
};

export default EnrolledUsersPage;
