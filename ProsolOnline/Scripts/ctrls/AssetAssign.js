var Asset = angular.module('ProsolApp', ['datatables', 'cgBusy', 'angular.filter', 'ui.bootstrap']);



Asset.controller('Funlocontroller',  function ($scope, $http, $timeout, $window, $filter) {



    $scope.usr = $('#usrHid').val();
   
    $scope.db = true;
    $scope.btnSubmit = true;
    $scope.btnUpdate = false;

    $scope.show2 = false;
    $scope.PVslcteditemre = [];
    $scope.show3 = false;
    
    /*$scope.selecteditem = 10;*/

    $scope.filteredassign = [],
        $scope.currentPage = 1
        , $scope.numPerPage = 10
        , $scope.maxSize = 5;
    
    $scope.ddlItems = function () {
        $scope.chkSelected = false;
        $scope.numPerPage = $scope.selecteditem;
        $scope.slcteditem = [];
        $scope.selected = 0;
    };

    $scope.reset1 = function () {
        $scope.UserName = null;
        $scope.role = "";
        $scope.recheckall = false;
        $scope.reassignUserName = "";
        $scope.slcteditemre = [];
        $scope.get_PVreassigndata = [];
        $scope.role = "";
        $scope.UserName = "";
        $scope.get_reassigndata = null;
        $scope.reassignloaddata();
        $scope.show2 = false;
        $scope.show3  = false;
        $scope.formr1.$setPristine();

    };


    $http({
        method: 'GET',
        url: '/FAR/getuserassignpv'
    }).success(function (response) {
        $scope.assigngetuserpv = response;
        //   $scope.erp.Plant = response[0].Plantcode;
    }).error(function (data, status, headers, config) {
        // alert("error");
    });


    
    $http({
        method: 'GET',
        url: '/FAR/getuseAsset'
    }).success(function (response) {
        $scope.getuseAsset = response;
        //   $scope.erp.Plant = response[0].Plantcode;
    }).error(function (data, status, headers, config) {
        // alert("error");
    });



    //$scope.BindPlantList = function () {
       
    //    $http({
    //        method: 'GET',
    //        url: '/FAR/GetBusiness'
    //    }).success(function (response) {
                      
    //        $scope.getplant1 = response;
    //    }).error(function (data, status, headers, config) {
    //         //alert("error");
    //    });
    //};
    //$scope.BindPlantList();
    $scope.BindBusinessList = function () {

        $http({
            method: 'GET',
            url: '/FAR/GetBusiness'
        }).success(function (response) {

          
          //  $scope.getminorcls = response.MinorClasses;
            $scope.getRegion = response.Regions;
            $scope.getcity = response.Cities;
            $scope.getarea = response.Areas;
            $scope.getSub = response.SubAreas;
            $scope.getloc = response.Locations;
            $scope.getidenti = response.Identifiers;
            $scope.getequipclass = response.EquipmentClasses;
            $scope.getequiptype = response.EquipmentTypes;


        }).error(function (data, status, headers, config) {
            //alert("error");
        });
    };
    $scope.BindBusinessList();
    $scope.changeCity = function (City) {

        $http.get('/FAR/getCityList',
            {
                params: { City: City }
            }).success(function (response) {

                $scope.getCitylist = response;

            });
    }
 

    //$scope.BindPlantList1 = function () {
     
    //    $http({
    //        method: 'GET',
    //         url: '/FAR/Getmajorlst'
    //    }).success(function (response) {

    //        $scope.getmajorlist = response;
    //    }).error(function (data, status, headers, config) {
    //        // alert("error");
    //    });
    //};
    //$scope.BindPlantList1();
    //$scope.BindMinorList1 = function () {
        
    //    $http({
    //        method: 'GET',
    //        url: '/FAR/getminorlist'
    //    }).success(function (response) {

    //        $scope.getminorlist = response;
    //    }).error(function (data, status, headers, config) {
    //        // alert("error");
    //    });
    //};
    //$scope.BindMinorList1();

    $scope.BindRegionList1 = function () {
        
        $http({
            method: 'GET',
            url: '/FAR/getRegionlist'
        }).success(function (response) {

            $scope.getRegionlist = response;
        }).error(function (data, status, headers, config) {
             //alert("error");
        });
    };
    $scope.BindRegionList1();
    $scope.BindAreaList1 = function () {

        $http({
            method: 'GET',
            url: '/FAR/getArealist'
        }).success(function (response) {

            $scope.getArealist = response;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    $scope.BindAreaList1();
    $scope.getSubArealist1 = function () {
       
        $http({
            method: 'GET',
            url: '/FAR/getSubArealist1'
        }).success(function (response) {

            $scope.getSubArealist = response;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    $scope.getSubArealist1();

    $scope.getChrlist1 = function () {

        $http({
            method: 'GET',
            url: '/FAR/getChrlist1'
        }).success(function (response) {

            $scope.getChrlist = response;
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    $scope.getChrlist1();


    $scope.reassignloaddata = function () {
        
        if ($scope.Area != undefined && $scope.Area != "" && $scope.Area != null && $scope.Area != '' && $scope.Business != undefined && $scope.Business != "" && $scope.Business != null && $scope.Business != '' && $scope.Region != undefined && $scope.Region != "" && $scope.Region != null && $scope.Region != '') {
           
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/FAR/get_reassigndata',

                params: { business: $scope.Business, Major: $scope.obj.Major, Region: $scope.obj.Region, Area: $scope.obj.Area, SubArea: $scope.obj.SubArea }
            }).success(function (response) {
                if (response.length > 0) {
                    $scope.show2 = true;
                    
                    $scope.get_reassigndata = response;
                    $scope.selected1 = "0"
                    if (response.length > 0) {
                        $scope.len1 = response.length;
                    } else {
                        $scope.len1 = "0";
                    }

                } else {
                    $scope.show2 = false;
                }
            }).error(function (data, status, headers, config) {
            });
        }
    };

   

    
   


    

    $scope.slcteditem = [];
    $scope.slcteditemre = [];

    $scope.selectAll = function () {
        $scope.removeselectall = [];
        $scope.slcteditem = [];
        if ($scope.chkSelected) {
            // var i = 0;
            angular.forEach($scope.get_reassigndata, function (lst) {
                lst.Duplicates = true;
                $scope.slcteditem.push(lst.UniqueId);
                //$('#chk' + i).prop('checked', true);
                //if ($('#chk' + i).is(':checked'))
                //    $scope.slcteditem.push(lst.Itemcode);
                //i++;
            });
            $scope.selected = $scope.slcteditem.length;
        }
        else {
            //var i = 0;
            angular.forEach($scope.get_reassigndata, function (lst) {
                lst.Duplicates = false;

                // $('#chk' + i).prop('checked', false);
                //if ($('#chk' + i).is(':checked')) {
                //    $scope.removeselectall.push(lst.Itemcode);
                //}
                //i++;
            });
            $scope.slcteditem = [];
            $scope.selected = 0;
        }

    }

    $scope.reselectAll = function () {
        
        $scope.slcteditemre = [];
        console.log($scope.get_reassigndata)
        if ($scope.rechkSelected) {
            var i = 0;
            angular.forEach($scope.get_reassigndata, function (lst) {
                $('#rechk' + i).prop('checked', true);
                if ($('#rechk' + i).is(':checked'))
                    $scope.slcteditemre.push(lst.UniqueId);
               

                i++;
            });
            $scope.selected1 = $scope.slcteditemre.length;
        }
        else {
            var i = 0;
            angular.forEach($scope.get_reassigndata, function (lst) {
                $('#rechk' + i).prop('checked', false);
                i++;
            });
            $scope.slcteditemre = [];
            $scope.selected1 = 0;
        }

    }

    $scope.singlechk = function (UniqueId, indx) {
        
        if ($('#chk' + indx).is(':checked')) {

            $scope.slcteditem.push(UniqueId);


        }
        else {
            var index = $scope.slcteditem.indexOf(UniqueId);
            $scope.slcteditem.splice(index, 1);


        }

        $scope.selected = $scope.slcteditem.length;

    };

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

    //submit data
    $scope.isUpload = false;
   
    $scope.assigntoPV = function () {
        
        $scope.kj = $scope.slcteditemre.length;
        
        if ($scope.slcteditemre.length != 0  && $scope.UserName != undefined && $scope.UserName != "") {
            var formData = new FormData();

            console.log($scope.slcteditemre)
            formData.append("selecteditem", angular.toJson($scope.slcteditemre));
           
            formData.append("PVuser", $scope.UserName);
            formData.append("Business", $scope.Business);
           
            $http({
                method: 'post',
                url: '/FAR/reAssignwrk_submit',
                //params: { business: $scope.Business, Major: $scope.obj.Major, Minor: $scope.obj.Minor, Region: $scope.obj.Region, Area: $scope.obj.Area, SubArea: $scope.obj.SubArea },

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                console.log(response)
                if (response > 0) {
                   
                    $scope.Res = response + " Data has been assigned to " + $scope.UserName;
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.rechkSelected = false;
                    $scope.slcteditemre = [];
                    $scope.reassigncodesearch();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.formr1.$setPristine();
                    $scope.show2 = false;
                    $scope.BtnFARmodel = "";
                    $scope.BtnLocationmodel = "";
                    $scope.UserName = "";
                    $scope.assignData = null;
                    $scope.isUpload = true;
                    $scope.reassigncodesearch();
                    if ($scope.get_reassigndata.length == 0) {
                        $scope.show2 = false;
                        $scope.BtnFARmodel = "";
                        $scope.BtnLocationmodel = "";
                        $scope.UserName = "";
                        $scope.assignData = null;
                        $scope.asset.Region = "";
                        $scope.asset.AssetDesc = "";
                    }
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
            //if ($scope.role == undefined || $scope.role == "") {
            //    $scope.Res = "Select role to reassign"
            //    $scope.Notify = "alert-danger";
            //    $scope.NotifiyRes = true;
            //    $('#divNotifiy').attr('style', 'display: block');
            //}
            
            if ($scope.UserName == undefined || $scope.UserName == "") {
                $scope.Res = "Select user to load"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
            else if ($scope.slcteditemre.length == 0) {
                $scope.Res = "Select data to assign"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }
            //else if ($scope.reassignUserName == undefined || $scope.reassignUserName == "") {
            //    $scope.Res = "Select user to reassign"
            //    $scope.Notify = "alert-danger";
            //    $scope.NotifiyRes = true;
            //    $('#divNotifiy').attr('style', 'display: block');
            //}
        }

    }


    $scope.changeBusiness = function (Business) {
       
        $http.get('/FAR/getbusinessMajor',
            {
                params: { Business: Business }
            }).success(function (response) {
                
                $scope.getmajorlist = response;

        });
    }

    $scope.changeMajor = function (Major) {
        
        $http.get('/FAR/getchgMajorList',
            {
                params: { Major: Major }
            }).success(function (response) {
                //alert(angular.toJson($scope.getRegionlist))
                $scope.getRegionlist = response;

            });
    }

    $scope.changeMinor = function (Minor) {
        
        $http.get('/FAR/getchgMinorList',
            {
                params: { Minor: Minor }
            }).success(function (response) {
                
                $scope.getRegionlist = response;

            });
    }

    $scope.changeRegion = function (Region) {

        $http.get('/FAR/getchgRegionList',
            {
                params: { Region: Region }
            }).success(function (response) {

                $scope.getCitylist = response;

            });
    }
    $scope.changeArea = function (Area) {

        $http.get('/FAR/getchgAreaList',
            {
                params: { Area: Area }
            }).success(function (response) {

                $scope.getSubArealist = response;

            });
    }

    //$scope.changeSubArea = function (SubArea) {

    //    $http.get('/FAR/getchgSubAreaList',
    //        {
    //            params: { SubArea: SubArea }
    //        }).success(function (response) {

    //            $scope.getFuctionlist = response;

    //        });
    //}

    $scope.asset = {};
    $scope.BtnFARmodel = '';
    $scope.BtnRegmodel = '';
    $scope.BtnDescmodel = '';
    $scope.BtnLocationmodel = '';

    $scope.reassigncodesearch = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData()

        //formData.append("Business", $scope.Business);

        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/FAR/reassignsearch_code1',
            params: { farId: $scope.BtnFARmodel },

            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,
        }).success(function (response) {
            if (response.length > 0) {
                $scope.get_reassigndata = response;
                if ($scope.BtnFARmodel)
                    $scope.get_reassigndata = $filter('filter')($scope.get_reassigndata, { 'FixedAssetNo': $scope.BtnFARmodel }, true);
                if ($scope.BtnLocationmodel)
                $scope.get_reassigndata = $filter('filter')($scope.get_reassigndata, { 'LocationHierarchy': $scope.BtnLocationmodel }, true);
                $scope.selected1 = "0"
                if (response.length > 0) {
                    $scope.len1 = response.length;
                } else {
                    $scope.len1 = "0";
                }
                $scope.show2 = true;
            } else {
                $scope.get_reassigndata = [];
                if (!$scope.isUpload) {
                    $scope.show2 = false;
                    $scope.Res = "No data avaliable to assign";

                    //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }
        }).error(function (data, status, headers, config) {

        });


        //$http.get('/FAR/getchgSubAreaList',
        //    {
        //        params: { SubArea: SubArea }
        //    }).success(function (response) {

        //        $scope.getFuctionlist = response;

        //    });

    }
    $scope.reassigncodesearch();

    $scope.BindFAR = function (far) {
        $http({
            method: 'GET',
            url: '/FAR/GetFarMaster'
        }).success(function (response) {

            //$scope.reassigncodesearch();
            $scope.FAR_Master = [];
            $scope.FAR_Master = response;
            $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
            //console.log(response)
            //console.log(far)
            //console.log($scope.FAR_Master)
            if (far != null && far != "" && far != undefined) {
                $scope.BtnFARmodel = far;
                console.log($scope.get_reassigndata)
                //$scope.get_reassigndata = $filter('filter')($scope.get_reassigndata, { 'FixedAssetNo': $scope.BtnFARmodel }, true);
                console.log($scope.get_reassigndata)
                $scope.asset.FARId = far;
                console.log($scope.FAR_Master);

                $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.Region)));
                $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.AssetDesc)));

                $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
                $scope.asset.Region = $scope.RegionMaster.join('');

                console.log($scope.asset.FARId);
                console.log($scope.RegionMaster);
                console.log($scope.AssetDescMaster);
                $scope.reassigncodesearch();
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

            $scope.reassigncodesearch();
            $scope.FAR_Master = [];
            $scope.FAR_Master = response;
            $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
            //console.log(response)
            //console.log(far)
            //console.log($scope.FAR_Master)
            if (far != null && far != "" && far != undefined) {
                $scope.BtnFARmodel = far;
                console.log($scope.get_reassigndata)
                $scope.get_reassigndata = $filter('filter')($scope.get_reassigndata, { 'FixedAssetNo': $scope.BtnFARmodel }, true);
                console.log($scope.get_reassigndata)
                $scope.asset.FARId = far;
                console.log($scope.FAR_Master);

                $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.Region)));
                $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.AssetDesc)));

                $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
                $scope.asset.Region = $scope.RegionMaster.join('');

                console.log($scope.asset.FARId);
                console.log($scope.RegionMaster);
                console.log($scope.AssetDescMaster);
                $scope.reassigncodesearch();
                $scope.searchFar = "";
            }

        }).error(function (data, status, headers, config) {
            //alert("error");
        });
    };
    //$scope.BindFARList();

    //Location
    $scope.BindLoc = function (far) {
        $scope.rechkSelected = null;
        $scope.selected1 = null;
        $http({
            method: 'GET',
            url: '/FAR/GetLocMaster'
        }).success(function (response) {
            //$scope.reassigncodesearch();
            $scope.LocMaster = response;

            $scope.LocList = Array.from(new Set(response.map(i => i.Location)));
            $scope.LochList = Array.from(new Set(response.map(i => i.LocationHierarchy)));

            if (far != null && far != "" && far != undefined) {
                $scope.BtnLocationmodel = far;
            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    $scope.BindLoc();
    $scope.BindLocList = function (far) {
        $scope.rechkSelected = null;
        $scope.selected1 = null;
        $http({
            method: 'GET',
            url: '/FAR/GetLocMaster'
        }).success(function (response) {
            $scope.reassigncodesearch();
            $scope.LocMaster = response;

            $scope.LocList = Array.from(new Set(response.map(i => i.Location)));
            $scope.LochList = Array.from(new Set(response.map(i => i.LocationHierarchy)));

            if (far != null && far != "" && far != undefined) {
                $scope.BtnLocationmodel = far;
                console.log($scope.get_reassigndata)
                $scope.get_reassigndata = $filter('filter')($scope.get_reassigndata, { 'LocationHierarchy': $scope.BtnLocationmodel }, true);
                console.log($scope.get_reassigndata)
            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    //$scope.BindLocList();

    //$scope.changeFun = function (Function) {

    //    $http.get('/FAR/getchgFunList',
    //        {
    //            params: { Function: Function }
    //        }).success(function (response) {

    //            $scope.getIdentifier = response;

    //        });
    //    $scope.reassigncodesearch();
    //}

    $scope.clearFar = function () {
        //alert('Clear')
        $scope.selectedRowIndex = -1;
        $scope.showlist = false;
        $scope.BtnFARmodel = '';
        $scope.BtnLocationmodel = '';
        $scope.asset.Region = '';
        $scope.asset.AssetDesc = '';
        $scope.UserName = '';
        $scope.reassigncodesearch();
        //$scope.LoadData();
        $scope.rechkSelected = null;
    }

    
    $scope.reassigncodesearch1 = function (Function) {
        
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData()

        //formData.append("Business", $scope.Business);
       
        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/FAR/reassignsearch_code1',
            params: { business: $scope.Business, Major: $scope.obj.Major, Minor: $scope.obj.Minor, Region: $scope.obj.Region, Area: $scope.obj.Area, SubArea: $scope.obj.SubArea, Function: $scope.obj.Function },

            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,
        }).success(function (response) {
           
            if (response.length > 0) {
                $scope.get_reassigndata = response;
                $scope.selected1 = "0"
                if (response.length > 0) {
                    $scope.len1 = response.length;
                } else {
                    $scope.len1 = "0";
                }
                $scope.show2 = true;
            } else {
                $scope.show2 = false;
                $scope.Res = "No data avaliable to assign";

                //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }).error(function (data, status, headers, config) {

        });


        $http.get('/FAR/getchgFunList',
            {
                params: { Function: Function }
            }).success(function (response) {

                $scope.getIdentifier = response;

            });

    }


    $scope.changeIdentifier = function (Identifier) {

        $http.get('/FAR/getchgIdentifier',
            {
                params: { Identifier: Identifier }
            }).success(function (response) {

                $scope.getEquipmentlist = response;

            });
    }
    $scope.PVreselectAll = function () {

        $scope.PVslcteditemre = [];
        if ($scope.rechkSelected1) {
            var i = 0;
            angular.forEach($scope.get_PVreassigndata, function (lst) {
                $('#rechk' + i).prop('checked', true);
                if ($('#rechk' + i).is(':checked'))
                    $scope.PVslcteditemre.push(lst.UniqueId);


                i++;
            });
            $scope.selected2 = $scope.PVslcteditemre.length;
        }
        else {
            var i = 0;
            angular.forEach($scope.get_PVreassigndata, function (lst) {
                $('#rechk' + i).prop('checked', false);
                i++;
            });
            $scope.PVslcteditemre = [];
            $scope.selected2 = 0;
        }

    }
    $scope.resinglechk1 = function (UniqueId, indx) {

        if ($('#rechk' + indx).is(':checked')) {

            $scope.PVslcteditemre.push(UniqueId);

        }
        else {

            var index = $scope.PVslcteditemre.indexOf(UniqueId);
            $scope.PVslcteditemre.splice(index, 1);


        }
        $scope.selected2 = $scope.PVslcteditemre.length;

    };

    $scope.findcontrol = function () {
        $scope.UserName = "";
        $scope.reassigngetuser = "";
        $scope.get_reassigndata = [];
        if ($scope.role != "" && $scope.role != undefined) {

            $http({
                method: 'GET',
                url: '/FAR/getuser',
                params: { role: $scope.role }

            }).success(function (response) {
                $scope.getuser = response;
                $scope.PVreassigngetuser = response;
            });
        } else {
            $scope.getuser = "";
            $scope.reassigngetuser = "";
            $scope.show2 = false;
        }
    }
    $scope.PV_reassign = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData()

        //formData.append("Business", $scope.Business);

        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/FAR/pv_reassign',
            params: { PVUsername: $scope.UserName, Role: $scope.role },

            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,
        }).success(function (response) {
            if (response.length > 0) {
                $scope.get_PVreassigndata = response;
                $scope.selected1 = "0"
                if (response.length > 0) {
                    $scope.len1 = response.length;
                } else {
                    $scope.len1 = "0";
                }
                $scope.show3 = true;
            } else {
                $scope.show3 = false;
                $scope.Res = "No data avaliable to Re-assign";

                //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }).error(function (data, status, headers, config) {

        });



    }
    $scope.PVreassignto = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.kj = $scope.PVslcteditemre.length;
        if ($scope.reassignUserName == null || $scope.reassignUserName == "") {
            $scope.Res = "Please select User to Re-Assign";
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
            $('#divNotifiy').attr('style', 'display: block');
        }
        if ($scope.PVslcteditemre != undefined && $scope.PVslcteditemre.length != 0 && $scope.UserName != undefined && $scope.UserName != "") {
            var formData = new FormData();
            formData.append("selecteditem", angular.toJson($scope.PVslcteditemre));
            formData.append("FirstName", $scope.reassignUserName);
            formData.append("Username", $scope.UserName);
            formData.append("Role", $scope.role);
            $http({
                method: 'post',
                url: '/FAR/PVreAssignwrk_submit',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData,
            }).success(function (response) {
                if (response) {

                    $scope.Res = $scope.kj + " Data has been Reassigned to " + $scope.reassignUserName;
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.rechkSelected = false;
                    $scope.reassignUserName = null;
                    $scope.PVslcteditemre = [];
                    $scope.PV_reassign();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.formr1.$setPristine();
                    $scope.show2 = false;
                    $scope.get_reassigndata = null;
                    if ($scope.get_PVreassigndata.length == 0) {
                        $scope.show2 = false;
                        $scope.BtnFARmodel = "";
                        $scope.asset.Region = "";
                        $scope.asset.AssetDesc = "";
                    }
                }
                else {
                    $scope.Res = "No data avaliable to reassign"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        } else {
            if ($scope.role == undefined || $scope.role == "") {
                $scope.Res = "Select role to reassign"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }

            else if ($scope.UserName == undefined || $scope.UserName == "") {
                $scope.Res = "Select user to load"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
            else if ($scope.PVslcteditemre.length == 0) {
                $scope.Res = "Select data to reassign"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }
            else if ($scope.reassignUserName == undefined || $scope.reassignUserName == "") {
                $scope.Res = "Select user to reassign"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }

    }

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

                if (response.length > 0) {

                    $scope.Res = $scope.i + " Data has been assigned to " + $scope.assignUserNamepv;
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.chkSelectedpv = false;
                   // $scope.loadtabledata1();
                    $scope.getcatuser();

                    $scope.slcteditemre = [];
                    $scope.assignUserNamepv = null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.show3 = false;
                    $scope.form2.$setPristine();
                    if ($scope.get_reassigndata.length == 0) {
                        $scope.show2 = false;
                        $scope.show3 = false;
                        $scope.BtnFARmodel = "";
                        $scope.asset.Region = "";
                        $scope.asset.AssetDesc = "";
                    }
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

    $scope.getcatuser = function () {
       
        $http.get('/FAR/getuserassign',
            {

            }).success(function (response) {
                $scope.assigngetuser = response;

            });
    }
    $scope.getcatuser();

    $scope.ShowHide = false;

    $scope.LoadFileData = function (files) {

        $scope.NotifiyRes = false;
        $scope.$apply();
        $scope.files = files;
        if (files[0] != null) {
            var ext = files[0].name.match(/\.(.+)$/)[1];
            if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
            } else {
                angular.element("input[type='file']").val(null);
                files[0] = null;

                $scope.Res = "Load valid excel file";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');

                $scope.$apply();



            }
        }
    };
    $scope.AssetCatBulkdata = function () {

        if ($scope.files[0] != null) {

            $scope.ShowHide = true;
            //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

            var formData = new FormData();
            formData.append('image', $scope.files[0]);

            $scope.promise = $http({
                url: "/FAR/AssetCatBulk_Upload",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                $scope.ShowHide = false;
                $scope.Res = data;
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

                $('.fileinput').fileinput('clear');


            }).error(function (data, status, headers, config) {
                $scope.ShowHide = false;
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');



            });
        };
    }

    //$scope.exportTrack = function () {
    //    var formData = new FormData();
    //    formData.append("Username", $scope.UserName);
    //    formData.append("Role", $scope.role);
    //    $window.location = '/FAR/PVreAssignwrk_submit';
    //};
    $scope.exportTrack = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 30000);

        var formData = new FormData();
        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/FAR/pv_reassign',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            params: { PVUsername: $scope.UserName, Role: $scope.role },
        }).success(function (response) {
            if (response.length > 0)
                $window.location = '/FAR/DownloadAssetAssign';
            else {
                $scope.Res = "Please enter valid material codes, if more than one code separate by comma(,)";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        });
    }


    $scope.Bulkdata = function () {

        if ($scope.files[0] != null) {


            $scope.ShowHide = true;
            $timeout(function () { $scope.NotifiyRes = false; }, 10000);

            var formData = new FormData();
            formData.append('image', $scope.files[0]);

            $http({
                url: "/FAR/BulkCat_Upload",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                //  alert(data);
                $scope.ShowHide = false;
                if (!data)
                    $scope.Res = "Records already exists"
                else $scope.Res = data;/*+ " Records uploaded successfully"*/


                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                $('.fileinput').fileinput('clear');

            }).error(function (data, status, headers, config) {
                $scope.ShowHide = false;
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

                $timeout(function () { $scope.NotifiyRes = false; }, 15000);
            });
        };
    }
    $scope.BulkQcdata = function () {

        if ($scope.files[0] != null) {


            $scope.ShowHide = true;
            $timeout(function () { $scope.NotifiyRes = false; }, 10000);

            var formData = new FormData();
            formData.append('image', $scope.files[0]);

            $http({
                url: "/FAR/BulkQc_Upload",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                //  alert(data);
                $scope.ShowHide = false;
                if (!data)
                    $scope.Res = "Records already exists"
                else $scope.Res = data;/*+ " Records uploaded successfully"*/


                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                $timeout(function () { $scope.NotifiyRes = false; }, 15000);
                $('.fileinput').fileinput('clear');

            }).error(function (data, status, headers, config) {
                $scope.ShowHide = false;
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

                $timeout(function () { $scope.NotifiyRes = false; }, 15000);
            });
        };
    }

});


