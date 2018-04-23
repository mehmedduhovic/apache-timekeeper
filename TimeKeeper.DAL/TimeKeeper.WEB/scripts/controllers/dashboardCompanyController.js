(function(){

    var app = angular.module("timeKeeper");

    app.controller("dashboardCompanyController", ["$scope", "dataService", "timeConfig", function($scope, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];

            $scope.year = 0;
            $scope.monthName = 0;

            var url = "reports/company/2017/6";

            $scope.buildCompanyDashboard = function()
            {
                url = "reports/company/"+ $scope.year +"/" + $scope.month;
                buildCompanyDashboardCharts(url);
            };

            function buildCompanyDashboardCharts(url)
            {
                dataService.list(url, function(data){
                    $scope.report = data;
                    console.log($scope.report);

                    $scope.label2 =["Total Possible Hours", "Total Hours Worked"];
                    $scope.data2 = [$scope.report.maxPossibleTotalHours, $scope.report.totalHours];

                    var overtimeEmployeesNames = [];
                    var overtimeEmployeesHours = [];
                    $scope.report.overtimeEmployees.forEach(function(element){
                        overtimeEmployeesNames.push(element.name);
                        overtimeEmployeesHours.push(element.sumHours);
                    });
                    $scope.label3 = overtimeEmployeesNames;
                    $scope.data3 =overtimeEmployeesHours;

                    var overtimeTeamName = [];
                    var overtimeTeamHours = [];
                    $scope.report.overtimeHoursTeams.forEach(function(element){
                        overtimeTeamName.push(element.teamName);
                        overtimeTeamHours.push(element.overtimeHours);
                    });

                    $scope.label4 = overtimeTeamName;
                    $scope.data4 = overtimeTeamHours;

                    $scope.label5 = ["Dev Utilization", "PM Utilization", "QA Utilization", "UI/UX Utilization"];
                    $scope.data5 = [$scope.report.devUtilization, $scope.report.pmUtilization, $scope.report.qaUtilization, $scope.report.uiuxUtilization];

                    $scope.label6 = ["Dev Count", "PM Count", "QA Count", "UI/UX Count"];
                    $scope.data6 = [$scope.report.devCount, $scope.report.pmCount, $scope.report.qaCount, $scope.report.uiuxCount];

                    var projectName = [];
                    var totalHours = [];

                    $scope.report.totalForProjects.forEach(function(element){
                        projectName.push(element.name);
                        totalHours.push(element.overtimeHours);
                    });

                    $scope.label7 = projectName;
                    $scope.data7 = totalHours;
                });
            }

            buildCompanyDashboardCharts(url);
    }]);

})();