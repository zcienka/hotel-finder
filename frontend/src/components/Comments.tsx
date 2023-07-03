import React, {useState} from "react"
import {v4 as uuid4} from "uuid"
import {useGetCommentsQuery} from "../services/commentsApi"
import {CommentItem} from "./CommentItem"
import Loading from "./Loading";

interface Props {
    hotelId: string,
    accessToken: string
}

export const Comments: React.FC<Props> = ({hotelId, accessToken}) => {
    const {
        data: getCommentData,
        isFetching: isGetCommentFetching,
        isSuccess: isGetCommentSuccess,
        isError: isGetCommentError,
    } = useGetCommentsQuery({accessToken, hotelId}, {
        skip: accessToken === ''
    })

    if (isGetCommentFetching) {
        return <div>Loading...</div>
    } else if (getCommentData === undefined) {
        return <>No comments found</>
    } else {
        const comments = getCommentData.results

        const allComments = comments.map((comment) => (
            <div key={uuid4()}>
                <CommentItem comment={comment}/>
            </div>
        ))

        return <div className="py-4">
            <h2 className="text-2xl font-bold">Comments</h2>
            <div className="flex flex-row">
                <input className="w-full py-2 px-4 my-2 bg-custom-blue-700 drop-shadow-lg rounded-2xl" placeholder="Add a comment"/>
                <button className="my-2 ml-2">Comment</button>
            </div>
            {allComments}
        </div>
    }
}
