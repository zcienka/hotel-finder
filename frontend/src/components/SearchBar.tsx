import {ReactComponent as Arrow} from "../icons/rightArrow.svg"
import {ReactComponent as CityIcon} from "../icons/city.svg"

const SearchBar = () => {
    return <>
        <div className="flex justify-center items-center bg-custom-blue-700 rounded-2xl w-160 drop-shadow-lg mb-2">
            <input
                className="rounded-l-full w-full py-4 px-6 leading-tight focus:outline-none border-transparent bg-custom-blue-700"
                id="search" type="text" placeholder="Search"/>
            <div className="p-3">
                <button
                    className="rounded-full p-2 focus:outline-none w-10 h-10 flex items-center justify-center text-gray-300">
                    <Arrow/>
                </button>
            </div>
        </div>

        <div className="flex flex-row mb-2">
            <div>
                <label htmlFor="cityInput">City</label>
                <div
                    className="flex justify-center items-center bg-custom-blue-700 rounded-xl shadow drop-shadow-lg h-12 mr-2 mt-1">
                    <input
                        className="w-full py-3 px-6 leading-tight focus:outline-none border-transparent rounded-xl bg-custom-blue-700"
                        id="cityInput" type="text"/>
                    <div className="pr-4">
                        <div className="rounded-xl focus:outline-none w-7 h-1 flex items-center justify-center">
                            <CityIcon/>
                        </div>
                    </div>
                </div>
            </div>

            <div>
                <label htmlFor="dateInput1">Check-in date</label>
                <div className="flex justify-center mr-2 h-12 cursor-pointer mt-1">
                    <input type="date"
                           className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 rounded-xl shadow focus:outline-none focus:shadow-outline drop-shadow-lg"
                           id="dateInput1">
                    </input>
                </div>
            </div>

            <div>
                <label htmlFor="dateInput2">Check-out date</label>
                <div className="flex justify-center h-12 cursor-pointer mt-1">
                    <input type="date"
                           className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 rounded-xl shadow focus:outline-none focus:shadow-outline drop-shadow-lg"
                           id="dateInput2">
                    </input>
                </div>
            </div>
        </div>
    </>
}

export default SearchBar
