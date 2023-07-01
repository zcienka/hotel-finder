import React from "react";
import { Hotel } from "../utils/Hotel";
import {HotelItem} from "./HotelItem";

interface Props {
    hotels: Hotel[];
}

export const Hotels: React.FC<Props> = ({ hotels }) => {
    const allHotels = hotels.map((hotel) => (
        <HotelItem hotel={hotel} />
    ));

    return <>
        {allHotels}
    </>
};
