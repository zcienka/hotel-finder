import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {ApiList} from "../utils/ApiList"
import {Room, RoomRequest} from "../utils/Room"

export const roomApi = createApi({
    reducerPath: "roomApi",
    baseQuery: baseQuery,
    tagTypes: ["Rooms"],
    endpoints: (builder) => ({
        getRoomsInHotel: builder.query<ApiList<Room>, { hotelId: string }>({
            query: ({hotelId}) => ({
                url: `/rooms/hotel/${hotelId}`,
                method: "GET",
            }),
            providesTags: ["Rooms"],
        }),
        getSingleRoom: builder.query<Room, { roomId: string }>({
            query: ({roomId}) => ({
                url: `/rooms/${roomId}`,
                method: "GET",
            }),
        }),
    })
})

export const {
    useGetRoomsInHotelQuery,
    useGetSingleRoomQuery,
} = roomApi
