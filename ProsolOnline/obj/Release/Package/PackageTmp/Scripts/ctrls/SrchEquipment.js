
var app = angular.module('ProsolApp', ['cgBusy','ui.bootstrap']);
app.controller('SrchEquipmentcontroller', function ($scope, $http, $rootScope, $timeout, $window) {

    $scope.selecteditem = 10;
    $scope.ddlItems = function () {
        if ($scope.searchkey == '' || $scope.searchkey == null) {
            $scope.bindAssets();
        } else {
            $scope.TrackBom();
        }
    };
    $scope.pageLoad = function () {
        $scope.promise = $http({

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
    $scope.pageLoad();




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
             alert("error");
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

    $scope.TrackBom = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.reset = false;
        $scope.srchResult = {};
        $scope.promise = $http({

            method: 'GET',
            url: '/SrchEquipment/funlocsearch?sKey=' + $scope.searchkey

        }).success(function (response) {
            if (response != '') {
                $scope.srchResult = response;
                $scope.Res = "Track results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.clear = false;
            }
            else {
                $scope.srchResult = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });

    };
    $scope.BulkBom = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
      
        $scope.promise = $http({

            method: 'GET',
            url: '/Bom/BulkBom?sKey=' + $scope.bsearchkey

        }).success(function (response) {
            $('#divNotifiy').attr('style', 'display: Block');
            if (response != '') {
                $scope.blukresult = response;
                $scope.Res = "Track results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.clear1 = false;
                $scope.bexp = false;
                $scope.reset1 = false;
            }
            else {
                $scope.blukresult = null;

                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.reset1 = true;
            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    //}).success(function (response) {

    //    if (response != '') {
    //        $scope.reset1 = true;
    //        $scope.blukresult = response;
    //        //$scope.blukresult = response.listtt;
    //        //if (response.emptyval != "") {
    //        //    $scope.Res = "Track results : " + response.listtt.length + " items."
    //        //    alert(angular.toJson("No records found for " + response.emptyval));
    //        //    $scope.bexp = true;
    //        //}
    //        //else
    //        $scope.Res = "Track results : " + blukresult.length + " items."
    //        $scope.Notify = "alert-info";
    //        $scope.NotifiyRes = true;
    //        $scope.clear1 = false;
    //        $scope.bexp = false;
    //        $scope.reset1 = false;



    //    }
    //    else {
    //        $scope.blukresult = null;
    //        $scope.Res = "No item found"
    //        $scope.Notify = "alert-danger";
    //        $scope.NotifiyRes = true;
    //        $scope.reset1 = true;

    //    }

    //}).error(function (data, status, headers, config) {
    //    // alert("error");


    $scope.ClearItem1 = function () {
        $scope.form.$setPristine();
        $scope.bsearchkey = null;
        $scope.reset1 = true;
        $scope.bexp = true;
        $scope.clear1 = true;

    }
    $scope.ClearItem = function () {
        $scope.form.$setPristine();
        $scope.searchkey = null;
        $scope.reset = true;
        $scope.Noitem = true;
        $scope.clear = true;

    }

    $scope.BomTrackingClick = function (TBM1, idx) {
       
        $scope.NotifiyRes = false;

        var i = 0;
        angular.forEach($scope.srchResult, function (lst) {
            $('#' + i).attr("style", "");
            i++;
        });

        $('#' + idx).attr("style", "background-color:lightblue");

        $scope.TechIdentNo = TBM1.TechIdentNo
        $scope.EquipDesc = TBM1.EquipDesc
        $scope.FunctLocation = TBM1.FunctLocation
        $scope.FunctDesc = TBM1.FunctDesc
        $scope.Objecttype = TBM1.Objecttype
        $scope.Manufacturer = TBM1.Manufacturer
        $scope.show = true;
        $scope.promise = $http({

          
            method: 'GET',
            url: '/Bom/getitemb?sKey=' + $scope.TechIdentNo

        }).success(function (response) {
            if (response != 'true') {

                $scope.Result = response;
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
                $scope.Result = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            }

        })

    }

    $scope.BomTrackExport = function () {
        //   alert(angular.toJson($scope.Result))
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $window.location = '/Bom/Download?sKey=' + $scope.TechIdentNo;

    }

    $scope.BulkBomTrackExport = function () {

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

    //$scope.promise = $http({

    //    method: 'GET',
    //    url: '/SrchEquipment/funlocsearch1'

    //}).success(function (response) {
    //    if (response != '') {
    //        $scope.srchResult = response.lst;

    //    }
    //    else {
    //        $scope.srchResult = null;

    //    }

    //})
    $scope.Noitem = true;
    $scope.clear = true;
    $scope.clear1 = true;
    $scope.bexp = true;


    $scope.TrackBom = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.reset = false;
        $scope.srchResult = {};
        $scope.promise = $http({

            method: 'GET',
            url: '/Bom/funlocsearch?sKey=' + $scope.searchkey

        }).success(function (response) {
            if (response != '') {
                $scope.srchResult = response;
                $scope.Res = "Track results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.clear = false;
            }
            else {
                $scope.srchResult = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });

    };



    $scope.ClearItem3 = function () {
        $scope.form.$setPristine();
        $scope.searchkey = null;
        $scope.reset = true;
        $scope.Noitem = true;
        $scope.clear = true;

    }

    $scope.BomTrackingClick1 = function (TBM1, idx) {
        $scope.NotifiyRes = false;
        //alert("hi")
        var i = 0;
        angular.forEach($scope.srchResult, function (lst) {
            $('#' + i).attr("style", "");
            i++;
        });

        $('#' + idx).attr("style", "background-color:lightblue");

        $scope.TechIdentNo = TBM1.TechIdentNo
        $scope.EquipDesc = TBM1.EquipDesc
        $scope.FunctLocation = TBM1.FunctLocation
        $scope.FunctDesc = TBM1.FunctDesc
        $scope.Objecttype = TBM1.Objecttype
        $scope.Manufacturer = TBM1.Manufacturer
        $scope.sfunloc = TBM1.SupFunctLoc

        $scope.show = true;
        $scope.promise = $http({

            method: 'GET',
            url: '/Bom/getitem?sKey=' + $scope.FunctLocation

        }).success(function (response) {
            if (response != 'true') {

                $scope.Result = response;
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
                $scope.Result = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            }

        })

    }

  

    $scope.BomReportExport = function () {

        //   alert(angular.toJson($scope.Result))
        $window.location = '/Bom/Download2?sKey=' + $scope.Itemcode + '&inda=' + $scope.ind_A + '&indb=' + $scope.ind_B + '&indc=' + $scope.ind_C

    }
    $scope.BulkBomReportExport = function () {

        //   alert(angular.toJson($scope.Result))
        $window.location = '/Bom/DetailDownload?sKey=' + $scope.Itemcode
    }
    $scope.BulkExportsum = function () {

        $window.location = '/Bom/BulkMatDownload?sKey=' + $scope.Itemcode

    }
    $scope.LoadBOM = function () {
        $http({
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
                }, 30);
                $scope.sResult = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }

        })
    }
    $scope.LoadBOM();
    $scope.bindMatsInx = function (inx) {

        if ($scope.searchkey1 == '' || $scope.searchkey1 == null) {
            $scope.cgBusyPromises = $http({

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

            $scope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/searching1',
                params: { sKey: $scope.searchkey1, currentPage: inx, maxRows: $scope.selecteditem }

            }).success(function (response) {
                if (response.matList != null) {
                    $scope.sResult = response.matList;
                    $scope.Res = "Search results : " + response.matList.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.clear = false;
                    $scope.reset = false;
                    $scope.Report = null;

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;

                }
                else {
                    $scope.sResult = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

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

    $scope.searching1 = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.sResult = {};
        $scope.promise = $http({
            method: 'GET',
            url: '/Bom/searching1?sKey=' + $scope.searchkey1
        }).success(function (response) {

            if (response != '') {
                $scope.sResult = response;
                $scope.Res = "Search results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.clear = false;
                $scope.reset = false;
                $scope.Report = null;

            } else {

                $scope.sResult = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

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
        $scope.reset = true;
        $scope.Noitem = true;
        $scope.clear = true;
        location.reload();

    }

    $scope.BomReportClick = function (TBM, idx) {
        $scope.NotifiyRes = false;

        var i = 0;
        angular.forEach($scope.sResult, function (lst) {
            $('#' + i).attr("style", "");
            i++;
        });

        $('#' + idx).attr("style", "background-color:lightblue");
        mymodal.open();
        $scope.Itemcode = TBM.Itemcode
        $scope.Shortdesc = TBM.Shortdesc

        $scope.Noun = TBM.Noun
        $scope.Modifier = TBM.Modifier

        $scope.Manufacturer = TBM.Manufacturer
        $scope.Partno = TBM.Partno
        $scope.Noitem = false;
        $scope.report = function () {


            $scope.promise = $http({

                method: 'GET',
                url: '/Bom/getreport?sKey=' + $scope.Itemcode + '&inda=' + $scope.ind_A + '&indb=' + $scope.ind_B + '&indc=' + $scope.ind_C

            }).success(function (response) {
                //  alert(angular.toJson(response))
                if (response != '') {
                    $scope.NotifiyRes = false;
                    mymodal.close();
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
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }

            })



        };

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
                $scope.Res = "Search results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.b1 = false;
                $scope.b2 = false;

            } else {

                $scope.b1 = true;
                $scope.b2 = true;
                $scope.blukmat = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {

        });


    };


});

