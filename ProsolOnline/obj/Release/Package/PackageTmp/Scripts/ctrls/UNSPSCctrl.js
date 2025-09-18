(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap', 'datatables']);

 
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
                    params :{srchtxt: $scope.srchText ,currentPage: 1 ,maxRows: $scope.selecteditem}
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
                    params :{currentPage: 1 ,maxRows: $scope.selecteditem}
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
                    params :{srchtxt:$scope.srchText ,currentPage:inx ,maxRows:$scope.selecteditem}
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
                    params :{srchtxt:$scope.srchText ,currentPage: inx ,maxRows: $scope.selecteditem}
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
                params :{Noun:$scope.Logic.Noun,Modifier: $scope.Logic.Modifier}
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
                    params :{id:_id}
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

})();