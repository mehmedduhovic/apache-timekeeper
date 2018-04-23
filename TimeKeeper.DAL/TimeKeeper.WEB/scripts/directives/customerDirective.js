(function(){
    var app = angular.module("timeKeeper");

    app.directive("customer", [function() {
        return {
            restrict: 'E',
            scope: {
                data: '='
            },
            controller: 'custController as $cust',
            templateUrl: 'views/custWidget.html'
        }
    }]);
}());