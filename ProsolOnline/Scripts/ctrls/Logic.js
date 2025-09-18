(function () {
    'use strict';
    var app = angular.module('ProsolApp',['ui.bootstrap','cgBusy']);
    app.controller('LogicController', function ($scope, $http, $rootScope, $timeout) {

        $scope.selecteditem = 10;
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
      
   // alert("in");
    $http.get('/Dictionary/GetAttributeList').success(function (response) {
        // alert('hi');
        // alert(angular.toJson(response));
    
        $scope.getAttribute = response;
        $scope.getAttribute1 = response;
        $scope.getAttribute2 = response;
        $scope.getAttribute3 = response;
        // alert(angular.toJson($scope.getAttribute))
    });

    $scope.getunitList = function (AttributeName1) {
        //alert(SeviceCategorycode);
        $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName1 }}
        ).success(function (response) {
            //alert('hi');
          // alert(angular.toJson(response));
            $scope.getunit = response;
           // $scope.getunit1 = response;
            
        }).error(function (data, status, headers, config) {
        });

    };

    $scope.getunitlist1 = function (AttributeName1) {
        //alert(SeviceCategorycode);
        $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName1 }}
        ).success(function (response) {
            //alert('hi');
          //  alert(angular.toJson(response));
            $scope.getunit1 = response;
            // $scope.getunit1 = response;

        }).error(function (data, status, headers, config) {
        });

    };
    $scope.getunitlist2 = function (AttributeName1) {
        //alert(SeviceCategorycode);
        $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName1 }}
        ).success(function (response) {
            //alert('hi');
            // alert(angular.toJson(response));
            $scope.getunit2 = response;
            // $scope.getunit1 = response;

        }).error(function (data, status, headers, config) {
        });

    };
    $scope.getunitlist3 = function (AttributeName1) {
        //alert(SeviceCategorycode);
        $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName1 }}
        ).success(function (response) {
            //alert('hi');
           // alert(angular.toJson(response));
            $scope.getunit3 = response;
            // $scope.getunit1 = response;

        }).error(function (data, status, headers, config) {
        });

    };



    $scope.SearchLogicList = function () {     
        $scope.BindLogicList();
    };

    $scope.BindLogicList = function () {
        //alert("hai");

        if ($scope.srchText != null && $scope.srchText != '') {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dictionary/GetLogicListSearch',
                params: { srchtxt: $scope.srchText, currentPage: 1, maxRows: $scope.selecteditem }
                //?srchtxt=' + $scope.srchText + '&currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
            }).success(function (response) {
                
               
                if (response != '') {
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.PageCount = response.PageCount;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                   // $scope.AttrRel = response.AttrRel;
                   // $scope.Values = response.Values;
                    $scope.AttrRel = response.LOGICList;
                    $scope.len = response.totItem;
                }
                else $scope.AttrRel = null;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        } else {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dictionary/GetLogicList1',
                params: { currentPage: 1, maxRows: $scope.selecteditem }
                //?currentPage=' + 1 + '&maxRows=' + $scope.selecteditem
            }).success(function (response) {
               
                if (response != '') {
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.PageCount = response.PageCount;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.AttrRel = response.LOGICList;
                   // $scope.AttrRel = response.AttrRel;
                   // $scope.Values = response.Values;
                    $scope.len = response.totItem;
                }
                else $scope.AttrRel = null;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

    };

    $scope.BindLogicinx = function (inx) {

      
        if ($scope.srchText != null && $scope.srchText != '') {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dictionary/GetLogicListSearch',
                params: { srchtxt: $scope.srchText, currentPage: inx, maxRows: $scope.selecteditem }
                //?srchtxt=' + $scope.srchText + '&currentPage=' + inx + '&maxRows=' + $scope.selecteditem
            }).success(function (response) {

                if (response != '') {
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.AttrRel = response.LOGICList;
                    //$scope.UOMs = response.UOMList;
                }
                else $scope.AttrRel = null;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        } else {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dictionary/GetLogicList1',
                params: { currentPage: inx, maxRows: $scope.selecteditem }
                //?currentPage=' + inx + '&maxRows=' + $scope.selecteditem
            }).success(function (response) {

                if (response != '') {
                    $scope.numPerPage = $scope.selecteditem;
                    $scope.currentPage = response.CurrentPageIndex;
                    $scope.totItem = response.totItem;
                    $scope.AttrRel = response.LOGICList;
                   // $scope.UOMs = response.UOMList;
                }
                else $scope.AttrRel = null;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        }

    };
    $scope.BindLogicList();

        //////////////////

//$scope.BindList = function () {

//    $http({
//        method: 'GET',
//        url: '/Dictionary/ATTRRELLIST'
//    }).success(function (response) {

//        $scope.AttrRel = response;
//        // alert(angular.toJson($scope.AttrRel));

//    }).error(function (data, status, headers, config) {
//        // alert("error");
//    });

//    $http({
//        method: 'GET',
//        url: '/Dictionary/GetValueList1'
//    }).success(function (response) {
//        //alert("hai");
//       // alert(angular.toJson($scope.response))
//        $scope.Values = response;
//       // alert(angular.toJson($scope.Values))
//    }).error(function (data, status, headers, config) {
//        // alert("error");
//    });


//};

        //$scope.BindList();
    $scope.ddlItems = function () {
        $scope.BindLogicList();
    };

$scope.reset = function () {

    $scope.form.$setPristine();
};

$scope.savedata = function () {
   //alert("hai");


    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

    
    var formData = new FormData();
    formData.append("obj", angular.toJson($scope.obj));

    $scope.cgBusyPromises = $http({
        url: "/Dictionary/insertdata",
        method: "POST",
        headers: { "Content-Type": undefined },
        transformRequest: angular.identity,
        data: formData
    }).success(function (data, status, headers, config) {

        $rootScope.Res = "Data has been Saved successfully"
        $rootScope.Notify = "alert-info";
        $rootScope.NotifiyRes = true;
        $('#divNotifiy').attr('style', 'display: block');
        
        $scope.obj.AttributeName1 = "";
        $scope.obj.value1 = "";
        $scope.obj.Unitname1 = "";
        $scope.obj.AttributeName2 = "";
        $scope.obj.value2 = "";
        $scope.obj.Unitname2 = "";
        $scope.obj.AttributeName3 = "";
        $scope.obj.value3 = "";
        $scope.obj.Unitname3 = "";
        $scope.obj.AttributeName4 = "";
        $scope.obj.value4 = "";
        $scope.obj.Unitname4 = "";
        $scope.BindLogicList();
        $scope.reset();
      

    }).error(function (data, status, headers, config) {
    });
    $scope.btnSubmit = true;
    $scope.btnUpdate = false;

};

$scope.ATTRRELedit = function (x, idx) { 

    var i = 0;
    angular.forEach($scope.obj, function (lst) {
        $('#Attribute' + i).attr("style", "");
        i++;
    });
    $('#Attribute' + idx).attr("style", "background-color:lightblue");

    $scope.obj = {};

    $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName1 } }
    ).success(function (response) {
   $scope.getunit = response;
  }).error(function (data, status, headers, config) {
    });
 //   $scope.obj.Unitname1 = x.Unitname1;
  
 
    $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName2 } }
        ).success(function (response) {
    $scope.getunit1 = response;
   }).error(function (data, status, headers, config) {
        });
   //   $scope.obj.Unitname2 = x.Unitname2;
 
    
    $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName3 }}
          ).success(function (response) {
     $scope.getunit2 = response;
    }).error(function (data, status, headers, config) {
          });
       //   $scope.obj.Unitname3 = x.Unitname3;

    $http.get('/Dictionary/getunitList', { params: { AttributeName1: AttributeName4 }}
      ).success(function (response) {
     $scope.getunit3 = response;
  }).error(function (data, status, headers, config) {
      });
        //  $scope.obj.Unitname4 = x.Unitname4;

    $scope.btnSubmit = false;
    $scope.btnUpdate = true;

    $scope.obj._id = x._id;
    $scope.obj.AttributeName1 = x.AttributeName1;
    $scope.obj.value1 = x.Value1;
    $scope.obj.Unitname1 = x.Unitname1;
    $scope.obj.AttributeName2 = x.AttributeName2;
    $scope.obj.value2 = x.Value2;
    $scope.obj.Unitname2 = x.Unitname2;
    $scope.obj.AttributeName3 = x.AttributeName3;
    $scope.obj.value3 = x.Value3;
    $scope.obj.Unitname3 = x.Unitname3;
    $scope.obj.AttributeName4 = x.AttributeName4;
    $scope.obj.value4 = x.Value4;
    $scope.obj.Unitname4 = x.Unitname4;

};
//alert("hai");
$scope.updateData = function () {
   // alert("hai");
    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
    var formData = new FormData();
    formData.append("obj", angular.toJson($scope.obj));

    $scope.cgBusyPromises = $http({
        url: "/Dictionary/updatedata",
        method: "POST",
        headers: { "Content-Type": undefined },
        transformRequest: angular.identity,
        data: formData
    }).success(function (data, status, headers, config) {

        $rootScope.Res = "Data has been Updated successfully"
        $rootScope.Notify = "alert-info";
        $rootScope.NotifiyRes = true;
        $('#divNotifiy').attr('style', 'display: block');
        $scope.BindLogicList();
        $scope.reset();
        $scope.obj = null;

    }).error(function (data, status, headers, config) {
    });
    $scope.btnSubmit = true;
    $scope.btnUpdate = false;
};
$scope.AttrRelDelete = function (_id) {
    //alert("hai");
    if (confirm("Are you sure, delete this record?")) {

        $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

        $scope.cgBusyPromises = $http({
            method: 'GET',
            url: '/Dictionary/DeleteAttrRel',
            params :{id: _id}

        }).success(function (response) {
            $rootScope.Res = "Data Deleted"
            $rootScope.Notify = "alert-info";
            $rootScope.NotifiyRes = true;
            $('#divNotifiy').attr('style', 'display: block');
            $scope.BindLogicList();
        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    }
};

});

    app.controller('NMAttrRelationship', function ($scope, $http, $rootScope, $timeout) {
        $scope.att ="";
        $scope.attvalue="";
        $scope.Characteristics = null;
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.uid = "";
        $scope.selecteditem = 10;

        $http.get('/Dictionary/GetNoun').success(function (response) {
            // alert('hi');
            $scope.getnoun = response;
      //      alert(angular.toJson($scope.getnoun))
        });



        //$scope.BindList = function () {

        //    $http({
        //        method: 'GET',
        //        url: '/Dictionary/getNMAttributeList'
        //    }).success(function (response) {

        //        $scope.NMAttr = response;
        //        // alert(angular.toJson($scope.AttrRel));

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");
        //    });

        //};
        //$scope.BindList();
        $scope.SearchNMATTRList = function () {
            $scope.BindNMList();
        };
        $scope.BindNMList = function () {
            //alert("hai");

            if ($scope.srchText != null && $scope.srchText != '') {
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/GetNMListSearch',
                    params : {srchtxt:$scope.srchText ,currentPage: 1 ,maxRows: $scope.selecteditem}
                }).success(function (response) {


                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        // $scope.AttrRel = response.AttrRel;
                        // $scope.Values = response.Values;
                        $scope.NMAttr = response.NMList;
                     
                        $scope.len = response.totItem;
                    }
                    else $scope.NMAttr = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/GetNMList1',
                    params:{currentPage:1 ,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.PageCount = response.PageCount;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.NMAttr = response.NMList;
                        // $scope.AttrRel = response.AttrRel;
                        // $scope.Values = response.Values;
                        $scope.len = response.totItem;
                    }
                    else $scope.NMAttr = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }

        };
        $scope.BindNMinx = function (inx) {


            if ($scope.srchText != null && $scope.srchText != '') {

                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/GetNMListSearch',
                    params :{srchtxt: $scope.srchText ,currentPage: inx ,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.NMAttr = response.NMList;
                        //$scope.UOMs = response.UOMList;
                    }
                    else $scope.NMAttr = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {

                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/GetNMList1',
                    params :{currentPage:inx ,maxRows: $scope.selecteditem}
                }).success(function (response) {

                    if (response != '') {
                        $scope.numPerPage = $scope.selecteditem;
                        $scope.currentPage = response.CurrentPageIndex;
                        $scope.totItem = response.totItem;
                        $scope.NMAttr = response.NMList;
                        // $scope.UOMs = response.UOMList;
                    }
                    else $scope.NMAttr = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        };
        $scope.BindNMList();

        $scope.ddlItems = function () {
            $scope.BindNMList();
        };

        $scope.reset = function () {

            $scope.form.$setPristine();
        };

        $scope.getkeyattr = function ( idx) {

          //  alert(angular.toJson(idx));
         //   $scope.Characteristics[idx].check = true;
            $scope.att = $scope.Characteristics[idx].Characteristic;
           // alert($scope.att);

            angular.forEach($scope.Characteristics, function (lst) {
                //alert(lst.Characteristic);
                if(lst.Characteristic == $scope.att)
                {
                   // alert("in");
                    lst.check = true;
                }
                else
                {
                    lst.check = false;
                }
            });

        };


        $scope.getModifier = function (Noun) {
           // alert("in");
            $http.get('/Dictionary/GetModifierValue', { params: { Noun: Noun }}
            ).success(function (response) {
              //  alert('hi');
                 $scope.getmodifier = response;
               //  alert(angular.toJson(getmodifier));
            }).error(function (data, status, headers, config) {
            });

        };

        $scope.GetAttributeList = function () {

           // alert("hai");

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Dictionary/GetNounModifier1',
                params : {Noun: $scope.obj.Noun,Modifier: $scope.obj.Modifier}
               
            }).success(function (response) {
                if (response != '') {
                    $scope.Characteristics = response;

                   //alert(angular.toJson(response));
                    $http({
                        method: 'GET',
                        url: '/Catalogue/GetUnits'
                    }).success(function (response) {
                        $scope.UOMs = response;
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });

                }
                else $scope.Characteristics = null;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        };

        $scope.NMATTRDelete = function (_id) {
            //alert("hai");
            if (confirm("Are you sure, delete this record?")) {
                //$timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                //$timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/DeleteNMAttrRel',
                    params :{id:_id}
                }).success(function (response) {
                   // alert("delete");
                    $rootScope.Res = "Data Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };

        $scope.NMATTRedit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.obj, function (lst) {
                $('#NM-Attribute' + i).attr("style", "");
                i++;
            });
            $('#NM-Attribute' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $http.get('/Dictionary/GetNounValue').success(function (response) {
                $scope.getnoun = response;
            });
            $scope.obj.Noun = x.Noun;

            $http.get('/Dictionary/GetModifierValue?Noun=' + x.Noun
             ).success(function (response) {
        $scope.getmodifier = response;
        }).error(function (data, status, headers, config) {
        });
            $scope.obj.Modifier = x.Modifier;

            $http.get('/Dictionary/GetTableListforkeyatt', { params: { Noun: x.Noun, Modifier: x.Modifier, KeyAttribute: x.KeyAttribute, KeyValue: x.KeyValue }}
        ).success(function (data, status, headers, config) {
            angular.forEach(data.Characteristics, function (lst) {
                if (lst.Characteristic == x.KeyAttribute) {
                    lst.check = true;
                }
            });
            $scope.Characteristics = data.Characteristics;
            $scope.uid = data._id;
     }).error(function (data, status, headers, config) {
    });
            $scope.obj.Characteristics = x.Characteristics;




            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Noun = x.Noun;
            $scope.obj.Modifier = x.Modifier;
            $scope.obj.Characteristics = x.Characteristics;


        };

    $scope.saveNMrelasionship = function () {
        // alert("hai");
       // $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
        //$timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
            angular.forEach($scope.Characteristics, function (lst) {
                //alert(lst.Characteristic);
                if (lst.Characteristic == $scope.att) {
                  //  alert("in");
                    $scope.attvalue = lst.Value;
                }
            });
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            formData.append("uo", angular.toJson($scope.Characteristics));
            formData.append("att", angular.toJson($scope.att));         
            formData.append("attvalue", angular.toJson($scope.attvalue));
            $scope.cgBusyPromises = $http({
                method: "POST",
                url: "/Dictionary/SaveNMAttributeRelationship",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
             
                $rootScope.Res = "Data has been Saved successfully"
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                //$scope.Res = "Data has been Saved successfully"
                //$scope.Notify = "alert-info";
                //$scope.NotifiyRes = true;
                //$('#divNotifiy').attr('style', 'display: block');

                $scope.obj.Noun = "";
                $scope.obj.Modifier = "";
                $scope.Characteristics = null;
                $scope.BindList();
               // $scope.reset();

               }).error(function (data, status, headers, config) {
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
    };

    $scope.updateData = function () {
        // alert("hai");
        $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
       // $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
        angular.forEach($scope.Characteristics, function (lst) {
            //alert(lst.Characteristic);
            if (lst.Characteristic == $scope.att) {
                //  alert("in");
                $scope.attvalue = lst.Value;
            }
        });

       // $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
        var formData = new FormData();
        formData.append("obj", angular.toJson($scope.obj));
        formData.append("uo", angular.toJson($scope.Characteristics));
        formData.append("att", angular.toJson($scope.att));
        formData.append("attvalue", angular.toJson($scope.attvalue));
        formData.append("uid", angular.toJson($scope.uid));

        $scope.cgBusyPromises = $http({
            url: "/Dictionary/UpdateNMAttributeRelationship",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData
        }).success(function (data, status, headers, config) {

            $rootScope.Res = "Data has been Updated successfully"
            $rootScope.Notify = "alert-info";
            $rootScope.NotifiyRes = true;
            $('#divNotifiy').attr('style', 'display: block');
           // $scope.reset();
            $scope.BindList();
            
            $scope.obj = null;

        }).error(function (data, status, headers, config) {
        });
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
    };
    $rootScope.NotifiyResclose = function () {
        $('#divNotifiy').attr('style', 'display: none');
    }

    });

    app.controller('BulkNMATTRController', function ($scope, $http, $timeout, $rootScope) {
       // alert("hai");

        $scope.ShowHide3 = false;
        $scope.files = [];

        $scope.LoadFileData1 = function (files) {
            $scope.$rootScope = false;
            $rootScope.$apply();

            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkNMATTR = function () {
           // alert("in");
            if ($scope.files[0] != null) {


                $scope.ShowHide3 = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Dictionary/NMATTRIBUTE_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                   // alert(data);
                    $scope.ShowHide3 = false;
                    $rootScope.Res = data + " Records uploaded successfully"
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    //if (data === 0)
                    //    $rootScope.Res = "Records already exists"
                    //else $rootScope.Res = data + " Records uploaded successfully"


                    //$rootScope.Notify = "alert-info";
                    //$rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide3 = false;
                    $rootScope.Res = "alert-danger"
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
                    //$rootScope.Res = data;
                    //$rootScope.Notify = "alert-danger";
                    //$rootScope.NotifiyRes = true;


                });
            };
        }

    });


})();