export type Comment = {
    id: string,
    description: string,
    userId: string,
    hotelId: string,
}

export type CommentRequest = {
    description: string,
    userId: string,
    hotelId: string,
}