(function(){
    var app = angular.module("timeKeeper", ["ngRoute", "ui.bootstrap", "toaster", "ngAnimate", "naif.base64", "chart.js", "LocalStorageModule"]);

    /*currentUser = {
      id: 0,
      name: '',
      role: '',
      teams: [],
      provider: ''
    };
    */

    app.constant('timeConfig', {
        apiUrl: 'http://localhost:50327/api/',
        idsUrl:'http://localhost:53812/connect/token',
        dayType: ['empty', 'workingday', 'publicholiday', 'otherabsence', 'religiousday', 'sickleave', 'vacation', 'businessabsence', 'weekend', 'future'],
        dayDesc: [' ', 'Working Day', 'Public Holiday', 'Other Absence', 'Religious Day', 'Sick Leave', 'Vacation', 'Business Absence'],
        months: ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec']

    });
    app.config(['$routeProvider', 'localStorageServiceProvider', function($routeProvider, localStorageServiceProvider) {
        $routeProvider
            .when('/teams',     { templateUrl: 'views/teams.html', controller: 'teamsController',logReq:true })
            .when('/employees', { templateUrl: 'views/employees.html', controller: 'employeesController',logReq:true })
            .when('/customers', { templateUrl: 'views/customers.html', controller: 'customersController',logReq:true })
            .when('/projects',  { templateUrl: 'views/projects.html', controller: 'projectsController',logReq:true })
            .when('/timetracking',  { templateUrl: 'views/timeTracking.html', controller: 'dayController',logReq:false })
            .when('/login',  { templateUrl: 'views/login.html', controller: 'loginController',logReq:false })
            .when('/logout',  { templateUrl: 'views/login.html', controller: 'logoutController',logReq:true })
            .when('/personalreport',  { templateUrl: 'views/personalreport.html', controller: 'personalReportController',logReq:false })
            .when('/monthlyreport',  { templateUrl: 'views/monthlyreport.html', controller: 'monthlyReportController',logReq:false })
            .when('/annualreport',  { templateUrl: 'views/annualreport.html', controller: 'annualReportController',logReq:false })
            .when('/projecthistoryreport',  { templateUrl: 'views/projectHistory.html', controller: 'projectHistoryController',logReq:false })
            .when('/companydashboard',  { templateUrl: 'views/companydashboard.html', controller: 'dashboardCompanyController',logReq:false })
            .when('/teamdashboard',  { templateUrl: 'views/teamdashboard.html', controller: 'dashboardTeamController',logReq:false })
            .when('/billing',  { templateUrl: 'views/billing.html', controller: 'billingController',logReq:false })
            .otherwise({ redirectTo: '/login' });

        localStorageServiceProvider
            .setPrefix('timeKeeper')
            .setStorageType('sessionStorage')
            .setNotify(true, true);

    }]).run(['$rootScope', '$location', function($rootScope, $location){
        $rootScope.$on('$routeChangeStart', function(event, next, current){
            if($rootScope.currentUser != null) {
                if ($rootScope.currentUser.id == undefined && next.$$route.logReq) {
                    $location.path("/login");
                }
            }
        })
    }]);
}());