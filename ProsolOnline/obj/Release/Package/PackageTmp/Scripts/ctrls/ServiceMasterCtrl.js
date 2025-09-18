(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables', 'cgBusy', 'ui.bootstrap', 'angular.filter']); // ['datatables']);


    app.controller('ServiceCategoryController', function ($scope, $http, $rootScope, $timeout) {
        //$scope.SAPCategorycode = "";
        $scope.SeviceCategorycode = "";
        $scope.SeviceCategoryname = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_Categoryuser'
            }).success(function (response) {

                $scope.shwusr = response;
               // alert(angular.toJson($scope.shwusr));

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();
       
        $scope.reset_catagory_single = function () {

            $scope.form_catagory_single.$setPristine();
        }

        $rootScope.NotifiyResclose = function () {
        //    alert("hi");
            $('#divNotifiy').attr('style', 'display: none');
        };
       
       
        $scope.createData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDatasercat",               
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
               // alert(data);
               
                   if (data === "False") {
                       $rootScope.Res = "Data already exists";
                     $rootScope.Notify = "alert-info";
                       $rootScope.NotifiyRes = true;
                       $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "ServiceCategory created successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                       $scope.BindList();
                       $('#divNotifiy').attr('style', 'display: block');
                        //$scope.obj.SAPCategorycode = "";
                        $scope.obj.SeviceCategorycode = "";
                        $scope.obj.SeviceCategoryname = "";
                        $scope.reset_catagory_single();
                        $scope.obj = null;
                    }
                   
                  
               
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.ServiceCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelServicecode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $rootScope.NotifiyResclose = function () {
            //    alert("hi");
            $('#divNotifiy').attr('style', 'display: none');
        };


        $scope.ServiceCodeedit = function (x, idx) {

            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

            if (x.Islive == true)
                {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

            var i = 0;
            angular.forEach($scope.ServiceCategory, function (lst) {
                $('#sercat' + i).attr("style", "");
                i++;
            });
            $('#sercat' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.SeviceCategorycode = x.SeviceCategorycode;
            $scope.obj.SeviceCategoryname = x.SeviceCategoryname;
          //  alert(angular.toJson(x.Islive));
            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }
        };

        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.SeviceCategorycode = "";
            $scope.obj.SeviceCategoryname = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };
        
        $scope.updateData = function () {
           
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));

            $http({
                url: "/ServiceMaster/InsertDatasercat",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                       
                }
               else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                   }
                   else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.reset();
                        $scope.obj = null;
                    }
                   
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };
       
        $scope.ServiceCategory = function (idx, enable, _id) {
            //alert(idx);
           // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicecategory?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicecategory disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                         $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.shwusr[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicecategory?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicecategory enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.shwusr[idx].Islive = false;

            }
        };



        // new codes



         $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
          //  alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                   // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        //$scope.visiable = function () {
        //    alert("hai");
        //    $scope.makevisiable1 = true;
        //};

        //$scope.visiable1 = function () {
        //    // alert("hai");
        //    $scope.makevisiable1 = false;
        //};
        $scope.BulkCategory = function () {

            if ($scope.files[0] != null) {

               // alert(angular.toJson($scope.files[0]));
                 $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/bulkCategory_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData}).success(function (response) {
                    //  alert(data);
                     $scope.ShowHide = false;
                     if (response === 0)
                        $rootScope.Res = "Records already exists"
                     else $rootScope.Res = response + " Records uploaded successfully"


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                  //  alert("hai");
                   $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    //$scope.Res = "Select file to be uploaded";
                    //$scope.Notify = "alert-danger";
                    //$scope.NotifiyRes = true;
                    //$('#divNotifiy').attr('style', 'display: block');
                });
            };
        }

    });

    app.controller('ServiceGroupController', function ($scope, $http, $rootScope, $timeout) {
       // $scope.SAPGroupcode = "";
        $scope.ServiceGroupcode = "";
        $scope.ServiceGroupname = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;
     
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_groupuser'
            }).success(function (response) {
                $scope.shwusr2 = response;
                //alert(angular.toJson($scope.shwusr2));
                //alert(angular.toJson($scope.shwusr2))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

     // DataTables configurable options
        $http.get('/ServiceMaster/showall_Categoryuser').success(function (response) {
            //alert('hi');
            $scope.shwusr1 = response
           // alert(angular.toJson($scope.shwusr1))
        });
        //servicegroup
        

        $scope.reset = function () {

            $scope.form1.$setPristine();
        }

        $scope.close_clear_all = function () {

        }

        //$rootScope.NotifiyResclose = function () {
        //    alert("hi");
        //    $('#divNotifiy').attr('style', 'display: none');
        //}
  
        $scope.createData = function () {

           // alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
           // $scope.obj.Label = "Service Group";
            var formData = new FormData();
            $scope.obj.SeviceCategoryname = $("#ServiceCategorycode").find("option:selected").text();
           // alert(angular.toJson($scope.obj.SeviceCategoryname));
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertData",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                   $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                $rootScope.Res = "ServiceGroup created successfully";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.BindList();
                $('#divNotifiy').attr('style', 'display: block');
                $scope.reset();
                $scope.obj = null;
               // $scope.SAPGroupcode = "";
               // $scope.obj.ServiceGroupcode = "";
               // $scope.obj.ServiceGroupname = "";
                }
               
              
           
            }).error(function (data, status, headers, config) {
             });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.GroupCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelGroupcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.GroupCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true) {
                $scope.shinsertupdate = true;

                $scope.viewadd = false;
                $scope.viewclose = true;

                var i = 0;
                angular.forEach($scope.shwusr2, function (lst) {
                    $('#grp' + i).attr("style", "");
                    i++;
                });
                $('#grp' + idx).attr("style", "background-color:lightblue");

                $scope.obj = {};

                $scope.btnSubmit = false;
                $scope.btnUpdate = true;

                $scope.obj._id = x._id;
                $scope.obj.ServiceCategorycode = x.ServiceCategorycode;
                $scope.obj.ServiceCategoryname = x.ServiceCategoryname;
                $scope.obj.ServiceGroupcode = x.ServiceGroupcode;
                $scope.obj.ServiceGroupname = x.ServiceGroupname;
            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        };

        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };

        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            //$scope.obj.ServiceCategorycode = "";
            $scope.obj.ServiceCategoryname = "";
            $scope.obj.ServiceGroupcode = "";
            $scope.obj.ServiceGroupname = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;
        };

        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            $scope.obj.SeviceCategoryname = $("#ServiceCategorycode").find("option:selected").text();
            formData.append("obj", angular.toJson($scope.obj));            
          //  alert(angular.toJson($scope.obj));
           // alert("obj");
            $http({
                url: "/ServiceMaster/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };
        //$scope.DataEdit = function (x, idx) {
        //    var i = 0;
        //    angular.forEach($scope.shwusr1, function (lst) {
        //        $('#grp' + i).attr("style", "");
        //        i++;
        //    });
        //    $('#grp' + idx).attr("style", "background-color:lightblue");
        //    $scope.obj = {};
        //    $scope.btnSubmit = false;
        //    $scope.btnUpdate = true;
        //    $scope.obj._id = x._id;
        //   // $scope.obj.SAPGroupcode = x.SAPGroupcode;
        //    $scope.obj.ServiceGroupcode = x.ServiceGroupcode;
        //    $scope.obj.ServiceGroupname = x.ServiceGroupname;
        //};

        //$scope.updateData = function () {
        //    $timeout(function () { $scope.NotifiyRes = false; }, 30000);
        //    var formData = new FormData();
        //    formData.append("data", angular.toJson($scope.obj));

        //    $http({
        //        url: "/ServiceMaster/InsertData",
        //        method: "POST",
        //        headers: { "Content-Type": undefined },
        //        transformRequest: angular.identity,
        //        data: formData
        //    }).success(function (data, status, headers, config) {

        //        if (data.success === false) {

        //        }
        //        else {
        //            if (data === false) {
        //                $rootScope.Res = "Data already exists";
        //                $rootScope.Notify = "alert-info";
        //                $rootScope.NotifiyRes = true;
        //                $('#divNotifiy').attr('style', 'display: block');
        //            }
        //            else {
        //                $rootScope.Res = "Data updated successfully";
        //                $rootScope.Notify = "alert-info";
        //                $rootScope.NotifiyRes = true;
        //                $scope.BindList();
        //                $('#divNotifiy').attr('style', 'display: block');
        //            }
        //            $scope.reset();
        //            $scope.obj = null;
        //        }
        //    }).error(function (data, status, headers, config) {
        //    });
        //    $scope.btnSubmit = true;
        //    $scope.btnUpdate = false;
        //};
  

        //$scope.ClearFrm = function () {
        //    $scope.obj = null;
        //    $scope.btnSubmit = true;
        //    $scope.btnUpdate = false;
        //    $scope.reset();
        //};

        $scope.ServiceGroup = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            //alert(_id);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicegroup?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicegroup disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.shwusr2[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicegroup?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicegroup enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.shwusr2[idx].Islive = false;

            }

        };

        //newcode bulk
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            //  alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                    // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkGroup = function () {
          //  alert("in");
            if ($scope.files[0] != null) {
              //  alert(angular.toJson($scope.files[0]));

                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/bulkGroup_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                   // alert(response);

                    $scope.ShowHide = false;
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";
                    else
                        $rootScope.Res = response + " Records uploaded successfully";
                    $scope.BindList();

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                   // $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    //  $scope.BindList();
                 //   $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                   // alert("hai");
                    $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                });
            };
        }


    });
   

    app.controller('ServiceUomController', function ($scope, $http, $rootScope, $timeout) {
        $scope.ServiceUomcode = "";
        $scope.ServiceUomname = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_Uomuser'
            }).success(function (response) {

                $scope.getUom = response;
                //alert(angular.toJson($scope.Plant))

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $rootScope.NotifiyResclose = function () {
            //alert("hi");
            $('#divNotifiy').attr('style', 'display: none');
        };

        $scope.createData = function () {

            //alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDataUom",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                //alert(data);
                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                $rootScope.Res = "ServiceUOM created successfully";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.BindList();
                $('#divNotifiy').attr('style', 'display: block');
                $scope.reset();
                $scope.obj = null;
                $scope.obj.ServiceUomcode = "";
                $scope.obj.ServiceUomname = "";
                }
               
               
            }
            ).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.UOMCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelUOMcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };


        $scope.UOMCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {

            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
            var i = 0;
            angular.forEach($scope.ServiceUom, function (lst) {
                $('#uom' + i).attr("style", "");
                i++;
            });
            $('#uom' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.ServiceUomcode = x.ServiceUomcode;
            $scope.obj.ServiceUomname = x.ServiceUomname;

            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }

        };

        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.ServiceUomcode = "";
            $scope.obj.ServiceUomname = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };


        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));

            $http({
                url: "/ServiceMaster/InsertDataUom",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };

        $scope.ServiceUom = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DserviceUom?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "serviceUom disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                    });
                } else {
                    $scope.getUom[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {
                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DserviceUom?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "serviceUom enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                    }).error(function (data, status, headers, config) {
                    });
                } else $scope.getUom[idx].Islive = false;
            }
        };
    });

    app.controller('ServiceValuationController', function ($scope, $http, $rootScope, $timeout) {
        $scope.SeviceValuationcode = "";
        $scope.SeviceValuationname = "";

        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_Valuationuser'
            }).success(function (response) {

                $scope.getValuation = response;
                //alert(angular.toJson($scope.Plant))

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        //$rootScope.NotifiyResclose = function () {
        //    alert("hi");
        //    $('#divNotifiy').attr('style', 'display: none');
        //};

        $scope.createData = function () {

            //alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDataValuation",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else
                    {
                    $rootScope.Res = "ServiceValuation created successfully";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.BindList();
                $('#divNotifiy').attr('style', 'display: block');
                $scope.obj.SeviceValuationcode = "";
                $scope.obj.SeviceValuationname = "";
                $scope.reset();
                $scope.obj = null;
                }
              
              
            }
            ).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;

        $scope.ValuationCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelValuationcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.ValuationCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {

            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
            var i = 0;
            angular.forEach($scope.ServiceValuation, function (lst) {
                $('#valuation' + i).attr("style", "");
                i++;
            });
            $('#valuation' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.ServiceValuationcode = x.ServiceValuationcode;
            $scope.obj.ServiceValuationname = x.ServiceValuationname;
            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }

        };
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.ServiceValuationcode = "";
            $scope.obj.ServiceValuationname = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };

        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));

            $http({
                url: "/ServiceMaster/InsertDataValuation",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };

        $scope.ServiceValuation = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicevaluation?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicevaluation disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.getValuation[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/Dservicevaluation?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "servicevaluation enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                         $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.getValuation[idx].Islive = false;

            }

        };
    });

    app.controller('MainCodeController', function ($scope, $http, $rootScope, $timeout) {
     
        $scope.MainCode = "";
        $scope.MainDiscription = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;

           $http.get('/ServiceMaster/showall_Categoryuser').success(function (response) {
                //alert('hi');
               $scope.getcatMC = response
                // alert(angular.toJson($scope.shwusr1))
            });

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_MainCode'
            }).success(function (response) {

                $scope.Maincode = response;
                // alert(angular.toJson($scope.Maincode));

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

        $scope.reset = function () {

            $scope.form1.$setPristine();
        }
          $scope.createData = function () {

            //alert("hi");
              $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            $scope.obj.SeviceCategoryname = $("#ServiceCategorycodeMC").find("option:selected").text();
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDataMainCode",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                // alert(data);

                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "MainCode created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    //$scope.obj.SAPCategorycode = "";
                    $scope.obj.MainCode = "";
                    $scope.obj.MainDiscription = "";
                    $scope.reset();
                    $scope.obj = null;
                    
                }
               

            }).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.MainCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelMaincode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.MainCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {

            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

            var i = 0;
            angular.forEach($scope.Maincode, function (lst) {
                $('#main' + i).attr("style", "");
                i++;
            });
            $('#main' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.ServiceCategorycode = x.ServiceCategorycode;
            $scope.obj.ServiceCategoryname = x.ServiceCategoryname;
            $scope.obj.MainCode = x.MainCode;
            $scope.obj.MainDiscription = x.MainDiscription;

            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }

        };
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
          //  $scope.obj.MainCode = "";
            $scope.obj.MainDiscription = "";
            $scope.obj.ServiceCategoryname = "";
            $scope.obj.MainDiscription = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };

        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            $scope.obj.SeviceCategoryname = $("#ServiceCategorycodeMC").find("option:selected").text();
            formData.append("obj", angular.toJson($scope.obj));
           // alert(angular.toJson($scope.obj))

            $http({
                url: "/ServiceMaster/InsertDataMainCode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };
        $scope.MainCode = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DMainCode?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "maincode disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.Maincode[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DMainCode?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "maincode enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Maincode[idx].Islive = false;

            }

        };

        //newcode bulk
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
             // alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                    // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkServiceMainCode = function () {
            //alert("in");
            if ($scope.files[0] != null) {
               // alert(angular.toJson($scope.files[0]));

                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/bulkServiceMainCode_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                   // alert(response);

                    $scope.ShowHide = false;
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";
                    else
                        $rootScope.Res = response + " Records uploaded successfully";
               

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    //$scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                   // alert("hai");
                    $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                });
            };
        }


    });
   
    app.controller('SubCodeController', function ($scope, $http, $rootScope, $timeout) {
       
        $scope.SubCode = "";
        $scope.SubDiscription = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;

        // DataTables configurable options
        $http.get('/ServiceMaster/showall_MainCode').success(function (response) {
            //alert('hi');
            $scope.MainCode1 = response
            // alert(angular.toJson($scope.MainCode1))
        });
      

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_SubCodeUser'
            }).success(function (response) {
                $scope.Subcode = response;
                //alert(angular.toJson($scope.Subcode));
                
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            // alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
           
            var formData = new FormData();
            $scope.obj.MainDiscription = $("#MainCode").find("option:selected").text();
            // alert(angular.toJson($scope.obj.MainDiscription));
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDataSubCode",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data === "False") {
                   $rootScope.Res = "Data already exists";
                   $rootScope.Notify = "alert-info";
                   $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "SubCode created successfully";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $scope.BindList();
                $('#divNotifiy').attr('style', 'display: block');
         
                $scope.obj.SubCode = "";
                $scope.obj.SubDiscription = "";
                $scope.reset();
                $scope.obj = null;
               
                }
                
            
            }).error(function (data, status, headers, config) {
             });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.SubCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelSubcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.SubCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
            var i = 0;
            angular.forEach($scope.MainCode1, function (lst) {
                $('#sub' + i).attr("style", "");
                i++;
            });
            $('#sub' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            // $scope.obj.SAPGroupcode = x.SAPGroupcode;
            $scope.obj.MainCode = x.MainCode;
            $scope.obj.MainDiscription = x.MainDiscription;
            $scope.obj.SubCode = x.SubCode;
            $scope.obj.SubDiscription = x.SubDiscription;
            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }

        };
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.SubCode = "";
            $scope.obj.SubDiscription = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };


        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            $scope.obj.MainDiscription = $("#MainCode").find("option:selected").text();
            formData.append("obj", angular.toJson($scope.obj));
            //alert(angular.toJson($scope.obj));

            $http({
                url: "/ServiceMaster/InsertDataSubCode",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };


        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };

        $scope.SubCode = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
          //  alert(_id);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DSubCode?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "subcode disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.Subcode[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DSubCode?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "subcode enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Subcode[idx].Islive = false;

            }

        };

        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
           // alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                    // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkServiceSubCode = function () {
            //alert("in");
            if ($scope.files[0] != null) {
               // alert(angular.toJson($scope.files[0]));

                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/bulkServiceSubCode_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                  //  alert(response);

                    $scope.ShowHide = false;
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";


                    else
                        $rootScope.Res = response + " Records uploaded successfully";


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    //$scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    // alert("hai");
                    $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                });
            };
        }


    });

    app.controller('SubSubCodeController', function ($scope, $http, $rootScope, $timeout) {
     
        $scope.SubSubCode = "";
        $scope.SubSubDiscription = "";
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;

        // DataTables configurable options
        $http.get('/ServiceMaster/showall_MainCode').success(function (response) {
            //alert('hi');
            $scope.getmain = response;
             //alert(angular.toJson($scope.getmain))
        });
        $scope.getsubcode = function (MainCode) {
            // alert(MainCode);
            $http.get('/ServiceMaster/getSubList?MainCode=' + MainCode
            ).success(function (response) {
                //alert('hi
                // alert(angular.toJson(response))
                $scope.getsub = response
                $scope.obj.SubCode = "";
                //  alert(angular.toJson($scope.getsub))
            }).error(function (data, status, headers, config) {
            });

        };
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_SubSubUser'
            }).success(function (response) {
                $scope.SubSub = response;
               // alert(angular.toJson($scope.SubSub))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

       

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            // alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            // $scope.obj.Label = "Service Group";
            var formData = new FormData();
            $scope.obj.MainDiscription = $("#Maincode2").find("option:selected").text();
            $scope.obj.SubDiscription = $("#SubCode2").find("option:selected").text();
       
            formData.append("obj", angular.toJson($scope.obj));
            //alert(angulr.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDataSubSub",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                
                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "SubSubCode created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.obj.SubSubCode = "";
                    $scope.obj.SubSubDiscription = "";
                    $scope.reset();
                    $scope.obj = null;
                    

                }
                
            }).error(function (data, status, headers, config) {
             });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.SubSubCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelSubSubcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.SubSubCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
            var i = 0;
            angular.forEach($scope.SubSub, function (lst) {
                $('#subsub' + i).attr("style", "");
                i++;
            });
            $('#subsub' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};

            $http.get('/ServiceMaster/showall_MainCode').success(function (response) {
                //alert('hi');
                $scope.getmain = response;
              //  alert(angular.toJson($scope.getmain))
            });
            $scope.obj.MainCode = x.MainCode;

            $http.get('/ServiceMaster/getsubcodeT?MainCode=' + x.MainCode
            ).success(function (response) {
              //  alert(angular.toJson(response))
                $scope.getsub = response;

            }).error(function (data, status, headers, config) {
            });
              //  alert("hi");
                $scope.obj.SubCode = x.SubCode;
              
              //  $scope.obj.SubDiscription = x.SubDiscription;
             //   alert(angular.toJson($scope.obj.SubCode))
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
           // alert(angular.toJson(x));
            //$scope.obj.MainCode = x.MainCode;
            //$scope.obj.MainDiscription = x.MainDiscription;
            //$scope.obj.SubCode = x.SubCode;
            //$scope.obj.SubDiscription = x.SubDiscription;
            $scope.obj.SubSubCode = x.SubSubCode;
            $scope.obj.SubSubDiscription = x.SubSubDiscription;

            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }

        };
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
        };

        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.SubSubCode = "";
            $scope.obj.SubSubDiscription = "";
            $scope.obj.MainCode = "";
            $scope.obj.MainDiscription = "";
            $scope.obj.SubCode = "";
            $scope.obj.SubDiscription = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };

        $scope.updateData = function () {
          //  alert("hai");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            $scope.obj.MainDiscription = $("#Maincode2").find("option:selected").text();
            $scope.obj.SubDiscription = $("#SubCode2").find("option:selected").text();

            formData.append("obj", angular.toJson($scope.obj));
          //  alert(angular.toJson($scope.obj));


            $http({
                url: "/ServiceMaster/InsertDataSubSub",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        };



        $scope.Sub_SubCode = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DSubSub?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "SubSub disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.SubSub[idx].Islive = true;
                }

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DSubSub?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "SubSub enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.SubSub[idx].Islive = false;


            }
        }

        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            //alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                    // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkServiceSubSubCode = function () {
           // alert("in");
            if ($scope.files[0] != null) {
                // alert(angular.toJson($scope.files[0]));

                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/bulkServiceSubSubCode_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                   // alert(response);

                    $scope.ShowHide = false;
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";


                    else
                        $rootScope.Res = response + " Records uploaded successfully";


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    //$scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    // alert("hai");
                    $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                });
            };
        }

    });

    app.controller('AttributeController', function ($scope, $window, $http, $rootScope, $timeout, $filter) {
     //   alert("hi")
        $http({
            method: 'GET',
            url: '/GeneralSettings/GetUnspsc',
            params: { Noun: 'Service', Modifier: 'Service' }
        }).success(function (response) {

            if (response != '') {
                $scope.Commodities = response;
                //if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                //    $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                //else $scope.cat.Unspsc = $scope.Commodities[0].Class;
            }
            else {
                $scope.Commodities = null;
            }

        }).error(function (data, status, headers, config) {
            // alert("error");

        });

        $scope.BindGroupinx = function (inx) {
         //   alert("hai");
           // $scope.showlb = $scope.showlb;
            if ($scope.valueSrch != null && $scope.valueSrch != "" && $scope.valueSrch != undefined) {
                $http({
                    method: 'GET',
                    url: '/ServiceMaster/GetValueListforcreate_tempsearch?currentPage=' + inx + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Activity=' + $scope.Activity + '&srchtxt=' + $scope.valueSrch
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 25;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        if (response.Abbrevatedvalues != null) {
                            //  alert("in");
                            $scope.ValueList = response.Abbrevatedvalues;
                            $scope.selectValue = [];
                            $('#divValAdd').attr('style', 'display: block');
                            $http({
                                method: 'GET',
                                url: '/ServiceMaster/GetAttributesDetail?Name=' + $scope.showlb
                            }).success(function (response) {
                               // alert(angular.toJson(response));
                                if (response._id != null) {
                                    if (response.ValueList != null) {
                                        $scope.selectValue = response.ValueList;
                                        angular.forEach($scope.ValueList, function (lst) {
                                            if (response.ValueList.indexOf(lst._id) !== -1) {
                                                // alert("in");
                                                lst.Checked = '1';
                                                $('#chk' + lst._id).prop('disabled', true);
                                            } else {
                                                lst.Checked = '0';
                                            }
                                        });
                                    }
                                }
                            }).error(function (data, status, headers, config) {
                            });
                        }
                        else {
                            $scope.ValueList = response.Characteristicvalues;
                            angular.forEach($scope.ValueList, function (lst) {
                                if ($scope.selectValue.indexOf(lst._id) !== -1) {
                                    lst.Checked = '1';
                                } else {
                                    lst.Checked = '0';
                                }
                            });
                        }
                    }
                }).error(function (data, status, headers, config) {
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/ServiceMaster/GetValueListforcreate_temp?currentPage=' + inx + '&maxRows=' + 25 + '&Name=' + $scope.showlb + 'Activity=' + $scope.Activity
                }).success(function (response) {
                    if (response != '') {
                        $scope.numPerPage = 25;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        if (response.Abbrevatedvalues != null) {
                            //  alert("in");
                            $scope.ValueList = response.Abbrevatedvalues;
                            $scope.selectValue = [];
                            $('#divValAdd').attr('style', 'display: block');
                            $http({
                                method: 'GET',
                                url: '/ServiceMaster/GetAttributesDetail?Name=' + $scope.showlb
                            }).success(function (response) {
                              //  alert(angular.toJson(response));
                                if (response._id != null) {
                                    if (response.ValueList != null) {
                                        $scope.selectValue = response.ValueList;
                                        angular.forEach($scope.ValueList, function (lst) {
                                            if (response.ValueList.indexOf(lst._id) !== -1) {
                                                // alert("in");
                                                lst.Checked = '1';
                                                $('#chk' + lst._id).prop('disabled', true);
                                            } else {
                                                lst.Checked = '0';
                                            }
                                        });
                                    }
                                }
                            }).error(function (data, status, headers, config) {
                            });
                        }
                        else {
                            $scope.ValueList = response.Characteristicvalues;
                            angular.forEach($scope.ValueList, function (lst) {
                                if ($scope.selectValue.indexOf(lst._id) !== -1) {
                                    lst.Checked = '1';
                                } else {
                                    lst.Checked = '0';
                                }
                            });

                            //            $scope.selectValue = [];
                            //   $('#divValAdd').attr('style', 'display: block');


                        }
                    }
                }).error(function (data, status, headers, config) {
                });

            }


        };

        $scope.BindGroupinxsearch = function () {
            // alert(
            //  "in");
            $http({
                method: 'GET',
                url: '/ServiceMaster/GetValueListforcreate_tempsearch?currentPage=' + 1 + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Activity=' + $scope.Activity + '&srchtxt=' + $scope.valueSrch
            }).success(function (response) {
                if (response != '') {
                    $scope.numPerPage = 25;
                    $scope.PageCount = response.PageCount;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    if (response.Abbrevatedvalues != null) {
                        //  alert("in");
                        $scope.ValueList = response.Abbrevatedvalues;
                        $scope.selectValue = [];
                        $('#divValAdd').attr('style', 'display: block');
                        $http({
                            method: 'GET',
                            url: '/ServiceMaster/GetAttributesDetail?Name=' + $scope.showlb
                        }).success(function (response) {
                            // alert(angular.toJson(response));
                            if (response._id != null) {
                                if (response.ValueList != null) {
                                    $scope.selectValue = response.ValueList;
                                    angular.forEach($scope.ValueList, function (lst) {
                                        if (response.ValueList.indexOf(lst._id) !== -1) {
                                            // alert("in");
                                            lst.Checked = '1';
                                            $('#chk' + lst._id).prop('disabled', true);
                                        } else {
                                            lst.Checked = '0';
                                        }
                                    });
                                }
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    else {
                        $scope.ValueList = response.Characteristicvalues;
                        angular.forEach($scope.ValueList, function (lst) {
                            if ($scope.selectValue.indexOf(lst._id) !== -1) {
                                lst.Checked = '1';
                            } else {
                                lst.Checked = '0';
                            }
                        });
                    }
                }
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.ShowValue = function (Name, MainCode, SubCode) {
            //alert("hai");
            //alert("value");
           
            $scope.showlb = Name;
           // alert(angular.toJson($scope.Name));
            $http({
                method: 'GET',
                url: '/ServiceMaster/GetValueListforcreate_temp?currentPage=' + 1 + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Activity=' + $scope.Activity
            }).success(function (response) {
                if (response != '') {
                    $scope.numPerPage = 25;
                    $scope.PageCount = response.PageCount;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;

                    if (response.Abbrevatedvalues != null) {
                        $scope.ValueList = response.Abbrevatedvalues;
                        $scope.selectValue = [];
                        $('#divValAdd').attr('style', 'display: block');
                        $http({
                            method: 'GET',
                            url: '/ServiceMaster/GetAttributesDetail?Name=' + Name
                        }).success(function (response) {

                            if (response._id != null) {
                                if (response.ValueList != null) {
                                    $scope.selectValue = response.ValueList;
                                    angular.forEach($scope.ValueList, function (lst) {
                                        if (response.ValueList.indexOf(lst._id) !== -1) {
                                            lst.Checked = '1';
                                            $('#chk' + lst._id).prop('disabled', true);
                                        } else {
                                            lst.Checked = '0';
                                        }
                                    });
                                }
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    else {
                        $scope.ValueList = response.Characteristicvalues;
                        $scope.selectValue = [];
                        $('#divValAdd').attr('style', 'display: block');
                        $http({
                            method: 'GET',
                            url: '/ServiceMaster/GetAttributesDetailfromcharacteristicvalues?Name=' + Name + '&Activity=' + $scope.Activity
                        }).success(function (response) {
                            // alert(angular.toJson(response));
                            if (response._id != null) {
                                if (response.ValueList != null) {
                                    $scope.selectValue = response.ValueList;
                                    angular.forEach($scope.ValueList, function (lst) {
                                        if (response.ValueList.indexOf(lst._id) !== -1) {
                                            lst.Checked = '1';
                                            $('#chk' + lst._id).prop('disabled', false);
                                        } else {
                                            lst.Checked = '0';
                                            $('#chk' + lst._id).prop('disabled', false);
                                        }
                                    });
                                }
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    }
                }
            }).error(function (data, status, headers, config) {
            });

    };
        $scope.saveData = function () {
            //alert("hai");
            var obj = $.grep($scope.rows, function (lst) {
                return lst.Attributes == $scope.showlb;
            })[0];
            var inx = $scope.rows.indexOf(obj)
            $scope.rows[inx].Values = $scope.selectValue;
            $('#divValAdd').attr('style', 'display: none');

        };
        $scope.closeData = function () {

            $('#divValAdd').attr('style', 'display: none');

        };
        $scope.selectValue = [];
        $scope.checkValue = function (id, indx) {
            // alert(angular.toJson(id));


            if ($('#chk' + id).is(':checked')) {

                $scope.selectValue.push(id);
                $scope.ValueList;
            }
            else {
                var index = $scope.selectValue.indexOf(id);
                $scope.selectValue.splice(index, 1);


            }
            //  alert(angular.toJson($scope.selectValue));

        };

        $scope.changeCheck = function () {
            var characterList = [];
            $scope.charcheck = false;
            angular.forEach($scope.rows, function (lst) {
              
                if (characterList.indexOf(lst.Attributes) !== -1) {
                    $scope.charcheck = true;
                    
                }               
                characterList.push(lst.Attributes);
              

            });
        };
        //$scope.getattibute1 = function () {

        //  //  $scope.CharaRw.AttributeName = [];
        //   // alert(angular.toJson($scope.CharaRw.AttributeName));
        //    $http.get('/ServiceMaster/getAttribute1'
        //     ).success(function (response) {
        //         if (response != null && response != "" && response != undefined) {
        //             $scope.rows = response;
        //           //  alert(angular.toJson($scope.rows));


        //         }
        //         else {
        //             //$rootScope.resdwn = false;
        //            // $rootScope.resdwn1 = false;
        //           //  $scope.files = [];
        //             $scope.rows = [{ 'Sequence': 1 }];
        //         }
        //     }).error(function (data, status, headers, config) {
        //     });
        //};
        //$scope.getattibute1();

        //$scope.getAttributeUniqueList = function () {
            

        //    $http({
        //        method: 'GET',
        //        url: '/ServiceMaster/getAttributeUniqueList'
        //    }).success(function (response) {
        //         $scope.AttributesList= response;                
        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });

            
        //}
        //$scope.getAttributeUniqueList();
        $scope.getAttributeList = function () {
           // alert("hai");
            $http({
                method: 'GET',
                url: '/ServiceMaster/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response
               // alert(angular.toJson($scope.AttributesList));
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getAttributeList();
        $scope.Listmodifer = function () {
            $http({
                method: 'GET',
                url: '/ServiceMaster/GetModifier',
                params: { Noun: $scope.Noun }
                //?Noun='+$scope.noun
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
      
        $scope.CreateAttribute = function () {
            //alert("hi");
            var arrList_Long = [];
            $scope.Seqcheck1 = false;

            var characterList = [];
            $scope.charcheck = false;

            angular.forEach($scope.rows, function (lst) {

                if (characterList.indexOf(lst.Attributes) !== -1) {
                    $scope.charcheck = true;
                }
                characterList.push(lst.Attributes);


            });

            angular.forEach($scope.rows, function (lst) {

                if (arrList_Long.indexOf(parseInt(lst.Sequence)) !== -1 || parseInt(lst.Sequence) < 1) {
                    $scope.Seqcheck1 = true;
                }
                arrList_Long.push(parseInt(lst.Sequence));


            });
            for (var i = 1; i <= arrList_Long.length; i++) {
                if (arrList_Long.indexOf(i) == -1) {
                    $scope.Seqcheck1 = true;
                }
            }
           // alert(angular.toJson($scope.Seqcheck1));
            //alert(angular.toJson($scope.charcheck));
            if ($scope.Seqcheck1 == false && $scope.charcheck == false) {
                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
              
                var formData = new FormData();
           //     $scope.obj.MainDiscription = $("#MainCodeAA").find("option:selected").text();
              //  $scope.obj.SubDiscription = $("#SubCodeAA").find("option:selected").text();
                formData.append("Noun", angular.toJson($scope.Noun));
                formData.append("Modifier", angular.toJson($scope.Modifier));
               // alert(angular.toJson($scope.obj));
                formData.append("CHA", angular.toJson($scope.rows));
               // alert(angular.toJson($scope.rows));
                $http({
                    url: "/ServiceMaster/addMS",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data.success === false) {

                        $rootScope.Res = data.errors;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;

                    }
                    else {

                        $scope.reset();

                        $scope.obj = null;
                        $scope.rows = null;
                        $scope.Activity = null;
                        $scope.Seqcheck1 = false;
                        $scope.charcheck = false;
                        $scope.Noun = "";
                        $scope.Modifier = "";
                      
                        $scope.rows = [{ Sequence: 1, 'Remove': 0 }];
                        //angular.element("input[type='file']").val(null);
                        $rootScope.Res = "Attribute created successfully";
                        //alert(angular.toJson($rootScope.Res));
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                           $('#divNotifiy').attr('style', 'display: block');
                        // $('.fileinput').fileinput('clear');
                    }
              }).error(function (data, status, headers, config) {
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                });
            }

        };


        $scope.rows = [{ 'Sequence': 1, 'Remove': 0 }];
        $scope.addRow = function () {
           
            var characterList = [];
            $scope.charcheck = false;
            angular.forEach($scope.rows, function (lst) {
               // alert(angular.toJson(characterList.indexOf(lst.Attributes)));
                if (characterList.indexOf(lst.Attributes) !== -1) {
                    $scope.charcheck = true;
                }
                characterList.push(lst.Attributes);
            });
            if ($scope.charcheck == false)
                $scope.rows.push({ 'Sequence': $scope.rows.length + 1, 'Remove': 0 });
          //  alert(angular.toJson($scope.rows));
        };

        $scope.RemoveRow = function () {
           // alert(angular.toJson(indx));
            if (confirm("Are you sure, wants to remove?")) {

                
                var cunt = 1;

                $scope.ar = [{ 'Sequence': 1, 'Remove': 0 }];
                angular.forEach($scope.rows, function (value, key) {
                    if (value.Remove == 0) {
                        $scope.ar.push(value);
                    }
                });

                //angular.forEach($scope.rows, function (value, key) {
                //    alert(angular.toJson($scope.rows));
                //    alert(angular.toJson(key));
                //    if(value.Remove != 0)
                //    {
                      
                //        $scope.rows.splice(key,1);
                //    }

                //    alert(angular.toJson($scope.rows));

                //});


                $scope.ar.splice(0, 1);
                $scope.rows = $scope.ar;
                

                angular.forEach($scope.rows, function (value, key) {

                    value.Sequence = cunt;
                    cunt++;

                });
            }
        };



        $scope.identifyremove_rows = function (index) {
         //  alert(angular.toJson(index));
            if ($scope.rows[index].Remove === 0) {
                $scope.rows[index].Remove = 1;
            } else {
                $scope.rows[index].Remove = 0;
            }
            
          //   alert(angular.toJson($scope.rows));

        };
        app.directive('loading', ['$http', function ($http) {
            return {
                restrict: 'A',
                link: function (scope, elm, attrs) {
                    scope.isLoading = function () {
                        return $http.pendingRequests.length > 0;
                    };

                    scope.$watch(scope.isLoading, function (v) {
                        if (v) {
                            elm.show();
                        } else {
                            elm.hide();
                        }
                    });
                }
            };

        }]);

        $scope.reset = function () {

            $scope.form_Attr_single.$setPristine();
        }

        //$http.get('/ServiceMaster/showall_MainCode'
        //        ).success(function (response) {
        //            //  alert('hi');
        //            $scope.getmainA = response;
        //            //alert(angular.toJson($scope.getmainA));
        //            $scope.getmainA = $filter('filter')($scope.getmainA, { 'Islive': 'true' })
                   
        //        }).error(function (data, status, headers, config) {
        //        });

        /////////

        $http.get('/ServiceMaster/showall_Categoryuser'
        ).success(function (response) {
            //  alert('hi');
            $scope.getmainA = response;
            //alert(angular.toJson($scope.getmainA));
            $scope.getmainA = $filter('filter')($scope.getmainA, { 'Islive': 'true' })

        }).error(function (data, status, headers, config) {
        });
        ////

        //$scope.getsub1 = function (MainCode) {
        //    $http.get('/ServiceMaster/getSubList?MainCode=' + MainCode
        //    ).success(function (response) {
        //        alert(angular.toJson(response));
        //        $scope.getsubA = response;

        //       // $scope.rows = response;
        //        $scope.getsubA = $filter('filter')($scope.getsubA, { 'Islive': 'true' })
        //    }).error(function (data, status, headers, config) {
        //    });
        //};
        $scope.getsub1 = function () {
            $http.get('/ServiceMaster/getSubList1?MainCode=' + $scope.obj.MainCode
           ).success(function (response) {
             
               $scope.getsubA = response;
               $scope.obj.SubCode = "";
             //  alert(angular.toJson($scope.getsubA));
              $scope.getsubA = $filter('filter')($scope.getsubA, { 'Islive': 'true' })
           }).error(function (data, status, headers, config) {
           });
        }

        $scope.getattibute = function (Noun, Modifier) {
         
            $http.get('/ServiceMaster/getAttribute?Noun=' + Noun + '&Modifier=' + Modifier
             ).success(function (response) {
                 if (response != null && response != "") {
                   
                     $scope.rows = response;
                     angular.forEach($scope.rows, function (lst) {
                         lst.Remove = 0;
                  
                     });    
                 }
                 else {
                    // $rootScope.resdwn = false;
                    // $rootScope.resdwn1 = false;
                  //   $scope.files = [];
                     $scope.rows = [{ 'Sequence': 1,'Remove': 0 }];
                 }
             }).error(function (data, status, headers, config) {
             });
        };


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
          //  alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    //alert("hai");
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    //$rootScope.Res = "Data already exists";
                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    angular.element("input[type='file']").val(null);
                    // files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkCharacteristic_Upload = function () {
          //  alert("in");
            if ($scope.files[0] != null) {
               // alert(angular.toJson($scope.files[0]));

                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 30000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/BulkCharacteristic_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                  //  alert(response);

                    $scope.ShowHide = false;
                    //if (response === 0)
                    //    $rootScope.Res = "Records already exists"
                    //else if (response === -1)
                    //    $rootScope.Res = "Please upload MainDiscription and SubDiscription"
                    //else $rootScope.Res = response + " Records uploaded successfully"
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";


                    else
                        $rootScope.Res = response + " Records uploaded successfully";


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    //$scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                   // $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    // alert("hai");
                    $rootScope.ShowHide = false;
                    $rootScope.Res = "Select file to be uploaded";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                });
            };
        }
        $scope.ShowHide2 = false;

        $scope.files = [];

        $scope.LoadFileData1 = function (files) {
          //  alert("hai");
            $scope.$rootScope = false;
            $rootScope.$apply();
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $scope.$rootScope = "Load valid excel file";
                    $scope.$rootScope = "alert-danger";
                    $scope.$rootScope = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.Bulkvalue_Upload = function () {

            ////alert("in");
            if ($scope.files[0] != null) {
                //(angular.toJson($scope.files[0]));
                $scope.ShowHide2 = true;

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/ServiceReview/Bulkvalue_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {

                    //  alert(response);

                    $scope.ShowHide = false;
                    //if (response === 0)
                    //    $rootScope.Res = "Records already exists"
                    //else if (response === -1)
                    //    $rootScope.Res = "Please upload MainDiscription and SubDiscription"
                    //else $rootScope.Res = response + " Records uploaded successfully"
                    if (response === 0)
                        $rootScope.Res = "No Records have been uploaded";


                    else
                        $rootScope.Res = response + " Records uploaded successfully";


                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    //$scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    // $scope.BindList();
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide2 = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }


    });

    app.controller('ValueAbbrController', function ($scope, $http, $rootScope, $timeout) {
        //$scope.SAPCategorycode = "";
        $scope.CharacteristicValue = "";
        $scope.ValueAbbreviation = "";
        $scope.shupdate = false;
        $scope.btnSubmit = true;
        $scope.shinsertupdate = false;
        $scope.viewadd = true;
        $scope.viewclose = false;


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/ListValAbbr'
            }).success(function (response) {

                $scope.valabbr = response;
                // alert(angular.toJson($scope.shwusr));

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();

        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $rootScope.NotifiyResclose = function () {
            //    alert("hi");
            $('#divNotifiy').attr('style', 'display: none');
        };


        $scope.createData = function () {

            //alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/Insertvalabbr",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                // alert(data);

                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "CharacteristicValue created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    //$scope.obj.SAPCategorycode = "";
                    $scope.obj.CharacteristicValue = "";
                    $scope.obj.ValueAbbreviation = "";
                    $scope.reset();
                    $scope.obj = null;
                }



            }).error(function (data, status, headers, config) {
            });
        };
        $scope.btnSubmit = true;
      //  $scope.btnUpdate = false;
        $scope.ValAbbrCodeDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/Delvalabbrcode?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.ValAbbrCodeedit = function (x, idx) {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);


            if (x.Islive == true)
            {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

            var i = 0;
            angular.forEach($scope.CharacteristicValue, function (lst) {
                $('#valabbr' + i).attr("style", "");
                i++;
            });
            $('#valabbr' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.shupdate = true;

            $scope.obj._id = x._id;
            $scope.obj.CharacteristicValue = x.CharacteristicValue;
            $scope.obj.ValueAbbreviation = x.ValueAbbreviation;

            }
            else {
                $rootScope.Res = "Disabled records can't be edited";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }



        };

        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;
            $scope.shupdate = false;
        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
            $scope.obj.CharacteristicValue = "";
            $scope.obj.ValueAbbreviation = "";
            $scope.btnUpdate = false;
            $scope.btnSubmit = true;

        };

        $scope.updateData = function () {
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));

            $http({
                url: "/ServiceMaster/Insertvalabbr",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $scope.BindList();
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.reset();
                        $scope.obj = null;
                    }

                }
            }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.shupdate = false;
        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.shupdate = false;
            $scope.reset();
        };

        $scope.ValAbbr = function (idx, enable, _id) {
            //alert(idx);
            // alert(enable);
            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DValAbbr?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "CharacteristicValue disabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else {
                    $scope.valabbr[idx].Islive = true;
                }
            } else {
                if (confirm("Are you sure, enable this record?")) {
                    $http({
                        method: 'get',
                        url: '/ServiceMaster/DValAbbr?id=' + _id + '&islive=' + enable
                    }).success(function (response) {
                        $rootscope.res = "CharacteristicValue enabled";
                        $rootscope.notify = "alert-info";
                        $('#divnotifiy').attr('style', 'display: block');
                        $scope.bindlist();
                    }).error(function (data, status, headers, config) {
                    });
                } else $scope.valabbr[idx].Islive = false;
            }
        };
    });

    app.controller('DefaultAttributeController', function ($scope, $http, $rootScope, $timeout, $filter) {
        //$scope.getAttributeUniqueList();


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/ServiceMaster/ShwDefaultAttr'
            }).success(function (response) {

                $scope.shwattr = response;
                //alert(angular.toJson($scope.Plant))

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.getAttributeList = function () {
            // alert("hai");
            $http({
                method: 'GET',
                url: '/ServiceMaster/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response
                // alert(angular.toJson($scope.AttributesList));
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getAttributeList();


        $scope.createData = function () {

            // alert("hi");
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

            var formData = new FormData();
           // $scope.obj.MainDiscription = $("#MainCode").find("option:selected").text();
            // alert(angular.toJson($scope.obj.MainDiscription));
            formData.append("CharaRw", angular.toJson($scope.CharaRw));
        //    alert(angular.toJson($scope.CharaRw));
            $http({
                method: "POST",
                url: "/ServiceMaster/InsertDefaultAttr",

                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data === "False") {
                    $rootScope.Res = "Data already exists";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else {
                    $rootScope.Res = "Attribute created successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                    // $scope.CharaRw.Attributes = "";
                    $scope.CharaRw.Attributes = "";
                      $scope.reset();
                   // $scope.obj = null;

                }
             }).error(function (data, status, headers, config) {
            });
        };
        $scope.ServiceAttrDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DeleteDefAttr?id=' + _id
                }).success(function (response) {
                    $rootScope.Res = "Record Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

    });

    app.controller('ServiceCodeLogicController', function ($scope, $http, $rootScope, $timeout) {
       // alert("in");
        $http({
            method: 'GET',
            url: '/ServiceMaster/loadversionforservice'
        }).success(function (response) {
            $scope.Versionddl = response;

        }).error(function (data, status, headers, config) {
        });


        $http({
            method: 'GET',
            url: '/ServiceMaster/Showdataservice'
        }).success(function (response) {
            if (response.SERVICECODELOGIC == "UNSPSC Code") {
                $scope.obj = { "SERVICECODELOGIC": "UNSPSC Code", "Version": response.Version };
                $scope.showunspscversion();
            }
            else if (response.SERVICECODELOGIC == "Customized Code") {
                $scope.obj = { "SERVICECODELOGIC": "Customized Code", "Version": response.Version };
                $scope.showunspscversion();
            }
            else {
                $scope.obj = { "SERVICECODELOGIC": "Item Code", "Version": response.Version };
                $scope.showunspscversion();
            }

        }).error(function (data, status, headers, config) {
        });
        $http({
            method: 'GET',
            url: '/User/roles'
        }).success(function (response) {
            $scope.role = response;

        });
        //  Versionddl

        $scope.hideversion = function () {
            $scope.showversion = false;
            $scope.unspscddl = false;
        };

        $scope.showunspscversion = function () {
            $scope.showversion = true;
            $scope.unspscddl = true;
        };

        $scope.createcodelogicforservice = function () {

            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            //  alert(angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/ServiceMaster/CodeSaveforservice",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                $rootScope.Res = "CodeLogic has been changed"
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }).error(function (data, status, headers, config) {
            });
        };
    });


    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/ServiceMaster/AutoCompleteNoun",
                    params: { term: term },
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Noun,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.Noun = selectedItem.item.value;


                        $.get("/ServiceMaster/GetNounDetail", { Noun: scope.Noun }).success(function (response) {

                            scope.NM = response;
                            scope.$apply();
                            event.preventDefault();
                        });

                        //$.ajax({
                        //     url: 'GetNounDetail?Noun=' + scope.NM.Noun,                        //    
                        //    type: 'GET',
                        //    data: { "Noun": scope.NM.Noun },
                        //    dataType: "json",
                        //    success: function (response) {
                        //        // alert(JSON.stringify(response.ALL_NM_Attributes));

                        //        scope.NM = response;                               
                        //        scope.$apply();
                        //        event.preventDefault();
                        //    },
                        //    error: function (xhr, ajaxOptions, thrownError) {
                        //        scope.Res = thrownError;

                        //    }
                        //});


                    }
                });

            }

        };
    }]);
})();