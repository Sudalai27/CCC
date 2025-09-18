var Asset = angular.module('ProsolApp', ['datatables', 'cgBusy', 'angular.filter', 'ui.bootstrap']);


Asset.controller('MPController', function ($scope, $http, $timeout, $window, $filter) {
   

    $scope.obj = {};
    $scope.SearchFL = function () {


        $scope.promise = $http({
            method: 'GET',
            url: '/Asset/getfunloc'
        }).success(function (response) {

            if (response != '') {
                $scope.FUNLOC = response;

             
                angular.forEach($scope.FUNLOC, function (value, key) {
                    value.check = 0;
                });


                mymodal_functionlocation.open();


            }
        });

    };

    $scope.Updatemp = function () {

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData();

        formData.append('obj', angular.toJson($scope.obj));

        $http({
            method: 'POST',
            url: '/Asset/updateMP',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData
        }).success(function (response) {

            if (response != false) {
                $('#divNotifiy').attr('style', 'display: Block');              
                $scope.Res = "Maintenance has been updated";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                $scope.obj = {};
                $scope.mcp_list = null;
                $scope.fun_list = null;
                $scope.form.$setPristine();


            }
            else
            {
                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "Error while updating Maintenance plan";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            }
        });

    };

    var mymodal_functionlocation = new jBox('Modal', {
        width: 1000,
        blockScroll: false,
        animation: 'zoomIn',
        overlay: true,
        closeButton: true,
        //height:350,
        content: jQuery('#cotentid5'),

    });

    $scope.identifyAdd_rows = function (fl) {

        angular.forEach($scope.FUNLOC, function (value, key) {

            if (value.FunctLocation != fl)
                value.check = 0;
            else {
                value.check = 1;
                $scope.obj.FunctionLocation = fl;

                $http({
                    method: 'GET',
                    url: '/Asset/getfunc_loc?fl=' + $scope.obj.FunctionLocation
                }).success(function (response) {

                    if (response != '') {
                        $scope.fun_list = response;
                    }
                    else {
                        $scope.fun_list = [];
                        alert("Function location not found for your search");
                    }

                });



            }
        });

    };

    $scope.Close_mymodal_functionlocation = function () {
        mymodal_functionlocation.close();
    };
    $scope.select_mcp = function () {

        $http({
            method: 'GET',
            url: '/Asset/getmcp?eqpmnt=' + $scope.obj.Equipment + '&model=' + $scope.obj.Model + '&make=' + $scope.obj.Make
        }).success(function (response) {

            if (response != '') {
                $scope.mcp_list = response;
            }
            else {
                $scope.mcp_list = response;
                alert("Tasklist not found for your search");
            }

        });
        
    };

});

Asset.controller('Funlocontroller',  function ($scope, $http, $timeout, $window, $filter) {



   
    $scope.db = true;
    $scope.btnSubmit = true;
    $scope.btnUpdate = false;

    $scope.selecteditem = 10;
    $scope.ddlItems = function () {
       
            $scope.loadFunloc();
        
    };
    $scope.loadFunloc = function () {
        $scope.promise = $http({
            method: 'GET',
            url: '/Bom/funlocsearc4',
            params: { currentPage: 1, maxRows: $scope.selecteditem }
        }).success(function (response) {
            //alert(angular.toJson(response))
            if (response.FunlocList != null) {

                $scope.Result = response.FunlocList;
                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;


            } else {
                $scope.Result = null;

            }
        });
    }
    $scope.loadFunloc();
    $scope.bindflInx = function (inx) {


            $scope.promise = $http({

                method: 'GET',
                url: '/Bom/funlocsearc4',
                params: { currentPage: inx, maxRows: $scope.selecteditem }
            }).success(function (response) {
                if (response.FunlocList != null) {

                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.Result = response.FunlocList;

                }
                else {
                    $scope.Result =null;

                }

            })
        
       
    }
  //  $scope.Discipline = [];

    


    $scope.Create = function () {
       
        $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
      
        var formData = new FormData();

        formData.append('Data', angular.toJson($scope.f));
        if ($scope.f != undefined)
            {
            if ($scope.f.drp == "1" || $scope.f.drp == undefined) {

                if ($scope.f.FunctLocation != null && $scope.f.FunctDesc != null && $scope.f.Objecttype != null) {
                    $scope.promise = $http({
                        method: 'POST',
                        url: "/Asset/CreateFun",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,

                    }).success(function (response) {
                        $('#divNotifiy').attr('style', 'display: Block');
                        if (response != false) {
                            $scope.Res = "Item Created Succesfully"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.Clear();

                        } else {

                            $scope.Res = "Item Already Exits"
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;

                        }


                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                }
           
                else if ($scope.f.FunctLocation == null) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    $scope.Res = "FunctLocation is required"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }
                else if ($scope.f.FunctDesc == null) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    $scope.Res = "Description is required"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }
                else if ($scope.f.Objecttype == null) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    $scope.Res = "Object type is required"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }


            }
            else if ($scope.f.FunctLocation != null && $scope.f.SupFunctLoc != null && $scope.f.FunctDesc != null && $scope.f.Objecttype != null) {
                $scope.promise = $http({
                    method: 'POST',
                    url: "/Asset/CreateFun",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,

                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response != false) {
                        $scope.Res = "Item Created Succesfully"
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.Clear();

                    } else {

                        $scope.Res = "Item Already Exits"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
            else if ($scope.f.FunctLocation == null) {
                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "FunctLocation is required"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }
            else if ($scope.f.SupFunctLoc == null) {
                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "SupFunctLoc is required"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }
            else if ($scope.f.FunctDesc == null) {
                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "Description is required"
                $scope.Notify = " -danger";
                $scope.NotifiyRes = true;
            }
            else if ($scope.f.Objecttype == null) {
                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "Object type is required"
                $scope.Notify = " -danger";
                $scope.NotifiyRes = true;
            }
        }
        else {
            $('#divNotifiy').attr('style', 'display: Block');
            $scope.Res = "FunctLocation is required"
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
        }
            }

        
    
    $scope.Clear = function () {

        $window.location.reload();
    }
    //$scope.drpdwn = function (drp) {
      
    //    if(drp == "")
    //    {
    //        $scope.M = false;
           
    //    }
    //    else {
    //        $scope.M = true;
            
    //    }
       
    //}
    $scope.Searchfunloc = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);

        if ($scope.searchfunloc != " " && $scope.searchfunloc != undefined && $scope.searchfunloc != null && $scope.searchfunloc != '') {
            $scope.Result = {};
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Bom/funlocsearch',
                params: { sKey: $scope.searchfunloc, currentPage: 1, maxRows: $scope.selecteditem }
            }).success(function (response) {
                $('#divNotifiy').attr('style', 'display: Block');
                if (response.FunlocList.length > 0) {

                    $scope.Result = response.FunlocList;
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;


                    $scope.Res = "Search results : " + response.FunlocList.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                } else {
                    $scope.Result = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }


            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        else
        {
            $scope.loadFunloc();
        }
    };
    $scope.Searchfun = function (SupFunctLoc) {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.SupFunctLoc = SupFunctLoc;
      
        if ($scope.SupFunctLoc != " " && $scope.SupFunctLoc != undefined && $scope.SupFunctLoc != null && $scope.SupFunctLoc != '') {
           
            $scope.promise = $http({
                method: 'GET',
                url: '/Bom/searchsuperfunloc?sKey=' + $scope.SupFunctLoc
            }).success(function (response) {
                $('#divNotifiy').attr('style', 'display: Block');
                if (response != '') {
                  
                    $scope.SearchResult = response;
                    $scope.Res = "Search results : " + response.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    mymodal.open();
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
    var mymodal = new jBox('Modal', {
        width: 1200,
        blockScroll: false,
        animation: 'zoomIn',

        overlay: true,
        closeButton: true,

        content: jQuery('#cotentid1'),

    });

    $scope.FunClick = function (fun, $index)
    {
       
            
            $scope.f.SupFunctLoc = fun.FunctLocation;
            $scope.dis = true;
            $scope.sb = true;
            $scope.db = false;
            mymodal.close();
        
    }
    $scope.remv = function () {
        $scope.db = true;
        $scope.f.SupFunctLoc = null;
        $scope.dis = false;
        $scope.sb = false;
    }
    $scope.edit = function (fun, idx) {


     //   alert("hai");

        $scope.btnSubmit = false;
        $scope.btnUpdate = true;


            $scope.f = {};

            $scope.f._id = fun._id;
        $scope.f.FunctLocation = fun.FunctLocation;
        $scope.f.FunctLocCat = fun.FunctLocCat;
        $scope.f.FunctDesc = fun.FunctDesc;
        $scope.f.ABCindic = fun.ABCindic;
        $scope.f.SupFunctLoc = fun.SupFunctLoc;
        $scope.f.Objecttype = fun.Objecttype;
        $scope.f.Startdate = fun.Startdate;
        $scope.f.AuthGroup = fun.AuthGroup;
        $scope.f.MainWrk = fun.MainWrk;
        $scope.f.WBSelement = fun.WBSelement;
        $scope.f.Plantsection = fun.Plantsection;
        $scope.f.Catalogprofile = fun.Catalogprofile;
        $scope.f.Singleinst = fun.Singleinst;
        $scope.f.Manufacturer = fun.Manufacturer;
        $scope.f.ManufCon = fun.ManufCon;
        $scope.f.Modelno = fun.Modelno;
        $scope.f.ManufSerialNo = fun.ManufSerialNo;
        $scope.f.FunclocClass1 = fun.FunclocClass1;
        $scope.f.AuthGroup = fun.AuthGroup;
        $scope.f.Comment = fun.Comment;
        $scope.f.Asset = fun.Asset;




    };
    $scope.Clone = function (fun) {
        $scope.f = {};
     //   $scope.f.FunctLocation = fun.FunctLocation;
        $scope.f.FunctLocCat = fun.FunctLocCat;
    //    $scope.f.FunctDesc = fun.FunctDesc;
        $scope.f.ABCindic = fun.ABCindic;
        $scope.f.SupFunctLoc = fun.SupFunctLoc;
        $scope.f.Objecttype = fun.Objecttype;
        $scope.f.Startdate = fun.Startdate;
        $scope.f.AuthGroup = fun.AuthGroup;
        $scope.f.MainWrk = fun.MainWrk;
        $scope.f.WBSelement = fun.WBSelement;
        $scope.f.Plantsection = fun.Plantsection;
        $scope.f.Catalogprofile = fun.Catalogprofile;
        $scope.f.Singleinst = fun.Singleinst;
        $scope.f.Manufacturer = fun.Manufacturer;
        $scope.f.ManufCon = fun.ManufCon;
        $scope.f.Modelno = fun.Modelno;
        $scope.f.ManufSerialNo = fun.ManufSerialNo;
        $scope.f.FunclocClass1 = fun.FunclocClass1;
        $scope.f.AuthGroup = fun.AuthGroup;
        $scope.f.Comment = fun.Comment;

    }
    $scope.update = function () {

      

        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData();

        formData.append('Data', angular.toJson($scope.f));
        formData.append('Update', "yes");
        $scope.promise = $http({
            method: 'POST',
            url: "/Asset/CreateFun",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,

        }).success(function (response) {
            $('#divNotifiy').attr('style', 'display: Block');
            if (response != false) {
                $scope.Res = "Item Upadted Succesfully"
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.Clear();

            } else {

                $scope.Res = "Item Already Exits"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }


        }).error(function (data, status, headers, config) {
            // alert("error");
        });

        // }
    };
    $scope.Del = function (fun) {

      
        if (confirm("Are you sure, delete this record?")) {
            $http({
                method: 'GET',
                url: '/Asset/DelFun?id=' + fun.FunctLocation
            }).success(function (response) {
             if (response == true)
                 $scope.Res = "Item deleted";
             $scope.Notify = "alert-danger";
             $scope.NotifiyRes = true;
                $scope.Clear();
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
    };
});
Asset.controller('Equipcontroller', function ($scope, $http, $timeout, $window, $filter) {
    $scope.btnSubmit = true;
    $scope.btnUpdate = false;
    $scope.db = true;
    $scope.selecteditem = 10;
    $scope.ddlItems = function () {

        $scope.pageLoad();

    };

    $scope.pageLoad = function () {
        $scope.promise = $http({
            method: 'GET',
            url: '/Bom/funlocsearchb',
            params: { currentPage: 1, maxRows: $scope.selecteditem }
        }).success(function (response) {
            if (response.FunlocList.length>0) {

              

                $scope.SearchResult = response.FunlocList;
                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;

            } else {
                $scope.SearchResult = null;

            }
        });
    }
    $scope.pageLoad();

    $scope.bindflInx = function (inx) {


        $scope.promise = $http({

            method: 'GET',
            url: '/Bom/funlocsearchb',
            params: { currentPage: inx, maxRows: $scope.selecteditem }
        }).success(function (response) {
            if (response.FunlocList.length>0) {
                $scope.SearchResult = response.FunlocList;
                $scope.numPerPage = $scope.selecteditem;
                $scope.currentPage = response.CurrentPageIndex;
                $scope.totItem = response.totItem;
               

            }
            else {
                $scope.SearchResult = null;

            }

        })


    }
    $scope.CreateEquip = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData();
      
        formData.append('EquipData', angular.toJson($scope.e));
        $scope.promise = $http({
            method: 'POST',
            url: "/Asset/CreateEquip",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,

        }).success(function (response) {
            $('#divNotifiy').attr('style', 'display: Block');
            if (response != false) {
                $scope.Res = "Item Created Succesfully"
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.Clear();

            } else {

                $scope.Res = "Item Already Exits"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }


        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    }
    $scope.Clear = function () {

        $window.location.reload();
    }
   
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
        else {
            $scope.pageLoad();
        }
    };
    $scope.Searchfun = function (SupFunctLoc) {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.SupFunctLoc = SupFunctLoc;

        if ($scope.SupFunctLoc != " " && $scope.SupFunctLoc != undefined && $scope.SupFunctLoc != null && $scope.SupFunctLoc != '') {

            $scope.promise = $http({
                method: 'GET',
                url: '/Bom/searchsuperfunloc?sKey=' + $scope.SupFunctLoc
            }).success(function (response) {
                $('#divNotifiy').attr('style', 'display: Block');
                if (response != '') {

                    $scope.Result = response;
                    $scope.Res = "Search results : " + response.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    mymodal.open();
                } else {
                    $scope.Result = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }


            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

    };
    var mymodal = new jBox('Modal', {
        width: 1200,
        blockScroll: false,
        animation: 'zoomIn',
        height:500,
        overlay: true,
        closeButton: true,

        content: jQuery('#cotentid1'),

    });

    $scope.FunClick = function (fun, $index) {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);


       if (fun.TechIdentNo != null)
       {
           $scope.ser = "Item Already Linked"
           $scope.Notifys = "alert-danger";
           $scope.Notifiy = true;
           
        }
       else {
          
           $http({
             
               method: 'GET',
               url: '/Asset/GenerateTechIdentNo'
           }).success(function (response) {
               $scope.e.TechIdentNo = response;
               $scope.e.FunctLocation = fun.FunctLocation;
               $scope.dis = true;
               $scope.sb = true;
               $scope.db = false;
               mymodal.close();
           });
       }
      
    }
    $scope.remv = function () {
        $scope.db = true;
        $scope.e.FunctLocation = null;
        $scope.dis = false;
        $scope.sb = false;
    }
    $scope.edit = function (fun, idx) {

        $scope.dis = true;
        $scope.sb = true;
        $scope.db = false;
        //   alert("hai");

        $scope.btnSubmit = false;
        $scope.btnUpdate = true;


        $scope.e = {};

        //$http({
             
        //    method: 'GET',
        //    url: '/Asset/GenerateTechIdentNo'
        //}).success(function (response) {
        //    $scope.e.TechIdentNo = response;
        //})
        $scope.e.TechIdentNo = fun.TechIdentNo;
        $scope.e.FunctLocation = fun.FunctLocation;
       
        $scope.e.EquipDesc = fun.EquipDesc;
        $scope.e.EquipCategory = fun.EquipCategory;
        $scope.e.Weight = fun.Weight;
        $scope.e.UOM = fun.UOM;
        $scope.e.Size = fun.Size;
        $scope.e.AcquisValue = fun.AcquisValue;
        $scope.e.AcquistnDat = fun.AcquistnDat;
        $scope.e.Manufacturer = fun.Manufacturer;
        $scope.e.ManufCon = fun.ManufCon;
        $scope.e.Modelno = fun.Modelno;
        $scope.e.ConstructYear = fun.ConstructYear;
        $scope.e.ConstructMth = fun.ConstructMth;
        $scope.e.ManufPartNo = fun.ManufPartNo;
        $scope.e.ManufSerialNo = fun.ManufSerialNo;
        $scope.e.AuthGroup = fun.AuthGroup;
        $scope.e.Startupdate = fun.Startupdate;
        $scope.e.MaintPlant = fun.MaintPlant;
        $scope.e.Companycode = fun.Companycode;
        $scope.e.Asset = fun.Asset;
        $scope.e.Subno = fun.Subno;
        $scope.e.ConID = fun.ConID;
        $scope.e.Catalogprofile = fun.Catalogprofile;
        $scope.e.Mainworkcenter = fun.Mainworkcenter;
    };
    $scope.update = function () {



        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        var formData = new FormData();

        formData.append('EquipData', angular.toJson($scope.e));
        formData.append('Update', "yes");
        $scope.promise = $http({
            method: 'POST',
            url: "/Asset/CreateEquip",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,

        }).success(function (response) {
            $('#divNotifiy').attr('style', 'display: Block');
            if (response != false) {
                $scope.Res = "Item Upadted Succesfully"
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.Clear();

            } else {

                $scope.Res = "Item Already Exits"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }


        }).error(function (data, status, headers, config) {
            // alert("error");
        });

        // }
    };
    $scope.Del = function (fun) {


        if (confirm("Are you sure, delete this record?")) {
            $http({
                method: 'GET',
                url: '/Asset/DelEquip?id=' + fun.TechIdentNo
            }).success(function (response) {
                if (response == true)
                    $scope.Res = "Item deleted";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.Clear();
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
    };
    $scope.Clone = function (fun) {
        $scope.dis = false;
        $scope.sb = false;
        $scope.db = true;
        $scope.e = {};
        $http({

            method: 'GET',
            url: '/Asset/GenerateTechIdentNo'
        }).success(function (response) {
            $scope.e.TechIdentNo = response;
        })
        $scope.e.EquipDesc = fun.EquipDesc;
        $scope.e.EquipCategory = fun.EquipCategory;
        $scope.e.Weight = fun.Weight;
        $scope.e.UOM = fun.UOM;
        $scope.e.Size = fun.Size;
        $scope.e.AcquisValue = fun.AcquisValue;
        $scope.e.AcquistnDat = fun.AcquistnDat;
        $scope.e.Manufacturer = fun.Manufacturer;
        $scope.e.ManufCon = fun.ManufCon;
        $scope.e.Modelno = fun.Modelno;
        $scope.e.ConstructYear = fun.ConstructYear;
        $scope.e.ConstructMth = fun.ConstructMth;
        $scope.e.ManufPartNo = fun.ManufPartNo;
        $scope.e.ManufSerialNo = fun.ManufSerialNo;
        $scope.e.AuthGroup = fun.AuthGroup;
        $scope.e.Startupdate = fun.Startupdate;
        $scope.e.MaintPlant = fun.MaintPlant;
        $scope.e.Companycode = fun.Companycode;
        $scope.e.Asset = fun.Asset;
        $scope.e.Subno = fun.Subno;
        $scope.e.ConID = fun.ConID;
        $scope.e.Catalogprofile = fun.Catalogprofile;
        $scope.e.Mainworkcenter = fun.Mainworkcenter;
    }
});
Asset.controller('Hierarchycontroller', function ($scope, $http, $timeout, $window, $rootScope, $filter) {

    $scope.promise = $http({

        method: 'GET',
        url: '/Bom/funlocsearch1'

    }).success(function (response) {
        if (response != '') {
            $scope.sResult = response.lst;
            //$scope.Res = "Track results : " + response.length + " items."
            //$scope.Notify = "alert-info";
            //$scope.NotifiyRes = true;
            // $scope.clear = false;
        }
        else {
            $scope.sResult = null;

        }

    })
    $scope.Noitem = true;
    $scope.clear = true;
    $scope.clear1 = true;
    $scope.bexp = true;


    $scope.TrackBom = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        $scope.reset = false;
        $scope.sResult = {};
        $scope.promise = $http({

            method: 'GET',
            url: '/Bom/funlocsearch?sKey=' + $scope.searchkey

        }).success(function (response) {
            if (response != '') {
                $scope.sResult = response;
                $scope.Res = "Track results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.clear = false;
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

    $scope.Open = function (Itemcode) {

      
        $rootScope.$broadcast("myEvent", { Itemcode: Itemcode });
    }

});
Asset.controller('MCPcontroller', function ($scope, $http, $timeout, $window, $rootScope, $filter) {


    $scope.Discipline = [{ 'code': 'M', 'title': 'Mechanical' }, { 'code': 'E', 'title': 'Electrical' }, { 'code': 'I', 'title': 'Instrumentation' }];


    //  alert("HAI");

    $scope.changedrive = function () {
        $scope.obj.Equipment = "";
        $scope.obj.Drivefunction = "";
        $scope.Drive_Main_Eqp_Function = [{ 'Dis': 'M', 'code': '01', 'title': 'Motor driven' }, { 'Dis': 'M', 'code': '02', 'title': 'Diesel driven' }, { 'Dis': 'M', 'code': '03', 'title': 'Gas driven' },
       { 'Dis': 'E', 'code': '01', 'title': 'Generators' }, { 'Dis': 'E', 'code': '02', 'title': 'Transformers' }, { 'Dis': 'E', 'code': '03', 'title': 'Switch yard' },
{ 'Dis': 'E', 'code': '04', 'title': 'Switch Board' }, { 'Dis': 'E', 'code': '05', 'title': 'Isolator' }, { 'Dis': 'E', 'code': '06', 'title': 'Circuit breakers' },
{ 'Dis': 'E', 'code': '07', 'title': 'Over head lines' }, { 'Dis': 'E', 'code': '08', 'title': 'Motors' }, { 'Dis': 'E', 'code': '09', 'title': 'Battery charges' },
{ 'Dis': 'E', 'code': '10', 'title': 'Station facilities including earthing systems' }, { 'Dis': 'E', 'code': '11', 'title': 'Control Panel' }, { 'Dis': 'E', 'code': '12', 'title': 'Beam pumps' },
{ 'Dis': 'E', 'code': '13', 'title': 'ESP (Electrical submersible pump)' }, { 'Dis': 'E', 'code': '14', 'title': 'Screw pumps' }, { 'Dis': 'E', 'code': '16', 'title': 'Capacitors' },
{ 'code': '20', 'title': 'Hazardous area classification' },
{ 'Dis': 'I', 'code': '01', 'title': 'Instrumented Protective Function (IPF)' }, { 'Dis': 'I', 'code': '02', 'title': 'Instrumented  Process Control (IPC)' }];

        $scope.Equipment = [{ 'Dis': 'M', 'code': 'DE', 'title': 'Diesel Engine' }, { 'Dis': 'M', 'code': 'EG', 'title': 'Gas Engine' }, { 'Dis': 'M', 'code': 'GB', 'title': 'Gearbox' },
        { 'Dis': 'M', 'code': 'KS', 'title': 'Screw Compressor' }, { 'Dis': 'M', 'code': 'PA', 'title': 'Diaphragm Pump' }, { 'Dis': 'M', 'code': 'BP', 'title': 'Beam Pump' },
    { 'Dis': 'M', 'code': 'HE', 'title': 'Hazardous Classified Equipment' }, { 'Dis': 'M', 'code': 'KC', 'title': 'Centrifugal Compressor' }, { 'Dis': 'M', 'code': 'KR', 'title': 'Reciprocating Compressor' },
    { 'Dis': 'M', 'code': 'PF', 'title': 'Centrifugal Pump' }, { 'Dis': 'M', 'code': 'PG', 'title': 'Gearwheel Pump' }, { 'Dis': 'M', 'code': 'PR', 'title': 'Rotary Pump' },
    , { 'Dis': 'M', 'code': 'BF', 'title': 'Blower/Fan' }, { 'Dis': 'M', 'code': 'TG', 'title': 'Gas Turbine' }, { 'Dis': 'M', 'code': 'SP', 'title': 'Screw Pump' },

    { 'Dis': 'E', 'code': 'CB', 'title': 'Circuit Breaker' }, { 'Dis': 'E', 'code': 'CA', 'title': 'Hv Capacitor Banks' }, { 'Dis': 'E', 'code': 'BC', 'title': 'Battery Chargers, Batteries & Ups + Batteries' },
    { 'Dis': 'E', 'code': 'IS', 'title': 'Isolator' }, { 'Dis': 'E', 'code': 'GE', 'title': 'Electric Generator' }, { 'Dis': 'E', 'code': 'CC', 'title': 'Control Panel' },
    { 'Dis': 'E', 'code': 'PE', 'title': 'Electric Submersible Pump' }, { 'Dis': 'E', 'code': 'OH', 'title': 'Over Head Lines' }, { 'Dis': 'E', 'code': 'ME', 'title': 'Electric Motor' },
    { 'Dis': 'E', 'code': 'TX', 'title': 'Transformer, Transformer Rectifier' }, { 'Dis': 'E', 'code': 'SB', 'title': 'Switchboard, Distribution Boards ' }, { 'Dis': 'E', 'code': 'RE', 'title': 'Hv Reactors' },
    { 'Dis': 'E', 'code': 'YD', 'title': 'Switchyard' },

    { 'Dis': 'I', 'code': 'BG', 'title': 'Break Glass Function Check' },
    { 'Dis': 'I', 'code': 'DS', 'title': 'Differential Pressure Switch Function Check' },
    { 'Dis': 'I', 'code': 'DT', 'title': 'Differential Pressure Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'AP', 'title': 'Accelerometer Probe Function Check' },
    { 'Dis': 'I', 'code': 'MS', 'title': 'Cooler Vibration Magnetic Switch Function Check' },
    { 'Dis': 'I', 'code': 'PP', 'title': 'Proximity Probe Function Check' },
    { 'Dis': 'I', 'code': 'FE', 'title': 'Final Element (Valve) Function Check' },
    { 'Dis': 'I', 'code': 'FT', 'title': 'Flow Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'ED', 'title': 'Explosion Detector Function Check' },
    { 'Dis': 'I', 'code': 'FD', 'title': 'Flame Detector Function Check' },
    { 'Dis': 'I', 'code': 'GD', 'title': 'Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'HD', 'title': 'Heat Detector Function Check' },
    { 'Dis': 'I', 'code': 'SD', 'title': 'Smoke Detector Function Check' },
    { 'Dis': 'I', 'code': 'SL', 'title': 'Sonic Leak Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'IU', 'title': 'Ir/Uv Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'LS', 'title': 'Level Switch Function Check' },
    { 'Dis': 'I', 'code': 'PB', 'title': 'Push Button Function Check' },
    { 'Dis': 'I', 'code': 'PS', 'title': 'Pressure Switch Function Check' },
    { 'Dis': 'I', 'code': 'PT', 'title': 'Pressure Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'TS', 'title': 'Temperature Switch Function Check' },
    { 'Dis': 'I', 'code': 'TC', 'title': 'Temperature Thermocouple Function Check' },
    { 'Dis': 'I', 'code': 'FS', 'title': 'Vent Flapper Switch Function Check' },
    { 'Dis': 'I', 'code': 'CV', 'title': 'Control Valve Stroke / Lubrication Check Procedure' },
    { 'Dis': 'I', 'code': 'LT', 'title': 'Level Transmitter Calibration Procedure' },
    { 'Dis': 'I', 'code': 'TT', 'title': 'Temperature Transmitter Calibration Procedure' }
        ];

    };
    $scope.fileList = [];

    $scope.SearchFL = function () {

        $scope.promise = $http({
            method: 'GET',
            url: '/Asset/getfunloc'
        }).success(function (response) {

            if (response != '') {
                $scope.FUNLOC = response;

                angular.forEach($scope.fl_Array, function (value1, key) {
           
                    angular.forEach($scope.FUNLOC, function (value, key) {

                        if (value.FunctLocation === value1)
                        {
                            value.check = 1;
                        }                        

                    });
                });   
               // alert(angular.toJson(response));
                mymodal.open();
                //$scope.Res = "Search results : " + response.length + " items."
                //$scope.Notify = "alert-info";
                //$scope.NotifiyRes = true;


            } else {

                //$scope.Res = "No item found"
                //$scope.Notify = "alert-danger";
                //$scope.NotifiyRes = true;

            }

        })

    }

    $scope.LoadFileData = function (files) {
        $scope.fileList = files;

    };

    $scope.identifyAdd_rows = function (index) {


    };


    $scope.fl_Array = [];

    $scope.addRow = function () {
        $scope.fl_Array = [];
        angular.forEach($scope.FUNLOC, function (value, key) {
           
                    if (value.check === 1) {
                        $scope.fl_Array.push(value.FunctLocation);
                    }
                });       

       // alert(angular.toJson($scope.fl_Array));     

        mymodal.close();
    };

    var mymodal = new jBox('Modal', {
        width: 1000,
        blockScroll: false,
        animation: 'zoomIn',
        overlay: true,
        closeButton: true,
        //height:350,
        content: jQuery('#cotentid5'),

    });

    $scope.removerow = function (mcpcode) {
        $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
        $http({
            method: "GET",
            url: "/Asset/remove_mcp?mcpcode=" + mcpcode
        }).success(function (response) {
           
                $rootScope.Res = "mcp deleted";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

                $scope.mcplistt = response;
          
        }).error(function (data, status, headers, config) {
        });
    };
   

    $scope.createmcp = function () {

        if (angular.toJson($scope.fileList[0]) === undefined)
            alert('select file');
        else if ($scope.fl_Array.length === 0)
            alert('select function location');
        else {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("decipline_desc", $("#ddldecipline_desc").find("option:selected").text());
            formData.append("Drivefunction_desc", $("#ddlDrivefunction_desc").find("option:selected").text());
            formData.append("equipment_desc", $("#ddlequipment_desc").find("option:selected").text());
            formData.append("obj", angular.toJson($scope.obj));
            formData.append('files', $scope.fileList[0]);
            formData.append("fl_Array", $scope.fl_Array);

         
            $http({
                method: "POST",
                url: "/Asset/insertdata",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
              //  alert(response);
                if (response === true) {
                    $rootScope.Res = "mcp created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.obj.Equipment = "";
                    $scope.obj.Drivefunction = "";
                    $scope.obj.decipline = "";
                    $scope.fl_Array = [];
                    $('.fileinput').fileinput('clear');
                    $scope.form.$setPristine();
                    $scope.BindList();

                    

                }
                else {
                    $rootScope.Res = "MCP creation failed";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.obj = [];
                    $scope.fl_Array = [];


                }



            }).error(function (data, status, headers, config) {
            });
        }
    };

    $scope.NotifiyResclose = function () {

        $('#divNotifiy').attr('style', 'display: none');
    }

    $scope.BindList = function()
    {
        $http({
            method: "GET",
            url: "/Asset/get_mcp_list"
        }).success(function (response) {
            if (response != null) {  

                $scope.mcplistt = response;
            }
        }).error(function (data, status, headers, config) {
        });

    }

    $scope.BindList();
});

Asset.controller('Tasklistcontroller', function ($scope, $http, $timeout, $window, $filter) {
    $scope.deciplinee = "";
    $scope.mcplistt2 = [];
    $scope.mcpshow2 = "false";
    $scope.tasklist1 = [];
    $scope.Discipline = [{ 'code': 'M', 'title': 'Mechanical' }, { 'code': 'E', 'title': 'Electrical' }, { 'code': 'I', 'title': 'Instrumentation' }];

    $scope.changedrive1 = function () {
        $scope.mcplistt2 = [];
    };
    $scope.changedrive2 = function () {
        $scope.mcplistt2 = [];
    }
    $scope.changedrive = function () {
        $scope.mcplistt2 = [];
        $scope.Equipmentt = "";
        $scope.Drivefunctionn = "";
        $scope.Drive_Main_Eqp_Function = [{ 'Dis': 'M', 'code': '01', 'title': 'Motor driven' }, { 'Dis': 'M', 'code': '02', 'title': 'Diesel driven' }, { 'Dis': 'M', 'code': '03', 'title': 'Gas driven' },
       { 'Dis': 'E', 'code': '01', 'title': 'Generators' }, { 'Dis': 'E', 'code': '02', 'title': 'Transformers' }, { 'Dis': 'E', 'code': '03', 'title': 'Switch yard' },
{ 'Dis': 'E', 'code': '04', 'title': 'Switch Board' }, { 'Dis': 'E', 'code': '05', 'title': 'Isolator' }, { 'Dis': 'E', 'code': '06', 'title': 'Circuit breakers' },
{ 'Dis': 'E', 'code': '07', 'title': 'Over head lines' }, { 'Dis': 'E', 'code': '08', 'title': 'Motors' }, { 'Dis': 'E', 'code': '09', 'title': 'Battery charges' },
{ 'Dis': 'E', 'code': '10', 'title': 'Station facilities including earthing systems' }, { 'Dis': 'E', 'code': '11', 'title': 'Control Panel' }, { 'Dis': 'E', 'code': '12', 'title': 'Beam pumps' },
{ 'Dis': 'E', 'code': '13', 'title': 'ESP (Electrical submersible pump)' }, { 'Dis': 'E', 'code': '14', 'title': 'Screw pumps' }, { 'Dis': 'E', 'code': '16', 'title': 'Capacitors' },
{ 'code': '20', 'title': 'Hazardous area classification' },
{ 'Dis': 'I', 'code': '01', 'title': 'Instrumented Protective Function (IPF)' }, { 'Dis': 'I', 'code': '02', 'title': 'Instrumented  Process Control (IPC)' }];

        $scope.Equipmnt = [{ 'Dis': 'M', 'code': 'DE', 'title': 'Diesel Engine' }, { 'Dis': 'M', 'code': 'EG', 'title': 'Gas Engine' }, { 'Dis': 'M', 'code': 'GB', 'title': 'Gearbox' },
        { 'Dis': 'M', 'code': 'KS', 'title': 'Screw Compressor' }, { 'Dis': 'M', 'code': 'PA', 'title': 'Diaphragm Pump' }, { 'Dis': 'M', 'code': 'BP', 'title': 'Beam Pump' },
    { 'Dis': 'M', 'code': 'HE', 'title': 'Hazardous Classified Equipment' }, { 'Dis': 'M', 'code': 'KC', 'title': 'Centrifugal Compressor' }, { 'Dis': 'M', 'code': 'KR', 'title': 'Reciprocating Compressor' },
    { 'Dis': 'M', 'code': 'PF', 'title': 'Centrifugal Pump' }, { 'Dis': 'M', 'code': 'PG', 'title': 'Gearwheel Pump' }, { 'Dis': 'M', 'code': 'PR', 'title': 'Rotary Pump' },
    , { 'Dis': 'M', 'code': 'BF', 'title': 'Blower/Fan' }, { 'Dis': 'M', 'code': 'TG', 'title': 'Gas Turbine' }, { 'Dis': 'M', 'code': 'SP', 'title': 'Screw Pump' },
    { 'Dis': 'E', 'code': 'CB', 'title': 'Circuit Breaker' }, { 'Dis': 'E', 'code': 'CA', 'title': 'Hv Capacitor Banks' }, { 'Dis': 'E', 'code': 'BC', 'title': 'Battery Chargers, Batteries & Ups + Batteries' },
    { 'Dis': 'E', 'code': 'IS', 'title': 'Isolator' }, { 'Dis': 'E', 'code': 'GE', 'title': 'Electric Generator' }, { 'Dis': 'E', 'code': 'CC', 'title': 'Control Panel' },
    { 'Dis': 'E', 'code': 'PE', 'title': 'Electric Submersible Pump' }, { 'Dis': 'E', 'code': 'OH', 'title': 'Over Head Lines' }, { 'Dis': 'E', 'code': 'ME', 'title': 'Electric Motor' },
    { 'Dis': 'E', 'code': 'TX', 'title': 'Transformer, Transformer Rectifier' }, { 'Dis': 'E', 'code': 'SB', 'title': 'Switchboard, Distribution Boards ' }, { 'Dis': 'E', 'code': 'RE', 'title': 'Hv Reactors' },
    { 'Dis': 'E', 'code': 'YD', 'title': 'Switchyard' },
    { 'Dis': 'I', 'code': 'BG', 'title': 'Break Glass Function Check' },
    { 'Dis': 'I', 'code': 'DS', 'title': 'Differential Pressure Switch Function Check' },
    { 'Dis': 'I', 'code': 'DT', 'title': 'Differential Pressure Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'AP', 'title': 'Accelerometer Probe Function Check' },
    { 'Dis': 'I', 'code': 'MS', 'title': 'Cooler Vibration Magnetic Switch Function Check' },
    { 'Dis': 'I', 'code': 'PP', 'title': 'Proximity Probe Function Check' },
    { 'Dis': 'I', 'code': 'FE', 'title': 'Final Element (Valve) Function Check' },
    { 'Dis': 'I', 'code': 'FT', 'title': 'Flow Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'ED', 'title': 'Explosion Detector Function Check' },
    { 'Dis': 'I', 'code': 'FD', 'title': 'Flame Detector Function Check' },
    { 'Dis': 'I', 'code': 'GD', 'title': 'Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'HD', 'title': 'Heat Detector Function Check' },
    { 'Dis': 'I', 'code': 'SD', 'title': 'Smoke Detector Function Check' },
    { 'Dis': 'I', 'code': 'SL', 'title': 'Sonic Leak Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'IU', 'title': 'Ir/Uv Gas Detector Function Check' },
    { 'Dis': 'I', 'code': 'LS', 'title': 'Level Switch Function Check' },
    { 'Dis': 'I', 'code': 'PB', 'title': 'Push Button Function Check' },
    { 'Dis': 'I', 'code': 'PS', 'title': 'Pressure Switch Function Check' },
    { 'Dis': 'I', 'code': 'PT', 'title': 'Pressure Transmitter Function Check' },
    { 'Dis': 'I', 'code': 'TS', 'title': 'Temperature Switch Function Check' },
    { 'Dis': 'I', 'code': 'TC', 'title': 'Temperature Thermocouple Function Check' },
    { 'Dis': 'I', 'code': 'FS', 'title': 'Vent Flapper Switch Function Check' },
    { 'Dis': 'I', 'code': 'CV', 'title': 'Control Valve Stroke / Lubrication Check Procedure' },
    { 'Dis': 'I', 'code': 'LT', 'title': 'Level Transmitter Calibration Procedure' },
    { 'Dis': 'I', 'code': 'TT', 'title': 'Temperature Transmitter Calibration Procedure' }
        ];
    };

    $scope.selectMcp = function () {        
        if ($scope.deciplinee != null && $scope.deciplinee != "" && $scope.deciplinee != undefined && $scope.Drivefunctionn != null && $scope.Drivefunctionn != "" && $scope.Drivefunctionn != undefined && $scope.Equipmentt != null && $scope.Equipmentt != "" && $scope.Equipmentt != undefined) {
            

            $http({
                method: "GET",
                url: "/TaskList/getmcpwithcondition?discipline=" + $scope.deciplinee + "&drive=" + $scope.Drivefunctionn + "&equipmnt=" + $scope.Equipmentt,
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity               
            }).success(function (response) {
         
                if (response != null) {
                    $scope.mcpshow2 = "true";
                    $scope.mcplistt2 = response;
                }else
                {
                    $scope.mcplistt2 = response;
                    $scope.mcpshow2 = "false";
                }


            }).error(function (data, status, headers, config) {
            });

        }
        else
        {

        }          
    };

    $scope.loadmcptofz = function (mcpcode) {
       // alert(mcpcode);
        $scope.mcp1 = mcpcode;
        mymodaltmcp.close();
    }

    $scope.inserttasklist = function () {
       // alert('hi');
        $scope.tasklist1.push({ "op_seq": $scope.tasklist1.length + 1, "desc1": $scope.desc1, "mcp1": $scope.mcp1, "mwc": $scope.mwc, "nmp": $scope.nmp });
        mymodaltl.close();
      // alert(angular.toJson($scope.tasklist1));
    }
    $scope.removetasklist = function(index)
    {
      //  alert(index);
        $scope.tasklist1.splice(index, 1);
        var i = 1;
        angular.forEach($scope.tasklist1, function (value) {
            value.op_seq = i;
            i = i + 1;

        });
    }

    $scope.CreateTasklist = function () {
        if ($scope.tasklist1.length === 0)
        {
            alert("Missing Operation Sequence");
        }
        else {            
            var formData = new FormData();
            formData.append("Equipment", angular.toJson($scope.Equipment));
            formData.append("Model", angular.toJson($scope.Model));
            formData.append("Make", angular.toJson($scope.Make));
            formData.append("tasklist1", angular.toJson($scope.tasklist1));
            $scope.promise = $http({
                url: "/TaskList/CreateTasklist",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                $scope.Equipment = "";
                $scope.Model = "";
                $scope.Make = "";
                $scope.tasklist1 = [];

                $('#divNotifiy').attr('style', 'display: Block');
                $scope.Res = "Task List submitted succesfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.tasklist_form.$setPristine();
                  //  $scope.$setPristine();
               
            }).error(function (data, status, headers, config) {
               
            });
        }
       
    }


    $scope.NotifiyResclose = function () {

        $('#divNotifiy').attr('style', 'display: none');
    }
    

    $scope.changedrive();
    var mymodaltl = new jBox('Modal', {
        width: 1000,
        blockScroll: false,
        animation: 'zoomIn',
        overlay: true,
        closeButton: true,
        content: jQuery('#cotentidtl'),
    });

    $scope.showAddfortasklist = function () {
        $scope.desc1 = "";
        $scope.mcp1 = "";
        $scope.mwc = "";
        $scope.nmp = "";


        mymodaltl.open();
    };

    var mymodaltmcp = new jBox('Modal', {
        width: 1200,
        height:250,
        blockScroll: false,
        animation: 'zoomIn',
        overlay: true,
        closeButton: true,
        content: jQuery('#mcp'),
    });

    $scope.showAddformcp = function () {
        mymodaltmcp.open();
    };
});

Asset.directive('numberonly', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, ctrl) {
            ctrl.$parsers.push(function (input) {
                if (input == undefined) return ''
                var inputNumber = input.toString().replace(/[^0-9]/g, '');
                if (inputNumber != input) {
                    ctrl.$setViewValue(inputNumber);
                    ctrl.$render();
                }
                return inputNumber;
            });
        }
    };
});