(function() {
'use strict';
    angular.module('controldriveApp')
        .config(function ($controllerProvider, $compileProvider, $filterProvider, $provide, $logProvider, toastrConfig) {
            // Enable log
            $logProvider.debugEnabled(true);

            // Set options third-party lib
            toastrConfig.allowHtml = true;
            toastrConfig.timeOut = 3000;
            toastrConfig.positionClass = 'toast-top-right';
            toastrConfig.preventDuplicates = false;
            toastrConfig.progressBar = false;
        });
    //var ApiUrl= "http://192.168.0.29/API/";
    //var ApiUrl= "http://Localhost/API2/";
    var ApiUrl = "http://" + window.location.hostname + "/API2/";
    angular.module('controldriveApp')
        .constant('ngAuthSettings', {
            apiServiceBaseUri: ApiUrl,
            clientId: 'ngAuthApp'
        });
    angular.module('controldriveApp')
        .config(function ($httpProvider) {
            $httpProvider.interceptors.push('authInterceptorService');
        });

    angular.module('controldriveApp')
        .run( function (authService) {
            authService.fillAuthData();
        });
})();