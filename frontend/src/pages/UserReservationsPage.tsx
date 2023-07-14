import Navbar from "../components/Navbar"
import {useGetReservationsByUserQuery} from "../services/ReservationApi"
import {useAuth0} from "@auth0/auth0-react"
import React, {useEffect, useState} from "react"
import Loading from "../components/Loading"
import {ErrorPage} from "./ErrorPage"
import {v4 as uuid4} from "uuid"

export const UserReservationsPage = () => {
    const {user, isAuthenticated, getAccessTokenSilently} = useAuth0()

    const [accessToken, setAccessToken] = useState<string>("")
    const [userEmail, setUserEmail] = useState<string>("")

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
        data: getReservationsByUser,
        isError: isGetReservationsByUserError,
    } = useGetReservationsByUserQuery({accessToken, userEmail},
        {
            skip: userEmail === "" || accessToken === ""
        })

    if (isGetReservationsByUserError) {
        return <ErrorPage/>
    } else if (getReservationsByUser === undefined) {
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

        return <>
            <Navbar/>
            <div className="flex items-center flex-col">
                <h2 className="w-256 justify-left text-3xl font-bold">Reservations</h2>
                {userReservationsList}
            </div>
        </>
    }
}