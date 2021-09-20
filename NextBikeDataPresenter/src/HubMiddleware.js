import * as signalR from "@microsoft/signalr";
import { reducerActions, reducerConstants } from "./reducer";

export const hubMiddleware = () => {
    let hub = null;
  
    return store => next => async action => {
      switch (action.type) {
        case reducerConstants.HUB_CONNECT:
          if (hub !== null) {
            hub.close();
          }  

          hub = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5001/nextBike", {
              skipNegotiation: true,
              transport: signalR.HttpTransportType.WebSockets,
            })
            .build();

          hub.on("updateStation", (station_name, lat, lng, bikes_available) => {
             store.dispatch(reducerActions.updateMarker({station_name, lat, lng, bikes_available}))
          });

          await hub.start();

          break;
        case reducerConstants.HUB_DISCONNECT:
          if (hub !== null) {
            hub.stop();
          }
          hub = null;

          break;
        default:
          return next(action);
      }
    };
  };