import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import {connect} from 'react-redux';

import Header, { Footer } from '../Layout/Layout';

class LoginModule extends Component {
    constructor(props){
       super(props);
       this.state = {
        username: '',
        password: '',
        regUsername: '',
        regPassword: '',
        regRePassword: ''
    };
    this.onLogin = this.onLogin.bind(this);
    this.onRegister = this.onRegister.bind(this);
};  

onRegister(){
    console.log('onRegister'); 
    var httpPostRequest = {  
  method: 'POST',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
    'Origin': '',
  },
  body: JSON.stringify({
    'username': this.state.regUsername,
    'password': this.state.regPassword
  })}; 

  fetch('http://localhost:57227/api/TidsReg/Register',httpPostRequest)
  .then(response => { if (response.ok){alert('Registration Successful.')}})
  .catch(err => { alert('Unable to register.'+ err); }) 
}

onLogin(){
    console.log('onLogin');
    console.log(this.state.username+':'+this.state.password);

    var auth = 'Basic '+btoa(this.state.username+':'+this.state.password);

    fetch('http://localhost:57227/api/TidsReg/FetchProjectList', 
    {  
      method: 'GET', 
      headers: 
      {
        'Authorization': auth, 
        'Accept': 'Application/json',
        'Content-Type': 'application/json',
        'Origin': ''
      }
    })
    .then(response => 
        {
            if (response.ok) 
            {
                return response.json();
            }
        })
    .then(results => {
        console.log('login results.', results);
        this.props.dispatchAction('empNr',this.state.username);
        this.props.dispatchAction('auth',auth);
        this.props.dispatchAction('LoggedIn',true);
        this.props.dispatchAction('projectList', results);
    })
.catch(err => alert('Login failed: '+err))
}

render() {
    console.log('render');
    if(this.props.LoggedIn)
        return <Redirect to='/TimeLog'/>;
    else
        return (
            <div>
            <Header/>
            <div align="center">
            <table style={{width:'100%'}}>
            <tbody>
            <tr>
            <td>
            <div align="center">
            <div>

            <div><h4>{this.props.empNr}</h4></div>
            <ul className="centerlist">
            <li style={{fontSize:'x-large'}}>Username</li>
            <li>
            <input id="username" className="textbox" onChange={(event) => {this.setState({username: event.target.value})}}/>
            </li><br/>

            <li style={{fontSize:'x-large'}}>Password</li>
            <li>
            <input id="password" type="password" className="textbox" onChange={(event) => {this.setState({password: event.target.value})}}/>
            </li><br/>

            <li>
            <button onClick={this.onLogin} className="button" style={{fontSize:'xx-large'}} value="Submit">Login</button> 
            </li>
            <div>
            </div>
            </ul>
            </div>
            </div>
            </td>
            <td>
            <div align="center">
            <ul className="centerlist">
            <li style={{fontSize:'x-large'}}>Username</li>
            <li>
            <input className="textbox" onChange={(event) => {this.setState({regUsername: event.target.value})}}/>
            </li><br/>

            <li style={{fontSize:'x-large'}}>Password</li>
            <li>
            <input type="password" className="textbox" onChange={(event) => {this.setState({regPassword: event.target.value})}}/>
            </li><br/>

            <li style={{fontSize:'x-large'}}>Repeat Password</li>
            <li>
            <input type="password" className="textbox" onChange={(event) => {this.setState({regRePassword: event.target.value})}}/>
            </li><br/>

            <li>
            <button onClick={this.onRegister} className="button" style={{fontSize:'xx-large'}}>Register</button> 
            </li>
            </ul>
            </div>
            </td>
            </tr>
            </tbody>
            </table>
            </div>
            <Footer />
            </div>);
}
}
function mapStateToProps(state){
    console.log('mapStateToProps',state);
    return {
        empNr: state.empNr,
        auth: state.auth,
        LoggedIn: state.LoggedIn,
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


        export default connect(mapStateToProps, mapDispatchToProps)(LoginModule);
