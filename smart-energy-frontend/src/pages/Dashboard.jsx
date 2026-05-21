import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import { portfolioService } from '../services/api'
import { useNavigate } from 'react-router-dom'
import './Dashboard.css'

function Dashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [totalValue, setTotalValue] = useState(0)
  const [portfolio, setPortfolio] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchData = async () => {
      if (!user) {
        setLoading(false)
        return
      }
      try {
        const [valueRes, portfolioRes] = await Promise.all([
          portfolioService.getTotalValue(),
          portfolioService.getMyPortfolio()
        ])
        setTotalValue(valueRes.data.totalValue)
        setPortfolio(portfolioRes.data)
      } catch {
        console.log('Data kunde ikke hentes')
      }
      setLoading(false)
    }
    fetchData()
  }, [user])

  const handleLogout = () => {
    logout()
    navigate('/dashboard')
  }

  if (loading) return <div className="loading">Indlæser...</div>

  return (
    <div className="dashboard">
      <nav className="navbar">
        <h1 className="logo">SmartEnergy</h1>
        <div className="nav-links">
          {user ? (
            <>
              <button onClick={() => navigate('/energy-products')}>Energiprodukter</button>
              <button onClick={() => navigate('/trades')}>Handler</button>
              {user?.role === 'Admin' && (
                <button onClick={() => navigate('/admin')}>Admin</button>
              )}
              <button className="logout-btn" onClick={handleLogout}>Log ud</button>
            </>
          ) : (
            <>
              <button onClick={() => navigate('/login')}>Log ind</button>
              <button className="register-btn" onClick={() => navigate('/register')}>Opret dig</button>
            </>
          )}
        </div>
      </nav>

      <div className="dashboard-content">
        {user ? (
          <>
            <div className="welcome">
              <h2>Velkommen, {user?.username}!</h2>
              <p>Her er din porteføljeoversigt</p>
            </div>

            <div className="stats-grid">
              <div className="stat-card" style={{background: '#1a1a2e', border: '1px solid rgba(255,255,255,0.15)'}}>
                <h3>Porteføljeværdi</h3>
                <p className="stat-value">{totalValue.toFixed(2)} DKK</p>
              </div>
              <div className="stat-card" style={{background: '#1a1a2e', border: '1px solid rgba(255,255,255,0.15)'}}>
                <h3>Antal positioner</h3>
                <p className="stat-value">{portfolio.length}</p>
              </div>
              <div className="stat-card" style={{background: '#1a1a2e', border: '1px solid rgba(255,255,255,0.15)'}}>
                <h3>Rolle</h3>
                <p className="stat-value">{user?.role}</p>
              </div>
            </div>

            {portfolio.length > 0 && (
              <div className="portfolio-section">
                <h3>Mine positioner</h3>
                <div className="portfolio-grid">
                  {portfolio.map((item) => (
                    <div key={item.id} className="portfolio-card">
                      <h4>{item.energyProduct?.name}</h4>
                      <p>Mængde: {item.quantity} MWh</p>
                      <p>Gns. købspris: {item.averageBuyPrice?.toFixed(2)} DKK</p>
                      <p>Nuværende pris: {item.energyProduct?.currentPrice?.toFixed(2)} DKK</p>
                      <p className={item.energyProduct?.currentPrice > item.averageBuyPrice ? 'profit' : 'loss'}>
                        {item.energyProduct?.currentPrice > item.averageBuyPrice ? '📈 Gevinst' : '📉 Tab'}
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            )}

            <div className="quick-actions">
              <h3>Hurtige handlinger</h3>
              <button onClick={() => navigate('/energy-products')}>
               Se energiprodukter
              </button>
              <button onClick={() => navigate('/trades')}>
               Mine handler
              </button>
            </div>
          </>
        ) : (
          <div className="guest-content">
            <h2>Fremtidens energihandel</h2>
            <p>Log ind for at se din portefølje og handle energi</p>
            <button className="cta-btn" onClick={() => navigate('/register')}>
              Kom i gang →
            </button>
          </div>
        )}
      </div>
    </div>
  )
}

export default Dashboard