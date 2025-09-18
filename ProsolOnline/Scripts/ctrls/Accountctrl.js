(function () {
    'use strict';
    var app = angular.module('ProsolApp',[]); 

    app.controller('AccountController', ['$scope', '$http', '$timeout', '$rootScope', function ($scope, $http, $timeout, $rootScope) {
        $scope.myres = false
        $scope.files = [];
        $http.get('/User/AccountUser').success(function (response) {
            $scope.shwusr = response;
            angular.forEach($scope.shwusr, function (lst) {
                $scope.UserName = lst.UserName;
                $scope.FirstName = lst.FirstName;
                $scope.LastName = lst.LastName;
                $scope.EmailId = lst.EmailId;
                $scope.Mobile = lst.Mobile;
                $scope.Plantcode = lst.Plantcode;
                $scope.Departmentname = lst.Departmentname;
                $scope.Islive = lst.Islive;
                $scope.Password = lst.Password;
                $scope.Pass = lst.Password;

                var rle = '';
                angular.forEach(lst.Roles, function (lst) {

                    rle = rle + lst.Name + '|';
                })
                //  $scope.Usertype = rle;
                $scope.Usertype = rle.slice(0, -1);
                //var rle = '|';
                //angular.forEach(lst.Roles, function (lst) {

                //    rle = rle + lst.Name + '|';
                //})
                //$scope.Usertype = rle;
            })
        });


        $http.get('/User/getprofilepicture').success(function (response) {

            $scope.FileData = response.FileData;
            $rootScope.FileData = response.FileData;
        });
        $scope.clear = function () {
            $scope.NewPassword = "";
            $scope.ConfirmPassword = "";
            $scope.Passwordchng = "";
            $scope.form.$setPristine();
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        /*To load the picture*/
        $scope.LoadFileData = function (files) {
            $scope.files = files;
            var reader = new FileReader();
            if (typeof (fileUpload.files) != "undefined") {
                $scope.size = parseFloat(fileUpload.files[0].size / 1024).toFixed(2);
                if (files[0] != null && $scope.size < 60.00) {
                    var flg = 0;
                    var ext = files[0].name.match(/\.(.+)$/)[1];
                    if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg' || angular.lowercase(ext) === 'png') {
                        flg = 1;
                    }
                    if (flg == 1) {
                        reader.onload = function (event) {
                            $scope.image_source = event.target.result
                            $scope.$apply()
                        }
                        if (files[0] == null) {
                            $scope.image_source = null;
                            $scope.$apply()
                        } else {
                            reader.readAsDataURL(files[0]);
                        }
                    }
                    else {
                        angular.element("input[type='file']").val(null);
                        $scope.image_source = null;
                        $scope.$apply()
                        files[0] = null;
                    }
                } else {
                    $scope.Res = "Upload maximum 60KB file"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.image_source = null;
                    $scope.$apply()
                }
                }
        };

        /*To Check current password*/
        $scope.checkcurrentpassword = function checkcurrentpassword() {
            if ($scope.Passwordchng != '' && $scope.Passwordchng != null && $scope.Passwordchng != "") {
                if ($scope.Pass != undefined) {
                    if ($scope.Passwordchng === $scope.Pass) {
                        $scope.res = false
                    }
                    else {
                        $scope.res = true
                    }
                }
            }
        }

        /*To Check new password and confirm password*/
        $scope.getcheck = function getcheck() {
            if ($scope.NewPassword === $scope.ConfirmPassword) {
                $scope.myres = false
            } else {
                $scope.myres = true
            }
        }


        /*To submit for profile*/
        $scope.Profilesubmit = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("FirstName", $scope.FirstName);
            formData.append("LastName", $scope.LastName);
            formData.append("Mobile", $scope.Mobile);
            formData.append('image', $scope.files[0]);
            $http({
                method: 'POST',
                url: '/User/Profilesubmit',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (data, status, headers, config) {
                $scope.Res = "Profile has been Updated successfully"
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                $http.get('/User/getprofilepicture').success(function (response) {

                    $scope.FileData = response.FileData;
                    $rootScope.FileData = response.FileData;
                });
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        /*To submit for Changepassword */
        $scope.Changepasswordsubmit = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.Passwordchng != $scope.Pass) {

                $scope.Res = " Current Password Worng"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
            else{
            if ($scope.ConfirmPassword === $scope.NewPassword) {
                var formData = new FormData();
                formData.append("ConfirmPassword", $scope.ConfirmPassword);
                $http({
                    method: 'POST',
                    url: '/User/Changepasswordsubmit',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (data, status, headers, config) {
                    $scope.Res = "Password has been changed successfully"
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.clear();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                });
            }
        }
        }

    }]);
})();