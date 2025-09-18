(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap', 'cgBusy']);


    app.controller('importcontroller', ['$scope', '$http', '$timeout', '$window', function ($scope, $http, $timeout, $window, DTOptionsBuilder) {

        //$scope.dtOptions = DTOptionsBuilder.newOptions()
        //   .withDisplayLength(10)
        //   .withOption('bLengthChange', true);

        $scope.files = [];
        $scope.GetLD = "";
        $scope.load = true;
        $scope.scrub = true;
        $scope.imp = true;



        $scope.filteredassign = [],
   $scope.currentPage = 1
, $scope.numPerPage = 10
, $scope.maxSize = 5;

        $scope.ddlItems = function () {
            $scope.chkSelected = false;
            $scope.numPerPage = $scope.selecteditem;
            $scope.slcteditem = [];
        };

        /*Loadfile for import*/
        $scope.LoadFileData = function (files) {
            $timeout(function () {
                $scope.NotifiyRes = false;
            }, 30000);
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
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }
        };

        /*Loadfile for import to load data for display*/
        $scope.Loadfile = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            if ($scope.files[0] != null) {
                $scope.GetLD = [];
                $scope.load = false;
                $scope.scrub = true;
                $scope.imp = true;
                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes1 = false; }, 5000);
                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                $scope.cgBusyPromises = $http({
                    url: "/File/Load_Data",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: block');
                    if (response == '') {
                        $scope.Res = "Load valid excel file";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                       
                    } else {

                      
                        $scope.dupload = response.loaddup;
                        $scope.lendup = $scope.dupload.length;
                        $scope.GetLD = response.loadndup;
                        $scope.len = $scope.GetLD.length;
                        $scope.Res = $scope.len + " Records Loaded Successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.$watch('currentPage + numPerPage', function () {
                            var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
                            $scope.filteredassign = $scope.GetLD.slice(begin, end);
                        });
                        /*Download duplicates of excel while loading*/
                        if (response.loaddup.length > 0) {
                            window.location.href = "/File/downloaddup";
                        }
                        $('.fileinput').fileinput('clear');
                    }
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                }
                ).error(function (data, status, headers, config) {
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                });
            } else {
                if ($scope.GetLD.length > 0)
                {
                    $scope.Res = "Data loaded";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;

                } else
                {
                    $scope.Res = "Load valid excel file";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                }
            }
        }


        /*Scrubing data of Import*/
        $scope.ScrubData = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            if ($scope.GetLD !== undefined && $scope.GetLD.length > 0) {
                $scope.load = true;
                $scope.scrub = false;
                $scope.imp = true;
                var formData = new FormData();
                formData.append('GetLD', angular.toJson($scope.GetLD));
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/File/ScrubData',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $scope.Res = "Data has been scrubed"
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.GetLD = data;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                    $scope.$watch('currentPage + numPerPage', function () {
                        var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
                        $scope.filteredassign = $scope.GetLD.slice(begin, end);
                    });
                }).error(function (data, status, headers, config) {
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                });
            } else {
                    $scope.Res = "No data avaliable for scrub";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
            }
        };

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        /*Importsubmit of Importpage*/
        $scope.importsubmit = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            if ($scope.GetLD != undefined && $scope.GetLD.length > 0) {
                $scope.load = true;
                $scope.scrub = true;
                $scope.imp = false;
                var formData = new FormData();
                formData.append('GetLD', angular.toJson($scope.GetLD));
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/File/Import_Submit',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                    /*Download duplicates comparing with db*/
                    if (response.length > 0) {
                        window.location.href = "/File/downloaddup";
                    }
                    $scope.Res = "Data has been Imported"
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.GetLD = "";
                    $scope.len = null;
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                    $scope.$watch('currentPage + numPerPage', function () {
                        var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
                        $scope.filteredassign = $scope.GetLD.slice(begin, end);
                    });
                }).error(function (data, status, headers, config) {
                    $scope.load = true;
                    $scope.scrub = true;
                    $scope.imp = true;
                });
            } else {
                $scope.Res = "No data avaliable to import";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }
    }]);


})();