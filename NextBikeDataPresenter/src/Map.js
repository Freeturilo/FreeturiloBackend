import React, { useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { useDispatch, useSelector} from 'react-redux';
import { reducerActions } from './reducer';

function Map() {
  const markers = useSelector(state => state.markers);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(reducerActions.getAll());
  }, [dispatch]);

  return (
    <div>
      <MapContainer id="mapid" center={[52.24, 21]} zoom={14} style={{height: "970px"}}>
        <TileLayer
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        { markers.map(marker => (
            <Marker key={marker.id} position={[marker.lng, marker.lat]}>
              {marker.availableBikes}
              <Popup>
                {marker.name} <br/>
                Available bikes: {marker.availableBikes}
              </Popup>
            </Marker>
          ) )
        }
      </MapContainer>
    </div>
  );
}

export default Map;
