import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import {connect} from 'react-redux';

class Header extends Component {
constructor(props){
  super(props);
  this.LogOff = this.LogOff.bind(this);
}

LogOff(){
  this.props.dispatchAction('LogOut','');
}

  render() {
    return (
      <div>
        <div className="header">
            <table id="HeadTable" className="header" align="Center">
            <tbody>
              <tr>
                <td align="center" style={{width:'30%'}} className="header">
                  <Link to='/TimeLog'>
                  <input type="submit" name="Home" value="TidsReg" id="Home" disabled={!this.props.LoggedIn} className="button" style={{fontSize:'XX-Large'}} onClick={this.clickHandler}/>
                  </Link>
                </td>
              <td align="center" style={{width:'70%'}}>
                
              </td>
              <td style={{width: '10%'}}>
              <Link to='/'>
              <input type="submit" name="LogOff" value="Log Off" id="LogOff" className="button" style={ this.props.LoggedIn ? {display: 'inherit'}: {display: 'none'} } onClick={this.LogOff}/>
              </Link>
              </td>
            </tr>
          </tbody>
          </table>
        </div>
        <div id="errorComponent" className="error"></div>
      </div>);
  }
}

function mapStateToProps(state){
    console.log('mapStateToProps',state);
    return {
        LoggedIn: state.LoggedIn
    }
}

function mapDispatchToProps(dispatch){
    console.log('mapDispatchToProps');
    
    return {
        dispatchAction:(type,value)=>{
            var action = {type: type, value:value};
            dispatch(action)}}
        }

export default connect(mapStateToProps, mapDispatchToProps)(Header)

export class Footer extends Component {
  render() {
    return (<div className="footer" align="center">A Time Registration Website</div>);
  }
}
