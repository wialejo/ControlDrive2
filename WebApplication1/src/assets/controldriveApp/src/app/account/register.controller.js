(function() {
'use strict';

  angular
    .module('controldriveApp')
        .controller('RegisterController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

            $scope.savedSuccessfully = false;
            $scope.message = "";

            $scope.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.signUp = function () {
                authService.saveRegistration($scope.registration).then(function (response) {
                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();
                },
                 function (response) {
                     var errors = [];
                     for (var key in response.data.modelState) {
                         for (var i = 0; i < response.data.modelState[key].length; i++) {
                             errors.push(response.data.modelState[key][i]);
                         }
                     }
                     $scope.message = "Failed to register user due to:" + errors.join(' ');
                 });
            };
            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }
        }]);
})();

//// signup controller
//app.controller('SignupFormController', ['$scope', '$http', '$state', function($scope, $http, $state) {
//    $scope.user = {};
//    $scope.authError = null;
//    $scope.signup = function() {
//      $scope.authError = null;
//      // Try to create
//      $http.post('api/signup', {name: $scope.user.name, email: $scope.user.email, password: $scope.user.password})
//      .then(function(response) {
//        if ( !response.data.user ) {
//          $scope.authError = response;
//        }else{
//          $state.go('app.dashboard-v1');
//        }
//      }, function(x) {
//        $scope.authError = 'Server Error';
//      });
//    };
//  }])
// ;