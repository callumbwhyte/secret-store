(function () {

  "use strict";

  function SecretStoreResource($http, umbRequestHelper) {

    let apiBaseUrl = "/umbraco/backoffice/api/SecretStore/";

    return {

      getAll: function () {
        return umbRequestHelper.resourcePromise(
          $http.get(apiBaseUrl + "GetSecrets"),
          "Failed to retrieve"
        );
      },

      save: function (item) {
        return umbRequestHelper.resourcePromise(
          $http.post(apiBaseUrl + "Save?key=" + item.name + "&value=" + item.secret),
          "Failed to save"
        );
      },

      delete: function (item) {
        return umbRequestHelper.resourcePromise(
          $http.delete(apiBaseUrl + "Delete?key=" + item.name),
          "Failed to delete"
        );
      }

    };

  };

  angular.module("umbraco.resources").factory("secretStoreResource", SecretStoreResource);

})();