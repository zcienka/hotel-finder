export type UserRequest = {
    username: string,
    token: string,
    url: string
}

export type UserRooms = {
    usernames: string[],
    id: string,
}

export type CreateRoomResponse = {
    id: string,
    usernames: string[],
    lastMessage: string,
}

export type AuthRequest = {
    username: string,
    password: string,
}

export type AuthResponse = {
    success: string,
    token: string,
}

export type DeleteAccountRequest = {
    username: string,
    token: string,
}