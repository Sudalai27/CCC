(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables']);


    app.controller('unspscController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
     
        //$scope.dtOptions = DTOptionsBuilder.newOptions()
        //  .withDisplayLength(100)
        //  .withOption('bLengthChange', true);
        
        $scope.resdwn = false;
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {

            $scope.NotifiyRes = false;
            $scope.$apply();
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.Res = "Load valid excel file";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.$apply();
                }
            }
        };

        $scope.Bulkdata = function () {
          
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/Bulkdata_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (!data)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data;/*+ " Records uploaded successfully"*/


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                    $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                });
            };
        }
        $scope.Bulkdata = function () {
          
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/Bulkdata_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (!data)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data;/*+ " Records uploaded successfully"*/


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                    $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                });
            };
        }
        $scope.BulkAttribute = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/BulkAttribute",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data + " Records uploaded successfully"


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
       
        $scope.runEs = function () {


            $http({
                method: 'GET',
                url: '/Search/runElasticSearch'
            }).success(function (response) {
                
                if (response === 0)
                    $scope.Res = "Ealstic Search running 0 items"
                else $scope.Res = response + " Ealstic Search running items"


                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

    }]);


})();