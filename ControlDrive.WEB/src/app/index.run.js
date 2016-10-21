(function() {
  'use strict';

  angular
    .module('controldriveApp')
    .run(runBlock);

  /** @ngInject */
  function runBlock($log, $rootScope, $state, authorizationService) {

      $rootScope.$on('$stateChangeStart', function (event, toState) {
          // route authorizationService check
          if (toState.data !== undefined && toState.data.access !== undefined) {
              var authorised = authorizationService.authorize(toState.data.access.loginRequired,
                                                   toState.data.access.requiredPermissions,
                                                   toState.data.access.permissionCheckType);

              if (authorised === authorizationService.constants.loginRequired) {
                  event.preventDefault();
                  $state.go('access.login');
              } else if (authorised === authorizationService.constants.notAuthorised) {
                  event.preventDefault();
                  $state.go('app.resumen');
              }
          }
      });

    //$log.debug('runBlock end');
  }

})();
