import React from "react"
import {ReactComponent as LoadingSpinner} from "../icons/loadingSpinner.svg"

const Loading = () => {
    return <div className="flex h-screen w-screen">
        <div className="h-screen w-screen">
            <div role="status" className="flex h-screen w-screen justify-center items-center">
                <LoadingSpinner/>
            </div>
        </div>
    </div>
}

export default Loading