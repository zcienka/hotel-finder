import React from "react"
import {useAuth0} from "@auth0/auth0-react"
import {useNavigate} from "react-router-dom"
import {useState} from "react"
import {UserDropdown} from "./UserDropdown";

const Navbar = () => {
    const [showDropdown, setShowDropdown] = useState(false)
    const {user, isAuthenticated, loginWithRedirect} = useAuth0()
    const navigate = useNavigate()

    const loginUser = async () => {
        await loginWithRedirect()
    }

    return <div className="h-16 bg-custom-blue-700 flex justify-center drop-shadow-lg">
            <div className="flex items-center w-256">
                <h1 className="text-3xl font-bold text-white-900 w-full cursor-pointer" onClick={() => navigate("/")}>
                    Hotel<span className="text-teal-700">finder</span>
                </h1>
                {isAuthenticated ? (
                    <div className="flex w-full justify-end cursor-pointer" onClick={() => setShowDropdown(!showDropdown)}>
                        {user && (
                            <div>
                                <img className="h-10 rounded-full mr-2" src={user.picture} alt="User profile"/>
                                <div>
                                    <p className="text-white text-m font-semibold">{user.name}</p>
                                    <p className="text-gray-300 text-xs">{user.email}</p>
                                </div>
                            </div>
                        )}
                    </div>
                ) : (
                    <div className="flex w-full justify-end">
                        <button className="text-white text-lg font-semibold" onClick={() => loginUser()}>
                            Log in
                        </button>
                    </div>
                )}
            </div>
        {showDropdown && <div className="absolute w-256"><UserDropdown/></div>}
    </div>
}

export default Navbar
