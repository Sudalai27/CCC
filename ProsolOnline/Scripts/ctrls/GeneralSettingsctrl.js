
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap', 'datatables', 'cgBusy']);


    app.controller('GroupCodeController', function ($scope, $http, $timeout, DTOptionsBuilder, $rootScope, $filter) {

        $("#txtFrom").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.Fromdate = $('#txtFrom').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                $("#txtTo").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });
        $("#txtTo").datepicker({
            numberOfMonths: 1,          
        });

        $scope.selecteditem = 10;
        $scope.SearchGroupList = function () {
            $scope.BindGroupcodeList();
        }

        $scope.BindGroupcodeList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetGroupListSearch',
                    params:{srchtxt:$scope.srchText, currentPage:1 , maxRows:$scope.selecteditem}
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Groupcodes = response.GroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.Groupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetGroupcodeList1',
                    params :{currentPage:1 , maxRows :$scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Groupcodes = response.GroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.Groupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindGroupinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetGroupListSearch',
                    params : {srchtxt:$scope.srchText,currentPage:inx,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Groupcodes = response.GroupList;
                    }
                    else $scope.Groupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetGroupcodeList1',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                    //?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Groupcodes = response.GroupList;
                    }
                    else $scope.Groupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };

        $scope.ddlItems = function () {
            $scope.BindGroupcodeList();
        };
        //$scope.BindGroupcodeList = function () {

        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetGroupcodeList1'
        //    }).success(function (response) {
        //        $scope.Groupcodes = response;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //};
        $scope.BindGroupcodeList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createGroupCode = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("grp", angular.toJson($scope.grp));

            $http({
                url: "/GeneralSettings/InsertGroupcode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Group already exists";
                    else {
                        $rootScope.Res = "Group created successfully";
                        $scope.BindGroupcodeList();
                    }
                    $scope.reset();
                    $scope.grp = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.GroupCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelGroupcode',
                    params:{id:_id}
                }).success(function (response) {
                    $rootScope.Res = "Group deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindGroupcodeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.GroupCodeedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Groupcodes, function (lst) {
                $('#grp' + i).attr("style", "");
                i++;
            });
            $('#grp' + idx).attr("style", "background-color:lightblue");

            $scope.grp = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.grp._id = x._id;
            $scope.grp.code = x.code;
            $scope.grp.title = x.title;


        };
        $scope.updateGroupCode = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes1 = false; }, 5000);

            var formData = new FormData();
            formData.append("grp", angular.toJson($scope.grp));

            $http({
                url: "/GeneralSettings/InsertGroupcode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Group already exists";
                    else {
                        $rootScope.Res = "Group updated successfully";
                        $scope.BindGroupcodeList();
                    }
                    $scope.reset();
                    $scope.grp = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.grp = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
    });
    app.controller('SubGroupCodeController', function ($scope, $http, $timeout, DTOptionsBuilder, $rootScope) {

        // DataTables configurable options
        //$scope.dtOptions = DTOptionsBuilder.newOptions()
        //    .withDisplayLength(10)
        //    .withOption('bLengthChange', true);
        $scope.selecteditem = 10;
        $scope.SearchSubGroupList = function () {
            $scope.BindSubGroupcodeList();
        }

        $scope.BindGroupcodeList = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetGroupcodeList'
            }).success(function (response) {
                $scope.GroupcodeList = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $rootScope.OnclickSubGroup = function () {
            $scope.BindGroupcodeList();
            $scope.BindSubGroupcodeList();
        };


        $scope.BindSubGroupcodeList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubGroupListSearch',
                    params:{srchtxt: $scope.srchText ,currentPage: 1 ,maxRows:$scope.selecteditem}
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubGroupcodes = response.SubGroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubGroupcodeList',
                    params : {currentPage: 1 , maxRows:$scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubGroupcodes = response.SubGroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindSubGroupinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubGroupListSearch',
                    params :{srchtxt: $scope.srchText ,currentPage: inx ,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubGroupcodes = response.SubGroupList;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubGroupcodeList',
                    params : {currentPage: inx ,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubGroupcodes = response.SubGroupList;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };

        $scope.ddlItems = function () {
            $scope.BindSubGroupcodeList();
        };
        //$scope.BindSubGroupcodeList = function () {          

        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetSubGroupcodeList'
        //    }).success(function (response) {
        //        $scope.SubGroupcodes = response;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //};
        // $scope.BindSubGroupcodeList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createSubGroupCode = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var groupTitle = $.grep($scope.GroupcodeList, function (lst) {
                return lst.code == $scope.subgrp.groupCode;
            })[0].title;

            $scope.subgrp.groupTitle = groupTitle;

            var formData = new FormData();
            formData.append("subgrp", angular.toJson($scope.subgrp));

            $http({
                url: "/GeneralSettings/InsertSubGroupcode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Sub Group already exists";
                    else {
                        $rootScope.Res = "Sub Group created successfully";
                        $scope.BindSubGroupcodeList();
                    }
                    $scope.reset();
                    $scope.subgrp = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });


            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.SubGroupCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelsubGroupcode',
                    params :{id :_id}
                }).success(function (response) {
                    $rootScope.Res = "Group deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindSubGroupcodeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.SubGroupCodeedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.SubGroupcodes, function (lst) {
                $('#subgrp' + i).attr("style", "");
                i++;
            });
            $('#subgrp' + idx).attr("style", "background-color:lightblue");

            $scope.subgrp = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;


            $scope.subgrp._id = x._id;
            $scope.subgrp.groupCode = x.groupCode;
            $scope.subgrp.code = x.code;
            $scope.subgrp.title = x.title;


        };
        $scope.updateSubGroupCode = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var groupTitle = $.grep($scope.GroupcodeList, function (lst) {
                return lst.code == $scope.subgrp.groupCode;
            })[0].title;

            $scope.subgrp.groupTitle = groupTitle;

            var formData = new FormData();
            formData.append("subgrp", angular.toJson($scope.subgrp));

            $http({
                url: "/GeneralSettings/InsertSubGroupcode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Sub Group already exists";
                    else {
                        $rootScope.Res = "Sub Group updated successfully";
                        $scope.BindSubGroupcodeList();
                    }
                    $scope.reset();
                    $scope.subgrp = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.subgrp = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
    });
    app.controller('SubSubGroupCodeController', function ($scope, $http, $rootScope, $timeout) {

        $scope.selecteditem = 10;
        $scope.SearchSubSubGroupList = function () {
            $scope.BindList();
        }

        $scope.SubSubGroupCode = "";
        $scope.SubSubGroupTitle = "";
        $scope.btnSubmit = true;
        $scope.BindGroupcodeList1 = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetGroupcodeList'
            }).success(function (response) {
                $scope.getmaingroup = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        //$http.get('/GeneralSettings/GetGroupcodeList').success(function (response) {
        //   // alert('hi');
        //    $scope.getmaingroup = response;
        //   // alert(angular.toJson($scope.getmaingroup))
        //});


        $rootScope.OnClickSubSub = function () {
          
            $scope.BindGroupcodeList1();
           // $scope.getsubgroupcode();
            $scope.BindList();
        };



        $scope.getsubgroupcode = function (MainGroupCode) {
            //alert("hi");
            //alert(MainGroupCode);
            $http.get('/GeneralSettings/GetSubGroupcodeList1', { params: { MainGroupCode: MainGroupCode }}
            ).success(function (response) {
                $scope.getsubgroup = response
                // alert(angular.toJson($scope.getsubgroup));
            }).error(function (data, status, headers, config) {
            });

        };
        $scope.BindList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubSubGroupListSearch',
                    //?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubSubGroup = response.SubSubGroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.SubSubGroup = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/ListofSubSubUser',
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
                    //?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem

                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubSubGroup = response.SubSubGroupList;
                        $scope.len = response.totItem;
                    }
                    else $scope.SubSubGroup = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindSubSubGroupinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetSubSubGroupListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }

                    //?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubSubGroup = response.SubSubGroupList;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/ListofSubSubUser',
                    params: { currentPage, inx , maxRows: $scope.selecteditem }
                    //?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.SubSubGroup = response.SubSubGroupList;
                    }
                    else $scope.SubGroupcodes = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };

        $scope.ddlItems = function () {
            $scope.BindList();
        };

        //$scope.BindList = function () {

        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/ListofSubSubUser'
        //    }).success(function (response) {
        //        $scope.SubSubGroup = response;
        //       // alert(angular.toJson($scope.SubSubGroup))
        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //};

        $scope.BindList();

        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $scope.createData = function () {

            // alert("hi");
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            $scope.obj.MainGroupTitle = $("#MainGroupCode1").find("option:selected").text();
            $scope.obj.SubGroupTitle = $("#SubGroupCode1").find("option:selected").text();

            formData.append("obj", angular.toJson($scope.obj));
            //alert(angulr.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/GeneralSettings/InsertSubSubGroup",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    // $('#NotifiyRes').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "SubSubCode created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    // $('#NotifiyRes').attr('style', 'display: block');
                    $scope.obj.MainGroupCode = "";
                    $scope.obj.SubGroupCode = "";
                    $scope.obj.SubSubGroupCode = "";
                    $scope.obj.SubSubGroupTitle = "";
                    $scope.reset();
                    $scope.obj = null;

                }

            }).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.SubSubGroupCodeDel = function (_id) {
            //  alert("hai")
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelsubsubGroupcode',
                    params : { id :_id}
                }).success(function (response) {
                    $rootScope.Res = "Group deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.SubSubGroupCodeedit = function (x, idx) {
            // alert(angular.toJson(x));
            $scope.obj = {};
            $http.get('/GeneralSettings/GetGroupcodeList').success(function (response) {
                $scope.getmaingroup = response;
            });
            $scope.obj.MainGroupCode = x.MainGroupCode;

            $http.get('/GeneralSettings/GetSubGroupcodeList1', { params: { MainGroupCode: x.MainGroupCode }}
    ).success(function (response) {
        $scope.getsubgroup = response

    }).error(function (data, status, headers, config) {
    });
            $scope.obj.SubGroupCode = x.SubGroupCode;

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;

            $scope.obj.SubSubGroupCode = x.SubSubGroupCode;
            $scope.obj.SubSubGroupTitle = x.SubSubGroupTitle;

        };
        $scope.updateData = function () {
            //  alert("hai");
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();

            $scope.obj.MainGroupTitle = $("#MainGroupCode1").find("option:selected").text();

            $scope.obj.SubGroupTitle = $("#SubGroupCode1").find("option:selected").text();

            formData.append("obj", angular.toJson($scope.obj));
            //  alert(angular.toJson($scope.obj));


            $http({
                url: "/GeneralSettings/InsertSubSubGroup",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };
    });

    app.controller('UOMController', function ($scope, $http, $timeout, $rootScope) {

        $scope.selecteditem = 10;
        $scope.SearchUOMList = function () {
            $scope.BindUomList();
        }
        $scope.BindUomList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUOMListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                    //?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UOMs = response.UOMList;
                        $scope.len = response.totItem;
                    }
                    else $scope.UOMs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUOMList1',
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
                    //?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UOMs = response.UOMList;
                        $scope.len = response.totItem;
                    }
                    else $scope.UOMs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindUOMinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUOMListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                    //?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UOMs = response.UOMList;
                    }
                    else $scope.UOMs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUOMList1',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                    //?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UOMs = response.UOMList;
                    }
                    else $scope.UOMs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };


        $scope.ddlItems = function () {
            $scope.BindUomList();
        };

        $scope.BindAttributesList = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $rootScope.OnclickUOM = function () {
            $scope.BindAttributesList();
            $scope.BindUomList();
        }
        //$scope.BindUomList = function () {

        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetUOMList'
        //    }).success(function (response) {
        //        $scope.UOMs = response;
        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //};

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createUOM = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("uom", angular.toJson($scope.uom));

            $http({
                url: "/GeneralSettings/InsertUOM",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "UOM already exists";
                    else {
                        $rootScope.Res = "UOM created successfully";
                        $scope.BindUomList();
                    }
                    $scope.reset();
                    $scope.uom = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.UOMDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Deluom',
                    params : {id:_id}
                }).success(function (response) {
                    $rootScope.Res = "UOM deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindUomList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.UOMedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.UOMs, function (lst) {
                $('#uom' + i).attr("style", "");
                i++;
            });
            $('#uom' + idx).attr("style", "background-color:lightblue");

            $scope.uom = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.uom._id = x._id;
            $scope.uom.UOMname = x.UOMname;
            $scope.uom.Unitname = x.Unitname;
            // $scope.uom.Attribute = x.Attribute;


        };
        $scope.updateUOM = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("uom", angular.toJson($scope.uom));

            $http({
                url: "/GeneralSettings/InsertUOM",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "UOM already exists";
                    else {
                        $rootScope.Res = "UOM updated successfully";
                        $scope.BindUomList();
                    }
                    $scope.reset();
                    $scope.uom = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

            ////if (!$scope.form.$invalid) { 
            //$timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

            //var formData = new FormData();
            //formData.append("uom", angular.toJson($scope.uom));

            //$http({
            //    url: "/GeneralSettings/InsertUOM",
            //    method: "POST",
            //    headers: { "Content-Type": undefined },
            //    transformRequest: angular.identity,
            //    data: formData
            //}).success(function (data, status, headers, config) {

            //    if (data.success === false) {
            //        $rootScope.Res = data.errors;
            //        $rootScope.Notify = "alert-danger";
            //        $rootScope.NotifiyRes = true;

            //    }
            //    else {
            //        if (data === false)
            //            $rootScope.Res = "UOM already exists";
            //        else {
            //            $rootScope.Res = "UOM updated successfully";
            //            $scope.BindUomList();
            //        }
            //        $scope.reset();
            //        $scope.uom = null;

            //        $rootScope.Notify = "alert-info";
            //        $rootScope.NotifiyRes = true;
            //    }
            //}).error(function (data, status, headers, config) {
            //    $rootScope.Res = data;
            //    $rootScope.Notify = "alert-danger";
            //    $rootScope.NotifiyRes = true;
            //});

            //$scope.btnSubmit = true;
            //$scope.btnUpdate = false;
            //// }

        };
        $scope.ClearFrm = function () {

            $scope.uom = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
    });

    app.controller('BulkUOMController', function ($scope, $http, $timeout, $rootScope) {


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkUOM = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/GeneralSettings/UOM_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                    {
                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }
                    else if (data === -1) {
                        $rootScope.Res = data + " Records uploaded successfully"
                        $rootScope.Notify = "alert-danger";
                    }
                    else {
                        $rootScope.Res = data + " Records uploaded successfully"
                        $rootScope.Notify = "alert-info";
                    }

                    
                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

    });

    app.controller('AbbrController', function ($scope, $http, $timeout, $rootScope, $window, $filter) {

        $("#txtFrom1").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.Fromdate = $('#txtFrom1').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                $("#txtTo1").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });
        $("#txtTo1").datepicker({
            numberOfMonths: 1,
        });

        $scope.selecteditem = 10;
        $scope.SearchAbbrList = function () {
            $scope.BindAbbrList();
        }
        // $scope.unitList = [];


        //$scope.setuom = function () {
        $http({
            method: 'GET',
            url: '/GeneralSettings/Getunit'
        }).success(function (response) {

            $scope.unitList = response;
        }).error(function (data, status, headers, config) {

        });
        //};

        //   $scope.setuom();
        $(document).keypress(
  function (event) {
      if (event.which == '13') {
          event.preventDefault();
      }
  });

        $scope.BindAbbrList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetAbbrListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                    //?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        console.log(response)
                        $scope.Abbrs = response.AbbrList;
                        console.log($scope.Abbrs)
                        $scope.len = response.totItem;
                    }
                    else $scope.Abbrs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetAbbrList1',
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
                    //?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Abbrs = response.AbbrList;
                        $scope.len = response.totItem;
                    }
                    else $scope.Abbrs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindAbbrinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetAbbrListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                    //?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Abbrs = response.AbbrList;
                    }
                    else $scope.Abbrs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetAbbrList1',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                    //?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Abbrs = response.AbbrList;
                    }
                    else $scope.Abbrs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };


        $scope.ddlItems = function () {
            $scope.BindAbbrList();
        };

        $rootScope.OnclickAbbr = function () {

            $scope.BindAbbrList();
        }
        $scope.BindAbbrList();
        $scope.BindAbbrListnonabb = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetAbbrList'
            }).success(function (response) {
               
                $scope.Abbrs1 = response;

                angular.forEach($scope.Abbrs1, function (lst) {
                    lst.bu = '0';
                    // }
                });
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindAbbrListnonabb();

        //
        $scope.checkone = function (indx) {
            $scope.App = true;
            if ($scope.Abbrs1[indx].bu == 1) {

                $scope.Abbrs1[indx].bu = 1;
            }
            else
                $scope.Abbrs1[indx].bu = 0;
            //  alert(angular.toJson($scope.Abbrs1))
        };
        $scope.checkall = function () {
            $scope.App = true;

            if ($scope.chkall == 1) {
              
                angular.forEach($scope.Abbrs1, function (lst) {

                    lst.bu =1;


                });
              
            }
            else
                angular.forEach($scope.Abbrs1, function (lst) {
                    lst.bu = 0;


                });
          //  alert(angular.toJson($scope.Abbrs1))
        };


        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createAbbr = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () {
                $scope.NotifiyRes = false;
            }, 30000);

            var formData = new FormData();
            formData.append("abbr", angular.toJson($scope.abbr));

            $http({
                url: "/GeneralSettings/InsertAbbr",
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
                   
                    if (data == false)
                    {
                        $scope.Res = "Abbreviate already exists";                      
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                    }
                    else {
                        $scope.Res = "Abbreviate created successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.BindAbbrList();
                    }
                    $scope.reset();
                    $scope.abbr = null;

                
                }
            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

           
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.approvevalue = function () {


            if ($filter('filter')($scope.Abbrs1, { 'bu': '1' }).length >= 1) {

                $scope.Abbrs1 = $filter('filter')($scope.Abbrs1, { 'bu': '1' })

            }

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Abbrs1", angular.toJson($scope.Abbrs1));

            $http({
                url: "/GeneralSettings/approvevalue",
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
                        $scope.Res = "Abbreviate already exists";
                    else {
                        $scope.Res = "Abbreviate value approved successfully";
                        $scope.BindAbbrListnonabb();
                    }
                  
                    $scope.Abbrs1 = null;

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

            // }
        };
       // $scope.btnSubmit = true;
     //   $scope.btnUpdate = false;
        $scope.AbbrDel = function (val, abv) {
            
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelAbbr',
                    params: { id: abv, val: val }
                    //?id=' + abv + '&val=' + val
                }).success(function (response) {
                    if (response == true) {
                        $scope.Res = "Abbreviate deleted";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.BindAbbrList();
                    } else {
                        $scope.Res = "Deleting process failed";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.unAbbrDel = function (_id) {
           
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/unAbbrDel',
                    params: { id: _id }
                    //?id=' + abv + '&val=' + val
                }).success(function (response) {
                    if (response == true) {
                        $scope.Res = "Un-Approved Value deleted";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.BindAbbrListnonabb();
                    } else {
                        $scope.Res = "Deleting process failed";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.Abbredit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Abbrs, function (lst) {
                $('#abbr' + i).attr("style", "");
                i++;
            });
            $('#abbr' + idx).attr("style", "background-color:lightblue");

            $scope.abbr = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.abbr._id = x._id;
            $scope.abbr.Value = x.Value;
            $scope.abbr.Abbrevated = x.Abbrevated;
            $scope.abbr.Equivalent = x.Equivalent;
            $scope.abbr.LikelyWords = x.LikelyWords;
            $scope.abbr.vunit = x.vunit;
            $scope.abbr.eunit = x.eunit;


        };
        $scope.updateAbbr = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("abbr", angular.toJson($scope.abbr));

            $http({
                url: "/GeneralSettings/InsertAbbr",
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
                        $scope.Res = "Abbreviate already exists";
                    else {
                        $scope.Res = "Abbreviate updated successfully";
                        $scope.BindAbbrList();
                    }
                    $scope.reset();
                    $scope.abbr = null;

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.abbr = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

        $scope.DownloadValueMaster = function () {
            if ($scope.Fromdate == undefined)
                $scope.Fromdate = "";
            if ($scope.Todate == undefined)
                $scope.Todate = "";

            $window.open('/GeneralSettings/DownloadValuemaster?FrmDte=' + $scope.Fromdate + '&ToDte=' + $scope.Todate);

        };

    });
    app.controller('BulkAbbriController', function ($scope, $http, $timeout, $rootScope) {


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkAbbri = function () {

            if ($scope.files[0] == null)
            {
                $rootScope.Res = "Please Select File First"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }

            if ($scope.files[0] != null)
            {


                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/GeneralSettings/Abbri_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $rootScope.Res = "Records already exists"
                    else $rootScope.Res = data + " Records uploaded successfully"


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

    });
    app.controller('VendortypeController', function ($scope, $http, $timeout, DTOptionsBuilder, $rootScope) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        //$scope.users = {};      
        $rootScope.onclickType = function () {
            $scope.BindTypeList();
        }
        $scope.BindTypeList = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetVendortypeList'
            }).success(function (response) {
                $scope.Vendors = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createVendortype = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Vendors", angular.toJson($scope.Vendortype));

            $http({
                url: "/GeneralSettings/InsertVendortype",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Type already exists";
                    else {
                        $rootScope.Res = "Type created successfully";
                        $scope.BindTypeList();
                    }
                    $scope.reset();
                    $scope.Vendortype = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.VendortypeDel = function (abv) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelVendortype',
                    params :{id: abv}
                }).success(function (response) {
                    $rootScope.Res = "Type deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindTypeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.Vendortypeedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Vendors, function (lst) {
                $('#Vendor' + i).attr("style", "");
                i++;
            });
            $('#Vendor' + idx).attr("style", "background-color:lightblue");

            $scope.Vendortype = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.Vendortype._id = x._id;
            $scope.Vendortype.Code = x.Code;
            $scope.Vendortype.Type = x.Type;


        };
        $scope.updateVendortype = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Vendors", angular.toJson($scope.Vendortype));

            $http({
                url: "/GeneralSettings/InsertVendortype",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Type already exists";
                    else {
                        $rootScope.Res = "Type updated successfully";
                        $scope.BindTypeList();
                    }
                    $scope.reset();
                    $scope.Vendortype = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.Vendortype = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
    });
    app.controller('ReftypeController', function ($scope, $http, $timeout, DTOptionsBuilder, $rootScope) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        //$scope.users = {};      
        $rootScope.onclickRefType = function () {
            $scope.BindTypeList();
        }
        $scope.BindTypeList = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetReftypeList'
            }).success(function (response) {
                $scope.Refs = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        // $scope.BindTypeList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createReftype = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Refs", angular.toJson($scope.Reftype));

            $http({
                url: "/GeneralSettings/InsertReftype",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Type already exists";
                    else {
                        $rootScope.Res = "Type created successfully";
                        $scope.BindTypeList();
                    }
                    $scope.reset();
                    $scope.Reftype = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.ReftypeDel = function (abv) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/DelReftype',
                    params : {id: abv}
                }).success(function (response) {
                    $rootScope.Res = "Type deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindTypeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.Reftypeedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Vendors, function (lst) {
                $('#Ref' + i).attr("style", "");
                i++;
            });
            $('#Ref' + idx).attr("style", "background-color:lightblue");

            $scope.Reftype = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.Reftype._id = x._id;
            $scope.Reftype.Code = x.Code;
            $scope.Reftype.Type = x.Type;


        };
        $scope.updateReftype = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("Refs", angular.toJson($scope.Reftype));

            $http({
                url: "/GeneralSettings/InsertReftype",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Type already exists";
                    else {
                        $rootScope.Res = "Type updated successfully";
                        $scope.BindTypeList();
                    }
                    $scope.reset();
                    $scope.Reftype = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.Reftype = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.Duplicate = function (idx, enable, _id) {

            if (enable == false) {



                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Duplicatecheck',
                    params:{id: _id, Islive:enable}
                }).success(function (response) {
                    $rootScope.Res = "Duplicate check disabled";
                    $rootScope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindTypeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });


            } else {


                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Duplicatecheck',
                    params :{id: _id ,Islive: enable}
                }).success(function (response) {
                    $rootScope.Res = "Duplicate check enabled";
                    $rootScope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindTypeList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });


            }

        };
    });
    //uom
    app.controller('UOM1_Controller', function ($scope, $http, $timeout, $rootScope) {

        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/getlistuom'
            }).success(function (response) {
                $scope.UOM1 = response;
            }).error(function (data, status, headers, config) {
                alert("error");
            });
        };


        //if ($scope.srchText != null && $scope.srchText != '') {
        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetUOMListSearchhhh?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
        //    }).success(function (response) {
        //        // alert(angular.toJson(response))
        //        if (response != '') {
        //            $scope.numPerPage = $scope.selecteditem;
        //            $scope.PageCount = response.PageCount;
        //            $scope.currentPage = response.CurrentPageIndex;
        //            $scope.totItem = response.totItem;
        //            $scope.UOM1 = response.UOMList;
        //            $scope.len = response.totItem;
        //        }
        //        else $scope.UOM1 = null;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //} else {
        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetUOMList2?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
        //    }).success(function (response) {
        //        // alert(angular.toJson(response))
        //        if (response != '') {
        //            $scope.numPerPage = $scope.selecteditem;
        //            $scope.PageCount = response.PageCount;
        //            $scope.currentPage = response.CurrentPageIndex;
        //            $scope.totItem = response.totItem;
        //            $scope.UOM1 = response.UOMList;
        //            $scope.len = response.totItem;
        //        }
        //        else $scope.UOM1 = null;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //}

        // };
        //$scope.BindUOMinx1 = function (inx) {
        //    if ($scope.srchText != null && $scope.srchText != '') {

        //        $http({
        //            method: 'GET',
        //            url: '/GeneralSettings/GetUOMListSearchhhh?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
        //        }).success(function (response) {

        //            if (response != '') {
        //                $scope.numPerPage = $scope.selecteditem;
        //                $scope.currentPage = response.CurrentPageIndex;
        //                $scope.totItem = response.totItem;
        //                $scope.UOM1 = response.UOMList;
        //            }
        //            else $scope.UOM1 = null;

        //        }).error(function (data, status, headers, config) {
        //            // alert("error");
        //        });
        //    } else {

        //        $http({
        //            method: 'GET',
        //            url: '/GeneralSettings/GetUOMList2?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
        //        }).success(function (response) {

        //            if (response != '') {
        //                $scope.numPerPage = $scope.selecteditem;
        //                $scope.currentPage = response.CurrentPageIndex;
        //                $scope.totItem = response.totItem;
        //                $scope.UOM1 = response.UOMList;
        //            }
        //            else $scope.UOM1 = null;

        //        }).error(function (data, status, headers, config) {
        //            // alert("error");
        //        });

        //    }

        //};
        //$scope.ddlItems = function () {
        //    $scope.BindList();
        //};


        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        };
        $scope.createUOM1 = function () {

            // $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();
            formData.append("uom", angular.toJson($scope.uom));

            $http({
                url: "/GeneralSettings/Insert_UOM1",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                // alert(data);

                if (data === false) {

                    $rootScope.Res = "UOM already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "UOM created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.uom.UOMName = "";

                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.UOM1Del = function (_id) {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            // alert("hai");
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Deluom1',
                    params:{id: _id}
                }).success(function (response) {
                    $rootScope.Res = "UOM deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                    $scope.BindList();

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.UOM1edit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.UOM1, function (lst) {
                $('#uom' + i).attr("style", "");
                i++;
            });
            $('#uom' + idx).attr("style", "background-color:lightblue");

            $scope.uom = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.uom._id = x._id;
            $scope.uom.UOMName = x.UOMNAME;
        };
        $scope.updateUOM1 = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("uom", angular.toJson($scope.uom));

            $http({
                url: "/GeneralSettings/Insert_UOM1",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "UOM already exists";
                    else {
                        $rootScope.Res = "UOM updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.uom = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };

        $scope.ClearFrm = function () {

            $scope.uom = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };

    });

    app.controller('HSNController', function ($scope, $http, $timeout, $rootScope) {

        $scope.selecteditem = 10;
        $scope.SearchHSNList = function () {
            $scope.BindHSNList();
        }
        $scope.BindHSNList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHSNListSearch?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.HSNs = response.HSNList;

                        $scope.len = response.totItem;
                    }
                    else $scope.HSNs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHSNList1?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.HSNs = response.HSNList;
                        $scope.len = response.totItem;

                    }
                    else $scope.HSNs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindHSNinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHSNListSearch?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.HSNs = response.HSNList;
                    }
                    else $scope.HSNs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHSNList1?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.HSNs = response.HSNList;
                    }
                    else $scope.HSNs = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };


        $scope.ddlItemsH = function () {
            $scope.BindHSNList();
        };



        $rootScope.OnclickHSN = function () {

            $scope.BindHSNList();
        }


        $scope.reset = function () {

            $scope.form1.$setPristine();
        }
        $scope.createHSN = function () {


            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("HSN", angular.toJson($scope.HSN));

            $http({
                url: "/GeneralSettings/InsertHSN",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "HSN already exists";
                    else {
                        $rootScope.Res = "HSN created successfully";
                        $scope.BindHSNList();
                    }
                    $scope.reset();
                    $scope.HSN = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };
        $scope.btnSubmitH = true;
        $scope.btnUpdateH = false;
        $scope.HSNDel = function (HSNID) {


            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Delhsn?id=' + HSNID
                }).success(function (response) {
                    $rootScope.Res = "HSN deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindHSNList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.HSNedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.HSNs, function (lst) {
                $('#uom' + i).attr("style", "");
                i++;
            });
            $('#uom' + idx).attr("style", "background-color:lightblue");

            $scope.HSN = {};

            $scope.btnSubmitH = false;
            $scope.btnUpdateH = true;

            $scope.HSN._id = x._id;

            $scope.HSN.HSNID = x.HSNID;

            $scope.HSN.Desc = x.Desc;

            // $scope.uom.Attribute = x.Attribute;


        };
        $scope.updateHSN = function () {



            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("HSN", angular.toJson($scope.HSN));
            alert(angular.toJson($scope.HSN));

            $http({
                url: "/GeneralSettings/InsertHSN",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $rootScope.Res = "HSN already exists";
                    else {
                        $rootScope.Res = "HSN Updated successfully";
                        $scope.BindHSNList();
                    }
                    $scope.reset();
                    $scope.HSN = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            });

            // }
        };

        $scope.ClearFrmH = function () {

            $scope.HSN = null;
            $scope.btnSubmitH = true;
            $scope.btnUpdateH = false;
            $scope.reset();
        }
    });

    app.controller('VendorController', function ($scope, $rootScope, $http, $timeout, $window) {

        $scope.NotifiyRes = true;

        //$scope.BindVendorList = function () {
        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetVendorList'
        //    }).success(function (response) {
        //        if (response != '')
        //        {
        //            $scope.Vendors = response;
        //            $scope.VendorLst = response;
        //        }        //        else $scope.Vendors = null;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //};
        $scope.searchVendor = function () {

        }
        $scope.selecteditem = 10;
        $scope.SearchVendorList = function () {
            $scope.BindVendorList();
        }
        $scope.BindVendorList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetVendorSearch',
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Vendors = response.VendorsList;
                        $scope.VendorLst = response.VendorsList;
                        $scope.len = response.totItem;
                    }
                    else $scope.Vendors = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetVendorList1',
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Vendors = response.VendorsList;
                        $scope.VendorLst = response.VendorsList;
                        $scope.len = response.totItem;
                    }
                    else $scope.Vendors = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindVendorinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetVendorSearch',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Vendors = response.VendorsList;
                        $scope.VendorLst = response.VendorsList;
                    }
                    else $scope.Vendors = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetVendorList1',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.Vendors = response.VendorsList;
                        $scope.VendorLst = response.VendorsList;
                    }
                    else $scope.Vendors = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };
        $scope.BindVendorList();
        $scope.resdwn = false;
        $scope.resdwn1 != false;
        $scope.ddlItems = function () {
            $scope.BindVendorList();
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createVendor = function () {

            //if (!$scope.form.$invalid) {               


            //$timeout(function () { $scope.NotifiyRes = false; }, 30000);
            //  $scope.vendor.AcquiredCompanyName = $scope.vendor.Acquiredby != null ? $("#ddlAcquire").find("option:selected").text() : null;
            var formData = new FormData();
            formData.append("vendor", angular.toJson($scope.vendor));

            $http({
                url: "/GeneralSettings/InsertVendor",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotify').attr('style', 'display: block');
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Vendor already exists";
                    else {
                        $rootScope.Res = "Vendor created successfully";
                        $scope.BindVendorList();
                    }
                    $scope.reset();
                    $scope.vendor = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotify').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
                $rootScope.Res = data;
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
                $('#divNotify').attr('style', 'display: block');
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.VendorDel = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/GeneralSettings/DisableVendor',
                        params: { id: _id, Enabled: enable }
                    }).success(function (response) {
                        $scope.Res = "Vendor disabled";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.BindVendorList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Vendors[idx].Enabled = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/GeneralSettings/DisableVendor',
                        params: { id: _id, Enabled: enable }
                    }).success(function (response) {
                        $scope.Res = "Vendor enabled";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.BindVendorList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Vendors[idx].Enabled = false;


            }

        };
        $scope.Vendoredit = function (x, idx) {
            var i = 0;
            angular.forEach($scope.Vendors, function (lst1) {
                $('#' + i).attr("style", "");
                i++;
            });
            $('#' + idx).attr("style", "background-color:lightblue");

            $scope.vendor = {};
            //alert(angular.toJson(x.ShortDescName));
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.vendor._id = x._id;
            $scope.vendor.Code = x.Code;
            $scope.vendor.ShortDescName = x.ShortDescName;
            $scope.vendor.Name = x.Name;
            $scope.vendor.Name2 = x.Name2;
            $scope.vendor.Name3 = x.Name3;
            $scope.vendor.Name4 = x.Name4;
            $scope.vendor.Address = x.Address;
            $scope.vendor.Address2 = x.Address2;
            $scope.vendor.Address3 = x.Address3;
            $scope.vendor.Address4 = x.Address4;
            $scope.vendor.City = x.City;
            $scope.vendor.Region = x.Region;
            $scope.vendor.Country = x.Country;
            $scope.vendor.Postal = x.Postal;
            $scope.vendor.Phone = x.Phone;
            $scope.vendor.Mobile = x.Mobile;
            $scope.vendor.Fax = x.Fax;
            $scope.vendor.Email = x.Email;
            $scope.vendor.Website = x.Website;
            $scope.vendor.Acquiredby = x.Acquiredby;
            $scope.vendor.Enabled = x.Enabled;
            $scope.vendor.AcquiredCompanyName = x.AcquiredCompanyName;

            //$scope.VendorLst=[];
            //angular.forEach($scope.Vendors, function (ls) {              
            //    if (ls.Code != x.Code) {
            //        $scope.VendorLst.push(ls);
            //    }

            //});


        };
        $scope.updateVendor = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 30000);
            // $scope.vendor.AcquiredCompanyName = $scope.vendor.Acquiredby != null ? $("#ddlAcquire").find("option:selected").text() : null;
            var formData = new FormData();
            formData.append("vendor", angular.toJson($scope.vendor));

            $http({
                url: "/GeneralSettings/InsertVendor",
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
                        $scope.Res = "Vendor already exists";
                    else {
                        $scope.Res = "Vendor updated successfully";
                        $scope.BindVendorList();
                    }
                    $scope.reset();
                    $scope.vendor = null;

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                }
            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.vendor = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

        $scope.DownloadMfr = function () {

            $window.open('/GeneralSettings/DownloadMfr');

        };

    });
    app.controller('BulkVendorController', function ($scope, $http, $timeout, $rootScope) {


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkVendor = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/GeneralSettings/Vendor_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0) {
                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }
                    else if (data === -1) {
                        $rootScope.Res = "Please valid Your Excel"
                        $rootScope.Notify = "alert-danger";
                    }
                    else {
                        $rootScope.Res = data + " Records uploaded successfully"
                        $rootScope.Notify = "alert-info";
                    }



                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;

                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

    });

    app.controller('BulkHSNController', function ($scope, $http, $timeout, $rootScope) {


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkHSN = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/GeneralSettings/HSN_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $rootScope.Res = "Records already exists"
                    else $rootScope.Res = data + " Records uploaded successfully"


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

    });


    app.controller('unspscController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

        //$scope.dtOptions = DTOptionsBuilder.newOptions()
        //  .withDisplayLength(100)
        //  .withOption('bLengthChange', true);
        $scope.selecteditem = 10;
        $scope.SearchUNSPSCList = function () {
            $scope.loadunspsc();
        }

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
        $scope.UnspscLst = null;
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;
        $scope.loadunspsc = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUNSPSCListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    // alert(angular.toJson(response))
                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UnspscLst = response.UNSPSCList;
                        $scope.len = response.totItem;
                    }
                    else $scope.UnspscLst = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspscc',
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UnspscLst = response.UNSPSCList;
                        $scope.len = response.totItem;
                    }
                    else $scope.UnspscLst = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindUNSPSCinx = function (inx) {
            if ($scope.srchText != null && $scope.srchText != '') {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUNSPSCListSearch',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UnspscLst = response.UNSPSCList;
                    }
                    else $scope.UnspscLst = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspscc',
                    params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.UnspscLst = response.UNSPSCList;
                    }
                    else $scope.UnspscLst = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };

        $scope.ddlItems = function () {
            $scope.loadunspsc();
        };

        //$scope.loadunspsc = function () {
        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetUnspscc'
        //    }).success(function (response) {
        //        if (response != '') {
        //            $scope.UnspscLst = response;
        //        }
        //        else $scope.UnspscLst = null;

        //    }).error(function (data, status, headers, config) {

        //    });
        //};


        $scope.loadunspsc();
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.SeviceCategorycode = "";
            $scope.obj.SeviceCategoryname = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };



        $scope.BulkUNSPSC = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/UNSPSC_Upload",
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
        $scope.ChangeModifier = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUnspsc',
                params: { Noun: $scope.Logic.Noun, Modifier: $scope.Logic.Modifier }
            }).success(function (response) {
                if (response != '') {
                    $scope.UnspscLst = response;
                }
                else $scope.UnspscLst = null;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });


        };

        $scope.UnspscDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/Delunspsc',
                    params: { id: _id }
                }).success(function (response) {
                    $scope.Res = "UNSPSC deleted";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ChangeModifier();
                    $scope.loadunspsc();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

    }]);
    app.directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };

    }]);
    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteNoun?term=" + term,
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Noun,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.Logic.Noun = selectedItem.item.value;

                        $.get("/Dictionary/GetModifier?Noun=" + selectedItem.item.value).success(function (response) {
                            scope.Modifiers = response;
                            scope.$apply();
                            event.preventDefault();
                        });

                    }
                });

            }

        };
    }]);
          app.directive('capitalize', function () {
              return {
                  require: 'ngModel',
                  link: function (scope, element, attrs, modelCtrl) {
                      var capitalize = function (inputValue) {
                          if (inputValue == undefined) inputValue = '';
                          var capitalized = inputValue.toUpperCase();
                          if (capitalized !== inputValue) {
                              // see where the cursor is before the update so that we can set it back
                              var selection = element[0].selectionStart;
                              modelCtrl.$setViewValue(capitalized);
                              modelCtrl.$render();
                              // set back the cursor after rendering
                              element[0].selectionStart = selection;
                              element[0].selectionEnd = selection;
                          }
                          return capitalized;
                      }
                      modelCtrl.$parsers.push(capitalize);
                      capitalize(scope[attrs.ngModel]); // capitalize initial value
                  }
              };
          });


    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/GeneralSettings/AutoCompleteVendor",
                    params: { term: term },
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Name,
                                    value: autocompleteResult.Name
                                }
                            }))
                        });
                    },
                    minLength: 2,

                });

            }

        };
    }]);
})();