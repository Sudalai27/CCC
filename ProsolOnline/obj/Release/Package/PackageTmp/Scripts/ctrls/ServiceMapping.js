var equip = angular.module('ProsolApp', ['cgBusy']);



equip.controller('ServiceMappingController',
    function ($scope, $window, $http, $timeout, $filter) {
        $scope.serchhide = true;
      
        $scope.Loaditem = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ServiceMaster/Loaditem'
            }).success(function (response) {

                if (response != '') {
                    $scope.sResult1 = response;

                } else {

                    $scope.sResult1 = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            });
        }
        $scope.Loaditem();
        $scope.removeshow = true;
        $scope.hide = true;
        $scope.flow = [];
        $scope.add = true;
        $scope.searching1 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.searchkey1 != " " && $scope.searchkey1 != undefined && $scope.searchkey1 != null && $scope.searchkey1 != '') {
                $scope.sResult1 = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/ServiceMaster/searching1?sKey=' + $scope.searchkey1
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response != '') {
                        $scope.sResult1 = response;
                        $scope.Res = "Search results : " + response.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                       
                    } else {

                        $scope.sResult1 = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {

                });

            }
        };
       
        var mymodal1 = new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid'),
            reposition: false,
            repositionOnOpen: false

        });
        $scope.SearchItem = function (ServiceCode) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.searchkey != " " && $scope.searchkey != undefined && $scope.searchkey != null && $scope.searchkey != '') {
                $scope.sResult = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/ServiceMaster/searching1?sKey=' + $scope.searchkey + '&ServiceCode=' + ServiceCode
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response != '') {
                        $scope.sResult = response;
                        $scope.Res = "Search results : " + response.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        angular.forEach($scope.sResult, function (src, idx) {
                            src.add = 0;
                        });
                        mymodal1.open();
                        $scope.add = true;
                    } else {

                        $scope.sResult = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;


                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };


        $scope.MatClick = function (mat, idx) {
          
            $scope.ServiceCode = mat.ServiceCode
            $scope.MainCode = mat.MainCode
            $scope.SubCode = mat.SubCode
            $scope.LongDesc = mat.LongDesc
            $scope.ShortDesc = mat.ShortDesc
            $scope.UomCode = mat.UomCode
            if (mat.parent == "Yes")
            {
                $scope.tabname = "Parent Info",
                $scope.tabname1 = "Child Info"
            }
            else {
                $scope.tabname = "Child Info",
                $scope.tabname1 = "Parent Info"
            }

              
            $scope.selected = false;
            $scope.searchkey = null;
            $scope.cgBusyPromises = $http({

                method: 'GET',
                url: '/ServiceMaster/srchchild?sKey=' + $scope.ServiceCode
            }).success(function (response) {
                if (response != '') {
                    $scope.flow = response;
                    
                    $scope.hide = false;
                    $scope.selected = false;
                    $scope.Create1 = true;
                    $scope.update = true;
                    $scope.serchhide = true;
                } else if (response == null) {
                    $scope.hide = false;
                    $scope.selected = true;
                    $scope.Create1 = true;
                    $scope.update = false;
                    $scope.serchhide = false;
                   
                }
                else {
                    $scope.serchhide = false;
                    $scope.flow = null;
                    $scope.tabname = "Parent Info";
                }
            });
        }

        $scope.identifyAdd_rows = function (index) {

            $('input:checkbox').click(function () {
                if ($(this).is(':checked')) {
                    $('#Submit').prop("disabled", false);
                } else {
                    if ($('.chk').filter(':checked').length < 1) {
                        $('#Submit').attr('disabled', true);
                    }
                }
            });
            if ($scope.sResult[index].add === 0) {
                $scope.sResult[index].add = 1;
                $scope.add = false;
            } else {
                $scope.sResult[index].add = 0;
            }

        };

        $scope.addRow = function () {
            var cc = 0;
            $scope.searchkey = " ";
            $scope.selected = true;
            $scope.hide = false;
            $scope.add = true;
            $scope.Create1 = false;
            if ($scope.flow != null) {
                angular.forEach($scope.sResult, function (src, idx) {
                    cc = 0;
                    angular.forEach($scope.flow, function (value, key) {
                        if (value.ServiceCode === src.ServiceCode && src.add === 1) {
                            cc = 1;
                            alert(angular.toJson("ServiceCode " + value.ServiceCode + " alredy exist"));
                        }
                    });

                    // alert(angular.toJson(src.add));
                    if (cc === 0) {
                        if (src.add === 1) {
                       
                            $scope.flow.push({ 'ServiceCode': src.ServiceCode, 'ShortDesc': src.ShortDesc, 'LongDesc': src.LongDesc, 'remove': 0 })
                            src.add = 0;
                        }
                    }
                });
            }
            else {
                $scope.flow = [];
                angular.forEach($scope.sResult, function (value, key) {
                   
                    if (value.add === 1) {
                        $scope.flow.push({ 'ServiceCode': value.ServiceCode, 'ShortDesc': value.ShortDesc, 'LongDesc': value.LongDesc, 'remove': 0 })
                    }
                });
            }

            mymodal1.close();
            $scope.removeshow = true;
            $scope.update = true;
            $scope.sResult = null;
            $scope.tabname1 = "Child Info";
        };


        $scope.RemoveRow = function () {
            $scope.removeshow = true;
            $scope.ar = [{ 'Squence': 1, 'ShortSquence': 1, 'remove': 0 }];

            angular.forEach($scope.flow, function (value, key) {
                if (value.remove === 0) {
                    $scope.ar.push(value);
                }
            });

            $scope.ar.splice(0, 1);
            $scope.flow = $scope.ar;

        };

        $scope.identifyremove_rows = function (index) {


            if ($scope.flow[index].remove === 0) {
                $scope.flow[index].remove = 1;
            } else {
                $scope.flow[index].remove = 0;
            }

            $scope.nnn = 0;
            angular.forEach($scope.flow, function (value, key) {
                if (value.remove === 1) {
                    $scope.nnn = 1;
                }
            });
            if ($scope.nnn === 1) {
                $scope.removeshow = false;
            }
            else {
                $scope.removeshow = true;
            }


        };

        $scope.ClearItem = function () {
            location.reload();
            $scope.add = true;

        }


        $scope.ClearItem1 = function () {

            $scope.ServiceCode = null;
            $scope.MainCode = null;
            $scope.SubCode = null;
            $scope.LongDesc = null;
            $scope.ShortDesc = null;
            $scope.UomCode = null;
            $scope.flow = [];
            $scope.hide = true;
            $scope.removeshow = true;
        }
        //insert

        $scope.Create = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            var formvalues = new FormData();
            formvalues.append('request', angular.toJson($scope.flow));
            formvalues.append('ServiceCode', angular.toJson($scope.ServiceCode));
            $scope.cgBusyPromises = $http({
                url: "/ServiceMaster/CreateChild",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formvalues
            }).success(function (data) {
                $('#divNotifiy').attr('style', 'display: Block');
                // alert(angular.toJson(data));
                if (data === 0) {
                    $scope.Res = "Already Exist";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                
                }
                else {
                  
                    $scope.rows = [];
                    $scope.Res = data + " Items Created";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ClearItem1();
                    $scope.removeshow = true;
                    $scope.Loaditem();
                }

            }).error(function () {
                $scope.Res = "Error occured";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
               
            });
        };


    });