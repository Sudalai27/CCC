
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'datatables']);

    app.controller('updatecontroller', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout, DTOptionsBuilder) {
        $scope.chkshow = $('#sess').val();

        $scope.create = true;
        $scope.obj = {};
        $scope.x = {};
        $scope.obj.Islive = 'Active';
        $scope.roles = [{ 'Name': 'Requester', 'Status': false, 'TargetId': null }, { 'Name': 'Approver', 'Status': false, 'TargetId': null }, { 'Name': 'Cataloguer', 'Status': false, 'TargetId': null }, { 'Name': 'Reviewer', 'Status': false, 'TargetId': null }, { 'Name': 'Releaser', 'Status': false, 'TargetId': null }, { 'Name': 'PV User', 'Status': false, 'TargetId': null }];
        $scope.Module = [{ 'Name': 'Material'}, { 'Name': 'Service'}, { 'Name': 'Asset'}, { 'Name': 'BOM'}, { 'Name': 'Business Partner'}];

        $scope.reset = function () {
            $scope.form.$setPristine();
        }

        $scope.getcheck = function getcheck() {
            if ($scope.obj.Password === $scope.obj.CPassword) {
                $scope.myres = false
            } else {
                $scope.myres = true
            }
        };
        $http.get('/User/getplant').success(function (response) {
            $scope.plant = response
            $scope.calshow();
        });//to get plant

        $http.get('/User/getdepartment').success(function (response) {
            $scope.gtdepartment = response
        });//to get department 


        //data to load all information of user

        $scope.calshow = function () {

            $http.get('/User/showall_user').success(function (response) {

                $scope.shwusr = response
                // var PlntArry = [];
                angular.forEach($scope.shwusr, function (lst) {
                    //  PlntArry = lst.Plantcode.split(",");

                    if (lst.Plantcode != null) {
                        var str = '';
                        angular.forEach($scope.plant, function (value, key) {
                            for (var f = 0; lst.Plantcode.length > f; f++) {
                                if (value.Plantcode === lst.Plantcode[f]) {
                                    str = str + value.Plantname + ' ';
                                    break;
                                }

                            }
                        })
                        lst.Plantcode = str
                    }

                    var rle = '';
                    angular.forEach(lst.Roles, function (lst1) {

                        rle = rle + lst1.Name + ' ';
                    })

                    lst.Roles = rle;

                })

            });


        }//data to load all information of user

        $scope.init = function () {
            if ($scope.response.Islive !== undefined) {
                $scope.Islive = response.Islive;
            }
            else {
                $scope.Islive = false;
            }
        }

        $scope.changeStatus = function () {
            $scope.Islive = !$scope.Islive;
        }

        $scope.checkusername = function () {
            if ($scope.obj.UserName != null) {

                $http({

                    method: 'GET',
                    url: '/User/Checkusernameavalibility',
                    params: { UserName: $scope.obj.UserName }
                }).success(function (response) {
                    if (response == "" || (response[0].Userid == $scope.obj.Userid && response[0].UserName == $scope.obj.UserName)) {
                        $scope.res = false;
                    }
                    else {
                        $scope.res = true;

                    }
                }).error(function (data, status, headers, config) {

                });
            }
        };// to get the Username Avalibility check

        $scope.checkemailid = function checkemailid() {

            if ($scope.obj.EmailId != null) {
                $http({
                    method: 'GET',
                    url: '/User/Checkemailidavalibility',
                    params: { EmailId: $scope.obj.EmailId }
                }).success(function (response) {

                    if (response != "") {
                        $scope.result2 = response[0].EmailId;
                        $scope.result1 = response[0].Userid;
                        if (response == "" || ($scope.result1 == $scope.obj.Userid && $scope.result2 == $scope.obj.EmailId)) {
                            $scope.res1 = false;
                        }
                        else {
                            $scope.res1 = true;
                        }
                    } else {
                        $scope.res1 = false;
                    }
                }).error(function (data, status, headers, config) {

                });
            }
        };// to get the Username Avalibility check

        $scope.checkSuperAdmin = function () {

            $scope.selection = [];
            if ($scope.chkSA == true) {
                $scope.selection.push({ 'Name': 'SuperAdmin', 'Status': true, 'TargetId': null });

                $scope.selection1 = null;
                $scope.shw = true;
            } else {
                $scope.shw = false;
                $scope.selection = '';
                $scope.selection1 = '';
            }

        }
        $scope.submit = function () {
          
            if ($scope.obj.ChangePassword === $scope.obj.Password) {

                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);
               
                if ($scope.obj != null && $scope.obj != "" && $scope.obj != '' && $scope.selection != '' && $scope.selection != undefined && $scope.selection1 != "" && $scope.selection1 != undefined || $scope.selection1 === null && $scope.Modules != undefined || $scope.Modules === null) {
                  
                    var formData = new FormData();
                    $scope.create = false;
                    formData.append("obj", angular.toJson($scope.obj));
                    formData.append("selection", angular.toJson($scope.selection));
                    formData.append("selection1", angular.toJson($scope.selection1));
                    formData.append("Modules", angular.toJson($scope.Modules));
                    var flg = 0;


                    if ($scope.roles[0].Name == "Requester" && $scope.roles[0].Status == true && $scope.roles[0].TargetId == null) {

                        flg = 1;
                    }

                    if ($scope.roles[1].Name == "Approver" && $scope.roles[1].Status == true && $scope.roles[1].TargetId == null) {

                        flg = 2;
                    }

                    if ($scope.roles[2].Name == "Cataloguer" && $scope.roles[2].Status == true && $scope.roles[2].TargetId == null) {

                        flg = 3;
                    }
                    if ($scope.roles[3].Name == "Reviewer" && $scope.roles[3].Status == true && $scope.roles[3].TargetId == null) {

                        flg = 4;
                    }



                    if (flg == 0) {
                        $scope.cgBusyPromises = $http({
                        method: 'POST',
                        url: '/User/save',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (data, status, headers, config) {
                        // alert(angular.toJson(data));
                        if (data === 'True') {
                            $scope.reset();
                            $scope.obj = {};
                            $scope.selection = [];
                            $scope.selection1 = [];
                            $scope.Module = [{ 'Name': 'Material' }, { 'Name': 'Service' }, { 'Name': 'Asset' }, { 'Name': 'BOM' }, { 'Name': 'Vendor' }];
                            $scope.obj.Islive = 'Active';
                            $scope.shw = false;
                            $scope.chkSA = false;
                            $scope.roles = [{ 'Name': 'Requester', 'Status': false, 'TargetId': null }, { 'Name': 'Approver', 'Status': false, 'TargetId': null }, { 'Name': 'Cataloguer', 'Status': false, 'TargetId': null }, { 'Name': 'Reviewer', 'Status': false, 'TargetId': null }, { 'Name': 'Releaser', 'Status': false, 'TargetId': null }];
                            $scope.Res = "User has been created successfully"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.calshow();
                            $scope.create = true;
                            $('#divNotifiy').attr('style', 'display: block');


                            $http.get('/User/getplant').success(function (response) {
                                $scope.gtplant = response
                                $scope.plant = response
                                angular.forEach($scope.plant, function (value, key) {
                                    value.Islive = false;
                                });
                            });
                        //} else {

                        //    $scope.Res = "You have completed 10 user creation limit"
                        //    $scope.Notify = "alert-danger";
                        //    $scope.NotifiyRes = true;
                        //    $('#divNotifiy').attr('style', 'display: block');

                        }


                    }).error(function (data, status, headers, config) {

                    });
                } else if (flg == 1) {
                    $scope.Res = "Select Approver"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (flg == 2) {
                    $scope.Res = "Select Cataloguer"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (flg == 3) {
                    $scope.Res = "Select Reviewer"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (flg == 4) {
                    $scope.Res = "Select Releaser"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
            }


            else {

                if ($scope.selection == '' || $scope.selection === undefined) {
                    $scope.Res = "Select User Role"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if ($scope.Modules == '' || $scope.Modules === undefined) {
                    $scope.Res = "Select Module Access"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if ($scope.selection1 == '' || $scope.selection1 === undefined) {
                    $scope.Res = "Select Plant"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }

            }

        }
            //    } else {
            //        //if ($scope.selection == '' || $scope.selection === undefined)
            //        //{
            //        $scope.Res = "Select user role and/or plant"
            //        $scope.Notify = "alert-danger";
            //        $scope.NotifiyRes = true;
            //        $('#divNotifiy').attr('style', 'display: block');
            //        //}

            //    }
            //}
        }// to add the access for user and registration


        $scope.search = function (id) {
           
            if (id != null) {
                $http({
                    method: 'GET',
                    url: '/User/drop',
                    params: { id: id }
                }).success(function (response) {
                    $scope.update = true;
                    $scope.create = false;
                    $scope.res = false;
                    $scope.obj = response;
                    $scope.obj.ChangePassword = $scope.obj.Password;

                    angular.forEach($scope.roles, function (value, key) {

                        for (var f = 0; $scope.obj.Roles.length > f; f++) {
                            if (value.Name === $scope.obj.Roles[f].Name) {
                                value.Status = true;
                                value.TargetId = $scope.obj.Roles[f].TargetId;
                                break;
                            }
                            else {
                                value.Status = false;
                                value.TargetId = "";
                            }
                        }
                    });
                    if (response.Modules != null) {
                        $scope.Modules = [];
                        angular.forEach($scope.Module, function (value, key) {
                            for (var f = 0; response.Modules.length > f; f++) {
                                if (value.Name === response.Modules[f]) {
                                    value.Status = true;
                                    $scope.Modules.push(value.Name);
                                    break;
                                }
                                else {
                                    value.Status = false;
                                }
                            }
                        });
                    }
                    else {
                        $scope.Module = [{ 'Name': 'Material' }, { 'Name': 'Service' }, { 'Name': 'Asset' }, { 'Name': 'BOM' }, { 'Name': 'Vendor' }];

                    }
                    if (response.Plantcode != null) {
                        $scope.selection1 = [];
                        angular.forEach($scope.plant, function (value, key) {
                            for (var f = 0; response.Plantcode.length > f; f++) {
                                if (value.Plantcode === response.Plantcode[f]) {
                                    value.Islive = true;
                                    $scope.selection1.push(value.Plantcode);
                                    break;
                                }
                                else {
                                    value.Islive = false;
                                }
                            }
                        });
                       
                        $http.get('/User/getuser?plants=' + angular.toJson($scope.selection1)).success(function (response) {
                            $scope.getuser = response
                        });//to get user
                        $scope.chkSA = false;
                        $scope.shw = false;
                    }
                    else {

                        $scope.chkSA = true;
                        $scope.roles = [{ 'Name': 'Requester', 'Status': false, 'TargetId': null }, { 'Name': 'Approver', 'Status': false, 'TargetId': null }, { 'Name': 'Cataloguer', 'Status': false, 'TargetId': null }, { 'Name': 'Reviewer', 'Status': false, 'TargetId': null }, { 'Name': 'Releaser', 'Status': false, 'TargetId': null }];
                        $scope.selection1 = null;
                        $scope.shw = true;
                        $scope.Module = [{ 'Name': 'Material' }, { 'Name': 'Service' }, { 'Name': 'Asset' }, { 'Name': 'BOM' }, { 'Name': 'Vendor' }];

                    }


                }).error(function (data, status, headers, config) {

                });
            }// to get the access of user
        }

        $scope.check1 = function () {
            $scope.selection = [];
            if ($scope.chkSA == true) {
                $scope.selection.push({ 'Name': 'SuperAdmin', 'Status': true, 'TargetId': null });
            } else {
                angular.forEach($scope.roles, function (obj) {
                    if (obj.Status == true) {
                        $scope.selection.push(obj);
                    }
                });
            }

        }
        $scope.check2 = function () {
            $scope.Modules = [];
            angular.forEach($scope.Module, function (obj) {
                if (obj.Status == true) {
              
                    $scope.Modules.push(obj.Name);
                }
            });

        }
        $scope.checkplant1 = function () {

            $scope.selection1 = [];

            angular.forEach($scope.plant, function (value, key) {
                if (value.Islive == true) {
                    $scope.selection1.push(value.Plantcode);

                }
            });
            $http.get('/User/getuser', { params: { plants: angular.toJson($scope.selection1) } }).success(function (response) {
                $scope.getuser = response
            });//to get user
        }

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.edit = function () {
            if ($scope.obj.ChangePassword === $scope.obj.Password) {

                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);
                $scope.check2();
                var length = "";
                var ln = "";
                var formData = new FormData();
                formData.append("obj", angular.toJson($scope.obj));
                formData.append("Modules", angular.toJson($scope.Modules));
               
                //formData.append('Plant', $scope.obj.Plantcode);
                $scope.check1();
                if ($scope.selection === undefined || $scope.selection == "") {
                    formData.append("selection", angular.toJson($scope.obj.Roles));
                    length = 0;
                }
                else {
                    formData.append("selection", angular.toJson($scope.selection));
                    length = $scope.selection.length;
                }

                if ($scope.selection1 === undefined || $scope.selection1 == "") {

                    formData.append("selection1", angular.toJson($scope.obj.Plantcode));
                    ln = 0;
                }
                else {
                    if ($scope.selection1 == null) {
                        ln = 1;
                    } else {
                        formData.append("selection1", angular.toJson($scope.selection1));
                        ln = $scope.selection1.length;
                    }
                }

                // $scope.checkplant1();

                // formData.append("selection1", angular.toJson($scope.selection1));


                if ($scope.obj != '' && length != 0 && ln != 0 && $scope.Modules != undefined || $scope.Modules === null) {

                    var flg = 0;


                    if ($scope.roles[0].Name == "Requester" && $scope.roles[0].Status == true && $scope.roles[0].TargetId == "" || $scope.roles[0].TargetId == null) {

                        flg = 1;
                    }

                    if ($scope.roles[1].Name == "Approver" && $scope.roles[1].Status == true && $scope.roles[1].TargetId == "" || $scope.roles[1].TargetId == null) {

                        flg = 2;
                    }

                    if ($scope.roles[2].Name == "Cataloguer" && $scope.roles[2].Status == true && $scope.roles[2].TargetId == "" || $scope.roles[2].TargetId == null) {

                        flg = 3;
                    }
                    if ($scope.roles[3].Name == "Reviewer" && $scope.roles[3].Status == true && $scope.roles[3].TargetId == "" || $scope.roles[3].TargetId == null) {

                        flg = 4;
                    }
                    if (flg == 0) {
                    length = "";
                    ln = "";

                    $scope.cgBusyPromises = $http({
                        method: 'post',
                        url: '/User/edit',
                        params: { id: $scope.obj.Userid },
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (data, status, headers, config) {
                        $scope.update = false;
                        $scope.create = true;
                        $scope.reset();
                        $scope.shw = false;
                        $scope.chkSA = false;
                        $scope.roles = [{ 'Name': 'Requester', 'Status': false, 'TargetId': null }, { 'Name': 'Approver', 'Status': false, 'TargetId': null }, { 'Name': 'Cataloguer', 'Status': false, 'TargetId': null }, { 'Name': 'Reviewer', 'Status': false, 'TargetId': null }, { 'Name': 'Releaser', 'Status': false, 'TargetId': null }];
                        $scope.obj = {};
                        $scope.Module = [{ 'Name': 'Material' }, { 'Name': 'Service' }, { 'Name': 'Asset' }, { 'Name': 'BOM' }, { 'Name': 'Vendor' }];
                        $scope.selection = [];
                        $scope.selection1 = [];
                        $scope.Modules = [];
                        $scope.obj.Islive = 'Active';
                        $scope.Res = "User has been updated successfully"
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.calshow();
                        $('#divNotifiy').attr('style', 'display: block');

                        $http.get('/User/getplant').success(function (response) {
                            $scope.gtplant = response
                            $scope.plant = response
                            angular.forEach($scope.plant, function (value, key) {
                                value.Islive = false;
                            });
                        });

                    }).error(function (data, status, headers, config) {

                    });
                } else if (flg == 1) {
                        $scope.Res = "Select Approver"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.create = true
                    }
                if (flg == 2) {
                    $scope.Res = "Select Cataloguer"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (flg == 3) {
                    $scope.Res = "Select Reviewer"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (flg == 4) {
                    $scope.Res = "Select Releaser"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
            }
            else {
                    if ($scope.Modules == '' || $scope.Modules === undefined) {
                        $scope.Res = "Select Module Access"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.create = true
                    }
                if (length == 0) {
                    $scope.Res = "Select user role"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
                if (ln == 0) {
                    $scope.Res = "Select plant"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.create = true
                }
            }
        }
        }
        //        }
        //        else {
        //            // if ($scope.length == 0) {
        //            $scope.Res = "Select user role and/or plant"
        //            $scope.Notify = "alert-danger";
        //            $scope.NotifiyRes = true;
        //            $('#divNotifiy').attr('style', 'display: block');
        //            //}
        //        }
        //    }
        //}// to edit and update of user

        $scope.getcheck1 = function () {
            // alert(angular.toJson($scope.obj.ChangePassword));
            if ($scope.obj.ChangePassword !== '' && $scope.obj.ChangePassword !== undefined) {
                if ($scope.obj.Password === $scope.obj.ChangePassword) {
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

    }]);

    //app.controller('AccountController', ['$scope', '$http', '$timeout', '$rootScope', function ($scope, $http, $timeout, $rootScope) {
    //    $scope.myres = false
    //    $scope.files = [];
    //    $http.get('/User/AccountUser').success(function (response) {
    //        $scope.shwusr = response;
    //        angular.forEach($scope.shwusr, function (lst) {
    //            $scope.userroleadd = lst.Usertype.join();
    //        })
    //        $scope.Usertype = $scope.userroleadd;
    //        angular.forEach($scope.shwusr, function (lst) {
    //            $scope.UserName = lst.UserName;
    //            $scope.FirstName = lst.FirstName;
    //            $scope.LastName = lst.LastName;
    //            $scope.EmailId = lst.EmailId;
    //            $scope.Mobile = lst.Mobile;
    //            $scope.Plantcode = lst.Plantcode;
    //            $scope.Departmentname = lst.Departmentname;
    //            $scope.Islive = lst.Islive;
    //            $scope.Password = lst.Password;

    //            $scope.Pass = lst.Password;
    //        })

    //    });

    //    $http.get('/User/getprofilepicture').success(function (response) {

    //        $scope.FileData = response.FileData;
    //        $rootScope.FileData = response.FileData;
    //    });
    //    $scope.clear = function () {
    //        $scope.NewPassword = "";
    //        $scope.ConfirmPassword = "";
    //        $scope.Passwordchng = "";
    //    }

    //    $scope.LoadFileData = function (files) {
    //        $scope.files = files;
    //        var reader = new FileReader();
    //        var fileUpload = document.getElementById("fileUpload");
    //        if (typeof (fileUpload.files) != "undefined") {
    //            var size = parseFloat(fileUpload.files[0].size / 1024).toFixed(2);
    //            alert(size + " KB.");

    //            if (files[0] != null) {
    //                var flg = 0;
    //                var ext = files[0].name.match(/\.(.+)$/)[1];
    //                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg' || angular.lowercase(ext) === 'png') {
    //                    flg = 1;
    //                }
    //                if (flg == 1) {
    //                    reader.onload = function (event) {
    //                        $scope.image_source = event.target.result
    //                        $scope.$apply()
    //                    }
    //                    if (files[0] == null) {

    //                        $scope.image_source = null;
    //                        $scope.$apply()
    //                    } else {
    //                        reader.readAsDataURL(files[0]);
    //                    }
    //                }
    //                else {
    //                    angular.element("input[type='file']").val(null);
    //                    $scope.image_source = null;
    //                    $scope.$apply()
    //                    files[0] = null;
    //                }
    //            } else {
    //                $scope.image_source = null;
    //                $scope.$apply()
    //            }
    //        }
    //        else {
    //            alert("This browser does not support HTML5.");
    //        }

    //    };

    //    $scope.checkcurrentpassword = function checkcurrentpassword() {
    //        if ($scope.Passwordchng != '' && $scope.Passwordchng != null && $scope.Passwordchng != "") {
    //            if ($scope.Pass != undefined) {
    //                if ($scope.Passwordchng === $scope.Pass) {
    //                    $scope.res = false
    //                }
    //                else {
    //                    $scope.res = true
    //                }
    //            }
    //        }
    //    }

    //    $scope.getcheck = function getcheck() {
    //        if ($scope.NewPassword === $scope.ConfirmPassword) {
    //            $scope.myres = false
    //        } else {
    //            $scope.myres = true
    //        }
    //    }

    //    $scope.Profilesubmit = function () {
    //        $timeout(function () {
    //            $('#divNotifiy').attr('style', 'display: none');
    //        }, 30000);
    //        var formData = new FormData();
    //        formData.append("FirstName", $scope.FirstName);
    //        formData.append("LastName", $scope.LastName);
    //        formData.append("Mobile", $scope.Mobile);
    //        formData.append('image', $scope.files[0]);
    //        $http({
    //            method: 'POST',
    //            url: '/User/Profilesubmit',
    //            headers: { "Content-Type": undefined },
    //            transformRequest: angular.identity,
    //            data: formData,
    //        }).success(function (data, status, headers, config) {
    //            $scope.Res = "Profile has been Updated successfully"
    //            $scope.Notify = "alert-info";
    //            $scope.NotifiyRes = true;
    //            $('#divNotifiy').attr('style', 'display: block');
    //            $http.get('/User/getprofilepicture').success(function (response) {

    //                $scope.FileData = response.FileData;
    //                $rootScope.FileData = response.FileData;
    //            });
    //        }).error(function (data, status, headers, config) {
    //        });
    //    }

    //    $scope.NotifiyResclose = function () {
    //        $('#divNotifiy').attr('style', 'display: none');
    //    }

    //    $scope.Changepasswordsubmit = function ()
    //    {
    //        $timeout(function () {
    //            $('#divNotifiy').attr('style', 'display: none');
    //        }, 30000);
    //        if ($scope.ConfirmPassword === $scope.NewPassword) {
    //            var formData = new FormData();
    //            formData.append("ConfirmPassword", $scope.ConfirmPassword);
    //            $http({
    //                method: 'POST',
    //                url: '/User/Changepasswordsubmit',
    //                headers: { "Content-Type": undefined },
    //                transformRequest: angular.identity,
    //                data: formData,
    //            }).success(function (data, status, headers, config) {
    //                $scope.Res = "Password has been changed successfully"
    //                $scope.Notify = "alert-info";
    //                $scope.NotifiyRes = true;
    //                $scope.clear();
    //                $('#divNotifiy').attr('style', 'display: block');
    //            }).error(function (data, status, headers, config) {
    //            });
    //        }
    //    }


    //}]);
})();