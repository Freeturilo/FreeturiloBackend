import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { reducerActions } from './reducer';

export const HubProvider = ({ children }) => {
    const dispatch = useDispatch();
    const connected = useSelector(state => state.connected);

    useEffect(() => {
        dispatch(reducerActions.connectToHub());
        return () => {
            dispatch(reducerActions.disconnectFromHub())
        };
    }, [dispatch]);

    return (
        <div>
            {connected && children}
        </div>
    )
}