
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter']);
    app.controller('RequestServiceController', function ($scope, $window, $http, $rootScope, $timeout, $filter) {
        $scope.sResultbulk1 = [];
        $scope.hidetb2 = true;
        $scope.getplant = [];
        $scope.getcategory = [];
        $scope.getgroup1S = [];
        $scope.getuomS = [];
        $scope.getapprover = [];

        $scope.getplant = [];
        $scope.getcategory1M = [];
        $scope.group = [];
        $scope.getuom1M = [];
        $scope.getCleanser1 = [];

        $scope.add_button = true;
        $scope.update_button = false;

        $scope.tablerow1 = 0;
        $scope.ngdis2 = false;
        $scope.ngdis1 = false;
        $scope.ngdis = false;

        $scope.show_loading_multiple = false;
        $scope.show_loading_single = false;


        // $scope.Cleanser = [];

        //singlemaster
        //gategory

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
        $scope.BindMaster = function () {
            $http({
                method: 'GET',
                url: '/Master/GetMaster'
            }).success(function (response) {

                $scope.MasterList = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindMaster();
        $scope.BindCategory = function () {
            $http({
                method: 'GET',
                url: '/Master/Getcategory'
            }).success(function (response) {

                $scope.getcategory = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindCategory();
        $http.get('/Master/GetDataListplnt'
            ).success(function (response) {
                // alert(angular.toJson(response));
                $scope.getplant = response;
                $scope.getplant = $filter('filter')($scope.getplant, { 'Islive': 'true' })

            }).error(function (data, status, headers, config) {

            });


        $http.get('/ServiceMaster/showall_Categoryuser'
            ).success(function (response) {
                //  alert('hi');
                $scope.getcategory = response;
                //alert(angular.toJson($scope.getcategory));
                $scope.getcategory = $filter('filter')($scope.getcategory, { 'Islive': 'true' })
                $scope.getcategory1M = response;
                $scope.getcategory1M = $filter('filter')($scope.getcategory1M, { 'Islive': 'true' })
                //  alert(angular.toJson($scope.getcategory));
            }).error(function (data, status, headers, config) {
            });

        $scope.getCleanser = function () {
            $http.get(
                '/ServiceMaster/getCleanser'
                ).success(function (response) {

                    $scope.getCleanser1 = response;
                    // alert(angular.toJson($scope.getCleanser1));
                }).error(function (data) {

                });
        };

        $scope.getCleanser();
        //group
        $scope.ReRequest = function () {
           
            var rows1 = [];
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            var planttitle = $.grep($scope.getplant, function (plant1) {
                return plant1.Plantcode == $scope.obj.PlantCode;
            })[0].Plantname;
            

            var catname = $.grep($scope.getcategory, function (plant1) {
                return plant1.SeviceCategorycode == $scope.obj.SeviceCategorycode;
            })[0].SeviceCategoryname;
            

            var groupname = $.grep($scope.MasterList, function (plant1) {
                return plant1.Code == $scope.obj.ServiceGroupCode;
            })[0].Title;

           
            //var storagetitle = $.grep($scope.Storageoptions1, function (storage1) {
            //    return storage1.Code == $scope.ddlstorage1;
            //})[0].Title;
        
           
            rows1.push({ ItemId: $scope.ItemId, PlantCode: $scope.obj.PlantCode, SeviceCategorycode: $scope.obj.SeviceCategorycode, ServiceGroupCode: $scope.obj.ServiceGroupCode, UomCode: $scope.obj.ServiceUomcode, ServiceCategoryName: catname, ServiceGroupName: groupname, PlantName: planttitle, Legacy: $scope.obj.ServiceDiscription1 });
           
         


            var form_singlerequest = new FormData();
            form_singlerequest.append('Rerequest', angular.toJson(rows1));
           


            $scope.cgBusyPromises = $http({
                method: 'POST',
                url: '/ServiceMaster/ReRequest',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: form_singlerequest
                //?plant=' + plantname + '&storage=' + storagename + '&group=' + groupname + '&subgroup=' + subgroupname + '&approver=' + approvername + '&source=' + source_desc

            }).success(function (response) {
                if (response === false) {
                    $scope.Res = "Request failed";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis1 = false;
                    $scope.sResult = [];
                }
                else {

                   

                    $scope.Res = "Request sent successfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ngdis1 = false;
                    $scope.sResult = [];
                    $scope.ItemId=null
                    $scope.reset_reqform_multi();
                    //$scope.bindRejecteditems();
                    //$scope.bindClarificationitems();
                    $scope.rereq_btn = false;
                    $scope.add_button = true;

                    $('.fileinput').fileinput('clear');
                }
            }).error(function (data) {

                $scope.Res = data.errors;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.sResult = [];
            });
        };
       
        $scope.getgroup1 = function (SeviceCategorycode) {
            //  alert(SeviceCategorycode);
            $http.get('/ServiceMaster/getgroup?SeviceCategorycode=' + SeviceCategorycode
            ).success(function (response) {
                //alert('hi');
                $scope.getgroup1S = response;
                $scope.getgroup1S = $filter('filter')($scope.getgroup1S, { 'Islive': 'true' })
                // alert(angular.toJson($scope.shwusr1))
            }).error(function (data, status, headers, config) {
            });
        };

        //uom

        $http.get('/ServiceMaster/getuomlist').success(function (response) {
            //S  alert('hi');
            $scope.getuom1M = response;
            //alert(angular.toJson($scope.uomList1))
        });

        //$http.get('/ServiceMaster/showall_Uomuser'
        //    ).success(function (response) {
        //        //  alert('hi');
        //        $scope.getuomS = response;
        //        //alert(angular.toJson($scope.getuomS));
        //        $scope.getuomS = $filter('filter')($scope.getuomS, { 'Islive': 'true' })
        //        $scope.getuom1M = response;
        //        $scope.getuom1M = $filter('filter')($scope.getuom1M, { 'Islive': 'true' })
        //        //  alert(angular.toJson($scope.getcategory));
        //    }).error(function (data, status, headers, config) {
        //    });

        //approvercodename

        //$http({
        //    method: 'GET',
        //    url: '/ServiceMaster/get_approvercodename'
        //}).success(function (result) {
        //   // $scope.getapprover = result;
        //    $scope.getCleanser1 = result;
        //  //  alert(angular.toJson($scope.getapprover1));
        //});


        //multiplemaster
        //group under category

        $scope.getgrpreqM = function (SeviceCategorycode) {
            // alert(SeviceCategorycode);
            $http.get('/ServiceMaster/getgroup?SeviceCategorycode=' + SeviceCategorycode
            ).success(function (response) {
                //alert('hi');
                $scope.Commodities = response;
                // alert(angular.toJson($scope.group))
            }).error(function (data, status, headers, config) {
            });
        };

        //new request
        $scope.newRequest_SM = function () {
            $scope.show_loading_single = true;
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.ngdis2 = true;


            $scope.rows_single = [];
            $scope.rows_single.push({
                servicecategory: $scope.obj.SeviceCategorycode, servicegroup: $scope.obj.ServiceGroupcode,
                unitofmeasurement: $scope.obj.ServiceUomcode, Cleanser: $scope.Cleanserddl,
                servicediscription: $scope.obj.ServiceDiscription

            });

            var form_singlerequest = new FormData();
            form_singlerequest.append('Single_request', angular.toJson($scope.rows_single));
            // alert(angular.toJson($scope.rows_single));
            $scope.cgBusyPromises = $http({
                method: 'POST',
                url: '/ServiceMaster/newRequestService',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: form_singlerequest


            }).success(function (data) {
                if (data.success === false) {
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis2 = false;
                    $scope.show_loading_single = false;
                }
                else {
                    //alert('hai');

                    $scope.Res = "Request sent successfully";
                    // alert(angular.toJson($scope.Res))
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ngdis2 = false;
                    $scope.show_loading_single = false;
                    $scope.reset_reqform();

                }

            }).error(function (data) {
                $scope.Res = "Request failed";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.ngdis2 = false;
                $scope.show_loading_single = false;
            });
        };

        $scope.reset_reqform = function () {
            // alert('hi');
            // $scope.getcategory = [];
            $scope.obj.SeviceCategorycode = "";
            $scope.getgroup1S = [];
            $scope.obj.ServiceGroupcode = "";
            $scope.obj.ServiceUomcode = "";
            // $scope.getuomS = [];
            // $scope.getapprover = [];
            // $scope.approverddl = "";
            $scope.obj.ServiceDiscription = "";

            $scope.reqform.$setPristine();
        };

        $scope.reset_reqform_multi = function () {
            $scope.obj.SeviceCategorycode = "";
            $scope.group = [];
            $scope.obj.ServiceGroupcode = "";
            $scope.obj.ServiceUomcode = "";
            $scope.Cleanserddl = "";
            $scope.obj.ServiceDiscription1 = "";
            $scope.obj.PlantCode = "";
            $scope.obj.ServiceGroupCode = "";
            //$scope.obj.Reject_reason = "";
            //  $scope.getCleanser();
            $scope.reqform_multi.$setPristine();

        };

        //adding multiple rows

        $scope.rows = [];
        $scope.addrowtobulktable_SM = function () {

            var PlantName = $.grep($scope.getplant, function (plant) {
                return plant.Plantcode == $scope.obj.PlantCode;
            })[0].Plantname;

            var PlantCode = $scope.obj.PlantCode;

            var ServiceCategoryName = $("#ServiceCategorycodee1M").find("option:selected").text();
            var ServiceGroupName = $("#ServiceGroupcode1M").find("option:selected").text();
            var ServiceCategoryCode = $scope.obj.SeviceCategorycode;
            var ServiceGroupCode = $scope.obj.ServiceGroupCode;
            //alert(group);
            var UomCode = $scope.obj.ServiceUomcode;
            //$.grep($scope.getuom1M, function (uom1) {
            //    return uom1.ServiceUomcode == $scope.obj.ServiceUomcode;
            //})[0].ServiceUomname;

            var UomName = $scope.obj.ServiceUomcode;
            //alert(UomName);


            //var Cleanser1 = $.grep($scope.getCleanser1, function (Cleanser1) {
            //    return Cleanser1.Userid == $scope.Cleanserddl;
            //})[0].UserName;
            //var Cleanser = $scope.Cleanserddl;


            // alert(approver);
            $scope.rows.push({
                // Sequence: $scope.rows.length + 1, PlantName: $scope.obj.PlantCode, ServiceCategoryName: $scope.obj.SeviceCategorycode, ServiceGroupName: $scope.obj.ServiceGroupcode, UomName: $scope.obj.ServiceUomcode, Cleanser: Cleanser, Cleanser1: Cleanser1, Legacy: $scope.obj.ServiceDiscription1, hPlantName: PlantName, hServiceCategoryName: ServiceCategoryName, hServiceGroupName: ServiceGroupName, hUomName: UomName, hCleanser1: Cleanser1, hLegacy: $scope.obj.ServiceDiscription1
                // Sequence: $scope.rows.length + 1, PlantName: PlantName, PlantCode: PlantCode, ServiceCategoryName: ServiceCategoryName, ServiceCategoryCode: ServiceCategoryCode, ServiceGroupName: ServiceGroupName, ServiceGroupCode: ServiceGroupCode, UomName: UomName, UomCode: UomCode, Cleanser: Cleanser, Cleanser1: Cleanser1, Legacy: $scope.obj.ServiceDiscription1
                Sequence: $scope.rows.length + 1, PlantName: PlantName, PlantCode: PlantCode, ServiceCategoryName: ServiceCategoryName, ServiceCategoryCode: ServiceCategoryCode, ServiceGroupName: ServiceGroupName, ServiceGroupCode: ServiceGroupCode, UomName: UomName, UomCode: UomCode, Legacy: $scope.obj.ServiceDiscription1


            });

            $scope.reset_reqform_multi();
        };
        $scope.clear_fields1 = function () {
            $scope.reset();
            $scope.getcategory1M = "";
            $scope.group = "";
            $scope.getuom1M = "";
            $scope.getCleanser1 = "";
            $scope.ServiceDiscription1 = "";
            $scope.ServiceDiscription = "";


            //     $http.get('/ServiceMaster/showall_Categoryuser'
            //).success(function (response) {
            //    $scope.getcategory1M = response;
            //});

            //     $http.get('/ServiceMaster/getgroup?SeviceCategorycode=' + SeviceCategorycode
            // ).success(function (response) {
            //     $scope.group = response;
            // });

            $scope.getCleanser();
        };
        $scope.update_table = function () {
         

            var PlantName = $.grep($scope.getplant, function (plant) {
                return plant.Plantcode == $scope.obj.PlantCode;
            })[0].Plantname;

            var PlantCode = $scope.obj.PlantCode;

            var ServiceCategoryName = $("#ServiceCategorycodee1M").find("option:selected").text();

            var ServiceGroupName = $("#ServiceGroupcode1M").find("option:selected").text();

            var ServiceCategoryCode = $scope.obj.SeviceCategorycode;

            var ServiceGroupCode = $scope.obj.ServiceGroupCode;

            var UomName = $scope.obj.ServiceUomcode;
            //$scope.rows[$scope.tablerow1] = ({
            //    Sequence: $scope.tablerow1 + 1, PlantName: PlantName, PlantCode: PlantCode, ServiceCategoryName: ServiceCategoryName, ServiceCategoryCode: ServiceCategoryCode, ServiceGroupName: ServiceGroupName, ServiceGroupCode: ServiceGroupCode, UomName: UomName, UomCode: UomCode, Cleanser: Cleanser, Cleanser1: Cleanser1, Legacy: $scope.obj.ServiceDiscription1, hPlantName: PlantName, hServiceCategoryName: ServiceCategoryName, hServiceGroupName: ServiceGroupName, hUomName: UomName, Cleanser: Cleanser, hCleanser1: Cleanser1, hLegacy: $scope.obj.ServiceDiscription1
            //    //   Sequence: $scope.tablerow1 + 1, PlantName: PlantName, PlantCode: PlantCode, ServiceCategoryName: ServiceCategoryName, ServiceCategoryCode: ServiceCategoryCode, ServiceGroupName: ServiceGroupName, ServiceGroupCode: ServiceGroupCode, UomName: UOMNAME, UomCode: UOMNAME, Cleanser: Cleanser,Cleanser1: Cleanser1, Legacy: $scope.obj.ServiceDiscription1


            //});

            $scope.rows[$scope.tablerow1] = ({
                Sequence: $scope.rows.length, PlantName: PlantName, PlantCode: PlantCode, ServiceCategoryName: ServiceCategoryName, ServiceCategoryCode: ServiceCategoryCode, ServiceGroupName: ServiceGroupName, ServiceGroupCode: ServiceGroupCode, UomName: UomName, UomCode: UomName, Legacy: $scope.obj.ServiceDiscription1, hPlantName: PlantName, hServiceCategoryName: ServiceCategoryName, hServiceGroupName: ServiceGroupName, hUomName: UomName, hLegacy: $scope.obj.ServiceDiscription1

            });
            $scope.update_button = false;
            $scope.add_button = true;

            $scope.reset_reqform_multi();
        };


        // removerow_SM(indexx)
        $scope.removerow = function (index) {
            //  alert($scope.removerow);
            $scope.reset_reqform_multi();
            $scope.add_button = true;
            $scope.update_button = false;
            if ($scope.rows.length > 1) {
                if ($window.confirm("Please confirm to remove row?")) {
                    if ($scope.rows.length > 1) {
                        $scope.rows.length
                        $scope.rows.splice(index, 1);
                    }
                    var cunt = 1;
                    angular.forEach($scope.rows, function (value, key) {
                        value.Sequence = cunt++;
                    });
                }
                else {

                }
            }
            else {
                alert("You cant delete, better update this row");
            }
        };

        // update row _SM
        $scope.updaterow = function (index) {
            //if ($window.confirm("Please confirm to load Row to update?")) {

            //    $scope.add_button = false;
            //    $scope.update_button = true;

            //    $scope.tablerow1 = index;
            //    $scope.obj.SeviceCategorycode = $scope.rows[index].ServiceCategoryName;
            //    $scope.getgrpreqM($scope.rows[index].SeviceCategoryName);
            //    $scope.obj.ServiceGroupcode = $scope.rows[index].ServiceGroupName;
            //    $scope.obj.ServiceUomcode = $scope.rows[index].UomName;
            //    $scope.Cleanserddl = $scope.rows[index].Cleanser;
            //    $scope.obj.ServiceDiscription1 = $scope.rows[index].Legacy;
            //} else {

            //}

            if ($window.confirm("Please confirm to load Row to update?")) {

                $scope.add_button = false;
                $scope.update_button = true;
                $scope.tablerow1 = index;
                $scope.obj.PlantCode = $scope.rows[index].PlantCode;
                $scope.obj.SeviceCategorycode = $scope.rows[index].ServiceCategoryCode;
                $scope.obj.ServiceGroupCode = $scope.rows[index].ServiceGroupCode;
                //  $scope.getgrpreqM($scope.rows[index].ServiceGroupCode);
                $scope.obj.ServiceUomcode = $scope.rows[index].UomCode;
                $scope.Cleanserddl = $scope.rows[index].Cleanser;
                $scope.obj.ServiceDiscription1 = $scope.rows[index].Legacy;
            } else {

            }
        };
        // requesting bulk material items

        $scope.bulk_material_request_SM = function () {
            $scope.show_loading_multiple = true;
            $timeout(function () { $scope.NotifiyRes = false; }, 3000);
            $scope.ngdis1 = true;
            var formvalues = new FormData();
            formvalues.append('Mul_request', angular.toJson($scope.rows));
            $scope.cgBusyPromises = $http({
                url: "/ServiceMaster/bulk_material_request_SM",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formvalues
            }).success(function (data) {
                if (data.success === false) {
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis1 = false;
                    $scope.show_loading_multiple = false;
                }
                else {
                    $scope.reset_reqform_multi();
                    $scope.rows = [];
                    $scope.Res = "Request sent successfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ngdis1 = false;
                    $scope.show_loading_multiple = false;
                }

            }).error(function () {
                $scope.Res = "Error occured";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.show_loading_multiple = false;
            });
        };
        //onchangefile

        $scope.LoadFileData2 = function (files) {
            $timeout(function () {
                $scope.NotifiyRes = false;
            }, 30000);
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
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }
        };


        $scope.visiable = function () {
            //  alert("hai");
            $scope.makevisiable1 = true;
        };

        $scope.visiable1 = function () {
            //   alert("hai");
            $scope.makevisiable1 = false;
        };
        $scope.visiable3 = function () {
           
            $http({

                method: 'GET',
                url: '/ServiceMaster/getRejected_Records1'
            }).success(function (result) {
                $scope.rows_rej = result;
                $scope.rej_visible = false;
                // if($scope.rows.length > 0)
                //$scope.clickdatatoapprove(0);

            });
            //$scope.bindClarificationitems();
            $scope.makevisiable = false;
            $scope.Loadpop2();

        };
        $scope.visiable4 = function () {
          
            $http({

                method: 'GET',
                url: '/ServiceMaster/getclarification_Records'
            }).success(function (result) {
                $scope.rows_clr = result;
               
                //angular.forEach(result, function (value, key)
                //{
                   
                //    $scope.Req2.Remarks = value.Remarks;
                //})
                
                $scope.clr_visible = false;
                // if($scope.rows.length > 0)
                //$scope.clickdatatoapprove(0);

            });
            //$scope.bindClarificationitems();
            $scope.makevisiable = false;
            $scope.Loadpop2();

        };
        
        $scope.DelRequest = function (ItemId) {

            if (confirm("Are you sure, delete this record?")) {

                // $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/DelRequest1',
                    params: { ItemId: ItemId }
                }).success(function (response) {
                    if (response === true) {
                        $scope.Res = ItemId + " deleted";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.visiable3();
                    } else {
                        $scope.Res = ItemId + " delete failed";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.load_data_of_rejected = function (index) {

            $scope.cb_approve = true;
            $scope.cb_reject = false;
            $scope.rejecttxt_show = false;

            $scope.cataloguerlist = [];
            // $scope.getcataloguernames_id();
            var val = 0;
            if ($scope.rows_rej.length > 0) {
                //for (var res in $scope.rows_rej) {
                //    if (val === index) {
                //        $scope.rows_rej[val].RequestStatus = "active";
                //    }
                //    else {
                //        $scope.rows_rej[val].RequestStatus = "inactive";
                //    }
                //    val = val + 1;
                //}

                $http({
                    method: 'GET',
                    url: 'getsingle_requested_record',
                    params: { abcsony: $scope.rows_rej[index].ItemId }
                }).success(function (result) {

                    $scope.Req2 = {};
                    $scope.Req2.ItemId = result[0].ItemId;
                    $scope.Req2.Reject_reason = result[0].Reject_reason;
                    $scope.Req2.RequestId = result[0].RequestId;

                    $scope.Req2.PlantName = result[0].PlantCode + ' / ' + result[0].PlantName;

                    $scope.Req2.ServiceCategoryName = result[0].ServiceCategoryName;
                    // $scope.Req1.ServiceCategoryName = result[0].ServiceCategoryName;


                    $scope.Req2.ServiceGroupName = result[0].ServiceGroupName;

                    $scope.Req2.UomName = result[0].UomName;
                    $scope.Req2.Legacy = result[0].Legacy;
                    // alert(angular.toJson(result[0].requester.Name));

                    // $scope.Req.requester.Name = result[0].requester.Name;
                    //alert(angular.toJson($scope.Req.requester.Name));
                    //$scope.Req.Legacy = result[0].Legacy;
                    ////$scope.Req.requestedOn = result[0].requestedOn;
                    $scope.Req2.approver = result[0].approver;
                    //$scope.Req.Materialtype = result[1].Materialtype + ' / ' + result[0].Materialtype;
                    //$scope.Req.Industrysector = result[1].Industrysector + ' / ' + result[0].Industrysector;
                    //$scope.Req.MaterialStrategicGroup = result[1].MaterialStrategicGroup + ' / ' + result[0].MaterialStrategicGroup;
                    //$scope.Req.UnitPrice = result[0].UnitPrice;
                    //$scope.Req.attachment = result[0].attachment;
                    $scope.rej_visible = true;
                });
            }
            else {
                $scope.rej_visible = false;
                $scope.Req2 = {};
                $scope.Req2.ItemId = "";
                $scope.Req2.RequestId = "";
                $scope.Req2.PlantName = "";
                $scope.Req2.ServiceCategoryName = "";
                $scope.Req2.ServiceGroupName = "";
                $scope.Req2.UomName = "";
                $scope.Req2.requester.Name = "";
                $scope.Req2.Legacy = "";
                $scope.Req2.requestedOn = "";
                $scope.Req2.approver = "";
                //$ = "";
                //$scope.Req.Industrysector = "";
                //$scope.Req.MaterialStrategicGroup = "";
                //$scope.Req.UnitPrice = "";
                //$scope.Req.attachment = "";
            }
        };


        $scope.load_data_of_clarification = function (index) {

            $scope.cb_approve = true;
            $scope.cb_reject = false;
            $scope.rejecttxt_show = false;

            $scope.cataloguerlist = [];
            // $scope.getcataloguernames_id();
            var val = 0;
            if ($scope.rows_clr.length > 0) {
                //for (var res in $scope.rows_rej) {
                //    if (val === index) {
                //        $scope.rows_rej[val].RequestStatus = "active";
                //    }
                //    else {
                //        $scope.rows_rej[val].RequestStatus = "inactive";
                //    }
                //    val = val + 1;
                //}

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/getsingle_requested_record',
                    params: { abcsony: $scope.rows_clr[index].ItemId }
                }).success(function (result) {
                   
                    $scope.Req2 = {};
                    $scope.Req2.ItemId = result[0].ItemId;
                    $scope.Req2.Remarks = result[0].Remarks;
                    $scope.Req2.RequestId = result[0].RequestId;

                    $scope.Req2.PlantName = result[0].PlantCode + ' / ' + result[0].PlantName;

                    $scope.Req2.ServiceCategoryName = result[0].ServiceCategoryName;
                    // $scope.Req1.ServiceCategoryName = result[0].ServiceCategoryName;


                    $scope.Req2.ServiceGroupName = result[0].ServiceGroupName;

                    $scope.Req2.UomName = result[0].UomName;
                    $scope.Req2.Legacy = result[0].Legacy;
                   
                    $scope.Req2.Cleanser = result[0].Cleanser;
                    
                    $scope.clr_visible = true;
                   
                });
            }
            else {
                $scope.clr_visible = false;
                $scope.Req2 = {};
                $scope.Req2.ItemId = "";
                $scope.Req2.RequestId = "";
                $scope.Req2.PlantName = "";
                $scope.Req2.ServiceCategoryName = "";
                $scope.Req2.ServiceGroupName = "";
                $scope.Req2.UomName = "";
                $scope.Req2.requester.Name = "";
                $scope.Req2.Legacy = "";
                $scope.Req2.Clarification_On = "";
                $scope.Req2.Cleanser = "";
                //$ = "";
                //$scope.Req.Industrysector = "";
                //$scope.Req.MaterialStrategicGroup = "";
                //$scope.Req.UnitPrice = "";
                //$scope.Req.attachment = "";
            }
        };
        $scope.clickdatatoapprove = function (index) {

            $scope.cb_approve = true;
            $scope.cb_reject = false;
            $scope.rejecttxt_show = false;

            $scope.cataloguerlist = [];
            // $scope.getcataloguernames_id();
            var val = 0;
            if ($scope.rows_app.length > 0) {
                //for (var res in $scope.rows_app) {
                //    if (val === index) {
                //        $scope.rows_app[val].RequestStatus = "active";
                //    }
                //    else {
                //        $scope.rows_app[val].RequestStatus = "inactive";
                //    }
                //    val = val + 1;
                //}

                $http({
                    method: 'GET',
                    url: 'getsingle_requested_record',
                    params: { abcsony: $scope.rows_app[index].ItemId }
                }).success(function (result) {
                    //alert(angular.toJson(result))
                    $scope.Req1 = {};
                    $scope.Req1.ItemId = result[0].ItemId;

                    $scope.Req1.RequestId = result[0].RequestId;

                    $scope.Req1.PlantName = result[0].PlantCode + ' / ' + result[0].PlantName;

                    $scope.Req1.ServiceCategoryName = result[0].ServiceCategoryName;
                    // $scope.Req1.ServiceCategoryName = result[0].ServiceCategoryName;


                    $scope.Req1.ServiceGroupName = result[0].ServiceGroupName;

                    $scope.Req1.UomName = result[0].UomName;
                    $scope.Req1.Legacy = result[0].Legacy;
                    // alert(angular.toJson(result[0].requester.Name));

                    // $scope.Req.requester.Name = result[0].requester.Name;
                    //alert(angular.toJson($scope.Req.requester.Name));
                    //$scope.Req.Legacy = result[0].Legacy;
                    ////$scope.Req.requestedOn = result[0].requestedOn;
                    $scope.Req1.approver = result[0].approver;
                    //$scope.Req.Materialtype = result[1].Materialtype + ' / ' + result[0].Materialtype;
                    //$scope.Req.Industrysector = result[1].Industrysector + ' / ' + result[0].Industrysector;
                    //$scope.Req.MaterialStrategicGroup = result[1].MaterialStrategicGroup + ' / ' + result[0].MaterialStrategicGroup;
                    //$scope.Req.UnitPrice = result[0].UnitPrice;
                    //$scope.Req.attachment = result[0].attachment;
                    $scope.app_visible = true;
                });
            }
            else {
                $scope.app_visible = false;
                $scope.Req1 = {};
                $scope.Req1.ItemId = "";
                $scope.Req1.RequestId = "";
                $scope.Req1.PlantName = "";
                $scope.Req1.ServiceCategoryName = "";
                $scope.Req1.ServiceGroupName = "";
                $scope.Req1.UomName = "";
                $scope.Req1.requester.Name = "";
                $scope.Req1.Legacy = "";
                $scope.Req1.requestedOn = "";
                $scope.Req1.approver = "";
                //$ = "";
                //$scope.Req.Industrysector = "";
                //$scope.Req.MaterialStrategicGroup = "";
                //$scope.Req.UnitPrice = "";
                //$scope.Req.attachment = "";
            }
        };
        $scope.editRequest = function (ItemId) {


            $http({
                method: 'GET',
                url: '/ServiceMaster/getsingle_requested_record',
                params: { abcsony: ItemId }
            }).success(function (result) {

                $scope.rereq_btn = true;
                $scope.add_button = false;


                $('#sig').attr('class', 'active');
                $('#a1').attr('aria-expanded', 'true');

                $('#rej').attr('class', '');
                $('#a3').attr('aria-expanded', 'false');

                $('#clari').attr('class', '');
                $('#a4').attr('aria-expanded', 'false');

                $('#single').attr('class', 'tab-pane active');
                $('#rejected').attr('class', 'tab-pane');
                $('#clarification').attr('class', 'tab-pane');

                //alert(angular.toJson(result[0].Legacy))
                $scope.obj = {}
                $scope.ItemId = result[0].ItemId;

                $scope.Remarks = result[0].Reject_reason;
              
                $scope.obj.ServiceDiscription1 = result[0].Legacy;
                
                $scope.obj.PlantCode = result[0].PlantCode;
               
                // $scope.getStoragecode_name1();
                $scope.obj.SeviceCategorycode = result[0].ServiceCategoryCode;
                
                $scope.obj.ServiceGroupCode = result[0].ServiceGroupCode;
               // alert(angular.toJson($scope.obj.ServiceGroupCode))
                //   $scope.getsubgroupCode_Name1();
                $scope.obj.ServiceUomcode = result[0].UomName;
                //alert(angular.toJson($scope.obj.ServiceUomcode))
                

              
                //alert(angular.toJson($scope.obj.ServiceUomcode))
                //alert(angular.toJson($scope.obj.ServiceDiscription1))
                //  $scope.Reject_reason = result[0].Reject_reason;
                //$scope.Materialtype = result[0].Materialtype;
                //$scope.Industrysector = result[0].Industrysector;
                //$scope.MaterialStrategicGroup = result[0].MaterialStrategicGroup;

            });
        }
        $scope.editclarRequest = function (ItemId) {


            $http({
                method: 'GET',
                url: '/ServiceMaster/getsingle_requested_record',
                params: { abcsony: ItemId }
            }).success(function (result) {

                $scope.rereq_btn = true;
                $scope.add_button = false;


                $('#sig').attr('class', 'active');
                $('#a1').attr('aria-expanded', 'true');

                $('#rej').attr('class', '');
                $('#a3').attr('aria-expanded', 'false');

                $('#clari').attr('class', '');
                $('#a4').attr('aria-expanded', 'false');

                $('#single').attr('class', 'tab-pane active');
                $('#rejected').attr('class', 'tab-pane');
                $('#clarification').attr('class', 'tab-pane');

                $scope.obj = {}
                $scope.ItemId = result[0].ItemId;

                $scope.Remarks = result[0].Remarks;

                $scope.obj.ServiceDiscription1 = result[0].Legacy;

                $scope.obj.PlantCode = result[0].PlantCode;

             
                $scope.obj.SeviceCategorycode = result[0].ServiceCategoryCode;

                $scope.obj.ServiceGroupCode = result[0].ServiceGroupCode;

                $scope.obj.ServiceUomcode = result[0].UomName;
            

            });
        }
        //$scope.clickdatatoapprove();
        $scope.visiable2 = function () {

            $http({

                method: 'GET',
                url: '/ServiceMaster/getApproved_Records1'
            }).success(function (result) {
                $scope.rows_app = result;
                // if($scope.rows.length > 0)
                //$scope.clickdatatoapprove(0);

            });

            $scope.makevisiable = false;
            $scope.Loadpop3();
        };

        //$scope.visiable4 = function () {
        //    //   alert("hai");
        //    $scope.makevisiable = false;
        //    $scope.Loadpop4();
        //};

        $scope.Loadpop2 = function () {

            new jBox('Tooltip', {
                attach: '#showstatus1',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }
        $scope.Loadpop3 = function () {
            //   alert("hai");
            new jBox('Tooltip', {
                attach: '#showstatus2',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }
        $scope.Loadpop4 = function () {
            //   alert("hai");
            new jBox('Tooltip', {
                attach: '#showstatus3',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }
        $scope.Loadfile2 = function () {
            // alert("hai");
            if ($scope.files != null && $scope.files != undefined && $scope.files != "") {
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 10000);
                $timeout(function () { $scope.NotifiyRes1 = false; }, 5000);
                var formData = new FormData();
                formData.append('file', $scope.files[0]);
                $scope.cgBusyPromises = $http({
                    url: "/ServiceMaster/Load_Data_for_bulkupload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {

                    if (response == "Check File Format") {
                        $scope.Res = "Check file format";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else if (response == "Uploaded File is Empty") {
                        $scope.Res = "Uploaded File is Emplty";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    }
                    else if (response != null) {
                        $scope.hidetb2 = false;
                        $scope.sResultbulk1 = response;

                        //   alert(angular.toJson($scope.sResultbulk1));

                    }

                }).error(function (data, status, headers, config) {

                });
                //} else {
                //    if ($scope.GetLD.length > 0) {
                //        $scope.Res = "Data loaded";
                //        $scope.Notify = "alert-info";
                //        $scope.NotifiyRes = true;
                //        $('#divNotifiy').attr('style', 'display: block');
                //        $scope.load = true;
                //        $scope.scrub = true;
                //        $scope.imp = true;

                //    } else {
                //        $scope.Res = "Load valid excel file";
                //        $scope.Notify = "alert-danger";
                //        $scope.NotifiyRes = true;
                //        $('#divNotifiy').attr('style', 'display: block');
                //        $scope.load = true;
                //        $scope.scrub = true;
                //        $scope.imp = true;
                //    }
                //}
            }

            else {
                $scope.Res = "Select file to be uploaded";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                //alert("hai");
                //$scope.files = "";

            }
        };
        $scope.bulkupload_service_request = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.ngdis = true;
            var formvalues = new FormData();
            formvalues.append('sResultbulk1', angular.toJson($scope.sResultbulk1));
            $scope.cgBusyPromises = $http({
                url: "/ServiceMaster/bulkupload_service_request",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formvalues
            }).success(function (data) {
                if (data === false) {
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                }
                else if (data === true) {
                    // $scope.reset_reqform_multi();
                    $scope.sResultbulk1 = [];
                    $scope.hidetb2 = true;
                    $scope.Res = "Request sent successfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                    $('.fileinput').fileinput('clear');
                    //  files[0] = null;
                    // $scope.sResultbulk1 = true;
                    // $scope.hidetb2 = false;

                }
                else {
                    $scope.Res = data;
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                }

            }).error(function () {
                $scope.Res = "Error occured";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
                $scope.hidetb2 = true;
                // $scope.show_loading_multiple = false;
            });
        };

        //$scope.showuserMap = function (itmcode) {

        //    $http({
        //        method: 'GET',
        //        url: '/User/getItemstatusMap',
        //        params: { itmcde: itmcode }
        //    }).success(function (response) {
        //        $scope.itemstatusLst = response;

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });
        //}

    });
    // var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter']);
    app.controller('app_controller1', function ($scope, $window, $http, $rootScope, $timeout, $filter) {

        $scope.sResultbulk1 = [];
        $scope.hidetb2 = true;
        $scope.getplant = [];
        $scope.getcategory = [];
        $scope.getgroup1S = [];
        $scope.getuomS = [];
        $scope.getapprover = [];

        $scope.getplant = [];
        $scope.getcategory1M = [];
        $scope.group = [];
        $scope.getuom1M = [];
        $scope.getCleanser1 = [];

        $scope.add_button = true;
        $scope.update_button = false;

        $scope.tablerow1 = 0;
        $scope.ngdis2 = false;
        $scope.ngdis1 = false;
        $scope.ngdis = false;

        $scope.show_loading_multiple = false;
        $scope.show_loading_single = false;


        new jBox('Tooltip', {
            attach: '#showstatus',
            //width: 400,
            //height: 500,                   
            closeButton: true,
            //animation: 'zoomIn',
            theme: 'TooltipBorder',
            trigger: 'click',
            width: 600,
            height: 240,
            adjustTracker: true,
            closeOnClick: 'body',
            closeOnEsc: true,
            animation: 'move',
            //position: {
            //    x: 'center',
            //    y: 'center'
            //},
            outside: 'y',
            content: jQuery('#flowconId')
        });
        $scope.Loadpop = function () {
          
            new jBox('Tooltip', {
                attach: '#showstatus',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }

        $scope.Loadpop1 = function () {
            new jBox('Tooltip', {
                attach: '#showstatus1',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }
        $scope.Loadpop2 = function () {
            new jBox('Tooltip', {
                attach: '#showstatus2',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 240,
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#flowconId')
            });
        }


    })




})();





