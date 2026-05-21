import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Dashboard from './pages/Dashboard'
import EnergyProducts from './pages/EnergyProducts'
import Trades from './pages/Trades'
import AdminPanel from './pages/AdminPanel'
import Home from './pages/Home'

function PrivateRoute({ children }) {
  const { user, loading } = useAuth()
  if (loading) return <div>Indlæser...</div>
  return user ? children : <Navigate to="/login" />
}

function AdminRoute({ children }) {
  const { user, loading } = useAuth()
  if (loading) return <div>Indlæser...</div>
  return user?.role === 'Admin' ? children : <Navigate to="/dashboard" />
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/energy-products" element={<PrivateRoute><EnergyProducts /></PrivateRoute>} />
          <Route path="/trades" element={<PrivateRoute><Trades /></PrivateRoute>} />
          <Route path="/admin" element={<AdminRoute><AdminPanel /></AdminRoute>} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App