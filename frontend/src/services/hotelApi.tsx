import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {Hotel} from "../utils/Hotel"
import {ApiList} from "../utils/ApiList"
import {SearchQuery} from "../utils/SearchQuery"

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
        searchHotels: builder.query<ApiList<Hotel>, SearchQuery>({
            query: (body: SearchQuery) => {
                let url = `/hotels?limit=10`;

                if (body.name) {
                    url += `&name=${body.name}`;
                }

                if (body.city) {
                    url += `&city=${body.city}`;
                }

                if (body.roomCount) {
                    url += `&roomCount=${body.roomCount}`;
                }

                if (body.checkInDate) {
                    url += `&checkInDate=${body.checkInDate}`;
                }

                if (body.checkOutDate) {
                    url += `&checkOutDate=${body.checkOutDate}`;
                }

                if (body.category !== "default" && body.category !== "") {
                    url += `&category=${body.category}`;
                }

                return {
                    url: url,
                    method: "GET",
                }
            },
        }),
    })
})

export const {
    useGetHotelsQuery,
    useGetSingleHotelQuery,
    useSearchHotelsQuery,
} = hotelApi
