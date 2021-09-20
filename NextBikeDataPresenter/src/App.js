import React from 'react';
import Map from './Map';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import { reducer } from './reducer';
import { HubProvider } from './HubProvider';
import { hubMiddleware } from './HubMiddleware';

const store = createStore(
  reducer,
  applyMiddleware(hubMiddleware()),
);

function App() {
  return (
    <Provider store={store}>
      <HubProvider>
        <Map/>
      </HubProvider>
    </Provider>
  )
}

export default App;
