const initialState = {
    markers: [],
};

export const reducerConstants = {
    "UPDATE_MARKER": "UPDATE_MARKER",

    "HUB_CONNECT": "HUB_CONNECT",
    "HUB_DISCONNECT": "HUB_DISCONNECT",
}

export const reducerActions = {
    updateMarker: station => ({ 
        type: reducerConstants.UPDATE_MARKER,
        station,
    }),

    connectToHub: () => ({type: reducerConstants.HUB_CONNECT}),
    disconnectFromHub: () => ({type: reducerConstants.HUB_DISCONNECT}),
}

export const reducer = function(state = initialState, action) {
    switch(action.type) {
        case  reducerConstants.UPDATE_MARKER:
            const { station_name, lat, lng, bikes_available} = action.station;
            const newMarkers = [...state.markers];
            
            const markerToBeUpdated = newMarkers.find(marker => marker.station_name === station_name);
            
            if(markerToBeUpdated) {
                markerToBeUpdated.bikes_available= bikes_available;
            } else {
                newMarkers.push({station_name, lat, lng, bikes_available});
            };
            
            return {
                ...state,
                markers: newMarkers,
            };
        default:
            return state;
    }
}