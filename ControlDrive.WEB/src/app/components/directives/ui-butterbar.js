angular.module('controldriveApp')
  .directive('uiButterbar',  function($rootScope, $http, $anchorScroll) {
     return {
      restrict: 'AC',
      template:'<span class="bar"></span>',
      link: function(scope, el, attrs) {        

        scope.isLoading = function () {
            return $http.pendingRequests.length > 0;
        };

        scope.$watch(scope.isLoading, function (v)
        {

            if(v){
                el.removeClass('hide').addClass('active');

            }else{
                el.addClass('hide').removeClass('active');
            }
        });


        el.addClass('butterbar hide');
        scope.$on('$stateChangeStart', function(event) {
          $anchorScroll();
          el.removeClass('hide').addClass('active');
        });
        scope.$on('$stateChangeSuccess', function( event, toState, toParams) {
          event.targetScope.$watch('$viewContentLoaded', function(){
            el.addClass('hide').removeClass('active');
          })
        });
      }
     };
  });