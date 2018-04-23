(function(){

    var app = angular.module("timeKeeper");

    app.controller("dashboardTeamController", ["$scope", "dataService", "$rootScope", "timeConfig",
        function($scope, dataService, $rootScope, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];

            var teamLeadCount = 0;
            var teamNames = [];
            var url;
            $scope.teamId = 'A';
            $scope.year = 2017;
            $scope.month = 6;

            $scope.year = 2017;
            $scope.monthName = 0;
            $scope.showReport = false;

            $scope.first = [];

            $rootScope.currentUser.teams.forEach(function(element){
                if(element.role.id == "TL ")
            {
                teamNames.push(element.id);
                teamLeadCount++;
            }});

            if($rootScope.currentUser.role == 'Administrator')
            {
                dataService.list("teams/all", function (data) {
                    $scope.teams = data;
                    $scope.teamId = $scope.teams[0].id;
                });
            }
            else if (teamLeadCount >= 1)
            {
                $scope.teams = [];
                dataService.list("teams/all", function (data) {
                    $scope.fullTeams = data;
                    for(i = 0; i < teamNames.length; i++) {
                        for(j = 0; j < $scope.fullTeams.length; j++){
                            if(teamNames[i] == $scope.fullTeams[j].id)
                            {
                                $scope.teams.push($scope.fullTeams[j]);
                            }
                        }
                    }
                    $scope.teamId = $scope.teams[0].id;
                });
            }

        $scope.buildTeamReport = function() {
            if($scope.teamId === undefined)
                window.alert('You have to choose a team.');
            else {
                $scope.showReport = true;
                listTeamReport($scope.teamId, $scope.year, $scope.month);
             }
        };

        function listTeamReport(empId, year, month) {
            url = "reports/TeamReport?teamId="+ empId +"&year="+ year +"&month=" + month;
            dataService.list(url, function(data) {
                $scope.teamReport = data;

                $scope.totalinfolabel = ['Working', 'Vacation', 'Sick Leave', 'Religious', 'Public Holidays', 'Other Absences', 'Bussiness Absences'];
                $scope.totalinfodata = [$scope.teamReport.dayss.workingDays, $scope.teamReport.dayss.vacationDays, $scope.teamReport.dayss.sickLeaveDays,
                    $scope.teamReport.dayss.religiousDays, $scope.teamReport.dayss.publicHolidays, $scope.teamReport.dayss.otherAbscenceDays, $scope.teamReport.dayss.businessAbscenceDays];

                $scope.buildChart = function(report)
                {
                    buildDayChart(report);
                    buildDetailedChart(report);
                }

                function buildDayChart(report)
                {
                    $scope.report = report;
                    $scope.intemplabel = ['Working Days', 'Vacation Days', 'Sick Leave Days',
                        'Religious Days', 'Public Holidays', 'Other Absence Days', 'Missing Entries', 'Bussiness Absence Days'];
                    $scope.indempdata = [report.days.workingDays, report.days.vacationDays, report.days.sickLeaveDays,
                        report.days.religiousDays, report.days.publicHolidays, report.days.otherAbscenceDays,
                        report.days.missingEntries, report.days.businessAbscenceDays];
                    $scope.intempseries = [report.employee.name];
                }

                function buildDetailedChart(report)
                {
                    month+=1;
                    var url = "reports/personalreport?employeeId=" + report.employee.id;
                    if(year != 'undefined') url += "&year=" + $scope.year;
                    if(month != 'undefined') url += "&month=" + $scope.month;
                    dataService.list(url, function(data){
                        $scope.individual = data;
                        var projectsWorkedOn = [];
                        $scope.individual.distinctProjects.forEach(function(element){
                            if(element.totalHours > 0)
                            {
                                projectsWorkedOn.push(element);
                            }
                        });

                        $scope.projectsWorkedOn = projectsWorkedOn;
                        console.log($scope.projectsWorkedOn);
                    });
                }
            });
        }
    }]);

})();