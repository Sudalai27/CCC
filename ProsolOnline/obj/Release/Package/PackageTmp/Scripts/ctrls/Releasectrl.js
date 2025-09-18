
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy','angular.filter']);

  
    app.controller('CatalogueController', ['$scope', '$http', '$timeout', '$filter', function ($scope, $http, $timeout, $filter) {


        $scope.showlist = false;

                $scope.desctab = "active";
                $scope.desctab1 = "active";              
                $scope.attcount1 = false;
                new jBox('Tooltip', {
                    attach: '#showstatus',
                    //width: 400,
                    //height: 500,                   
                    closeButton: true,
                    //animation: 'zoomIn',
                    theme: 'TooltipBorder',
                    trigger: 'click',
                    width: 600,
                    height: 200,
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



                $scope.checkshort = function (index) {

                    if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {

                        $http({
                            method: 'GET',
                            url: '/GeneralSettings/getVendorAbbrForShortDesc',
                            params: { mfr: $scope.rows[index].Name }
                        }).success(function (response) {

                            if (response != '' && response != false && response != 'false') {
                                $scope.rows[index].shortmfr = response.ShortDescName;
                            }
                            else {
                                $scope.rows[index].shortmfr = "";
                            }

                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });


                        var i = 0;
                        angular.forEach($scope.rows, function (val, key) {
                            if (i === index) {
                                if (val.s != 0) {
                                    val.s = '1';
                                }
                                else {
                                    val.s = '0';
                                }
                            }
                            else {
                                val.s = '0';
                            }
                            i++;
                        });


                    } else {
                        $scope.rows[index].redname = "red";
                        $scope.rows[index].s = '0';
                    }


                };

                $scope.checklong = function (index) {
                    if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                        var tst = $scope.rows[index].l;
                        if (tst != 0) {
                            $scope.rows[index].l = '1';
                        }
                        else {
                            $scope.rows[index].l = '1';
                        }
                    } else {
                        $scope.rows[index].redname = "red";
                        $scope.rows[index].l = '1';
                    }
                };


                $scope.checkvendortype = function (index) {
                    if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {
                        $scope.rows[index].red = "";
                    }
                    else {
                        $scope.rows[index].Name = "";
                        $scope.rows[index].red = "red";
                    }
                };

                $scope.refnochange = function (index) {
                    if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined) {
                        $scope.rows[index].redrefflag = "";
                    }
                    else {
                        $scope.rows[index].RefNo = "";
                        $scope.rows[index].redrefflag = "red";
                    }
                };


                $scope.vebdortypechange = function (index) {
                    if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {
                        var kjkj = 'vendor' + index;
                        var name = $window.document.getElementById(kjkj);
                        name.focus();
                        $scope.rows[index].red = "";
                        $scope.rows[index].red = "";
                        if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined)
                            $scope.rows[index].redname = "";
                        else
                            $scope.rows[index].redname = "";
                    }
                    else {
                        if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                            $scope.rows[index].redname = "";
                            $scope.rows[index].red = "red";
                        }
                        else {
                            $scope.rows[index].redname = "";
                            $scope.rows[index].red = "";
                            $scope.rows[index].s = '0';
                            $scope.rows[index].l = '1';

                        }

                    }

                };


                $scope.refflagchange = function (index) {
                    if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined) {
                        if ($scope.rows[index].Refflag === "DRAWING & POSITION NUMBER") {
                            $scope.rows[index].placeholder = "Drawing,Position no";
                        }
                        else {
                            $scope.rows[index].placeholder = "";
                        }
                        var kjkj = 'refno' + index;
                        var name = $window.document.getElementById(kjkj);
                        name.focus();
                        $scope.rows[index].redrefflag = "";
                        $scope.rows[index].redrefflag = "";
                        if ($scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined)
                            $scope.rows[index].redrefno = "";
                        else
                            $scope.rows[index].redrefno = "";
                    }
                    else {
                        if ($scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined) {
                            $scope.rows[index].redrefno = "";
                            $scope.rows[index].redrefflag = "red";
                        }
                        else {
                            $scope.rows[index].redrefno = "";
                            $scope.rows[index].redrefflag = "";
                        }
                    }
                };


                $scope.vendornameblur = function (index) {
                    //if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {
                    //    $scope.rows[index].red = "";
                    //    if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined)
                    //        $scope.rows[index].redname = "";
                    //    else
                    //        $scope.rows[index].redname = "red";
                    $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                    if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {

                        $scope.rows[index].red = "";
                        if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                            //alert(angular.toJson($scope.rows[index].redname));
                            $http({
                                method: 'GET',
                                url: '/GeneralSettings/FINDVENDORMASTER',
                                params: { mfr: $scope.rows[index].Name }
                            }).success(function (response) {
                                // alert(angular.toJson($scope.rows[index].redname));

                                if (response === false) {
                                    //alert("i");
                                    $scope.rows[index].redname = "red";

                               
                                }
                                else {
                                    $scope.rows[index].redname = "";

                                }

                            });



                        }

                        else
                            $scope.rows[index].redname = "red";
                    }
                    else {
                        //if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                        //}
                        //else {
                        //    $scope.rows[index].s = '1';
                        //    $scope.rows[index].l = '1';

                        //}

                        //$scope.rows[index].red = "";
                        //$scope.rows[index].redname = "";
                    }

                    if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined && $scope.rows[index].s != '0') {
                        $http({
                            method: 'GET',
                            url: '/GeneralSettings/getVendorAbbrForShortDesc',
                            params: { mfr: $scope.rows[index].Name }
                        }).success(function (response) {

                            if (response != '' && response != false && response != 'false') {
                                $scope.rows[index].shortmfr = response.ShortDescName;
                            }
                            else {
                                $scope.rows[index].shortmfr = "";
                            }

                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });

                    }

                };

                $scope.refnoblur = function (index) {
                    if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined) {
                        $scope.rows[index].redrefflag = "";
                        if ($scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined) {

                            if ($scope.rows[index].Refflag === "DRAWING & POSITION NUMBER") {
                                var hh = $scope.rows[index].RefNo;
                                $scope.rows[index].RefNo = hh.replace(/[\s]/g, '');

                                var comma = $scope.rows[index].RefNo.slice(-1);

                                if (comma === ',') {
                                    $scope.rows[index].redrefno = "red";
                                    //$scope.rows[index].RefNo = $scope.rows[index].RefNo.slice(0, -1);
                                }
                                else {
                                    $scope.rows[index].redrefno = "";
                                }
                                if ($scope.rows[index].RefNo.indexOf(',') === -1) {
                                    $scope.rows[index].redrefno = "red";
                                }

                                if (($scope.rows[index].RefNo.split(",").length - 1) > 1) {
                                    $scope.rows[index].redrefno = "red";
                                }
                            }
                            else
                                $scope.rows[index].redrefno = "";
                        }
                        else
                            $scope.rows[index].redrefno = "red";
                    }
                    else {
                        $scope.rows[index].redrefflag = "";
                        $scope.rows[index].redrefno = "";
                    }
                };


                $scope.showuserMap = function (itmcode) {

                    $http({
                        method: 'GET',
                        url: '/User/getItemstatusMap',
                        params: { itmcde: itmcode }
                    }).success(function (response) {

                        $scope.itemstatusLst = response;

                        //angular.forEach($scope.Characteristics, function (value1, key1) {

                        //    angular.forEach(LstObj, function (value2, key2) {

                        //        if (value1.Characteristic === value2.Characteristic) {

                        //            value1.Value = value2.Value;
                        //            value1.UOM = value2.UOM;

                        //        }

                        //    });
                        //});
                        angular.forEach($scope.itemstatusLst, function (val1, key5) {

                            if (val1.UserName === "") {
                                val1.itemshow = false;
                            }
                            else {
                                val1.itemshow = true;
                            }

                        });

                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                }

        $scope.OpenPop = function () {
            $scope.getSimiliar();
            $scope.callpopup();
            $scope.simText = "";
        }

        $scope.callpopup = function () {

            var inx = 0;
            angular.forEach($scope.LstsimItems, function (lst) {
                new jBox('Tooltip', {
                    attach: '#chrId' + inx,
                    //width: 400,
                    //height: 500,                   
                    //closeOnMouseleave: true,
                    //animation: 'zoomIn',

                    theme: 'TooltipBorder',
                    trigger: 'click',
                    width: 400,
                    height: 500,
                    // height: ($(window).height() - 160),
                    adjustTracker: true,
                    closeOnClick: 'body',
                    closeOnEsc: true,
                    animation: 'move',
                    position: {
                        x: 'right',
                        y: 'center'
                    },
                    outside: 'x',
                    content: jQuery('#conId' + inx)
                });
                inx = inx + 1;

            })

            new jBox('Modal', {
                width: 550,
                height: 500,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: 'title',
                closeButton: true,
                content: jQuery('#simItems'),
                //content: $('#simItems').html(),
                title: 'Click here to drag me around',
                overlay: false,
                reposition: false,
                repositionOnOpen: false,
                position: {
                    x: 'right',
                    y: 'right'
                }
            }).open();


        }
        $scope.showlitpopup = function (inx) {


            new jBox('Modal', {
                width: 550,
                height: 500,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: 'title',
                closeButton: true,
                content: jQuery('#conId' + inx),
                //content: $('#simItems').html(),
                title: 'Click here to drag me around',
                overlay: false,

                reposition: false,
                repositionOnOpen: false,
                position: {
                    x: 'right',
                    y: 'right'
                }
            }).open();


        }


        $scope.NMLoad = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Catalogue/GetDBNounList'
            }).success(function (response) {
                $scope.NounList = response;
                $scope.sNoun = "";
                // alert(angular.toJson(response))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.GetUserList = function () {
          //  $scope.NMLoad();
            $http({
                method: 'GET',
                url: '/Catalogue/showall_user'
            }).success(function (response) {
                $scope.UserList = response;

                //  alert(angular.toJson($scope.UserList))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

            //$http({
            //    method: 'GET',
            //    url: '/Catalogue/getReleaserList'
            //}).success(function (response) {
            //    $scope.UserList = response;
            //}).error(function (data, status, headers, config) {
            //    // alert("error");
            //});

        };
        $scope.GetUserList();


        $scope.similarDataFill = function (LstObj) {

            angular.forEach($scope.Characteristics, function (value1, key1) {

                angular.forEach(LstObj, function (value2, key2) {

                    if (value1.Characteristic === value2.Characteristic) {
                        value1.Value = value2.Value;
                        value1.UOM = value2.UOM;
                        value1.Abbrevated = "false";
                        value1.Approve = false;
                    }

                });
            });
        }
        $scope.checkSimilar = function () {
            // var nonNullList=$scope.LstsimItems;
            angular.forEach($scope.LstsimItems, function (value1, key1) {
                var incr = 0;
                angular.forEach($scope.Characteristics, function (value2, key2) {
                    if (value2.Value != null && value2.Value != "") {
                        angular.forEach(value1.Characteristics, function (value1, key1) {
                            if (value1.Characteristic === value2.Characteristic && value1.Value != null && value1.Value != "") {
                                if (value2.Value == value1.Value)
                                    incr = incr + 1;

                            }
                        });

                    }
                });
                value1.ItemStatus = incr;
            });

        }
        $scope.BindPlant = function () {
            $scope.erp = {};
            $http({
                method: 'GET',
                url: '/Catalogue/getplantCode_Name'
            }).success(function (response) {
                $scope.PlantList = response;
                $scope.erp.Plant = response[0].Plantcode;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindPlant();
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
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.sts1 = false;
        $scope.checkSubmit = function () {

            $scope.sts1 = false;
            angular.forEach($scope.DataList, function (value, key) {
                if (value.ItemStatus == 5) {
                    $scope.sts1 = true;
                }
            })
        };

        ///uom////

        $http.get('/Catalogue/getuomlist').success(function (response) {
          //  alert('hi');
            $scope.uomList1 = response;
            //alert(angular.toJson($scope.uomList1))
        });

        $scope.tempPlace = [];
        $scope.ChangeModifier = function () {

            if ($scope.cat.Modifier != null && $scope.cat.Modifier!='') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspsc',
                    params :{Noun: $scope.cat.Noun ,Modifier: $scope.cat.Modifier}
                }).success(function (response) {
                    if (response != '') {
                        $scope.Commodities = response;
                        if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                            $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                        else $scope.cat.Unspsc = $scope.Commodities[0].Class;
                    }
                    else {
                        $scope.Commodities = null;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

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

           
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetForamted',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    $scope.Type = response;
                    if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                        $scope.ven = true;
                        //angular.forEach($scope.rows, function (lst) {
                        //    lst.s = '1';

                        //});
                    }
                    else {
                        $scope.ven = false;
                    }


                });
                //Attribute List
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;
                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            if (value1.Characteristic == 'DESIGNATION') {

                                $http({
                                    method: 'GET',
                                    url: '/Catalogue/FetchNMRelation',
                                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                                }).success(function (response) {

                                    if (response != '') {
                                        $scope.tempPlace = response;
                                        var res = $filter('filter')(response, { 'KeyAttribute': value1.Characteristic }, true);
                                        if (res.length > 0) {

                                            value1.style = "Yes";

                                        }
                                        else {
                                            value1.style = "No";
                                        }
                                    }
                                    else {
                                        $scope.tempPlace = null;
                                    }

                                }).error(function (data, status, headers, config) {


                                });


                            }

                        });
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
            } else {             
                $scope.Characteristics = null;
              
            }

            //   $scope.getSimiliar();           

        };
        $scope.Exceptional = function () {

            if ($scope.cat.Exchk == true) {

                angular.forEach($scope.Characteristics, function (value1) {
                    value1.Mandatory = 'No'
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                }).success(function (response) {
                    if (response != '') {

                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach(response.ALL_NM_Attributes, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {
                                    value1.Mandatory = value2.Mandatory
                                }
                            });
                        });

                    }
                    else {
                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }
                    //   alert(angular.toJson($scope.Characteristics));
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        }
        $scope.getSimiliar = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Catalogue/GetsimItemsList',
                params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
            }).success(function (response) {

                if (response != null && response.length > 0) {
                    $scope.LstsimItems = response;
                   // $scope.checkSimilar();
                }
                else {
                    $scope.LstsimItems = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.loadUMO = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUOMList'
            }).success(function (response) {
                $scope.UOMs = response;
                $scope.uomList1 = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        }
        $scope.LoadData = function () {

            
            $http({
                method: 'GET',
                url: '/Catalogue/GetReleaseDataList'
            }).success(function (response) {
                $scope.DataList = response;

                angular.forEach($scope.DataList, function (lst) {
                    lst.bu = '0';
                   // $scope.sortColumn = "Itemcode";
                    $scope.reverseSort = false;

                    $scope.sortData = function (Column) {
                        $scope.reverseSort = ("Itemcode" == Column) ? !$scope.reverseSort : false;
                        $scope.sortColumn = Column;


                    }

                    $scope.getSortClass = function (Column) {
                        if ($scope.sortColumn == Column) {
                            return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                        }
                        return '';
                    }
                });
                $scope.saveditems = ($filter('filter')($scope.DataList, { 'ItemStatus': '5' })).length;
                $scope.balanceitems = ($filter('filter')($scope.DataList, { 'ItemStatus': '4' })).length;
               // $scope.DataList1 = response;
                $scope.checkSubmit();
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
         
           
        };
        $scope.LoadData();

      
       

        //Attachment
        $scope.fileList = [];
        $scope.attachment = [];
        $scope.imgList = [];

        $scope.LoadFileData = function (files) {
            $scope.fileList = files;

        };
        $scope.addImg = function () {

            if ($scope.imgList === null) {
                $scope.imgList = [];
            }
         

            $scope.attachment.push($scope.fileList[0]);

         
          //  alert(angular.toJson(ext));

            var bytes = $scope.fileList[0].size;
            var k = 1024; //Or 1 kilo = 1000
            var sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB"];
            var i = Math.floor(Math.log(bytes) / Math.log(k));
            var size = parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];

            $scope.imgList.push({ "_id": "0", 'Title': $scope.Title == null ? '' : $scope.Title, 'FileName': $scope.fileList[0].name, 'FileSize': size });
            $('.fileinput').fileinput('clear');
           
           
        };
        $scope.Delfile = function (indx, _id, imgId) {
            if (confirm("Are you sure, deactivate this record?")) {
                $http({
                    method: 'GET',
                    url: '/Catalogue/Deletefile',
                    params: { id: _id, imgId: imgId }
                }).success(function (response) {
                    $scope.AtttachmentList.splice(indx, 1);

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.Downloadfile = function (fileName, type, imgId) {

            $window.open('/Catalogue/Downloadfile?fileName=' + fileName + '&type=' + type + '&imgId=' + imgId);

        };
        $scope.RemoveFile = function (indx) {
            if ($scope.imgList.length > 0) {
                $scope.attachment.splice(indx, 1);
                $scope.imgList.splice(indx, 1);
            }
        };

        //$scope.getmultiplecoderesult = function (code) {
        //   // alert("hai");
        //    // alert(angular.toJson(code));
        //    if (code != undefined && code != "") {
        //        //alert('in');
        //        $scope.DataList = [{ 'Itemcode': " " }];
        //        // $scope.sCode = "";
        //        $scope.sCode1 = [];
        //        $scope.temp1 = [];

        //        if (code != null) {
        //            $scope.temp1 = code.split(/[, " "]/);
        //            angular.forEach($scope.temp1, function (lst) {
        //                // alert(angular.toJson(lst));
        //                angular.forEach($scope.DataList1, function (lst1) {
        //                    //alert(angular.toJson(lst1));
        //                    //alert(angular.toJson(lst1.Itemcode));
        //                    if (lst == lst1.Itemcode) {
        //                        $scope.DataList.push(lst1);
        //                    }
        //                })
        //            });
        //            //$scope.DataList.RemoveRow[0];
        //            $scope.DataList.splice(0, 1);

        //        }
        //    }
        //    else {
        //        $scope.DataList = $scope.DataList1;
        //    }
        //    //  alert(angular.toJson($scope.DataList));

        //    ////////////////////
        //    // alert("in");
        //    //// alert("hai");
        //    // $http({
        //    //     method: 'GET',
        //    //     url: '/Catalogue/getmulticodesearch?sCode=' + $scope.sCode
        //    // }).success(function (response) {
        //    //    // alert(angular.toJson(response));
        //    //     $scope.DataList = response;

        //    // }).error(function (data, status, headers, config) {
        //    //     // alert("error");
        //    // });
        //};


        $scope.createData = function (sts) {


            //if (!$scope.form.$invalid) {               

            //$timeout(function () { $scope.NotifiyRes = false; }, 30000);

            var formData = new FormData();
            for (var i = 0; i < $scope.attachment.length; i++) {
                formData.append('files', $scope.attachment[i]);
            }


            //general             
            $scope.erp.Industrysector_ = $scope.erp.Industrysector != null ? $("#ddlindustry").find("option:selected").text() : null;
            $scope.erp.Materialtype_ = $scope.erp.Materialtype != null ? $("#ddlmaterial").find("option:selected").text() : null;
            $scope.erp.BaseUOP_ = $scope.erp.BaseUOP != null ? $("#ddlbaseuop").find("option:selected").text() : null;
            $scope.erp.Unit_issue_ = $scope.erp.Unit_issue != null ? $("#ddluis").find("option:selected").text() : null;
            $scope.erp.AlternateUOM_ = $scope.erp.AlternateUOM != null ? $("#ddlalteruom").find("option:selected").text() : null;
            $scope.erp.Inspectiontype_ = $scope.erp.Inspectiontype != null ? $("#ddlinstype").find("option:selected").text() : null;
            $scope.erp.Inspectioncode_ = $scope.erp.Inspectioncode != null ? $("#ddlinscode").find("option:selected").text() : null;
            $scope.erp.Division_ = $scope.erp.Division != null ? $("#ddldivision").find("option:selected").text() : null;
            $scope.erp.Salesunit_ = $scope.erp.Salesunit != null ? $("#ddlsaleunit").find("option:selected").text() : null;

            //plant
            $scope.erp.Plant_ = $scope.erp.Plant != null ? $("#ddlPlant").find("option:selected").text() : null;
            $scope.erp.ProfitCenter_ = $scope.erp.ProfitCenter != null ? $("#ddlprofit").find("option:selected").text() : null;
            $scope.erp.StorageLocation_ = $scope.erp.StorageLocation != null ? $("#ddlstoreage").find("option:selected").text() : null;
         //   $scope.erp.StorageBin_ = $scope.erp.StorageBin != null ? $("#ddlbin").find("option:selected").text() : null;
            $scope.erp.ValuationClass_ = $scope.erp.ValuationClass != null ? $("#ddlvclass").find("option:selected").text() : null;
            $scope.erp.PriceControl_ = $scope.erp.PriceControl != null ? $("#ddlprice").find("option:selected").text() : null;
            $scope.erp.ValuationCategory_ = $scope.erp.ValuationCategory != null ? $("#ddlvcat").find("option:selected").text() : null;
            $scope.erp.VarianceKey_ = $scope.erp.VarianceKey != null ? $("#ddlvkey").find("option:selected").text() : null;


            //Mrp data
            $scope.erp.MRPType_ = $scope.erp.MRPType != null ? $("#ddlmrptype").find("option:selected").text() : null;
            $scope.erp.MRPController_ = $scope.erp.MRPController != null ? $("#ddlmrpcontrol").find("option:selected").text() : null;
            $scope.erp.LOTSize_ = $scope.erp.LOTSize != null ? $("#ddllotsize").find("option:selected").text() : null;
            $scope.erp.ProcurementType_ = $scope.erp.ProcurementType != null ? $("#ddlprocurement").find("option:selected").text() : null;
            $scope.erp.PlanningStrgyGrp_ = $scope.erp.PlanningStrgyGrp != null ? $("#ddlplanning").find("option:selected").text() : null;
            $scope.erp.AvailCheck_ = $scope.erp.AvailCheck != null ? $("#ddlavilchk").find("option:selected").text() : null;
            $scope.erp.ScheduleMargin_ = $scope.erp.ScheduleMargin != null ? $("#ddlschedule").find("option:selected").text() : null;

            //Sales & others
            $scope.erp.AccAsignmtCategory_ = $scope.erp.AccAsignmtCategory != null ? $("#ddlasscat").find("option:selected").text() : null;
            $scope.erp.TaxClassificationGroup_ = $scope.erp.TaxClassificationGroup != null ? $("#ddltaxclass").find("option:selected").text() : null;
            $scope.erp.ItemCategoryGroup_ = $scope.erp.ItemCategoryGroup != null ? $("#ddlitemcat").find("option:selected").text() : null;
            $scope.erp.SalesOrganization_ = $scope.erp.SalesOrganization != null ? $("#ddlsales").find("option:selected").text() : null;
            $scope.erp.DistributionChannel_ = $scope.erp.DistributionChannel != null ? $("#ddldistri").find("option:selected").text() : null;
            $scope.erp.MaterialStrategicGroup_ = $scope.erp.MaterialStrategicGroup != null ? $("#ddlmatstra").find("option:selected").text() : null;
            $scope.erp.PurchasingGroup_ = $scope.erp.PurchasingGroup != null ? $("#ddlpurchasegrp").find("option:selected").text() : null;
            $scope.erp.PurchasingValueKey_ = $scope.erp.PurchasingValueKey != null ? $("#ddlpurval").find("option:selected").text() : null;

            //newly added fields
            $scope.erp.TaxClassification2_ = $scope.erp.TaxClassification2 != null ? $("#ddltaxclass2").find("option:selected").text() : null;
            $scope.erp.DeliveringPlant_ = $scope.erp.DeliveringPlant != null ? $("#ddldelplant").find("option:selected").text() : null;
            $scope.erp.TransportationGroup_ = $scope.erp.TransportationGroup != null ? $("#ddltransgrp").find("option:selected").text() : null;
            $scope.erp.LoadingGroup_ = $scope.erp.LoadingGroup != null ? $("#ddlloadgrp").find("option:selected").text() : null;
            $scope.erp.OrderUnit_ = $scope.erp.OrderUnit != null ? $("#ddlorderunit").find("option:selected").text() : null;
            $scope.erp.AutomaticPO_ = $scope.erp.AutomaticPO != null ? $("#ddlatmpo").find("option:selected").text() : null;

            // formData.append('files', $scope.attachment);
            formData.append("itemsts", "5");
            formData.append("cat", angular.toJson($scope.cat));
            formData.append("attri", angular.toJson($scope.Characteristics));
            formData.append("ERP", angular.toJson($scope.erp));

            //formData.append("Generalinfo", angular.toJson($scope.gen));
            //formData.append("Plantinfo", angular.toJson($scope.plant));
            //formData.append("MRPdata", angular.toJson($scope.mrpdata));
            //formData.append("Salesinfo", angular.toJson($scope.sales));
            formData.append("Equ", angular.toJson($scope.Equ));
            formData.append("vendorsupplier", angular.toJson($scope.rows));
            formData.append("Attachments", angular.toJson($scope.imgList));
            formData.append("sts", sts);


            $scope.cgBusyPromises = $http({
                url: "/Catalogue/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }
                else {

                    if (data > -1) {
                        $scope.NotifiyRes = true;
                        if (data == 8) {

                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please add vendor in vendor master"
                            $scope.NotifiyRes = true;
                            $scope.dis = false;
                        } else if (data == 9) {
                            alert(data)
                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please add all values"
                            $scope.NotifiyRes = true;
                            $scope.dis = false;
                        } else if (data == 10) {
                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please approve/add values in value master"
                            $scope.NotifiyRes = true;
                            $scope.dis = false;
                        } else {


                            $scope.dis = false;
                            $scope.reset();
                            $scope.Type = null;
                            $scope.cat = null;
                            $scope.Characteristics = null;
                            $scope.gen = null;
                            $scope.plant = null;
                            $scope.mrpdata = null;
                            $scope.sales = null;
                            $scope.Equ = null;
                            $scope.rows = null;
                            $scope.erp = null;
                            $scope.imgList = null;
                            $scope.Title = null;
                            $scope.AtttachmentList = null;

                            if ($scope.Commodities != null && $scope.Commodities.length > 0) {
                                $scope.Commodities[0].Class = null;
                                $scope.Commodities[0].ClassTitle = null;
                                $scope.Commodities = null;
                            }

                            $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];



                            //  $scope.LoadData();

                            $scope.Notify = "alert-info";
                            if (data == 0) {
                                $scope.fromsave = 1;
                                $scope.attachment = [];
                                // $scope.searchMaster();
                                $scope.Res = "Data saved successfully";
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);
                            }
                            if (data == 1) {
                                $scope.Res = "Data duplicated successfully"
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);
                                // $scope.Res = "Data saved successfully";
                            }
                            if (data == 2) {
                                $scope.Res = "Duplicate data deleted successfully"
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);
                            }

                            if (data == 3) {
                                $scope.Notify = "alert-danger";
                                $scope.Res = "Duplicate data not saved"
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);

                            }


                        }

                        //  $scope.RowClick(lst, idx);

                        // $scope.LoadData();
                    } else {
                        if ($scope.Characteristics === null)
                            $scope.Res = "Please add characteristics"
                        else
                            $scope.Res = "Data save process failed"

                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }

                }

            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
            });

            // }
        };


        $scope.loadmodifier = function (noun) {
            $scope.Type = null;
            $scope.cat.Modifier = null;
            $scope.cat.Exceptional = false;
            $scope.Characteristics = null;
            $scope.cat.RevRemarks = null; $scope.cat.RelRemarks = null; $scope.cat.Remarks = null; $scope.cat.EnrichedValue = null;
            $scope.cat.MissingValue = null;
            $scope.cat.RepeatedValue = null;
            $scope.cat.Longdesc = null;
            $scope.cat.Shortdesc = null;
            $scope.cat.Additionalinfo = null;
            $scope.cat.UOM = null;
            $scope.Equ = null;
            $scope.sts4 = false;
            $scope.sts3 = false;
            $scope.Legacy1 = true;
            $scope.reset();
            $scope.cat.Noun = $scope.cat.Noun.toUpperCase();
            // alert("mod");
            //alert(angular.toJson(noun));
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params : {Noun:noun}
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }
        $scope.RowClick = function (lst, idx) {
            $scope.attcount1 = false;
            $scope.showlist = true;
            $scope.red_commodity = "";
            $scope.Maincode3 = "";
            $scope.Subcode3 = "";
            $scope.Subsubcode3 = "";

            $scope.codeforreject = lst.Itemcode;

            $scope.sts2 = false;
            $scope.sts3 = true;
            $scope.sts4 = false;
            $scope.Partnum = false;
            $('#divPartnum').attr('style', 'display: none');
            $scope.listptnodup = null;

            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUnspsc?Noun=' + lst.Noun + '&Modifier=' + lst.Modifier
            }).success(function (response) {
                if (response != '') {
                    $scope.Commodities = response;
                    if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                        $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                    else $scope.cat.Unspsc = $scope.Commodities[0].Class;
                }
                else {
                    $scope.Commodities = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

            var i = 0;
            angular.forEach($scope.DataList, function (lst1) {
                $('#' + i).attr("style", "");
                i++;
            });
            $('#' + idx).attr("style", "background-color:lightblue");

          
            $http({
                method: 'GET',
                url: '/GeneralSettings/GetHsn?Noun=' + lst.Noun + '&Modifier=' + lst.Modifier
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
                url: '/Catalogue/GetERPInfo',
                params: { itemcode: lst.Itemcode }
            }).success(function (response) {
                $scope.erp = response;
                if ($scope.erp.Plant == null || $scope.erp.Plant == '')
                    $scope.erp.Plant = $scope.PlantList[0].Plantcode;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

            //Attachments
            $http({
                method: 'GET',
                url: '/Catalogue/GetAttachment?itemcode=' + lst.Itemcode
            }).success(function (response) {
                $scope.AtttachmentList = response;
                if (response.length > 0) {
                    $scope.attcount = response.length;
                    $scope.attcount1 = true;
                }
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            $scope.cat = {};

            $http({
                method: 'GET',
                url: '/Catalogue/GetSingleItem',
                params: { itemcode: lst.Itemcode }
            }).success(function (response) {

                $scope.cat = response;
                $scope.Equ = response.Equipment;

            

                if ($scope.cat.Unspsc != null || $scope.cat.Unspsc != "") {
                    $http({
                        method: 'GET',
                        url: '/GeneralSettings/GetUnspsc',
                        params: { Noun: lst.Noun, Modifier: lst.Modifier }
                    }).success(function (response) {
                        if (response != '') {
                            $scope.Commodities = response;
                            if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                                $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                            else $scope.cat.Unspsc = $scope.Commodities[0].Class;
                        }
                        else {
                            $scope.Commodities = null;
                        }

                    }).error(function (data, status, headers, config) {
                        // alert("error");

                    });
                }

                var tst = response.Vendorsuppliers;
                if (tst != null && tst != '') {
                    $scope.rows = response.Vendorsuppliers;
                } else {
                    $scope.rows = null;
                    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                }



                $http({
                    method: 'GET',
                    url: '/Dictionary/GetModifier',
                    params: { Noun: lst.Noun }
                }).success(function (response) {
                    var flg = 0;
                    $scope.Modifiers = response;

                    angular.forEach($scope.Modifiers, function (obj) {
                        if (obj.Modifier == lst.Modifier)
                            flg = 1;
                    });
                    if (flg == 0) {
                        $scope.cat.Modifier = '';

                    }
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

                ///  $scope.getSimiliar();
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetForamted',
                    //?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    $scope.Type = response;
                    if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                        $scope.ven = true;
                        //angular.forEach($scope.rows, function (lst) {
                        //    lst.s = '1';

                        //});
                    }
                    else {
                        $scope.ven = false;
                    }


                });
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;
                        angular.forEach($scope.Characteristics, function (value1, key1) {
                            if (value1.Characteristic == 'DESIGNATION') {
                                $http({
                                    method: 'GET',
                                    url: '/Catalogue/FetchNMRelation',
                                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                                }).success(function (response) {

                                    if (response != '') {
                                        $scope.tempPlace = response;
                                        var res = $filter('filter')(response, { 'KeyAttribute': value1.Characteristic }, true);
                                        if (res.length > 0) {

                                            value1.style = "Yes";

                                        }
                                        else {
                                            value1.style = "No";
                                        }

                                    }
                                    else {
                                        $scope.tempPlace = null;
                                    }

                                })


                            }
                        });
                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });

                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach($scope.cat.Characteristics, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {

                                    value1.Value = value2.Value;
                                    value1.UOM = value2.UOM;
                                    value1.Source = value2.Source;
                                    value1.SourceUrl = value2.SourceUrl;
                                    value1.Abbrevated = value2.Abbrevated;
                                    value1.Approve = value2.Approve;
                                    value1.btnName = "Approve";
                                    if ($scope.cat.Exchk == true)
                                        value1.Mandatory = 'No';
                                }

                            });
                        });
                    }
                    else {

                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }).error(function (data, status, headers, config) {
                // alert("error");

            });






        };

        $scope.bumu = '0';


        $scope.chkmuall = function () {
           // alert(angular.toJson($scope.bumu));

            if ($scope.bumu === 1) {
                angular.forEach($scope.DataList, function (value1, key1) {
                    if (value1.bu != 1)
                        value1.bu = '1';
                });
            }
            else {
                angular.forEach($scope.DataList, function (value1, key1) {
                    if (value1.bu != 0)
                        value1.bu = '0';
                });
            }
            // alert(angular.toJson($scope.DataList));
        };

     
        $scope.SubmitData = function () {
          
                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                //if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1)
                //{
                //    $scope.DataList1 = $filter('filter')($scope.DataList, { 'bu': '1' })
                //}
                //else
                //{
                //    $scope.DataList2 = $filter('filter')($scope.DataList1, { 'ItemStatus': '5' })
            //}
                var datalst = [];
                if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                    datalst = $filter('filter')($scope.DataList, { 'bu': '1' })

                }

                datalst = $filter('filter')(datalst, { 'ItemStatus': '5' });

                if (datalst != "" && datalst != undefined) {
                 
                    var formData = new FormData();

                    formData.append("DataList", angular.toJson(datalst));

                    $scope.cgBusyPromises = $http({
                        url: "/Catalogue/InsertReleaseDataList",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {

                        if (data =="Success") {

                            $scope.reset();
                            $scope.Type = null;
                            $scope.cat = null;
                            $scope.Characteristics = null;
                            $scope.Equ = null;
                            $scope.sts4 = false;
                            $scope.sts3 = false;
                            $scope.Res = " Data submitted successfully"
                            $scope.showlist = false;
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            // $scope.searchMaster();
                             $scope.LoadData();
                        } else {
                            $scope.Res = data;
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                        }



                    }).error(function (data, status, headers, config) {
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    });

                }
                else {
                    $scope.Res = "Select saved items to submit";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                 //   alert("Select saved items to submit");
                    $scope.LoadData();
                }
            
        };
        $scope.LoadMasterData = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetVendortypeList'
            }).success(function (response) {
                $scope.VendortypeList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetReftypeList'
            }).success(function (response) {
                $scope.ReftypeList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        $scope.LoadMasterData();
        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
        $scope.addRow = function () {

            //var flg = 0;
            //angular.forEach($scope.rows, function (value, key) {
            //    if (value.Name == null || value.Name == '') {

            //        flg = 1;
            //    }
            //});
            //if (flg == 0) {
            //    $scope.rows.push({ 'slno': $scope.rows.length + 1, 's': '0', 'l': '1' });

            //}
            $scope.rows.push({ 'slno': $scope.rows.length + 1, 's': '1', 'l': '1' });
            // alert(angular.toJson($scope.rows));

        };

        $scope.RemoveRow = function (indx) {

            // alert("hi")
            //if ($scope.rows[indx].s != 1)
            //{
            if ($scope.rows.length > 1) {
                $scope.rows.splice(indx, 1);
                //  $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];
            }

            if ($scope.rows.length === 1 && indx === 0) {
                $scope.rows.splice(indx, 1);
                $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
            }

            var cunt = 1;
            angular.forEach($scope.rows, function (value, key) {
                value.slno = cunt++;

            });
            //    }
            //else
            //{
            //    $timeout(function () {
            //        $('#divNotifiy').attr('style', 'display: none');
            //    }, 5000);

            //    $scope.Res = "You can't remove this row, coz it will appear in Short Desc.";
            //    $scope.Notify = "alert-info";
            //    $('#divNotifiy').attr('style', 'display: block');

            //}
        };


        //$scope.checkPartno = function () {

        //    $http({
        //        method: 'GET',
        //        url: '/Catalogue/checkPartno?Partno=' + $scope.cat.Partno + '&icode=' + $scope.cat.Itemcode
        //    }).success(function (response) {
        //        if (response != '') {
        //            $scope.listptnodup = response;
        //            $scope.showModal = !$scope.showModal;
        //            $('#divPartnum').attr('style', 'display: block');
        //        }
        //        else {
        //            $('#divPartnum').attr('style', 'display: none');
        //            $scope.listptnodup = response;
        //        }

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");

        //    });

        //};


        $scope.checkPartno = function (index) {



            if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined) {
                $scope.rows[index].redrefflag = "";
                if ($scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined) {

                    if ($scope.rows[index].Refflag === "DRAWING & POSITION NUMBER") {
                        var hh = $scope.rows[index].RefNo;
                        $scope.rows[index].RefNo = hh.replace(/[\s]/g, '');

                        var comma = $scope.rows[index].RefNo.slice(-1);

                        if (comma === ',') {
                            $scope.rows[index].redrefno = "red";
                            //$scope.rows[index].RefNo = $scope.rows[index].RefNo.slice(0, -1);
                        }
                        else {
                            $scope.rows[index].redrefno = "";
                        }
                        if ($scope.rows[index].RefNo.indexOf(',') === -1) {
                            $scope.rows[index].redrefno = "red";
                        }

                        if (($scope.rows[index].RefNo.split(",").length - 1) > 1) {
                            $scope.rows[index].redrefno = "red";
                        }
                    }
                    else
                        $scope.rows[index].redrefno = "";
                }
                else
                    $scope.rows[index].redrefno = "red";
            }
            else {
                $scope.rows[index].redrefflag = "";
                $scope.rows[index].redrefno = "";
            }




            if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined && $scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined) {

               // alert(angular.toJson($scope.rows[index].Refflag));
                $http({
                    method: 'GET',
                    url: '/Catalogue/checkPartno',
                    params: { Partno: $scope.rows[index].RefNo, icode: $scope.cat.Itemcode, Flag: $scope.rows[index].Refflag }
                }).success(function (response) {
                    if (response != '') {
                      //  alert(angular.toJson(response));
                        $scope.listptnodup = response;
                        $scope.showModal = !$scope.showModal;
                        $scope.Partnum = true;
                        $('#divPartnum').attr('style', 'display: block');
                    }
                    else {
                        $scope.Partnum = false;
                        $('#divPartnum').attr('style', 'display: none');
                        $scope.listptnodup = response;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }

        };
        $scope.plantintab = "active";
        $scope.plantintab1 = "active active-box";

        $scope.checkDuplicate = function (form) {
            if (form != false) {

                var formData = new FormData();
                formData.append("cat", angular.toJson($scope.cat));
                formData.append("attri", angular.toJson($scope.Characteristics));
                formData.append("vendorsupplier", angular.toJson($scope.rows));
                formData.append("Equ", angular.toJson($scope.Equ));

                if ($scope.codelogic1 != "UNSPSC Code") {
                    if ($scope.codelogic1 == "Item Code") {
                        $scope.red_commodity = "";
                        $scope.Maincode3 = "";
                        $scope.Subcode3 = "";
                        $scope.Subsubcode3 = "";

                        $http({
                            url: "/Catalogue/checkDuplicate",
                            method: "POST",
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData
                        }).success(function (response) {

                            if (response != '') {
                                $scope.listptnodup1 = response;

                                $('#divduplicate').attr('style', 'display: block');
                            }
                            else {
                                $scope.createData('No');

                                $('#divduplicate').attr('style', 'display: none');
                                $scope.listptnodup1 = response;
                            }


                        }).error(function (data, status, headers, config) {

                        });
                    }
                    else {


                        $scope.red_commodity = "";
                        $scope.Maincode3 = "";
                        $scope.Subcode3 = "";
                        $scope.Subsubcode3 = "";

                        $http({
                            url: "/Catalogue/checkDuplicate",
                            method: "POST",
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData
                        }).success(function (response) {
                            if (response != '') {
                                $scope.listptnodup1 = response;

                                $('#divduplicate').attr('style', 'display: block');
                            }
                            else {
                                $scope.createData('No');

                                $('#divduplicate').attr('style', 'display: none');
                                $scope.listptnodup1 = response;
                            }


                        }).error(function (data, status, headers, config) {

                        });

                    }

                }
                else {
                    if ($scope.cat.Unspsc != "" && $scope.cat.Unspsc != null && $scope.cat.Unspsc != undefined) {
                        $scope.red_commodity = "";
                        $scope.Maincode3 = "";
                        $scope.Subcode3 = "";
                        $scope.Subsubcode3 = "";

                        $http({
                            url: "/Catalogue/checkDuplicate",
                            method: "POST",
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData
                        }).success(function (response) {
                            if (response != '') {
                                $scope.listptnodup1 = response;
                                // $scope.red_maincode = "Border: 1px solid #a94442";
                                $('#divduplicate').attr('style', 'display: block');
                            }
                            else {
                                $scope.createData('No');

                                $('#divduplicate').attr('style', 'display: none');
                                $scope.listptnodup1 = response;
                            }

                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });
                    }
                    else {
                        // alert("hi");
                        $scope.unspscact = "active";

                        $scope.desctab = "";

                        $scope.planttab = "";
                        $scope.red_commodity = "Border: 1px solid #a94442";
                        // $scope.red_commodity = "";
                        $scope.Maincode3 = "";
                        $scope.Subcode3 = "";
                        $scope.Subsubcode3 = "";

                    }
                }
            //}
            //else {
            //    alert("Error found at MFR/SPLR section");
            //}
            } else {

                angular.element('input.ng-invalid,select.ng-invalid').first().focus();
                var contactForm = document.getElementById('ganForm');
                var list = contactForm.querySelectorAll(':invalid');
                //  alert(angular.toJson(list[0].getAttribute("name")));
                for (var item of list) {
                // item.setAttribute("style", "background-color: red;")
                    var nme = item.getAttribute("name");
                    if (nme == "ddlPlant1" || nme == "ddlprofit1" || nme == "ddlstoreage1" || nme == "ddlstbin" || nme == "ddlvclass1" || nme == "ddlprice1" || nme == "ddlmaterial1") {
                        $scope.unspscact = "";
                        $scope.desctab = "";
                        $scope.planttab = "active";
                        $scope.plantintab = "active";
                        $scope.plantintab1 = "active active-box";
                        $scope.generalintab = "";
                        $scope.generalintab1 = "";
                        $scope.mrpintab = "";
                        $scope.mrpintab1 = "";
                        $scope.salesintab = "";
                        $scope.salesintab1 = "";
                        break;

                    } else if (nme == "ddlindustry1" || nme == "ddldivision1") {
                        $scope.unspscact = "";
                        $scope.desctab = "";
                        $scope.planttab = "active";
                        $scope.plantintab = "";
                        $scope.plantintab1 = "";
                        $scope.generalintab = "active";
                        $scope.generalintab1 = "active active-box";
                        $scope.mrpintab = "";
                        $scope.mrpintab1 = "";
                        $scope.salesintab = "";
                        $scope.salesintab1 = "";
                        break;

                    }
                    else if (nme == "ddlmrptype1" || nme == "ddlmrpcontrol1" || nme == "ddllotsize1" || nme == "ddlprocurement1") {
                        $scope.unspscact = "";
                        $scope.desctab = "";
                        $scope.planttab = "active";
                        $scope.plantintab = "";
                        $scope.plantintab1 = "";
                        $scope.generalintab = "";
                        $scope.generalintab1 = "";
                        $scope.mrpintab = "active";
                        $scope.mrpintab1 = "active active-box";
                        $scope.salesintab = "";
                        $scope.salesintab1 = "";
                        break;

                    }
                    else if (nme == "ddltaxclass1" || nme == "ddltaxclass2" || nme == "ddlatmponame" || nme == "ddltaxclass11" || nme == "ddltaxclass21" || nme == "ddlatmponame1"
                        || nme == "ddlitemcat1" || nme == "ddlsales1" || nme == "ddldistri1" || nme == "ddlmatstra1" || nme == "ddlpurchasegrp1" || nme == "saltxt") {
                        $scope.unspscact = "";
                        $scope.desctab = "";
                        $scope.planttab = "active";
                        $scope.plantintab = "";
                        $scope.plantintab1 = "";
                        $scope.generalintab = "";
                        $scope.generalintab1 = "";
                        $scope.mrpintab = "";
                        $scope.mrpintab1 = "";
                        $scope.salesintab = "active";
                        $scope.salesintab1 = "active active-box";
                        break;

                    }
                }
                alert("Please fill the highlighted mandatory field(s)");
            }

        };



        $scope.writeData = function () {
            $scope.createData('Yes');
          
            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;
        };
        $scope.NodupData = function () {
            $scope.createData('No');
            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;
        };

        $scope.closeData = function () {
            $('#divPartnum').attr('style', 'display: none');
            $scope.listptnodup = null;
        };
        $scope.closeData1 = function () {
            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;
        };
        $scope.fillData = function (ls) {

            // $scope.cat = {};
            $http({
                method: 'GET',
                url: '/Catalogue/GetSingleItem',
                params: { itemcode: ls.Itemcode }
            }).success(function (response) {

                //$scope.cat = response;
                $scope.cat.Noun = response.Noun;
                $scope.cat.Modifier = response.Modifier;
                $scope.cat.Shortdesc = response.Shortdesc;
                $scope.cat.Longdesc = response.Longdesc;
                $scope.cat.Additionalinfo = response.Additionalinfo;
                $scope.cat.Junk = response.Junk;
                $scope.cat.Manufacturer = response.Manufacturer;
                $scope.cat.Application = response.Application;
                $scope.cat.Drawingno = response.Drawingno;
                $scope.cat.Referenceno = response.Referenceno;
                $scope.cat.Remarks = response.Remarks;
                $scope.cat.Characteristics = response.Characteristics;

                $scope.cat.Maincode = $scope.cat.Maincode;
             

                //Equipment
                $scope.Equ = response.Equipment;

                //Vendor

                var tst = response.Vendorsuppliers;
                if (tst != null && tst != '') {
                    $scope.rows = response.Vendorsuppliers;
                } else {
                    $scope.rows = null;
                    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                }

                //UNSPSC
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspsc',
                    params :{Noun:+ $scope.cat.Noun ,Modifier: $scope.cat.Modifier}
                }).success(function (response) {
                    if (response != '') {
                        $scope.Commodities = response;
                        if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                            $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                        else $scope.cat.Unspsc = $scope.Commodities[0].Class;
                    }
                    else {
                        $scope.Commodities = null;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                $http({
                    method: 'GET',
                    url: '/Dictionary/GetModifier',
                    params: { Noun: $scope.cat.Noun }
                }).success(function (response) {
                    $scope.Modifiers = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetForamted',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    $scope.Type = response;
                    if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                        $scope.ven = true;
                        //angular.forEach($scope.rows, function (lst) {
                        //    lst.s = '1';

                        //});
                    }
                    else {
                        $scope.ven = false;
                    }


                });


                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;
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
                            // alert("error");
                        });

                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach($scope.cat.Characteristics, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {

                                    value1.Value = value2.Value;
                                    value1.UOM = value2.UOM;

                                }

                            });
                        });
                    }
                    else {

                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        };
        $scope.Uomalert = function (Uom, indx) {

            if (Uom != undefined) {
                var inxx = 0;

                angular.forEach($scope.Characteristics, function (value1, key1) {
                    if (value1.UOM != undefined) {
                        var res = $filter('filter')($scope.UOMs, { 'Attribute': value1.Characteristic }, true);

                        angular.forEach(res, function (value2, key2) {
                            // alert(angular.toJson(value2.UOMList))
                            if (value2.UOMList != undefined && value2.UOMList.indexOf(Uom) != -1 && value1.UOM != Uom) {

                                $('#U' + inxx).attr('style', 'display:block;border:2px solid #f7ac02;border-radius: 10px;');
                            } else {
                                $('#U' + inxx).attr('style', 'display:block;background:none;');
                            }
                        });

                    }
                    inxx = inxx + 1;
                });
            }
        }
        $scope.SelectDesg = function (Noun, Modifier, Attribute) {


            $http({
                method: 'GET',
                url: '/Catalogue/GetValues',

                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute }
            }).success(function (response) {
                $scope.Desg = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };
        $scope.Reworkbtn = function () {
        //    alert("hai");
            //if (!$scope.form.$invalid) {               
            if (confirm("Are you sure, rework this record?")) {
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Catalogue/Reworkrelease',
                  //  ?itemcode=' + $scope.cat.Itemcode + '&RelRemarks=' + $scope.cat.RelRemarks
                    params:{itemcode:$scope.cat.Itemcode ,RelRemarks: $scope.cat.RelRemarks}
                }).success(function (data, status, headers, config) {
                    if (data.success === false) {

                        $scope.Res = data.errors;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }
                    else {

                        if (data === true) {
                            $scope.reset();
                            $scope.Type = null;
                            $scope.cat = null;
                            $scope.Characteristics = null;
                            $scope.gen = null;
                            $scope.plant = null;
                            $scope.mrpdata = null;
                            $scope.sales = null;
                            $scope.Equ = null;
                            $scope.rows = null;

                            $scope.imgList = null;
                            $scope.Title = null;
                            $scope.AtttachmentList = null;

                            if ($scope.Commodities != null && $scope.Commodities.length > 0) {
                                $scope.Commodities[0].Class = null;
                                $scope.Commodities[0].ClassTitle = null;
                                $scope.Commodities = null;
                            }

                            $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];

                            $scope.Res = "Record sent to rework"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.searchMaster();
                          //  $scope.LoadData();
                        } else {
                            $scope.Res = "Record sending fail to rework"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                        }

                    }

                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                });
            }
        };
        $scope.SelectCharater = function (Noun, Modifier, Attribute, inx) {

            $http({
                method: 'GET',
                url: '/Catalogue/GetValues',

                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute }
            }).success(function (response) {

                $("#div" + inx).addClass("sample-arrow");

                $("#values" + inx).autocomplete({

                    minLength: 0,
                    source: response,
                    select: function (event, ui) {
                        $scope.Characteristics[inx].Value = ui.item.label;
                    }
                }).autocomplete("search", "");
            }).error(function (data, status, headers, config) {
                // alert("error");
            });




            //  $("#values" + inx).click(function () { $(this) });


        };


        // Material Code
        $scope.BindGroupcodeList = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetGroupcodeList'
            }).success(function (response) {
                $scope.Groupcodes = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindGroupcodeList();
        $scope.getSubcode = function () {


            if ($scope.cat.Maincode != null)
                $scope.Maincode3 = "";

            $http.get('/GeneralSettings/GetSubGroupcodeList1', { params: { MainGroupCode: $scope.cat.Maincode }}
           ).success(function (response) {
               $scope.getsubgroup = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }
        $scope.getSubsubcode = function () {

            if ($scope.cat.Subcode != null)
                $scope.Subcode3 = "";

            $http.get('/GeneralSettings/GetSubsubGroupcodeList', { params: { SubGroupCode: $scope.cat.Subcode }}
           ).success(function (response) {
               $scope.getSubsubgroup = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }

        $scope.subsubcodechange = function () {
            if ($scope.cat.Subsubcode != null)
                $scope.Subsubcode3 = "";
            // else

        };

        //Code Logic
        $scope.GetCodeLogic = function () {
            $http({
                method: 'GET',
                url: '/Dictionary/Showdata'
            }).success(function (response) {
                $scope.codelogic1 = response.CODELOGIC;
                if (response.CODELOGIC === "Customized Code") {
                    $scope.codeLogic = true;
                    $scope.req_maincode = true;

                }
                else {
                    $scope.codeLogic = false;
                    $scope.req_maincode = false;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.GetCodeLogic();

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {

           // Value = Value.replace('&', '***');
            if (Value != null && Value != '') {

                if (Attribute == "TYPE" || Attribute == "MATERIAL") {
                    angular.forEach($scope.Commodities, function (value1, key1) {

                        if (value1.value === Value) {
                            if (value1.Commodity != null && value1.Commodity != "")
                                return $scope.cat.Unspsc = value1.Commodity;
                            else return $scope.cat.Unspsc = value1.Class;
                            ;
                        }

                    });
                }


                var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value });

                if (res != null && res != '') {

                    if (res[0].KeyAttribute === Attribute && res[0].KeyValue === Value) {

                      
                        angular.forEach($scope.Characteristics, function (value2, key2) {
                           
                            angular.forEach(res[0].Characteristics, function (value1, key1) {

                                if (value1.Characteristic === value2.Characteristic) {

                                    value2.Value = value1.Value;
                                    value2.UOM = value1.UOM;
                                  

                                }

                            });
                          
                        });


                    }
                }

                $http({
                    method: 'GET',
                    url: '/Catalogue/CheckValue',
                 //   ?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value'
                    params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value }
                }).success(function (response) {



                    if (response == 'false') {
                        $scope.Characteristics[indx].Abbrevated = '';
                        $scope.Characteristics[indx].btnName = 'Add';
                        $scope.Characteristics[indx].Approve = true;

                    }
                    else {
                        $scope.Characteristics[indx].Abbrevated = response;


                        $http({
                            method: 'GET',
                            url: '/Catalogue/CheckValApprove',
                            params:{Value:Value}
                        }).success(function (response) {

                            if (response == "false") {
                                $scope.Characteristics[indx].btnName = 'Approve';
                                $scope.Characteristics[indx].Approve = true;
                            }
                            else {

                                $scope.Characteristics[indx].Approve = false;

                            }

                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });


                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                $http({
                    method: 'GET',
                    url: '/Catalogue/getunitforvalue',
                    params: { Value: Value }
                }).success(function (response) {
                    if (response != null) {
                        $scope.Characteristics[indx].UOM = response;
                    }


                }).error(function (data, status, headers, config) {

                });
            } else {

                $('#checkval' + indx).attr('style', 'display:none');
            }
        }

        $scope.ApproveValue = function (Noun, Modifier, Attribute, Value, Abbrevated, indx) {
           // Value = Value.replace('&', '***');
           // Abbrevated = Abbrevated.replace('&', 'AND');
            $http({
                method: 'GET',
                url: '/Catalogue/ApproveValue',
               // ?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value + '&abb=' + Abbrevated
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: Abbrevated }
            }).success(function (response) {
                if (response === false) {

                    $scope.Characteristics[indx].Approve = true;
                }
                else {

                    $scope.Characteristics[indx].Approve = false;

                }



            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        //$scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {

        //    if (Value != null && Value != '') {

        //        var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value });

        //        if (res != null && res != '') {

        //            if (res[0].KeyAttribute === Attribute && res[0].KeyValue === Value) {


        //                angular.forEach($scope.Characteristics, function (value2, key2) {

        //                    angular.forEach(res[0].Characteristics, function (value1, key1) {

        //                        if (value1.Characteristic === value2.Characteristic) {

        //                            value2.Value = value1.Value;

        //                        }

        //                    })
        //                });


        //            }
        //        }

        //        $http({
        //            method: 'GET',
        //            url: '/Catalogue/CheckValue?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value
        //        }).success(function (response) {

        //            if (response === "false") {
        //                $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
        //                $('#checkval' + indx).attr('style', 'display:block');
        //                $scope.Characteristics[indx].Abbrivation = " ";
        //            }
        //            else {
        //                $scope.Characteristics[indx].Abbrivation = response;

        //                $http({
        //                    method: 'GET',
        //                    url: '/Catalogue/CheckValue1?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value

        //                }).success(function (response) {
        //                    if (response === "false") {

        //                        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
        //                        $('#checkval' + indx).attr('style', 'display:block');

        //                    }
        //                    else {
        //                        $('#btnabv' + indx).attr('style', 'display:none');
        //                        $('#checkval' + indx).attr('style', 'display:block');
        //                        //  $scope.Characteristics[indx].Abbrivation = $scope.Characteristics[indx].Abbrivation;
        //                    }
        //                }).error(function (data, status, headers, config) {
        //                    // alert("error");

        //                });

        //            }

        //            //  alert(angular.toJson(response));
        //            //  alert(angular.toJson(indx));

        //        }).error(function (data, status, headers, config) {
        //            // alert("error");

        //        });

        //        //    if (response == "false") {
        //        //        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
        //        //        $('#checkval' + indx).attr('style', 'display:block');
        //        //        $scope.Characteristics[indx].Abbrivation = null;
        //        //    }
        //        //    else {
        //        //        $('#btnabv' + indx).attr('style', 'display:none');
        //        //        $('#checkval' + indx).attr('style', 'display:block');
        //        //        $scope.Characteristics[indx].Abbrivation = response
        //        //    }

        //        //}).error(function (data, status, headers, config) {
        //        //    // alert("error");

        //        //});


        //        $http({
        //            method: 'GET',
        //            url: '/Catalogue/getunitforvalue?Value=' + Value
        //        }).success(function (response) {
        //            if (response != null) {
        //                $scope.Characteristics[indx].UOM = response;
        //            }
        //        }).error(function (data, status, headers, config) {
                   
        //        });

        //    } else {

        //        $('#checkval' + indx).attr('style', 'display:none');
        //    }
        //}
        $scope.Desgdrop = function (Noun, Modifier, Attribute, Value, indx) {
            //  Value = Value.replace('&', '***');

            if (Value != null && Value != '') {

                var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value }, true);
                //    alert(angular.toJson(res[0]))
                if (res != null && res != '') {

                    if (res[0].KeyAttribute === Attribute && res[0].KeyValue === Value) {


                        angular.forEach($scope.Characteristics, function (value2, key2) {

                            angular.forEach(res[0].Characteristics, function (value1, key1) {

                                if (value1.Characteristic === value2.Characteristic) {

                                    value2.Value = value1.Value;
                                    value2.UOM = value1.UOM;
                                }

                            })
                        });


                    }
                }

            }

        };

        $scope.AddValue = function (Noun, Modifier, Attribute, Value, abb, indx) {
            //Value = Value.replace('&', '***');
           // abb = abb.replace('&', '***');
            $http({
                method: 'GET',
                url: '/Catalogue/AddValue',
               // ?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value + '&abb=' + abb
                params : {Noun : Noun ,Modifier:Modifier , Attribute : Attribute , Value : Value , abb:abb }
            }).success(function (response) {
                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:none');
                $scope.Characteristics[indx].Abbrivation = null;

            }).error(function (data, status, headers, config) {
               //  alert("error");

            });
        }

        $scope.CancelValue = function (indx) {
            $('#checkval' + indx).attr('style', 'display:none');
            $scope.Characteristics[indx].Abbrivation = null;
        }
        $scope.Clarbtn = function () {
            $scope.sts3 = false;
            $scope.sts2 = true;

            $scope.sts4 = true;

        }
        //reject
        $scope.RejectData = function  () {
           
           if ($scope.cat.RelRemarks == undefined || $scope.cat.RelRemarks == null) {
                alert(angular.toJson("Please Provide Remarks for Clarification"));
            }
            else {
                if (confirm("Are you sure, send to clarification this record?")) {

                    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                    $scope.cgBusyPromises = $http({
                        method: 'GET',
                        url: '/Catalogue/GetRejectCode2',
                      //  ?Itemcode=' + $scope.codeforreject + '&RevRemarks=' + $scope.cat.RevRemarks + '&usr=' + $scope.usr
                        params: { Itemcode: $scope.codeforreject, RelRemarks: $scope.cat.RelRemarks }
                    }).success(function (response) {
                        $scope.Res = "Record sent to Clarification"
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.reset();
                        $scope.Type = null;
                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.searchMaster();
                        //$scope.LoadData();
                        $scope.sts2 = false;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                }
            }
        };

        //Search 

        $scope.searchMaster = function (sCode, sSource, sNoun, sModifier, sUser) {

            var formData = new FormData();           
            formData.append("sCode", $scope.sCode);
            formData.append("sSource", $scope.sSource);
            formData.append("sShort", $scope.sShort);
            formData.append("sLong", $scope.sLong);
            formData.append("sNoun", $scope.sNoun);
            formData.append("sModifier", $scope.sModifier);
            formData.append("sUser", $scope.sUser);
            formData.append("sStatus", $scope.sStatus);
            formData.append("sType", $scope.sType);
            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sShort != undefined && $scope.sShort != '') || ($scope.sLong != undefined && $scope.sLong != '') || ($scope.sNoun != undefined && $scope.sNoun != '') || ($scope.sModifier != undefined && $scope.sModifier != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sType != undefined && $scope.sType != '') || ($scope.sStatus != undefined && $scope.sStatus != '')) {
             
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Catalogue/searchMaster',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    $scope.DataList = response;

                    //angular.forEach($scope.DataList, function (lst) {
                    //    lst.bu = '0';
                    //});

                    if (response != null && response.length > 0) {

                        $scope.Res = "Search result : " + response.length + " records found";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;

                    } else {
                        $scope.Res = "Records not found";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            } else {
             
               $scope.LoadData();
            }
        }
        $scope.sCode = window.location.search.split("itemId=")[1];
        if ($scope.sCode != undefined) {
            $scope.searchMaster($scope.sCode);
        }
        $scope.Searchhsn = function (hsn) {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Catalogue/getHSNList?sKey=' + hsn
            }).success(function (response) {

                if (response != '') {
                    $scope.HSN = response;
                    mymodal.open();
                    $scope.Res = "Search results : " + response.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;


                } else {

                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            })

        }
        var mymodal = new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',

            overlay: true,
            closeButton: true,

            content: jQuery('#cotentid1'),

        });
        $scope.HSNClick = function (H, idx, Noun, Modifier) {

            $scope.HSNID = H.HSNID;
            $scope.Desc = H.Desc;
            $scope.hsn = null;
            mymodal.close();

            if (Noun != null && Modifier != null) {

                $http({
                    method: "GET",
                    url: '/GeneralSettings/Inserthsn?Noun=' + Noun + '&Modifier=' + Modifier + '&HSNID=' + H.HSNID + '&Desc=' + H.Desc
                })
            }
            else {

                alert("Select Noun And Modifier To Assign HSN Code")
                $scope.HSNShow = false;
                $scope.HSNID = null;
                $scope.Desc = null;
            }


        }
        var mymodal = new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid12'),
            reposition: false,
            repositionOnOpen: false
        });
        $scope.clickToOpen = function (Itemcode) {
           
            $scope.cat1 = {};
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params: { Itmcode: Itemcode }
            }).success(function (response) {
                if (response != '') {
                    $scope.cat1 = response;
                    $scope.img = {};
                    $http({
                        method: 'GET',
                        url: '/Search/GetImage',
                        params: { Noun: $scope.cat1.Noun, Modifier: $scope.cat1.Modifier }
                        //?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                    }).success(function (response) {
                        if (response != '') {
                            $scope.img = response;

                        } else {
                            $scope.img = null;

                        }
                    })

                } else {
                    $scope.cat1 = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
            $scope.erp1 = {};
            $http({
                method: 'GET',
                url: '/Search/GetItemERP',
                params: { Itmcode: Itemcode }
            }).success(function (response) {
                if (response != '') {
                    $scope.erp1 = response;
                    angular.forEach($scope.erp1, function (value1, key1) {
                        if (value1.bu != 1)
                            value1.bu = '1';
                    });

                } else {
                    $scope.cat1 = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            mymodal.open();

        };
        
    }]);
    app.directive('capitalize', function () {
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
                    url: "/Dictionary/AutoCompleteNoun",
                    params :{term: term},
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
                        scope.cat.Noun = selectedItem.item.value;

                        $.get("/Dictionary/GetModifier",{Noun: selectedItem.item.value}).success(function (response) {
                            scope.Modifiers = response;
                            scope.$apply();
                            event.preventDefault();
                        });

                    }
                });

            }

        };
    }]);

    app.factory("AutoCompleteService1", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Catalogue/AutoCompleteVendor",
                    params :{term: term},
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete1", ["AutoCompleteService1", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4),
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.cat.Manufacturer = selectedItem.item.value.Name + (selectedItem.item.value.Name2 = selectedItem.item.value.Name2 == null ? '' : selectedItem.item.value.Name2) + (selectedItem.item.value.Name3 = selectedItem.item.value.Name3 == null ? '' : selectedItem.item.value.Name3) + (selectedItem.item.value.Name4 = selectedItem.item.value.Name4 == null ? '' : selectedItem.item.value.Name4);
                        // scope.cat.Manufacturercode = selectedItem.item.value.Code;
                        //scope.uo.UOM = selectedItem.item.value.Unitname;
                        scope.$apply();
                        event.preventDefault();
                    }
                });

            }

        };
    }]);

    app.factory("AutoCompleteService1", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Catalogue/AutoCompleteVendor",
                    params :{term: term},
                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });
            }
        };
    }]);
    app.directive("autoComplete2", ["AutoCompleteService1", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {

                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4),
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        scope.rw.Name = selectedItem.item.value.Name + (selectedItem.item.value.Name2 = selectedItem.item.value.Name2 == null ? '' : selectedItem.item.value.Name2) + (selectedItem.item.value.Name3 = selectedItem.item.value.Name3 == null ? '' : selectedItem.item.value.Name3) + (selectedItem.item.value.Name4 = selectedItem.item.value.Name4 == null ? '' : selectedItem.item.value.Name4);
                        scope.rw.Code = selectedItem.item.value.Code;
                        //scope.uo.UOM = selectedItem.item.value.Unitname;
                        scope.$apply();
                        event.preventDefault();
                    }
                });

            }

        };
    }]);

})();

