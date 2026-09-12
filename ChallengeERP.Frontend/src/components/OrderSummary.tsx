import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined'
import LockOutlinedIcon from '@mui/icons-material/LockOutlined'
import { Box, Typography } from '@mui/material'
import type { Order } from '../types/api'

type OrderSummaryProps = {
  order: Order
}

export function OrderSummary({ order }: OrderSummaryProps) {
  const isOpen = order.status === 'Open'

  return (
    <Box sx={{ width: '100%', boxSizing: 'border-box', p: { xs: 2, md: 3 }, backgroundColor: 'transparent', borderBottom: '1px solid #d8d0c5' }}>
      <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: { xs: 2, sm: 5 }, alignItems: { xs: 'flex-start', sm: 'center' } }}>
        <Box>
          <Typography color="text.secondary" variant="caption">Orden</Typography>
          <Typography sx={{ fontWeight: 700 }}>{order.id}</Typography>
        </Box>
        <Box>
          <Typography color="text.secondary" variant="caption">Proveedor</Typography>
          <Typography sx={{ fontWeight: 700 }}>{order.provider}</Typography>
        </Box>
        <Box sx={{ ml: { sm: 'auto' } }}>
          <Typography color="text.secondary" variant="caption" sx={{ display: 'block' }}>Estado</Typography>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.75 }}>
            {isOpen
              ? <CheckCircleOutlinedIcon color="success" fontSize="small" aria-hidden="true" />
              : <LockOutlinedIcon color="disabled" fontSize="small" aria-hidden="true" />}
            <Typography sx={{ color: isOpen ? '#315a39' : 'text.primary', fontWeight: 700 }}>
              {isOpen ? 'Abierta' : 'Cerrada'}
            </Typography>
          </Box>
        </Box>
      </Box>
    </Box>
  )
}
