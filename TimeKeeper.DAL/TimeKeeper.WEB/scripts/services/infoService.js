(function () {
    var app = angular.module("timeKeeper");

    app.factory("infoService", ['toaster', function (toaster) {

        return {
            info: function(title, message) {
                toaster.pop("info", title, message, 3000);
            },
            success: function(title, message) {
                toaster.pop("success", title, message, 3000);
            },
            warning: function(title, message) {
                toaster.pop("warning", title, message, 3000);
            },
            error: function(title, message){
                swal({
                    title: title,
                    text: message,
                    type: "error"
                });
            }
        }
    }]);
}());