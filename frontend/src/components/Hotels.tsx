import React from "react";
import { Hotel } from "../utils/Hotel";
import {HotelItem} from "./HotelItem";
import {v4 as uuid4} from "uuid";

interface Props {
    hotels: Hotel[];
}

export const Hotels: React.FC<Props> = ({ hotels }) => {
    const allHotels = hotels.map((hotel) => (
        <HotelItem hotel={hotel} key={uuid4()} />
    ));

    return <>
        {allHotels}
    </>
};
