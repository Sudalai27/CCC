(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables','cgBusy']);


    app.controller('CatalogueController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

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
           // alert("hai");
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/GeneralSettings/Bulkdata_Upload",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.BulkSingleAttribute = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/BulkSingleAttribute",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.BulkAttribute = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/BulkAttribute",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.Vendordata = function () {
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; },5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/Vendordata_Upload",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.Equipdata = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/Equipdata_Upload",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.ShortLong = function () {
          

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/ShortLong",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data)
                        $scope.Res = data;
                    else $scope.Res = data;


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.BulkUrl = function () {
          

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/BulkUrl",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data)
                        $scope.Res = data;
                    else $scope.Res = data;


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.BulkNM = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/Catalogue/BulkNM",
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
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
    }]);


})();