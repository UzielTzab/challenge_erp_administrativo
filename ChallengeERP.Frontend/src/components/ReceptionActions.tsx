import { Box, Button } from '@mui/material'

type ReceptionActionsProps = {
  orderStatus: string
  submitting: boolean
  onSubmit: () => void
}

export function ReceptionActions({ orderStatus, submitting, onSubmit }: ReceptionActionsProps) {
  return (
    <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, justifyContent: 'end', alignItems: { xs: 'stretch', sm: 'center' }, gap: 2, borderTop: '1px solid #d8d0c5', pt: 3 }}>
      <Button
        variant="contained"
        onClick={onSubmit}
        disabled={submitting || orderStatus !== 'Open'}
        sx={{ minWidth: 190 }}
      >
        {submitting ? 'Guardando...' : 'Registrar recepción'}
      </Button>
    </Box>
  )
}
