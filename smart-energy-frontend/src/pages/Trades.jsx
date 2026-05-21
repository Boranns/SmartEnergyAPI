import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { tradeService } from '../services/api'
import './Trades.css'

function Trades() {
  const navigate = useNavigate()
  const [trades, setTrades] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchTrades = async () => {
      try {
        const res = await tradeService.getMyTrades()
        setTrades(res.data)
      } catch {
        console.log('Handler kunne ikke hentes')
      }
      setLoading(false)
    }
    fetchTrades()
  }, [])

  if (loading) return <div className="loading">Indlæser...</div>

  return (
    <div className="trades-page">
      <nav className="navbar">
        <h1 className="logo" onClick={() => navigate('/dashboard')}>SmartEnergy</h1>
        <button onClick={() => navigate('/dashboard')}>← Tilbage</button>
      </nav>

      <div className="trades-content">
        <h2>Mine handler</h2>
        <p className="subtitle">Din handelshistorik</p>

        {trades.length === 0 ? (
          <div className="empty">
            <p>Du har ingen handler endnu.</p>
            <button onClick={() => navigate('/energy-products')}>
              🔋 Start med at handle
            </button>
          </div>
        ) : (
          <div className="trades-table">
            <div className="table-header">
              <span>Produkt</span>
              <span>Type</span>
              <span>Mængde</span>
              <span>Pris</span>
              <span>Total</span>
              <span>Dato</span>
            </div>
            {trades.map((trade) => (
              <div key={trade.id} className="table-row">
                <span>{trade.energyProduct?.name}</span>
                <span className={trade.type === 'Buy' ? 'buy' : 'sell'}>
                  {trade.type === 'Buy' ? '📈 Køb' : '📉 Sælg'}
                </span>
                <span>{trade.quantity} MWh</span>
                <span>{trade.price?.toFixed(2)} DKK</span>
                <span>{trade.total?.toFixed(2)} DKK</span>
                <span>{new Date(trade.executedAt).toLocaleDateString('da-DK')}</span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

export default Trades