import type { ApiError, Order, ReceptionLine } from '../types/api'

export async function registerReception(
  orderId: string,
  lines: ReceptionLine[],
): Promise<Order> {
  const response = await fetch(`/ordenes/${orderId}/recepciones`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ lines }),
  })

  if (!response.ok) {
    const apiError = await response.json() as ApiError
    const detail = apiError.lineId
      ? ` Línea ${apiError.lineId}: ${apiError.message ?? 'Solicitud inválida.'}`
      : apiError.message ?? 'No se pudo registrar la recepción.'

    throw new Error(`${response.status}. ${detail}`)
  }

  return await response.json() as Order
}
