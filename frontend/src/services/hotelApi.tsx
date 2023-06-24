import {createApi} from '@reduxjs/toolkit/query/react'
import baseQuery from '../utils/baseQuery'
import {Hotel} from '../utils/Hotel'

export const hotelApi = createApi({
    reducerPath: 'hotelApi',
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getHotels: builder.query<Hotel[], string>({
            query: (token) => {
                return {
                    url: '/hotels',
                    method: 'GET',
                    headers: {authorization: `Bearer ${token}`},
                }
            },
        })
    })
})

export const {useGetHotelsQuery} = hotelApi
