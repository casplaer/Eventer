import { useState } from 'react'
import './App.css'
import EventsPage from './pages/EventsPage'
import Sidebar from './components/Sidebar'

function App() {
    const [count, setCount] = useState(0)

    return (
        <div>
            <Sidebar />
            <div className="content">
                <EventsPage />
            </div>
        </div>
    )
}

export default App
