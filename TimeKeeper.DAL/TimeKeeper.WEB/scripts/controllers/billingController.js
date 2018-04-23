(function(){

    var app = angular.module("timeKeeper");

    app.controller("billingController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];

            $scope.year = 0;
            $scope.monthName = 0;

            $scope.showBilling = false;


            $scope.billing = function()
            {
                $scope.showBilling = true;
                var url="/reports/listInvoices/"+$scope.year+"/"+ ($scope.month + 1);
                dataService.list(url, function(data){
                    $scope.invoices = data;
                });
            };

            $scope.individualInvoice = function(invoice)
            {
                $scope.invoice = invoice;
                var dt = new Date(Date.now());
                $scope.currentDate = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();

                var payingDate = new Date(Date.now());
                payingDate.setDate(payingDate.getDate() + 7);
                $scope.payingDate = payingDate.getFullYear() + "/" + (payingDate.getMonth() + 1) + "/" + payingDate.getDate();

                $scope.totalPrice = 0;

                $scope.total = invoice.roles.forEach(function(element){
                    $scope.totalPrice += element.quanity * element.unitPrice;
                });

                $scope.tax = $scope.totalPrice * 17 / 100;

            };

            $scope.printDiv = function(divName) {
                var printContents = document.getElementById(divName).innerHTML;
                var popupWin = window.open('', '_blank', 'width=600,height=300');
                popupWin.document.open();

                popupWin.document.write('<html><head>' +
                    '<link rel="stylesheet" type="text/css" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />' +
                    '<link rel="stylesheet" type="text/html" href="//raw.githubusercontent.com/bracikaa/AdminLTE/38b885bc27624195c4afa85a8535e283e3d46cd0/dist/css/AdminLTE.min.css"/>' +
                    '</head><body onload="window.print()">' + printContents + '</body></html>');
                popupWin.document.close();
            }

            $scope.send = function(invoice){
                console.log(invoice);
                dataService.insert("invoice", $scope.invoice, function(data){
                });
            }

            $scope.delete=function (data) {
                $scope.invoice.roles=$scope.invoice.roles.filter(function (el) {
                    return el  != data;
                })
            }
        }]);

}());