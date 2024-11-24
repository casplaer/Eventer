import { useState } from 'react'
import './App.css'
import EventsPage from './pages/EventsPage'
import Sidebar from './components/Sidebar'
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import AboutPage from "./pages/AboutPage"
import { useNavigate, BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from './components/AuthContext';
import AdminPage from './pages/AdminPage';
import CreateEventPage from './pages/CreateEventPage'
import EditEventPage from './pages/EditEventPage';


const AuthWrapper = ({ children }) => {
    const navigate = useNavigate();
    return <AuthProvider navigate={navigate}>{children}</AuthProvider>;
};

function App() {
    return (
        <Router>
            <AuthWrapper>
                <div style={{ display: "flex" }}>
                    <Sidebar />
                    <div style={{ flex: 1, padding: "20px" }}>
                        <Routes>
                            <Route path="/" element={<Navigate to="/about" />} />
                            <Route path="/about" element={<AboutPage />} />
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/register" element={<RegisterPage />} />
                            <Route path="/events" element={<EventsPage />} />
                            <Route path="/admin" element={<AdminPage />} />
                            <Route path="/create_event" element={<CreateEventPage />}></Route>
                            <Route path="/edit_event/:id" element={<EditEventPage />} />
                        </Routes>
                    </div>
                </div>
            </AuthWrapper>
        </Router>
    )
}

export default App
