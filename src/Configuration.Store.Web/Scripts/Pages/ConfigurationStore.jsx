import ConfigurationList from './ConfigurationList'

var data = JSON.parse(jQuery("#configKeys").html());

var element = React.createElement(ConfigurationList, data);
var rootElement = document.getElementById('root');

ReactDOM.render(element, rootElement);