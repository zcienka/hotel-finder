import {useParams} from "react-router-dom";
import Navbar from "../components/Navbar";
import {useEffect, useState} from "react";
import {useGetSingleHotelQuery} from "../services/hotelApi";
import Loading from "../components/Loading";
import {useAuth0} from "@auth0/auth0-react";

export const HotelDetailPage = () => {
    const {id} = useParams()
    const [accessToken, setAccessToken] = useState<string | "">("")
    const {getAccessTokenSilently} = useAuth0()

    const [hotelId, setHotelId] = useState<string>("")

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

    const {
        data: getSingleHotelData,
        isFetching: isGetSingleHotelFetching,
        isSuccess: isGetSingleHotelSuccess,
        isError: isGetSingleHotelError,
    } = useGetSingleHotelQuery({
        accessToken,
        hotelId
    }, {
        skip: accessToken === "" ||
            hotelId === ""
    })

    useEffect(() => {
        if (id !== undefined) {
            setHotelId(id)
        }
    }, [id])


    if (getSingleHotelData === undefined) {
        return <Loading/>
    } else {
        const hotel = getSingleHotelData
        return <>
            <Navbar/>
            <div className="flex justify-center">
                <div className="w-160 p-4">
                    <div className="bg-custom-blue-700 shadow-lg rounded-lg overflow-hidden">
                        <img
                            className="w-full h-64 object-cover object-center"
                            src={hotel.image[0]}
                            alt="hotel image"
                        />
                        <div className="p-6">
                            <h2 className="text-3xl font-bold mb-2">{hotel.name}</h2>
                            <p className="text-gray-200">
                                {hotel.city}, {hotel.address}
                            </p>
                            <p className="font-semibold text-gray-400 mb-2">{hotel.phoneNumber}</p>
                            <p className="text-gray-400">{hotel.description}</p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    }
}