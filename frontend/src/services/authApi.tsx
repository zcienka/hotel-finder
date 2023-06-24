import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/baseQuery"
import {AuthRequest, AuthResponse, DeleteAccountRequest} from "../utils/User"

export const authApi = createApi({
    reducerPath: "authApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        loginUser: builder.mutation<AuthResponse, AuthRequest>({
            query: (body) => {
                return {
                    url: "/login",
                    method: "POST",
                    body,
                }
            },
        }),
        registerUser: builder.mutation<AuthResponse, AuthRequest>({
            query: (body) => {
                return {
                    url: "/register",
                    method: "POST",
                    body,
                }
            },
        }),
        deleteAccount: builder.mutation<DeleteAccountRequest, any>({
            query: (body) => {
                return {
                    url: `/user/${body.username}`,
                    method: "DELETE",
                    headers: {authorization: `Bearer ${body.token}`},
                }
            },
        }),
    }),
})

export const {
    useLoginUserMutation,
    useRegisterUserMutation,
    useDeleteAccountMutation,
} = authApi
