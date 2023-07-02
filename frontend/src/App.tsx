import React from 'react'
import {LoginRegister} from "./pages/LoginRegister"
import {Route, Routes} from "react-router-dom"
import Navbar from "./components/Navbar"
import {HomePage} from "./pages/HomePage"
import {HotelDetailPage} from "./pages/HotelDetailPage";

function App() {
    return (
        <Routes>
            <Route path="/" element={<HomePage/>}/>
            <Route path="/hotels/:id" element={<HotelDetailPage/>}/>
            <Route path="/login" element={<LoginRegister/>}/>
        </Routes>
    )
}

export default App
