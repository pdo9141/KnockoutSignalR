/// <reference path="knockout-3.3.0.js" />
/// <reference path="jquery-2.1.4.js" />
/// <reference path="jquery.signalR-2.2.0.js" />


function RosterPosition(id, position, minNumber) {
	var self = this;
	self.id = id;
	self.position = ko.observable(position);
	self.number = ko.observable(0);
	self.minNumber = ko.observable(minNumber);
}

function Player(data) {
	var self = this;

	if (!data) data = { id: 0, fullName: '', nflTeam: '', position: '', auctionAmount: 0, team: '', teamId: 0 };
	self.id = ko.observable(data.id);
	self.fullName = ko.observable(data.fullName);
	self.nflTeam = ko.observable(data.nflTeam);
	self.position = ko.observable(data.position);
	self.auctionAmount = ko.observable(data.auctionAmount);
	self.teamId = ko.observable(data.teamId);
	self.team = ko.observable(data.team);
	self.sortNumber = function () {
		switch (self.position()) {
			case 'QB': return 1; break;
			case 'RB': return 2; break;
			case 'WR': return 3; break;
			case 'TE': return 4; break;
			case 'K': return 5; break;
			case 'D': return 6; break;
		}
	}
}

function NflTeam(data) {
	var self = this;
	self.id = ko.observable(data.id);
	self.name = ko.observable(data.name);
}

function Team(data) {
	var self = this;
	self.id = ko.observable(data.id);
	self.owner = ko.observable(data.owner);
	self.name = ko.observable(data.name);
	self.lastBid = ko.observable(new Date(data.lastBid));

	self.players = ko.observableArray([]);

	self.rosterPositions = ko.observableArray([
		new RosterPosition(1, 'QB', 1),
		new RosterPosition(2, 'RB', 2),
		new RosterPosition(3, 'WR', 3),
		new RosterPosition(4, 'TE', 1),
		new RosterPosition(5, 'K', 1),
		new RosterPosition(6, 'D', 1),
	]);

	self.addPlayer = function (player) {
		player.team(self.name());
		player.teamId(self.id());
		self.players.unshift(player);

		self.resetRosterPositions();

		self.totalLeft(self.totalLeft() - player.auctionAmount());
	}

	self.removePlayer = function (player) {
		self.players.remove(function (currentPlayer) { return currentPlayer.id() == player.id(); });
		self.resetRosterPositions();

		self.totalLeft(self.totalLeft() + player.auctionAmount());
	}

	self.resetRosterPositions = function () {
		self.rosterPositions().forEach(function (rosterPosition) {
			rosterPosition.number(0);
			self.players().forEach(function (player) {
				if (player.position() == rosterPosition.position()) {
					rosterPosition.number(rosterPosition.number() + 1);
				}
			});
		});
	};

	self.totalLeft = ko.observable(100);
	self.maxBid = ko.pureComputed(function () { return self.totalLeft() - (16 - self.players().length) + 1; });
	self.rosterFull = ko.pureComputed(function () { return self.players().length >= 16 });
};

function AppViewModel() {
	var self = this;

	self.teams = ko.observableArray([]);
	self.nflTeams = ko.observableArray([]);
	self.recentlyAuctionedPlayers = ko.observableArray([]);

	self.auctionOrder = ko.pureComputed(function () {
		var sorted = self.teams().sort(function (left, right) {
			if (left.lastBid() > right.lastBid()) return 1;
			if (left.lastBid() < right.lastBid()) return -1;
			return 0;
		});
		var result = new Array();
		sorted.forEach(function (t) { if (!t.rosterFull()) result.push(t); });
		return result;
	});
	
	self.rosterPositions = ko.observableArray([
		new RosterPosition(1, 'QB', 1),
		new RosterPosition(2, 'RB', 2),
		new RosterPosition(3, 'WR', 3),
		new RosterPosition(4, 'TE', 1),
		new RosterPosition(5, 'K', 1),
		new RosterPosition(6, 'D', 1),
	]);

	//self.displayPlayers = ko.pureComputed(function () {
	//	return self.currentTeam().players.sort(function (l, r) {
	//		if (l.rosterPosition().id > r.rosterPosition().id) return 1;
	//		if (l.rosterPosition().id > r.rosterPosition().id) return -1;
	//		return 0;
	//	});
	//});

	self.currentTeam = ko.observable();
	self.currentPlayer = ko.observable(new Player(null));
	self.originalPlayer = ko.observable(new Player(null));
	self.isNewPlayer = ko.observable(true);

	self.displayTeam = function (team) {
		self.currentTeam(team);
	}

	self.resetPlayer = function () {
		self.currentPlayer(new Player(null));
		self.originalPlayer(new Player(null));
		self.isNewPlayer(true);
	}

	self.handlePlayer = function () {
		if (self.isNewPlayer()) self.auctionPlayer();
		else self.updatePlayer();
	}

	self.auctionPlayer = function () {
		if(self.validateMove()) $playerHub.server.auctionPlayer(ko.toJS(self.currentPlayer)).fail(function (error) { alert(error) });
	}

	self.auctionedPlayer = function (player) {
		self.resetPlayer();
		player = new Player(player);
		var team = self.findTeam(player.teamId());
		self.findTeam(ko.toJS(self.auctionOrder()[0].id)).lastBid(new Date());
		team.addPlayer(player);

		self.recentlyAuctionedPlayers.unshift(player);
		if (self.recentlyAuctionedPlayers().length > 3) self.recentlyAuctionedPlayers.pop();
	}
	
	self.displayPlayer = function (player) {
		self.originalPlayer(player);
		self.currentPlayer(new Player(ko.toJS(player)));
		self.isNewPlayer(false);
	}

	self.updatePlayer = function () {
		if (self.validateMove()) $playerHub.server.updatePlayer(ko.toJS(self.currentPlayer), ko.toJSON(self.originalPlayer().teamId));
	}

	self.playerUpdated = function (player, originalTeamId) {
		self.resetPlayer();
		player = new Player(player);
		self.recentlyAuctionedPlayers().forEach(function (recentPlayer) {
			if (ko.toJSON(recentPlayer.id) == player.id()) {
				self.recentlyAuctionedPlayers.replace(recentPlayer, player);
			}
		});

		self.findTeam(originalTeamId).removePlayer(player);
		self.findTeam(player.teamId()).addPlayer(player);
	}

	self.deletePlayer = function () {
		$playerHub.server.deletePlayer(ko.toJS(self.currentPlayer));
	}

	self.playerDeleted = function (player) {
		player = new Player(player);
		var team = self.findTeam(ko.toJS(player.teamId()));
		team.removePlayer(player);
		self.recentlyAuctionedPlayers.remove(function (currentPlayer) { return ko.toJSON(currentPlayer.id) == player.id() });
	}

	self.validateMove = function () {
		var validMove = true;
		var team = self.findTeam(self.currentPlayer().teamId());
		if (team.players().length >= 16) {
			alert(team.name() + ' would have 17 players');
			validMove = false;
		}
		if (team.maxBid() < self.currentPlayer().auctionAmount()) {
			alert(team.name() + ' only has $' + team.maxBid() + ' remaining.')
			validMove = false;
		}
		if (self.currentPlayer().auctionAmount() <= 0) {
			alert('Must bid at least $1');
			validMove = false;
		}
		return validMove;
	}

	self.findTeam = function (id) {
		var team;
		self.teams().forEach(function (tempTeam) {
			if (tempTeam.id() == id) {
				team = tempTeam;
			}
		});
		return team;
	}
}


var appViewModel = new AppViewModel();

var $playerHub;
$(function () {
	ko.components.register('display-player', {
		template: { element: 'player-template' },
		viewModel: function (params) {
			this.players = params.players;
		}
	});
	ko.applyBindings(appViewModel);


	$.getJSON('/api/Teams/', function (teams) {
		var mappedTeams = $.map(teams, function (team) { return new Team(team); });
		appViewModel.teams(mappedTeams);
	}).done(function () { $.getJSON('/api/Teams/Players',
		function (teams) {
			teams.forEach(function (team) {
				var players = team.players;
				appViewModel.teams().forEach(function (tempTeam) {
					if (tempTeam.id() == team.id) {
						team = tempTeam;
					}
				});
				players.forEach(function (player) {
					team.addPlayer(new Player(player));
				});
			})		
		})
	})
	//.done(function () {
	//	$.getJSON('/api/Teams/AuctionOrder',
	//	function(teams) {
	//		appViewModel.auctionOrder($.map(teams, function (team) { return new Team(team) }));
	//	})
	//});

	$.getJSON('/api/NflTeams/', function (nflTeams) {
		var mappedNflTeams = $.map(nflTeams, function (nflTeam) { return new NflTeam(nflTeam); });
		appViewModel.nflTeams(mappedNflTeams);
	})

	$.getJSON('/api/Players', function (players) {
		var mappedPlayers = $.map(players, function (player) { return new Player(player) });
		appViewModel.recentlyAuctionedPlayers(players);
	})
	
	//load hub
	$playerHub = $.connection.playerHub;
	$playerHub.client.playerAuctioned = appViewModel.auctionedPlayer;
	$playerHub.client.playerUpdated = appViewModel.playerUpdated;
	$playerHub.client.playerDeleted = appViewModel.playerDeleted;

	$.connection.hub.logging = true;
	$.connection.hub.start().done(function () {
	});
});

