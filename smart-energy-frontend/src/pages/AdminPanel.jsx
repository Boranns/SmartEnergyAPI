import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { adminService } from '../services/api'
import './AdminPanel.css'

function AdminPanel() {
  const navigate = useNavigate()
  const [stats, setStats] = useState(null)
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [statsRes, usersRes] = await Promise.all([
          adminService.getDashboardStats(),
          adminService.getAllUsers()
        ])
        setStats(statsRes.data)
        setUsers(usersRes.data)
      } catch {
        console.log('Data kunne ikke hentes')
      }
      setLoading(false)
    }
    fetchData()
  }, [])

  const handleToggleStatus = async (id) => {
    try {
      await adminService.toggleUserStatus(id)
      const res = await adminService.getAllUsers()
      setUsers(res.data)
    } catch {
      console.log('Status kunne ikke ændres')
    }
  }

  if (loading) return <div className="loading">Indlæser...</div>

  return (
    <div className="admin-page">
      <nav className="navbar">
        <h1 className="logo" onClick={() => navigate('/dashboard')}>SmartEnergy</h1>
        <button onClick={() => navigate('/dashboard')}>← Tilbage</button>
      </nav>

      <div className="admin-content">
        <h2>Admin Panel</h2>

        {stats && (
          <div className="stats-grid">
            <div className="stat-card">
              <h3>Brugere i alt</h3>
              <p className="stat-value">{stats.totalUsers}</p>
            </div>
            <div className="stat-card">
              <h3>Aktive brugere</h3>
              <p className="stat-value">{stats.activeUsers}</p>
            </div>
            <div className="stat-card">
              <h3>Handler i alt</h3>
              <p className="stat-value">{stats.totalTrades}</p>
            </div>
            <div className="stat-card">
              <h3>Samlet volumen</h3>
              <p className="stat-value">{stats.totalVolume?.toFixed(2)} DKK</p>
            </div>
          </div>
        )}

        <h3 className="section-title">Brugere</h3>
        <div className="users-table">
          <div className="table-header">
            <span>ID</span>
            <span>Brugernavn</span>
            <span>E-mail</span>
            <span>Rolle</span>
            <span>Saldo</span>
            <span>Status</span>
            <span>Handling</span>
          </div>
          {users.map((user) => (
            <div key={user.id} className="table-row">
              <span>{user.id}</span>
              <span>{user.username}</span>
              <span>{user.email}</span>
              <span className={user.role === 'Admin' ? 'admin-role' : 'trader-role'}>
                {user.role}
              </span>
              <span>{user.balance?.toFixed(2)} DKK</span>
              <span className={user.isActive ? 'active' : 'inactive'}>
                {user.isActive ? '✅ Aktiv' : '❌ Inaktiv'}
              </span>
              <button
                className={user.isActive ? 'deactivate-btn' : 'activate-btn'}
                onClick={() => handleToggleStatus(user.id)}
              >
                {user.isActive ? 'Deaktiver' : 'Aktiver'}
              </button>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}

export default AdminPanel