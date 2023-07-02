import React from "react";
import {useAuth0} from "@auth0/auth0-react";

const Navbar = () => {
    const {user, isAuthenticated} = useAuth0();

    return (
        <div className="h-16 bg-custom-blue-700 flex px-8 justify-center">
            <div className="flex items-center w-160">
                <h1 className="text-3xl font-bold text-white-900 w-full">
                    Hotel<span className="text-orange-700">finder</span>
                </h1>
                    {isAuthenticated && user ? (<div className="flex w-full justify-end">
                            <img className="h-10 rounded-full mr-2" src={user.picture} alt="User profile"/>
                            <div>
                                <p className="text-white text-m font-semibold">{user.name}</p>
                                <p className="text-gray-300 text-xs">{user.email}</p>
                            </div>
                        </div>)
                        : (
                            <p className="text-white text-lg font-semibold">Please log in</p>)
                    }
                {/*{isAuthenticated && (*/}
                {/*    <button*/}
                {/*        className="ml-2"*/}
                {/*        onClick={() => logout()}*/}
                {/*    >*/}
                {/*        Logout*/}
                {/*    </button>*/}
                {/*)}*/}
            </div>
        </div>
    );
};

export default Navbar;
