import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/baseQuery"
import {Hotel} from "../utils/Hotel"
import {ApiList} from "../utils/ApiList";

export const hotelApi = createApi({
    reducerPath: "hotelApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getHotels: builder.query<ApiList<Hotel>, string>({
            query: (accessToken: string) => ({
                url: "/hotels",
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
        }),
        getSingleHotel: builder.query<Hotel, { accessToken: string, hotelId: string }>({
            query: ({accessToken, hotelId}) => ({
                url: `/hotels/${hotelId}`,
                method: "GET",
                // headers: {authorization: `Bearer ${accessToken}`},
            }),
        }),
    })
})

export const {
    useGetHotelsQuery,
    useGetSingleHotelQuery
} = hotelApi
