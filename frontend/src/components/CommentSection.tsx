import React, {useState} from "react"
import {v4 as uuid4} from "uuid"
import {useAddCommentMutation, useGetCommentsQuery} from "../services/CommentsApi"
import {SingleComment} from "./SingleComment"
import {Comment, CommentRequest} from "../utils/Comment"

interface Props {
    hotelId: string
    accessToken: string
    email: string | undefined
}

export const CommentSection: React.FC<Props> = ({hotelId, accessToken, email}) => {
    const [comment, setComment] = useState<string>("")

    const {
        data: getCommentData,
        isFetching: isGetCommentFetching,
        isSuccess: isGetCommentSuccess,
        isError: isGetCommentError,
    } = useGetCommentsQuery({accessToken, hotelId}, {
        skip: accessToken === ""
    })

    const [addComment] = useAddCommentMutation()

    const commentHotel = async (comment: string) => {
        if (email !== undefined) {
            const fullComment: CommentRequest = {
                userEmail: email,
                hotelId: hotelId,
                description: comment,
            }
            await addComment({
                comment: fullComment,
                accessToken: accessToken,
            })
        }
    }

    if (isGetCommentFetching) {
        return <div>Loading...</div>
    } else if (getCommentData === undefined) {
        return <p>No comments found</p>
    } else {
        const comments = getCommentData.results

        const allComments = comments.map((comment) => (
            <div key={uuid4()}>
                <SingleComment comment={comment}/>
            </div>
        ))

        return <div className="py-4">
            <h2 className="text-2xl font-bold">Comments</h2>
            {email !== undefined &&
                <div className="flex flex-row">
                    <input className="w-full py-2 px-4 my-2 bg-custom-blue-700 drop-shadow-lg rounded-2xl"
                           placeholder="Add a comment"
                           onChange={(e) => setComment(() => e.target.value)}/>
                    <button className="my-2 ml-2" onClick={() => commentHotel(comment)}>Comment</button>
                </div>}
            {allComments}
        </div>
    }
}
