import * as signalR from "@microsoft/signalr";
import { reducerActions, reducerConstants } from "./reducer";

export const hubMiddleware = () => {
    let hub = null;
  
    return store => next => action => {
      switch (action.type) {
        case reducerConstants.HUB_CONNECT:
          if (hub !== null) {
            hub.close();
          }  

          hub = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:5001/nextBike", {
              skipNegotiation: true,
              transport: signalR.HttpTransportType.WebSockets,
            })
            .build();

          hub.on("updateStation", (stations) => {
             store.dispatch(reducerActions.setAll(stations));
          });

          hub.on("getAllStations", (stations) => {
            store.dispatch(reducerActions.setAll(stations));
          });
          
          hub.start().then(() => {
            store.dispatch(reducerActions.onConnected());
          });
          break;
        case reducerConstants.GET_ALL:
          hub.send("GetAllStations");
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