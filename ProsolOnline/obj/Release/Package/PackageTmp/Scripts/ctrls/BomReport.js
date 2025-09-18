
var Report = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);




    Report.controller('BomReportcontroller', function ($scope, $http, $timeout, $window ,$filter){

        $scope.selecteditem = 10;
        $scope.ddlItems = function () {
            if ($scope.searchkey1 == '' || $scope.searchkey1 == null)
                $scope.Loadpage();
            else
                $scope.searching1()
        };

        $scope.Loadpage = function () {
            $scope.cgBusyPromises = $http({
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
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            })

        }

        $scope.Loadpage();
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
        
        //$scope.checka = function () {
        //    alert(angular.toJson($scope.ind_A));
        //};
        $scope.searching1 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.sResult = {};
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Bom/searching1',
               params: {sKey: $scope.searchkey1, currentPage: 1, maxRows: $scope.selecteditem }
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

        $scope.TBMClick = function (TBM, idx) {
            $scope.NotifiyRes = false;

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


            $scope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/getreport?sKey=' + $scope.Itemcode + '&inda=' + true + '&indb=' + true + '&indc=' + true

            }).success(function (response) {
                //  alert(angular.toJson(response))
                if (response.length >0) {
                    $scope.NotifiyRes = false;
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
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
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
        $scope.Export = function () {
           
            //   alert(angular.toJson($scope.Result))
            $window.location = '/Bom/Download2?sKey=' + $scope.Itemcode + '&inda=' + $scope.ind_A + '&indb=' + $scope.ind_B + '&indc=' + $scope.ind_C

        }
        $scope.BulkExport = function () {
          
            //   alert(angular.toJson($scope.Result))
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

                $scope.cgBusyPromises = $http({
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

                        $scope.Res = "Loaded successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    } else {
                        $scope.Res = "Please enter valid material codes, if more than one code separate by comma(,)";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
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

                    $scope.cgBusyPromises = $http({
                        method: 'POST',
                        url: '/Bom/Trackload',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {
                      //  alert(angular.toJson($scope.response));
                        if (response != "" && response.length <= 100) {
                            $scope.tables1 = [];
                            $scope.tables1 = response;
                            // alert(angular.toJson($scope.tables));
                            $scope.len1 = response.length;

                            $scope.Res = "Loaded successfully";
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');


                        } else if (response != "" && response.length > 100) {
                            $scope.tables1 = [];
                            $scope.len1 = response.length;
                            $scope.Res = "100 records only load here, You can export more than 100 records";
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');

                        } else if (response == "") {
                            $scope.len1 = null;
                            $scope.tables1 = [];
                            $scope.Res = "No data avaliable for load";
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');

                        }

                        if (response.length == 0) {
                            $scope.reqtbls = [];
                            $scope.columns = [];
                            $scope.Res = "No data avaliable for Load";
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
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
                            $scope.Res = "Loaded Successfully";
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');
                        }
                    });
                }
                else {

                     if ($scope.Fromdate == undefined) {
                        $scope.Res = "Select FromDate";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                     else if ($scope.Option == undefined || $scope.Option == "") {
                        $scope.Res = "Select Option";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $scope.Res = "Provide Search Datas";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
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
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Bom/DownloadTrack1',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    if (response > 0)
                        $window.location = '/Bom/DownloadMulticode';
                    else {
                        $scope.Res = "Please enter valid material codes, if more than one code separate by comma(,)";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
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
                //    $scope.Res = "Please load first";
                //    $scope.Notify = "alert-danger";
                //    $scope.NotifiyRes = true;
                //    $('#divNotifiy').attr('style', 'display: block');
                //}


            } else {
                if ($scope.Fromdate != undefined  && $scope.Option != undefined) {

                    var fdate = null, tdate = null;
                    if ($scope.Fromdate != undefined)
                        fdate = $scope.Fromdate;
                    if ($scope.Todate != undefined)
                        tdate = $scope.Todate;



                    $window.location = '/Bom/DownloadTrack?plant=' + $scope.materialcode + '&Fromdate=' + fdate + '&Todate=' + tdate + '&option=' + $scope.Option;


                }
                else {
        
                     if ($scope.Fromdate == undefined) {
                        $scope.Res = "Select FromDate";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if ($scope.Option == undefined) {
                        $scope.Res = "Select Option";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $scope.Res = "Provide Search Datas";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }

                }
            }
        }


    });
