import React from "react"
import {useAuth0} from "@auth0/auth0-react"
import {useNavigate} from "react-router-dom"

const Navbar = () => {
    const {user, isAuthenticated} = useAuth0()
    const navigate = useNavigate()

    return (
        <div className="h-16 bg-custom-blue-700 flex px-8 justify-center drop-shadow-lg">
            <div className="flex items-center w-256">
                <h1 className="text-3xl font-bold text-white-900 w-full cursor-pointer" onClick={() => navigate('/')}>
                    Hotel<span className="text-teal-700">finder</span>
                </h1>
                    {isAuthenticated && user ? <div className="flex w-full justify-end">
                            <img className="h-10 rounded-full mr-2" src={user.picture} alt="User profile"/>
                            <div>
                                <p className="text-white text-m font-semibold">{user.name}</p>
                                <p className="text-gray-300 text-xs">{user.email}</p>
                            </div>
                        </div>
                        : <p className="text-white text-lg font-semibold">Please log in</p>
                    }
            </div>
        </div>
    )
}

export default Navbar
