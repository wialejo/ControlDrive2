(function () {
    'use strict';

    angular
      .module('controldriveApp')
      .directive('access', access);

    /** @ngInject */
    function access(authorizationService) {
        var directive = {
            restrict: 'A',
            link: linkFunc,
        };

        return directive;

        /** @ngInject */
        function linkFunc($scope, $element, $attrs) {
            var makeVisible = function () {
                $element.removeClass('hidden');
            };

            var makeHidden = function () {
                $element.addClass('hidden');
            };

            var determineVisibility = function (resetFirst) {
                var result;

                if (resetFirst) {
                    makeVisible();
                }

                result = authorizationService.authorize(true, roles, $attrs.accessPermissionType);

                if (result === authorizationService.constants.authorised) {
                    makeVisible();
                } else {
                    makeHidden();
                }
            };

            var roles = $attrs.access.split(',');

            if (roles.length > 0) {
                determineVisibility(true);
            }
        }
    }

})();