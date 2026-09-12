import type { Order } from '../types/api'

export async function getOrder(orderId: string): Promise<Order> {
  const response = await fetch(`/ordenes/${orderId}`)

  if (!response.ok) {
    throw new Error('No se pudo cargar la orden.')
  }

  return await response.json() as Order
}
