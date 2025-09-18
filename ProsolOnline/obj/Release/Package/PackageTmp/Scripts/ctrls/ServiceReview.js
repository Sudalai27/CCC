(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter']);
    app.controller('ServiceCreationController', function ($scope, $http, $rootScope, $timeout, $filter) {
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
        $scope.SL = "";
        $scope.Showsubmit = false;
        $scope.showlist = false;
        $scope.obj1 = { "UserId": "", "Name": "" };
        $scope.defatt = "";
        $scope.hidesubmit = false;


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
              //  alert(angular.toJson($scope.response));

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

        $scope.BindGroupcodeList = function () {
            // alert("maincode");
            // alert(angular.toJson($scope.obj.ServiceCategorycode));
            $http({
                method: 'GET',
                url: '/ServiceMaster/showall_MainCode1?catagory=' + $scope.obj.ServiceCategorycode
            }).success(function (response) {
                $scope.getmainSC = response;
                //  alert(angular.toJson($scope.getmainSC))
                /////
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };


        $scope.SelectAttr = function (Noun, Modifier, Attribute) {
            //  alert("hai");

            $http({
                method: 'GET',
                url: '/ServiceMaster/GetValues?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute
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

        $scope.AddValue1 = function (Noun, Modifier, Attribute, Value, abb, indx) {
            $http({
                method: 'GET',
                url: '/ServiceMaster/AddValue1',
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: abb }
                //?SubCode=' + Activity + '&Attribute=' + Attribute + '&Value=' + Value + '&abb=' + abb
            }).success(function (response) {

                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:block');

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

        $http.get('/ServiceMaster/LoadReleaser').success(function (reviewer) {
            $scope.Reviewer_list = reviewer;
        });
       


        $scope.getdatalistforCleanser = function () {
            $rootScope.cgBusyPromises = $http.get('/ServiceMaster/getdatalistforReviewer'
           ).success(function (response) {
               $scope.DataList = response;

               $scope.tot = response.length;
               $scope.saveditems = ($filter('filter')($scope.DataList, { 'ServiceStatus': 'QC' })).length;
               $scope.balanceitems = ($filter('filter')($scope.DataList, { 'ServiceStatus': 'Review' })).length;

               $scope.showlist = true;

             

               if ($scope.DataList.ServiceStatus === "Review") {
                           $scope.Showsubmit = true;
                       }

               else if ($scope.DataList.ServiceStatus === "QC") {
                           $scope.Showsubmit = true;
                       }

               else if ($scope.DataList.ServiceStatus === "Cleanse") {
                           alert("Data sent back to Cleanser");
                           $scope.Showsubmit = false;

                       }
                       else {
                         //  alert("Service Data already Submitted");
                           $scope.Showsubmit = false;
                       }




           }).error(function (data, status, headers, config) {
           });
        };


        $scope.rejectitem = function () {
            $http.get('/ServiceMaster/rejectitem?_id=' + $scope._id + '&rejectedas=Reviewer&Remarks='+ $scope.obj.Remarks
            ).success(function (response) {
                $rootScope.Res = "Service has been rejected";
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                $scope.Showsubmit = false;

            }).error(function (data, status, headers, config) {
            });
        };




        $scope.getdatalistforCleanser();

        //subcode
        $scope.getsubcodeSC = function (MainCode) {
            $http.get('/ServiceMaster/getSubList?MainCode=' + MainCode
            ).success(function (response) {
                $scope.getsubSC = response;
                $scope.obj.SubCode = "";
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getsubsubcodesc1 = function (MainCode, SubCode) {
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
        $scope.getsubsubsubcodesc1 = function () {
            //alert(angular.toJson($scope.obj.SubSubCode));
            if ($scope.CLogic === "Customized Code") {
                if ($scope.obj.SubSubCode === "" || $scope.obj.SubSubCode === null || $scope.obj.SubSubCode === undefined) {
                    $scope.subsub = "red";
                }
                else {
                    $scope.subsub = "";
                }
            }
            else {

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
            var i = 0;
            angular.forEach($scope.Attributess, function (lst1) {
                if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                    i = 1;
                }
            });
            if (i == 1) {
                

                $scope.obj._id = $scope._id;
                //$scope.obj.RequestId = $scope.rid;
                //$scope.obj1.ServiceCategoryName = $scope.obj.ServiceCategoryName;
                //$scope.obj1.ServiceGroupName = $scope.obj.ServiceGroupName;
                //formData.append("obj1", angular.toJson($scope.obj1))
                //alert(angular.toJson($scope.obj.ServiceCategoryName));
                //$scope.obj = [];
                //$scope.obj.push({ ServiceCategoryName: $scope.obj.ServiceCategoryName, ServiceGroupName: $scope.obj.ServiceGroupName })
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
            // alert("inside create function");
            $scope.checkk = 0;
            $scope.subsub = "";
            $scope.unspsccom = "";
            //if ($scope.CLogic === "Customized Code")
            //{
            //    if($scope.obj.SubSubCode === "" || $scope.obj.SubSubCode === null || $scope.obj.SubSubCode === undefined )
            //    {
            //        $scope.subsub = "red";
            //        $scope.checkk = 1;
            //        $scope.checkcode = "active";
            //        $scope.checkcode1 = "";
            //    }
            //}
            //if ($scope.CLogic === "UNSPSC Code") 
            //{
               
            //    if ($scope.obj.Unspsc === "" || $scope.obj.Unspsc === null || $scope.obj.Unspsc === undefined) {
                   
            //        $scope.unspsccom = "red";
            //        $scope.checkk = 1;
            //        $scope.checkcode = "active";
            //        $scope.checkcode1 = "";
            //    }

            //}

            //if ($scope.checkk === 0) {
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
                var formData = new FormData();
                var i = 0;
                angular.forEach($scope.Attributess, function (lst1) {
                    if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                        //  alert("in");
                        i = 1;
                    }
                });
                if (i == 1) {
                 
                    formData.append("Attributess", angular.toJson($scope.Attributess));
                    //$scope.obj.ServiceCategoryName = $("#ServiceCategorycodeSC").find("option:selected").text();
                    //$scope.obj.ServiceGroupName = $("#ServiceGroupcodeSC").find("option:selected").text();
                    $scope.obj.UomName = $("#ServiceUomcodeSC").find("option:selected").text();
                    $scope.obj.ValuationName = $("#ServiceValuationcodeSC").find("option:selected").text();
                    $scope.obj.MainCodeName = $("#Service").find("option:selected").text();
                    $scope.obj.SubCodeName = $("#Activity").find("option:selected").text();
                    $scope.obj.SubSubCodeName = $("#SubSubCodeSC").find("option:selected").text();
                    $scope.obj.PlantName = $("#PlantCode").find("option:selected").text();
                    $scope.obj._id = $scope._id;
                    $scope.obj.RequestId = $scope.rid;
                    $scope.obj.ShortDesc = $scope.obj.short;
                    $scope.obj.LongDesc = $scope.obj.long;
                   
                    if ($scope.UserId != "" && $scope.UserId != null && $scope.UserId != "undefined") {
                        $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                        if ($scope.obj1.Name == "Select Releaser") {
                            $scope.obj1.UserId = "";
                        }
                        else {
                            $scope.obj1.UserId = $scope.UserId;
                        }
                    }
                    $scope.obj1.ServiceCategoryName = $scope.obj.ServiceCategoryName;
                    $scope.obj1.ServiceGroupName = $scope.obj.ServiceGroupName;

               
                    // $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                    formData.append("obj", angular.toJson($scope.obj));
                    formData.append("obj1", angular.toJson($scope.obj1))
                    $http({
                        method: "POST",
                        url: '/ServiceMaster/SaveServiceReview?SL=' + $scope.SL,
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {

                        //if (data[2] === "true") {
                        //    $rootScope.Res = "Record Saved";
                        //    $rootScope.Notify = "alert-info";
                        //    $rootScope.NotifiyRes = true;
                        //    $('#divNotifiy').attr('style', 'display: block');
                        //$scope.obj = null;
                        //$scope.Attributess = null;
                        //$scope.UserId = null;
                        //$scope.form.$setPristine();
                        $scope.obj = null;
                        $scope.Attributess = null;
                        $scope.UserId = null;
                        $scope.getdatalistforCleanser();
                        $scope.obj.LongDesc = data[1];
                       $scope.obj.ShortDesc = data[0];
                    //    $scope.searchMaster1($scope.sCode, $scope.sSource, $scope.sCategory, $scope.sGroup, $scope.sUser);
                        $rootScope.Res = "Data Saved Successfully";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        //$scope.Showsubmit = false;
                        //}
                        //else {
                        //    $rootScope.Res = "Error on processing try again";
                        //    $rootScope.Notify = "alert-info";
                        //    $rootScope.NotifiyRes = true;
                        //    $('#divNotifiy').attr('style', 'display: block');
                        //}
                        //// $scope.reset();
                        //$scope.obj = null;
                     
                    }).error(function (data, status, headers, config) {

                    });
                }
                else {
                    alert("mandatory value fields must be filled");
                }

                //  alert(angular.toJson($scope.Attributess));
                //$scope.SL = "SL";

                //var formData = new FormData();
                //formData.append("Attributess", angular.toJson($scope.Attributess));

                //$http({
                //    url: '/ServiceMaster/createshortlong',
                //    method: "POST",
                //    headers: { "Content-Type": undefined },
                //    transformRequest: angular.identity,
                //    data: formData
                //}).success(function (data, status, headers, config) {
                //    // alert("Inside sl creation");
                //    $scope.obj.long = data[1];
                //    $scope.obj.short = data[0];

                //}).error(function (data, status, headers, config) {
                //});
           // }
        };


        //showtable for main nd subcode


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
        //$scope.getattibuteTable = function (MainCode, SubCode) {
        //    $http.get('/ServiceMaster/GetMainSubAttributeTable?MainCode=' + MainCode + '&SubCode=' + SubCode
        //               ).success(function (response) {
        //                   if (response != false) {
        //                       $scope.Attributess = response;
        //                       angular.forEach($scope.Attributess, function (value, key) {
        //                           //var i = 0;
        //                           //$scope.abvshow = false;
        //                       });
        //                   }
        //                   else {
        //                       $scope.Attributess = null;
        //                   }
        //                   $http.get('/ServiceMaster/GetSubSubList?MainCode=' + MainCode + '&SubCode=' + SubCode
        //               ).success(function (response) {
        //                   $scope.getsubsubSC = response;
        //                   $scope.obj.SubSubCode = "";
        //               }).error(function (data, status, headers, config) {
        //               });
        //               }).error(function (data, status, headers, config) {
        //               });
        //};

        $scope.senttoCleanser = function () {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            // alert(angular.toJson($scope._id));
            $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope._id).success(function (response) {
                $scope.result = response;

                if ($scope.result.ServiceStatus != "Completed") {
                    if ($scope.result.ServiceStatus == "Cleanse") {
                        $rootScope.Res = "Already sent to cleanser";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.Showsubmit = false;
                    }
                    else if ($scope.result.ServiceStatus == "Release") {
                        $rootScope.Res = "Already submitted to releaser";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.Showsubmit = false;
                    }
                    else {

                        $http.get('/ServiceMaster/senttocleanser?_id=' + $scope._id +'&Remarks='+$scope.obj.Remarks).success(function (response) {

                            $scope.obj = null;
                            $scope.Attributess = null;
                            $scope.UserId = null;
                            $scope.form.$setPristine();
                            $rootScope.Res = "Sent back to cleanser";
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                            $scope.getdatalistforCleanser();
                            
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.Showsubmit = false;
                            
                        });
                    }
                }
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.RowClick = function (cat1, idx) {
            $scope.hidesubmit = false;
            $scope.sc = $scope.DataList[idx].ServiceCode;
            $scope.rid = $scope.DataList[idx].RequestId;
            $scope._id = $scope.DataList[idx]._id;
            $scope.obj = $scope.DataList[idx];

            $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope.DataList[idx]._id).success(function (response) {
                $scope.obj = response;
                $http.get('/ServiceMaster/GetModifier?Noun=' + $scope.obj.Noun).success(function (response) {
                    $scope.Modifiers = response;
                }).error(function (data, status, headers, config) {

                });
                $scope.obj.Unspsc = $scope.obj.Commodity;
                $http.get('/Master/GetDataListplnt'
            ).success(function (response) {
                $scope.getplant = response;
                $scope.getplant = $filter('filter')($scope.getplant, { 'Islive': 'true' })
            }).error(function (data, status, headers, config) {
            });

                var i = 0;
                angular.forEach($scope.DataList, function (lst1) {
                    $('#' + i).attr("style", "");
                    i++;
                });
                $('#' + idx).attr("style", "background-color:lightblue");



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




                $http.get('/ServiceMaster/LoadReleaser').success(function (reviewer) {
                    $scope.Reviewer_list = reviewer;
                });               

          

             

                if ($scope.obj.Releaser != null)
                    $scope.UserId = $scope.obj.Releaser.UserId;
                else
                    $scope.UserId = null;

          

                //if ($scope.values1.Releaser.UserId != null)
                //    $scope.UserId = $scope.values1.Releaser.UserId;




                ///find default attribute
           
                $http.get('/ServiceMaster/Defaultattribute'
                       ).success(function (response) {
                           $scope.Defaultatt = response;


                       }).error(function (data, status, headers, config) {
                       });

                ////

                //$http.get('/ServiceMaster/GetMainSubAttributeTable?MainCode=' + $scope.obj.ServiceCategorycode + '&SubCode=' + $scope.obj.ServiceGroupcode
                $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier
               ).success(function (response) {
                   if (response != false) {

                       $scope.Attributess = response;

                       ////for default attributes
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

               // if ($scope.values1.Characteristics)
               // {
               //     $scope.Attributess = $scope.values1.Characteristics;
               // }
               // else {

               //     $http.get('/ServiceMaster/GetMainSubAttributeTable?MainCode=' + $scope.obj.ServiceCategorycode + '&SubCode=' + $scope.obj.ServiceGroupcode
               //).success(function (response) {
               //    if (response != false) {
               //        $scope.Attributess = response;
               //    }
               //    else {
               //        $scope.Attributess = null;
               //    }
               //}).error(function (data, status, headers, config) {
               //});

               // }

                if ($scope.obj.ServiceStatus === "Review") {
                    $scope.Showsubmit = true;
                }

                else if ($scope.obj.ServiceStatus === "QC") {
                    $scope.Showsubmit = true;
                }

                else if ($scope.obj.ServiceStatus === "Cleanse")
                {
                    alert("Data sent back to Cleanser");
                    $scope.Showsubmit = false;

                }
                else if ($scope.obj.ServiceStatus === "Rejected") {
                    alert("Service already rejected");
                    $scope.Showsubmit = false;
                }
                else
                {
                    alert("Service Data already Submitted");
                    $scope.Showsubmit = false;
                }
            });
        };

        $scope.Searchclasstitle = function (ClassTitlesh) {

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

                    //$scope.Res = "No item found"
                    //$scope.Notify = "alert-danger";
                    //$scope.NotifiyRes = true;

                }

            })

        }
        $scope.SearchCommodityTitle = function (CommodityTitlesh) {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ServiceMaster/getcommcommlist?sKey=' + CommodityTitlesh
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

            content: jQuery('#cotentid3'),

        });
        $scope.COMMClick = function (C, idx) {

            $scope.obj.Class = C.Class;
            $scope.obj.ClassTitle = C.ClassTitle;
            $scope.obj.Unspsc = C.Commodity;
            $scope.obj.CommodityTitle = C.CommodityTitle;
            $scope.ClassTitlesh = null;
            mymodal.close();


        }
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
                        $scope.obj.RequestId = $scope.rid;
                        formData.append("obj", angular.toJson($scope.obj));

                        formData.append("Attributess", angular.toJson($scope.Attributess));
                        $rootScope.cgBusyPromises = $http({
                            method: "POST",
                            url: '/ServiceMaster/GetclarifyCode1',
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

        $scope.createData = function () {
            
            // alert("inside create function");
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();
            var i = 0;
            angular.forEach($scope.Attributess, function (lst1) {
                if (lst1.Value != "" && lst1.Value != "undefined" && lst1.Value != null) {
                    //  alert("in");
                    i = 1;
                }
            });
            if (i == 1) {

                formData.append("Attributess", angular.toJson($scope.Attributess));

                //$scope.obj.ServiceCategoryName = $("#ServiceCategorycodeSC").find("option:selected").text();
                //$scope.obj.ServiceGroupName = $("#ServiceGroupcodeSC").find("option:selected").text();
                $scope.obj.UomName = $("#ServiceUomcodeSC").find("option:selected").text();
                $scope.obj.ValuationName = $("#ServiceValuationcodeSC").find("option:selected").text();
                $scope.obj.MainCodeName = $("#Service").find("option:selected").text();
                $scope.obj.SubCodeName = $("#Activity").find("option:selected").text();
                $scope.obj.SubSubCodeName = $("#SubSubCodeSC").find("option:selected").text();
                $scope.obj.PlantName = $("#PlantCode").find("option:selected").text();
                $scope.obj._id = $scope._id;
                $scope.obj.RequestId = $scope.rid;
                $scope.obj.ShortDesc = $scope.obj.short;
                $scope.obj.LongDesc = $scope.obj.long;


                if ($scope.UserId != "" && $scope.UserId != null && $scope.UserId != "undefined") {
                    $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                    if ($scope.obj1.Name == "Select Reviewer") {
                        $scope.obj1.UserId = "";
                    }
                    else {
                        $scope.obj1.UserId = $scope.UserId;
                    }
                }
                $scope.obj1.ServiceCategoryName = $scope.obj.ServiceCategoryName;
                $scope.obj1.ServiceGroupName = $scope.obj.ServiceGroupName;

                //if ($scope.obj1.UserId != "" && $scope.obj1.UserId != null && $scope.obj1.UserId != "undefined") {
                //    $scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();
                //}

                //$scope.obj1.Name = $("#Reviewer_id").find("option:selected").text();

                formData.append("obj", angular.toJson($scope.obj));
                formData.append("obj1", angular.toJson($scope.obj1))
                $http({
                    method: "POST",
                    url: '/ServiceMaster/updateServiceReview?SL=' + $scope.SL,

                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data[2] === "true") {
                        $scope.obj = null;
                        $scope.Attributess = null;
                        $scope.UserId = null;
                        $scope.form.$setPristine();
                        $scope.getdatalistforCleanser();
                        $rootScope.Res = "Sent to releaser";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                      
                        $scope.obj.LongDesc = data[1];
                       $scope.obj.ShortDesc = data[0];
                        $scope.Showsubmit = false;
                     
                    }
                    else {
                        $rootScope.Res = "Error on processing try again";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    //// $scope.reset();
                    // $scope.obj = null;
                }).error(function (data, status, headers, config) {
                });
            }
            else {
                alert("mandatory value fields must be filled");
            }
        };

        $scope.getgroupcodeforcatagory = function (catagory) {
            $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + catagory).success(function (response) {
                $scope.getgroupSC = response;
            });

            if (catagory != null && catagory != "undefined" && catagory != "") {
                $http.get('/ServiceMaster/showall_MainCode1?catagory=' + catagory).success(function (maincode) {
                    $scope.getmainSC = maincode;
                });
            }
        };
        ////


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
        $scope.searchMaster1 = function (sCode, iCode, sSource, sCategory, sGroup, sUser) {
            //alert("hai");
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
            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.iCode != undefined && $scope.iCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sShort != undefined && $scope.sShort != '') || ($scope.sLong != undefined && $scope.sLong != '') || ($scope.sCategory != undefined && $scope.sCategory != '') || ($scope.sGroup != undefined && $scope.sGroup != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sStatus != undefined && $scope.sStatus != '')) {

                //$(".loaderb_div").show();
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/ServiceMaster/searchMaster1',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    $scope.DataList = response;
                    ////////////
                    $scope.showlist = true;
                    if ($scope.DataList != "" && $scope.DataList != null && $scope.DataList != "undefinded") {

                        $('#0').attr("style", "background-color:lightblue");

                        $scope.sc = $scope.DataList[0].ServiceCode;
                        $scope.rid = $scope.DataList[0].RequestId;
                        $scope._id = $scope.DataList[0]._id;
                        $scope.obj = $scope.DataList[0];
                        $http.get('/ServiceMaster/getdatalistusing_id?_id=' + $scope.DataList[0]["_id"]).success(function (response) {

                            $scope.values1 = response;

                            $http.get('/ServiceMaster/showall_Categoryuser').success(function (catagory) {
                                $scope.getcatSC = catagory;
                            });


                            $http.get('/Master/GetDataListplnt'
                        ).success(function (response) {
                            //alert(angular.toJson(response));
                            $scope.getplant = response;
                            $scope.getplant = $filter('filter')($scope.getplant, { 'Islive': 'true' })
                        }).error(function (data, status, headers, config) {

                        });

                            var i = 0;
                            angular.forEach($scope.DataList, function (lst1) {
                                $('#' + i).attr("style", "");
                                i++;
                            });
                            $('#0').attr("style", "background-color:lightblue");



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



                            if ($scope.values1.ServiceCategoryCode != null && $scope.values1.ServiceCategoryCode != "undefined" && $scope.values1.ServiceCategoryCode != "") {
                                $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.values1.ServiceCategoryCode).success(function (maincode) {
                                    $scope.getmainSC = maincode;
                                });
                            }

                            $http.get('/ServiceMaster/LoadReleaser').success(function (reviewer) {
                                $scope.Reviewer_list = reviewer;
                            });

                            //alert(angular.toJson($scope.values1.PlantCode));
                            if ($scope.values1.PlantCode != null && $scope.values1.PlantCode != "" && $scope.values1.PlantCode != "undefined") {
                                $scope.obj.PlantCode = $scope.values1.PlantCode;
                            }

                            if ($scope.values1.Legacy != null)
                                $scope.obj.legacyy = $scope.values1.Legacy;
                            if ($scope.values1.ServiceCategoryCode != null)
                                $scope.obj.ServiceCategorycode = $scope.values1.ServiceCategoryCode;
                            if ($scope.values1.ServiceCategoryCode != null) {
                                $http.get('/ServiceMaster/getgroupcodeforcatagory?catagory=' + $scope.values1.ServiceCategoryCode).success(function (grpcode) {
                                    $scope.getgroupSC = grpcode;
                                });
                            }

                            //  alert(angular.toJson($scope.values1));

                            if ($scope.values1.ServiceGroupCode != null)
                                $scope.obj.ServiceGroupcode = $scope.values1.ServiceGroupCode;
                            if ($scope.values1.UomCode != null)
                                $scope.obj.UomCode = $scope.values1.UomCode;
                            if ($scope.values1.ShortDesc != null)
                                $scope.obj.short = $scope.values1.ShortDesc;
                            if ($scope.values1.LongDesc != null)
                                $scope.obj.long = $scope.values1.LongDesc;
                            if ($scope.values1.Valuationcode != null)
                                $scope.obj.Valuationcode = $scope.values1.Valuationcode;

                            if ($scope.values1.Releaser != null)
                                $scope.UserId = $scope.values1.Releaser.UserId;
                            else
                                $scope.UserId = null;

                            if ($scope.values1.Remarks != null && $scope.values1.Remarks != "") {
                                $scope.obj.Remarks = $scope.values1.Remarks;
                            }
                            else {
                                $scope.obj.Remarks = "";
                            }



                            if ($scope.values1.ServiceCategoryCode != null && $scope.values1.ServiceCategoryCode != "undefined" && $scope.values1.ServiceCategoryCode != "") {
                                $http.get('/ServiceMaster/showall_MainCode1?catagory=' + $scope.values1.ServiceCategoryCode).success(function (maincode) {
                                    $scope.getmainSC = maincode;
                                });
                                $scope.obj.MainCode = $scope.values1.MainCode;
                            }

                            ///

                            ////
                            if ($scope.values1.SubCode != null) {
                                $http.get('/ServiceMaster/getSubList?MainCode=' + $scope.values1.MainCode
                            ).success(function (subsc) {
                                $scope.getsubSC = subsc;

                            }).error(function (data, status, headers, config) {
                            });
                                $scope.obj.SubCode = $scope.values1.SubCode;
                            }

                            if ($scope.values1.SubSubCode != null) {
                                $http.get('/ServiceMaster/GetSubSubList?MainCode=' + $scope.values1.MainCode + '&SubCode=' + $scope.values1.SubCode
                                     ).success(function (subsub) {
                                         $scope.getsubsubSC = subsub;

                                     }).error(function (data, status, headers, config) {
                                     });
                                $scope.obj.SubSubCode = $scope.values1.SubSubCode;
                            }
                            if ($scope.values1.Characteristics) {
                                $scope.Attributess = $scope.values1.Characteristics;
                            }
                            else {
                                $http.get('/ServiceMaster/getAttribute?Noun=' + $scope.obj.Noun + '&Modifier=' + $scope.obj.Modifier).success(function (response) {
                                  if (response != false) {
                                      $scope.Attributess = response;
                                  }
                                  else {
                                      $scope.Attributess = null;
                                  }
                              }).error(function (data, status, headers, config) {
                              });

                            }


                            if ($scope.values1.ServiceStatus === "Review") {
                                $scope.Showsubmit = true;
                            }

                            else if ($scope.values1.ServiceStatus === "QC") {
                                $scope.Showsubmit = true;
                            }

                            else if ($scope.values1.ServiceStatus === "Cleanse") {
                              //  alert("Data sent back to Cleanser");
                                $scope.Showsubmit = false;

                            }
                            else {
                               // alert("Service Data already Submitted");
                                $scope.Showsubmit = false;
                            }




                        });

                        // ----

                    }
                    else {
                        $scope.showlist = false;
                    }
                    /////
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