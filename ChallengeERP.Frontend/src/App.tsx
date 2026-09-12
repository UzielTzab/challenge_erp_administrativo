import { useEffect, useState } from 'react'
import {
  Alert,
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  Button,
  CircularProgress,
  Container,
  CssBaseline,
  ThemeProvider,
  Typography,
} from '@mui/material'
import ExpandMoreIcon from '@mui/icons-material/ExpandMore'
import { getOrder } from './api/getOrder'
import { registerReception } from './api/registerReception'
import { OrderLinesTable } from './components/OrderLinesTable'
import { OrderSummary } from './components/OrderSummary'
import { ReceptionActions } from './components/ReceptionActions'
import { appTheme } from './theme'
import type { Order } from './types/api'

const orderId = 'OC-1001'

function App() {
  const [order, setOrder] = useState<Order | null>(null)
  const [quantities, setQuantities] = useState<Record<number, string>>({})
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [orderExpanded, setOrderExpanded] = useState(true)

  async function loadOrder() {
    setLoading(true)
    setError('')

    try {
      setOrder(await getOrder(orderId))
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Error de conexión.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void loadOrder()
  }, [])

  function updateQuantity(lineId: number, value: string) {
    setQuantities((current) => ({ ...current, [lineId]: value }))
  }

  async function handleRegisterReception() {
    const lines = Object.entries(quantities)
      .map(([lineId, quantity]) => ({ orderLineId: Number(lineId), quantity: Number(quantity) }))
      .filter((line) => line.quantity > 0)

    if (lines.length === 0) {
      setError('Captura al menos una cantidad mayor que cero.')
      return
    }

    setSubmitting(true)
    setError('')
    setSuccess('')

    try {
      setOrder(await registerReception(orderId, lines))
      setQuantities({})
      setSuccess('Recepción registrada correctamente.')
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Error de conexión.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <ThemeProvider theme={appTheme}>
      <CssBaseline />
      <Container maxWidth="lg" sx={{ py: { xs: 3, md: 6 } }}>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 4 }}>
          <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, justifyContent: 'space-between', alignItems: { xs: 'stretch', sm: 'flex-end' }, gap: 2 }}>
            <Box>
              <Typography variant="h1" sx={{ fontSize: { xs: '2.2rem', md: '3.5rem' }, mt: 1 }}>
                Recepción contra orden de compra
              </Typography>
            </Box>
            <Button variant="outlined" onClick={() => void loadOrder()} disabled={loading}>
              Actualizar
            </Button>
          </Box>

          {loading && <CircularProgress size={28} />}
          {error && <Alert severity="error">{error}</Alert>}
          {success && <Alert severity="success">{success}</Alert>}

          {order && (
            <>
              <Accordion
                expanded={orderExpanded}
                onChange={(_, expanded) => setOrderExpanded(expanded)}
                disableGutters
                elevation={1}
                sx={{ borderRadius: 1, overflow: 'hidden' }}
              >
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls="order-detail-content"
                  id="order-detail-header"
                  sx={{
                    p: 0,
                    backgroundColor: '#f3eee7',
                    '& .MuiAccordionSummary-content': { m: 0, flexGrow: 1, width: '100%' },
                    '& .MuiAccordionSummary-expandIconWrapper': { mr: 1 },
                  }}
                >
                  <OrderSummary order={order} />
                </AccordionSummary>
                <AccordionDetails id="order-detail-content" sx={{ p: 0 }}>
                  <OrderLinesTable
                    order={order}
                    quantities={quantities}
                    submitting={submitting}
                    onQuantityChange={updateQuantity}
                  />
                  <ReceptionActions
                    orderStatus={order.status}
                    submitting={submitting}
                    onSubmit={() => void handleRegisterReception()}
                  />
                </AccordionDetails>
              </Accordion>
            </>
          )}
        </Box>
      </Container>
    </ThemeProvider>
  )
}

export default App
