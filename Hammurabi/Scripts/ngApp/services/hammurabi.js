(function () {
    angular.module('Hammurabi')
        .service('hammurabiService', function ($http, $routeParams, $location) {
            var self = this;

            var gameId = $routeParams.gameId;
            if (!gameId) {
                $.ajax({
                    url: '/api/hammurabi',
                    method: 'GET',
                    async: false
                }).success(function (results) {
                    gameId = results;
                });
                $location.path('/' + gameId);
            }

            var data = {};

            $http.get('/api/hammurabi/' + gameId)
                .success(function (results) {
                    data = results;
                });

            var delegateProp = function (propName) {
                Object.defineProperty(self, propName, {
                    get: function () {
                        return data[propName];
                    }
                });
            };
            delegateProp('landPrice');
            delegateProp('bushels');
            delegateProp('peasants');
            delegateProp('acres');
            delegateProp('harvest');
            delegateProp('deaths');
            delegateProp('migrants');
            delegateProp('year');

            self.validate = function (acreExchange, feed, plant) {
                if (acreExchange * self.landPrice + feed + plant > self.bushels) {
                    return "I regret to inform you you do not have enough bundles of food available.";
                }
                if (plant > self.acres + acreExchange) {
                    return "You do not have enough acres to plant that much food!";
                }
                if (plant > self.peasants * 10) {
                    return "There are not enough peasants to plant that many acres!";
                }
                if (acreExchange < 0 && self.acres + acreExchange < 0) {
                    return "We cannot sell more acres than we own";
                }
            };

            self.playTurn = function (acreExchange, feed, plant) {
                $http.post('/api/hammurabi/' + gameId, {
                    acreExchange: acreExchange,
                    feed: feed,
                    plant: plant
                }).success(function (results) {
                    data = results;
                });
            };
        });
})();