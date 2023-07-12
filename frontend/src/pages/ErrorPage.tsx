import {useNavigate} from "react-router-dom"

export const ErrorPage = () => {
    const navigate = useNavigate()

    return <div className="flex flex-col items-center justify-center h-screen">
            <h1 className="text-8xl font-bold custom-blue-700 mb-4">Oops!</h1>
            <p className="text-3xl text-gray-400 mb-8">Something went wrong.</p>
            <button
                className="text-2xl px-6 py-6 rounded-3xl" onClick={() => navigate("/")}>
                Go back to homepage
            </button>
        </div>
}