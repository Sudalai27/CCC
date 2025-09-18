
(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);


    app.controller('UOMController', ['$scope', '$http', '$timeout', '$filter', function ($scope, $http, $timeout, $filter) {

      
       

        $scope.myshow = true;
        $scope.BindSeqList = function () {
            $http({
                method: 'GET',
                url: '/Sequence/GetUOMSettings'
            }).success(function (response) {
                $scope.x = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindSeqList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createUOMSettings = function () {   
          

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append("xSet", angular.toJson($scope.x));

                $http({
                    url: "/Sequence/InsertUOMSettings",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data.success === false) {
                        $scope.Res = data.errors;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }
                    else {
                        if (data === false)
                            $scope.Res = "Already exists";
                        else {
                            $scope.Res = "Updated successfully";
                            $scope.BindSeqList();
                            $scope.myshow = true;
                            $scope.btnSubmit = true;
                            $scope.btnUpdate = false;
                        }
                        $scope.reset();
                        $scope.seq = null;

                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                });

           
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;

        $scope.editUOMSettings = function () {

            $scope.myshow = false;
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;


        };

    }]);

})();