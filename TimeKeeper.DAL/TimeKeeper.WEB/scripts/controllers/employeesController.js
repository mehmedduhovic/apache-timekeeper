(function(){

    var app = angular.module("timeKeeper");

    app.controller("employeesController", ["$scope", "dataService", "timeConfig", "$location", "infoService", function($scope, dataService, timeConfig, $location, infoService) {

        if(currentUser.role==="Administrator"){
            $scope.showEmployees=true;
        }
        $scope.currentPage = 0;
        $scope.num = 1;
        $scope.message = "Wait...";
        $scope.empPagination = false;
        listEmployees();

        var source = timeConfig.apiUrl;
        var endpoint = "employees?";

        dataService.list("roles", function(data){
            $scope.roles = data;
        });

        dataService.list("engagements", function(data){
            $scope.engagements = data;
        });

        function listEmployees() {
            dataService.list("employees", function (data, headers) {
                $scope.page = angular.fromJson(headers('Pagination'));
                $scope.message = "";
                $scope.totalItems = $scope.page.TotalItems;
                $scope.people = data;
            });
        }

        $scope.$on('deleted', function(event) {
            dataService.list("employees", function(data){
                $scope.message = "";
                $scope.people = data;
                //console.log('iz baze'+$scope.teams);
            });
        });

        $scope.pageChanged = function() {
                dataService.list(endpoint + "page=" + ($scope.currentPage - 1), function (data, headers) {
                    $scope.people = data;
                });
        };


        $scope.toggleSort = function(number) {
            if (number == 1) {
                dataService.list(endpoint + "sort=" + 1, function (data, headers) {
                    $scope.people = data;
                });
                $scope.num = 2;
            }
            else {
                dataService.list(endpoint + "sort=" + 2, function (data, headers) {
                    $scope.people = data;
                });
                $scope.num = 1;
            }
        };

        $scope.filter = function (filter) {
            if (filter !== "") {
                dataService.list("employees?" + "searchBy=firstname&filter=" + filter, function (data, headers) {
                    $scope.people = data;
                    $scope.empPagination = true;
                })
            }
            else {
                dataService.list("employees", function (data, headers) {
                    $scope.people = data;
                })
            }
        };


        $scope.edit = function(person){
            $scope.person = person;
        };

        $scope.unSet = function(){
            $scope.person = null;
        };

        $scope.setPerson = function(person){
            $scope.person = person;
        };

        $scope.save = function(person){
            //console.log(person);
            person.image = ($scope.image)?$scope.image.base64:"";
           // console.log($scope.image);
            if(person.id == undefined){
                dataService.insert("employees", person, function(data){
                    $scope.$emit("deleted");
                    $scope.image = "";
                });
            }
            else{
                dataService.update("employees", person.id, person, function(data){
                    $scope.$emit("deleted");
                    $scope.image = "";
                });
            }
        };

        $scope.confirmDelete = function (person) {
            swal({
                    title: person.firstName + ' ' + person.lastName,
                    text: "Are you sure you want to delete this employee?",
                    type: "warning",
                    //imageUrl: 'images/hhasic.jpg',
                    //imageSize: '240x100',
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
                        dataService.delete("employees", person.id, function (data) { $scope.$emit('deleted'); })
                        swal.close();
                    }
                });
        }

    }]);
}());