(function () {
    'use strict';

    angular.module('controldriveApp')
        .controller('PasswordChangeController', function ($location, $timeout, authService, toastr) {
            var vm = this;
            vm.cambiar = function () {
                authService.passwordChange(vm.passwordData)
                    .then(function () {
                        toastr.success("Constraseña actualizada correctamente, es necesario iniciar sesión nuevamente");
                        authService.logOut();
                        startTimer();
                    }).catch(function (error) {
                        toastr.error(error.data.ExceptionMessage);
                    })
            }

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