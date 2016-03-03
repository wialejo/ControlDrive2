(function() {
'use strict';

    angular.module('controldriveApp')
        .controller('RegisterController', function ( $location, $timeout, authService) {
            var vm = this;
            vm.savedSuccessfully = false;
            vm.message = "";

            vm.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            vm.signUp = function () {
                authService.saveRegistration(vm.registration).then(function () {
                    vm.savedSuccessfully = true;
                    vm.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();
                },
                 function (response) {
                     var errors = [];
                     for (var key in response.data.modelState) {
                         for (var i = 0; i < response.data.modelState[key].length; i++) {
                             errors.push(response.data.modelState[key][i]);
                         }
                     }
                     vm.message = "Failed to register user due to:" + errors.join(' ');
                 });
            };
            var startTimer = function () {
                var timer = $timeout(function () {
                      $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }
        });
})();

//// signup controller
//app.controller('SignupFormController', ['vm', '$http', '$state', function(vm, $http, $state) {
//    vm.user = {};
//    vm.authError = null;
//    vm.signup = function() {
//      vm.authError = null;
//      // Try to create
//      $http.post('api/signup', {name: vm.user.name, email: vm.user.email, password: vm.user.password})
//      .then(function(response) {
//        if ( !response.data.user ) {
//          vm.authError = response;
//        }else{
//          $state.go('app.dashboard-v1');
//        }
//      }, function(x) {
//        vm.authError = 'Server Error';
//      });
//    };
//  }])
// ;