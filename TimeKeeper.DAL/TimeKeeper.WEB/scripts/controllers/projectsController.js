(function(){

    var app = angular.module("timeKeeper");

    app.controller("projectsController", ["$scope", "dataService", "timeConfig", "$location", "infoService", "$window", function($scope, dataService, timeConfig, $location, infoService, $window) {

        listProjects();
        $scope.currentPage = 0;
        $scope.num = 1;
        $scope.data = null;

        var source = timeConfig.apiUrl;
        var endpoint = "projects?";

        dataService.list("customers", function(data){
            $scope.customers = data;

        });

        dataService.list("teams", function(data){
            $scope.teams = data;
        });

        /*if($scope.currentUser.role!="Administrator"){
            $location.path('/timetracking');
        }
        else {*/
            $scope.$on("projectUpdated", function () {
                listProjects();
            });

            $scope.message = "Wait...";

            function listProjects() {
                dataService.list("projects", function (data, headers) {
                    $scope.page = angular.fromJson(headers('Pagination'));
                    $scope.message = "";
                    $scope.totalItems = $scope.page.TotalItems;
                    $scope.projects = data;
                });
            }

            $scope.$on('deleted', function (event) {
                dataService.list("projects", function (data) {
                    $scope.message = "";
                    $scope.projects = data;
                    //console.log('iz baze'+$scope.teams);
                });
            });

        $scope.pageChanged = function() {
            dataService.list(endpoint + "page=" + ($scope.currentPage - 1), function (data, headers) {
                $scope.projects = data;
            });
        };


        $scope.toggleSort = function(number) {
            if (number == 1) {
                dataService.list(endpoint + "sort=" + 1, function (data, headers) {
                    $scope.projects = data;
                });
                $scope.num = 2;
            }
            else {
                dataService.list(endpoint + "sort=" + 2, function (data, headers) {
                    $scope.projects = data;
                });
                $scope.num = 1;
            }
        };

        $scope.filter = function (filter) {
            if (filter !== "") {
                dataService.list("projects?" + "searchBy=name&filter=" + filter, function (data, headers) {
                    $scope.projects = data;
                })
            }
            else {
                dataService.list("projects", function (data, headers) {
                    $scope.projects = data;
                })
            }
        };


        $scope.edit = function(project){
            $scope.project = project;
        };

        $scope.unSet = function(){
            $scope.project = null;
        };

        $scope.setProject = function(project){
            $scope.project = project;
        };

        $scope.save = function(project){
            if(project.id === undefined){
                dataService.insert("projects", project, function(data){
                    $scope.emit("projectUpdated");
                });
            }
            else{
                dataService.update("projects", project.id, project, function(data){
                    $scope.emit("projectUpdated");

                });
            }
        };

        $scope.confirmDelete = function (project) {
            swal({
                    title: project.name,
                    text: "Are you sure you want to delete this project?",
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
                function(isConfirm){
                    if(isConfirm){
                        dataService.delete("projects", project.id, function (data) { $scope.$emit('deleted');})
                        swal.close();
                    }
                });
        }

    }]);
}());