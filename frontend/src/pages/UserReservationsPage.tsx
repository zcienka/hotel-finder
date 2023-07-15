import Navbar from "../components/Navbar"
import {useGetReservationsByUserQuery} from "../services/ReservationApi"
import {useAuth0} from "@auth0/auth0-react"
import React, {useEffect, useState} from "react"
import Loading from "../components/Loading"
import {ErrorPage} from "./ErrorPage"
import {v4 as uuid4} from "uuid"
import {useNavigate} from "react-router-dom"
import {Helmet} from "react-helmet"
import {useGetCommentsByUserQuery} from "../services/CommentsApi"
import {Comment} from "../utils/Comment"

export const UserReservationsPage = () => {
    const {user, isAuthenticated, getAccessTokenSilently} = useAuth0()
    const [accessToken, setAccessToken] = useState("")
    const [userEmail, setUserEmail] = useState("")

    const navigate = useNavigate()

    const getAccessToken = async () => {
        if (isAuthenticated) {
            const token = await getAccessTokenSilently()
            setAccessToken(token)
            if (user?.email !== undefined) {
                setUserEmail(user.email)
            }
        }
    }

    useEffect(() => {
        getAccessToken()
    }, [getAccessToken])

    const {
        data: getCommentsByUser,
    } = useGetCommentsByUserQuery({accessToken, userEmail}, {skip: userEmail === ""})

    const {
        data: getReservationsByUser,
        isError: isGetReservationsByUserError,
    } = useGetReservationsByUserQuery({accessToken, userEmail},
        {
            skip: userEmail === "" || accessToken === ""
        })

    if (!isAuthenticated) {
        navigate("/")
        return <Loading/>
    } else if (isGetReservationsByUserError) {
        return <ErrorPage/>
    } else if (getReservationsByUser === undefined || getCommentsByUser === undefined) {
        return <Loading/>
    } else {
        const userReservations = getReservationsByUser.results

        const userReservationsList = userReservations.map((reservation) => {
            const checkInDate = new Date(reservation.checkInDate).toISOString().split("T")[0]
            const checkOutDate = new Date(reservation.checkOutDate).toISOString().split("T")[0]

            return <div key={uuid4()} className="w-256 bg-custom-blue-700 rounded-xl my-2 shadow-lg p-4">
                <p className="text-3xl font-bold mb-2">{reservation.hotel.name}</p>
                <p className="text-gray-200 font-semibold">Room number: {reservation.roomId}</p>
                <p className="font-semibold text-gray-400">Check-in date: {checkInDate}</p>
                <p className="font-semibold text-gray-400">Check-out date: {checkOutDate}</p>
            </div>
        })

        const userComments = getCommentsByUser.results
        const userCommentsList = userComments.map((comment: Comment) => {
            return <div key={uuid4()} className="w-256 bg-custom-blue-700 rounded-xl my-2 shadow-lg p-4">
                <p className="text-2xl font-bold mb-2">{comment.description}</p>
                <p className="text-gray-400 font-semibold">{comment.userEmail}</p>
            </div>
        })

        return <>
            <Helmet>
                <title>User details</title>
            </Helmet>
            <Navbar/>
            <div className="flex items-center flex-col">
                <div className="flex items-center justify-center mt-8">
                    <div className="flex flex-col items-center">
                        <p className="text-4xl font-bold">Hello {user?.name}</p>
                        <p className="text-gray-400 font-semibold">{user?.email}</p>
                    </div>
                </div>
                <h2 className="w-256 justify-left text-3xl font-bold">Your reservations</h2>
                {userReservationsList}
                <h2 className="w-256 justify-left text-3xl font-bold">Your comments</h2>
                {userCommentsList}
            </div>
        </>
    }
}
