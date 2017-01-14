/// <reference path="knockout-3.3.0.js" />

define(['knockout-3.3.0', 'newModels'], function (ko, models) {
	ko.components.register('display-team', {
		template: { require: 'text!/Scripts/Templates/team.html' },
		viewModel: function(params){
			this.team = ko.observable(params.team);
		}
	});

	return function appViewModel() {
		var self = this;

		self.teams = ko.observableArray([]);
		$.getJSON('/api/Teams/', function (teams) {
			var mappedTeams = $.map(teams, function (team) { return new models.Team(team); });
			self.teams(mappedTeams);
		}).done(function () {
			$.getJSON('/api/Teams/Players',
			function (teams) {
				teams.forEach(function (team) {
					var players = team.players;
					self.teams().forEach(function (tempTeam) {
						if (tempTeam.id() == team.id) {
							team = tempTeam;
						}
					});
					players.forEach(function (player) {
						team.addPlayer(new Player(player));
					});
				})
			})
		});
	};
});
