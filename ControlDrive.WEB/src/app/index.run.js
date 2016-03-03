(function() {
  'use strict';

  angular
    .module('controldriveApp')
    .run(runBlock);

  /** @ngInject */
  function runBlock($log) {

    $log.debug('runBlock end');
  }

})();
