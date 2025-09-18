var equip = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);



equip.controller('MatBomController',
    function ($scope, $window, $http, $timeout, $filter) {


        $scope.removeshow = true;
        $scope.hide = true;
        $scope.flow = [];
        $scope.add = true;
        $scope.getrq = false;
        $scope.searching1 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.searchkey1 != " " && $scope.searchkey1 != undefined && $scope.searchkey1 != null && $scope.searchkey1 != '') {
                $scope.sResult1 = {};
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/searching1' ,
                    params: { sKey: $scope.searchkey1, currentPage: 1, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response.matList != '') {
                      
                        $scope.Res = "Search results : " + response.matList.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                        $scope.sResult1 = response.matList;
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;

                       
                    } else {
                        $scope.getrq = true;
                        $scope.sResult1 = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {

                });

            }
        };

        $scope.selecteditem = 10;
        $scope.ddlItems = function () {
            if ($scope.searchkey1 == '' || $scope.searchkey1 == null) {
                $scope.pageloaddata();
            } else {
                $scope.searching1();
            }
          

        };

        $scope.pageloaddata = function () {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Bom/frstmat',
                params: { currentPage: 1, maxRows: $scope.selecteditem }
            }).success(function (response) {

                if (response.matList != '') {
                  
                    $scope.sResult1 = response.matList;
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;

                } else {

                    $scope.sResult1 = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            });
        };
        $scope.pageloaddata();
        //var mymodal = new jBox('Modal', {
        $scope.bindmatbomInx = function (inx) {

            if ($scope.searchkey1 == '' || $scope.searchkey1 == null) {
                $scope.promise = $http({

                    method: 'GET',
                    url: '/Bom/frstmat',
                    params: { currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    if (response.matList != null) {

                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.sResult1 = response.matList;

                    }
                    else {
                        $scope.sResult1 = null;

                    }

                })
            } else {
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Bom/searching1',
                    params: { sKey: $scope.searchkey1, currentPage: inx, maxRows: $scope.selecteditem }
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response.matList != '') {

                        $scope.Res = "Search results : " + response.matList.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                        $scope.sResult1 = response.matList;
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;


                    } else {
                        $scope.getrq = true;
                        $scope.sResult1 = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {

                });

            }


        }
        var mymodal1 = new jBox('Modal', {
            width: 1200,
            height:500,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid'),
            reposition: false,
            repositionOnOpen: false

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
            $scope.Itemcode = mat.Itemcode
            $scope.Noun = mat.Noun
            $scope.Modifier = mat.Modifier
            $scope.Manufacturer = mat.Manufacturer
            $scope.Shortdesc = mat.Shortdesc
            $scope.Partno = mat.Partno
            $scope.selected = false;
            $scope.searchkey = null;
            $scope.cgBusyPromises = $http({

                method: 'GET',
                url: '/Bom/srchbom?sKey=' + $scope.Itemcode
            }).success(function (response) {
                if (response != '') {
                    // alert(angular.toJson(response))

                    $scope.flow = response;
                    angular.forEach($scope.flow, function (value, key) {
                        value.remove = 0;
                    });
                    $scope.hide = false;
                    $scope.selected = true;
                    $scope.Create1 = true;
                    $scope.update = false;
                } else {
                    $scope.flow = null;


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
            $scope.Create1 = false;

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
                    angular.forEach($scope.flow, function (value, key) {
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
                if (value.remove === 1) {
                    $scope.nnn = 1;
                    // $scope.removeshow = false;
                }
                //else
                //{
                //    $scope.nnn = 0;
                //   // $scope.removeshow = true;
                //}              

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

            $scope.Itemcode = null;
            $scope.Noun = null;
            $scope.Modifier = null;
            $scope.Manufacturer = null;
            $scope.Shortdesc = null;
            $scope.Partno = null;
          
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
            formvalues.append('Itemcode', angular.toJson($scope.Itemcode));
            formvalues.append('Noun', angular.toJson($scope.Noun));
            formvalues.append('Modifier', angular.toJson($scope.Modifier));
            formvalues.append('Manufacturer', angular.toJson($scope.Manufacturer));
            formvalues.append('Partno', angular.toJson($scope.Partno));
            $scope.cgBusyPromises = $http({
                url: "/Bom/CreateBom",
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


        $scope.Download = function () {
           // alert("hi")
            $window.location = '/Bom/Downloadmasterdata'
        }

    });