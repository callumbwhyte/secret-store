(function () {

  "use strict";

  angular.module("umbraco")
    .controller("Our.Umbraco.SecretStore.OverviewController",
      function ($filter, editorService, localizationService, navigationService, notificationsService, secretStoreResource) {

        var vm = this;

        vm.loading = true;

        vm.labels = {
          title: "treeHeaders_secretStore",
          addSecret: "actions_addSecret",
          saved: "general_saved",
          secret: "general_secret",
          updateDate: "general_updateDate"
        };

        vm.options = {};

        vm.items = [];

        vm.selectedItems = [];

        vm.bulkActionCount = 0;

        vm.bulkActionProcessing = false;

        vm.addItem = function () {
          openEditor({}, function (item) {
            secretStoreResource.save(item)
              .then(function (secret) {
                notificationsService.success(vm.labels.saved, secret.name);
              })
              .finally(function () {
                loadSecrets();
              })
          });
        };

        vm.selectItem = function (item) {
          // set selected state
          item.selected = !item.selected;
          if (item.selected) {
            // add to selected items list
            vm.selectedItems.push(item)
          } else {
            // remove from selected items list
            let index = vm.selectedItems.indexOf(item);
            vm.selectedItems.splice(index, 1);
          }
        };

        vm.selectAll = function () {
          let selectAll = vm.isSelectedAll();
          // set selected state
          vm.items.forEach(item => {
            item.selected = !selectAll;
          });
          // update selected items list
          vm.selectedItems = vm.items.filter(item => item.selected);
        };

        vm.isSelectedAll = function () {
          return vm.items.length == vm.selectedItems.length;
        };

        vm.clearAll = function () {
          // set selected state
          vm.items.forEach(item => {
            item.selected = false;
          });
          // update selected items list
          vm.selectedItems = vm.items.filter(item => item.selected);
        };

        vm.deleteSelected = function () {
          applyBulkAction(secretStoreResource.delete)
            .then(function () {
              loadSecrets();
            });
        };

        vm.$onInit = function () {
          // loalize labels
          localizationService.localizeMany(Object.values(vm.labels))
            .then(function (values) {
              // map localized values
              Object.keys(vm.labels).forEach((x, i) => {
                vm.labels[x] = values[i];
              });
              // set include properties
              vm.options.includeProperties = [
                {
                  alias: "secret",
                  header: vm.labels.secret,
                  isSensitive: true
                },
                {
                  alias: "updateDate",
                  header: vm.labels.updateDate
                }
              ];
            });
          // load secrets
          loadSecrets();
          // sync tree
          navigationService.syncTree({ tree: "secretStore", path: "-1" });
        };

        function loadSecrets() {
          // set loading state
          vm.loading = true;
          // get all secrets
          secretStoreResource.getAll()
            .then(function (data) {
              // build items
              vm.items = data.map(x => {
                return {
                  name: x.name,
                  icon: "icon-key",
                  updateDate: $filter("date")(x.updateDate, "yyyy-MM-dd hh:mm:ss")
                };
              });
              // remove loading state
              vm.loading = false;
            });
        }

        function openEditor(item, callback) {
          var options = {
            title: vm.labels.addSecret,
            view: "/App_Plugins/SecretStore/backoffice/dialogs/secretDialog.html",
            size: "small",
            content: item,
            submit: function (data) {
              if (callback) {
                callback(data);
              }
              editorService.close();
            },
            close: function () {
              editorService.close();
            }
          };
          editorService.open(options);
        }

        function applyBulkAction(action) {
          // set bulk action status
          vm.bulkActionProcessing = true;
          // get actions for items
          var actions = vm.selectedItems.map(item => action(item)
            .then(function () {
              vm.bulkActionCount++;
            })
          );
          return Promise.all(actions)
            .then(function () {
              // reset selected items
              vm.clearAll();
              // reset bulk action status
              vm.bulkActionCount = 0;
              vm.bulkActionProcessing = false;
            });
        }

      });

})();