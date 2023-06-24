import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css'
import {Auth0Provider} from '@auth0/auth0-react';
import {BrowserRouter} from 'react-router-dom';
import store from "./app/store";
import {Provider} from "react-redux";

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

root.render(
    <Provider store={store}>
        <Auth0Provider
            domain={process.env.AUTH0_DOMAIN as string}
            clientId={process.env.AUTH0_CLIENT_ID as string}
            cacheLocation='localstorage'
            authorizationParams={{
                audience: process.env.AUTH0_AUDIENCE as string,
                redirect_uri: process.env.AUTH0_REDIRECT_URI as string
            }}
        >
            <React.StrictMode>
                <BrowserRouter>
                    <App/>
                </BrowserRouter>
            </React.StrictMode>
        </Auth0Provider>
    </Provider>
);