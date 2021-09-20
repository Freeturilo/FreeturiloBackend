import React from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { useSelector} from 'react-redux';

function Map() {
  const markers = useSelector(state => state.markers);
  return (
    <div>
      <MapContainer id="mapid" center={[52.24, 21]} zoom={14} style={{height: "970px"}}>
        <TileLayer
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {
          markers.map(marker => (
            <Marker key={marker.station_name} position={[marker.lng, marker.lat]}>
              <Popup>
                {marker.station_name} <br/>
                {marker.bikes_available}
              </Popup>
            </Marker>
          ) )
        }
      </MapContainer>
    </div>
  );
}

export default Map;
