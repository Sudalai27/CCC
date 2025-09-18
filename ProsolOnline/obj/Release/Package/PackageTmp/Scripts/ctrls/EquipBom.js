var equip = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);

equip.controller('EquipController',
    function ($scope, $window, $http, $timeout, $filter) {

      
     
        $scope.selecteditem = 10;
        $scope.ddlItems = function () {
            if ($scope.searchfunloc == '' || $scope.searchfunloc == null)
                $scope.Loadpage();
            else
                $scope.Searchfunloc()
        };

        $scope.Loadpage = function () {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Bom/funlocsearch1',
                params: { currentPage: 1, maxRows: $scope.selecteditem }
            }).success(function (response) {
                if (response.lst != null) {

                    $scope.SearchResult = response.lst;
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    //   mymodal.open();
                } else {
                    $scope.SearchResult = null;

                }
            });
        }
        $scope.Loadpage();
        $scope.SearchVendorList = function () {
            $scope.BindVendorList();
        }
        $scope.BindVendorList = function () {

            if ($scope.srchText != null && $scope.srchText != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetVendorSearch',
                    params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
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
                    params: { currentPage: 1, maxRows: $scope.selecteditem }
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
        $scope.bindflInx = function (inx) {
            if ($scope.searchfunloc == '' || $scope.searchfunloc == null) {


                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/funlocsearch1',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    if (response.lst != null) {

                        $scope.SearchResult = response.lst;
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        //   mymodal.open();
                    } else {
                        $scope.SearchResult = null;

                    }
                });

            } else {

                $scope.SearchResult = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/funlocsearch',
                    params: { sKey: $scope.searchfunloc, currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response.FunlocList.length > 0) {

                        $scope.SearchResult = response.FunlocList;
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;

                     
                        $scope.Res = "Search results : " + response.FunlocList.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                    } else {
                        $scope.SearchResult = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }
        }
        $scope.removeshow = true;
        $scope.hide = true;
        $scope.flow = [];
        $scope.add = true;
        $scope.Searchfunloc = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
       
            if ($scope.searchfunloc != " " && $scope.searchfunloc != undefined && $scope.searchfunloc != null && $scope.searchfunloc != '') {
                $scope.SearchResult = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/funlocsearch',
                    params: { sKey: $scope.searchfunloc, currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response.FunlocList.length>0) {
                
                        $scope.SearchResult = response.FunlocList;
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;

                      
                        $scope.Res = "Search results : " + response.FunlocList.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                  
                    } else {
                        $scope.SearchResult = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };

        //var mymodal = new jBox('Modal', {
        //    width: 1200,
        //    blockScroll: false,
        //    animation: 'zoomIn',
        //    draggable: false,
        //    overlay: true,
        //    closeButton: true,
        //    //  mouseclick:false,
        //    content: jQuery('#cotentid1'),
        //    reposition: false,
        //    repositionOnOpen: false


        //});
        var mymodal1 = new jBox('Modal', {
            width: 1200,

            animation: 'zoomIn',

            overlay: true,

            closeButton: true,
            content: jQuery('#cotentid'),
            // content: jQuery('#cotentid3'),

        });
        $scope.SearchItem = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.searchkey != " " && $scope.searchkey != undefined && $scope.searchkey != null && $scope.searchkey != '') {
                $scope.sResult = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/searching?sKey=' + $scope.searchkey
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response != '') {
                        $scope.sResult = response;

                        angular.forEach($scope.sResult, function (src, idx) {
                            src.add = 0;
                        });
                        $scope.Res = "Search results : " + response.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        mymodal1.open();

                    } else {
                        $scope.sResult = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {

                });
            }
        };

        $scope.FunClick = function (fun, idx) {
         
           
         
            $scope.FunctLocation = fun.FunctLocation;
            $scope.FunctDesc = fun.FunctDesc;
            $scope.SupFunctLoc = fun.SupFunctLoc;
            $scope.Objecttype = fun.Objecttype;
            $scope.TechIdentNo = fun.TechIdentNo;
            $scope.EquipDesc = fun.EquipDesc;
            $scope.Manufacturer = fun.Manufacturer;;
            $scope.ManufCon = fun.ManufCon;
            $scope.Modelno = fun.Modelno;
            $scope.ManufSerialNo = fun.ManufSerialNo;
            $scope.ABCindic = fun.ABCindic;
            
            $scope.selected = false;
         
            $scope.cgBusyPromises = $http({
            method: 'GET',
            url: '/Bom/srch?sKey=' + $scope.FunctLocation
            }).success(function (response) {
               
                if (response != '') {                   
               
                    $scope.flow = response;

                    angular.forEach($scope.flow, function (value, key) {
                        value.remove = 0;                         
                    });
                   // alert(angular.toJson($scope.flow));

                    $scope.hide = false;
                    $scope.selected = true;
                    $scope.Create = true;
                    $scope.update = false;
                } else {
                    $scope.flow = null;
                   

                }
            });
        };
    
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

          //  alert(angular.toJson($scope.sResult));


        };

        $scope.addRow = function () {
            var cc = 0;
            //  $scope.removeshow = false;
            //   alert(angular.toJson($scope.sResult));
            $scope.searchkey = " ";
            $scope.selected = true;
            $scope.hide = false;
            $scope.add = true;
            $scope.Create = false;

            //angular.forEach($scope.sResult, function (src, idx) {
            // //   var cc = 0;

            //    angular.forEach($scope.flow, function (value, key) {
            //        if (value.Itemcode == src.Itemcode && src.add === 1) {
            //            cc = 1;
            //            //if (cc = 1) {
            //            alert(angular.toJson("Itemcode " +value.Itemcode + "  is alredy added"));
            //            //}
            //        }
            //    });


            //});


            //   alert(angular.toJson($scope.sResult));
            // alert(angular.toJson($scope.flow));

            if ($scope.flow != null) {
                angular.forEach($scope.sResult, function (src, idx) {
                    cc = 0;
              //      alert(angular.toJson($scope.sResult));
                    angular.forEach($scope.flow, function (value, key) {
                //        alert(angular.toJson($scope.flow));
                        if (value.Itemcode === src.Itemcode && src.add === 1) {
                            cc = 1;
                            alert(angular.toJson("Itemcode " + value.Itemcode + " alredy exist"));
                        }
                    });

                    // alert(angular.toJson(src.add));
                    if (cc === 0) {
                        if (src.add === 1) {
                            $scope.flow.push({ 'Itemcode': src.Itemcode, 'Shortdesc': src.Shortdesc, 'Longdesc': src.Longdesc, 'remove': 0 })
                            src.add = 0;
                        }
                    }
                    // cc = 0;

                    //   cc = 0;
                });
            }
            else {
                $scope.flow = [];
                angular.forEach($scope.sResult, function (value, key) {

                    if (value.add === 1) {
                        //  alert(angular.toJson(value.Itemcode));
                        $scope.flow.push({ 'Itemcode': value.Itemcode, 'Shortdesc': value.Shortdesc, 'Longdesc': value.Longdesc, 'remove': 0 })
                    }
                });
            }

            mymodal1.close();
            $scope.removeshow = true;
            $scope.update = true;
            $scope.sResult = null;
        };

        //$scope.AddRow = function (src, idx) {
           
           
            //$scope.removeshow = false;
            //$scope.searchkey = ' ';
            //$scope.selected = true;
            //$scope.hide = false;

        //    var cc = 0;
        //    angular.forEach($scope.flow, function (value, key) {
        //        if (value.Itemcode == src.Itemcode) {
        //            cc = 1;
        //        }
        //    });
        //    if (cc == 0) {

               
        //        $("#data tr").click(function () {
        //            $(this).attr("style", "background-color:lightblue");

        //        });



        //        $scope.fl = $scope.flow;

                
               
        //        $scope.flow.push({ 'Squence': $scope.flow.length + 1, 'Itemcode': src.Itemcode, 'Shortdesc': src.Shortdesc, 'Longdesc': src.Longdesc, 'Remove': 0 })

        //    }

        //};

        //  $scope.flow = [{ 'Squence': 1, 'Remove': 0 }];

      
      // $scope.removeshow = true;

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
              //  $scope.removeshow = false;
            } else {
                $scope.flow[index].remove = 0;
            }

            $scope.nnn = 0;
            angular.forEach($scope.flow, function (value, key) {
                if( value.remove === 1)
                {
                    $scope.nnn = 1;
                   // $scope.removeshow = false;
                }
                //else
                //{
                //    $scope.nnn = 0;
                //   // $scope.removeshow = true;
                //}              

            });
            if ($scope.nnn === 1)
            {
                $scope.removeshow = false;
            }
            else
            {
                $scope.removeshow = true;
            }
          //  alert(angular.toJson($scope.flow[index]));

        };

        $scope.ClearItem = function () {
            location.reload();
            $scope.add = true;
            //$scope.FunctLocation = null;         
            //$scope.searchfunloc = null;
            
        }
        $scope.ClearItem1 = function () {
          
            $scope.flow = [];

            $scope.FunctLocation = null;
            $scope.FunctDesc = '';
            $scope.SupFunctLoc = '';
            $scope.Objecttype = '';
            $scope.TechIdentNo = '';    
            $scope.EquipDesc = '';
            $scope.Manufacturer = '';
            $scope.ManufCon = '';
            $scope.Modelno = '';
            $scope.ManufSerialNo = '';
            $scope.searchkey = " ";
            $scope.searchfunloc = " ";
            $scope.add = true;
            $scope.hide = true;
            $scope.removeshow = true;
          
        }
        //insert

        $scope.Create1 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formvalues = new FormData();
            formvalues.append('request', angular.toJson($scope.flow));
            formvalues.append('FunctLocation', angular.toJson($scope.FunctLocation));
            formvalues.append('FunctDesc', angular.toJson($scope.FunctDesc));
            formvalues.append('Objecttype', angular.toJson($scope.Objecttype));
            formvalues.append('TechIdentNo', angular.toJson($scope.TechIdentNo));
            formvalues.append('SupFunctLoc', angular.toJson($scope.SupFunctLoc));
            formvalues.append('EquipDesc', angular.toJson($scope.EquipDesc));
            formvalues.append('Manufacturer', angular.toJson($scope.Manufacturer));
            formvalues.append('ManufCon', angular.toJson($scope.ManufCon));
            formvalues.append('Modelno', angular.toJson($scope.Modelno));
            formvalues.append('ManufSerialNo', angular.toJson($scope.ManufSerialNo));
            formvalues.append('ABCindic', angular.toJson($scope.ABCindic));
            
            $scope.cgBusyPromises = $http({
                url: "/Bom/Create",
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

                }

            }).error(function () {
                $scope.Res = "Error occured";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;

            });
        };

    });