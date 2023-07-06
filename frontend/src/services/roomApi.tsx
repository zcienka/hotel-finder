import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/baseQuery"
import {ApiList} from "../utils/ApiList"
import {Room} from "../utils/Room"

export const roomApi = createApi({
    reducerPath: "roomApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getRoomsInHotel: builder.query<ApiList<Room>, { hotelId: string }>({
            query: ({hotelId}) => ({
                url: `/rooms/hotel/${hotelId}`,
                method: "GET",
            }),
        }),
    })
})

export const {
    useGetRoomsInHotelQuery,
} = roomApi
