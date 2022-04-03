(function () {

  "use strict";

  angular.module("umbraco")
    .controller("Our.Umbraco.SecretStore.Dialogs.SecretDialogController",
      function ($scope, formHelper) {

        var vm = this;

        vm.item = $scope.model.content;

        vm.close = function () {
          if ($scope.model.close) {
            $scope.model.close();
          }
        };

        vm.submit = function () {
          // validate form
          let isValid = formHelper.submitForm({ scope: $scope });
          if (isValid) {
            // submit dialog
            if ($scope.model.submit) {
              $scope.model.submit(vm.item);
            }
          }
        };

      });

})();