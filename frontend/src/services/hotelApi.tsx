import {createApi} from '@reduxjs/toolkit/query/react'
import baseQuery from '../utils/baseQuery'
import {Hotel} from '../utils/Hotel'
import {ApiList} from "../utils/ApiList";

export type RoomRequest = {
    roomId: string,
    token: string,
    url: string,
}
export const hotelApi = createApi({
    reducerPath: 'hotelApi',
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        // getHotels: builder.query<Hotel[], string>({
        //     query: () => ({
        //         url: '/hotels',
        //         method: 'GET',
        //         headers: {authorization: `Bearer `},
        //     }),
        // })
        getHotels: builder.query<ApiList<Hotel>, string >({
            query: (accessToken: string) => ({
                url: '/hotels',
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
        }),
    })
})

export const {useGetHotelsQuery} = hotelApi
