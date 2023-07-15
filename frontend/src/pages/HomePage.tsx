import SearchBar from "../components/SearchBar"
import Navbar from "../components/Navbar"
import React, {useState} from "react"
import {useGetHotelsQuery, useSearchHotelsQuery} from "../services/HotelApi"
import Loading from "../components/Loading"
import {Hotels} from "../components/Hotels"
import {Helmet} from "react-helmet"

export const HomePage = () => {
    const [searchValue, setSearchValue] = useState("")
    const [cityValue, setCityValue] = useState("")
    const [roomValue, setRoomValue] = useState("")
    const [checkInDate, setCheckInDate] = useState("")
    const [checkOutDate, setCheckOutDate] = useState("")
    const [category, setCategory] = useState("")
    const [isSearchHotel, setIsSearchHotel] = useState(false)

    const {
        data: getHotelData,
    } = useGetHotelsQuery()

    const {
        data: getSearchHotelsData,
    } = useSearchHotelsQuery(
        {
            name: searchValue,
            city: cityValue,
            roomCount: parseInt(roomValue),
            checkInDate: checkInDate,
            checkOutDate: checkOutDate,
            category: category,
        }, {
            skip: !isSearchHotel,
        }
    )

    if (getHotelData === undefined) {
        return <Loading/>
    } else {
        return <div className="custom-blue-900 h-screen">
            <Navbar/>
            <Helmet>
                <title>Hotel finder</title>
            </Helmet>
            <div className="flex flex-col items-center">
                <div className="w-256">
                    <p className="text-5xl font-bold my-4 mx-2">
                        Search for hotels
                    </p>
                    <div className="flex flex-col items-center">
                        <SearchBar
                            setSearchValue={setSearchValue}
                            setCityValue={setCityValue}
                            setRoomValue={setRoomValue}
                            setCheckInDate={setCheckInDate}
                            setCheckOutDate={setCheckOutDate}
                            setCategory={setCategory}
                            setIsSearchHotel={setIsSearchHotel}
                        />
                        {getSearchHotelsData ?
                            <Hotels hotels={getSearchHotelsData.results}/> :
                            <Hotels hotels={getHotelData.results}/>}
                    </div>
                </div>
            </div>
        </div>
    }
}