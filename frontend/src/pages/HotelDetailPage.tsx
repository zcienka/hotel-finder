import {useParams} from "react-router-dom"
import Navbar from "../components/Navbar"
import {useEffect, useState} from "react"
import {useGetSingleHotelQuery} from "../services/hotelApi"
import Loading from "../components/Loading"
import {useAuth0} from "@auth0/auth0-react"
import {v4 as uuid4} from "uuid"

export const HotelDetailPage = () => {
    const {id} = useParams()
    const [accessToken, setAccessToken] = useState<string | "">("")
    const {getAccessTokenSilently} = useAuth0()

    const [hotelId, setHotelId] = useState<string>("")

    const getAccessToken = async () => {
        try {
            const token = await getAccessTokenSilently()
            setAccessToken(token)
        } catch (e: any) {
            throw e
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
    } = useGetSingleHotelQuery(
        {
            accessToken,
            hotelId,
        },
        {
            skip: accessToken === "" || hotelId === "",
        }
    )

    useEffect(() => {
        if (id !== undefined) {
            setHotelId(id)
        }
    }, [id])

    if (getSingleHotelData === undefined) {
        return <Loading/>
    } else {
        const hotel = getSingleHotelData
        return (
            <>
                <Navbar/>
                <div className="flex justify-center">
                    <div className="w-256 py-4">
                        <div className="bg-custom-blue-700 shadow-lg rounded-lg overflow-hidden">
                            <div className="flex flex-row">
                                <img
                                    className="h-128 w-192 object-cover"
                                    src={hotel.image[0]}
                                    alt="Main Hotel Image"
                                />
                                <div className="grid grid-cols-1">
                                    {hotel.image.slice(1, 4).map((image, index) => (
                                        <img
                                            key={uuid4()}
                                            className="h-32 w-128 object-cover"
                                            src={image}
                                            alt={`Hotel Image ${index + 1}`}
                                        />
                                    ))}
                                    {hotel.image.length > 1 && (
                                        <div
                                            className="flex items-center justify-center w-full h-32 bg-gray-800 text-white">
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                className="h-6 w-6"
                                                fill="none"
                                                viewBox="0 0 24 24"
                                                stroke="currentColor"
                                            >
                                                <path
                                                    strokeLinecap="round"
                                                    strokeLinejoin="round"
                                                    strokeWidth={2}
                                                    d="M12 6v6m0 0v6m0-6h6m-6 0H6"
                                                />
                                            </svg>
                                            {hotel.image.length - 4} photos
                                        </div>
                                    )}
                                </div>
                            </div>
                            <div className="p-6">
                                <div className="flex flex-row">
                                    <h2 className="text-3xl font-bold mb-2 w-full">
                                        {hotel.name}
                                    </h2>
                                    <div className="w-full justify-end flex">
                                        <button>
                                            Book a room
                                        </button>
                                    </div>
                                </div>
                                <p className="text-gray-200">
                                    {hotel.city}, {hotel.address}
                                </p>
                                <p className="font-semibold text-gray-400 mb-2">
                                    {hotel.phoneNumber}
                                </p>
                                <p className="text-gray-400">{hotel.description}</p>

                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}
