import React, {useEffect, useState} from "react"
import {ReservationRequest} from "../utils/Reservation"
import Navbar from "../components/Navbar"
import {useGetSingleRoomQuery} from "../services/RoomApi"
import Loading from "../components/Loading";
import {useParams} from "react-router-dom";
import {ReactComponent as PersonIcon} from "../icons/person.svg";

export const CreateReservationPage = () => {
    const {hotelId, id} = useParams()

    const [roomId, setRoomId] = useState<string>("")
    const [reservation, setReservation] = useState<ReservationRequest>({
        checkInDate: "",
        checkOutDate: "",
        hotelId: "",
        roomId: "",
        userEmail: ""
    })

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target
        setReservation((prevReservation) => ({
            ...prevReservation,
            [name]: value
        }))
    }

    const {
        data: getSingleRoomData,
        isFetching: isGetHotelFetching,
        isSuccess: isGetHotelSuccess,
        isError: isGetHotelError,
    } = useGetSingleRoomQuery({roomId},
        {
            skip: roomId === "",
        })

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault()
    }

    useEffect(() => {
        if (id !== undefined) {
            setRoomId(id)
        }
    }, [id])

    if (getSingleRoomData === undefined) {
        return <Loading/>
    } else {
        const room = getSingleRoomData

        return <div className="min-h-screen">
            <Navbar/>
            <div className="flex flex-col items-center">
                <div className="w-256 my-4">
                    <div className="bg-custom-blue-700 rounded-2xl overflow-hidden shadow-lg">
                        <form className="flex flex-col" onSubmit={handleSubmit}>
                            <img src={room.image[0]} alt={room.name} className="w-full"/>
                            <div className="p-4">
                                <h2 className="text-3xl font-bold mb-1 w-full">{room.name}</h2>
                                <div className="flex flex-row items-center ">
                                    <div className="h-5 w-5 mr-2 ">
                                        <PersonIcon/>
                                    </div>
                                    <p className="text-lg text-gray-400 font-semibold">{room.capacity}</p>
                                </div>
                                <p className="text-lg text-gray-400 font-semibold">${room.price} / night</p>
                                <p className="text-lg">{room.description}</p>
                            </div>
                            <div className="px-4">
                                <label className="my-2">
                                    Check-in date:
                                </label>
                                <input
                                    type="date"
                                    name="checkInDate"
                                    value={reservation.checkInDate}
                                    onChange={handleInputChange}
                                    className="w-full py-3 px-6 rounded-lg bg-custom-blue-900"
                                />

                                <label className="my-2">
                                    Check-out date:
                                </label>
                                <input
                                    type="date"
                                    name="checkOutDate"
                                    value={reservation.checkOutDate}
                                    onChange={handleInputChange}
                                    className="w-full py-3 px-6 rounded-lg bg-custom-blue-900"
                                />
                                <div className="flex justify-end">
                                    <button className="mt-3 mb-4 py-2 px-4 ">
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
