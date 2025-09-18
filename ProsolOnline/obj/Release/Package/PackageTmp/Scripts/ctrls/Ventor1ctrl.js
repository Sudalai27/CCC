
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap']);

    app.controller('VendorController', function ($scope, $http, $timeout, $window) {


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
                    params :{srchtxt:$scope.srchText ,currentPage:1 ,maxRows: $scope.selecteditem}
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
                    params :{currentPage:1 ,maxRows: $scope.selecteditem}
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
                    params :{srchtxt: $scope.srchText ,currentPage: inx ,maxRows: $scope.selecteditem}
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
                    params :{currentPage: inx,maxRows: $scope.selecteditem}
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


            $timeout(function () { $scope.NotifiyRes = false; }, 30000);
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
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }
                else {
                    if (data === false)
                        $scope.Res = "Vendor already exists";
                    else {
                        $scope.Res = "Vendor created successfully";
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
                        params :{id: _id ,Enabled: enable}
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
                    else if(data === -1){
                        $rootScope.Res = "Please valid Your Excel"
                        $rootScope.Notify = "alert-danger";
                    }
                    else{
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
    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/GeneralSettings/AutoCompleteVendor",
                    params:{term: term},
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