(function(){

    var app = angular.module("timeKeeper");

    app.controller("projectHistoryController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            ];

            $scope.year = 0;
            $scope.monthName = 0;

            $scope.buildProjectHistory = function()
            {
                $scope.showOutput = true;
                listInformation($scope.projectId);
            };

            dataService.list("projects/all/", function(data){
                $scope.projects = data;
            });

            function listInformation(projectId) {
                var url = "reports/projecthistory?";
                if(projectId != 'undefined') url += "projectId=" + projectId;
                dataService.list(url, function(data){
                    $scope.project = data;
                    //console.log("projects", $scope.project);
                    console.log($scope.project.totalHours);
                    $scope.beginYear = new Date(data.beginDate).getFullYear();
                    $scope.beginMonth = $scope.monthNames[new Date(data.beginDate).getMonth()];
                    $scope.endMonth = $scope.monthNames[new Date(data.endDate).getMonth()];
                    $scope.endYear = new Date(data.endDate).getFullYear();

                });
            };
        }]);
}());