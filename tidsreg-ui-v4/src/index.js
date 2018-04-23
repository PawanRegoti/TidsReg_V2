import React from 'react';
import ReactDOM from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import { Provider } from 'react-redux';
import './index.css';

import RoutePage from './RoutePage';
import registerServiceWorker from './registerServiceWorker';
import Store from './Store/Store';

ReactDOM.render(
	<Provider store={Store}>
	<BrowserRouter>
	<RoutePage/>
	</BrowserRouter>
	</Provider>
, document.getElementById('root'));
registerServiceWorker();
