(function() {
'use strict';

angular.module('controldriveApp')
  .directive('setNgAnimate', function ($animate) {
    return {
        link: function ($scope, $element, $attrs) {
            $scope.$watch( function() {
                return $scope.$eval($attrs.setNgAnimate, $scope);
            }, function(valnew){
                $animate.enabled(!!valnew, $element);
            });
        }
    };
  });
})();