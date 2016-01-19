// questEditorController.js
(function () {
    "use strict";

    angular.module("app-quests")
        .controller("questEditorController", questEditorController);

    function questEditorController($routeParams, $http) {
        var vm = this;

        vm.questName = $routeParams.questName;
        vm.reward = "";
        vm.errorMessage = "";
        vm.isBusy = true;
    }

})();