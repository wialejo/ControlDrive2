(function () {
    'use strict';

    angular
      .module('controldriveApp')
      .factory('authorizationService', authorization);

    /** @ngInject */
    function authorization($rootScope, UsuarioSvc) {
        var service = {
            authorize: authorize,
            constants: {
                authorised: 0,
                loginRequired: 1,
                notAuthorised: 2
            }
        };

        return service;

        function authorize(loginRequired, requiredPermissions, permissionCheckType) {
            var result = service.constants.authorised,
                user = UsuarioSvc.ObtenerActual(),
                loweredPermissions = [],
                hasPermission = true,
                permission;

            permissionCheckType = permissionCheckType || 'atLeastOne';

            if (loginRequired === true && user === undefined) {
                result = service.constants.loginRequired;

            } else if ((loginRequired === true && user !== undefined) &&
                        (requiredPermissions === undefined || requiredPermissions.length === 0)) {
                result = service.constants.authorised;

            } else if (requiredPermissions) {

                loweredPermissions = [user.Rol.toLowerCase()];


                //angular.forEach(user.roles, function (permission) {
                //    loweredPermissions.push(permission.toLowerCase());
                //});

                for (var i = 0; i < requiredPermissions.length; i += 1) {
                    permission = requiredPermissions[i].toLowerCase();

                    if (permissionCheckType === 'combinationRequired') {
                        hasPermission = hasPermission && loweredPermissions.indexOf(permission) > -1;
                        // if all the permissions are required and hasPermission is false there is no point carrying on
                        if (hasPermission === false) {
                            break;
                        }
                    } else if (permissionCheckType === 'atLeastOne') {
                        hasPermission = loweredPermissions.indexOf(permission) > -1;
                        // if we only need one of the permissions and we have it there is no point carrying on
                        if (hasPermission) {
                            break;
                        }
                    }
                }

                result = hasPermission ?
                         service.constants.authorised :
                         service.constants.notAuthorised;
            }

            return result;
        }
    }
})();