(function(){

    var app = angular.module("timeKeeper");

    app.controller("personalReportController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.showEmployees=false;
            $scope.people=[];
            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];

            $scope.year = 2018;
            $scope.month = 4;

            if($scope.currentUser.role == "Administrator") {
                $scope.showEmployees=true;
                dataService.list("employees/all/", function (data) {
                    $scope.people = data;
                });
            }
            else if($scope.currentUser.teams.length>0){
                $scope.showEmployees=true;
                var membersInTeam = [];
                membersInTeam.push($scope.currentUser.id);
                dataService.list("Engagements?pageSize=100", function (data) {
                    $scope.members = data;
                    for(i=0;i<$scope.members.length;i++){
                        for(j=0;j<$scope.currentUser.teams.length;j++) {
                            if ($scope.members[i].team.id == currentUser.teams[j].id && $scope.members[i].employee.id != $scope.currentUser.id) {
                                membersInTeam.push($scope.members[i].employee.id);
                            }
                        }
                    }
                    for(j=0;j<membersInTeam.length;j++){
                        dataService.read("employees", membersInTeam[j], function (data) {
                            $scope.people.push(data);
                        })
                    }
                });
            }
            else{
                $scope.showEmployees=false;
                $scope.empId=$scope.currentUser.id;
            }


            $scope.buildPersonalReport = function() {
                if($scope.empId === undefined)
                {   $scope.empId=$scope.currentUser.id;

                    $scope.showOutput = true;
                    listWorkingAndNonWorkingDays($scope.empId, $scope.year, $scope.month);
                    listNonWorkingDaysStatistics($scope.empId, $scope.year, $scope.month);
                    listWorkingDaysStatistics($scope.empId, $scope.year, $scope.month);
                    listIndividualDaysStatistics($scope.empId, $scope.year, $scope.month);

                }
                else {
                    $scope.showOutput = true;
                    listWorkingAndNonWorkingDays($scope.empId, $scope.year, $scope.month);
                    listNonWorkingDaysStatistics($scope.empId, $scope.year, $scope.month);
                    listWorkingDaysStatistics($scope.empId, $scope.year, $scope.month);
                    listIndividualDaysStatistics($scope.empId, $scope.year, $scope.month);
                }
            };

                function listWorkingAndNonWorkingDays(empId, year, month) {
                month+=1;
                var url = "reports/personalreport?employeeId=" + empId;
                if(year != 'undefined') url += "&year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    $scope.person = data.employee;
                    var absenceCount = 0;
                    var totalAbasences = 0;
                    var counting = [];
                    data.days.forEach(function(element){
                        if(element.type !== "WorkingDay")
                        {
                            totalAbasences++;
                            counting.push(1);
                        }

                        counting.push(0);
                    });

                    $scope.brantfordFactor = 4 * totalAbasences;

                    var totalAbsences2 = 0;

                    data.days.forEach(function(element){
                       if(element.type !== "WorkingDay"){
                           totalAbsences2++;
                       }

                       $scope.paidTimeOff = totalAbsences2 * 8;
                    });

                    //var totalPaidOffDays = data.nonWorkingDaysTotal - data.missingEntries;
                    //$scope.paidTimeOff = (totalPaidOffDays / data.days.length) * 30;
                    $scope.utilization = data.percentageOfWorkingDays;
                    $scope.boxClass = $scope.utilization > 75 ? 'green' : 'red';
                    $scope.thumbs = $scope.utilization > 75 ? 'up' : 'down';
                    $scope.labels2 = ["Working Days", "Missing Entries", "Non-Working Days"];
                    $scope.colors2 = ['#00a65a', '#FF5252', '#d2d6de'];
                    $scope.data2 = [
                        data.workingDays,
                        data.missingEntries,
                        data.nonWorkingDaysTotal

                    ];
                });
            };

            function listNonWorkingDaysStatistics(empId, year, month) {
                month+=1;
                var url = "reports/personalreport?employeeId=" + empId;
                if(year != 'undefined') url += "&year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    $scope.labels3 = ['Religious Days', 'Sick Leave Days', 'Vacation Days', 'Bussines Absences', 'Other Absences', 'Public Holiday'];
                    $scope.data3 =
                        [data.religiousDays, data.sickLeaveDays, data.vacationDays, data.bussinessAbsenceDays, data.otherAbsenceDays, data.publicHolidayDays];
                });
            };

            function listWorkingDaysStatistics(empId, year, month) {
                month+=1;
                var url = "reports/personalreport?employeeId=" + empId;
                if(year != 'undefined') url += "&year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    var names = [];
                    var hours = [];

                    data.distinctProjects.forEach(function(element){
                        names.push(element.name);
                        hours.push(element.totalHours);
                    });

                    $scope.labels4 = names;
                    $scope.data4 = hours;
                });
            };

            function listIndividualDaysStatistics(empId, year, month) {
                month+=1;
                var url = "reports/personalreport?employeeId=" + empId;
                if(year != 'undefined') url += "&year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){

                    var days = [0, 0, 0, 0, 0, 0, 0];

                    data.days.forEach(function(element){
                        //new Date(data.days[0].date).getDay() - 1;
                        days[new Date(element.date).getDay()] += (element.hours);
                    });

                    $scope.label5 = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
                    $scope.data5 = days;
                });
            };

         
        }]);
}());