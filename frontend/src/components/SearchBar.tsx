import {ReactComponent as Arrow} from "../icons/rightArrow.svg"
import {ReactComponent as ChevronDown} from "../icons/chevronDown.svg"
import {ReactComponent as CityIcon} from "../icons/city.svg"

const SearchBar = () => {
    return <>
        <div className="flex justify-center items-center bg-custom-blue-700 rounded-3xl w-160 drop-shadow-lg mb-3">
            <input
                className="rounded-l-full w-full py-4 px-6 leading-tight focus:outline-none border-transparent bg-custom-blue-700"
                id="search" type="text" placeholder="Search"/>
            <div className="p-4">
                <button
                    className="rounded-full p-2 focus:outline-none w-12 h-12 flex items-center justify-center">
                    <Arrow/>
                </button>
            </div>
        </div>

        <div className="flex flex-row mb-2">
            <div className="flex justify-center items-center bg-custom-blue-700 rounded-xl shadow drop-shadow-lg h-12 mr-2">
                <input
                    className="w-full py-3 px-6 leading-tight focus:outline-none border-transparent rounded-xl bg-custom-blue-700"
                    id="search" type="text" placeholder="City"/>
                <div className="pr-4">
                    <div className="rounded-xl focus:outline-none w-7 h-1 flex items-center justify-center">
                        <CityIcon/>
                    </div>
                </div>
            </div>

            <div className="flex justify-center mr-2 h-12 cursor-pointer">
                <input type="date"
                       className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 pl-8 rounded-xl shadow leading-tight focus:outline-none focus:shadow-outline drop-shadow-lg">
                </input>
            </div>
            <div className="flex justify-center h-12 cursor-pointer">
                <input type="date"
                       className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 pl-8 rounded-xl shadow leading-tight focus:outline-none focus:shadow-outline drop-shadow-lg">
                </input>
            </div>
        </div>
    </>
}

export default SearchBar