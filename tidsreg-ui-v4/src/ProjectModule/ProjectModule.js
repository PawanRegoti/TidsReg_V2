import React, { Component } from 'react';
import {connect} from 'react-redux';
import { DateRange} from 'react-date-range';
import FusionCharts from 'fusioncharts';
import Charts from 'fusioncharts/fusioncharts.charts';
import ReactFC from 'react-fusioncharts';

import moment from 'moment';

import Header, {  Footer } from '../Layout/Layout';

class ProjectModule extends Component {
	constructor(props){
		super(props);
		Charts(FusionCharts);

		this.state = 
		{
			isDataFetched: false,
			selectedProject: this.props.projectList[0],
			projectData: [],
			chartData:[],
			dateRange: {
				endDate: moment(),
				startDate: moment(),
			}
		};

		this.CalenderUpdate = this.CalenderUpdate.bind(this);
		this.FetchData = this.FetchData.bind(this);
		this.CreateChart = this.CreateChart.bind(this);
		this.ExportData = this.ExportData.bind(this);

		this.DownloadObjectAsJson = this.DownloadObjectAsJson.bind(this);
	};  

	CalenderUpdate(ranges){

		console.log('range',ranges);
		this.setState({
			dateRange: ranges
		});
	}

	FetchData(){

		var httpPostRequest = {  
			method: 'POST',
			headers: {
				'Authorization': this.props.auth,
				'Content-Type': 'application/json',
				'Origin': '',
			},
			body: JSON.stringify({
				'project': this.state.selectedProject,
				'from': this.state.dateRange.startDate.format('DD-MM-YYYY').toString(),
				'to': this.state.dateRange.endDate.format('DD-MM-YYYY').toString()
			})}; 

			console.log('statistics', httpPostRequest.body);

			fetch('http://localhost:57227/api/TidsReg/FetchStatistics',httpPostRequest)
			.then(response => { 
				if(response.status === 204){
					alert('No time has been logged for given time period.')
				}
				else if (response.ok){
					return response.json()
				}
			})
			.then(results => {
				this.setState({
					isDataFetched: true,
					projectData: results
				});
				this.CreateChart();
				console.log(this.state.projectData);
			})
			.catch(err => { alert('Unable to fetch time log for given project for the given time period.'+ err); }) 

		}

		
		CreateChart(){
			if(this.state.isDataFetched){

				var myDataSource = {
					chart: {
						caption: 'Project '+this.state.selectedProject,
						subCaption: 'Hours Logged',
						theme: "ocean",
						xAxisName: "Days",
						"yAxisName": "Hours"
					},
					data: this.state.projectData.map((timelog) => 						
					{
						return (
						{
							label: timelog.WorkDay,
							value: timelog.TimeLog
						});
					}
					)
				};

				var chartConfigs = {
					id: "TimeLog-chart",
					type: "column2d",
					width: 500,
					height: 400,
					dataFormat: "json",
					dataSource: myDataSource
				};

				this.setState({chartData: chartConfigs});
			}
		}

		ExportData(){
			this.DownloadObjectAsJson(this.state.projectData, 'JsonData');
		}

		DownloadObjectAsJson(exportObj, exportName){
			var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(exportObj));
			var downloadAnchorNode = document.createElement('a');
			downloadAnchorNode.setAttribute("href",     dataStr);
			downloadAnchorNode.setAttribute("download", exportName + ".json");
			downloadAnchorNode.click();
			downloadAnchorNode.remove();
		}

		render() {
			return (
				<div>
				<Header/>
				<div align="center">
				<table style={{width: '100%'}}>
				<tbody>
				<tr>
				<td style={{width: '30%'}}>
				<div>
				<select name="ProjectDropDown" id="ProjectDropDown" className="button" onChange={(event) => { this.setState({selectedProject: event.target.value});}}>
				{this.props.projectList.map((option, index) => 
					<option key={index} value={option}>{option}</option>)}

				</select>

				<div style={{margin: '20px'}}>
				<DateRange
				calendars= '1'
				onInit= {this.CalenderUpdate}
				onChange=
				{this.CalenderUpdate}
				/>

				</div>
				</div>
				</td>

				<td style={{width: '70%'}} align="right">
				<div align="center"> 
				<ReactFC {...this.state.chartData} />
				</div>
				</td>
				</tr>
				<tr>
				<td style={{width: '30%', alignContent: 'center'}}>
				<div>
				<input type="submit" name="CreateGrid" value="Go" id="CreateGrid" className="button" style={{width:'100px'}} onClick={this.FetchData}/>
				</div>
				</td>

				<td style={{width: '70%', margin: '50px'}} align="right">
				<div>
				<input type="submit" name="Export" value="Export" id="Export" className="button" onClick={this.ExportData}/>
				</div>
				</td>
				</tr>
				</tbody>
				</table>
				</div>

				<Footer />			
				</div>
				);
		}
	}

	function mapStateToProps(state){
		console.log('mapStateToProps',state);
		return {
			auth: state.auth,
			projectList: state.projectList
		}
	}

	export default connect(mapStateToProps)(ProjectModule);
