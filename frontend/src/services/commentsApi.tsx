import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {Comment} from "../utils/Comment"
import {ApiList} from "../utils/ApiList";

export const commentsApi = createApi({
    reducerPath: "commentsApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getComments: builder.query<ApiList<Comment>, {accessToken: string, hotelId: string}>({
            query: ({accessToken, hotelId}) => ({
                url: `/comments/hotel/${hotelId}`,
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
        })
    })
})

export const {
    useGetCommentsQuery,
} = commentsApi
