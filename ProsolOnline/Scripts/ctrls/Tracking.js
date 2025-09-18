(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy']);




    app.controller('Trackingcontroller', function ($scope, $http, $timeout, $window) {

       
            $http({
                method: 'GET',
                url: '/Master/GetMaster'
            }).success(function (response) {

                $scope.MasterList = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        

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
      
        $("#txtTo").datepicker({ maxDate: new Date() });
        //$("#txtTo").datepicker({
        //    numberOfMonths: 1,
        //    //onSelect: function (selected) {
        //    //    var dt = new Date(selected);
        //    //    dt.setDate(dt.getDate() );
        //    //    $("#txtFrom").datepicker("option", "maxDate", dt);
        //    //}
        //});

        var array = [];
        $http.get('/Report/getplant').success(function (response) {
            $scope.gtplant = response
        });//to get plant

        //$http.get('/User/getdepartment').success(function (response) {
        //    $scope.gtdepartment = response
        //});//to get department

        $scope.export = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            if ($scope.dwldata != undefined && $scope.dwldata.length > 0) {
                //  window.open ("/Report/exportexcel",angular.toJson($scope.dwldata));
                //  $scope.dwldata = [];

                var blob = new Blob([document.getElementById('tblid').innerHTML], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
                });
                // saveAs(blob, "Report.xls");
                $scope.filename = $scope.Plant;
                saveAs(blob, $scope.filename + ".xls");
                $scope.Res = "Exported successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            } else {
                $scope.Res = "No data avaliable for Download";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }//to export the data

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }


        $scope.trackingload = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            $scope.flg = "A";
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
                    url: '/Report/Trackload',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    if (response != "" && response.length <= 100) {
                        $scope.tables = [];
                        $scope.tables = response;
                        // alert(angular.toJson($scope.tables));
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
                if ($scope.Option == "MaterialType") {
                    if ($scope.Materialtype == null && $scope.Materialtype == undefined)
                    {
                        $scope.flg = "B";
                    }
                }
                if ($scope.Fromdate != undefined && $scope.Plant != undefined && $scope.Option != undefined && $scope.flg == "A") {
                    
                        var formData = new FormData();
                        formData.append("plant", $scope.Plant);

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
                        formData.append("MaterialType", $scope.Materialtype);
                        $scope.cgBusyPromises = $http({
                            method: 'POST',
                            url: '/Report/Trackload',
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData,
                        }).success(function (response) {

                            if (response != "" && response.length <= 100) {
                                $scope.tables = [];
                                $scope.tables = response;
                                // alert(angular.toJson($scope.tables));
                                $scope.len = response.length;

                                $scope.Res = "Loaded successfully";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');


                            } else if (response != "" && response.length > 100) {
                                $scope.tables = [];
                                $scope.len = response.length;
                                $scope.Res = "100 records only load here, You can export more than 100 records";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');

                            } else if (response == "") {
                                $scope.len = null;
                                $scope.tables = [];
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

                                if ($scope.Option == "Request") {
                                    $scope.columns = ["RequestId", "ItemId", "Source", "Plant", "Requester", "Approver", "Cataloguer", "RequestedOn", "ApprovedOn", "RejectedOn", "ItemStatus", "Reason_rejection"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.len = response.length;
                                } else if ($scope.Option == "Created") {
                                    $scope.columns = ["Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Cataloguer", "Reviewer", "Releaser", "Plant", "Remarks"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.trackdata = response;
                                    $scope.len = response.length;
                                } else {
                                    $scope.columns = ["Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Cataloguer", "Reviewer", "Releaser", "Plant", "Duplicates", "Remarks"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.trackdata = response;
                                    $scope.len = response.length;

                                }
                                $scope.Res = "Loaded Successfully";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');
                            }
                        });
                    
                   
                }
                else if ($scope.Option == "PVcompleted" || $scope.Option == "Overall") {
                    
                        var formData = new FormData();
                        formData.append("plant", $scope.Plant);

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
                        formData.append("MaterialType", $scope.Materialtype);
                        $scope.cgBusyPromises = $http({
                            method: 'POST',
                            url: '/Report/Trackload',
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData,
                        }).success(function (response) {

                            if (response != "" && response.length <= 100) {
                                $scope.tables = [];
                                $scope.tables = response;
                                // alert(angular.toJson($scope.tables));
                                $scope.len = response.length;

                                $scope.Res = "Loaded successfully";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');


                            } else if (response != "" && response.length > 100) {
                                $scope.tables = [];
                                $scope.len = response.length;
                                $scope.Res = "100 records only load here, You can export more than 100 records";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');

                            } else if (response == "") {
                                $scope.len = null;
                                $scope.tables = [];
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

                                if ($scope.Option == "Request") {
                                    $scope.columns = ["RequestId", "ItemId", "Source", "Plant", "Requester", "Approver", "Cataloguer", "RequestedOn", "ApprovedOn", "RejectedOn", "ItemStatus", "Reason_rejection"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.len = response.length;
                                } else if ($scope.Option == "Created") {
                                    $scope.columns = ["Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Cataloguer", "Reviewer", "Releaser", "Plant", "Remarks"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.trackdata = response;
                                    $scope.len = response.length;
                                } else {
                                    $scope.columns = ["Itemcode", "CreatedOn", "Legacy", "Shortdesc", "Cataloguer", "Reviewer", "Releaser", "Plant", "Duplicates", "Remarks"];
                                    $scope.reqtbls = [];
                                    $scope.reqtbls.push({ rows: response });
                                    $scope.cols = $scope.columns;
                                    $scope.dwldata = response;
                                    $scope.trackdata = response;
                                    $scope.len = response.length;

                                }
                                $scope.Res = "Loaded Successfully";
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');
                            }
                        });
                    
                   
                }
                else {
                  
                   
                    if ($scope.Plant == undefined) {
                        $scope.Res = "Choose Plant";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if ($scope.Fromdate == undefined) {
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
                    else if ($scope.Materialtype == undefined) {
                        $scope.Res = "Select Material Type";
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


        $scope.exportTrack = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);

            if ($scope.materialcode != "" && $scope.materialcode != null && $scope.materialcode != undefined) {         
                var formData = new FormData();               
                formData.append("materialcode", $scope.materialcode);
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Report/DownloadTrack1',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    if (response>0)
                        $window.location = '/Report/DownloadMulticode';
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
                if ($scope.Fromdate != undefined && $scope.Plant != undefined && $scope.Option != undefined) {

                    var fdate = null, tdate = null;
                    if ($scope.Fromdate != undefined)
                        fdate = $scope.Fromdate;
                    if ($scope.Todate != undefined)
                        tdate = $scope.Todate;



                    $window.location = '/Report/DownloadTrack?plant=' + $scope.Plant + '&Fromdate=' + fdate + '&Todate=' + tdate + '&option=' + $scope.Option + '&MaterialType=' + $scope.Materialtype;


                }
                else if ($scope.Option == "PVcompleted" || $scope.Option == "Overall") {

                    var fdate = null, tdate = null;
                    if ($scope.Fromdate != undefined)
                        fdate = $scope.Fromdate;
                    if ($scope.Todate != undefined)
                        tdate = $scope.Todate;



                    $window.location = '/Report/DownloadTrack?plant=' + $scope.Plant + '&Fromdate=' + fdate + '&Todate=' + tdate + '&option=' + $scope.Option + '&MaterialType=' + $scope.Materialtype;


                }
                else {
                    if ($scope.Plant == undefined) {
                        $scope.Res = "Choose Plant";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if ($scope.Fromdate == undefined) {
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
    app.controller('Erplogcontroller', function ($scope, $http, $timeout, $window, $rootScope) {

        $scope.getlog = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            $scope.ErpLogs = null;
            $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Report/GetERPLogs',
                params: { code: $scope.materialcode }
                //?sKey=' + $scope.searchkey + '&sBy=' + $scope.search
            }).success(function (response) {
                if (response != '') {
                    $scope.ErpLogs = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                } else {
                    $scope.ErpLogs = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.ClearItem = function () {

            $scope.materialcode = null;
            $scope.ErpLogs = null;
            $scope.reset();
        }
    });
})();