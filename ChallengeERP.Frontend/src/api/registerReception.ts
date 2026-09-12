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
    if (response.status === 422 && apiError.lineId) {
      throw new Error(
        `No se pudo completar la recepción. La línea ${apiError.lineId} excede la cantidad permitida. La recepción completa fue cancelada y ninguna línea fue guardada.`,
      )
    }
    const lineDetail = apiError.lineId
      ? ` Línea ${apiError.lineId}. Pendiente actual: ${apiError.pendingQuantity?.toFixed(2) ?? 'N/D'}. Máximo aceptable: ${apiError.maximumAcceptable?.toFixed(2) ?? 'N/D'}.`
      : ''
    const detail = `${apiError.message ?? 'No se pudo registrar la recepción.'}${lineDetail}`

    throw new Error(`${response.status}. ${detail}`)
  }

  return await response.json() as Order
}
