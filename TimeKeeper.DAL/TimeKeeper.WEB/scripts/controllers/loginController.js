(function(){

    var app = angular.module("timeKeeper");

    app.controller("loginController", ["$scope", "$rootScope", "$http", "timeConfig", "$location", "infoService", "localStorageService",
        function($scope, $rootScope, $http, timeConfig, $location, infoService, localStorageService) {
            $rootScope.canSee=false;
            $rootScope.currentUser = localStorageService.get("currentUser");
            if($rootScope.currentUser==null){
                $rootScope.currentUser={
                    id: 0,
                    name: '',
                    role: '',
                    teams: [],
                    provider: ''
                }
            }

            $http.defaults.headers.common.Authorization = 'Bearer ' + localStorageService.get("authToken");

            startApp("loginBtn");
            function startApp(actionButton) {
                gapi.load("auth2", function () {
                    auth2 = gapi.auth2.init({
                        client_id: "496587010155-laqpk92c089btscvb3s37s6igkh4sn8f.apps.googleusercontent.com"
                    });
                    attachSignin(document.getElementById(actionButton));
                });
            };

            function attachSignin(element) {



                auth2.attachClickHandler(element, {}, function (googleUser) {

                    var authToken = googleUser.getAuthResponse().id_token;
                    $http.defaults.headers.common.Authorization = "Bearer " + authToken;
                    $http.defaults.headers.common.Provider = "google";
                    $http({method: "post", url: timeConfig.apiUrl + 'login'})
                        .then(function (response) {
                            currentUser = response.data;
                            console.log("current user",currentUser);
                            $rootScope.currentUser = currentUser;
                            localStorageService.set("currentUser",currentUser);
                            console.log($rootScope.currentUser);
                            if (currentUser.role=='Administrator')
                            { $location.path('/companydashboard'); }
                            else if ($rootScope.canSee == true)
                            { $location.path('/teamdashboard'); }
                            else
                            { $location.path('/personalreport'); }

                            document.getElementById("dashboardroute").classList.add('active');
                            infoService.success("Welcome back " + currentUser.name, "Your role is: " + currentUser.role);
                        }, function (error) {
                            console.log(error);
                            window.alert(error.message);
                        });
                })
            };

            $scope.login = function(){

                var userData = {
                    grant_type: 'password',
                    username: $scope.user.name,
                    password: $scope.user.pass,
                    scope: 'openid'
                };
                var urlEncodedUrl = {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Authorization': 'Basic dGltZWtlZXBlcjokY2gwMGw='
                };

                $http({
                    method: 'POST',
                    url: timeConfig.idsUrl,
                    headers: urlEncodedUrl,
                    data: userData,
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    }
                }).success(function (data, status, headers, config) {
                    authToken = data.access_token;
                    localStorageService.set("authToken", authToken);
                    $http.defaults.headers.common.Authorization = 'Bearer ' + authToken;
                    $http.defaults.headers.common.Provider = "iserver";
                    $http({
                        method: 'GET',
                        url: timeConfig.apiUrl + 'login'
                    }).success(function(data, status, headers, config){
                        currentUser = data;
                        $rootScope.currentUser = currentUser;
                        console.log("current user",currentUser);
                        currentUser.teams.forEach(function (element) {
                            console.log(element);
                            if(element.role.id=="TL ")
                                $rootScope.canSee=true;
                        })

                        //$rootScope.currentUser = currentUser;
                        localStorageService.set("currentUser", +currentUser);
                        if (currentUser.role=='Administrator')
                        { $location.path('/companydashboard'); }
                        else if ($rootScope.canSee == true)
                        { $location.path('/teamdashboard'); }
                        else
                        { $location.path('/personalreport'); }
                        document.getElementById("dashboardroute").classList.add('active');
                        infoService.success("Logged", "Successfull login");
                    });
                }).error(function (data, status, headers, config) {
                    console.log('ERROR: ' + status);
                    infoService.error("Error", "Invalid account");
                });
            };
        }]);

    app.controller("logoutController", ["$rootScope", "$location", "localStorageService", function($rootScope, $location, localStorageService) {
        //currentUser = { id: 0 };
        //$rootScope.currentUser = currentUser;
        console.log($rootScope.currentUser);
        localStorageService.clearAll();
        window.location.reload();
        $location.path("/login");
        console.log($rootScope.currentUser);
    }]);
}());

/*
var profile = googleUser.getBasicProfile();
console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
console.log('Name: ' + profile.getName());
console.log('Image URL: ' + profile.getImageUrl());
console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

console.log(id_token);
*/