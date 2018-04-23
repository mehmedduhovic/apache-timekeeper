(function () {

    var app = angular.module("timeKeeper");

    app.controller("engagementController", ["$scope", "dataService", "timeConfig", "$location", "infoService", "team","$uibModalInstance",
        function ($scope, dataService, timeConfig, $location, infoService, team, $uibModalInstance) {

            $scope.member = team;
            $scope.message = "Wait...";
            $scope.empPagination = false;

            $scope.$on('deleted', function (event) {
                dataService.list("teams", function (data) {
                    $scope.message = "";
                    $scope.teams = data;
                });
            });


            var source = timeConfig.apiUrl;
            var endpoint = "engagements";

            dataService.list("employees/all", function (data) {
                $scope.employees = data;
                console.log($scope.employees);
            });

            dataService.list("roles", function (data) {
                $scope.roles = data;
            });

            dataService.list("teams", function (data) {
                $scope.teams = data;
            });


            $scope.save = function (engagement) {
                console.log(engagement);
                var newMember = {
                    role: {Id: engagement.role.id},
                    team: {Id: team.id},
                    employee: {Id: engagement.employee.id},
                    hours: 40

                };

                console.log(newMember);

                dataService.insert("engagements", newMember, function (data) {
                    $scope.$emit("deleted");
                });


            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss();
            };

            $scope.update = function (engagement) {
                console.log(engagement);

                var newMember = {
                    role: {Id: engagement.role.id},
                    team: {Id: engagement.team.id},
                    id: engagement.id,
                    employee: {Id: engagement.employee.id},
                    hours: 40
                }

                console.log(newMember);

                dataService.update("engagements", newMember.id, newMember, function (data) {
                    $scope.$emit('deleted');

                });

            };


        }]);
}());