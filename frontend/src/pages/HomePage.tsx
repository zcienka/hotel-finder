import React, {useEffect, useState} from 'react'
import {useAuth0} from '@auth0/auth0-react'
import {useGetHotelsQuery} from '../services/hotelApi'

function HomePage() {
    const {loginWithRedirect, user, isAuthenticated, getAccessTokenSilently} = useAuth0()
    const [accessToken, setAccessToken] = useState<string | ''>('')

    const {
        data: getHotelData,
        isFetching: isGetHotelFetching,
        isSuccess: isGetHotelSuccess,
        isError: isGetHotelError,
    } = useGetHotelsQuery(accessToken, {
        skip: accessToken === ''
    })

    const getAccessToken = async () => {
        const accessToken = await getAccessTokenSilently()
        setAccessToken(accessToken)
    }

    useEffect(() => {
        getAccessToken()
    }, [getAccessToken])


    if (isGetHotelFetching) {
        return <div>Fetching...</div>
    }
    if (isGetHotelError) {
        return <div>Error</div>
    }
    if (isGetHotelSuccess) {
        const hotels = getHotelData!.map((hotel) => {
            return <div>
                {hotel.name}
            </div>
        })

        return (
            <div>
                {hotels}
            </div>
        )
    }
}

export default HomePage
