import {createSlice, PayloadAction} from "@reduxjs/toolkit"
import {TokenAuth} from "../utils/token"

const initialState: TokenAuth = {
    token: null,
}

export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setUser: (
            state,
            action: PayloadAction<{ token: string, success: string }>
        ) => {
        },
        logout: (state) => {
        },
        setCredentials: (
            state,
            {payload: {token}}: PayloadAction<{ token: string }>
        ) => {
        },
    },
})

export const {setUser, logout} = authSlice.actions

export default authSlice.reducer
