(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap', 'cgBusy']);

    app.controller('Assignworkcontroller', function ($scope, $http, $timeout, $filter) {

        
        $scope.selected = "0"
        $scope.len2 = "0"
        $scope.show3 = false;
        $scope.show2 = false;
        var x;
        var getnew = [];


        $scope.asset = {};
        $scope.BtnFARmodel = '';
        $scope.BtnRegmodel = '';
        $scope.BtnDescmodel = '';

        $scope.BindFAR = function (far) {
            $http({
                method: 'GET',
                url: '/FAR/GetFarMaster'
            }).success(function (response) {

                $scope.FAR_Master = [];
                $scope.FAR_Master = response;
                $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
                console.log(response)
                console.log(far)
                console.log($scope.FAR_Master)
                if (far != null && far != "" && far != undefined) {
                    $scope.BtnFARmodel = far;
                    $scope.asset.FARId = far;
                    console.log($scope.FAR_Master);

                    $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.Region)));
                    $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.AssetDesc)));

                    $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
                    $scope.asset.Region = $scope.RegionMaster.join('');

                    console.log($scope.asset.FARId);
                    console.log($scope.RegionMaster);
                    console.log($scope.AssetDescMaster);
                    $scope.catreloaddataa();
                    $scope.searchFar = "";
                }

            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.BindFAR();
        $scope.BindFARList = function (far) {
            $http({
                method: 'GET',
                url: '/FAR/GetFarMaster'
            }).success(function (response) {

                $scope.FAR_Master = [];
                $scope.FAR_Master = response;
                $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
                console.log(response)
                console.log(far)
                console.log($scope.FAR_Master)
                if (far != null && far != "" && far != undefined) {
                    $scope.BtnFARmodel = far;
                    $scope.asset.FARId = far;
                    console.log($scope.FAR_Master);
                    $scope.filteredassign1 = $filter('filter')($scope.filteredassign1, { 'FixedAssetNo': $scope.BtnFARmodel }, true);
                    $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.Region)));
                    $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.AssetDesc)));

                    $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
                    $scope.asset.Region = $scope.RegionMaster.join('');

                    console.log($scope.asset.FARId);
                    console.log($scope.RegionMaster);
                    console.log($scope.AssetDescMaster);
                    $scope.catreloaddataa();
                    $scope.searchFar = "";
                }

            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        //$scope.BindFARList();

        $scope.catreloaddataa = function () {

            $http({
                method: 'GET',
                url: '/FAR/catreloaddataa',
                params: { farId: $scope.BtnFARmodel}
            }).success(function (response) {
                if (response.length > 0) {
                    $scope.filteredassign1 = response;
                    //if ($scope.BtnFARmodel)
                    //    $scope.filteredassign1 = $filter('filter')($scope.filteredassign1, { 'FixedAssetNo': $scope.BtnFARmodel }, true);
                    $scope.selected2 = "0"
                    if (response.length > 0) {
                        $scope.len2 = response.length;
                    } else {
                        $scope.len2 = "0";
                    }
                    $scope.show2 = true;
                    $scope.show3 = true;
                } else {
                    $scope.filteredassign1 = [];
                    if (!$scope.isUpload) {
                        $scope.show2 = false;
                        $scope.Res = "No data avaliable to load";
                        //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                }
            }).error(function (data, status, headers, config) {

            });



        }
        $scope.catreloaddataa();
        
        $scope.filteredassign = [],
            $scope.currentPage = 1
            , $scope.numPerPage = 10
            , $scope.maxSize = 5;

        $scope.ddlItems1 = function () {
            $scope.chkSelectedpv = false;
            $scope.numPerPage1 = $scope.selecteditempv;
            $scope.slcteditempv = [];
            $scope.selected2 = 0;
        };


        $scope.LoadMaster = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetBusiness'
            }).success(function (response) {


                $scope.getBusiness = response.Businesses;
                $scope.getMajorClass = response.MajorClasses;
                $scope.getRegion = response.Regions;
                $scope.getcity = response.Cities;
                $scope.getarea = response.Areas;
                $scope.getSub = response.SubAreas;
                

            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.LoadMaster();

     


        $scope.loadtabledata1 = function () {
         
            $scope.cgBusyPromises = $http({
                method: 'GET',
                
                url: '/FAR/get_assigndata1',
               
            }).success(function (response) {
                if (response.length > 0) {

                    $scope.show3 = true;
                    $scope.filteredassign1 = response;
                   
                    
                    $scope.selected2 = "0"
                    $scope.len2 = response.length;


                }
               
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        
        //$scope.loadtabledata1();

       

        $scope.catreloaddata1 = function () {
           
            var formData = new FormData()
              $http({
                   method: 'GET',
                  url: '/FAR/catreloaddata1',
                  params: { Business: $scope.BusinessId, Major: $scope.MajorClass, Region: $scope.Regionid, City: $scope.City, Area: $scope.Area, SubArea: $scope.SubArea},

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                if (response.length > 0) {
                    $scope.filteredassign1 = response;
                    $scope.selected2 = "0"
                    if (response.length > 0) {
                        $scope.len2 = response.length;
                    } else {
                        $scope.len2= "0";
                    }
                    $scope.show2 = true;
                    $scope.show3 = true;
                } else {
                    $scope.show2 = false;
                    $scope.Res = "No data avaliable to load";

                    //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });


           
        }

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.$watch('currentPage1 + numPerPage', function () {


            var begin = (($scope.currentPage1 - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
            if ($scope.get_assigndata1 != null)
            $scope.filteredassign1 = $scope.get_assigndata1.slice(begin, end);
            angular.forEach($scope.filteredassign1, function (lst) {
                lst.Duplicates = false;
            });

        });


        $scope.getcatuser1 = function () {
            
            $http.get('/FAR/getcatuser',
                {

                }).success(function (response) {
                    $scope.getuser = response;

                });
        }
        $scope.getcatuser1();

        

        $scope.getcatuser = function () {
           
            $http.get('/FAR/getuserassign',
                {
               
                }).success(function (response) {
                    $scope.assigngetuser = response;

            });
        }
        $scope.getcatuser();

        $scope.slcteditemre = [];
        $scope.reselectAll = function () {

            $scope.slcteditemre = [];
            console.log($scope.filteredassign1)
            if ($scope.rechkSelected) {
                var i = 0;
                angular.forEach($scope.filteredassign1, function (lst) {
                    $('#rechk' + i).prop('checked', true);
                    if ($('#rechk' + i).is(':checked'))
                        $scope.slcteditemre.push(lst.AssetNo);


                    i++;
                });
                $scope.selected1 = $scope.slcteditemre.length;
            }
            else {
                var i = 0;
                angular.forEach($scope.filteredassign1, function (lst) {
                    $('#rechk' + i).prop('checked', false);
                    i++;
                });
                $scope.slcteditemre = [];
                $scope.selected1 = 0;
            }

        }

        //$scope.singlechk = function (UniqueId, indx) {

        //    if ($('#chkpv' + indx).is(':checked')) {

        //        $scope.slcteditem.push(UniqueId);


        //    }
        //    else {
        //        var index = $scope.slcteditem.indexOf(UniqueId);
        //        $scope.slcteditem.splice(index, 1);


        //    }

        //    $scope.selected = $scope.slcteditem.length;

        //};
        $scope.resinglechk = function (UniqueId, indx) {

            if ($('#rechk' + indx).is(':checked')) {

                $scope.slcteditemre.push(UniqueId);

            }
            else {

                var index = $scope.slcteditemre.indexOf(UniqueId);
                $scope.slcteditemre.splice(index, 1);


            }
            $scope.selected1 = $scope.slcteditemre.length;

        };

       

        /*Code search of Reassignworkpage*/
        $scope.reassigncodesearch1 = function () {
           
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData()
            formData.append("UserName", $scope.PVuser);
            formData.append("Business", $scope.Business);
            $scope.cgBusyPromises = $http({
                method: 'POST',
                url: '/FAR/reassignsearch_code1',
                //params: { username: $scope.PVuser, role: $scope.Business },
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                if (response.length > 0) {
                    $scope.get_reassigndata = response;
                    $scope.selected2 = "0"
                    
                    if (response.length > 0) {
                        $scope.len2 = response.length;

                        $scope.loaddata1();
                        $scope.show3 = true;
                    } else {
                        $scope.len2 = "0";
                        $scope.show3 = true;
                    }
                    
                } else {
                    
                    $scope.Res = "No data avaliable to reassign";

                    //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                
            
            }).error(function (data, status, headers, config) {

            });
            

        }

       



        


        $scope.reset = function () {
            $scope.code = "";
            $scope.chkSelectedpv = false;
            $scope.assignUserName = ""
            $scope.slcteditem = [];
            $scope.loaddata1();
            $scope.show1 = false;
            $scope.form1.$setPristine();
        };

        $scope.reset1 = function () {

            $scope.role = "";
            $scope.recheckall = false;
            $scope.reassignUserName = "";
            $scope.slcteditemre = [];
            $scope.UserName = "";
            $scope.get_reassigndata = "";
            $scope.reassignloaddata();
            $scope.show2 = false;
            $scope.formr1.$setPristine();

        };

        $scope.customFilter = function (item) {
            if (!$scope.BtnFARmodel && !$scope.UserName) {
                return true;
            }
            return (item.FixedAssetNo === $scope.BtnFARmodel || item.PVuser.Name === $scope.UserName);
        };
        $scope.userFilter = function (user) {
            $http({
                method: 'GET',
                url: '/FAR/catreloaddataa',
                params: { farId: $scope.BtnFARmodel }
            }).success(function (response) {
                if (response.length > 0) {
                    $scope.filteredassign1 = response;
                } else {
                    $scope.filteredassign1 = [];
                }
            }).error(function (data, status, headers, config) {

            });
            alert('Initial filteredassign1:', angular.toJson($scope.filteredassign1));

            if (user) {
                $scope.filteredassign1 = $scope.filteredassign1.filter(function (item) {
                    return item.PVuser && item.PVuser.Name === user;
                });
                alert('Filtered filteredassign1:', angular.toJson($scope.filteredassign1));
            }
        };


        $scope.clearFar = function () {
            //alert('Clear')
            $scope.catreloaddataa();
            $scope.selectedRowIndex = -1;
            $scope.showlist = false;
            $scope.rechkSelected = false;
            $scope.BtnFARmodel = '';
            $scope.asset.Region = '';
            $scope.asset.AssetDesc = '';
            $scope.UserName = '';
            $scope.LoadData();
        }
        $scope.isUpload = false;
        /*Submit of assignworkpage*/
        $scope.assignto = function () {
            
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.i = $scope.slcteditemre.length;
            if ($scope.slcteditemre.length != 0 && $scope.assignUserNamepv != undefined && $scope.assignUserNamepv != "" && $scope.assignUserNamepv != null) {
                var formData = new FormData();
                formData.append("selecteditem", angular.toJson($scope.slcteditemre));
                formData.append("FirstName", $scope.assignUserNamepv);
                formData.append("PlantCode", $scope.Plant);
                var Plant_ = $("#ddlPlant").find("option:selected").text();
                formData.append("PlantName", Plant_);
                $http({
                    method: 'post',
                    url: '/FAR/catAssignwrk_submit',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    
                    if (response > 0) {
                        $scope.Res = response + " Data has been assigned to " + $scope.assignUserNamepv;
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.chkSelectedpv = false;
                        $scope.rechkSelected = false;
                        //$scope.loadtabledata1();
                        $scope.getcatuser();
                        $scope.getcatuser1();
                        $scope.catreloaddataa();
                        $scope.slcteditemre = [];
                        $scope.assignUserNamepv = null;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.isUpload = true;
                        $scope.catreloaddataa();
                        $scope.show3 = false;
                        if ($scope.filteredassign1.length == 0) {
                            $scope.show2 = false;
                            $scope.BtnFARmodel = "";
                            $scope.asset.Region = "";
                            $scope.asset.AssetDesc = "";
                        }
                        //$scope.form2.$setPristine();

                    }
                    else {
                        $scope.Res = "No data avaliable to assign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                }).error(function (data, status, headers, config) {

                });
            } else {
                
                if ($scope.slcteditemre.length == 0) {
                    $scope.Res = "Select data to Assign"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else if ($scope.assignUserNamepv == null || $scope.assignUserNamepv == "") {
                    $scope.Res = "Select User to Assign"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }
        }
        //$scope.assignto();

       
        //pvdata assign
        $scope.filteredassign1 = [],
    $scope.currentPage1 = 1
, $scope.numPerPage = 10
            , $scope.maxSize = 5;

        
        //$scope.loaddata1 = function () {
        //   alert("load")
        //    $scope.filteredassign1 = [];
        //    if ($scope.PVuser != undefined && $scope.PVuser != "" && $scope.PVuser != null && $scope.PVuser != '' && $scope.Business != undefined && $scope.Business != "" && $scope.Business != null && $scope.Business != '') {

        //        $scope.cgBusyPromises = $http({
        //            method: 'GET',
        //            url: '/FAR/filteredassign1',

        //            params: { username: $scope.PVuser, business: $scope.Business}
        //        }).success(function (response) {
        //            if (response.length > 0) {
                       
        //                $scope.filteredassign1 = response;
        //                $scope.selected2 = "0"
        //                if (response.length > 0) {
        //                    $scope.len2 = response.length;
        //                    $scope.show3 = true;
        //                } else {
        //                    $scope.len2 = "0";
        //                }

        //            } else {
                       
        //            }
        //        }).error(function (data, status, headers, config) {
        //        });
        //    }
        //};
        //$scope.loaddata1();

        $scope.$watch('currentPage + numPerPage', function () {


            var begin = (($scope.currentPage1 - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
            if ($scope.get_assigndata1 != null)
            $scope.filteredassign1 = $scope.get_assigndata1.slice(begin, end);
            angular.forEach($scope.filteredassign1, function (lst) {
                lst.Duplicates1 = false;
            });

        });
       

        $scope.singlechkpv = function (UniqueId, indx) {

            if ($('#rechk' + indx).is(':checked')) {

                $scope.slcteditemre.push(UniqueId);

            }
            else {

                var index = $scope.slcteditemre.indexOf(UniqueId);
                $scope.slcteditemre.splice(index, 1);


            }
            $scope.selected1 = $scope.slcteditemre.length;

        };
        //pvuser

        $scope.bindPlant = function () {
            
            $http.get('/User/getplant').success(function (response) {
                
                $scope.PlantList = response
                //alert(angular.toJson($scope.PlantList))
                $scope.Plant = response[0].Plantcode;
                $http.get('/FAR/getuserassign',
                {
                 params: { plant: response[0].Plantcode }
                 }).success(function (response) {
                    $scope.assigngetuser = response;
                });

            });
        }
        $scope.bindPlant();


        $http({
            method: 'GET',
            url: '/FAR/getuseAsset'
        }).success(function (response) {
            $scope.getuseAsset = response;
            //   $scope.erp.Plant = response[0].Plantcode;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
       
        $http({
            method: 'GET',
            url: '/FAR/getuserassignpv'
        }).success(function (response) {
            $scope.getuserassignpv = response;
            //   $scope.erp.Plant = response[0].Plantcode;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
        
        $http({
            method: 'GET',
            url: '/FAR/getuserassignpv'
        }).success(function (response) {
            $scope.assigngetuserpv = response;
            //   $scope.erp.Plant = response[0].Plantcode;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });


        $scope.BindPlantList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetBusiness'
            }).success(function (response) {

                $scope.getplant1 = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.BindPlantList();

       
        
        $scope.slcteditemre = [];
       
        $scope.selectAllpv = function () {
            
            $scope.removeselectallpv = [];
            $scope.slcteditemre = [];
            if ($scope.chkSelectedpv) {
                // var i = 0;
                angular.forEach($scope.filteredassign1, function (lst) {
                    lst.Duplicates1 = true;
                    $scope.slcteditemre.push(lst.UniqueId);
                    //$('#chk' + i).prop('checked', true);
                    //if ($('#chk' + i).is(':checked'))
                    //    $scope.slcteditem.push(lst.Itemcode);
                    //i++;
                });
                $scope.selected2 = $scope.slcteditemre.length;
            }
            else {
                //var i = 0;
                angular.forEach($scope.filteredassign1, function (lst) {
                    lst.Duplicates1 = false;

                    
                });
                $scope.slcteditemre = [];
                $scope.selected2 = 0;
            }

        }
        //
        
        $scope.searchMasterpv = function (sCode, sSource, sNoun, sModifier) {

            //searchdata

            var formData = new FormData();
            formData.append("UniqueId", $scope.UniqueId);
            formData.append("Plant", $scope.Plant);
            formData.append("PVuser", $scope.PVuser);
            formData.append("Business", $scope.Business);
            //formData.append("Region", $scope.Region);
            //formData.append("Area", $scope.Area);
            //formData.append("SubArea", $scope.SubArea);

            if (($scope.Plant != undefined && $scope.Plant != '') || ($scope.PVuser != undefined && $scope.PVuser != '') || ($scope.Business != undefined && $scope.Business != '')) {

                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Catalogue/searchmasterpv',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    $scope.DataList = response;

                    if (response != null && response.length > 0) {
                        //  alert(angular.toJson($scope.fromsave));
                        if ($scope.fromsave === 1) {
                            $scope.fromsave = 0;
                            $scope.Res = "Data Saved successfully";
                            // alert(angular.toJson($scope.fromsave));
                        }
                        else {
                            $scope.Res = "Search result : " + response.length + " records found";
                        }

                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');


                    } else {
                        $scope.Res = "Records not found";
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    //$(".loaderb_div").hide();
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            } else {

                $scope.loadtabledata1();
            }

        }
     

    });


})();