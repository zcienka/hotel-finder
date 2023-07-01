const SearchBar = () => {
    return <>
        <div className="flex justify-center items-center bg-custom-blue-800 rounded-3xl w-1/2 drop-shadow-lg">
            <input
                className="rounded-l-full w-full py-4 px-6 leading-tight focus:outline-none border-transparent bg-custom-blue-800"
                id="search" type="text" placeholder="Search"/>
            <div className="p-4">
                <button
                    className="bg-orange-600 text-white rounded-full p-2 hover:bg-orange-500 focus:outline-none w-12 h-12 flex items-center justify-center">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="3"
                         stroke="currentColor" className="w-6 h-6">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M13.5 4.5L21 12m0 0l-7.5 7.5M21 12H3"/>
                    </svg>
                </button>
            </div>
        </div>
        <div className="flex flex-row">
            <div className="relative inline-block">
                <input type="text" placeholder="City"
                       className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 pl-8 rounded-xl shadow leading-tight focus:outline-none focus:shadow-outline drop-shadow-lg">
                </input>
                <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center px-2 text-gray-700">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                         stroke="currentColor" className="w-6 h-6">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5"/>
                    </svg>
                </div>
            </div>

            <div className="relative inline-block">
                <input type="date" placeholder="City"
                       className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 pl-8 rounded-xl shadow leading-tight focus:outline-none focus:shadow-outline drop-shadow-lg">
                </input>
                <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center px-2 text-gray-700">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                         stroke="currentColor" className="w-6 h-6">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5"/>
                    </svg>
                </div>
            </div>
            <div className="relative inline-block">
                <input type="date" placeholder="City"
                       className="block appearance-none w-full bg-custom-blue-700 px-4 py-2 pl-8 rounded-xl shadow leading-tight focus:outline-none focus:shadow-outline drop-shadow-lg">
                </input>
                <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center px-2 text-gray-700">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                         stroke="currentColor" className="w-6 h-6">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5"/>
                    </svg>
                </div>
            </div>
        </div>
    </>
}

export default SearchBar