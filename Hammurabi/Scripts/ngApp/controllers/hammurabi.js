(function () {
    angular.module('Hammurabi')
        .controller('HammurabiController', function (hammurabiService) {
            var vm = this;

            vm.acreBuyOrSell = 1;
            vm.acreExchange = 0;
            vm.feed = 0;
            vm.plant = 0;

            var feedRatio;
            Object.defineProperty(vm, 'feedRatio', {
                get: function () {
                    return vm.feed / hammurabiService.peasants;
                },
                set: function (value) {
                    vm.feed = hammurabiService.peasants * value;
                }
            });
            var plantRatio;
            Object.defineProperty(vm, 'plantRatio', {
                set: function (value) {
                    var maxAcres = Math.min(vm.peasants * 10, vm.acres + vm.acreExchange * vm.acreBuyOrSell);
                    vm.plant = Math.floor(maxAcres * value);
                }
            });

            var delegateProp = function (propName) {
                Object.defineProperty(vm, propName, {
                    get: function () {
                        return hammurabiService[propName];
                    }
                });
            };
            delegateProp('landPrice');
            delegateProp('acres');
            delegateProp('peasants');
            delegateProp('bushels');
            delegateProp('deaths');
            delegateProp('migrants');
            delegateProp('harvest');
            delegateProp('year');

            vm.submit = function () {
                vm.error = hammurabiService.validate(vm.acreExchange * vm.acreBuyOrSell, vm.feed, vm.plant);
                if (!vm.error) {
                    hammurabiService.playTurn(vm.acreExchange * vm.acreBuyOrSell, vm.feed, vm.plant);
                    vm.acreExchange = 0;
                    vm.feed = 0;
                    vm.plant = 0;
                }
            }
        });
})();