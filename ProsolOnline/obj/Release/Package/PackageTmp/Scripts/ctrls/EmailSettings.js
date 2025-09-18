(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);
    app.controller('EmailSettingscontroller', function ($scope, $http, $timeout, $rootScope) {
        $scope.btnSubmit = true;
       // alert("hi");
        $scope.obj = { EnableSSL: false };
       // $scope.obj.ssl = 0;
        //alert(angular.toJson($scope.obj.EnableSSL));

        $http({
            method: 'GET',
            url: '/Settings/ShowEmaildata'
        }).success(function (response) {

            $scope.obj.FromId = response.FromId;
            $scope.obj.Password = response.Password;
            $scope.obj.ConformPassword = response.ConformPassword;
            $scope.obj.SMTPServerName = response.SMTPServerName;
            $scope.obj.PortNo = response.PortNo;
            $scope.obj.EnableSSL = response.EnableSSL;
            $scope.obj.cc = response.CC;
            $scope.obj.BCC = response.BCC;


        }).error(function (data, status, headers, config) {
        });
        $scope.reset = function () {

            $scope.form.$setPristine();
        };
        $scope.getcheck1 = function () {
            // alert(angular.toJson($scope.obj.ChangePassword));
            if ($scope.obj.ConformPassword !== '' && $scope.obj.ConformPassword !== undefined) {
                if ($scope.obj.Password === $scope.obj.ConformPassword) {
                    $scope.myres1 = false
                } else {
                    $scope.myres1 = true
                }
            }
            else {
                $scope.myres1 = false
            }
            //  alert(angular.toJson($scope.myres1));
        };
        $scope.sendmail = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 3000);
            if ($scope.obj.emailtest != null && $scope.obj.emailtest != '') {
                var formData = new FormData();
                formData.append("obj", angular.toJson($scope.obj));
                $http({
                    method: "POST",
                    url: "/Settings/SendEmail",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data == "true") {
                        $rootScope.Res = "Email Sent successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }

                    // alert(data);
                })
            } else {
                $rootScope.Res = "Please enter valid email address";
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }

        };


        $scope.CreateEmail = function () {
            if ($scope.obj.Password === $scope.obj.ConformPassword) {
                //alert("in");
                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
                var formData = new FormData();
                formData.append("obj", angular.toJson($scope.obj));
                $http({
                    method: "POST",
                    url: "/Settings/SubmitEmail",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    // alert(data);

                    if (data === "False") {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data Submited successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;

                        $('#divNotifiy').attr('style', 'display: block');
                        //$scope.obj.SAPCategorycode = "";
                        //$scope.obj.FromId = "";
                        //$scope.obj.Password = "";
                        //$scope.obj.ConformPassword = "";
                        //$scope.obj.SMTPServerName = "";
                        // $scope.obj.PortNo = "";
                        // $scope.obj.EnableSSL = "";
                        // $scope.obj.cc = "";
                        // $scope.obj.BCC = "";
                        $scope.reset();
                        $scope.obj = null;
                    }



                }).error(function (data, status, headers, config) {
                });
            }
        };
        
    });


    //app.factory("AutoCompleteService5", ["$http", function ($http) {
    //    return {
    //        search: function (term) {
    //            return $http({
    //                url: "/Settings/AutoCompleteEmailId?term=" + term,
    //                method: "GET"
    //            }).success(function (response) {
    //                return response.data;
    //            });
    //        }
    //    };
    //}]);
    //app.directive("autoComplete5", ["AutoCompleteService5", function (AutoCompleteService) {
    //    return {
    //        restrict: "A",
    //        link: function (scope, elem, attr, ctrl) {
    //            elem.autocomplete({
    //                source: function (searchTerm, response) {

    //                    AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

    //                        response($.map(autocompleteResults, function (autocompleteResult) {
    //                            return {
    //                                label: autocompleteResult.FromId,
    //                                value: autocompleteResult
    //                            }
    //                        }))
    //                    });
    //                },
    //                minLength: 1,
    //                select: function (event, selectedItem, http) {
    //                    scope.obj.FromId = selectedItem.item.FromId;

    //                    // $.get("/Dictionary/GetModifier?Noun=" + selectedItem.item.value).success(function (response) {
    //                    //  scope.Modifiers = response;                           
    //                    scope.$apply();
    //                    event.preventDefault();
    //                    //});

    //                }
    //            });

    //        }

    //    };
    //}]);



})();
