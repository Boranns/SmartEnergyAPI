import axios from 'axios'

const API_URL = 'https://localhost:7034/api'

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export const authService = {
  register: (data) => api.post('/Auth/register', data),
  login: (data) => api.post('/Auth/login', data),
  refresh: (token) => api.post('/Auth/refresh', token)
}

export const energyService = {
  getAll: () => api.get('/EnergyProduct'),
  getById: (id) => api.get(`/EnergyProduct/${id}`),
  seed: () => api.post('/EnergyProduct/seed'),
  updatePrices: () => api.post('/EnergyProduct/update-prices')
}

export const tradeService = {
  executeTrade: (data) => api.post('/Trade', data),
  getMyTrades: () => api.get('/Trade/my-trades'),
  getAllTrades: () => api.get('/Trade/all')
}

export const portfolioService = {
  getMyPortfolio: () => api.get('/Portfolio'),
  getTotalValue: () => api.get('/Portfolio/total-value')
}

export const adminService = {
  getAllUsers: () => api.get('/Admin/users'),
  getUserById: (id) => api.get(`/Admin/users/${id}`),
  toggleUserStatus: (id) => api.put(`/Admin/users/${id}/toggle-status`),
  getDashboardStats: () => api.get('/Admin/dashboard')
}

export default api