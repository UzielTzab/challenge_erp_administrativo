export type OrderLine = {
  id: number
  article: string
  orderedQuantity: number
  receivedQuantity: number
  pendingQuantity: number
  maximumAcceptable: number
}

export type Order = {
  id: string
  provider: string
  status: string
  lines: OrderLine[]
}

export type ReceptionLine = {
  orderLineId: number
  quantity: number
}

export type ApiError = {
  message?: string
  lineId?: number
}
