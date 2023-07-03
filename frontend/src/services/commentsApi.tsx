import {createApi} from "@reduxjs/toolkit/query/react"
import baseQuery from "../utils/baseQuery"
import {Comment} from "../utils/Comment"
import {ApiList} from "../utils/ApiList";

export const commentsApi = createApi({
    reducerPath: "CommentApi",
    baseQuery: baseQuery,
    endpoints: (builder) => ({
        getComments: builder.query<ApiList<Comment>, string>({
            query: (accessToken: string) => ({
                url: "/comments",
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
        }),
        getSingleComment: builder.query<Comment, { accessToken: string, commentId: string }>({
            query: ({accessToken, commentId}) => ({
                url: `/comments/${commentId}`,
                method: "GET",
                headers: {authorization: `Bearer ${accessToken}`},
            }),
        }),
    })
})

export const {
    useGetCommentsQuery,
    useGetSingleCommentQuery
} = commentsApi
