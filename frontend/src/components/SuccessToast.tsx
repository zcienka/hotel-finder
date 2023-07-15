import React from "react"
import {Toast} from "react-hot-toast"

interface Props {
    t: Toast
    message: string
}

export const SuccessToast = ({t, message}: Props) => {
    return <div
        className={`${
            t.visible  ? "animate-enter" : "animate-leave"
        } bg-custom-blue-700 px-4 rounded-xl py-4 flex w-128`}
    >
        <div
            className="inline-flex items-center justify-center flex-shrink-0 w-8 h-8 text-green-200 bg-green-500 rounded-lg">
            <svg className="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                 viewBox="0 0 20 20">
                <path
                    d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"/>
            </svg>
        </div>
        <div className="flex items-center">
            <p className="flex items-center mx-2 text-xl">{message}</p>
        </div>
    </div>
}
