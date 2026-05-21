import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { energyService, tradeService } from '../services/api'
import './EnergyProducts.css'

function EnergyProducts() {
  const navigate = useNavigate()
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [selectedProduct, setSelectedProduct] = useState(null)
  const [quantity, setQuantity] = useState(1)
  const [tradeType, setTradeType] = useState('Buy')
  const [message, setMessage] = useState('')

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const res = await energyService.getAll()
        setProducts(res.data)
      } catch {
        console.log('Produkter kunne ikke hentes')
      }
      setLoading(false)
    }
    fetchProducts()
  }, [])

  const handleTrade = async () => {
    if (!selectedProduct) return
    try {
      await tradeService.executeTrade({
        energyProductId: selectedProduct.id,
        type: tradeType,
        quantity: parseFloat(quantity)
      })
      setMessage(`✅ ${tradeType === 'Buy' ? 'Køb' : 'Salg'} gennemført!`)
      setSelectedProduct(null)
    } catch {
      setMessage('❌ Handlen mislykkedes. Tjek din saldo.')
    }
  }

  if (loading) return <div className="loading">Indlæser...</div>

  return (
    <div className="energy-page">
      <nav className="navbar">
        <h1 className="logo" onClick={() => navigate('/dashboard')}>SmartEnergy</h1>
        <button onClick={() => navigate('/dashboard')}>← Tilbage</button>
      </nav>

      <div className="energy-content">
        <h2>Energiprodukter</h2>
        <p className="subtitle">Klik på et produkt for at handle</p>

        {message && (
          <div className={`message ${message.includes('✅') ? 'success' : 'error'}`}>
            {message}
          </div>
        )}

        <div className="products-grid">
          {products.map((product) => (
            <div
              key={product.id}
              className={`product-card ${selectedProduct?.id === product.id ? 'selected' : ''}`}
              onClick={() => setSelectedProduct(product)}
            >
              <div className="product-icon">
                {product.symbol === 'SOL' && '☀️'}
                {product.symbol === 'WIN' && '💨'}
                {product.symbol === 'GAS' && '🔥'}
                {product.symbol === 'NUC' && '⚛️'}
              </div>
              <h3>{product.name}</h3>
              <p className="symbol">{product.symbol}</p>
              <p className="price">{product.currentPrice?.toFixed(2)} DKK/MWh</p>
              <p className={product.currentPrice > product.previousPrice ? 'up' : 'down'}>
                {product.currentPrice > product.previousPrice ? '▲' : '▼'}
                {Math.abs(product.currentPrice - product.previousPrice).toFixed(2)} DKK
              </p>
            </div>
          ))}
        </div>

        {selectedProduct && (
          <div className="trade-panel">
            <h3>Handle {selectedProduct.name}</h3>
            <p>Pris: {selectedProduct.currentPrice?.toFixed(2)} DKK/MWh</p>

            <div className="trade-type">
              <button
                className={tradeType === 'Buy' ? 'active buy' : 'buy'}
                onClick={() => setTradeType('Buy')}
              >
                Køb
              </button>
              <button
                className={tradeType === 'Sell' ? 'active sell' : 'sell'}
                onClick={() => setTradeType('Sell')}
              >
                Sælg
              </button>
            </div>

            <input
              type="number"
              min="0.1"
              step="0.1"
              value={quantity}
              onChange={(e) => setQuantity(e.target.value)}
              placeholder="Mængde (MWh)"
            />

            <p className="total">
              Total: {(quantity * selectedProduct.currentPrice).toFixed(2)} DKK
            </p>

            <button className="confirm-btn" onClick={handleTrade}>
              Bekræft {tradeType === 'Buy' ? 'køb' : 'salg'}
            </button>
            <button className="cancel-btn" onClick={() => setSelectedProduct(null)}>
              Annuller
            </button>
          </div>
        )}
      </div>
    </div>
  )
}

export default EnergyProducts