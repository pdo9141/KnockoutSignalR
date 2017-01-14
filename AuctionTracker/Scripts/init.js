/// <reference path="require.js" />

$appViewModel = null;
require(['knockout-3.3.0', 'newModels', 'appViewModel', 'domReady!'],
	function (ko, appViewModel) { ko.applyBindings(new appViewModel()); });