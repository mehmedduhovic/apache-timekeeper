(function(){
    var app = angular.module("timeKeeper");

    app.controller("teamsController", ["$scope", "dataService", function($scope, dataService) {

        $scope.message = "Wait...";
        $scope.teamsShow=false;

            dataService.list("teams", function (data) {
                $scope.message = "";
                $scope.teams = data;
            });

        dataService.list("employees/all", function(data){
            $scope.employees = data;
            console.log($scope.employees);
        });

        $scope.$on('deleted', function(event) {
            dataService.list("teams", function(data){
                $scope.message = "";
                $scope.teams = data;
            });
        });

        dataService.list("roles", function(data){
            $scope.roles = data;
        });

        $scope.filter = function (filter) {
            if (filter !== "") {
                dataService.list("teams?" + "searchBy=name&filter=" + filter, function (data, headers) {
                    $scope.teams = data;
                })
            }
            else {
                dataService.list("teams", function (data, headers) {
                    $scope.teams = data;
                })
            }
        };

    }]);
    app.controller("tmController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {
        var $team = this;
        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/teamEditModal.html',
                controller: 'ModalTeam',
                controllerAs: '$team',
                closeOnCancel: true,
                //closeOnCancel: true,
                resolve: {
                    team: function () {
                        return data;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

        $scope.add = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/teamAddModal.html',
                controller: 'ModalTeam',
                controllerAs: '$team',
                closeOnCancel: true,
                resolve: {
                    team: function () {
                        return;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

       $scope.addMember = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/teamAddMemberModal.html',
                controller: 'engagementController',
                controllerAs: '$team',
                closeOnCancel: true,
                resolve: {
                    team: function () {
                        return data;
                    }
                }
            }).closed.then(function () {
                $scope.$emit("deleted")});
        }; 

        $scope.editMember = function (member) {
            console.log(member);
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/teamEditMemberModal.html',
                controller: 'engagementController',
                controllerAs: '$team',
                closeOnCancel: true,
                resolve: {
                    team: function () {
                        return member;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

        $scope.confirm = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/teamConfirmModal.html',
                controller: 'ModalTeam',
                size: 'sm',
                controllerAs: '$team',
                resolve: {
                    team: function () {
                        return data;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

        $scope.deleteMember = function (engagement) {
            swal({
                    title:"",
                    text: "Are you sure you want to remove team member?",
                    type: "warning",
                    showCancelButton: true,
                    customClass: "sweetClass",
                    confirmButtonColor: "teal",
                    confirmButtonText: "Yes, sure",
                    allowOutside: true,
                    cancelButtonColor: "darkred",
                    cancelButtonText: "No, not ever!",
                    closeOnConfirm: false,
                    closeOnCancel: true

                },
                function(isConfirm){
                    if(isConfirm){
                        dataService.delete("engagements", engagement.id, function (data) { $scope.$emit('deleted'); })
                        swal.close();
                    }
                });
        }

        $scope.confirmDelete = function (team) {
            swal({
                    title: team.name,
                    text: "Are you sure you want to delete this team?",
                    type: "warning",
                    //imageUrl: 'images/hhasic.jpg',
                    //imageSize: '240x100',
                    showCancelButton: true,
                    customClass: "sweetClass",
                    confirmButtonColor: "teal",
                    confirmButtonText: "Yes, sure",
                    cancelButtonColor: "darkred",
                    cancelButtonText: "No, not ever!",
                    closeOnConfirm: false,
                    closeOnCancel: true,

                },
                function (isConfirm) {
                    if (isConfirm) {
                        dataService.delete("teams", team.id, function (data) {
                            $scope.$emit('deleted');

                        })
                        swal.close();
                    }
                });

        };

    }]);

    app.controller('ModalTeam', function ($uibModalInstance, $scope, team, dataService) {
        var $team = this;
        console.log(team);
        $scope.team = team;

        dataService.list("employees/all", function(data){
            $scope.employees = data;
            console.log($scope.employees);
        });

        dataService.list("roles", function(data){
            $scope.roles = data;
        });

        $scope.save = function(team){
                dataService.insert("teams", team, function(data){
                });
            $uibModalInstance.close();
            };

        $scope.update = function(team){
                dataService.update("teams", team.id, team, function(data){
                });
            $uibModalInstance.close();
            };

        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        };
    });
}());

