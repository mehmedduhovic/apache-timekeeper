(function(){

    var app = angular.module("timeKeeper");

    app.controller("customersController", ["$scope", "dataService", "$location" ,function($scope, dataService,$location) {

        if($scope.currentUser.role!="Administrator"){
            $location.path('/timetracking');
        }
        else {
            $scope.message = "Wait...";
            dataService.list("customers", function (data) {
                $scope.message = "";
                $scope.customers = data;
            });


            $scope.$on('deleted', function (event) {
                dataService.list("customers", function (data) {
                    $scope.message = "";
                    $scope.customers = data;
                    //console.log('iz baze'+$scope.teams);
                });
            });
        }

        $scope.filter = function (filter) {
            if (filter !== "") {
                dataService.list("customers?" + "searchBy=name&filter=" + filter, function (data, headers) {
                    $scope.customers = data;
                })
            }
            else {
                dataService.list("customers", function (data, headers) {
                    $scope.customers = data;
                })
            }
        };
    }]);

    app.controller("custController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {

        var $cust = this;
        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/custModal.html',
                controller: 'ModalCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
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
                templateUrl: 'views/custModal.html',
                controller: 'ModalCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
                        return;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

        $scope.confirm=function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/custConfirmModal.html',
                controller: 'ModalCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
                        return data;
                    }
                }
            }).closed.then(function () {$scope.$emit("deleted")});
        };

        $scope.confirmDelete = function (customer) {
            swal({
                    title: customer.name,
                    text: "Are you sure you want to delete this customer?",
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
                        dataService.delete("customers", customer.id, function (data) { $scope.$emit('deleted');})
                        swal.close();
                    }
                });
        }
    }]);

    app.controller('ModalCtrl', function ($uibModalInstance, $scope, customer, dataService) {
        var $cust = this;
        console.log(customer);
        $scope.customer = customer;

        $scope.save = function(customer){
            console.log(customer);
            if(customer.id === undefined){
                dataService.insert("customers", customer, function(data){
                    $uibModalInstance.close();
                });

            }
            else{
                dataService.update("customers", customer.id, customer, function(data){
                    $uibModalInstance.close();
                });
            }

        };
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        };
    });
}());