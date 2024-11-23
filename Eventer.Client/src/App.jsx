import { useState } from 'react'
import './App.css'
import EventsPage from './pages/EventsPage'
import Sidebar from './components/Sidebar'
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import { useNavigate, BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from './components/AuthContext';

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
                            <Route path="/events" element={<EventsPage />} />
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/register" element={<RegisterPage />} />
                        </Routes>
                    </div>
                </div>
            </AuthWrapper>
        </Router>
    )
}

export default App
