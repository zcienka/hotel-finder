import Navbar from "../components/Navbar"
import {useGetReservationsByUserQuery} from "../services/ReservationApi"
import {useAuth0} from "@auth0/auth0-react"
import React, {useEffect, useState} from "react"
import Loading from "../components/Loading"
import {ErrorPage} from "./ErrorPage";

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
        isFetching: isGetReservationsByUserFetching,
        isSuccess: isGetReservationsByUserSuccess,
        isError: isGetReservationsByUserError,
    } = useGetReservationsByUserQuery({accessToken, userEmail},
        {
            skip: userEmail === "" || accessToken === ""
        })

    if (isGetReservationsByUserError) {
        return <ErrorPage/>
    } else if (getReservationsByUser === undefined) {
        return <Loading/>
    }  else {
        return <>
            <Navbar/>
        </>
    }
}