(function(){

    var app = angular.module("timeKeeper");

    app.controller("dayController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {

            //$scope.year = new Date().getFullYear();
            //$scope.month = new Date().getMonth();
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.showEmployees=false;
            $scope.people=[];
            $scope.showCalendar = false;
           if ($scope.currentUser.role == "Administrator"){
                    $scope.showEmployees=true;
                    dataService.list("employees/all", function(data) {
                        $scope.people = data;
                    });

            }
           else if($scope.currentUser.teams.length>0){
                $scope.showEmployees=true;
                var membersInTeam = [];
                membersInTeam.push($scope.currentUser.id);
                dataService.list("Engagements?pageSize=100", function(data){
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
                $scope.empId=currentUser.id;
            }

            $scope.buildCalendar = function() {
                if($scope.empId === undefined)
                    window.alert('You have to choose an employee');
                else {
                    listCalendar($scope.empId, $scope.year, $scope.month);
                    $scope.showCalendar = true;
                }
            };

            $scope.$on('calendarUpdated', function(event) {
                listCalendar($scope.empId, $scope.year, $scope.month);
            });

            function listCalendar(empId, year, month) {
                month = month + 1;
                var url = "calendar/days/" + empId;
                if(year != 'undefined') url += "/" + year;
                if(month != 'undefined') url += "/" + month;
                dataService.list(url, function(data){
                    $scope.calendar = data.days;
                    $scope.empId = data.employee.id;
                    $scope.year = data.year;
                    $scope.month = data.month - 1;
                    $scope.num = function(){
                        var size = new Date(data.days[0].date).getDay()-1;
                        if (size < 0) size = 6;
                        return new Array(size);
                    }
                });
            };

            $scope.edit = function(day) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'views/dayModal.html',
                    controller: 'ModalCalendarCtrl',
                    size: 'lg',
                    resolve: {
                        day: function () {
                            return day;
                        }
                    }
                });
            }
        }]);

    app.controller('ModalCalendarCtrl', ['$uibModalInstance', '$scope',
        'dataService', 'timeConfig', 'day', 'infoService', function ($uibModalInstance, $scope,
                                                  dataService, timeConfig, day, infoService) {

        $scope.day = day;
        $scope.dayType = timeConfig.dayDesc;
        dataService.list("projects", function(data){
            $scope.projects = data;
        });
        initNewTask();

        $scope.add = function(task){
            console.log("task", task);
            if(task.hours > 12 || task.hours < 0)
            {
                infoService.error("Invalid Insert Into Time", "Task cant be longer than 12 or negative.");
            }
            else
            {
                $scope.day.details.push(task);
                sumHours();
                initNewTask();
            }
        };

        $scope.upd = function(task, index) {
            sumHours();
        };

        $scope.del = function(index) {
            $scope.day.details[index].deleted = true;
            sumHours();
        };

        function sumHours() {
            $scope.day.hours = 0;
            for(var i=0; i<$scope.day.details.length; i++) {
                if(!$scope.day.details[i].deleted) $scope.day.hours += Number($scope.day.details[i].hours);
            }
        }

        function initNewTask() {
            $scope.newTask = {id: 0, description: '', hours: 0, project: {id: 0, name: ''}, deleted: false};
        }

        $scope.ok = function () {
            dataService.insert("days", $scope.day, function(data){
                $scope.$emit('calendarUpdated');
            });
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        };

        $scope.typeChanged = function() {
            if($scope.day.type != 1) {
                $scope.day.hours = 8;
            }
        }
    }]);
}());