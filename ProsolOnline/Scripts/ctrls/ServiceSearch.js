(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['angular.filter']);

    app.controller('ServiceSearchController', function ($scope, $http, $rootScope, $timeout) {

        $scope.TableData = [];
        $scope.singlerow1 = [];
        $scope.tableVisible = false;
      //  $scope.ServiceCategoryCode = "";
        $http({
            method: 'GET',
            url: '/GeneralSettings/GetUnspsc',
            params: { Noun: 'Service', Modifier: 'Service' }
        }).success(function (response) {

            if (response != '') {
                $scope.Commodities = response;
            }
            else {
                $scope.Commodities = null;
            }

        }).error(function (data, status, headers, config) {
            // alert("error");

        });
        $http.get('/ServiceSearch/getcategory').success(function (response) {
            //alert('hi');
            $scope.cate = response
            //alert(angular.toJson($scope.cate))
        });

        $scope.getgrpppp = function (ServiceCategorycode) {
            // alert(ServiceCategorycode);
            $scope.ServiceGroupCode = null;
            $http.get('/ServiceSearch/getgroupp?ServiceCategorycode=' + ServiceCategorycode
                ).success(function (response) {
                    //alert('hi');
                    $scope.grpppSS = response;
                    //alert(angular.toJson($scope.grpppSS))
                }).error(function (data, status, headers, config) {
                });

        };


      //  $http.get('/ServiceSearch/getUOM').success(function (UOM) {
        //    $scope.UomSS = UOM;
        //});
        $http.get('/ServiceMaster/getuomlist').success(function (UOM) {
            //   alert('hi');
            $scope.UomSS = UOM;
            //  alert(angular.toJson($scope.UomSS))
        });
        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $scope.clearfields = function () {
         //   alert("hi");

         //   $scope.form.$setPristine();
            $scope.TableData = null;
            //  $scope.singlerow1 = [];
            $scope.tableVisible = false;
            $scope.searchkey = null;
            
            $scope.grpppSS = [];
            $scope.reset();
            //$http.get('/ServiceSearch/getcategory').success(function (response) {
            //    //alert('hi');
            //    $scope.cate = response
            //    //alert(angular.toJson($scope.cate))
            //});

            //$http.get('/ServiceSearch/getUOM').success(function (UOM) {
            //    $scope.UomSS = UOM;
            //});
            

        };

        $scope.ServiceMasterSearch = function () {
       //     $timeout(function () { $scope.NotifiyRes = false; }, 5000);
       $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            //if (($scope.ServiceCategoryCode != "undefined" && $scope.ServiceCategoryCode != null && $scope.ServiceCategoryCode != "") || ($scope.ServiceGroupCode != "undefined" && $scope.ServiceGroupCode != null && $scope.ServiceGroupCode != "") || ($scope.UomCode != "undefined" && $scope.UomCode != null && $scope.UomCode != "") || ($scope.ServiceCode != "undefined" && $scope.ServiceCode != null && $scope.ServiceCode != "") || ($scope.ShortDesc != "undefined" && $scope.ShortDesc != null && $scope.ShortDesc != "") || ($scope.LongDesc != "undefined" && $scope.LongDesc != null && $scope.LongDesc != ""))
            //{
              // alert("hi");
                var formdata = new FormData();
                //formdata.append("ServiceCategoryCode", angular.toJson($scope.ServiceCategoryCode));
                //formdata.append("ServiceGroupCode", angular.toJson($scope.ServiceGroupCode));
                //formdata.append("UomCode", angular.toJson($scope.UomCode));
                //formdata.append("ServiceCode", angular.toJson($scope.ServiceCode));
                //formdata.append("ShortDesc", angular.toJson($scope.ShortDesc));
                //formdata.append("LongDesc", angular.toJson($scope.LongDesc));
                formdata.append("search", angular.toJson($scope.search));
                formdata.append("searchkey", angular.toJson($scope.searchkey));
                $http({
                    url: "/ServiceSearch/ServiceMasterSearch",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formdata
                }).success(function (response) {
                    $scope.TableData = response;
                   // alert(angular.toJson($scope.TableData));

                    if ($scope.TableData != '') {
                        $scope.tableVisible = true;
                        $rootScope.Res = "Search results : " + response.length + " items."
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $scope.tableVisible = false;

                        $rootScope.Res = "No records found for your search, try again";
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                }).error(function () {
                });
            //}
            //else
            //{
            //    $scope.TableData = null;
            //    $scope.tableVisible = false;
            //    if (($scope.ServiceCategoryCode == "undefined" || $scope.ServiceCategoryCode == null || $scope.ServiceCategoryCode == "") && ($scope.ServiceGroupCode == "undefined" || $scope.ServiceGroupCode == null || $scope.ServiceGroupCode == "") && ($scope.UomCode == "undefined" || $scope.UomCode == null || $scope.UomCode == "") && ($scope.ServiceCode == "undefined" || $scope.ServiceCode == null || $scope.ServiceCode == "") && ($scope.ShortDesc == "undefined" || $scope.ShortDesc == null || $scope.ShortDesc == "") && ($scope.LongDesc == "undefined" || $scope.LongDesc == null || $scope.LongDesc == ""))
            //    {
            //        $rootScope.Res = "Pls provide any value(s)";
            //        $rootScope.Notify = "alert-info";
            //        $rootScope.NotifiyRes = true;
            //        $('#divNotifiy').attr('style', 'display: block');
            //       // alert("Provide any data");

            //    }
               

            //}
        };

        $scope.NotifiyResclose = function () {
            //    alert("hi");
            $('#divNotifiy').attr('style', 'display: none');
        };

        //$scope.SearchCode = function (cat, grp, uom) {
        //   // alert(grp);
        //    if (cat === null)
        //        cat = "undefined";
        //    if (grp === null)
        //        grp = "undefined";
        //    if (uom === null)
        //        uom = "undefined";

        //  // alert(uom);
        //   //alert(grp);
        //    $http.get('/ServiceSearch/getServiceCode?cat=' + cat + '&grp=' + grp + '&uom=' + uom
        //        ).success(function (response) {
        //            $scope.TableData = response;

        //            if ($scope.TableData != '')
        //                $scope.tableVisible = true;
        //            else
        //                $scope.tableVisible = false;
        //            //alert(angular.toJson(response));
        //        });
        //};

        //$scope.getdetailsforcode = function (code) {
        //   // alert(code);
        //    if(code != '')
        //    {
        //      //  alert(code);
        //        $http.get('/ServiceSearch/getdetailsforcode?code=' + code
        //            ).success(function (response) {
        //                $scope.TableData = response;                       
        //                if ($scope.TableData != '')
        //                    $scope.tableVisible = true;
        //                else
        //                    $scope.tableVisible = false;                       
        //                // alert(angular.toJson($scope.TableData));
        //            });
        //    }
        //    else
        //    {
        //        $scope.TableData = [];
        //        $scope.tableVisible = false;               
        //    }
        //};

        //$scope.getdetailsforsd = function (sd) {
        //    if (sd != '') {
        //        //  alert(code);
        //        $http.get('/ServiceSearch/getdetailsforsd?sd=' + sd
        //            ).success(function (response) {
        //                $scope.TableData = response;
        //                if ($scope.TableData != '')
        //                    $scope.tableVisible = true;
        //                else
        //                    $scope.tableVisible = false;
        //               //  alert(angular.toJson($scope.TableData));
        //            });
        //    }
        //    else {
        //        $scope.TableData = [];
        //        $scope.tableVisible = false;
        //    }
        //};
      
        //$scope.getdetailsforld = function (ld) {
        //    if (ld != '') {
        //        //  alert(code);
        //        $http.get('/ServiceSearch/getdetailsforld?ld=' + ld
        //            ).success(function (response) {
        //                $scope.TableData = response;
        //                if ($scope.TableData != '')
        //                    $scope.tableVisible = true;
        //                else
        //                    $scope.tableVisible = false;
        //               // alert(angular.toJson($scope.TableData));
        //            });
        //    }
        //    else {
        //        $scope.TableData = [];
        //        $scope.tableVisible = false;
        //    }
        //};

        $scope.clickToOpen = function (idx) {
        
            $scope.singlerow = $scope.TableData[idx];
            $scope.show_att = $scope.TableData[idx].Characteristics;
            //$http.get('/ServiceSearch/gettabledetails?ServiceCode=' + ServiceCode
            //    ).success(function (response) {  
            //           // alert("hi");
                  
            //          //  alert(angular.toJson($scope.singlerow));
                   
            //    });

        };

    });
   

    app.controller('ServiceDashboardController', function ($scope, $http, $rootScope, $timeout) {
        // alert("hai");


        $http({
            method: 'GET',
            url: '/ServiceSearch/BindTotalItemService'
        }).success(function (response) {
            $scope.totallist = response;


            //  $.each(response, function (lst, row) {
            //  var data = new google.visualization.DataTable();
            // data.addColumn('number', row.PlantName);
            // data1.addRow([row.MainCategory, row.TotalCategory, row.CompletedCategory]);
            //data2.addRow([row.PlantName, row.TotCatalogue, row.TotReview, row.TotRelease]);

            //  });

        });

        $http({
            method: 'GET',
            url: '/ServiceSearch/BindTotalItemServiceCategory'
        }).success(function (response) {
            $scope.totallistcategory = response;

        });
        //group
        $http({
            method: 'GET',
            url: '/ServiceSearch/BindTotalItemServiceGroup'
        }).success(function (response) {
            $scope.totallistcategorygroup = response;

        });

        $scope.drawChart1 = function () {

            // alert("HAI");
            $http({
                method: 'GET',
                url: '/ServiceSearch/BindTotalItemService'
            }).success(function (response) {
                $scope.totallist = response;
                //  alert(angular.toJson($scope.totallist));
                var data1 = new google.visualization.DataTable();
                data1.addColumn('string', 'PlantName');
                data1.addColumn('number', 'TotalItems');
                data1.addColumn('number', 'CompletedItems');

                //var data2 = new google.visualization.DataTable();
                //data2.addColumn('string', 'MainCategoryName');
                //data2.addColumn('number', 'TotalCategory');
                //data2.addColumn('number', 'CompletedCategory');

                var options = {
                    title: 'Created Service',
                    //  legend: { position: 'top', maxLines: 3 },
                    vAxis: { title: 'Total Number of Service' },
                    seriesType: 'bars',
                    bar: { groupWidth: 40 },
                    //  isStacked: true,
                    colors: ['#1b9e77', '#d95f02', '#7570b3']
                };



                $.each(response, function (lst, row) {


                    data1.addRow([row.PlantName, row.TotalItems, row.CompletedItems]);
                    // data2.addRow([row.MainCategoryName, row.TotalCategory, row.CompletedCategory]);


                });

                var chart2 = new google.visualization.ComboChart(document.getElementById('chart_div2'));
                chart2.draw(data1, options)


                //var options = {
                //    title: 'Service Category Details',
                //};

                //var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                //chart.draw(data2, options);
            });

            ///categoryyyyyyy
            $http({
                method: 'GET',
                url: '/ServiceSearch/BindTotalItemServiceCategory'
            }).success(function (response) {
                $scope.totallistcategory = response;

                var data2 = new google.visualization.DataTable();
                data2.addColumn('string', 'MainCategoryName');
                data2.addColumn('number', 'TotalCategory');
                data2.addColumn('number', 'CompletedCategory');

                $.each(response, function (lst, row) {


                    // data1.addRow([row.PlantName, row.TotalItems, row.CompletedItems]);
                    data2.addRow([row.MainCategoryName, row.TotalCategory, row.CompletedCategory]);

                    var options = {
                        title: 'Service Category Details',
                    };

                    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                    chart.draw(data2, options);
                });

            });

            //group

            $http({
                method: 'GET',
                url: '/ServiceSearch/BindTotalItemServiceGroup'
            }).success(function (response) {
                $scope.totallistcategorygroup = response;
                // alert(angular.toJson($scope.totallistcategorygroup));
                var data2 = new google.visualization.DataTable();
                data2.addColumn('string', 'MainCategoryName');
                data2.addColumn('string', 'ServiceGroupName');
                data2.addColumn('number', 'TotalService');
                data2.addColumn('number', 'CompletedService');

                $.each(response, function (lst, row) {


                    // data1.addRow([row.PlantName, row.TotalItems, row.CompletedItems]);
                    data2.addRow([row.MainCategoryName, row.ServiceGroupName, row.TotalService, row.CompletedService]);


                });

            });

        }

        //$scope.drawChart1();
    });

})();