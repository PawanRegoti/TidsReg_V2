import React from 'react';
import ReactDOM from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import { Provider } from 'react-redux';
import './index.css';

import Routes from './Routes';
import registerServiceWorker from './registerServiceWorker';
import Store from './Store/Store';

ReactDOM.render(
	<Provider store={Store}>
	<BrowserRouter>
	<Routes/>
	</BrowserRouter>
	</Provider>
, document.getElementById('root'));
registerServiceWorker();
