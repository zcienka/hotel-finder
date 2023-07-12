import React from "react"
import ReactDOM from "react-dom/client"
import App from "./App"
import "./index.css"
import {Auth0Provider} from "@auth0/auth0-react"
import {BrowserRouter} from "react-router-dom"
import store from "./app/store"
import {Provider} from "react-redux"

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
)

root.render(
    <React.StrictMode>
        <Provider store={store}>
            <BrowserRouter>
                <Auth0Provider
                    domain={process.env.REACT_APP_AUTH0_DOMAIN as string}
                    clientId={process.env.REACT_APP_AUTH0_CLIENT_ID as string}
                    cacheLocation="localstorage"
                    authorizationParams={{
                        audience: process.env.REACT_APP_AUTH0_AUDIENCE as string,
                        redirect_uri: process.env.REACT_APP_AUTH0_REDIRECT_URI as string
                    }}
                >
                    <App/>
                </Auth0Provider>
            </BrowserRouter>
        </Provider>
    </React.StrictMode>
)