(function () {
    'use strict';
    //var ApiUrl= "http://127.0.0.0:5000/";
    var ApiUrl = "http://" + location.hostname + "/" + location.pathname.split("/")[1] + "/API/";
    angular.module('controldriveApp')
        .constant('ngAuthSettings', {
            apiServiceBaseUri: ApiUrl,
            clientId: 'ngAuthApp'
        });
})();

