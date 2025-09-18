(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap', 'datatables', 'cgBusy']);

    app.controller('AttController', ['$scope', '$http', '$window', '$timeout', '$rootScope', function ($scope, $http, $window, $timeout, $rootScope) {
        $scope.selectValue = [];
        $scope.att = { "Attribute": "" };

        // Loading uom List
        $scope.LoadUOMList = function () {
            $http({
                method: 'GET',
                url: '/Dictionary/GetUOMList'
            }).success(function (response) {
                $scope.UOMList = response;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.LoadUOMList();

        // Loading value list
        $scope.LoadValueList = function () {
          //  alert("hai");
            if ($scope.att.Attribute != null && $scope.att.Attribute != "" && $scope.att.Attribute != undefined) {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueList',
                    params: { currentPage: 1, maxRows: 50, Name: $scope.att.Attribute }
                    //?currentPage=' + 1 + '&maxRows=' + 50 + '&Name=' + $scope.att.Attribute
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 50;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.ValueList = response.ValueList;
                    }
                }).error(function (data, status, headers, config) {
                });
            }
        }


        // searchbox on change
        $scope.BindGroupinxsearch = function () {
            if ($scope.valueSrch != null && $scope.valueSrch != '' && $scope.valueSrch != undefined) {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueListSearch',
                    //?Name=' + $scope.att.Attribute + '&srchtxt=' + $scope.valueSrch + '&currentPage=' + 1 + '&maxRows=' +
                    params : {Name : $scope.att.Attribute , srchtxt: $scope.valueSrch , currentPage : 1 ,maxRows :50}
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 50;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.ValueList = response.ValueList;
                        $scope.changeCheck2();
                    }
                    else $scope.ValueList = null;
                }).error(function (data, status, headers, config) {
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueList',
                    //?currentPage=' + 1 + '&maxRows=' + 50 + '&Name=' + $scope.att.Attribute
                    params: { currentPage: 1, maxRows: 50, Name: $scope.att.Attribute }
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 50;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.ValueList = response.ValueList;
                        $scope.changeCheck2();
                    }
                    else $scope.ValueList = null;
                }).error(function (data, status, headers, config) {
                });
            }
        };


        // Loading values on page selection or change
        $scope.BindGroupinx = function (inx) {
            if ($scope.valueSrch != null && $scope.valueSrch != '' && $scope.valueSrch != undefined) {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueListSearch',
                    //?Name=' + $scope.att.Attribute + '&srchtxt=' + $scope.valueSrch + '&currentPage=' + inx + '&maxRows=' + 50
                    params: { Name: $scope.att.Attribute, srchtxt: $scope.valueSrch, currentPage: inx, maxRows: 50 }

                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 50;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.ValueList = response.ValueList;
                        $scope.changeCheck2();
                    }
                    else $scope.ValueList = null;
                }).error(function (data, status, headers, config) {
                });
            } else {

                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueList',
                    //?currentPage=' + inx + '&maxRows=' + 50 + '&Name=' + $scope.att.Attribute
                    params: { currentPage: inx, maxRows: 50, Name: $scope.att.Attribute }
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 50;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.ValueList = response.ValueList;
                        $scope.changeCheck2();
                    }
                    else $scope.ValueList = null;
                }).error(function (data, status, headers, config) {
                });
            }
        };

        // Checkbox selection change
        $scope.checkValue = function (id, indx) {
            if ($('#chk' + id).is(':checked')) {
                if ($scope.selectValue.indexOf(id) !== -1) {
                }
                else {
                    $scope.selectValue.push(id);
                }
            }
            else {
                angular.forEach($scope.selectValue, function (lst) {
                    if (lst === id) {
                        var index = $scope.selectValue.indexOf(id);
                        $scope.selectValue.splice(index, 1);
                    }
                });
            }
        };


        // selecting uom
        $scope.selectUOM = [];
        $scope.checkUOM = function (id, indx) {
            if ($('#chkU' + indx).is(':checked')) {
                $scope.selectUOM.push(id);
            }
            else {
                var index = $scope.selectUOM.indexOf(id);
                $scope.selectUOM.splice(index, 1);
            }
        };

        $scope.reset = function () {
            $scope.form.$setPristine();
        }

        // Save or update attribute values
        $scope.createAtt = function () {

            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append("objAtt", angular.toJson($scope.att));
            formData.append("AttirbuteList", angular.toJson($scope.selectValue));
            formData.append("UomList", angular.toJson($scope.selectUOM));
            $http({
                url: "/Dictionary/InsertAttribute",
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
                        $rootScope.Res = "Attribute already exists";
                    else {
                        $scope.att = {};
                        $scope.att.Validation = 0;
                        $scope.selectValue = [];
                        $scope.selectUOM = [];
                        $rootScope.Res = "Attribute created successfully";
                        $scope.LoadUOMList();
                        $scope.LoadValueList();
                    }
                    $scope.reset();

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }

            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
            });
        }

        $scope.getAttributeList = function () {
            $http({
                method: 'GET',
                url: '/Dictionary/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response
            }).error(function (data, status, headers, config) {
            });
        };
          $scope.getAttributeList();
       // 


        // Check box selection for filter
        $scope.changeCheck2 = function () {
            angular.forEach($scope.ValueList, function (lst) {
                if ($scope.selectValue.indexOf(lst._id) !== -1) {
                    lst.Checked = '1';
                } else {
                    lst.Checked = '0';
                }
            });
        };


        // Initial attribute value load and checkbox selection
        $scope.changeCheck = function () {
          
            $scope.valueSrch = "";
            $scope.selectValue = [];

            $scope.LoadValueList();
          
            $http({
                method: 'GET',
                url: '/Dictionary/GetAttributesDetail?Name=' + $scope.att.Attribute
            }).success(function (response) {
                $scope.att = response;

                if (response._id != null) {

                    if (response.ValueList != null) {

                        $scope.selectValue = response.ValueList;

                        angular.forEach($scope.ValueList, function (lst) {
                            if (response.ValueList.indexOf(lst._id) !== -1) {
                                lst.Checked = '1';
                            } else {
                                lst.Checked = '0';
                            }
                        });
                    }

                    var i = 0;
                    if (response.UOMList != null) {
                        angular.forEach($scope.UOMList, function (lst) {
                            if (response.UOMList.indexOf(lst._id) !== -1) {
                                lst.UOMname = '0';
                                $scope.selectUOM.push(lst._id);
                                $('#chkU' + i).prop('checked', true);
                            } else {
                                lst.UOMname = '1';
                                $('#chkU' + i).prop('checked', false);
                            }
                            i++;
                        });
                    }
                }
            }).error(function (data, status, headers, config) {
            });
        };
    }]);

})();


