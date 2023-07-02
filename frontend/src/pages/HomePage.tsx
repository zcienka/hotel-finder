import SearchBar from "../components/SearchBar";
import Navbar from "../components/Navbar";
import {useAuth0} from "@auth0/auth0-react";
import {useEffect, useState} from "react";
import {useGetHotelsQuery} from "../services/hotelApi";
import Loading from "../components/Loading";
import {Hotels} from "../components/Hotels";

export const HomePage = () => {
    const {getAccessTokenSilently, loginWithRedirect} = useAuth0()
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
        try {
            const token = await getAccessTokenSilently();
            setAccessToken(token)
        } catch (e: any) {
            throw e;
        }
    }

    useEffect(() => {
        getAccessToken()
    }, [getAccessToken])

    if (getHotelData === undefined) {
        return <Loading/>
    } else {
        return <div className="custom-blue-900 h-screen">
            <Navbar/>
            <div className="flex flex-col items-center">
                <div className="w-160">
                    <p className="text-4xl font-bold my-2 mx-2">
                        Search for hotels
                    </p>
                    <div className="flex flex-col items-center">
                        <SearchBar/>
                        <Hotels hotels={getHotelData.results}/>
                    </div>
                </div>
            </div>
        </div>
    }
}