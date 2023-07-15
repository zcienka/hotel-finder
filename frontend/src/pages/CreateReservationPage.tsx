import React, {useEffect, useState} from "react"
import {ReservationRequest} from "../utils/Reservation"
import Navbar from "../components/Navbar"
import {useGetSingleRoomQuery} from "../services/RoomApi"
import Loading from "../components/Loading"
import {useNavigate, useParams} from "react-router-dom"
import {ReactComponent as PersonIcon} from "../icons/person.svg"
import {Helmet} from "react-helmet"
import {useCreateReservationMutation} from "../services/ReservationApi"
import {useAuth0} from "@auth0/auth0-react"
import {toast, Toaster} from "react-hot-toast"
import {WarningToast} from "../components/WarningToast"
import {SuccessToast} from "../components/SuccessToast"

export const CreateReservationPage = () => {
    const {hotelId, id} = useParams()
    const navigate = useNavigate()
    const [accessToken, setAccessToken] = useState<string | "">("")
    const [roomId, setRoomId] = useState<string>("")

    const {getAccessTokenSilently, isAuthenticated} = useAuth0()

    const [reservation, setReservation] = useState<ReservationRequest>({
        checkInDate: "",
        checkOutDate: "",
        hotelId: "",
        roomId: "",
        userEmail: ""
    })

    const getAccessToken = async () => {
        if (isAuthenticated) {
            const token = await getAccessTokenSilently()
            setAccessToken(token)
        }
    }

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target
        setReservation((prevReservation) => ({
            ...prevReservation,
            [name]: value
        }))
    }

    const {
        data: getSingleRoomData,
    } = useGetSingleRoomQuery({roomId}, {skip: roomId === ""})

    useEffect(() => {
        getAccessToken()
    }, [getAccessToken])

    const [createReservation, {
        isSuccess: isCreateReservationSuccess,
        isError: isCreateReservationError,
        error: createReservationError
    }] = useCreateReservationMutation()

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault()

        if (reservation.checkInDate !== "" && reservation.checkOutDate !== "") {
            createReservation({reservation, accessToken})

        } else {
            toast.custom((t) => (
                <WarningToast t={t} message="Please fill in all the fields"/>
            ), {
                id: "warning-toast",
                duration: 5000,
            })
        }
    }

    useEffect(() => {
        if (isCreateReservationSuccess) {
            toast.custom((t) => (
                <SuccessToast t={t} message="Reservation successfully created"/>
            ), {
                id: "success-toast",
                duration: 5000,
            })
            navigate("/")
        } else {
            if (createReservationError !== undefined) {
                    const errorMessage: string = (createReservationError as any).data || "An error occurred while creating the reservation"
                    toast.custom((t) => (
                        <WarningToast t={t} message={errorMessage}/>
                    ), {
                        id: "warning-toast",
                        duration: 5000,
                    })
            }
        }
    }, [createReservationError, isCreateReservationSuccess, navigate])

    useEffect(() => {
        if (id !== undefined) {
            setRoomId(id)
            setReservation((prevReservation) => ({
                ...prevReservation,
                roomId: id
            }))
        }

        if (hotelId !== undefined) {
            setReservation((prevReservation) => ({
                ...prevReservation,
                hotelId: hotelId
            }))
        }
    }, [hotelId, id])

    if (getSingleRoomData === undefined) {
        return <Loading/>
    } else {
        const room = getSingleRoomData

        return <div className="min-h-screen">
            <Toaster position="bottom-right"/>
            <Helmet>
                <title>Book a room</title>
            </Helmet>
            <Navbar/>
            <div className="flex flex-col items-center">
                <div className="w-256 my-4">
                    <div className="bg-custom-blue-700 rounded-2xl overflow-hidden shadow-lg">
                        <form className="flex flex-col" onSubmit={handleSubmit}>
                            <img src={room.image[0]} alt={room.name} className="w-full"/>
                            <div className="p-8 pb-2">
                                <h2 className="text-3xl font-bold mb-1 w-full">{room.name}</h2>
                                <p className="text-lg text-gray-400 font-semibold">${room.price} / night</p>
                                <div className="flex flex-row items-center ">
                                    <div className="h-5 w-5 mr-2 ">
                                        <PersonIcon/>
                                    </div>
                                    <p className="text-lg text-gray-400 font-semibold">{room.capacity}</p>
                                </div>
                                <p className="text-lg">{room.description}</p>
                            </div>
                            <div className="flex flex-col px-8 text-lg">
                                <label>
                                    Check-in date:
                                </label>
                                <input
                                    type="date"
                                    name="checkInDate"
                                    value={reservation.checkInDate}
                                    onChange={handleInputChange}
                                    className="w-52 py-2 px-4 rounded-lg bg-custom-blue-900 my-2"
                                />

                                <label>
                                    Check-out date:
                                </label>
                                <input
                                    type="date"
                                    name="checkOutDate"
                                    value={reservation.checkOutDate}
                                    onChange={handleInputChange}
                                    className="w-52 py-2 px-4 rounded-lg bg-custom-blue-900 my-2"
                                />
                                <div className="flex justify-end">
                                    <button className="mt-3 mb-8 py-2 px-4">
                                        Book Now
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    }
}
