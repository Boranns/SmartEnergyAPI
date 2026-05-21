import { createContext, useContext, useState, useEffect } from 'react'
import { authService } from '../services/api'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
  const token = localStorage.getItem('accessToken')
  const username = localStorage.getItem('username')
  const role = localStorage.getItem('role')

  if (token && username) {
    setUser({ username, role })
  }
  setLoading(false)
    }, [])

  const login = async (data) => {
    const response = await authService.login(data)
    const { accessToken, refreshToken, username, role } = response.data

    localStorage.setItem('accessToken', accessToken)
    localStorage.setItem('refreshToken', refreshToken)
    localStorage.setItem('username', username)
    localStorage.setItem('role', role)

    setUser({ username, role })
  }

  const register = async (data) => {
    const response = await authService.register(data)
    const { accessToken, refreshToken, username, role } = response.data

    localStorage.setItem('accessToken', accessToken)
    localStorage.setItem('refreshToken', refreshToken)
    localStorage.setItem('username', username)
    localStorage.setItem('role', role)

    setUser({ username, role })
  }

  const logout = () => {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('username')
    localStorage.removeItem('role')
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, login, register, logout, loading }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}