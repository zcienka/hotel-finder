import React from "react";
import {Hotel} from "../utils/Hotel";

interface Props {
    hotel: Hotel;
}

export const HotelItem: React.FC<Props> = ({ hotel }) => {
    console.log(hotel.image[0])
    return <div className="flex flex-col items-center">
        <p>{hotel.name}</p>
        <p>{hotel.description}</p>
        <p>{hotel.city}</p>
        <img src={hotel.image[0]} alt="hotel image"/>
    </div>
}
