(function() {
'use strict';
    angular.module('controldriveApp')
        .config(function ($controllerProvider, $httpProvider, $compileProvider, $filterProvider, $provide, $logProvider, toastrConfig) {
            // Enable log
            $logProvider.debugEnabled(true);

            $httpProvider.interceptors.push('authInterceptorService');

            // Set options third-party lib
            toastrConfig.allowHtml = true;
            toastrConfig.timeOut = 3000;
            toastrConfig.positionClass = 'toast-top-right';
            toastrConfig.preventDuplicates = false;
            toastrConfig.progressBar = false;
        });
})();