(function(){

    var app = angular.module("timeKeeper");

    app.controller("monthlyReportController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];

            $scope.year = 0;
            $scope.monthName = 0;


            $scope.buildMonthlyReport = function()
            {
                $scope.showOutput = true;
                listAscendingEmployeeHours($scope.year, $scope.month);
                listDescendingEmployeeHours($scope.year, $scope.month);
                listProjectHours($scope.year, $scope.month);
            };

            function getCol(matrix, col){
                var column = [];
                for(var i=0; i<matrix.length; i++){
                    column.push(matrix[i][col]);
                }
                return column;
            }

            function listAscendingEmployeeHours(year, month) {
                var url = "reports/monthlyreport?";
                if(year != 'undefined') url += "year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    var employees = [];
                    data.items.forEach(function(element)
                    {
                        employees.push([element.total, element.employee]);
                    });

                    employees.sort(function(a, b){
                        return b[0] - a[0];
                    });

                    employees.length = 10;
                    var hours = getCol(employees, 0);
                    var names = getCol(employees, 1);

                    $scope.label2 = names;
                    $scope.data2 = hours;

                });
            };

            function listDescendingEmployeeHours(year, month) {
                var url = "reports/monthlyreport?";
                if(year != 'undefined') url += "year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    var employees = [];
                    data.items.forEach(function(element)
                    {
                        employees.push([element.total, element.employee]);
                    });

                    employees.sort(function(a, b){
                        return a[0] - b[0];
                    });

                    employees.length = 10;
                    var hours = getCol(employees, 0);
                    var names = getCol(employees, 1);

                    $scope.label3 = names;
                    $scope.data3 = hours;

                });
            };

            function listProjectHours(year, month) {
                var url = "reports/monthlyreport?";
                if(year != 'undefined') url += "year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    var projects = data.projects;
                    $scope.totalHours = data.total;
                    $scope.averagePerEmployee = Math.round($scope.totalHours / data.items.length, 2);

                    console.log(data);
                    var projectName = [];
                    var projectHours = [];

                    projects.forEach(function(element){
                        projectName.push(element.project);
                        projectHours.push(element.hours);
                    });

                    $scope.label4 = projectName;
                    $scope.data4 = projectHours;

                });
            };


        }]);
}());