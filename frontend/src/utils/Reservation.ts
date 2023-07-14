import {Hotel} from "./Hotel"

export type Reservation = {
    id: string,
    checkInDate: string,
    checkOutDate: string,
    roomId: string,
    userEmail: string,
    hotel: Hotel
}

export type ReservationRequest = {
    checkInDate: string,
    checkOutDate: string,
    hotelId: string,
    roomId: string,
    userEmail: string,
}