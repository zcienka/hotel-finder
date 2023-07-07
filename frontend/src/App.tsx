import React from "react"
import {Route, Routes} from "react-router-dom"
import {HomePage} from "./pages/HomePage"
import {HotelDetailPage} from "./pages/HotelDetailPage"
import {SearchResults} from "./pages/SearchResults"

function App() {
    return (
        <Routes>
            <Route path="/" element={<HomePage/>}/>
            <Route path="/hotels/:id" element={<HotelDetailPage/>}/>
            <Route path="/search" element={<SearchResults/>}/>
        </Routes>
    )
}

export default App
