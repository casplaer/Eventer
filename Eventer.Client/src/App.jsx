import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import EventsPage from './pages/EventsPage'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
          <div>
              <EventsPage />
          </div>
    </>
  )
}

export default App
