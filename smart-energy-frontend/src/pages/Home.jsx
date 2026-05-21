import { useNavigate } from 'react-router-dom'
import './Home.css'

function Home() {
  const navigate = useNavigate()

  return (
    <div className="home">
      <nav className="home-nav">
        <h1 className="logo">SmartEnergy</h1>
        <div className="home-nav-links">
          <button onClick={() => navigate('/login')}>Log ind</button>
          <button className="register-btn" onClick={() => navigate('/register')}>Opret dig</button>
        </div>
      </nav>

      <div className="home-content">
        <h2>Fremtidens energihandel</h2>
        <p>Køb og sælg vedvarende energi i realtid</p>
        <button className="cta-btn" onClick={() => navigate('/register')}>
          Kom i gang →
        </button>
      </div>
    </div>
  )
}

export default Home