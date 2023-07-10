import React from "react"
import {Route, Routes} from "react-router-dom"
import {HomePage} from "./pages/HomePage"
import {HotelDetailPage} from "./pages/HotelDetailPage"
import {CreateReservationPage} from "./pages/CreateReservationPage"

function App() {
    return <Routes>
            <Route path="/" element={<HomePage/>}/>
            <Route path="/hotels/:id" element={<HotelDetailPage/>}/>
            <Route path="/hotels/:hotelId/create-reservation/:id" element={<CreateReservationPage/>}/>
        </Routes>
}

export default App
