import React from "react";
import {Hotel} from "../utils/Hotel";

export const HotelItem = (hotel: Hotel) => {
    return <div>
        {hotel.name}
    </div>
};