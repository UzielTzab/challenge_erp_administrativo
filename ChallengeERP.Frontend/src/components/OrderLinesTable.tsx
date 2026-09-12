import { Box, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from '@mui/material'
import type { Order } from '../types/api'

type OrderLinesTableProps = {
  order: Order
  quantities: Record<number, string>
  submitting: boolean
  onQuantityChange: (lineId: number, value: string) => void
}

export function OrderLinesTable({ order, quantities, submitting, onQuantityChange }: OrderLinesTableProps) {
  return (
    <Box>
      <TableContainer component={Box} sx={{ overflowX: 'auto' }}>
        <Table sx={{ minWidth: 820 }}>
          <TableHead>
            <TableRow>
              <TableCell>Artículo</TableCell>
              <TableCell>Unidad</TableCell>
              <TableCell align="right">Ordenado</TableCell>
              <TableCell align="right">Recibido</TableCell>
              <TableCell align="right">Pendiente</TableCell>
              <TableCell align="right">Máximo</TableCell>
              <TableCell align="right">Recibir ahora</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {order.lines.map((line) => (
              <TableRow key={line.id}>
                <TableCell>
                  <Typography sx={{ fontWeight: 700 }}>{line.article}</Typography>
                  <Typography color="text.secondary" variant="caption">Línea {line.id}</Typography>
                </TableCell>
                <TableCell>{line.unitOfMeasure}</TableCell>
                <TableCell align="right">{line.orderedQuantity.toFixed(2)}</TableCell>
                <TableCell align="right">{line.receivedQuantity.toFixed(2)}</TableCell>
                <TableCell align="right"><Typography color="primary" sx={{ fontWeight: 700 }}>{line.pendingQuantity.toFixed(2)}</Typography></TableCell>
                <TableCell align="right">{line.maximumAcceptable.toFixed(2)}</TableCell>
                <TableCell align="right">
                  <TextField
                    type="number"
                    size="small"
                    value={quantities[line.id] ?? '0'}
                    onChange={(event) => onQuantityChange(line.id, event.target.value)}
                    disabled={order.status !== 'Open' || submitting}
                    slotProps={{ htmlInput: { step: 0.01, 'aria-label': `Cantidad a recibir de ${line.article}` } }}
                    placeholder="0.00"
                    sx={{ width: 120 }}
                  />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}
