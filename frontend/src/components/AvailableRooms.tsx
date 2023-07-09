import React, {useState} from "react"
import {Room} from "../utils/Room"
import {v4 as uuid4} from "uuid"
import {RoomPreview} from "./RoomPreview"

interface Props {
    rooms: Room[]
}

export const AvailableRooms: React.FC<Props> = ({rooms}) => {
    const [roomPreview, setShowRoomPreview] = useState<Room | undefined>(undefined)

    const allRooms = rooms.map((room) => (
        <div className="flex" key={uuid4()} onClick={() => setShowRoomPreview(() => room)}>
            <RoomPreview room={room}/>
        </div>
    ))

    return <div className="">
        <h2 className="text-2xl font-bold mt-2">Available rooms</h2>
        <div className="flex flex-row">
            {allRooms}
        </div>
    </div>
}
