
var app = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);
app.controller('BomTrackingcontroller', function ($scope, $http, $timeout,$rootScope, $window, $location) {






    $scope.Noitem = true;
    $scope.clear = true;
    $scope.clear1 = true;
    $scope.bexp = true;
    $scope.Open = function (Itemcode) {


        $http({
            method: 'GET',
            url: '/Search/GetItemDetail?Itmcode=' + Itemcode
        }).success(function (response) {
            if (response != '') {
                $scope.cat = response;
                $scope.img = {};
                $http({
                    method: 'GET',
                    url: '/Search/GetImage?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                }).success(function (response) {
                    if (response != '') {
                        $scope.img = response;

                    } else {
                        $scope.img = null;

                    }
                })

            } else {
                $scope.cat = null;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
        $scope.erp = {};
        $http({
            method: 'GET',
            url: '/Search/GetItemERP?Itmcode=' + Itemcode
        }).success(function (response) {
            if (response != '') {
                $scope.erp = response;


            } else {
                $scope.cat = null;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });


        new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid'),
            reposition: false,
            repositionOnOpen: false
        }).open();

    };

    $scope.downloadfilemcpcode = function (mcpcode) {
        // alert(mcpcode);
        if (mcpcode != null && mcpcode != undefined && mcpcode != "")
            window.location.href = "/Asset/Download?mcpcode=" + mcpcode
        else
            alert("No document found for ur search");
    };


    $scope.BulkBom = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);

        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/BulkBom?sKey=' + $scope.bsearchkey

        }).success(function (response) {
            $('#divNotifiy').attr('style', 'display: Block');
            if (response != '') {
                $scope.blukresult = response;
                $rootScope.Res = "Track results : " + response.length + " items."
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.clear1 = false;
                $scope.bexp = false;
                $rootScope.Reset1 = false;
            }
            else {
                $scope.blukresult = null;

                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
                $rootScope.Reset1 = true;
            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };



    $scope.ClearItem1 = function () {
        $scope.form.$setPristine();
        $scope.bsearchkey = null;
        $rootScope.Reset1 = true;
        $scope.bexp = true;
        $scope.clear1 = true;

    }
    $scope.ClearItem = function () {
        $scope.form.$setPristine();
        $scope.searchkey = null;
        $rootScope.Reset = true;
        $scope.Noitem = true;
        $scope.clear = true;
        $scope.bindAssets();
    }

    $scope.TBMClick = function (TBM, idx) {


        $rootScope.NotifiyRes = false;

        var i = 0;
        angular.forEach($scope.sResult, function (lst) {
            $('#eq' + i).attr("style", "");
            i++;
        });

        $('#eq' + idx).attr("style", "background-color:lightblue");

        $scope.TechIdentNo = TBM.TechIdentNo
        $scope.EquipDesc = TBM.EquipDesc
        $scope.FunctLocation = TBM.FunctLocation
        $scope.FunctDesc = TBM.FunctDesc
        $scope.Objecttype = TBM.Objecttype
        $scope.Manufacturer = TBM.Manufacturer
        $scope.show = true;
        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/getitemb?sKey=' + $scope.TechIdentNo

        }).success(function (response) {
            if (response != 'true') {

                $rootScope.Result = response;
                $scope.Noitem = false;
                $scope.show = false;
                $scope.clear = false;
                $scope.trackebom = response.eqpbom;
                $scope.trackmbom = response.matbom;
                $scope.trackmmbom = response.mmatbom;
            }
            else {
                $scope.Noitem = true;
                $scope.show = true;
                $rootScope.Result = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }

        })

    }

    $scope.Export = function () {
        //   alert(angular.toJson($rootScope.Result))
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $window.location = '/Bom/Download?sKey=' + $scope.TechIdentNo;

    }

    $scope.BulkExport = function () {

        //var formvalues = new FormData();
        //formvalues.append('BULK', angular.toJson($scope.blukresult));
        //$scope.promise = $http({
        //    url: "/Bom/BulkDownload",
        //    method: "POST",
        //    headers: { "Content-Type": undefined },
        //    transformRequest: angular.identity,
        //    data: formvalues
        //});
        //$timeout(function () {
        //    $('#divNotifiy').attr('style', 'display: none');
        //}, 30000);


        $window.location = '/Bom/BulkDownload?sKey=' + $scope.blukresult;


    }



    $scope.selecteditem = 10;
    $scope.ddlItems = function () {
        if ($scope.searchkey == '' || $scope.searchkey == null) {
            $scope.bindAssets();
        } else {
            $scope.TrackBom();
        }
    };




    $scope.bindAssets = function () {

        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/funlocsearch1',
            params: { currentPage: 1, maxRows: $scope.selecteditem }
        }).success(function (response) {
            if (response != '') {

                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;
                $scope.sResult = response.lst;

                if (response.eqpbom != null) {
                    $scope.trackebom = response.eqpbom;
                    $scope.TechIdentNo = $scope.trackebom[0].TechIdentNo;
                    $scope.EquipDesc = $scope.trackebom[0].EquipDesc;
                    $scope.FunctLocation = $scope.trackebom[0].FunctLocation;
                    $scope.FunctDesc = $scope.trackebom[0].FunctDesc;
                    $scope.Objecttype = $scope.trackebom[0].Objecttype;
                    $scope.Manufacturer = $scope.trackebom[0].Manufacturer;
                    $scope.sfunloc = $scope.trackebom[0].SupFunctLoc;
                    if ($scope.trackebom != null) {
                        $scope.show = true;
                        $scope.Noitem = false;
                        $scope.show = false;
                        $scope.clear = false;
                    }
                }
                $scope.trackmbom = response.matbom;
                $scope.trackmmbom = response.mmatbom;


            }
            else {
                $scope.sResult = null;

            }

        })

    }
    $scope.bindAssets();

    $scope.bindAssetsInx = function (inx) {

        if ($scope.searchkey == '' || $scope.searchkey == null) {
            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/funlocsearch1',
                params: { currentPage: inx, maxRows: $scope.selecteditem }
            }).success(function (response) {
                if (response != '') {

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.sResult = response.lst;

                }
                else {
                    $scope.sResult = null;

                }

            })
        } else {

            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/funlocsearch',
                params: { sKey: $scope.searchkey, currentPage: inx, maxRows: $scope.selecteditem }

            }).success(function (response) {
                if (response.FunlocList != '') {
                    $scope.sResult = response.FunlocList;
                    $rootScope.Res = "Track results : " + response.FunlocList.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.clear = false;

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;

                }
                else {
                    $scope.sResult = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.clrhiarchy();
    }

    $scope.TrackBom = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $rootScope.Reset = false;
        $scope.sResult = {};

        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/funlocsearch',
            params: { sKey: $scope.searchkey, currentPage: 1, maxRows: $scope.selecteditem }

        }).success(function (response) {
            if (response.FunlocList != '') {
                $scope.sResult = response.FunlocList;
                $rootScope.Res = "Track results : " + response.FunlocList.length + " items."
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.clear = false;

                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;

            }
            else {
                $scope.sResult = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {
            alert("error");
        });
        $scope.clrhiarchy();
    };
    $scope.Noitem = true;
    $scope.clear = true;
    $scope.clear1 = true;
    $scope.bexp = true;






    $scope.ClearItem3 = function () {
        $scope.form.$setPristine();
        $scope.searchkey = null;
        $rootScope.Reset = true;
        $scope.Noitem = true;
        $scope.clear = true;

    }
    $scope.clrhiarchy = function () {
        $scope.fun = null;
        $scope.fun2 = null;
        $scope.fun3 = null;
        $scope.fun4 = null;
        $scope.fun5 = null;
        $scope.fun6 = null;
        $scope.fun7 = null;

        $scope.Noitem = true;

    }
    $scope.TBMClick1 = function (TBM, idx) {
        $rootScope.NotifiyRes = false;

        var i = 0;
        angular.forEach($scope.sResult, function (lst) {
            $('#' + i).attr("style", "");
            i++;
        });

        $('#' + idx).attr("style", "background-color:lightblue");

        $scope.TechIdentNo = TBM.TechIdentNo
        $scope.EquipDesc = TBM.EquipDesc
        $scope.FunctLocation = TBM.FunctLocation
        $scope.FunctDesc = TBM.FunctDesc
        $scope.Objecttype = TBM.Objecttype
        $scope.Manufacturer = TBM.Manufacturer
        $scope.sfunloc = TBM.SupFunctLoc

        $scope.show = true;
        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/getitem?sKey=' + $scope.FunctLocation

        }).success(function (response) {

            if (response != 'true') {

                $rootScope.Result = response;
                $scope.Noitem = false;
                $scope.show = false;
                $scope.clear = false;

                $scope.fun = response.fun1;
                $scope.fun2 = response.fun2;
                $scope.fun3 = response.fun3;
                $scope.fun4 = response.fun4;
                $scope.fun5 = response.fun5;
                $scope.fun6 = response.fun6;
                $scope.fun7 = response.fun7;

            }
            else {

                $scope.Noitem = true;
                $scope.show = true;
                $rootScope.Result = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }

        })

    }

    $scope.Open1 = function (Itemcode) {
        $window.location = '/Bom/Bomtracking?id=' + Itemcode;

        //$rootScope.cgBusyPromises = $http({

        //    method: 'GET',
        //    url: '/Bom/getitemb?sKey=' + Itemcode

        //}).success(function (response) {
        //    if (response != 'true') {   

        //        $window.location = '/Bom/Bomtracking?id=' + Itemcode;
        //        $rootScope.Result = response;
        //        $scope.Noitem = false;
        //        $scope.show = false;
        //        $scope.clear = false;
        //        $scope.trackebom = response.eqpbom;
        //        $scope.trackmbom = response.matbom;
        //        $scope.trackmmbom = response.mmatbom;


        //    }
        //    else {
        //        $scope.Noitem = true;
        //        $scope.show = true;
        //        $scope.Result = null;
        //        $scope.Res = "No item found"
        //        $rootScope.Notify = "alert-danger";
        //        $rootScope.NotifiyRes = true;
        //    }

        //})



    }

});
app.controller('BomReportcontroller', function ($scope, $http, $timeout, $rootScope, $window, $filter) {

    $scope.selecteditem = 10;
    $scope.ddlItems = function () {
        if ($scope.searchkey1 == '' || $scope.searchkey1 == null)
            $scope.Loadpage();
        else
            $scope.searching1()
    };

    $scope.Loadpage = function () {
        $rootScope.cgBusyPromises = $http({
            method: 'GET',
            url: '/Bom/frstmat',
            params: { currentPage: 1, maxRows: $scope.selecteditem }
        }).success(function (response) {

            if (response.matList != null) {


                $scope.sResult = response.matList;
                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;


            } else {
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 5000);
                $scope.sResult = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;

            }

        })

    }

    $scope.Loadpage();
    $scope.bindMatsInx = function (inx) {

        if ($scope.searchkey1 == '' || $scope.searchkey1 == null) {
            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/frstmat',
                params: { currentPage: inx, maxRows: $scope.selecteditem }
            }).success(function (response) {
                if (response.matList != null) {

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.sResult = response.matList;

                }
                else {
                    $scope.sResult = null;

                }

            })
        } else {

            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/searching1',
                params: { sKey: $scope.searchkey1, currentPage: inx, maxRows: $scope.selecteditem }

            }).success(function (response) {
                if (response.matList != null) {
                    $scope.sResult = response.matList;
                    $rootScope.Res = "Search results : " + response.matList.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.clear = false;
                    $rootScope.Reset = false;
                    $scope.Report = null;

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;

                }
                else {
                    $scope.sResult = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

    }
    $scope.ind_A = true;
    $scope.ind_B = true;
    $scope.ind_C = true;
    $scope.b1 = true;
    $scope.b2 = true;
    $scope.Noitem = true;
    $scope.clear = true;
    $scope.clear1 = true;

    //$scope.checka = function () {
    //    alert(angular.toJson($scope.ind_A));
    //};
    $scope.searching1 = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.sResult = {};
        $rootScope.cgBusyPromises = $http({
            method: 'GET',
            url: '/Bom/searching1',
            params: { sKey: $scope.searchkey1, currentPage: 1, maxRows: $scope.selecteditem }
        }).success(function (response) {

            if (response.matList != null) {
                $scope.sResult = response.matList;
                $rootScope.Res = "Search results : " + response.matList.length + " items."
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.clear = false;
                $rootScope.Reset = false;
                $scope.Report = null;

                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;

            } else {

                $scope.sResult = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {

        });


    };
    var mymodal = new jBox('Modal', {
        width: 500,
        blockScroll: false,
        animation: 'zoomIn',
        draggable: false,
        overlay: true,
        closeButton: true,
        //  mouseclick:false,
        content: jQuery('#cotentid1'),
        reposition: false,
        repositionOnOpen: false


    });
    $scope.ClearItem = function () {
        $scope.form.$setPristine();
        $scope.searchkey1 = " ";
        $rootScope.Reset = true;
        $scope.Noitem = true;
        $scope.clear = true;
        location.reload();

    }

    $scope.TBMClick2 = function (TBM, idx) {
        $rootScope.NotifiyRes = false;

        var i = 0;
        angular.forEach($scope.sResult, function (lst) {
            $('#' + i).attr("style", "");
            i++;
        });

        $('#' + idx).attr("style", "background-color:lightblue");
        // mymodal.open();
        $scope.Itemcode = TBM.Itemcode
        $scope.Shortdesc = TBM.Shortdesc

        $scope.Noun = TBM.Noun
        $scope.Modifier = TBM.Modifier

        $scope.Manufacturer = TBM.Manufacturer
        $scope.Partno = TBM.Partno
        $scope.Noitem = false;
        //  $scope.report = function () {


        $rootScope.cgBusyPromises = $http({

            method: 'GET',
            url: '/Bom/getreport?sKey=' + $scope.Itemcode + '&inda=' + true + '&indb=' + true + '&indc=' + true

        }).success(function (response) {
            //  alert(angular.toJson(response))
            if (response.length > 0) {
                $rootScope.NotifiyRes = false;
                // mymodal.close();
                $scope.Report = response;

                $scope.cou = $scope.Report
                //    alert(angular.toJson($scope.Report))
                $scope.Noitem = false;
                $scope.show = false;
                $scope.clear = false;

                $scope.getTotal = function (type) {

                    var total = 0;
                    angular.forEach($scope.Report, function (el) {
                        total += el[type];
                    });
                    return total;
                };

            }
            else {

                $scope.Noitem = true;
                $scope.show = true;
                $scope.Report = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }

        })



        // };

    };

    $scope.BulkMat = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);

        $scope.blukmat = {};
        $http({
            method: 'GET',
            url: '/Bom/BulkMat?sKey=' + $scope.bmsrch
        }).success(function (response) {

            if (response != '') {
                $scope.blukmat = response;
                $rootScope.Res = "Search results : " + response.length + " items."
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.b1 = false;
                $scope.b2 = false;

            } else {

                $scope.b1 = true;
                $scope.b2 = true;
                $scope.blukmat = null;
                $rootScope.Res = "No item found"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {

        });


    };
    $scope.Export = function () {

        //   alert(angular.toJson($rootScope.Result))
        $window.location = '/Bom/Download2?sKey=' + $scope.Itemcode + '&inda=' + $scope.ind_A + '&indb=' + $scope.ind_B + '&indc=' + $scope.ind_C

    }
    $scope.BulkExport = function () {

        //   alert(angular.toJson($rootScope.Result))
        $window.location = '/Bom/DetailDownload?sKey=' + $scope.Itemcode
    }
    $scope.BulkExportsum = function () {

        $window.location = '/Bom/BulkMatDownload?sKey=' + $scope.Itemcode

    }


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
        //onSelect: function (selected) {
        //    var dt = new Date(selected);
        //    dt.setDate(dt.getDate() );
        //    $("#txtFrom").datepicker("option", "maxDate", dt);
        //}
    });


    $scope.trackingload1 = function () {

        //  alert("hai");
        //  alert(angular.toJson($scope.materialcode));

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);

        //if ($scope.materialcode != undefined && $scope.materialcode != "")
        //{

        //}
        if ($scope.materialcode != "" && $scope.materialcode != null && $scope.materialcode != undefined) {

            var formData = new FormData();
            //formData.append("plant", $scope.Plant);

            //if ($scope.Fromdate != undefined)
            //    formData.append("fromdate", $scope.Fromdate.toISOString());
            //if ($scope.Fromdate == undefined)
            //    $scope.Fromdate = null;
            //if ($scope.Todate != undefined)
            //    formData.append("todate", $scope.Todate.toISOString());
            //if ($scope.Todate == undefined)
            //    $scope.Todate = null;
            //formData.append("option", $scope.Option);
            formData.append("materialcode", $scope.materialcode);

            $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Bom/Trackload',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                //    alert(angular.toJson(response));
                if (response != "" && response.length <= 100) {
                    $scope.tables = [];
                    $scope.tables = response;

                    $scope.len = response.length;

                    $rootScope.Res = "Loaded successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                } else {
                    $rootScope.Res = "Please enter valid material codes, if more than one code separate by comma(,)";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            });
        }
        else {

            if ($scope.Fromdate != undefined && $scope.Option != undefined) {

                var formData = new FormData();


                if ($scope.Fromdate != undefined)
                    formData.append("fromdate", $scope.Fromdate);
                if ($scope.Fromdate == undefined)
                    $scope.Fromdate = null;
                if ($scope.Todate != undefined)
                    formData.append("todate", $scope.Todate);
                if ($scope.Todate == undefined)
                    $scope.Todate = null;
                formData.append("option", $scope.Option);
                formData.append("materialcode", $scope.materialcode);

                $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Bom/Trackload',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    //  alert(angular.toJson($rootScope.Response));
                    if (response != "" && response.length <= 100) {
                        $scope.tables1 = [];
                        $scope.tables1 = response;
                        // alert(angular.toJson($scope.tables));
                        $scope.len1 = response.length;

                        $rootScope.Res = "Loaded successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');


                    } else if (response != "" && response.length > 100) {
                        $scope.tables1 = [];
                        $scope.len1 = response.length;
                        $rootScope.Res = "100 records only load here, You can export more than 100 records";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    } else if (response == "") {
                        $scope.len1 = null;
                        $scope.tables1 = [];
                        $rootScope.Res = "No data avaliable for load";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    }

                    if (response.length == 0) {
                        $scope.reqtbls = [];
                        $scope.columns = [];
                        $rootScope.Res = "No data avaliable for Load";
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    } else {

                        if ($scope.Option == "InTime") {
                            $scope.columns = ["Materialcode", "FunctLocation", "TechIdentNo", "INtime", "OutTime", "Createdon"];
                            $scope.reqtbls = [];
                            $scope.reqtbls.push({ rows: response });
                            $scope.cols = $scope.columns;
                            $scope.dwldata = response;
                            $scope.len1 = response.length;
                        } else {
                            $scope.columns = ["Materialcode", "FunctLocation", "TechIdentNo", "INtime", "OutTime", "Createdon"];
                            $scope.reqtbls = [];
                            $scope.reqtbls.push({ rows: response });
                            $scope.cols = $scope.columns;
                            $scope.dwldata = response;
                            $scope.trackdata = response;
                            $scope.len1 = response.length;

                        }
                        $rootScope.Res = "Loaded Successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                });
            }
            else {

                if ($scope.Fromdate == undefined) {
                    $rootScope.Res = "Select FromDate";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else if ($scope.Option == undefined || $scope.Option == "") {
                    $rootScope.Res = "Select Option";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "Provide Search Datas";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }

            }
        }

    }//To load data of tracking

    $scope.exportTrack1 = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);

        if ($scope.materialcode != "" && $scope.materialcode != null && $scope.materialcode != undefined) {
            var formData = new FormData();
            formData.append("materialcode", $scope.materialcode);
            $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Bom/DownloadTrack1',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                if (response > 0)
                    $window.location = '/Bom/DownloadMulticode';
                else {
                    $rootScope.Res = "Please enter valid material codes, if more than one code separate by comma(,)";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            });

            //if ($scope.tables != null) {
            //    var data = $scope.tables;
            //    var blob = new Blob([document.getElementById('exportable').innerHTML], {
            //        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            //    });
            //    saveAs(blob, "TrackReport.xls");
            //} else {
            //    $rootScope.Res = "Please load first";
            //    $rootScope.Notify = "alert-danger";
            //    $rootScope.NotifiyRes = true;
            //    $('#divNotifiy').attr('style', 'display: block');
            //}


        } else {
            if ($scope.Fromdate != undefined && $scope.Option != undefined) {

                var fdate = null, tdate = null;
                if ($scope.Fromdate != undefined)
                    fdate = $scope.Fromdate;
                if ($scope.Todate != undefined)
                    tdate = $scope.Todate;



                $window.location = '/Bom/DownloadTrack?plant=' + $scope.materialcode + '&Fromdate=' + fdate + '&Todate=' + tdate + '&option=' + $scope.Option;


            }
            else {

                if ($scope.Fromdate == undefined) {
                    $rootScope.Res = "Select FromDate";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else if ($scope.Option == undefined) {
                    $rootScope.Res = "Select Option";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "Provide Search Datas";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }

            }
        }
    }


});

