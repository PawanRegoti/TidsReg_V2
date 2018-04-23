import React, { Component } from 'react';
import {connect} from 'react-redux';

class AddNewProject extends Component {
	constructor(props){
		super(props);
		this.state = {
			newProject: ''
		};
		this.addNewProjectClick = this.addNewProjectClick.bind(this);
	};  

	addNewProjectClick(){
		console.log('addNewProjectClick'); 
		var httpGetRequest = {  
			method: 'GET',
			headers: {
				'Authorization': this.props.auth,
				'Accept': 'application/json',
				'Content-Type': 'application/json',
				'Origin': '',
			}}; 

			fetch('http://localhost:57227/api/TidsReg/AddNewProject/'+this.state.newProject,httpGetRequest)
			.then(response => { 
				if (response.ok)
				{
					var newProjectList = this.props.projectList.slice();
					newProjectList.push(this.state.newProject);   
					this.props.dispatchAction('projectList',newProjectList);


					alert('Successful added new Project: '+this.state.newProject);
				}
				else{
					alert('Unable to connect to server');
				}
				})

			.catch(err => { alert('Unable to added new Project:'+ err); }) 
		}

		render() {
			return (
				<div>
				<ul className="centerlist">
				<li style={{fontSize: 'x-large'}}>Add New Project</li>
				<li>
				<input name="NewProject" type="text" id="NewProject" className="textbox" style={{fontSize:'Large'}} onChange={(event) => {this.setState({newProject: event.target.value})}}/><br />
				</li>
				<li>
				<input type="submit" name="AddNewProject" value="Add New Project" id="AddNewProject" className="button" style={{fontSize:'XX-Large'}} onClick={this.addNewProjectClick}/>
				</li>
				</ul>
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

	function mapDispatchToProps(dispatch){
		console.log('mapDispatchToProps');

		return {
			dispatchAction:(type,value)=>{
				var action = {type: type, value:value};
				dispatch(action)}}
			}


			export default connect(mapStateToProps, mapDispatchToProps)(AddNewProject);
