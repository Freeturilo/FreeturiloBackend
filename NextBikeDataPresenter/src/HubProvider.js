import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { reducerActions } from './reducer';

export const HubProvider = ({ children }) => {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(reducerActions.connectToHub());
        return () => {
            dispatch(reducerActions.disconnectFromHub())
        };
    }, [dispatch]);

    return (
        <div>
            {children}
        </div>
    )
}