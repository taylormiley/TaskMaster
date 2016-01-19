// app-quests.js
(function () {

    "use strict";

    angular.module("app-quests", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {
        
            $routeProvider.when("/", {
                controller: "questsController",
                controllerAs: "vm",
                templateUrl: "/views/questsView.html"
            });

            $routeProvider.when("/editor/:questName", {
                controller: "questEditorController",
                controllerAs: "vm",
                templateUrl: "/views/questEditorView.html"
            });

            $routeProvider.otherwise({ redirectTo: "/"});

        });

})();
