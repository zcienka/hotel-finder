import {configureStore} from '@reduxjs/toolkit'
import {hotelApi} from "../services/hotelApi"
import {authApi} from "../services/authApi"
import {setupListeners} from "@reduxjs/toolkit/query"

const store = configureStore({
    reducer: {
        [hotelApi.reducerPath]: hotelApi.reducer,
        [authApi.reducerPath]: authApi.reducer,
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(hotelApi.middleware, authApi.middleware)

})

setupListeners(store.dispatch)

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export default store