import {configureStore} from '@reduxjs/toolkit'
import {hotelApi} from "../services/HotelApi"
import {setupListeners} from "@reduxjs/toolkit/query"
import {commentsApi} from "../services/CommentsApi";
import {roomApi} from "../services/RoomApi";
import {reservationApi} from "../services/ReservationApi";

const store = configureStore({
    reducer: {
        [hotelApi.reducerPath]: hotelApi.reducer,
        [commentsApi.reducerPath]: commentsApi.reducer,
        [roomApi.reducerPath]: roomApi.reducer,
        [reservationApi.reducerPath]: reservationApi.reducer
},
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(
            hotelApi.middleware,
            commentsApi.middleware,
            roomApi.middleware,
            reservationApi.middleware)
})

setupListeners(store.dispatch)

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export default store