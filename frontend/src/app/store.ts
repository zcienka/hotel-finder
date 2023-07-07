import {configureStore} from '@reduxjs/toolkit'
import {hotelApi} from "../services/HotelApi"
import {setupListeners} from "@reduxjs/toolkit/query"
import {commentsApi} from "../services/CommentsApi";
import {roomApi} from "../services/RoomApi";

const store = configureStore({
    reducer: {
        [hotelApi.reducerPath]: hotelApi.reducer,
        [commentsApi.reducerPath]: commentsApi.reducer,
        [roomApi.reducerPath]: roomApi.reducer
},
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(
            hotelApi.middleware,
            commentsApi.middleware,
            roomApi.middleware)
})

setupListeners(store.dispatch)

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export default store