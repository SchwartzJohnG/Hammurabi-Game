(function () {
    angular.module("Hammurabi", ['ngRoute', 'ui.bootstrap', 'ngResource', 'ngAnimate'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
            /*
                .when('url/:urlParam1', {
                    templateUrl: '/url/to/template.html',
                    controller: 'BlahController',
                    controllerAs: 'vm'
                })
            */
                .when('/:gameId?', {
                    templateUrl: '/ngViews/hammurabi.html',
                    controller: 'HammurabiController',
                    controllerAs: 'vm'
                })
                .otherwise({
                    templateUrl: '/ngViews/notFound.html'
                });
        }]);
})();