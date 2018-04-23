import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import {connect} from 'react-redux';
import Grid from "react-json-grid";
import moment from 'moment';

class TimeLogGrid extends Component {
  constructor(props){
    super(props);

    this.state = {
      timeLog: [], 
      from: moment().startOf('isoWeek'),
      to: moment().endOf('isoWeek')
    };

    this.previousWeek = this.previousWeek.bind(this);
    this.nextWeek = this.nextWeek.bind(this);
    this.FetchTimeLog = this.FetchTimeLog.bind(this);
    this.UpdateTimeLog = this.UpdateTimeLog.bind(this);
    this.SaveTimeLog = this.SaveTimeLog.bind(this);
    this.GoToProjectOverview = this.GoToProjectOverview.bind(this);
    this.FetchTimeLog();
  };  


  previousWeek(){
    this.setState({
      from: this.state.from.add(-7, 'd'),
      to: this.state.to.add(-7, 'd')
    });
    console.log('previousWeek', this.state.from);
    this.FetchTimeLog();
  }

  nextWeek(){
    this.setState({
      from: this.state.from.add(7, 'd'),
      to: this.state.to.add(7, 'd')
    });
    console.log('previousWeek', this.state.from);
    this.FetchTimeLog();
  }

  
  FetchTimeLog() {

    var httpPostRequest = {  
      method: 'POST',
      headers: {
        'Authorization': this.props.auth,
        'Content-Type': 'application/json',
        'Origin': '',
      },
      body: JSON.stringify({
        'from': this.state.from.format('DD-MM-YYYY').toString(),
        'to': this.state.to.format('DD-MM-YYYY').toString()
      })}; 

      fetch('http://localhost:57227/api/TidsReg/FetchTimeLog',httpPostRequest)
      .then(response => { 
        if (response.ok){
          return response.json()
        }
      })
      .then(results => {
        this.setState({
          timeLog: results
        });
        console.log(this.state.timeLog);
      })
      .catch(err => { alert('Unable to fetch time log.'+ err); }) 
    }

    UpdateTimeLog(x,y,objKey,value){
      var gridData = this.state.timeLog;
      gridData[y][objKey]=value;
      this.setState({
        timeLog: gridData
      });
    }

    SaveTimeLog(){
      console.log(this.state.timeLog);

      var httpPostRequest = {  
        method: 'POST',
        headers: {
          'Authorization': this.props.auth,
          'Content-Type': 'application/json',
          'Origin': '',
        },
        body: JSON.stringify(this.state.timeLog)}; 

        fetch('http://localhost:57227/api/TidsReg/AddTimeLog',httpPostRequest)
        .then(response => { 
          if (response.ok){
            console.log('added timelog successfully');
            alert('Time Log saved successfully.');
          }
        })
        .catch(err => { alert('Unable to add time log.'+ err); }) 
      }


      GoToProjectOverview(){
        console.log('Statistics');
        
      }

      render() {
        return (
          <table style={{width: '100%'}}>
          <tbody>
          <tr>
          <td style={{width: '60%'}}>
          <div align="center">
          <input type="submit" name="Backward" value="<" id="Backward" className="button" style={{fontSize:'XX-Large',width:'70px'}} onClick={this.previousWeek}/>
          <input type="submit" name="Forward" value=">" id="Forward" className="button" style={{fontSize:'XX-Large',width:'70px'}} onClick={this.nextWeek}/>
          </div>
          <div align="center" style={{margin: '20px'}}>

          <Grid 
          padWide={10}
          styleHeaderCell={{ backgroundColor: '#013275' }} 
          styleHeaderData={{color: 'white'}} 
          styleCellOddRow={{ backgroundColor: '#cce1ff' }} 
          data={this.state.timeLog} 
          onChange={(x,y,objKey,value)=>{this.UpdateTimeLog(x,y,objKey,value);}}
          />
          </div>
          </td>
          <td style={{width: '40%'}}>
          <ul className="centerlist">
          <li>
          <input type="submit" name="Save" value="Save" id="Save" className="button" onClick={this.SaveTimeLog}/><br/>
          <input type="submit" name="Route" value="Route" id="Route" className="button" /><br/>
          </li>
          <li>
          <Link to='/ProjectOverview'><input type="submit" name="Statistics" value="Statistics / Invoices" id="Statistics" className="button" onClick={this.GoToProjectOverview}/></Link><br/>
          </li>
          </ul>
          </td>
          </tr>
          </tbody>
          </table>
          )
      }
    }
    function mapStateToProps(state){
      console.log('mapStateToProps',state);
      return {
        auth: state.auth,
        projectList: state.projectList
      }
    }

    export default connect(mapStateToProps)(TimeLogGrid);
