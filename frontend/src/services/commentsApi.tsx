import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {Comment, CommentRequest} from "../utils/Comment"
import {ApiList} from "../utils/ApiList";

export const commentsApi = createApi({
    reducerPath: "commentsApi",
    baseQuery: baseQuery,
    tagTypes: ["Comments", "UserComments"],
    endpoints: (builder) => ({
        getComments: builder.query<ApiList<Comment>, {accessToken: string, hotelId: string}>({
            query: ({accessToken, hotelId}) => ({
                url: `/comments/hotel/${hotelId}`,
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
            providesTags: ["Comments"],
        }),
        getCommentsByUser: builder.query<ApiList<Comment>, {accessToken: string, userEmail: string}>({
            query: ({accessToken, userEmail}) => ({
                url: `/comments/user/${userEmail}`,
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
            providesTags: ["UserComments"],
        }),
        addComment: builder.mutation<Comment, {comment: CommentRequest, accessToken: string}>({
            query: (body) => ({
                url: "/comments",
                method: "POST",
                body: body.comment,
                headers: {authorization: `Bearer ${body.accessToken}`},
            }),
            invalidatesTags: ["Comments"],
        }),
    })
})

export const {
    useGetCommentsQuery,
    useAddCommentMutation,
    useGetCommentsByUserQuery,
} = commentsApi
