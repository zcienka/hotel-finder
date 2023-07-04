import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/baseQuery"
import {Hotel} from "../utils/Hotel"
import {ApiList} from "../utils/ApiList";

export const hotelApi = createApi({
    reducerPath: "hotelApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getHotels: builder.query<ApiList<Hotel>, void>({
            query: () => ({
                url: "/hotels",
                method: "GET",
            }),
        }),

        getSingleHotel: builder.query<Hotel, { hotelId: string }>({
            query: ({hotelId}) => ({
                url: `/hotels/${hotelId}`,
                method: "GET",
            }),
        }),
    })
})

export const {
    useGetHotelsQuery,
    useGetSingleHotelQuery
} = hotelApi
