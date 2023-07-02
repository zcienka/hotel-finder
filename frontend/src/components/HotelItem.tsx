import React from "react";
import {Hotel} from "../utils/Hotel";

interface Props {
    hotel: Hotel;
}

export const HotelItem: React.FC<Props> = ({hotel}) => {
    return (
        <div className="flex items-center w-full bg-custom-blue-700 rounded-2xl m-2 drop-shadow-lg cursor-pointer">
            <img className="h-40 w-40 rounded-l-2xl mr-4" src={hotel.image[0]} alt="hotel image"/>
            <div>
                <h2 className="text-2xl font-bold">{hotel.name}</h2>
                <p className="text-gray-400">{hotel.description}</p>
                <p className="mt-1 text-gray-300">{hotel.city}</p>
            </div>
        </div>
    );
};

