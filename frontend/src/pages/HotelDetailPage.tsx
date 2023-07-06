import {useParams} from "react-router-dom"
import Navbar from "../components/Navbar"
import {useEffect, useState} from "react"
import {useGetSingleHotelQuery} from "../services/hotelApi"
import Loading from "../components/Loading"
import {useAuth0} from "@auth0/auth0-react"
import {v4 as uuid4} from "uuid"
import {CommentSection} from "../components/CommentSection"
import {ReactComponent as PlusIcon} from "../icons/plusIcon.svg"
import {ReactComponent as LeftArrowIcon} from "../icons/leftArrow.svg"
import {ReactComponent as RightArrowIcon} from "../icons/rightArrow.svg"
import {ReactComponent as XMark} from "../icons/xMark.svg"
import {AvailableRooms} from "../components/AvailableRooms";
import {useGetRoomsInHotelQuery} from "../services/roomApi";

export const HotelDetailPage = () => {
    const {id} = useParams()
    const [accessToken, setAccessToken] = useState<string | "">("")
    const {getAccessTokenSilently} = useAuth0()
    const [selectedImage, setSelectedImage] = useState("")
    const [selectedImageIndex, setSelectedImageIndex] = useState(0)

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
    } = useGetSingleHotelQuery({
            hotelId,
        }, {
            skip: hotelId === "",
        }
    )

    const {
        data: getRoomsData,
        isFetching: isGetRoomsFetching,
        isSuccess: isGetRoomsSuccess,
        isError: isGetRoomsError,
    } = useGetRoomsInHotelQuery({
            hotelId,
        }, {
            skip: hotelId === "",
        }
    )

    useEffect(() => {
        if (id !== undefined) {
            setHotelId(id)
        }
    }, [id])

    if (getSingleHotelData !== undefined && getRoomsData !== undefined) {
        const hotel = getSingleHotelData
        const rooms = getRoomsData.results

        const handlePrevImage = () => {
            if (selectedImageIndex > 0) {
                setSelectedImageIndex(selectedImageIndex - 1)
                setSelectedImage(hotel.image[selectedImageIndex - 1])
            }
        }

        const handleNextImage = () => {
            if (selectedImageIndex < hotel.image.length - 1) {
                setSelectedImageIndex(selectedImageIndex + 1)
                setSelectedImage(hotel.image[selectedImageIndex + 1])
            }
        }

        return <>
            <Navbar/>
            <div className="flex justify-center">
                <div className="w-256 py-4">
                    <div className="bg-custom-blue-700 shadow-lg rounded-2xl overflow-hidden">
                        <div className="flex flex-row">
                            <img
                                className="h-128 w-192 object-cover cursor-pointer"
                                src={hotel.image[0]}
                                alt="Main Hotel Image"
                                onClick={() => setSelectedImage(hotel.image[0])}
                            />
                            <div className="grid grid-cols-1">
                                {hotel.image.slice(1, 4).map((image, index) => (
                                    <img
                                        key={uuid4()}
                                        className="h-32 w-128 object-cover cursor-pointer"
                                        src={image}
                                        alt={`Hotel Image ${index + 1}`}
                                        onClick={() => setSelectedImage(image)}
                                    />
                                ))}
                                {hotel.image.length > 1 && (
                                    <div
                                        className="flex items-center justify-center w-full h-32 bg-gray-800 text-white">
                                        <div className="h-6">
                                            <PlusIcon/>
                                        </div>
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
                            </div>
                            <p className="text-gray-200 font-semibold">
                                {hotel.city}, {hotel.address}
                            </p>
                            <p className="font-semibold text-gray-400 mb-2">
                                {hotel.phoneNumber}
                            </p>
                            <p className="text-gray-400">{hotel.description}</p>
                        </div>
                    </div>
                    <AvailableRooms rooms={rooms}/>
                    <CommentSection hotelId={hotelId} accessToken={accessToken}/>
                </div>
            </div>
            {selectedImage && (
                <div
                    className="fixed top-0 left-0 flex items-center justify-center w-full h-full bg-black bg-opacity-50 z-1">
                    <img className="max-h-full max-w-full" src={selectedImage} alt="Selected Image"/>

                    <button
                        className="absolute top-2 right-2 text-white text-xl focus:outline-none bg-transparent hover:bg-transparent"
                        onClick={() => setSelectedImage("")}
                    >
                        <XMark/>
                    </button>

                    {selectedImageIndex !== 0 && (
                        <button
                            className="absolute top-1/2 left-4 transform -translate-y-1/2 focus:outline-none bg-transparent hover:bg-transparent"
                            onClick={handlePrevImage}
                        >
                            <LeftArrowIcon className="w-8 h-8 text-white"/>
                        </button>
                    )}

                    {selectedImageIndex !== hotel.image.length - 1 && (
                        <button
                            className="absolute top-1/2 right-4 transform -translate-y-1/2 focus:outline-none bg-transparent hover:bg-transparent"
                            onClick={handleNextImage}
                        >
                            <RightArrowIcon className="w-8 h-8 text-white"/>
                        </button>
                    )}
                </div>
            )}
        </>
    } else {
        return <Loading/>
    }
}
