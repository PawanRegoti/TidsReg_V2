import React, { Component } from 'react';

import Header,{ Footer } from '../Layout/Layout';
import AddNewProject from './AddNewProject';
import TimeLogGrid from './TimeLogGrid';

class TimeLogModule extends Component {

	render() {
		return (
			<div>
			<Header/>
			<div align="center">
			<table style={{width: '100%'}}>
			<tbody>
			<tr>
			<td style={{width: '20%'}}>
			<AddNewProject />
			</td>
			<td style={{width: '80%'}}>
			<TimeLogGrid/>
			</td>
			</tr>
			</tbody>
			</table>
			</div>
			<Footer />
			</div>);
		}
	}


	export default TimeLogModule;
