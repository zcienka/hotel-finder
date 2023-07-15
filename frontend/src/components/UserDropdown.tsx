import React from "react"
import {useNavigate} from "react-router-dom"
import {useAuth0} from "@auth0/auth0-react";

export const UserDropdown = () => {
    const navigate = useNavigate()
    const {logout} = useAuth0()

    const handleLogout = (e: any) => {
        logout()
        e.preventDefault()
        navigate("/")
    }

    return <div className="absolute bg-custom-blue-800 rounded-lg shadow-lg top-16 right-0 cursor-pointer">
            <p className="px-4 py-2 hover:bg-custom-blue-700" onClick={(e) => handleLogout(e)}>
                Log out
            </p>
            <p className="px-4 py-2 hover:bg-custom-blue-700" onClick={() => navigate("/user/reservations")}>
                Your Reservations
            </p>
        </div>
}
