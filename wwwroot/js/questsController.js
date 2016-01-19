// questsController.js
(function () {

    "use strict";

    // Getting the existing module
    angular.module("app-quests")
      .controller("questsController", questsController);

    function questsController($http) {

        var vm = this;

        vm.quests = [];

        vm.newQuest = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/quests")
          .then(function (response) {
              // Success
              angular.copy(response.data, vm.quests);
              console.log(vm.quests);
          }, function (error) {
              // Failure
              vm.errorMessage = "Failed to load data: " + error;
          })
          .finally(function () {
              vm.isBusy = false;
          });

        vm.addQuest = function () {
            vm.newQuest.status = "Incomplete";
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/quests", vm.newQuest)
              .then(function (response) {
                  // success
                  vm.quests.push(response.data);
                  vm.newQuest = {};
              }, function () {
                  // failure
                  vm.errorMessage = "Failed to save new quest";
              })
              .finally(function () {
                  vm.isBusy = false;
              });

        };

        vm.deleteQuest = function (quest) {
            vm.questToDelete = JSON.stringify(quest);
            console.log(vm.questToDelete);
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.delete("/api/quests", vm.questToDelete)
              .then(function (response) {
                  console.log(response.data);
                  angular.copy(response.data, vm.quests);
                  //success
              }, function () {
                  // failure
                  vm.errorMessage = "Failed to delete quest";
              })
              .finally(function () {
                  vm.isBusy = false;
              });

        };
        console.log(vm.deleteQuest);



    }

})();