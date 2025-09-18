(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter']);
    app.controller('ServiceCreationController', function ($scope, $http, $rootScope, $timeout, $filter, $location, $window) {
        $scope.legacyy = "";
        $scope.DataList = [];
        $scope.getcatSC = [];
        $scope.getgroupSC = [];
        $scope.getUOMSC = [];
        $scope.getVALSC = [];
        $scope.getmainSC = [];
        $scope.getsubSC = [];
        $scope.Attributess = null;
        $scope.getsubsubSC = [];
        $scope.long = "";
        $scope.short = "";
        $scope.sc = "";
        $scope.rid = "";
        $scope._id = "";
        $scope.Reviewer_list = [];
        $scope.UserId = "";
        $scope.Showsubmit = false;
        $scope.showlist = false;
        $scope.obj = 
        $scope.obj1 = { "UserId": "", "Name": "" };

        ///find default attribute
        $scope.defatt = "";
        $scope.hidesubmit = false;

        //$scope.obj1.Name = "";
        //$scope.obj1.UserId = "";


        //$http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
        //    $scope.getcatSC = catagory;
        //});
        //$scope.SelectAttr = function (Attribute) {


        //    $http({
        //        method: 'GET',
        //        url: '/ServiceMaster/GetValues?Attribute=' + Attribute 
        //    }).success(function (response) {
        //        $scope.Values = response;
        //        //alert(angular.toJson($scope.Values));
        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });     


        //};

        //$http({
        //    method: 'GET',
        //    url: '/GeneralSettings/GetUnspsc',
        //    params: { Noun: 'Service', Modifier: 'Service' }
        //}).success(function (response) {

        //    if (response != '') {
        //        $scope.Commodities = response;
        //        //if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
        //        //    $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
        //        //else $scope.cat.Unspsc = $scope.Commodities[0].Class;
        //    }
        //    else {
        //        $scope.Commodities = null;
        //    }

        //}).error(function (data, status, headers, config) {
        //    // alert("error");

        //});

        $scope.GetCodeLogic = function () {
            $http({
                method: 'GET',
                url: '/ServiceMaster/Showdatacodelogic'
            }).success(function (response) {

                $scope.CLogic = response;
                if ($scope.CLogic === "Customized Code")
                    $scope.CLogic_maincode = true;
                else
                    $scope.CLogic_maincode = false;
                if ($scope.CLogic === "UNSPSC Code")
                    $scope.CLogic_unspsc = true;
                else
                    $scope.CLogic_unspsc = false;
             //   alert(angular.toJson($scope.CLogic_maincode));

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

        $scope.GetCodeLogic();

        //maincode
        $scope.BindGroupcodeList = function () {
          // alert("maincode");
          //  alert(angular.toJson($scope.obj.ServiceCategorycode));
            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_MainCode1?catagory=' + $scope.obj.ServiceCategorycode
            }).success(function (response) {
                $scope.getmainSC = response;
                //alert(angular.toJson(response))
                /////
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindGroupcodeList();


        $scope.SelectAttr = function (Noun,Modifier, Attribute) {
            //  alert("hai");

            $http({
                method: 'GET',
                url: '/ServiceMaster/GetValues?Noun=' + Noun +'&Modifier='+Modifier +'&Attribute=' + Attribute
             //   obj.SubCode
            }).success(function (response) {
                $scope.Values = response;
                //alert(angular.toJson($scope.Values));
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };

        ////checkvalue

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {
            //  alert(Value);
            if (Value != null && Value != '') {

                var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value });

                if (res != null && res != '') {

                    if (res[0].KeyAttribute === Attribute && res[0].KeyValue === Value) {


                        angular.forEach($scope.Attributess, function (value2, key2) {

                            angular.forEach(res[0].Attributess, function (value1, key1) {

                                if (value1.Attributes === value2.Attributes) {

                                    value2.Value = value1.Value;

                                }

                            })
                        });


                    }
                }

                //  alert("in");
                $http({
                    method: 'GET',
                    url: '/ServiceMaster/CheckValue?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value
                }).success(function (response) {
                    if (response == 'false') {
                        //


                    //    alert(angular.toJson(response));

                        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
                        $('#checkval' + indx).attr('style', 'display:block');
                        $scope.Attributess[indx].Abbrivation = " ";

                        ///
                     //   $('#checkval' + indx).attr('style', 'display:block');

                    }
                    else {
                     //  alert("ab");
                        $('#btnabv' + indx).attr('style', 'display:none');
                        $('#checkval' + indx).attr('style', 'display:block');
                        $scope.Attributess[indx].Abbrivation = response;

                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                //  alert(angular.toJson($scope.Attributess));

            } else {

                $('#checkval' + indx).attr('style', 'display:none');
            }
        };

        //addvalue

        $scope.AddValue1 = function (Noun,Modifier, Attribute, Value, abb, indx) {
            $http({
                method: 'GET',
                url: '/ServiceMaster/AddValue1',
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: abb }
                //?SubCode=' + Activity + '&Attribute=' + Attribute + '&Value=' + Value + '&abb=' + abb
            }).success(function (response) {

                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:block');

                ////////////
                //$('#checkval' + indx).attr('style', 'display:none');
                //$scope.Attributess[indx].Abbrivation = null;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.CancelValue1 = function (indx) {
            $('#checkval' + indx).attr('style', 'display:none');
            $scope.Attributess[indx].Abbrivation = null;
        }

        //loadreviewer

        $http.get('/ServiceMaster/LoadReviewer').success(function (reviewer) {
            $scope.Reviewer_list = reviewer;
        });

        //$http.get('/ServiceMaster/showall_Uomuser').success(function (uom) {
        //    $scope.getUOMSC = uom;
        //});

        //$http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
        //    $scope.getVALSC = valsc;
        //});      

        //$scope.balanceitems = 0;
        //$scope.saveditems = 0;

        $scope.getdatalistforCleanser = function () {
            $rootScope.cgBusyPromises = $http.get('/ServiceMaster/getdatalistforCleanser'
           ).success(function (response) {
               $scope.DataList = response;
              
               $scope.tot = response.length;
               $scope.saveditems = ($filter('filter')($scope.DataList, { 'ServiceStatus': 'Cleansed' })).length;
               $scope.balanceitems = ($filter('filter')($scope.DataList, { 'ServiceStatus': 'Cleanse' })).length;

               $scope.showlist = true;
               //////////12.04.2019 BLOCK
               //if ($scope.DataList == "" || $scope.DataList == null || $scope.DataList == "undefinded") {
               //    $scope.showlist = false;

               //}
               //else {
               //    $scope.sc = $scope.DataList[0].ServiceCode;
               //    $scope.rid = $scope.DataList[0].RequestId;
               //    $scope._id = $scope.DataList[0]._id;
               //    $scope.obj = $scope.DataList[0];

               //    $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope.DataList[0]["_id"]).success(function (response) {

               //        $scope.values1 = response;
               //        // alert(angular.toJson(response));
               //        $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
               //            $scope.getcatSC = catagory;
               //        });

               //        $http.get('/ServiceMaster/LoadReviewer').success(function (reviewer) {
               //            $scope.Reviewer_list = reviewer;
               //        });

               //        //$http.get('/ServiceMaster/showall_Uomuser').success(function (uom) {
               //        //    $scope.getUOMSC = uom;
               //        //});

               //        $http.get('/ServiceMaster/getuomlist').success(function (uom) {
               //            //alert('hi');
               //            $scope.getUOMSC = uom;
               //            // alert(angular.toJson($scope.getUOMSC))
               //        });

               //        $http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
               //            $scope.getVALSC = valsc
               //        });


               //        $http.get('/Master/GetDataListplnt'
               //    ).success(function (response) {
               //        $scope.getplant1 = response;
               //        $scope.getplant1 = $filter('filter')($scope.getplant1, { 'Islive': 'true' })

               //    }).error(function (data, status, headers, config) {

               //    });

               //        if ($scope.values1.ServiceCategoryCode != null) {
               //            $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.values1.ServiceCategoryCode).success(function (maincode) {
               //                $scope.getmainSC = maincode;
               //            });
               //        }

               //        var i = 0;
               //        angular.forEach($scope.DataList, function (lst1) {
               //            $('#' + i).attr("style", "");
               //            i++;
               //        });
               //        $('#0').attr("style", "background-color:lightblue");


               //        // alert(angular.toJson($scope.values1.PlantCode));
               //        if ($scope.values1.PlantCode != null && $scope.values1.PlantCode != "" && $scope.values1.PlantCode != "undefined") {
               //            // alert("hi");
               //            $scope.obj.PlantCode = $scope.values1.PlantCode;
               //        }

               //        if ($scope.values1.Legacy != null) {
               //            // alert("hi");
               //            $scope.obj.legacyy = $scope.values1.Legacy;
               //        }

               //        if ($scope.values1.ServiceCategoryCode != null)
               //            $scope.obj.ServiceCategorycode = $scope.values1.ServiceCategoryCode;

               //        if ($scope.values1.ServiceCategoryCode != null) {
               //            $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + $scope.values1.ServiceCategoryCode).success(function (response) {
               //                $scope.getgroupSC = response;
               //            });
               //        }

               //        //   alert(angular.toJson($scope.values1.Reviewer));
               //        if ($scope.values1.Reviewer != null)
               //            $scope.UserId = $scope.values1.Reviewer.UserId;
               //        else
               //            $scope.UserId = null;

               //        if ($scope.values1.ServiceGroupCode != null)
               //            $scope.obj.ServiceGroupcode = $scope.values1.ServiceGroupCode;


               //        //   $scope.getattibuteTable($scope.obj.ServiceCategorycode, $scope.obj.ServiceGroupcode);

               //        if ($scope.values1.UomCode != null)
               //            $scope.obj.UomCode = $scope.values1.UomCode;
               //        else
               //            $scope.obj.UomCode = "";

               //        if ($scope.values1.ShortDesc != null)
               //            $scope.obj.short = $scope.values1.ShortDesc;
               //        else
               //            $scope.obj.short = "";

               //        if ($scope.values1.LongDesc != null)
               //            $scope.obj.long = $scope.values1.LongDesc;
               //        else
               //            $scope.obj.long = "";


               //        if ($scope.values1.Valuationcode != null)
               //            $scope.obj.Valuationcode = $scope.values1.Valuationcode;
               //        else
               //            $scope.obj.Valuationcode = "";



               //        if ($scope.values1.MainCode != null)
               //            $scope.obj.MainCode = $scope.values1.MainCode;
               //        else
               //            $scope.obj.MainCode = "";

               //        if ($scope.values1.SubCode != null) {

               //            $http.get('/ServiceMaster/getSubList?MainCode=' + $scope.values1.MainCode
               //             ).success(function (subsc) {
               //                 $scope.getsubSC = subsc;
               //             }).error(function (data, status, headers, config) {
               //             });
               //            $scope.obj.SubCode = $scope.values1.SubCode;
               //        }
               //        else {
               //            $scope.obj.SubCode = "";
               //        }


               //        // FOR LOSDING ATTRIBUTES




               //        if ($scope.values1.SubSubCode != null) {
               //            $http.get('/ServiceMaster/GetSubSubList?MainCode=' + $scope.values1.MainCode + '&SubCode=' + $scope.values1.SubCode
               //                    ).success(function (subsub) {
               //                        $scope.getsubsubSC = subsub;
               //                        // $scope.obj.SubSubCode = "";
               //                    }).error(function (data, status, headers, config) {
               //                    });
               //            $scope.obj.SubSubCode = $scope.values1.SubSubCode;
               //        }
               //        else {
               //            $scope.obj.SubSubCode = "";
               //        }



               //        //      alert("load");
               //        if ($scope.values1.Characteristics != null && $scope.values1.Characteristics != "") {
               //            $scope.Attributess = $scope.values1.Characteristics;
               //        }
               //        else {

               //            $http.get('/ServiceMaster/GetMainSubAttributeTable?MainCode=' + $scope.obj.ServiceCategorycode + '&SubCode=' + $scope.obj.ServiceGroupcode
               //       ).success(function (response) {
               //           if (response != false) {
               //               $scope.Attributess = response;
               //           }
               //           else {
               //               $scope.Attributess = null;
               //           }
               //       }).error(function (data, status, headers, config) {
               //       });

               //        }

               //        if ($scope.values1.Remarks != null && $scope.values1.Remarks != "") {
               //            $scope.obj.Remarks = $scope.values1.Remarks;
               //        }
               //        else {
               //            $scope.obj.Remarks = "";
               //        }

               if ($scope.DataList.ServiceStatus === "Cleanse") {
                           $scope.Showsubmit = true;
                       }
               else if ($scope.DataList.ServiceStatus === "Cleansed") {

                           $scope.Showsubmit = true;
                       }
                       else {
                          // alert("data already submitted");
                           $scope.showsubmit = false;
                       }

                       ///////////12.04.2019/end


                       //    $scope.values1 = response;
                       //    $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
                       //        $scope.getcatSC = catagory;
                       //    });

                       //    $http.get('/ServiceMaster/showall_Uomuser').success(function (uom) {
                       //        $scope.getUOMSC = uom;
                       //    });

                       //    $http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
                       //        $scope.getVALSC = valsc
                       //    });

                       //    $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.DataList[0].ServiceCategoryCode).success(function (maincode) {
                       //        $scope.getmainSC = maincode;
                       //    });

                       //    $http.get('/Master/GetDataListplnt'
                       //).success(function (response) {
                       //    $scope.getplant1 = response;
                       //    $scope.getplant1 = $filter('filter')($scope.getplant1, { 'Islive': 'true' })

                       //}).error(function (data, status, headers, config) {

                       //});


                       //}

                       //var i = 0;
                       //angular.forEach($scope.DataList, function (lst1) {
                       //    $('#' + i).attr("style", "");
                       //    i++;
                       //});
                       // $('#0').attr("style", "background-color:lightblue");

                       //alert(angular.toJson($scope.values1[0]));
                       //if ($scope.values1.Legacy != null) {
                       //    // alert("hi");
                       //    $scope.obj.legacyy = $scope.values1[0].Legacy;
                       //}

                       //if ($scope.values1.ServiceCategoryCode != null)
                       //    $scope.obj.ServiceCategorycode = $scope.values1.ServiceCategoryCode;

                       //if ($scope.values1.ServiceCategoryCode != null) {
                       //    $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + $scope.values1.ServiceCategoryCode).success(function (response) {
                       //        $scope.getgroupSC = response;
                       //    });
                       //}

                       //if ($scope.values1.ServiceGroupCode != null)
                       //    $scope.obj.ServiceGroupcode = $scope.values1.ServiceGroupCode;

                       //if ($scope.values1.UomCode != null)
                       //    $scope.obj.UomCode = $scope.values1.UomCode;
                       //else
                       //    $scope.obj.UomCode = "";

                       //if ($scope.values1.ShortDesc != null)
                       //    $scope.obj.short = $scope.values1.ShortDesc;
                       //else
                       //    $scope.obj.short = "";

                       //if ($scope.values1.LongDesc != null)
                       //    $scope.obj.long = $scope.values1.LongDesc;
                       //else
                       //    $scope.obj.long = "";


                       //if ($scope.values1.Valuationcode != null)
                       //    $scope.obj.Valuationcode = $scope.values1.Valuationcode;
                       //else
                       //    $scope.obj.Valuationcode = "";

                       //if ($scope.values1.MainCode != null)
                       //    $scope.obj.MainCode = $scope.values1.MainCode;
                       //else
                       //    $scope.obj.MainCode = "";

                       //if ($scope.values1.SubCode != null) {

                       //    $http.get('/ServiceMaster/getSubList?MainCode=' + $scope.values1.MainCode
                       //     ).success(function (subsc) {
                       //         $scope.getsubSC = subsc;
                       //     }).error(function (data, status, headers, config) {
                       //     });
                       //    $scope.obj.SubCode = $scope.values1.SubCode;
                       //}
                       //else {
                       //    $scope.obj.SubCode = "";
                       //}

                       //if ($scope.values1.SubSubCode != null) {
                       //    $http.get('/ServiceMaster/GetSubSubList?MainCode=' + $scope.values1.MainCode + '&SubCode=' + $scope.values1.SubCode
                       //            ).success(function (subsub) {
                       //                $scope.getsubsubSC = subsub;
                       //            }).error(function (data, status, headers, config) {
                       //            });
                       //    $scope.obj.SubSubCode = $scope.values1.SubSubCode;
                       //}
                       //else {
                       //    $scope.obj.SubSubCode = "";
                       //}

                       //if ($scope.values1.Reviewer != null) {
                       //    $scope.UserId = $scope.values1.Reviewer.UserId;
                       //    //  alert($scope.values1.Reviewer);
                       //}
                       //else {
                       //    $scope.UserId = "";
                       //}



                       //if ($scope.values1.servicestatus === "cleanse") {
                       //    $scope.showsubmit = true;
                       //}
                       //else {
                       //    alert("data already submitted");
                       //    $scope.showsubmit = false;
                       //}
                       //if ($scope.values1.ServiceStatus === "Cleansed") {
                       //    $scope.Showsubmit = true;
                       //}
                  ///////////////////////
                  // });
               //}
           }).error(function (data, status, headers, config) {
           });
        };

        $scope.getdatalistforCleanser();

        //subcode
        $scope.getsubcodeSC = function (MainCode) {
            $http.get('/ServiceMaster/getSubList?MainCode=' + MainCode
            ).success(function (response) {
                // $scope.getmainSC = response;
                $scope.getsubSC = response;
                $scope.obj.SubCode = "";
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getsubsubcodesc = function (MainCode, SubCode) {
            // alert("hai");
            $http.get('/ServiceMaster/GetSubSubList?MainCode=' + MainCode + '&SubCode=' + SubCode
        ).success(function (response) {

            //   alert("hi");
            //  if (response != null) {
            // alert("if");
            $scope.getsubsubSC = response;
            $scope.obj.SubSubCode = "";
            //  }
            //  else {
            //   alert("else");
            //   $scope.obj.SubSubCode = "";
            //   }
        }).error(function (data, status, headers, config) {
        });

        };

        $scope.rejectitem = function () {
            $http.get('/ServiceMaster/rejectitem?_id=' + $scope._id + '&rejectedas=Cleanser&Remarks=' + $scope.obj.Remarks
            ).success(function (response) {
                $rootScope.Res = "Service has been rejected";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                $scope.Showsubmit = false;

            }).error(function (data, status, headers, config) {
            });
        };

        $scope.getsubsubsubcodesc = function () {
            //alert(angular.toJson($scope.obj.SubSubCode));
            if ($scope.CLogic === "Customized Code") {
                if ($scope.obj.SubSubCode === "" || $scope.obj.SubSubCode === null || $scope.obj.SubSubCode === undefined) {
                    $scope.subsub = "red";
                }
                else {
                    $scope.subsub = "";
                }
            }
            else
            {

                if ($scope.CLogic === "UNSPSC Code") {
                    if ($scope.obj.Unspsc === "" || $scope.obj.Unspsc === null || $scope.obj.Unspsc === undefined) {
                        $scope.unspsccom = "red";
                    }
                    else {
                        $scope.unspsccom = "";
                    }
                }

            }

            
        };

        $scope.closeData1 = function () {
            $scope.dis = false;
            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;
        };

        $scope.docheck = function () {
         
            var formData = new FormData();
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();
            var i = 1;
          
            angular.forEach($scope.Attributess, function (lst1) {
                if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                    i = 1;
                }
            });
            if (i == 1) {
                $scope.obj._id = $scope._id;
                //$scope.obj.RequestId = $scope.rid;
        
                formData.append("obj", angular.toJson($scope.obj));
              
                formData.append("Attributess", angular.toJson($scope.Attributess));

                $rootScope.cgBusyPromises = $http({
                    url: "/ServiceMaster/checkDuplicate",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                    //  alert(angular.toJson(response));
                    //  alert(response)

                    if (response != '') {
                        $scope.listptnodup1 = response;
                        $scope.hidesubmit = true;
                        $('#divduplicate').attr('style', 'display: block');
                    }
                    else {
                        //  alert('hi')
                        $scope.createSL();
                        $('#divduplicate').attr('style', 'display: none');
                        $scope.hidesubmit = false;
                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }
        }


        $scope.checkcode = "";
        $scope.checkcode1 = "active";

        $scope.createSL = function () {
       
            $scope.checkk = 0;
            $scope.subsub = "";
            $scope.unspsccom = "";
          
            // alert($scope.Attributess);
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();
            // alert(angular.toJson($scope.Attributess));
            var i = 0;
            angular.forEach($scope.Attributess, function (lst1) {
             
                if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                    //  alert("in");
                    i = 1;
                }
            });
            //  alert(i);

            if (i == 1) {
                // alert("Inside create data");

                formData.append("Attributess", angular.toJson($scope.Attributess));
                // formData.append("obj", angular.toJson($scope.obj));

                $scope.obj.UomName = $("#ServiceUomcodeSC").find("option:selected").text();
                $scope.obj.ValuationName = $("#ServiceValuationcodeSC").find("option:selected").text();
                $scope.obj.MainCodeName = $("#Service").find("option:selected").text();
                $scope.obj.SubCodeName = $("#Activity").find("option:selected").text();
                $scope.obj.SubSubCodeName = $("#SubSubCodeSC").find("option:selected").text();
                $scope.obj.PlantName = $("#PlantCode").find("option:selected").text();
                $scope.obj._id = $scope._id;
                //$scope.obj.ServiceCategoryName = $scope.obj.ServiceCategoryName;
                //$scope.obj.ServiceGroupName = $scope.obj.ServiceGroupName;
              
               
              //  alert(angular.toJson($scope.obj1))
                //$scope.obj.RequestId = $scope.rid;
                //   $scope.obj.ShortDesc = $scope.obj.short;
                //  $scope.obj.LongDesc = $scope.obj.long;
              //  alert(angular.toJson($scope.obj.MainCodeName));
                //if ($scope.UserId != "" && $scope.UserId != null && $scope.UserId != "undefined") {
                //    $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                //    if ($scope.obj1.Name == "Select Reviewer") {
                //        $scope.obj1.UserId = "";
                //    }
                //    else {
                //        $scope.obj1.UserId = $scope.UserId;
                //    }
                //}
                //$scope.obj1 = [];
                //  $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                // $scope.obj1.UserId = $scope.UserId;
                //formData.append("obj1", angular.toJson($scope.obj1))
                formData.append("obj", angular.toJson($scope.obj));
               
              


                $rootScope.cgBusyPromises = $http({
                    url: "/ServiceMaster/createshortlongsl",
                    // url: "/ServiceMaster/createshortlong",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    // alert(data);

                    $scope.obj.LongDesc = data[1];
                    $scope.obj.ShortDesc = data[0];
                     $scope.obj = null;
                      $scope.Attributess = null;
                      $scope.UserId = null;
                    $scope.form.$setPristine();
                    $scope.getdatalistforCleanser();
                    // $scope.searchMaster1($scope.sCode, $scope.sSource, $scope.sCategory, $scope.sGroup, $scope.sUser);
                    $rootScope.Res = "Data Saved Successfully";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                    // $rootscope.Res = "Data Saved Successfully";
                    //alert("Data Saved Successfully");
                    //  alert(angular.toJson($scope.Attributess));

                    // alert(angular.toJson($scope.obj.long));

                }).error(function (data, status, headers, config) {
                });
            }
            else {
              
                alert("mandatory value fields must be filled");
            }

            //  }
        };
        $scope.clarificationitem = function () {
          
            // alert(angular.toJson($scope.cat));
            var formData = new FormData();
            if ($scope.obj.Remarks == undefined || $scope.obj.Remarks == null || $scope.obj.Remarks == "") {
                alert(angular.toJson("Please Provide Remarks for Clarification"));
            }
            else {


                if (confirm("Are you sure,to send this record for clarification ?")) {
                    $scope.dis1 = true;

                    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 2000);
                    var i = 1;
                    angular.forEach($scope.Attributess, function (lst1) {
                        if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                            i = 1;
                        }
                    });
                    if (i == 1) {
                        $scope.obj._id = $scope._id;
                        //$scope.obj.RequestId = $scope.rid;
                        formData.append("obj", angular.toJson($scope.obj));
                       
                        formData.append("Attributess", angular.toJson($scope.Attributess));
                        $rootScope.cgBusyPromises = $http({
                            method: "POST",
                            url: '/ServiceMaster/GetclarifyCode',
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData

                        }).success(function (response) {
                            $scope.obj = null;
                            $scope.Attributess = null;
                            $scope.UserId = null;
                            $scope.form.$setPristine();
                            $scope.getdatalistforCleanser();
                            $rootScope.Res = "Record sent to Clarification";
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                          
                            $('#divNotifiy').attr('style', 'display: block');
                            //  alert(angular.toJson($scope.checkSubmit));
                            // $scope.checkSubmit();

                            $scope.reset();
                           
                            $scope.getdatalistforCleanser();
                            // $scope.reset1();

                            $scope.sts2 = false;
                            $scope.sts3 = false;
                            $scope.sts4 = false;

                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    }
                }
            }


        };
        //showtable for main nd subcode
        //$scope.getattibuteTable = function (MainCode, SubCode) {

        //    $http.get('/ServiceMaster/GetMainSubAttributeTable?MainCode=' + MainCode + '&SubCode=' + SubCode
        //               ).success(function (response) {
        //                   if (response != false) {
        //                       $scope.Attributess = response;
        //                   }
        //                   else {
        //                       $scope.Attributess = null;
        //                   }
             
        //               }).error(function (data, status, headers, config) {
        //               });
        //};

        $scope.getattibuteTable = function () {
            $scope.Attributess = null;
            $scope.obj.Unspsc = null;
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUnspsc',
                params: { Noun: $scope.obj.Noun, Modifier: $scope.obj.Modifier }
            }).success(function (response) {

                if (response != '') {
                    $scope.Commodities = response;
                    if (response[0].Commodity != null && response[0].Commodity != "")
                        $scope.obj.Unspsc = response[0].Commodity;
                    else $scope.obj.Unspsc = response[0].Class;
                }
                else {
                    $scope.obj.Unspsc = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
            $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier
             ).success(function (response) {
                 if (response != false) {

                     $scope.Attributess = response;
                   
                     angular.forEach(response, function (value1, key1) {

                         angular.forEach($scope.obj.Characteristics, function (value2, key2) {
                             if (value1.Attributes === value2.Attributes) {

                                 value1.Value = value2.Value;



                             }

                         });
                     });
           
                 }
                 else {
                     $scope.Attributess = null;
                 }
             }).error(function (data, status, headers, config) {
             });
            
        };
        //   csi

        $scope.getItemser = function () {

            var url = $location.$$absUrl;
            if (url.indexOf("ServiceMaster/ServiceCreation?itemId") !== -1) {

                var arrId = url.split('itemId=');

                $scope.obj = {};

                $http({
                    method: 'GET',
                    url: '/ServiceMaster/getItemser',
                    params: { itemId: arrId[1] }
                }).success(function (response) {
                    //$http({
                    //    method: 'GET',
                    //    url: '/GeneralSettings/GetUnspsc',
                    //    params: { ServiceCategorycode: response.ServiceCategorycode, ServiceGroupcode: response.ServiceGroupcode }
                    //}).success(function (response) {

                    //    if (response != '') {
                    //        $scope.Commodities = response;
                    //        if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                    //            $scope.obj.Unspsc = $scope.Commodities[0].Commodity;
                    //        else $scope.obj.Unspsc = $scope.Commodities[0].Class;
                    //    }
                    //    else {
                    //        $scope.Commodities = null;
                    //    }

                  //  }).error(function (data, status, headers, config) {
                      

                 //   });

                    $scope.obj = response;
                   
                    if ($scope.obj.ServiceCategoryCode != null) {
                        $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.obj.ServiceCategoryCode).success(function (maincode) {
                            $scope.getmainSC = maincode;
                        });
                    }
                    $scope.obj.Maincode = $scope.obj.Maincode;

                    if ($scope.obj.SubCode != null) {

                        $http.get('/ServiceMaster/getSubList?MainCode=' + $scope.obj.MainCode
                         ).success(function (subsc) {
                             $scope.getsubSC = subsc;
                         }).error(function (data, status, headers, config) {
                         });
                        $scope.obj.SubCode = $scope.obj.SubCode;
                    }
                    else {
                        $scope.obj.SubCode = "";
                    }

                    //  alert(angular.toJson($scope.values1.SubSubCode))
                    
                    if ($scope.obj.SubSubCode != null) {
                        $http.get('/ServiceMaster/GetSubSubList?MainCode=' + $scope.obj.MainCode + '&SubCode=' + $scope.obj.SubCode
                                ).success(function (subsub) {
                                    $scope.getsubsubSC = subsub;

                                    //    alert(angular.toJson($scope.getsubsubSC))
                                    //  $scope.obj.SubSubCode = "";
                                }).error(function (data, status, headers, config) {
                                });
                        $scope.obj.SubSubCode = $scope.obj.SubSubCode;
                    }
                    else {
                        $scope.obj.SubSubCode = "";
                    }

                    $http.get('/Master/GetDataListplnt'
                    ).success(function (response) {
                            $scope.getplant1 = response;
                            $scope.getplant1 = $filter('filter')($scope.getplant1, { 'Islive': 'true' })

                            }).error(function (data, status, headers, config) {

                           });

                   
                    $scope.obj.PlantCode = $scope.obj.PlantCode;
              
                    $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {

                        $scope.getcatSC = catagory;
                    });
                   // if ($scope.cat1.ServiceCategoryCode != null)
                    $scope.obj.ServiceCategorycode = $scope.obj.ServiceCategoryCode;
           
                    if ($scope.obj.ServiceCategoryCode != null) {
                        $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + $scope.obj.ServiceCategoryCode).success(function (response) {
                            $scope.getgroupSC = response;
                        });
                    }
                    if ($scope.obj.ServiceGroupCode != null)
                        $scope.obj.ServiceGroupcode = $scope.obj.ServiceGroupCode;


         
                        $http.get('/ServiceMaster/getuomlist').success(function (uom) {
                            //   alert('hi');
                            $scope.getUOMSC = uom;
                            //  alert(angular.toJson($scope.getUOMSC))
                        });
                        $scope.obj.UomCode = $scope.obj.UomCode;
               
                        $scope.obj.Class = $scope.obj.Class;
                        $scope.obj.ClassTitle = $scope.obj.ClassTitle;
                        $scope.obj.Unspsc = $scope.obj.Commodity;
                        $scope.obj.CombmodityTitle = $scope.obj.CommodityTitle;
                      

                        $http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
                            $scope.getVALSC = valsc
                        });
                        $scope.obj.Valuationcode = $scope.obj.Valuationcode;

                    ///find default attribute
                    //    $scope.defatt = "";
                        //$http.get('/ServiceMaster/Defaultattribute'
                        //       ).success(function (response) {
                        //           $scope.Defaultatt = response;

                    
                        //       }).error(function (data, status, headers, config) {
                        //       });

                    // FOR LOSDING ATTRIBUTES



                    $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier).success(function (response) {
                             if (response != false) {

                                 $scope.Attributess = response;

                                 //for default attributes
                                 //angular.forEach(response, function (value1, key1) {

                                 //    angular.forEach($scope.Defaultatt, function (value2, key2) {

                                 //        if (value1.Attributes === value2.Attributes) {
                                 //            value1.Default = 'Yes';
                                 //        }

                                 //    });
                                 //});

                                 //For item values
                                 angular.forEach(response, function (value1, key1) {

                                     angular.forEach($scope.obj.Characteristics, function (value2, key2) {
                                         if (value1.Attributes === value2.Attributes) {

                                             value1.Value = value2.Value;



                                         }

                                     });
                                 });
                             }
                             else {
                                 $scope.Attributess = null;
                             }
                         }).error(function (data, status, headers, config) {
                         });
                        if ($scope.obj.ServiceStatus === "Cleanse") {
                            $scope.Showsubmit = true;
                        }
                        else if ($scope.obj.ServiceStatus === "Cleansed") {
                            $scope.Showsubmit = true;
                        }
                        //$http.get('/ServiceMaster/LoadReviewer').success(function (reviewer) {
                        //    $scope.Reviewer_list = reviewer;
                        //});
                        //$scope.UserId = $scope.obj.Reviewer.UserId;
                })
            }
        }
        $scope.getItemser();

        $scope.RowClick = function (idx) {
          //  alert("hai");
            //  alert(angular.toJson($scope.DataList[idx]));
          //  $scope.sc = $scope.DataList[idx].ServiceCode;
          //  $scope.rid = $scope.DataList[idx].RequestId;
          //  $scope._id = $scope.DataList[idx]._id;
          //  $scope.obj = $scope.DataList[idx];
            $scope.hidesubmit = false;
            $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope.DataList[idx]._id).success(function (response) {
                $scope.obj = response;
                $scope.obj.Unspsc = response.Commodity;
                $http.get('/ServiceMaster/GetModifier?Noun=' + $scope.obj.Noun).success(function (response) {
                    $scope.Modifiers = response;
                }).error(function (data, status, headers, config) {

                });
                $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
                  
                    $scope.getcatSC = catagory;
                });

                $http.get('/ServiceMaster/LoadReviewer').success(function (reviewer) {
                    $scope.Reviewer_list = reviewer;
                });

                //$http.get('/ServiceMaster/showall_Uomuser').success(function (uom) {
                //    $scope.getUOMSC = uom;
                //});
                $http.get('/ServiceMaster/getuomlist').success(function (uom) {
                    //   alert('hi');
                    $scope.getUOMSC = uom;
                    //  alert(angular.toJson($scope.getUOMSC))
                });

                $http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
                    $scope.getVALSC = valsc
                });


                $http.get('/Master/GetDataListplnt'
            ).success(function (response) {
                $scope.getplant1 = response;
                $scope.getplant1 = $filter('filter')($scope.getplant1, { 'Islive': 'true' })

            }).error(function (data, status, headers, config) {

            });

              
                var i = 0;
                angular.forEach($scope.DataList, function (lst1) {
                    $('#' + i).attr("style", "");
                    i++;
                });
                $('#' + idx).attr("style", "background-color:lightblue");


                // alert(angular.toJson($scope.values1.Reviewer));
                if ($scope.obj.Reviewer != null)
                    $scope.UserId = $scope.obj.Reviewer.UserId;
                else
                    $scope.UserId = null;

               
                ///find default attribute
            //    $scope.defatt = "";
                //$http.get('/ServiceMaster/Defaultattribute'
                //       ).success(function (response) {
                //           $scope.Defaultatt = response;

                    
                //       }).error(function (data, status, headers, config) {
                //       });
             
            
                $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier
             ).success(function (response) {
                 if (response != false) {

                     $scope.Attributess = response;

                     //for default attributes
                     //angular.forEach(response, function (value1, key1) {

                     //    angular.forEach($scope.Defaultatt, function (value2, key2) {

                     //        if (value1.Attributes === value2.Attributes) {
                     //            value1.Default = 'Yes';
                     //        }

                     //    });
                     //});

                     //For item values
                  
                     angular.forEach(response, function (value1, key1) {
                       
                         angular.forEach($scope.obj.Characteristics, function (value2, key2) {
                             if (value1.Attributes === value2.Attributes) {

                                 value1.Value = value2.Value;
                                
                             }
                          
                         });
                    
                     });       
                     
                      
                 }
                 else {
                     $scope.Attributess = null;
                 }
             }).error(function (data, status, headers, config) {
             });


                if ($scope.obj.ServiceStatus === "Cleanse") {
                    $scope.Showsubmit = true;
                }
                else if ($scope.obj.ServiceStatus === "Cleansed") {
                    $scope.Showsubmit = true;
                }
                else if ($scope.obj.ServiceStatus === "Rejected") {
                    alert("Service already rejected");
                    $scope.Showsubmit = false;
                }
                else {
                    alert("Service Data already Submitted");
                    $scope.Showsubmit = false;
                }
            });

        };

        $scope.Searchclasstitle = function (ClassTitlesh)
        {
           
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ServiceMaster/getcommlist?sKey=' + ClassTitlesh
            }).success(function (response) {

                if (response != '') {
                    $scope.COMM = response;
                   mymodal.open();
                    //$scope.Res = "Search results : " + response.length + " items."
                    //$scope.Notify = "alert-info";
                    //$scope.NotifiyRes = true;


                } else {

                    //$scope.res = "no item found"
                    //$scope.notify = "alert-danger";
                    //$scope.notifiyres = true;

                }

            })

        }
        $scope.SearchCommodityTitle = function (Unspsc) {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ServiceMaster/getcommcommlist?sKey=' + $scope.cat.Unspsc
            }).success(function (response) {

                if (response != '') {
                    $scope.COMM = response;
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
        var mymodal = new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',

            overlay: true,
            closeButton: true,

            content: jQuery('#cotentid2'),

        });
        $scope.COMMClick = function (C, idx) {
      
            $scope.obj.Class = C.Class;
            $scope.obj.ClassTitle = C.ClassTitle;
            $scope.obj.Unspsc = C.Commodity;
            $scope.obj.CommodityTitle = C.CommodityTitle;
            $scope.ClassTitlesh = null;
            mymodal.close();
            

        }


        $scope.createData = function (ItemId) {

           
      
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            //$timeout(function () { $scope.NotifiyRes = false; }, 8000);
            var formData = new FormData();
          
            var i = 0;
            angular.forEach($scope.Attributess, function (lst1) {
                
                if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                    //  alert("in");
                    i = 1;
                }
            });
            //  alert(i);

            if (i == 1) {
               
                $http({
                    method: 'GET',
                    url: '/ServiceMaster/InsertServiceCreation?ItemId=' + ItemId
                }).success(function (data, status, headers, config) {

                 //   alert(data);
            
                    if (data !== "" || data !== null || data !== undefined) {

                        $scope.obj = null;
                        $scope.Attributess = null;
                        $scope.UserId = null;
                        //$scope.form.$setPristine();
                   
                        $rootScope.Res = "Sent to reviewer";
                        $scope.getdatalistforCleanser();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    
                       // $scope.obj.long = data[1];
                      //  $scope.obj.short = data[0];
                        $scope.Showsubmit = false;
                    

                    }
                    else {
                        $rootScope.Res = "Error on processing try again";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    // $scope.reset();
                    // $scope.obj = null;
                }).error(function (data, status, headers, config) {
                });
            }
            else {
                alert("mandatory value fields must be filled");
            }
        };

        $scope.getgroupcodeforcatagory = function (catagory) {
            //  alert(catagory);

            $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + catagory).success(function (response) {
                $scope.getgroupSC = response;
            });

            if (catagory != null && catagory != "undefined" && catagory != "") {
                $http.get('/ServiceMaster/showall_MainCode1?catagory=' + catagory).success(function (maincode) {
                    $scope.getmainSC = maincode;
                });
            }
        };

        //  $scope.RowClick(0);
        $scope.loadcatlist = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ServiceMaster/GetcatList'
            }).success(function (response) {
                $scope.getcatSC1 = response;
                $scope.sCategory = "";

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.loadcatlist();

        $scope.GetUserList = function () {
           // $scope.NMLoad();
            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_user'
            }).success(function (response) {
                $scope.UserList = response;

                //  alert(angular.toJson($scope.UserList))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        $scope.GetUserList();
        $scope.searchMaster1 = function (sCode,iCode, sSource, sCategory, sGroup, sUser) {

            //alert(angular.toJson($scope.sCode));
            var formData = new FormData();
         
            formData.append("sCode", $scope.sCode);
            formData.append("iCode", $scope.iCode);
            formData.append("sSource", $scope.sSource);
            formData.append("sShort", $scope.sShort);
            formData.append("sLong", $scope.sLong);
            formData.append("sCategory", $scope.sCategory);
            formData.append("sGroup", $scope.sGroup);
            formData.append("sUser", $scope.sUser);
            formData.append("sStatus", $scope.sStatus);
           // alert(angular.toJson($scope.iCode));
           // formData.append("sType", $scope.sType);
            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.iCode != undefined && $scope.iCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sShort != undefined && $scope.sShort != '') || ($scope.sLong != undefined && $scope.sLong != '') || ($scope.sCategory != undefined && $scope.sCategory != '') || ($scope.sGroup != undefined && $scope.sGroup != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sStatus != undefined && $scope.sStatus != ''))
            {

                //$(".loaderb_div").show();
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/ServiceMaster/searchMaster1',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    $scope.DataList = response;

                    $scope.showlist = true;
                    if ($scope.DataList == "" || $scope.DataList == null || $scope.DataList == "undefinded") {
                        $scope.showlist = false;

                    }
                    else {
                        $scope.sc = $scope.DataList[0].ServiceCode;
                        $scope.rid = $scope.DataList[0].RequestId;
                        $scope._id = $scope.DataList[0]._id;
                        $scope.obj = $scope.DataList[0];

                        $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope.DataList[0]["_id"]).success(function (response) {

                            $scope.values1 = response;
                            // alert(angular.toJson(response));
                            $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
                                $scope.getcatSC = catagory;
                            });

                            $http.get('/ServiceMaster/LoadReviewer').success(function (reviewer) {
                                $scope.Reviewer_list = reviewer;
                            });

                            //$http.get('/ServiceMaster/showall_Uomuser').success(function (uom) {
                            //    $scope.getUOMSC = uom;
                            //});

                            $http.get('/ServiceMaster/getuomlist').success(function (uom) {
                                //alert('hi');
                                $scope.getUOMSC = uom;
                                // alert(angular.toJson($scope.getUOMSC))
                            });

                            $http.get('/ServiceMaster/showall_Valuationuser').success(function (valsc) {
                                $scope.getVALSC = valsc
                            });


                            $http.get('/Master/GetDataListplnt'
                        ).success(function (response) {
                            $scope.getplant1 = response;
                            $scope.getplant1 = $filter('filter')($scope.getplant1, { 'Islive': 'true' })

                        }).error(function (data, status, headers, config) {

                        });

                            if ($scope.values1.ServiceCategoryCode != null) {
                                $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.values1.ServiceCategoryCode).success(function (maincode) {
                                    $scope.getmainSC = maincode;
                                });
                            }

                            var i = 0;
                            angular.forEach($scope.DataList, function (lst1) {
                                $('#' + i).attr("style", "");
                                i++;
                            });
                            $('#0').attr("style", "background-color:lightblue");


                            // alert(angular.toJson($scope.values1.PlantCode));
                            if ($scope.values1.PlantCode != null && $scope.values1.PlantCode != "" && $scope.values1.PlantCode != "undefined") {
                                // alert("hi");
                                $scope.obj.PlantCode = $scope.values1.PlantCode;
                            }

                            if ($scope.values1.Legacy != null) {
                                // alert("hi");
                                $scope.obj.legacyy = $scope.values1.Legacy;
                            }

                            if ($scope.values1.ServiceCategoryCode != null)
                                $scope.obj.ServiceCategorycode = $scope.values1.ServiceCategoryCode;

                            if ($scope.values1.ServiceCategoryCode != null) {
                                $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + $scope.values1.ServiceCategoryCode).success(function (response) {
                                    $scope.getgroupSC = response;
                                });
                            }

                            //   alert(angular.toJson($scope.values1.Reviewer));
                            if ($scope.values1.Reviewer != null)
                                $scope.UserId = $scope.values1.Reviewer.UserId;
                            else
                                $scope.UserId = null;

                            if ($scope.values1.ServiceGroupCode != null)
                                $scope.obj.ServiceGroupcode = $scope.values1.ServiceGroupCode;


                            //   $scope.getattibuteTable($scope.obj.ServiceCategorycode, $scope.obj.ServiceGroupcode);

                            if ($scope.values1.UomCode != null)
                                $scope.obj.UomCode = $scope.values1.UomCode;
                            else
                                $scope.obj.UomCode = "";

                            if ($scope.values1.ShortDesc != null)
                                $scope.obj.short = $scope.values1.ShortDesc;
                            else
                                $scope.obj.short = "";

                            if ($scope.values1.LongDesc != null)
                                $scope.obj.long = $scope.values1.LongDesc;
                            else
                                $scope.obj.long = "";


                            if ($scope.values1.Valuationcode != null)
                                $scope.obj.Valuationcode = $scope.values1.Valuationcode;
                            else
                                $scope.obj.Valuationcode = "";



                            if ($scope.values1.MainCode != null)
                                $scope.obj.MainCode = $scope.values1.MainCode;
                            else
                                $scope.obj.MainCode = "";

                            if ($scope.values1.SubCode != null) {

                                $http.get('/ServiceMaster/getSubList?MainCode=' + $scope.values1.MainCode
                                 ).success(function (subsc) {
                                     $scope.getsubSC = subsc;
                                 }).error(function (data, status, headers, config) {
                                 });
                                $scope.obj.SubCode = $scope.values1.SubCode;
                            }
                            else {
                                $scope.obj.SubCode = "";
                            }


                            // FOR LOSDING ATTRIBUTES




                            if ($scope.values1.SubSubCode != null) {
                                $http.get('/ServiceMaster/GetSubSubList?MainCode=' + $scope.values1.MainCode + '&SubCode=' + $scope.values1.SubCode
                                        ).success(function (subsub) {
                                            $scope.getsubsubSC = subsub;
                                            // $scope.obj.SubSubCode = "";
                                        }).error(function (data, status, headers, config) {
                                        });
                                $scope.obj.SubSubCode = $scope.values1.SubSubCode;
                            }
                            else {
                                $scope.obj.SubSubCode = "";
                            }



                            //      alert("load");
                            if ($scope.values1.Characteristics != null && $scope.values1.Characteristics != "") {
                                $scope.Attributess = $scope.values1.Characteristics;
                            }
                            else {

                                $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier
                           ).success(function (response) {
                               if (response != false) {
                                   $scope.Attributess = response;
                               }
                               else {
                                   $scope.Attributess = null;
                               }
                           }).error(function (data, status, headers, config) {
                           });

                            }

                            if ($scope.values1.Remarks != null && $scope.values1.Remarks != "") {
                                $scope.obj.Remarks = $scope.values1.Remarks;
                            }
                            else {
                                $scope.obj.Remarks = "";
                            }

                            if ($scope.values1.ServiceStatus === "Cleanse") {
                                $scope.Showsubmit = true;
                            }
                            else if ($scope.values1.ServiceStatus === "Cleansed") {

                                $scope.Showsubmit = true;
                            }
                            else {
                               // alert("data already submitted");
                                $scope.showsubmit = false;
                            }


                        });
                    }


                    if (response != null && response.length > 0) {

                        $scope.Res = "Search result : " + response.length + " records found";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                    } else {
                        $scope.Res = "Records not found";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }

                    //angular.forEach($scope.DataList, function (lst) {
                    //    lst.bu = '0';

                    //});

                    //if (response != null && response.length > 0) {
                    //    //  alert(angular.toJson($scope.fromsave));
                    //    if ($scope.fromsave === 1)
                    //    {
                    //    $rootScope.Res = "Data Saved Successfully";
                    //    $rootScope.Notify = "alert-info";
                    //    $rootScope.NotifiyRes = true;
                    //    $('#divNotifiy').attr('style', 'display: block');
                    //        // alert(angular.toJson($scope.fromsave));
                    //    }
                    //    else {
                    //        $scope.Res = "Search result : " + response.length + " records found";
                    //    }

                    //    $scope.Notify = "alert-info";
                    //    $('#divNotifiy').attr('style', 'display: block');


                    //} else {
                    //    $scope.Res = "Records not found";
                    //    $scope.Notify = "alert-danger";
                    //    $('#divNotifiy').attr('style', 'display: block');
                    //}
                    //$(".loaderb_div").hide();
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            } else {

                $scope.getdatalistforCleanser();

            }

        }
        //$scope.resetForm = function () {
        //    //$scope.resetme.reset();
        //    document.getElementById('form').reset();
        //};
        $scope.ChangeModifier = function () {
            $scope.cat.Shortdesc = "";
            $scope.cat.Longdesc = "";
            $scope.cat.MissingValue = "";
            $scope.cat.EnrichedValue = "";
            $scope.cat.RepeatedValue = "";


            if ($scope.cat.Modifier != null && $scope.cat.Modifier != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspsc',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    if (response != '') {
                        $scope.Commodities = response;
                        if (response[0].Commodity != null && response[0].Commodity != "")
                            $scope.cat.Unspsc = response[0].Commodity;
                        else $scope.cat.Unspsc = response[0].Class;
                    }
                    else {
                        $scope.Commodities = null;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHsn?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                }).success(function (response) {
                    if (response.HSNID != null) {
                        $scope.HSNID = response.HSNID;
                        $scope.Desc = response.Desc;
                        $scope.HSNShow = true;
                    }
                    else {
                        $scope.HSNShow = false;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                    }


                });

                $http({
                    method: 'GET',
                    url: '/Dictionary/getuomlist1',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    if (response.length > 0) {
                        $scope.uomList1 = response;

                    }
                    else {

                        $http.get('/Catalogue/getuomlist').success(function (response) {
                            // alert('hi');
                            $scope.uomList1 = response;
                            //alert(angular.toJson($scope.uomList1))
                        });
                    }


                });

                //  $scope.getItem();
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetForamted',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    $scope.Type = response;

                    //angular.forEach($scope.rows, function (lst) {
                    //    lst.s = '1';

                    //});
                    if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                        $scope.ven = true;


                    }
                    else {
                        $scope.ven = false;
                    }

                });

                //$http({
                //    method: 'GET',
                //    url: '/Catalogue/FetchNMRelation',
                //    params:{Noun:$scope.cat.Noun , Modifier: $scope.cat.Modifier}
                //}).success(function (response) {

                //    if (response != '') {
                //        $scope.tempPlace = response;

                //    }
                //    else {
                //        $scope.tempPlace = null;
                //    }

                //}).error(function (data, status, headers, config) {
                //    // alert("error");

                //});

                //$http({
                //    method: 'GET',
                //    url: '/Catalogue/FetchATTRelation?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                //}).success(function (response) {

                //    if (response != '') {                       
                //        $scope.tempRelation = response;

                //    }
                //    else {
                //        $scope.tempRelation = null;
                //    }

                //}).error(function (data, status, headers, config) {
                //    // alert("error");

                //});

                //Attribute List
                //    $scope.FetchNMRelation();
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;

                        $scope.Characteristics = response.ALL_NM_Attributes;
                        $scope.FetchNMRelation();


                        angular.forEach($scope.Characteristics, function (value1, key1) {
                            if (value1.Characteristic === "DESIGNATION") {

                                $scope.SelectDesg($scope.cat.Noun, $scope.cat.Modifier, "DESIGNATION")
                            }
                        });



                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;

                        }).error(function (data, status, headers, config) {
                            // 
                        });



                    }
                    else $scope.Characteristics = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

            }
            else {

                // $scope.cat = null;
                $scope.Characteristics = null;
                //  $scope.equ = null;
            }
            //   $scope.getSimiliar();    

        };

    });


    app.directive('capitalize', function () {
    //    alert("cp");
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                var capitalize = function (inputValue) {
                    if (inputValue == undefined) inputValue = '';
                    var capitalized = inputValue.toUpperCase();
                    if (capitalized !== inputValue) {
                        // see where the cursor is before the update so that we can set it back
                        var selection = element[0].selectionStart;
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                        // set back the cursor after rendering
                        element[0].selectionStart = selection;
                        element[0].selectionEnd = selection;
                    }
                    return capitalized;
                }
                modelCtrl.$parsers.push(capitalize);
                capitalize(scope[attrs.ngModel]); // capitalize initial value
            }
        };
    });

    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/ServiceMaster/AutoCompleteNoun?term=" + term,
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
                        scope.obj.Noun = selectedItem.item.value;

                        $.get("/ServiceMaster/GetModifier", { Noun: selectedItem.item.value }).success(function (response) {
                            scope.Modifiers = response;
                            scope.$apply();
                            event.preventDefault();
                        });

                    }
                });

            }

        };
    }]);
})();