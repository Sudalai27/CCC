(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);
    app.controller('ServiceReportController', function ($scope, $http, $rootScope, $timeout, $filter, $window) {

        $scope.Fromdate = "";
        $scope.Todate = "";
        $scope.role = "Select Role";
        $scope.getuser = [];
        $scope.options = [{ 'Name': 'Pending', 'Status': false }, { 'Name': 'Completed', 'Status': false }, { 'Name': 'Rejected', 'Status': false }];
        $scope.PlantCode = "";
        $scope.Userid = "";


        $http.get('/Master/GetDataListplnt'
    ).success(function (response) {
        // alert(angular.toJson(response));
        $scope.getplantt = response;
        // alert(angular.toJson($scope.getplantt));
        $scope.getplantt = $filter('filter')($scope.getplantt, { 'Islive': 'true' })
    }).error(function (data, status, headers, config) {
    });

        $scope.getuserr = function (role) {
           // alert(role);
            if (role == "Cleanser") {
                role = "Cataloguer";
            }
          //  alert(role);
            $http.get('/ServiceReport/getuser?role=' + role
            ).success(function (response) {
                $scope.getuser = response;
               // alert(angular.toJson($scope.getuser));
                $scope.getuser = $filter('filter')($scope.getuser, { 'Islive': 'Active' })
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.setrole = function () {
            $scope.role = $("#roledll").find("option:selected").text();
          //  alert(angular.toJson($scope.role));
            $scope.getuserr($scope.role);
        };

        $scope.check1 = function () {
            $scope.selection = [];
            angular.forEach($scope.options, function (value, key) {
                if (value.Status == true) {
                    $scope.selection.push(value.Name);
                }
            });
        };

        $scope.ReportSearch = function () {
           // alert("hi");
          //  alert(angular.toJson($scope.Fromdate));
            var formdata = new FormData();
           // alert(angular.toJson($scope.PlantCode));
            if ($scope.PlantCode != "undefined" && $scope.PlantCode != null && $scope.PlantCode != "") {
                formdata.append("PlantCode", angular.toJson($scope.PlantCode));
                //alert("in");
            }
            else {
                formdata.append("PlantCode", "");
            }
            if ($scope.role != "undefined" && $scope.role != null && $scope.role != "") {

                formdata.append("role", angular.toJson($scope.role));
                //alert(angular.toJson($scope.role));
            }
            else {
                formdata.append("role", "");
            }
            if ($scope.Userid != "undefined" && $scope.Userid != null && $scope.Userid != "") {

                formdata.append("Userid", angular.toJson($scope.Userid));
               // alert(angular.toJson($scope.Userid));
            }
            else {
                formdata.append("Userid", "");
            }
            formdata.append("selection", angular.toJson($scope.selection));
           // alert(angular.toJson($scope.selection));

            if ($scope.Fromdate != "" && $scope.Fromdate != null && $scope.Fromdate != "unknown") {
                formdata.append("Fromdate", angular.toJson($scope.Fromdate.toISOString()));

                formdata.append("Todate", angular.toJson($scope.Todate.toISOString()));
              //  alert(angular.toJson($scope.Fromdate));
            }
            else
            {
                formdata.append("Fromdate", "");
                formdata.append("Todate", "");
            }

            $scope.cgBusyPromises = $http({
                url: "/ServiceReport/ServiceReportSearch",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formdata
            }).success(function (response) {
                if (response != null && response != "") {
                    $scope.Reportlist = response;
                }
                else
                {
                    $rootScope.Res = "No data available to load";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.Reportlist = null;
                }
                
            });

        };

        //cancel
        $scope.clearfields = function () {

            $scope.Reportlist = null;
            $scope.PlantCode = "";
            $scope.role = "Select Role";
            $scope.options = [{ 'Name': 'Pending', 'Status': false }, { 'Name': 'Completed', 'Status': false }, { 'Name': 'Rejected', 'Status': false }];
            $scope.Fromdate = "";
            $scope.Todate = "";
            $scope.Userid = "";
            $scope.getuser = [];

  };


        $scope.ServiceExport = function () {

           // alert("hai");
          //  alert(angular.toJson($scope.options));
            if ($scope.selection == undefined) {
                $scope.selection = [];
                angular.forEach($scope.options, function (x) {
                    $scope.selection.push(x.Name);
                });
            }

            if ($scope.Fromdate != undefined || $scope.PlantCode != undefined && $scope.options != "undefined") {

            var fdate = null, tdate = null;
            if ($scope.Fromdate != undefined)
                fdate = $scope.Fromdate;
            if ($scope.Todate != undefined)
                tdate = $scope.Todate;



            //  $window.location = '/Report/DownloadServiceReport?PlantCode=' + $scope.PlantCode + '&Fromdate=' + fdate + '&Todate=' + tdate + '&option=' + $scope.option.Status;
            //$window.location = '/ServiceReport/DownloadServiceReport?PlantCode=' + $scope.PlantCode + '&Fromdate=' + fdate + '&Todate=' + tdate + '&options=' + $scope.options + '&role=' + $scope.role + '&Userid=' + $scope.Userid;

            $scope.cgBusyPromises = $window.location = '/ServiceReport/DownloadServiceReport?PlantCode=' + $scope.PlantCode + '&Fromdate=' + fdate + '&Todate=' + tdate + '&options=' + $scope.selection + '&Where=UserName&Value=' + $scope.value + '&role=' + $scope.role + '&Userid=' + $scope.Userid;

            // }
            //    else {
            //        if ($scope.Plant == undefined) {
            //            $scope.Res = "Choose Plant";
            //            $scope.Notify = "alert-danger";
            //            $scope.NotifiyRes = true;
            //            $('#divNotifiy').attr('style', 'display: block');
            //        }
            //        else if ($scope.Fromdate == undefined) {
            //            $scope.Res = "Select FromDate";
            //            $scope.Notify = "alert-danger";
            //            $scope.NotifiyRes = true;
            //            $('#divNotifiy').attr('style', 'display: block');
            //        }
            //        else if ($scope.Option == undefined) {
            //            $scope.Res = "Select Option";
            //            $scope.Notify = "alert-danger";
            //            $scope.NotifiyRes = true;
            //            $('#divNotifiy').attr('style', 'display: block');
            //        }
            //        else {
            //            $scope.Res = "Provide Search Datas";
            //            $scope.Notify = "alert-danger";
            //            $scope.NotifiyRes = true;
            //            $('#divNotifiy').attr('style', 'display: block');
            //        }

               }
        }

    });
    


})();