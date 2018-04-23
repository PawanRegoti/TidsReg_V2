import React, {Component} from 'react';
import {connect} from 'react-redux';
import { Route, Redirect, withRouter } from 'react-router-dom';

import LoginModule from './LoginModule/LoginModule';
import TimeLogModule from './TimeLogModule/TimeLogModule';
import ProjectModule from './ProjectModule/ProjectModule'

const ProtectedRoute = ({ component: Comp, loggedIn, path, ...rest }) => {
  return (
    <Route
      exact
      path={path}
      {...rest}
      render={props => {
        return loggedIn ? <Comp {...props} /> : <Redirect to="/" />;
      }}
    />
  );
};

class RoutePage extends Component{

	render(){
		return (
			<div>	
			<Route exact path= '/'  component= {LoginModule} />
			<Route path= '/Login'  component= {LoginModule} />
			<Route path= '/TimeLog'  component= {TimeLogModule} />
			<Route path= '/Project'  component= {ProjectModule} />
			{/*<ProtectedRoute path='/TimeLog' loggedIn={this.props.loggedIn} component={TimeLogModule} />
			<ProtectedRoute path='/ProjectOverview' loggedIn={this.props.loggedIn} component={ProjectModule} />*/}
			</div>

			)};
	}

function mapStateToProps(state){
		console.log('mapStateToProps',state);
		return {
			auth: state.auth,
			projectList: state.projectList
		}
	}

export default withRouter(connect(mapStateToProps)(RoutePage));
