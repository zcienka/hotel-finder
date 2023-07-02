import React from "react";
import {useAuth0} from "@auth0/auth0-react";

const Navbar = () => {
    const {user, isAuthenticated, logout} = useAuth0();

    return (
        <div className="h-16 bg-custom-blue-700 flex px-8 justify-end">
            <div className="flex items-center">
                {isAuthenticated && user && (
                    <img className="h-10 rounded-full mr-2" src={user.picture} alt="User profile"/>
                )}
                <div>
                    {isAuthenticated && user ? <>
                            <p className="text-white text-lg font-semibold">{user.name}</p>
                            <p className="text-gray-300 text-sm">{user.email}</p>
                        </>
                        : (
                            <p className="text-white text-lg font-semibold">Please log in</p>
                        )}
                </div>
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
