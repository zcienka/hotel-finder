import SearchBar from "../components/SearchBar"
import Navbar from "../components/Navbar"
import {useAuth0} from "@auth0/auth0-react"
import {useEffect, useState} from "react"
import {useGetHotelsQuery, useSearchHotelsQuery} from "../services/HotelApi"
import Loading from "../components/Loading"
import {Hotels} from "../components/Hotels"
import {SearchQuery} from "../utils/SearchQuery";
import {SearchResults} from "./SearchResults";

export const HomePage = () => {
    const {getAccessTokenSilently, loginWithRedirect} = useAuth0()
    const [accessToken, setAccessToken] = useState<string | "">("")

    const [searchValue, setSearchValue] = useState("")
    const [cityValue, setCityValue] = useState("")
    const [roomValue, setRoomValue] = useState("")
    const [checkInDate, setCheckInDate] = useState("")
    const [checkOutDate, setCheckOutDate] = useState("")
    const [isSearchHotel, setIsSearchHotel] = useState(false)

    const {
        data: getHotelData,
        isFetching: isGetHotelFetching,
        isSuccess: isGetHotelSuccess,
        isError: isGetHotelError,
    } = useGetHotelsQuery()

    const {
        data: getSearchHotelsData,
        isFetching: isSearchHotelsFetching,
        isSuccess: isSearchHotelsSuccess,
        isError: isSearchHotelsError,
    } = useSearchHotelsQuery(
        {
            name: searchValue,
            city: cityValue,
            roomCount: parseInt(roomValue),
            checkInDate: checkInDate,
            checkOutDate: checkOutDate,
        } as SearchQuery, {
            skip: !isSearchHotel,
        }
    )

    const getAccessToken = async () => {
        try {
            const token = await getAccessTokenSilently()
            setAccessToken(token)
        } catch (e: any) {
            throw e
        }
    }

    console.log({searchValue})

    useEffect(() => {
        getAccessToken()
    }, [getAccessToken])

    if (getHotelData === undefined) {
        return <Loading/>
    } else {
        return <div className="custom-blue-900 h-screen">
            <Navbar/>
            <div className="flex flex-col items-center">
                <div className="w-256">
                    <p className="text-5xl font-bold my-4 mx-2">
                        Search for hotels
                    </p>
                    <div className="flex flex-col items-center">
                        <SearchBar
                            searchValue={searchValue}
                            setSearchValue={setSearchValue}
                            cityValue={cityValue}
                            setCityValue={setCityValue}
                            roomValue={roomValue}
                            setRoomValue={setRoomValue}
                            checkInDate={checkInDate}
                            setCheckInDate={setCheckInDate}
                            checkOutDate={checkOutDate}
                            setCheckOutDate={setCheckOutDate}
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