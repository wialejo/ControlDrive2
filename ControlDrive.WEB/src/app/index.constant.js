(function () {
    'use strict';
    //var ApiUrl= "http://192.168.0.29/API/";
    var ApiUrl= "http://localhost:50579/API/";
    //var ApiUrl = "http://" + location.hostname + "/" + location.pathname.split("/")[1] + "/API/";
    angular.module('controldriveApp')
        .constant('ngAuthSettings', {
            apiServiceBaseUri: ApiUrl,
            clientId: 'ngAuthApp'
        });
})();

