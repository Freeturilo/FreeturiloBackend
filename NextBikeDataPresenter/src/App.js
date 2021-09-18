import * as signalR from '@microsoft/signalr';
import './App.css';

function App() {
  var connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/nextBike",{
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  })
  .build();

  connection.on("UpdateStation", console.log);

  connection.start()
  .then(() => console.log("starrted"));

  return (
    <div className="App">
      <button onClick={() => {
        connection.send("UpdateStation", "Station Rakowiec", 123);
      }}>
        Button
      </button>
    </div>
  );
}

export default App;
