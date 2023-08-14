import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/BaseQuery"
import {Comment, CommentRequest} from "../utils/Comment"
import {ApiList} from "../utils/ApiList";

export const commentsApi = createApi({
    reducerPath: "commentsApi",
    baseQuery: baseQuery,
    tagTypes: ["Comments", "UserComments"],
    endpoints: (builder) => ({
        getComments: builder.query<ApiList<Comment>, {hotelId: string}>({
            query: ({hotelId}) => ({
                url: `/hotels/${hotelId}/comments`,
                method: "GET",
            }),
            providesTags: ["Comments"],
        }),
        getCommentsByUser: builder.query<ApiList<Comment>, {accessToken: string, userId: string}>({
            query: ({accessToken, userId}) => ({
                url: `/comments/user/${userId}`,
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
