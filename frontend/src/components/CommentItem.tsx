import React from "react";
import {Comment} from "../utils/Comment";
import {useNavigate} from "react-router-dom";

interface Props {
    comment: Comment;
}

export const CommentItem: React.FC<Props> = ({comment}) => {
    return <div className="flex items-center w-full bg-custom-blue-700 rounded-2xl my-2 drop-shadow-lg p-4">
        <p className="text-gray-400 overflow-hidden max-h-24">
            <p className="text-gray-200">{comment.userEmail}</p>
            <p>{comment.description}</p>
        </p>
    </div>
}
