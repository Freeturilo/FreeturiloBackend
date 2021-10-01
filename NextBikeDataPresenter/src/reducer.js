const initialState = {
    markers: [],
    loading: true,
    connected: false,
};

export const reducerConstants = {
    "UPDATE_MARKER": "UPDATE_MARKER",
    "SET_ALL": "SET_ALL",
    "ON_CONNECTED": "ON_CONNECTED",

    "GET_ALL": "GET_ALL",

    "HUB_CONNECT": "HUB_CONNECT",
    "HUB_DISCONNECT": "HUB_DISCONNECT",
}

export const reducerActions = {
    updateMarker: station => ({ 
        type: reducerConstants.UPDATE_MARKER,
        station,
    }),
    setAll: stations => ({
        type: reducerConstants.SET_ALL,
        stations,
    }),
    onConnected: () => ({type: reducerConstants.ON_CONNECTED}),

    getAll: () => ({type: reducerConstants.GET_ALL}),

    connectToHub: () => ({type: reducerConstants.HUB_CONNECT}),
    disconnectFromHub: () => ({type: reducerConstants.HUB_DISCONNECT}),
}

export const reducer = function(state = initialState, action) {
    switch(action.type) {
        case  reducerConstants.UPDATE_MARKER:
            const { Id, Name, Lat, Lng, AvailableBikes} = action.station;
            const newMarkers = [...state.markers];
            
            const markerToBeUpdated = newMarkers.find(marker => marker.Id === Id);
            
            if(markerToBeUpdated) {
                markerToBeUpdated.AvailableBikes= AvailableBikes;
            } else {
                newMarkers.push({Id, Name, Lat, Lng, AvailableBikes});
            };
            return {
                ...state,
                markers: newMarkers,
            };
        case reducerConstants.ON_CONNECTED:
            return {
                ...state,
                connected: true,
            }
        case reducerConstants.SET_ALL:
            return {
                ...state,
                loading: false,
                markers: action.stations,
            }
        default:
            return state;
    }
}