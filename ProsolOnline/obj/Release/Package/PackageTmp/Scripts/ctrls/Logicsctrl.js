(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);

    
    app.controller('LogicsController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {


        $scope.BindAttributesList = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindAttributesList();

        $scope.rows = [{ 'slno': 1 }];
        $scope.addRow = function () {
         
            $scope.rows.push({ 'slno': $scope.rows.length + 1 });

        };

        $scope.RemoveRow = function (indx) {
            if ($scope.rows.length > 1) {
                $scope.rows.splice(indx, 1);
            }
            var cunt = 1;
            angular.forEach($scope.rows, function (value, key) {
                value.slno = cunt++;
              
            });
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createLogics = function () {


            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Logic", angular.toJson($scope.Logic));
            formData.append('Attributes', angular.toJson($scope.rows));
            $http({
                url: "/GeneralSettings/addLogic",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    alert(data.errors);
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }
                else {

                    $scope.reset();

                    $scope.Logic = null;
                    $scope.rows = null;
                  
                    $scope.rows = [{ slno: 1 }];
                    //angular.element("input[type='file']").val(null);
                    $scope.Res = "Logic created successfully"
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                   
                }



            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

            // }
        };

        $scope.ChangeModifier = function () {
          
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetLogic',
                params :{Noun: $scope.Logic.Noun, Modifier:$scope.Logic.Modifier}
            }).success(function (response) {
              
                if (response != "")
                {
                    $scope.Logic = response;
                    $scope.rows = response.Attributes;
                } else {
                    $scope.Logic.Generalterm = "";
                    $scope.Logic.Partno = "";
                    $scope.Logic.Refno = "";
                    $scope.Logic.Manufacturer = "";
                    $scope.rows = [{ 'slno': 1 }];
                }
               

            }).error(function (data, status, headers, config) {
                // alert("error");

            });


        };

    }]);



    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteNoun",
                    params:{term: term},
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Noun,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.Logic.Noun = selectedItem.item.value;

                        $.get("/Dictionary/GetModifier",{Noun: selectedItem.item.value}).success(function (response) {
                            scope.Modifiers = response;
                            scope.$apply();
                            event.preventDefault();
                        });

                    }
                });

            }

        };
    }]);

})();