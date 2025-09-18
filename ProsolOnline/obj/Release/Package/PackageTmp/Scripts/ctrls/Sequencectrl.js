
(function () {
    'use strict';
    var app = angular.module('ProsolApp', []);



    app.controller('SequenceController', ['$scope', '$http', '$timeout', '$filter', function ($scope, $http, $timeout, $filter) {

        $scope.Req_lst = ['No', 'Yes'];
        $scope.Sep_lst = [{'SeparatorName':'comma without space','Separator':',' }, {'SeparatorName':'semicolon withoutspace','Separator': ';' }, {'SeparatorName':'comma with space','Separator': ', '}, {'SeparatorName':'semicolon with space','Separator': '; '}];
      
        $scope.myshow = true;
        $scope.BindSeqList = function () {
            $http({
                method: 'GET',
                url: '/Sequence/GetSequenceList'
            }).success(function (response) {
                $scope.Lists = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindSeqList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createSequence = function () {

            //if (!$scope.form.$invalid) {   




            //};
            //alert($scope.result);
            $scope.Seqcheck = false;
            $scope.Seqcheck1 = false;
            $scope.Seqcheck2 = false;
            var arrList_Long = [];
            var arrList_Generic = [];
            var arrList_OEM = [];
            angular.forEach($scope.Lists, function (lst) {
                if (lst.Description == 'Long') {

                    if (arrList_Long.indexOf(lst.Seq) !== -1 || parseInt(lst.Seq) < 1 ||  parseInt(lst.Seq) > 15) {
                        $scope.Seqcheck = true;                       
                    }
                    arrList_Long.push(lst.Seq);

                }
                if (lst.Description == 'Short_Generic') {
                    if (arrList_Generic.indexOf(lst.Seq) !== -1 || parseInt(lst.Seq) < 1 || parseInt(lst.Seq) > 15) {
                        $scope.Seqcheck1 = true;
                    }
                    arrList_Generic.push(lst.Seq);

                }
                if (lst.Description == 'Short_OEM') {
                    if (arrList_OEM.indexOf(lst.Seq) !== -1 || parseInt(lst.Seq) < 1 || parseInt(lst.Seq) > 15) {
                        $scope.Seqcheck2 = true;
                    }
                    arrList_OEM.push(lst.Seq);

                }
            });

            if ($scope.Seqcheck == false && $scope.Seqcheck1 == false && $scope.Seqcheck2 == false) {

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append("seque", angular.toJson($scope.Lists));

                $http({
                    url: "/Sequence/InsertSequence",
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

                 }
            };
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;

            $scope.editSequence = function () {

                $scope.myshow = false;
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;


            };
        
    }]);

})();