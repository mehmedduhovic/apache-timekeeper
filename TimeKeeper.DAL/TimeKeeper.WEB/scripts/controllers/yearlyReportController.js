(function(){

    var app = angular.module("timeKeeper");

    app.controller("annualReportController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;


            $scope.year = 0;
            $scope.monthName = 0;


            $scope.buildAnnualReport = function()
            {
                $scope.showOutput = true;
                listInformation($scope.year);
            };

            $scope.calculateDays = function(year, month, projectName)
            {
              daysInMonth(year, month, projectName);
            };



            function listInformation(year, month) {
                var url = "reports/annualreport?";
                if(year != 'undefined') url += "year=" + year;
                dataService.list(url, function(data){
                    $scope.projects = data;
                    $scope.last = data.list[data.list.length - 1];
                   });
            };

            function daysInMonth(year, month, projectName)
            {
                var url = "reports/monthlyByDays/"+ year + "/" + month;
                dataService.list(url, function(data){
                    for(var i = 0; i < data.length; i++)
                    {
                        if(data[i].name == projectName)
                        {
                            $scope.project = data[i];
                        }
                    }

                    var dates = [];
                    var hours = [];


                    $scope.project.outer.forEach(function(element){
                        dates.push(new Date(element.date).getDate() + "/" + new Date(element.date).getMonth());
                        hours.push(element.sum);
                    });

                    /*
                    console.log(dates);
                    console.log(hours);
                       */
                    $scope.linedata = hours;
                    $scope.datasetOverride = [{ yAxisID: 'y-axis-1' }, { yAxisID: 'y-axis-2' }];
                    $scope.linelabels = dates;
                    //$scope.colors = ["#46BFBD"];
                    $scope.lineoptions = {
                        scales: {
                            yAxes: [
                                {
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'total hours worked'
                                    },
                                    display: true,
                                    position: 'left',
                                    id: 'y-axis-1',
                                    type: 'linear',
                                    ticks: {
                                        suggestedMin: 0   // minimum will be 0, unless there is a lower value.
                                    }
                                    }
                            ],
                            xAxes: [{
                                scaleLabel: {
                                    display: true,
                                    labelString: 'working days in month'
                                }
                            }]
                        }
                    };
                });

            };

        }]);
}());