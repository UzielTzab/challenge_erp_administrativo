export type Line = {
  id: number
  article: string
  unitOfMeasure: string
  orderedQuantity: number
  receivedQuantity: number
  pendingQuantity: number
  maximumAcceptable: number
}

export type Order = {
  id: string
  provider: string
  status: string
  lines: Line[]
}

export type ReceptionLine = {
  lineId: number
  quantity: number
}

export type ApiError = {
  message?: string
  lineId?: number
  pendingQuantity?: number
  maximumAcceptable?: number
}
