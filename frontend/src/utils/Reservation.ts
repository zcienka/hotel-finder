export type Reservation = {
    id: string,
    checkInDate: string,
    checkOutDate: string,
    hotelId: string,
    roomId: string,
    userEmail: string,
}

export type ReservationRequest = {
    checkInDate: string,
    checkOutDate: string,
    hotelId: string,
    roomId: string,
    userEmail: string,
}