import React from "react"
import {Hotel} from "../utils/Hotel"
import {v4 as uuid4} from "uuid"
import {useNavigate} from "react-router-dom"

interface Props {
    hotels: Hotel[]
}

export const Hotels: React.FC<Props> = ({hotels}) => {
    const navigate = useNavigate()

    const allHotels = hotels.map((hotel) => (
        <div className="flex items-center w-full bg-custom-blue-700 rounded-2xl m-2 drop-shadow-lg cursor-pointer"
             onClick={() => navigate(`/hotels/${hotel.id}`)} key={uuid4()}>
            <img className="h-48 w-48 rounded-l-2xl" src={hotel.image[0]} alt="hotel image"/>
            <div className="p-4">
                <h2 className="text-2xl font-bold">{hotel.name}</h2>
                <p className="mt-1 text-gray-200">{hotel.city}</p>
                <p className="text-gray-400 overflow-hidden max-h-24">
                    {hotel.description}
                </p>
            </div>
        </div>
    ))

    return <>
        {allHotels}
    </>
}
