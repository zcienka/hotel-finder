import React from "react"
import {Room} from "../utils/Room"
import {ReactComponent as PersonIcon} from "../icons/person.svg"
import {useNavigate} from "react-router-dom"

interface Props {
    room: Room
}

export const RoomPreview: React.FC<Props> = ({room}) => {
    const navigate = useNavigate()

    return <div className="w-96 bg-custom-blue-700 rounded-2xl shadow-md overflow-hidden mr-2 mt-2">
        <img className="w-full h-56 object-cover" src={room.image[0]} alt="Room Image"/>

        <div className="p-4 flex flex-col">
            <div className="flex flex-row">
                <h2 className="text-xl font-semibold grow">{room.name}</h2>
                <button className="flex justify-end" onClick={() => navigate(`create-reservation/${room.id}`)}>
                    Book Now
                </button>
            </div>
            <div className="flex flex-row items-center">
                <div className="h-5 w-5 mr-2">
                    <PersonIcon/>
                </div>
                <p className="text-gray-400 font-semibold">{room.capacity}</p>
            </div>

            <p className="text-gray-400 font-semibold">${room.price} / night</p>
            <p className="text-gray-400 mt-1">{room.description}</p>
        </div>
    </div>
}
