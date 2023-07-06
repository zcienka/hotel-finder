import {withAuthenticationRequired} from '@auth0/auth0-react';
import React from 'react';
import Loading from './Loading'

export const Redirect = ({component}: any) => {
    const Component = withAuthenticationRequired(component, {
        onRedirecting: () => (
            <div className='page-layout'>
                <Loading/>
            </div>
        ),
    })
    return <Component/>
}