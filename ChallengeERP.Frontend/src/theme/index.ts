import { createTheme } from '@mui/material'

export const appTheme = createTheme({
  palette: {
    primary: { main: '#b84b32' },
    background: { default: '#fffaf4', paper: '#ffffff' },
  },
  typography: {
    fontFamily: 'DM Sans, ui-sans-serif, system-ui, sans-serif',
    h1: { fontFamily: 'Fraunces, Georgia, serif', fontWeight: 400 },
    h2: { fontFamily: 'Fraunces, Georgia, serif', fontWeight: 400 },
  },
  shape: { borderRadius: 4 },
})
