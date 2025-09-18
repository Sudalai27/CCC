
(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);

    app.controller('CatalogueController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

        $scope.GetItemsList = function () {

            $.ajax({
                type: "POST",
                url: 'GetItemList',
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response;

                    var $container = $("#myHandsonTable");
                    $container.handsontable({
                        data: data,
                        startRows: 5,
                        startCols: 5,
                        colHeaders: true,
                        rowHeaders: true,
                        contextMenu: true,
                        manualColumnResize: true,
                        manualRowResize: true,
                        dropdownMenu: true,
                        filters: true,
                        fixedColumnsLeft: 1,
                         colWidths:250,

                    });
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            $scope.btnexcelLoad = false;
            $scope.btnexcelsave = true;

        };
        $scope.GetItemsList();

        $scope.SaveItemsList = function () {
          
            var $container = $('#myHandsonTable');
            var ht = $container.handsontable('getInstance');
            var htContents = ht.getData();
            //alert(htContents);
            $.ajax({
                type: "POST",
                url: 'SaveItemsList',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ 'data': '" + JSON.stringify(htContents) + "' }",
                success: function (data, status, headers, config) {

                    if (data == true) {
                      
                        $scope.Res = "Data saved successfully"
                        $scope.$apply();
                       
                    } else {

                        $scope.Res = "Save failed"                      
                    }
                },
                failure: function (response) {
                    $scope.Res = "Save failed"
                   
                }
            });

        };

    }]);

})();