export type Room = {
    id: string,
    capacity: number,
    name: string,
    description: string,
    price: number,
    hotelId: string,
    image: string[],
}

export type RoomRequest = {
    id: string,
    capacity: number,
    name: string,
    description: string,
    price: number,
    hotelId: string,
    image: string[],
}