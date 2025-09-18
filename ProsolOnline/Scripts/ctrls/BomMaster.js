(function () {
    'use strict';
    //var app = angular.module('ProsolApp', ['datatables']);


    //app.controller('BomMasterController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout, $window) {
    var app = angular.module('ProsolApp', ['datatables']);




        app.controller('BomMasterController', function ($scope, $http, $timeout, $window ,$filter){


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.NotifiyRes = false;
            $scope.$apply();
            $scope.files = files;
           
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                  
                    angular.element("input[type='file']").val(null);
                   
                    $scope.Res = "Load valid excel file";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.$apply();
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }
        };


        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }


        $scope.BulkFunloc = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Bom/BulkFunloc",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.Res1 = data;
                  //  alert(angular.toJson($scope.Res1));
                    //  alert(data);
                  
                    if ($scope.Res1 == 0) {
                        $scope.Res = "Records already exists"
                        $scope.Notify = "alert-danger";
                    }
                    else {
                        $scope.Res = data + " Records uploaded successfully"
                    }

               
                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.BulkEquip = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Bom/BulkEquip",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.Res1 = data;
                    //  alert(angular.toJson($scope.Res1));
                    //  alert(data);

                    if ($scope.Res1 == 0) {
                        $scope.Res = "Records already exists"
                        $scope.Notify = "alert-danger";
                    }
                    else {
                        $scope.Res = data + " Records uploaded successfully"
                    }


                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }


        $scope.Download = function () {
            $window.location = '/Bom/Downloadmaster'
        }
    });


})();