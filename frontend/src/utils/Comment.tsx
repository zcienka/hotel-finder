export type Comment = {
    id: string,
    description: string,
    userEmail: string,
    hotelId: string,
}

export type CommentRequest = {
    description: string,
    userEmail: string,
    hotelId: string,
}