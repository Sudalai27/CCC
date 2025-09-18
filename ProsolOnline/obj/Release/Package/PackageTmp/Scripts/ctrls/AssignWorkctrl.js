(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['ui.bootstrap','cgBusy']);   

    app.controller('Assignworkcontroller', function ($scope, $http, $timeout) {

        $scope.show1 = false;
        $scope.show2 = false;
        $scope.selected="0"
        $scope.len = "0"
        $scope.ShowHide = false;

        var x;
        var getnew = [];


        $scope.bindPlant = function () {
            $http.get('/User/getplant').success(function (response) {
                $scope.PlantList = response
                $scope.Plant = response[0].Plantcode;              
                $http.get('/File/getuserassign', { params: { plant: response[0].Plantcode } }).success(function (response) {
                    $scope.assigngetuser = response;                   
                });

            });
        }
        $scope.bindPlant();
        $scope.changePlant = function () {
          
            $http.get('/File/getuserassign', { params: { plant: $scope.Plant } }).success(function (response) {
                $scope.assigngetuser = response;
              
            });
        }
        //$http.get('/File/getuserassign').success(function (response) {
        //    $scope.assigngetuser = response;
        //    $scope.selected = "0"
        //    if ($scope.assigngetuser > 0) {
        //        $scope.len = $scope.assigngetuser.length;
        //    }else
        //    {
        //        $scope.len = "0";
        //    }
        //});

        $scope.filteredassign = [],
    $scope.currentPage = 1
, $scope.numPerPage = 10
, $scope.maxSize = 5;

$scope.ddlItems=function(){
    $scope.chkSelected = false;
    $scope.numPerPage = $scope.selecteditem;
    $scope.slcteditem = [];
    $scope.selected =0;
};

$scope.NotifiyResclose = function () {
    $('#divNotifiy').attr('style', 'display: none');
}


        /*Load data of assignworkpage*/
        $scope.loaddata = function () {
            $scope.get_assigndata = [];
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/File/get_assigndata'
            }).success(function (response) {
                if (response.length > 0) {

                    $scope.show1 = true;
                    $scope.get_assigndata = response;

                    $scope.selected = "0"
                    $scope.len = response.length;

                    $scope.$watch('currentPage + numPerPage', function () {

                        var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;                      
                        $scope.filteredassign = $scope.get_assigndata.slice(begin, end);
                        angular.forEach($scope.filteredassign, function (lst) {
                            var str = $scope.slcteditem.indexOf(lst.Itemcode);
                            if (str != -1) {
                                lst.Duplicates = true;                            
                            } 
                        });               
                    });
                   
                }
                //else {
                //    $scope.Res = "No data avaliable to assign"
                //    $scope.Notify = "alert-danger";
                //    $scope.NotifiyRes = true;
                //}
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.loaddata();
        $scope.$watch('currentPage + numPerPage', function () {

           
            var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
            $scope.filteredassign = $scope.get_assigndata.slice(begin, end);          
            angular.forEach($scope.filteredassign, function (lst) {             
                lst.Duplicates = false;
            });

        });



        /*Load data of Reassignworkpage*/
        $scope.reassignloaddata = function () {
            if ($scope.UserName != undefined && $scope.UserName != "" && $scope.UserName != null && $scope.UserName != '' && $scope.role != undefined && $scope.role != "" && $scope.role != null && $scope.role != '') {
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/File/get_reassigndata',
                    //?role=' + $scope.role + '&username=' + $scope.UserName
                    params :{role: $scope.role,username: $scope.UserName}
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
   
        $scope.reassignloaddata();
        $scope.slcteditem = [];
        $scope.slcteditemre = [];
        $scope.selectAll = function () {
            $scope.removeselectall = [];
            $scope.slcteditem = [];
            if ($scope.chkSelected) {
               // var i = 0;
                angular.forEach($scope.filteredassign, function (lst) {
                    lst.Duplicates = true;
                    $scope.slcteditem.push(lst.Itemcode);
                    //$('#chk' + i).prop('checked', true);
                    //if ($('#chk' + i).is(':checked'))
                    //    $scope.slcteditem.push(lst.Itemcode);
                    //i++;
                });
                $scope.selected = $scope.slcteditem.length;
            }
            else {                             
                //var i = 0;
                angular.forEach($scope.filteredassign, function (lst) {
                    lst.Duplicates = false;
                   
                   // $('#chk' + i).prop('checked', false);
                    //if ($('#chk' + i).is(':checked')) {
                    //    $scope.removeselectall.push(lst.Itemcode);
                    //}
                    //i++;
                });
                $scope.slcteditem=[];
                $scope.selected =0;
            }
            
        }

        $scope.reselectAll = function () {
            $scope.slcteditemre = [];
            if ($scope.rechkSelected) {
                var i = 0;
                angular.forEach($scope.get_reassigndata, function (lst) {
                    $('#rechk' + i).prop('checked', true);
                    if ($('#rechk' + i).is(':checked'))
                        $scope.slcteditemre.push(lst.Itemcode);
                   
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

        $scope.singlechk = function (Itemcode, indx) {
         
            if ($('#chk' + indx).is(':checked')) {
               
                $scope.slcteditem.push(Itemcode);
              
                
            }
            else {
                var index = $scope.slcteditem.indexOf(Itemcode);
                $scope.slcteditem.splice(index, 1);
              
                   
            }
          
            $scope.selected = $scope.slcteditem.length;
           
        };

        $scope.resinglechk = function (Itemcode, indx) {
            if ($('#rechk' + indx).is(':checked')) {
                $scope.slcteditemre.push(Itemcode);
              
            }
            else {
                var index = $scope.slcteditemre.indexOf(Itemcode);
                $scope.slcteditemre.splice(index, 1);
               
               
            }
            $scope.selected1 = $scope.slcteditemre.length;
        };

        /*Code search of assignworkpage*/
        $scope.assigncodesearch = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.code == null) {
                $('#divNotifiy').attr('style', 'display: block');
                    $scope.Res = "Enter code to search"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    
                }else
                {
                    var formData = new FormData()
                    formData.append("code", $scope.code);
                    $scope.cgBusyPromises = $http({
                        method: 'POST',
                        url: '/File/search_code',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {
                        $scope.len = response.length;
                        $('#divNotifiy').attr('style', 'display: block');
                        if (response == null || response == "" ) {
                            $scope.Res = "No Item Found"
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                        }
                        else {
                            $scope.show1 = true;
                            $scope.get_assigndata = response;

                            $scope.$watch('currentPage + numPerPage', function () {
                                var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

                                $scope.filteredassign = $scope.get_assigndata.slice(begin, end);

                                angular.forEach($scope.filteredassign, function (lst) {
                                    var str = $scope.slcteditem.indexOf(lst.Itemcode);
                                    if (str != -1) {
                                        lst.Duplicates = true;
                                    }
                                });
                            });
                            $scope.form1.$setPristine();
                            $scope.Res = response.length + " Item Found"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                        }

                    }).error(function (data, status, headers, config) {
                        $scope.Res = data.errors;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    });
                }
            }

        /*Code search of Reassignworkpage*/
        $scope.reassigncodesearch = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
                    var formData = new FormData()
                    formData.append("UserName", $scope.UserName);
                    formData.append("Role", $scope.role);
                    $scope.cgBusyPromises = $http({
                        method: 'POST',
                        url: '/File/reassignsearch_code',
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
                            $scope.Res = "No data avaliable to reassign";
                           
                          //  $timeout(function () { $scope.NotifiyRes6 = false; }, 300000);
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');
                        }
                    }).error(function (data, status, headers, config) {
                      
                    });
                
            }

            //$scope.download = function (get_assigndata) {
            //    $scope.code = ""
            //    $scope.trackIds = [];
            //    angular.forEach($scope.get_assigndata, function (x) {
            //        $scope.trackIds.push(x.Itemcode)
            //    });
            //    window.location.href="/File/Download?get_assigndata=" + $scope.trackIds;
            //};

            $scope.reset = function () {
                $scope.code = "";
                $scope.chkSelected = false;
                $scope.assignUserName=""
                $scope.slcteditem = [];
                $scope.loaddata();
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


        /*Get userlist based on role*/
            $scope.findcontrol = function () {
                $scope.UserName = "";
                $scope.reassigngetuser = "";
                $scope.get_reassigndata = "";
                if ($scope.role != "" && $scope.role != undefined) {
                    $http({
                        method: 'GET',
                        url: '/File/getuser',
                        params: { role: $scope.role, plant: $scope.Plant }
                        ///?role=' + $scope.role + '&plant=' + $scope.Plant
                       // data: $scope.role
                    }).success(function (response) {
                        $scope.getuser = response;
                        $scope.reassigngetuser = response;
                    });
                } else {
                    $scope.getuser = "";
                    $scope.reassigngetuser = "";
                    $scope.show2 = false;
                }
            }          

           
         
        /*Submit of assignworkpage*/
            $scope.assignto = function () {
               
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 5000);
                $scope.i = $scope.slcteditem.length;
                if ($scope.slcteditem.length != 0 && $scope.assignUserName != undefined && $scope.assignUserName != "" && $scope.assignUserName != null) {
                    var formData = new FormData();
                    formData.append("selecteditem", angular.toJson($scope.slcteditem));
                    formData.append("FirstName", $scope.assignUserName);
                    formData.append("PlantCode", $scope.Plant);
                    var Plant_ = $("#ddlPlant").find("option:selected").text();
                    formData.append("PlantName", Plant_);
                    $http({
                        method: 'post',
                        url: '/File/Assignwrk_submit',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {
                        if (response.length > 0) {
                            $scope.Res = $scope.i + " Data has been assigned to " + $scope.assignUserName;
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.chkSelected = false;
                            $scope.loaddata();
                            $scope.code = "";
                            $scope.slcteditem = [];
                            $scope.assignUserName = null;
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.show1 = false;
                            $scope.form2.$setPristine();
                        }
                        else {
                            $scope.Res = "No data avaliable to assign"
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');
                        }
                    }).error(function (data, status, headers, config) {
                        
                    });
                }else
                {
                    if($scope.slcteditem.length == 0) {
                        $scope.Res = "Select data to Assign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if($scope.assignUserName == null || $scope.assignUserName == "")
                    {
                        $scope.Res = "Select User to Assign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                }
            }


        /*Submit of reassignworkpage*/
            $scope.reassignto = function () {
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 5000);
                $scope.kj = $scope.slcteditemre.length;
                if ($scope.slcteditemre != undefined && $scope.slcteditemre.length != 0 && $scope.reassignUserName != undefined && $scope.UserName != undefined && $scope.role != undefined && $scope.reassignUserName != "" && $scope.UserName != "" && $scope.role != "") {
                    var formData = new FormData();
                    formData.append("selecteditem", angular.toJson($scope.slcteditemre));
                    formData.append("FirstName", $scope.reassignUserName);
                    formData.append("Username", $scope.UserName);
                    formData.append("Role", $scope.role);
                    $http({
                        method: 'post',
                        url: '/File/reAssignwrk_submit',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {
                        if (response.length > 0) {
                         
                            $scope.Res = $scope.kj + " Data has been Reassigned to " + $scope.reassignUserName;
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.rechkSelected = false;
                            $scope.reassignUserName = null;
                            $scope.slcteditemre = [];
                            $scope.reassignloaddata();
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.formr1.$setPristine();
                            $scope.show2 = false;
                            $scope.get_reassigndata = null;
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
                    if ($scope.role == undefined || $scope.role=="")
                    {
                        $scope.Res = "Select role to reassign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }

                    else if ($scope.UserName == undefined || $scope.UserName == "")
                    {
                        $scope.Res = "Select user to load"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if ($scope.slcteditemre.length == 0)
                    {
                        $scope.Res = "Select data to reassign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    }
                    else if ($scope.reassignUserName == undefined || $scope.reassignUserName == "")
                    {
                        $scope.Res = "Select user to reassign"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                }

            }
        //pvdata assign
            $scope.filteredassign1 = [],
        $scope.currentPage1 = 1
, $scope.numPerPage = 10
, $scope.maxSize = 5;
            $scope.BindPlant1 = function () {
                //  $scope.erp = {};
                $http({
                    method: 'GET',
                    url: '/Catalogue/getplantCode_Name1'
                }).success(function (response) {
                    $scope.PlantListpv = response;
                    //   $scope.erp.Plant = response[0].Plantcode;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
            $scope.BindPlant1();

        //location
            $http({
                method: 'GET',
                url: '/Master/GetDataList?label=Storage location'
            }).success(function (response) {
                $scope.strloc = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        //noun
            $scope.NMLoad = function () {
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Catalogue/GetDBNounList'
                }).success(function (response) {
                    $scope.NounList = response;
                    $scope.sNoun = "";

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
            $scope.NMLoad();
            $scope.searchMasterpv = function (sCode, sSource, sNoun, sModifier) {

                //bin
                $http.get('/Master/getbincode?label= storage bin' + '&storagelocation=' + $scope.StorageLocation
                ).success(function (response) {
                    $scope.Masterlist1 = response
                    //  alert(angular.toJson($scope.Masterlist1));
                }).error(function (data, status, headers, config) {
                });

                //searchdata

                var formData = new FormData();
                formData.append("sCode", $scope.sCode);
                formData.append("sPlant", $scope.sPlant);
                formData.append("StorageLocation", $scope.StorageLocation);
                formData.append("storagebin", $scope.storagebin);
                formData.append("sNoun", $scope.sNoun);
                formData.append("sModifier", $scope.sModifier);

                if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.sPlant != undefined && $scope.sPlant != '') || ($scope.StorageLocation != undefined && $scope.StorageLocation != '') || ($scope.storagebin != undefined && $scope.storagebin != '') || ($scope.sNoun != undefined && $scope.sNoun != '') || ($scope.sModifier != undefined && $scope.sModifier != '')) {

                    $scope.cgBusyPromises = $http({
                        method: 'POST',
                        url: '/Catalogue/searchmasterpv',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {

                        $scope.DataList = response;

                        //angular.forEach($scope.DataList, function (lst) {
                        //    lst.bu = '0';

                        //});

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

                    $scope.loaddata1();
                }

            }




            $scope.ddlItems1 = function () {
                $scope.chkSelectedpv = false;
                $scope.numPerPage1 = $scope.selecteditempv;
                $scope.slcteditempv = [];
                $scope.selected2 = 0;
            };

            $scope.loaddata1 = function () {
                $scope.get_assigndata1 = [];
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/File/get_assigndata1'
                }).success(function (response) {
                    if (response.length > 0) {

                        $scope.show3 = true;
                        $scope.get_assigndata1 = response;

                        $scope.selected2 = "0"
                        $scope.len2 = response.length;

                        $scope.$watch('currentPage + numPerPage', function () {

                            var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
                            $scope.filteredassign1 = $scope.get_assigndata1.slice(begin, end);
                            angular.forEach($scope.filteredassign1, function (lst) {
                                var str = $scope.slcteditempv.indexOf(lst.Itemcode);
                                if (str != -1) {
                                    lst.Duplicates = true;
                                }
                            });
                        });

                    }
                    //else {
                    //    $scope.Res = "No data avaliable to assign"
                    //    $scope.Notify = "alert-danger";
                    //    $scope.NotifiyRes = true;
                    //}
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
            $scope.$watch('currentPage + numPerPage', function () {


                var begin = (($scope.currentPage1 - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;
                $scope.filteredassign1 = $scope.get_assigndata1.slice(begin, end);
                angular.forEach($scope.filteredassign1, function (lst) {
                    lst.Duplicates1 = false;
                });

            });
            $scope.loaddata1();

            $scope.slcteditempv = [];
            $scope.selectAllpv = function () {
                $scope.removeselectallpv = [];
                $scope.slcteditempv = [];
                if ($scope.chkSelectedpv) {
                    // var i = 0;
                    angular.forEach($scope.filteredassign1, function (lst) {
                        lst.Duplicates1 = true;
                        $scope.slcteditempv.push(lst.Itemcode);
                        //$('#chk' + i).prop('checked', true);
                        //if ($('#chk' + i).is(':checked'))
                        //    $scope.slcteditem.push(lst.Itemcode);
                        //i++;
                    });
                    $scope.selected2 = $scope.slcteditempv.length;
                }
                else {
                    //var i = 0;
                    angular.forEach($scope.filteredassign1, function (lst) {
                        lst.Duplicates1 = false;

                        // $('#chk' + i).prop('checked', false);
                        //if ($('#chk' + i).is(':checked')) {
                        //    $scope.removeselectall.push(lst.Itemcode);
                        //}
                        //i++;
                    });
                    $scope.slcteditempv = [];
                    $scope.selected2 = 0;
                }

            }
        //
            $scope.singlechkpv = function (Itemcode, indx) {

                if ($('#chkpv' + indx).is(':checked')) {

                    $scope.slcteditempv.push(Itemcode);


                }
                else {
                    var index = $scope.slcteditempv.indexOf(Itemcode);
                    $scope.slcteditempv.splice(index, 1);


                }

                $scope.selected2 = $scope.slcteditempv.length;

            };
        //pvuser
            $scope.pvplant = function () {
                //  $scope.erp = {};
                $http({
                    method: 'GET',
                    url: '/Catalogue/getplantCode_Name1'
                }).success(function (response) {
                    $scope.PlantListpvuser = response;
                    //   $scope.erp.Plant = response[0].Plantcode;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
            $scope.pvplant();

        //   $scope.getpvuser = function () {
            $http({
                method: 'GET',
                url: '/File/getuserassignpv'
            }).success(function (response) {
                $scope.assigngetuserpv = response;
                //   $scope.erp.Plant = response[0].Plantcode;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        //$http.get('/File/getuserassignpv?plant=' + $scope.Plantpv
        //    ).success(function (response) {
        //    $scope.assigngetuserpv = response;

        // });
        //   }

        /////////////

            $scope.assigntopv = function () {

                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 5000);
                $scope.iii = $scope.slcteditempv.length;
                if ($scope.slcteditempv.length != 0 && $scope.assignUserNamepv != undefined && $scope.assignUserNamepv != "" && $scope.assignUserNamepv != null) {
                    var formData = new FormData();
                    formData.append("selecteditem", angular.toJson($scope.slcteditempv));
                    formData.append("FirstName", $scope.assignUserNamepv);
                    formData.append("PlantCode", $scope.Plantpv);
                    formData.append("User", $scope.assignUserNamepv);
                    var Plant_ = $("#ddlPlant11").find("option:selected").text();

                    formData.append("PlantName", Plant_);
                    $http({
                        method: 'post',
                        //url: '/File/Assignwrk_submitforpv',
                        url: '/File/Assignwrk_submitforpv?Role=PV User',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData,
                    }).success(function (response) {
                        if (response.length > 0) {
                            $scope.Res = $scope.iii + " Data has been assigned to " + $scope.assignUserNamepv;
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.chkSelectedpv = false;
                            $scope.loaddata1();
                            $scope.code = "";
                            $scope.slcteditempv = [];
                            $scope.assignUserNamepv = null;
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.form2.$setPristine();
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
                    if ($scope.slcteditempv.length == 0) {
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

        //BulkPV


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
                    $scope.NotifiyRes = true;
                    $scope.$apply();
                }
            }
        };

        $scope.BulkPvdata = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/BulkPV_Upload",
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
        $scope.Bulkdata = function () {

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/GeneralSettings/BulkCat_Upload",
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
                    url: "/GeneralSettings/BulkQc_Upload",
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

        $scope.promise = $scope.Rework = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/GeneralSettings/Rework",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data + " Records uploaded successfully"


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
    });  


})();