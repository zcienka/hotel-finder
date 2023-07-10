import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {ApiList} from "../utils/ApiList"
import {Reservation, ReservationRequest} from "../utils/Reservation"

export const reservationApi = createApi({
    reducerPath: "reservationApi",
    baseQuery: baseQuery,
    tagTypes: ["Reservation"],
    endpoints: (builder) => ({
        getReservationsByUser: builder.query<ApiList<Reservation>, { hotelId: string }>({
            query: ({hotelId}) => ({
                url: `/reservations/hotel/${hotelId}`,
                method: "GET",
            }),
            providesTags: ["Reservation"],
        }),
        createReservation: builder.mutation<Reservation, {reservation: ReservationRequest, accessToken: string}>({
            query: (body) => ({
                url: "/reservations",
                method: "POST",
                body: body.reservation,
                headers: {authorization: `Bearer ${body.accessToken}`},
            }),
            invalidatesTags: ["Reservation"],
        }),
    })
})

export const {
    useGetReservationsByUserQuery,
    useCreateReservationMutation,
} = reservationApi
