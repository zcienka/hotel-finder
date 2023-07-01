import {useAuth0} from "@auth0/auth0-react";
import React, {useEffect, useState} from "react";

export const LoginRegister = () => {
    const {loginWithRedirect, getAccessTokenSilently} = useAuth0()

    const getMessage = async () => {
        await loginWithRedirect()

        // const accessToken = await getAccessTokenSilently()

        // console.log({accessToken})
    }

    return (
        <div className="content-layout">
            <h1 id="page-title">
                Protected Page
            </h1>
            <div>
                <p id="page-description">
            <span>
              This page retrieves a <strong>protected message</strong> from an
              external API.
            </span>
            <span>
              <strong>Only authenticated users can access this page.</strong>
            </span>
                </p>
                <button onClick={() => getMessage()}>message</button>
            </div>
        </div>
    );
};