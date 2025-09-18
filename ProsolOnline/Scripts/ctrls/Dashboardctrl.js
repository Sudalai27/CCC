(function () {
    'use strict';

    var app = angular.module('ProsolApp', ['cgBusy']);

    app.controller('DashboardController', function ($scope, $http, $timeout, $window, $filter) {




        $("#txtdte2").datepicker({
            numberOfMonths: 1, dateFormat: 'dd-M-yy',
            onSelect: function (selected) {
                $scope.dte = $('#txtdte2').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                $("#txtdte3").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });
        $("#txtdte3").datepicker({
            numberOfMonths: 1, dateFormat: 'dd-M-yy',

        });

        $scope.dte = $filter('date')(new Date(), 'dd-MMM-yyyy'); //new Date().toLocaleDateString();
        $scope.dte1 = $filter('date')(new Date(), 'dd-MMM-yyyy'); //new Date().toLocaleDateString();


        //$scope.loadNM = function () {
        //    $http({
        //        method: 'GET',
        //        url: '/Dashboard/BindTotalItem'
        //    }).success(function (response) {
        //        $scope.PlantList = response;

        //        var data3 = new google.visualization.DataTable();
        //        data3.addColumn('date', 'Month');
        //        data3.addColumn('number', 'Vendor/Manufacturer (' + response[0].TotVendors + ')');

        //        angular.forEach(response[0].VendorDataList, function (lst) {
        //            data3.addRow([new Date(lst.Code), Number(lst.Name)])
        //        })

        //        var options = {
        //            title: 'Vendor/Manufacturer created by month/year',
        //            legend: { position: 'top' },
        //            vAxis: { title: 'Number of vendor/Manufacturer' },
        //            //  hAxis: { title: 'Month/Year' }

        //        };

        //        var chart3 = new google.visualization.LineChart(document.getElementById('chart_div3'));
        //        chart3.draw(data3, options);
        //    }).error(function (data, status, headers, config) {

        //    });
        //}

        $scope.drawChart = function () {


            $http({
                method: 'GET',
                url: '/Dashboard/BindTotalItem'
            }).success(function (response) {
                $scope.PlantList = response;
              
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Month');

                //var data1 = new google.visualization.DataTable();
                //data1.addColumn('string', 'Plant Name');
                //data1.addColumn('number', 'Pending');               
                //data1.addColumn('number', 'Approved');
                //data1.addColumn('number', 'Rejected');

                var data2 = new google.visualization.DataTable();
                data2.addColumn('string', 'Plant Name');
                data2.addColumn('number', 'Catalogue');
                data2.addColumn('number', 'Review');
                data2.addColumn('number', 'Release');

                var data3 = new google.visualization.DataTable();
                data3.addColumn('date', 'Month');
                data3.addColumn('number', 'Vendor/Manufacturer (' + response[0].TotVendors + ')');

                angular.forEach(response[0].VendorDataList, function (lst) {
                    data3.addRow([new Date(lst.Code), Number(lst.Name)])
                })

                var data4 = new google.visualization.DataTable();
                data4.addColumn('date', 'Month');
                data4.addColumn('number', 'Noun & Modifiers (' + response[0].TotNounModifiers + ')');
                angular.forEach(response[0].NMDataList, function (lst) {
                    data4.addRow([new Date(lst.Noun), Number(lst.Modifier)])

                })


                $.each(response, function (lst, row) {
                    data.addColumn('number', row.PlantName);
                    //  data1.addRow([row.PlantName, row.TotRequest, row.TotApprove, row.TotReject]);
                    data2.addRow([row.PlantName, row.TotCatalogue, row.TotReview, row.TotRelease]);

                });
                var arr = [];
                $scope.calColumns = function () {
                    arr.push(0);
                    $.each(response, function (lst, row) {
                        arr.push(0);
                    });
                }

                var inc = 1;
                var listOfObjects = [];
                angular.forEach(response, function (lst) {

                    angular.forEach(lst.DataList, function (lst1) {
                        $scope.calColumns();
                        arr.splice(0, 1)
                        // arr.splice(0, 0, new Date(lst1.Itemcode));
                        arr.splice(0, 0, lst1.Itemcode);
                        arr.splice(inc, 1)
                        arr.splice(inc, 0, lst1.ItemStatus);
                        var flg = 0;
                        for (var i = 0; i < listOfObjects.length; i++) {
                            if (listOfObjects[i][0] === lst1.Itemcode) {
                                flg = 1;
                                listOfObjects[i].splice(inc, 1, arr[inc])
                            }
                        }
                        if (flg == 0)
                            listOfObjects.push(arr);
                        arr = [];

                    });
                    inc = inc + 1;
                })
                listOfObjects.forEach(function (entry) {
                    data.addRow(entry);
                });


                // for Total item month & year

                var options = {
                    title: 'Item created by month/year',
                    vAxis: { title: 'Number of items' },
                    seriesType: 'bars',
                    bar: { groupWidth: (15 * response.length) },
                    // bars: 'vertical',
                    // vAxis: {format: 'decimal'},               
                    colors: ['#1b9e77', '#d95f02', '#7570b3', '#e8b2b2', '#9b7a7a', '#efbfbf', '#efe1e1', '#edddc9', '#c6b6a3', '#e0efc2', '#a1b280', '#96e0b0', '#aef2c6', '#88c8c9', '#77c3c4', '#8ea1e5', '#3c4566', '#c49ce2', '#6b537c', '#e88fc6', '#e06279']
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div1'));
                chart.draw(data, options);

                // for catalogue & cleansing

                var options = {
                    title: 'Cleansing/Enrichment detail by plant',
                    //  legend: { position: 'top', maxLines: 3 },
                    vAxis: { title: 'Number of items' },
                    seriesType: 'bars',
                    bar: { groupWidth: 40 },
                    //  isStacked: true,
                    colors: ['#1b9e77', '#d95f02', '#7570b3']
                };

                //  var chart2 = new google.visualization.SteppedAreaChart(document.getElementById('chart_div2'));
                //  var chart2 = new google.visualization.ColumnChart(document.getElementById('chart_div2'));
                var chart2 = new google.visualization.ComboChart(document.getElementById('chart_div2'));
                chart2.draw(data2, options);


                //// for New item request
                //var options = {
                //    title: 'Item request detail by plant',
                //    vAxis: { title: 'Number of items request' },
                //    //  legend: { position: 'top', maxLines: 3 },
                //    seriesType: 'bars',
                //    bar: { groupWidth: 40 },
                //   // isStacked: true,
                //    colors: ['#7570b3', '#1b9e77', '#d95f02']
                //};

                ////var chart1 = new google.visualization.ColumnChart(document.getElementById('chart_div'));
                //var chart1 = new google.visualization.ComboChart(document.getElementById('chart_div'));
                //chart1.draw(data1, options);

                var options = {
                    title: 'Vendor/Manufacturer created by month/year',
                    legend: { position: 'top' },
                    vAxis: { title: 'Number of vendor/Manufacturer' },
                    //  hAxis: { title: 'Month/Year' }

                };

                var chart3 = new google.visualization.LineChart(document.getElementById('chart_div3'));
                chart3.draw(data3, options);

                var options = {
                    title: 'Noun & Modifier created by month/year',
                    legend: { position: 'top' },
                    vAxis: { title: 'Number of Noun & Modifier' },
                    //  hAxis: { title: 'Month/Year' }

                };

                var chart4 = new google.visualization.LineChart(document.getElementById('chart_div4'));
                chart4.draw(data4, options);

            }).error(function (data, status, headers, config) {

            });
        }

        $scope.bindReleaser = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/BindReleaser'
            }).success(function (response) {
                $scope.RelList = response;
                $scope.usrsts =
                 $http({
                     method: 'GET',
                     url: '/Dashboard/BindReviewTarget'
                 }).success(function (response) {
                     $scope.RelTargetList = response;

                 }).error(function (data, status, headers, config) {

                 });

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindReview = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/BindReview'
            }).success(function (response) {
                $scope.RevList = response;

                $http({
                    method: 'GET',
                    url: '/Dashboard/BindCatalogueTarget'
                }).success(function (response) {
                    $scope.RevTargetList = response;

                }).error(function (data, status, headers, config) {

                });


            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindCatalogue = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/BindCatalogue'

            }).success(function (response) {
                $scope.CatList = response;

                //$http({
                //    method: 'GET',
                //    url: '/Dashboard/BindApproveTarget'
                //}).success(function (response) {
                //    $scope.CatTargetList = response;

                //}).error(function (data, status, headers, config) {

                //});

            }).error(function (data, status, headers, config) {

            });
        };

        $scope.bindApprove = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/BindApprove'
            }).success(function (response) {
                $scope.AppList = response;

                $http({
                    method: 'GET',
                    url: '/Dashboard/BindRequestTarget'
                }).success(function (data) {
                    $scope.AppTargetList = data;
                }).error(function (data, status, headers, config) {

                });


            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindRequest = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/BindRequest'
            }).success(function (response) {
                $scope.ReqList = response;

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindReleaser();
        $scope.bindReview();
        $scope.bindCatalogue();
        // $scope.bindApprove();
        // $scope.bindRequest();




        $scope.bindItemHistoryGO = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dashboard/BindItem',
                params :{dte: $scope.dte,dte1: $scope.dte1}
            }).success(function (response) {
                $scope.ItemHistoryList = response;
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dashboard/overalldatalist',
                    params :{dte: $scope.dte ,dte1: $scope.dte1}
                }).success(function (response) {

                    if (response != false) {
                        $scope.overalldata = response;
                        $scope.showOverallReport = true;
                    }
                    else {
                        $scope.showOverallReport = false;
                        $scope.overalldata = [];
                    }
                }).error(function (data, status, headers, config) {

                });



            }).error(function (data, status, headers, config) {

            });






        };


        $scope.bindItemHistoryGO();



    });
    app.controller('UploadController', function ($scope, $http, $timeout, $window,$rootScope, $filter) {
        $scope.viewadd = true;
        $scope.viewclose = false;
        $scope.shinsertupdate = false
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
           

        };
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
           
            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'pdf' || angular.lowercase(ext) === 'docx') {
                } else {
                   
                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid PDF/WORD file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                   
                }
            }
        };
        $scope.getuserinfo = function () {
         
            $http({
                method: 'GET',
                url: '/User/getusrinfo'
            }).success(function (response) {
           
                if (response.length==0)
                {
                    $scope.showupload = false;
               }
                else {
               
                   $scope.showupload = true;
               }
            
            });
        }
        $scope.getuserinfo();
        $scope.UploadFile = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
        
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles()

                    $scope.ShowHide = false;
                    if (data == 2 || 0) {

                        $rootScope.Res = "File already exists"
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = " File uploaded successfully"
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }





                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
     
        $scope.getfiles = function () {

          
            $rootScope.promise1 = $http({
                    method: 'GET',
                    url: "/Dashboard/fileslist",
                }).success(function (response) {
                    $scope.Folderfiles = response;
                }).error(function (data, status, headers, config) {

                });


        };
       
        $scope.getfiles()
      
        $scope.Remove = function (x) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile?FileName=' + x
            }).success(function (response) {
                if(response == 1)
                {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });
            
            };
        
    });

})();