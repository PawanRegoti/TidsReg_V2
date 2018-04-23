import {createStore} from 'redux';

const initialState = {
	empNr: '',
	auth: '',
	LoggedIn: false,
	projectList: []
};

const reducer = (state = initialState, action) => {
	console.log('reducer');
	switch (action.type){
		case 'empNr':
		return Object.assign({},state,{empNr: action.value});
		case 'auth':
		return Object.assign({},state,{auth: action.value});
		case 'LoggedIn':
		return Object.assign({},state,{LoggedIn: action.value});
		case 'projectList':
		return Object.assign({},state,{projectList: [...action.value]});
		case 'LogOut':
		return Object.assign({},state,{
			LoggedIn: false,
			auth: '',
			empNr: ''});
		default:
		return state;
	}
}

const Store = createStore(reducer);

export default Store;