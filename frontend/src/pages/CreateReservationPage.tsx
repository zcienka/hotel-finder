import React, {useEffect, useState} from "react"
import {ReservationRequest} from "../utils/Reservation"
import Navbar from "../components/Navbar"
import {useGetSingleRoomQuery} from "../services/RoomApi"
import Loading from "../components/Loading";
import {useParams} from "react-router-dom";

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
        return <div className="min-h-screen">
            <Navbar/>
            <div className="flex flex-col items-center">
                <div className="w-256 my-4">
                    <div className="bg-custom-blue-700 shadow-lg rounded-2xl overflow-hidden">
                        <form className="flex flex-col p-4" onSubmit={handleSubmit}>
                            <h2 className="text-2xl font-bold">Book a room</h2>
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
                            <button className="mt-4 py-2 px-4">
                                Submit
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    }
}
