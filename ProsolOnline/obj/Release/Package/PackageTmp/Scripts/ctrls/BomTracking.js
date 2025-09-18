
var app = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);
app.controller('BomTrackingcontroller', function ($scope, $http, $rootScope, $timeout, $window, $location) {
      
        //$scope.promise = $http({

        //    method: 'GET',
        //    url: '/Bom/funlocsearch1'

        //}).success(function (response) {
           
        //    if (response != '') {

        //        $scope.sResult = response.lst;
        //        $scope.trackebom = response.eqpbom;
        //        $scope.TechIdentNo = $scope.trackebom[0].TechIdentNo;
        //        $scope.EquipDesc = $scope.trackebom[0].EquipDesc;
        //        $scope.FunctLocation = $scope.trackebom[0].FunctLocation;
        //        $scope.FunctDesc = $scope.trackebom[0].FunctDesc;
        //        $scope.Objecttype = $scope.trackebom[0].Objecttype;
        //        $scope.Manufacturer = $scope.trackebom[0].Manufacturer;
        //        $scope.sfunloc = $scope.trackebom[0].SupFunctLoc;
        //        if ($scope.trackebom != null)
        //        {
        //            $scope.show = true;
        //            $scope.Noitem = false;
        //            $scope.show = false;
        //            $scope.clear = false;
        //        }
        //        $scope.trackmbom = response.matbom;
        //        $scope.trackmmbom = response.mmatbom;
              
        //    }
        //    else {
        //        $scope.sResult = null;

        //    }

        //})

        


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
            $scope.bindAssets();
        }

        $scope.TBMClick = function (TBM, idx) {
          
           
            $scope.NotifiyRes = false;

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

        $scope.Export = function () {
            //   alert(angular.toJson($scope.Result))
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
        $scope.bindAssets();
      
        $scope.bindAssetsInx = function (inx) {

            if ($scope.searchkey == ''|| $scope.searchkey == null) {
                $scope.promise = $http({

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
               
                $scope.promise = $http({

                    method: 'GET',
                    url: '/Bom/funlocsearch',
                    params: { sKey: $scope.searchkey, currentPage: inx, maxRows: $scope.selecteditem }

                }).success(function (response) {
                    if (response.FunlocList != '') {
                        $scope.sResult = response.FunlocList;
                        $scope.Res = "Track results : " + response.FunlocList.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.clear = false;

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
            $scope.clrhiarchy();
        }

        $scope.TrackBom = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.reset = false;
            $scope.sResult = {};
        
            $scope.promise = $http({

                method: 'GET',
                url: '/Bom/funlocsearch',
                params: {sKey: $scope.searchkey, currentPage: 1, maxRows: $scope.selecteditem }

            }).success(function (response) {
                if (response.FunlocList != '') {
                    $scope.sResult = response.FunlocList;
                    $scope.Res = "Track results : " + response.FunlocList.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.clear = false;

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
            $scope.reset = true;
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
            $scope.NotifiyRes = false;

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

        $scope.Open1 = function (Itemcode)
        {
            $window.location = '/Bom/Bomtracking?id=' + Itemcode;
          
                //$scope.promise = $http({

                //    method: 'GET',
                //    url: '/Bom/getitemb?sKey=' + Itemcode

                //}).success(function (response) {
                //    if (response != 'true') {   

                //        $window.location = '/Bom/Bomtracking?id=' + Itemcode;
                //        $scope.Result = response;
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
                //        $scope.Notify = "alert-danger";
                //        $scope.NotifiyRes = true;
                //    }

                //})
            
        
           
        }

    });
 
