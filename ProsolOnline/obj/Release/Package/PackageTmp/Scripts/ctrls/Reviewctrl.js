
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter']);

    //app.controller('CatalogueController', ['$scope', '$http', '$timeout', '$filter', function ($scope, $http, $timeout, $filter, $window, $location) {
    app.controller('CatalogueController', function ($scope, $http, $timeout, $filter, $window, $location, $rootScope) {

        $scope.showlist = false;


        $scope.desctab = "active";
        $scope.desctab1 = "active";
        $scope.attcount1 = false;
        $scope.checkshort = function (index) {

            if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/getVendorAbbrForShortDesc',
                    //?mfr=' + $scope.rows[index].Name
                    params : {mfr : $scope.rows[index].Name}
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
                            val.s = 1;
                        }
                        else {
                            val.s = 0;
                        }
                    }
                    else {
                        val.s = 0;
                    }
                    i++;
                });


            } else {
                $scope.rows[index].redname = "red";
                $scope.rows[index].s = 0;
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

            $timeout(function () { $scope.NotifiyRes = false; }, 30000);

            if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {

                $scope.rows[index].red = "";
                if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {
                    //alert(angular.toJson($scope.rows[index].redname));
                    $http({
                        method: 'GET',
                        url: '/GeneralSettings/FINDVENDORMASTER',
                        params : {mfr:$scope.rows[index].Name}
                    }).success(function (response) {
                        // alert(angular.toJson($scope.rows[index].redname));

                        if (response === false) {
                         
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
                //    $scope.rows[index].s = '0';
                //    $scope.rows[index].l = '1';

                //}

                $scope.rows[index].red = "";
                $scope.rows[index].redname = "";
            }

            if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined && $scope.rows[index].s != '0') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/getVendorAbbrForShortDesc',
                    params :{mfr: $scope.rows[index].Name}
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
            ///NEW CODE FOR VENDORMASTER

            //if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined && $scope.rows[index].s != '0') {
            //    $http({
            //        method: 'GET',
            //        url: '/GeneralSettings/FINDVENDORMASTER?mfr=' + $scope.rows[index].Name
            //    }).success(function (response) {
            //        //alert(response);
            //        if (response == false) {
            //            $scope.myres1 = true;
            //        } else {
            //            $scope.myres1 = false;
            //        }

            //    }).error(function (data, status, headers, config) {
            //        // alert("error");

            //    });

            //}

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

        $scope.showuserMap = function (itmcode) {

            $http({
                method: 'GET',
                url: '/User/getItemstatusMap',
                params :{itmcde:itmcode}
            }).success(function (response) {

                $scope.itemstatusLst = response;
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
           // $scope.NMLoad();
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

        $scope.OpenPop = function () {
            $scope.getSimiliar();
            $scope.callpopup();
            $scope.simText = "";
        }

        $scope.callpopup = function () {

          

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
                if (value.ItemStatus == 3) {
                    $scope.sts1 = true;
                }
            })
        };
        ///uom////

        $http.get('/Catalogue/getuomlist').success(function (response) {

            $scope.uomList1 = response;
            //    //alert(angular.toJson($scope.uomList1))
        });
        $scope.tempPlace = [];
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
                    params :{Noun:$scope.cat.Noun ,Modifier: $scope.cat.Modifier}
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
                    params : {Noun:$scope.cat.Noun , Modifier:$scope.cat.Modifier}
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
                    url: '/Dictionary/GetNounModifier2',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    console.log(response)
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;

                      

                                //angular.forEach($scope.Characteristics, function (value1, key1) {

                                //    if (value1.Characteristic == 'DESIGNATION') {

                                //        $http({
                                //            method: 'GET',
                                //            url: '/Catalogue/FetchNMRelation',
                                //            params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                                //        }).success(function (response) {

                                //            if (response != '') {
                                //                $scope.tempPlace = response;
                                //                var res = $filter('filter')(response, { 'KeyAttribute': value1.Characteristic }, true);
                                //                if (res.length > 0) {

                                //                    value1.style = "Yes";

                                //                }
                                //                else {
                                //                    value1.style = "No";
                                //                }
                                //            }
                                //            else {
                                //                $scope.tempPlace = null;
                                //            }

                                //        }).error(function (data, status, headers, config) {


                                //        });

                                       
                                //    }

                                //});

                           
                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;
                            // alert(angular.toJson($scope.UOMs));
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

        //$scope.balanceitems = 0;
        //$scope.saveditems = 0;

        $scope.LoadData = function () {

           
                $http({
                    method: 'GET',
                    url: '/Catalogue/GetRDataList'
                }).success(function (response) {
                    $scope.DataList = response;
                    //  $scope.DataList1 = response;

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

                    $scope.saveditems = ($filter('filter')($scope.DataList, { 'ItemStatus': '3' })).length;
                    $scope.balanceitems = ($filter('filter')($scope.DataList, { 'ItemStatus': '2' })).length;

                    //  alert(angular.toJson($scope.DataList));


                    $scope.checkSubmit();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
           
            
        };
        $scope.LoadData();

        $scope.getList = function (indx, vals) {
            $scope.sepVal = [];
            $scope.temp = [];
            $scope.sepVal1 = [];
            if (vals != null) {
                $scope.temp = vals[0].Values.split(",");
                angular.forEach($scope.temp, function (lst) {
                    $scope.sepVal1.push(lst);

                })
                $scope.sepVal = $scope.sepVal1;
            }

        };

        $scope.GetUserList = function () {

            $http({
                method: 'GET',
                url: '/Catalogue/getReleaserList'
            }).success(function (response) {
                $scope.UserList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        //  $scope.GetUserList();

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

       
        //$scope.Downloadfile = function (fileName, type, imgId) {
        //  //  alert(angular.toJson(fileName));
        //  //  alert(angular.toJson(type));
        //  //  alert(angular.toJson(imgId));

        //    $window.open('/Catalogue/Downloadfile?fileName=' + fileName + '&type=' + type + '&imgId=' + imgId);

        //};
        $scope.Downloadfile = function (fileName, type, imgId) {

            $window.open('/Catalogue/Downloadfile?ItemId=' + imgId + '&fName=' + fileName);

        };
        $scope.RemoveFile = function (indx) {
            if ($scope.imgList.length > 0) {
                $scope.attachment.splice(indx, 1);
                $scope.imgList.splice(indx, 1);
            }
        };

        $scope.getmultiplecoderesult = function (code) {
            // alert("hai");
            // alert(angular.toJson(code));
            if (code != undefined && code != "") {
                //alert('in');
                $scope.DataList = [{ 'Itemcode': " " }];
                // $scope.sCode = "";
                $scope.sCode1 = [];
                $scope.temp1 = [];

                if (code != null) {
                    $scope.temp1 = code.split(/[, " "]/);
                    angular.forEach($scope.temp1, function (lst) {
                        // alert(angular.toJson(lst));
                        angular.forEach($scope.DataList1, function (lst1) {
                            //alert(angular.toJson(lst1));
                            //alert(angular.toJson(lst1.Itemcode));
                            if (lst == lst1.Itemcode) {
                                $scope.DataList.push(lst1);
                            }
                        })
                    });
                    //$scope.DataList.RemoveRow[0];
                    $scope.DataList.splice(0, 1);

                }
            }
            else {
                $scope.DataList = $scope.DataList1;
            }
            //  alert(angular.toJson($scope.DataList));

            ////////////////////
            // alert("in");
            //// alert("hai");
            // $http({
            //     method: 'GET',
            //     url: '/Catalogue/getmulticodesearch?sCode=' + $scope.sCode
            // }).success(function (response) {
            //    // alert(angular.toJson(response));
            //     $scope.DataList = response;

            // }).error(function (data, status, headers, config) {
            //     // alert("error");
            // });
        };

            $scope.formShortLong = function () {

                //if ($scope.myres1 != true) {
                //    alert(angular.toJson("Vendor not in the Master. Please update to proceed further"));
                //}
                //else
                //{
                //     alert("HAI");
                //  alert(angular.toJson($scope.Equ));

                var formData = new FormData();
                formData.append("cat", angular.toJson($scope.cat));
                formData.append("attri", angular.toJson($scope.Characteristics));
                console.log(angular.toJson($scope.Characteristics))
                formData.append("vendorsupplier", angular.toJson($scope.rows));
                formData.append("Equ", angular.toJson($scope.Equ));

                //  alert(angular.toJson($scope.Equ));
                $http({
                    url: "/Catalogue/GenerateShortLong",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (response) {
                    //alert(angular.toJson(response))
                    $scope.cat.Shortdesc = response[0];
                    $scope.cat.Longdesc = response[1];
                    $scope.cat.MissingValue = response[2];
                    $scope.cat.EnrichedValue = response[3];
                    $scope.cat.Shortdesc_ = response[4];
                    $scope.cat.exMissingValue = response[5];
                    $scope.cat.RepeatedValue = response[6];
                    $scope.Res = "Short and long generated.";
                    $scope.NotifiyRes = true;
                    $scope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    // alert(angular.toJson($scope.RepeatedValue));
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                //////////////////////

                //$http({
                //    method: 'GET',
                //    url: '/Dictionary/GetForamted?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                //}).success(function (response) {

                //    $scope.Type = response;
                //    alert(angular.toJson($scope.Type));


                //});
                // }

            };

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
            $scope.erp.StorageBin_ = $scope.erp.StorageBin != null ? $("#ddlbin").find("option:selected").text() : null;
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
            formData.append("itemsts", "3");
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
                        }else if (data == 9) {
                         
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

        $scope.createData1 = function (sts) {

            if ($scope.cat.RelRemarks == undefined || $scope.cat.RelRemarks == null || $scope.cat.RelRemarks == "" || $scope.cat.RelRemarks == "CATALOGUER-REMARK:") {
                $scope.isClf = true;
                alert(angular.toJson("Please Provide Remarks for Rework"));
            }
            else {


                if (confirm("Are you sure, send to rework for pv this record?")) {
                    //    alert(angular.toJson($scope.cat));

                    if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                        $scope.DataList = $filter('filter')($scope.DataList, { 'bu': '1' })

                    }

                    //   $scope.DataList = $filter('filter')($scope.DataList, { 'ItemStatus': '11' });


                    $timeout(function () {
                        $('#divNotifiy').attr('style', 'display: none');
                    }, 5000);

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
                    $scope.erp.StorageBin_ = $scope.erp.StorageBin != null ? $("#ddlbin").find("option:selected").text() : null;
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


                    // formData.append('files', $scope.attachment);

                    formData.append("itemsts", "11");
                    formData.append("PVStatus", "Pending");
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
                    formData.append("HSNID", angular.toJson($scope.HSNID));
                    formData.append("Desc", angular.toJson($scope.Desc));


                    //  formData.append("DataList", angular.toJson($scope.DataList));


                    $scope.promise = $http({
                        url: "/Catalogue/InsertData",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {
                        // alert(angular.toJson(data));
                        if (data.success === false) {

                            $scope.Res = data.errors;
                            $scope.Notify = "alert-danger";
                            $('#divNotifiy').attr('style', 'display: block');

                        }
                        else {
                            //   alert(angular.toJson(data));
                            //   alert(angular.toJson(data.success));
                            if (data > -1) {

                                $scope.reset();

                                $scope.cat = null;
                                $scope.Characteristics = null;
                                $scope.gen = null;
                                $scope.plant = null;
                                $scope.mrpdata = null;
                                $scope.sales = null;
                                $scope.Equ = null;
                                $scope.rows = null;
                                $scope.HSNID = null;
                                $scope.Desc = null;
                                $scope.imgList = null;
                                $scope.Title = null;
                                $scope.AtttachmentList = null;

                                if ($scope.Commodities != null && $scope.Commodities.length > 0) {
                                    $scope.Commodities[0].Class = null;
                                    $scope.Commodities[0].ClassTitle = null;
                                    $scope.Commodities = null;
                                }

                                $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];



                                //  $scope.LoadData();
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);

                                if (data == 0) {
                                    $scope.fromsave = 1;
                                    $scope.attachment = [];
                                    $scope.attcount = "";
                                    $scope.attcount1 = false;
                                    // $scope.searchMaster();
                                    $scope.Res = "PV data Send successfully";

                                    $scope.Notify = "alert-info";
                                    $('#divNotifiy').attr('style', 'display: block');
                                }
                                //if (data == 1) {
                                //    $scope.Res = "Data duplicated successfully"
                                //}
                                //if (data == 2) {
                                //    $scope.Res = "Duplicate data deleted successfully"
                                //}
                                //$scope.Notify = "alert-info";
                                //if (data == 3) {
                                //    $scope.Notify = "alert-danger";
                                //    $scope.Res = "Duplicate data not saved"
                                //}
                                // $('#divNotifiy').attr('style', 'display: block');

                                //  $scope.RowClick(lst, idx);

                                // $scope.LoadData();
                            } else {
                                if ($scope.Characteristics === null)
                                    $scope.Res = "Please add characteristics"
                                else
                                    $scope.Res = "Data save process failed"

                                $scope.Notify = "alert-info";
                                $('#divNotifiy').attr('style', 'display: block');
                            }

                        }

                    }).error(function (data, status, headers, config) {
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    });

                }
            }
        }


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
            //$scope.cat.UOM = null;
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
                params: { Noun: noun }
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }
        var usr = "";
        $scope.RowClick = function (lst, idx) {
            $scope.attcount1 = false;
            usr = $('#usrHid').val();
            $scope.currentUsr = usr;
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
                url: '/GeneralSettings/GetUnspsc',
                params :{Noun: lst.Noun ,Modifier:lst.Modifier}
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

                $scope.Equ = response.Equipment;
                //alert(usr);
                //alert(angular.toJson(response.Review ));
                if (
                    (
                        (response.Review != null && response.Review.Name === usr) 
                    ) &&
                    (response.ItemStatus === 2 || response.ItemStatus === 3)
                ) {
                    if (["Althaf"].includes(usr)) {
                        $scope.editAction = true;
                    }
                    $scope.editAction = true;
                } else if (["Althaf"].includes(usr)) {
                    $scope.editAction = true;
                }else {
                    $scope.editAction = false;
                }

                //alert($scope.editAction);

             

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
                    url: '/Dictionary/GetNounModifier2',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;
                        //angular.forEach($scope.Characteristics, function (value1, key1) {
                        //    if (value1.Characteristic == 'DESIGNATION') {
                        //        $http({
                        //            method: 'GET',
                        //            url: '/Catalogue/FetchNMRelation',
                        //            params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                        //        }).success(function (response) {

                        //            if (response != '') {
                        //                $scope.tempPlace = response;
                        //                var res = $filter('filter')(response, { 'KeyAttribute': value1.Characteristic }, true);
                        //                if (res.length > 0) {

                        //                    value1.style = "Yes";

                        //                }
                        //                else {
                        //                    value1.style = "No";
                        //                }

                        //            }
                        //            else {
                        //                $scope.tempPlace = null;
                        //            }

                        //        })


                        //    }
                        //});
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
                                    value1.Squence = value2.Squence;
                                    value1.Abbrevated = value2.Abbrevated;
                                    value1.Approve = value2.Approve;
                                    value1.btnName="Approve";
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
        $scope.Exceptional = function () {

            if ($scope.cat.Exchk == true) {

                angular.forEach($scope.Characteristics, function (value1) {
                    //if ($scope.cat.PVstatus == null || $scope.cat.PVstatus == "") {
                    //    if (value1.Mandatory === "Yes") {
                    //        if (!value1.Value) {
                    //            value1.Value = "--";
                    //        }
                    //    }
                    //}
                    //else {
                        value1.Mandatory = 'No';
                    //}
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier2?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
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
        $scope.ddlusrChange = function () {
            if ($scope.usr === undefined || $scope.usr === null) {
                $scope.revshow = true;
            }
            else {
                $scope.revshow = false;

            }
        };
        $scope.SubmitData = function () {
                    

            $timeout(function () { $scope.NotifiyRes = false; }, 30000);

            var datalst = [];
            if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                datalst = $filter('filter')($scope.DataList, { 'bu': '1' })

            }

            datalst = $filter('filter')(datalst, { 'ItemStatus': '3' });

            if (datalst != "" && datalst != undefined) {
              
                var formData = new FormData();
                formData.append("DataList", angular.toJson(datalst));


                $scope.cgBusyPromises = $http({
                    url: "/Catalogue/InsertRDataList",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data > 0) {

                        $scope.reset();
                        $scope.Type = null;
                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data + " Data submitted successfully"
                        $scope.showlist = false;
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        //$scope.searchMaster();
                       $scope.LoadData();
                    } else {
                        $scope.Res = "Save failed"
                        $scope.Notify = "alert-info";
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

        $scope.bumu = '0';

        $scope.chkmuall = function () {
          
           
            if ($scope.bumu === 1) {
                angular.forEach($scope.DataList, function (value1, key1) {
                    if (value1.bu != 1)
                    value1.bu = '1';
                });
            }
            else
            {
                angular.forEach($scope.DataList, function (value1, key1) {
                    if (value1.bu != 0)
                    value1.bu = '0';
                });
            }
           // alert(angular.toJson($scope.DataList));
        };

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

                //  alert(angular.toJson($scope.rows[index].Refflag));
                $http({
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
            //    alert(angular.toJson("Vendor not in the Master. Please update to proceed further"));
     
                // alert("check");
                //$scope.chkman = 0;
                //angular.forEach($scope.rows, function (value1, key1) {
                //    if (((value1.Type != null || value1.Type != "") && (value1.Name === null || value1.Name === ""))) {
                //        $scope.chkman = 1;
                //    }

                //    if (((value1.Type === null || value1.Type === "") && (value1.Name != null || value1.Name != ""))) {
                //        $scope.chkman = 1;
                //    }

                //});

                //if ($scope.chkman === 0) {
                var formData = new FormData();
                formData.append("cat", angular.toJson($scope.cat));
                formData.append("attri", angular.toJson($scope.Characteristics));
                formData.append("vendorsupplier", angular.toJson($scope.rows));

                $scope.createData('No');

                //if ($scope.codelogic1 != "UNSPSC Code") {
                //    //$scope.cat.Maincode = "Border: 1px solid #a94442";
                //    // $scope.cat.Subcode = "Border: 1px solid #a94442";
                //    // $scope.cat.Subsubcode = "Border: 1px solid #a94442";
                //    if ($scope.codelogic1 == "Item Code") {
                //        //  $scope.red_commodity = "";
                //        $scope.red_commodity = "";
                //        $scope.Maincode3 = "";
                //        $scope.Subcode3 = "";
                //        $scope.Subsubcode3 = "";

                //        // alert("hi");
                //        $http({
                //            url: "/Catalogue/checkDuplicate",
                //            method: "POST",
                //            headers: { "Content-Type": undefined },
                //            transformRequest: angular.identity,
                //            data: formData
                //        }).success(function (response) {
                //            //if (response != '') {
                //            //    $scope.listptnodup1 = response;

                //            //    $('#divduplicate').attr('style', 'display: block');
                //            //}
                //            //else {
                //            //    $scope.createData('No');

                //            //    $('#divduplicate').attr('style', 'display: none');
                //            //    $scope.listptnodup1 = response;
                //            //}
                //            $scope.createData('No');
                        

                //        }).error(function (data, status, headers, config) {
                //            // alert("error");

                //        });
                    
                //    }
                //    else {
                //        //  alert(angular.toJson($scope.cat.Maincode));



                //        $scope.red_commodity = "";
                //        $scope.Maincode3 = "";
                //        $scope.Subcode3 = "";
                //        $scope.Subsubcode3 = "";

                //        // alert("hi");
                //        $http({
                //            url: "/Catalogue/checkDuplicate",
                //            method: "POST",
                //            headers: { "Content-Type": undefined },
                //            transformRequest: angular.identity,
                //            data: formData
                //        }).success(function (response) {
                //            //if (response != '') {
                //            //    $scope.listptnodup1 = response;

                //            //    $('#divduplicate').attr('style', 'display: block');
                //            //}
                //            //else {
                //            //    $scope.createData('No');

                //            //    $('#divduplicate').attr('style', 'display: none');
                //            //    $scope.listptnodup1 = response;
                //            //}
                //            $scope.createData('No');


                //        }).error(function (data, status, headers, config) {
                //            // alert("error");

                //        });

                //    }

                //}
                //    //else {
                //    //    
                //    //}

                //else {
                //    if ($scope.cat.Unspsc != "" && $scope.cat.Unspsc != null && $scope.cat.Unspsc != undefined) {
                //        $scope.red_commodity = "";
                //        $scope.Maincode3 = "";
                //        $scope.Subcode3 = "";
                //        $scope.Subsubcode3 = "";
                //        // $scope.red_maincode = "Border: 1px solid #a94442";
                //        // alert("hi");
                //        $http({
                //            url: "/Catalogue/checkDuplicate",
                //            method: "POST",
                //            headers: { "Content-Type": undefined },
                //            transformRequest: angular.identity,
                //            data: formData
                //        }).success(function (response) {
                //            //if (response != '') {
                //            //    $scope.listptnodup1 = response;
                //            //    // $scope.red_maincode = "Border: 1px solid #a94442";
                //            //    $('#divduplicate').attr('style', 'display: block');
                //            //}
                //            //else {
                //            //    $scope.createData('No');

                //            //    $('#divduplicate').attr('style', 'display: none');
                //            //    $scope.listptnodup1 = response;

                //            //}
                //            $scope.createData('No');

                //        }).error(function (data, status, headers, config) {
                //            // alert("error");

                //        });
                //    }
                //    else {
                //        // alert("hi");
                //        $scope.unspscact = "active";

                //        $scope.desctab = "";

                //        $scope.planttab = "";
                //        $scope.red_commodity = "Border: 1px solid #a94442";
                //        // $scope.red_commodity = "";
                //        $scope.Maincode3 = "";
                //        $scope.Subcode3 = "";
                //        $scope.Subsubcode3 = "";

                //    }
                //}
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

                    } else if (nme == "ddlindustry1"  || nme == "ddldivision1") {
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

        //$scope.checkDuplicate = function () {


        //    var formData = new FormData();
        //    formData.append("cat", angular.toJson($scope.cat));
        //    formData.append("attri", angular.toJson($scope.Characteristics));

        //    $http({
        //        url: "/Catalogue/checkDuplicate",
        //        method: "POST",
        //        headers: { "Content-Type": undefined },
        //        transformRequest: angular.identity,
        //        data: formData
        //    }).success(function (response) {
        //        if (response != '') {
        //            $scope.listptnodup1 = response;
        //            $('#divduplicate').attr('style', 'display: block');
        //        }
        //        else {
        //            $scope.createData('No');
        //            $('#divduplicate').attr('style', 'display: none');
        //            $scope.listptnodup1 = response;
        //        }

        //    }).error(function (data, status, headers, config) {
        //        // alert("error");

        //    });

        //};
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
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
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
                    url: '/Dictionary/GetNounModifier2',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;
                      
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
            $scope.rework = false;
        $scope.Reworkbtn = function () {

            $scope.rework = true;
         //   alert("hai");
            
            //if (!$scope.form.$invalid) {               
            if (confirm("Are you sure, rework this record?")) {
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Catalogue/Reworkreview',
                    params : {itemcode:$scope.cat.Itemcode , RevRemarks:$scope.cat.RelRemarks}
                }).success(function (data, status, headers, config) {
                    if (data.success === false) {
                      //  alert(angular.toJson($scope.cat.RevRemarks));
                        $scope.Res = data.errors;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $scope.rework = false;
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
                            // $scope.searchMaster();
                            $scope.rework = false;
                             $scope.LoadData();
                        } else {
                            $scope.Res = "Record sending fail to rework"
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                            $scope.rework = false;
                        }

                    }

                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.rework = false;
                });
            }
        };


        $scope.SelectCharater = function (Noun, Modifier, Attribute, inx) {

            $http({
                method: 'GET',
                url: '/Catalogue/GetValuesList',

                params: { Noun: Noun, Modifier: Modifier, Characteristic: Attribute }
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

            $http.get('/GeneralSettings/GetSubGroupcodeList1?MainGroupCode=' + $scope.cat.Maincode
           ).success(function (response) {
               $scope.getsubgroup = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }
        $scope.getSubsubcode = function () {

            if ($scope.cat.Subcode != null)
                $scope.Subcode3 = "";
            $http.get('/GeneralSettings/GetSubsubGroupcodeList?SubGroupCode=' + $scope.cat.Subcode
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


        $scope.valChange = function (value, idx) {
            if (value == null || value == "" || value == undefined) {
                $scope.Characteristics[idx].UOM = "";
            }
        }

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx, Uom) {
         //   Value = Value.replace('&', '***');

            if (Value == null || Value == "" || Value == undefined) {
                $scope.Characteristics[indx].UOM = "";
            }
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

                            })
                        });


                    }
                }

                $http({
                    method: 'GET',
                    url: '/Catalogue/CheckValue',
                //?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value
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
                            params : {Value:Value}
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
                    params : {Value:Value}
                }).success(function (response) {
                    if (response != null) {
                        //$scope.Characteristics[indx].UOM = response;
                        $scope.Characteristics[indx].UOM = Uom;
                    }                   


                }).error(function (data, status, headers, config) {

                });
            } else {

                $('#checkval' + indx).attr('style', 'display:none');
            }
        }
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
       
        //$scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {
        //    //  alert(Value);
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
                       
        //                $scope.Characteristics[indx].Abbrivation = " ";
        //                $scope.Characteristics[indx].Approve = false;
        //            }
        //            else {
        //                $http({
        //                    method: 'GET',
        //                    url: '/Catalogue/CheckValApprove?Value=' + Value
        //                }).success(function (response) {
                        
        //                    if (response === "false") {
        //                        $('#btnabv' + indx).attr('style', 'display:none');
        //                        $scope.Characteristics[indx].Approve = true;
        //                    }
        //                    else {

        //                        $scope.Characteristics[indx].Approve = false;

        //                    }
        //                }).error(function (data, status, headers, config) {
        //                    // alert("error");

        //                });
        //                $scope.Characteristics[indx].Abbrivation = response;

        //                $http({
        //                    method: 'GET',
        //                    url: '/Catalogue/CheckValue1?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value

        //                }).success(function (response) {
                           
        //                    if (response === "false") {

        //                        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
        //                        $scope.Characteristics[indx].Approve = false;

        //                    }
        //                    else {
                              
        //                        $('#btnabv' + indx).attr('style', 'display:none');
                               
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

        //        //  alert(angular.toJson($scope.Characteristics));
        //        $http({
        //            method: 'GET',
        //            url: '/Catalogue/getunitforvalue?Value=' + Value
        //        }).success(function (response) {
        //            if (response != null) {
        //                $scope.Characteristics[indx].UOM = response;
        //            }
        //            // alert(response);
        //            //if (response != null) {
        //            //    angular.forEach($scope.UOMList, function (valueee) {
        //            //        if(valueee == response)
        //            //        {
        //            //          //  alert("hi");
        //            //            $scope.Characteristics[indx].UOM = response;
        //            //        }
        //            //    });

        //            //}
        //        }).error(function (data, status, headers, config) {
        //            // alert("error");

        //        });




        //    } else {
               
              
        //    }

            //};
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
           // abb = abb.replace('&', '***');
          //  Value = Value.replace('&', '***');
            $http({
                method: 'GET',
                url: '/Catalogue/AddValue',
               // ?Noun=' + Noun + '&Modifier=' + Modifier + '&Attribute=' + Attribute + '&Value=' + Value + '&abb=' + abb + '&role=' + "Yes"
                params : {Noun : Noun ,Modifier:Modifier , Attribute : Attribute , Value : Value , abb:abb ,role : 'Yes'}
            }).success(function (response) {

                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:none');

                //$('#btnabv' + indx).attr('style', 'display:none');
                //$('#checkval' + indx).attr('style', 'display:block');
               

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.ApproveValue = function (Noun, Modifier, Attribute, Value, Abbrevated, indx) {
           // Value = Value.replace('&', '***');
          //  Abbrevated = Abbrevated.replace('&', '***');
            $http({
                method: 'GET',
                url: '/Catalogue/ApproveValue',
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
        $scope.CancelValue = function (indx) {
            $('#checkval' + indx).attr('style', 'display:none');
            $scope.Characteristics[indx].Abbrivation = null;
        }
        $scope.Clarbtn = function () {
            $scope.sts3 = false;
            $scope.sts2 = true;

            $scope.sts4 = true;

        }
        $scope.remarksChange = function () {
            if ($scope.cat.RelRemarks == undefined || $scope.cat.RelRemarks == null || $scope.cat.RelRemarks == "") {
                $scope.isClf = true;
            }
            else {
                $scope.isClf = false;
            }
        }
        //reject
        $scope.isClf = false;
        $scope.RejectData = function  () {
           
            if ($scope.cat.RelRemarks == undefined || $scope.cat.RelRemarks == null || $scope.cat.RelRemarks == "") {
                $scope.isClf = true;
                alert(angular.toJson("Please Provide Remarks for Clarification"));
            }
            else {
                if (confirm("Are you sure, send to clarification this record?")) {

                    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);

                    $scope.cgBusyPromises = $http({
                        method: 'GET',
                        url: '/Catalogue/GetRejectCode1',
                        params : { Itemcode : $scope.codeforreject ,RevRemarks: $scope.cat.RevRemarks , usr :  $scope.usr}
                    }).success(function (response) {
                        $scope.isClf = false;
                        $scope.Res = "Record sent to Clarification"
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.reset();
                        $scope.Type = null;
                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                       // $scope.searchMaster();
                        $scope.LoadData();
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

                //$(".loaderb_div").show();
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
                    //$(".loaderb_div").hide();
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            } else {

                $scope.LoadData();
            }

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

        //UNSPSC
        $scope.SearchCommodityTitle = function (CommodityTitlesh) {

            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Catalogue/getcommcommlist?sKey=' + CommodityTitlesh
            }).success(function (response) {

                if (response != '') {
                    $scope.COMM = response;
                    $scope.Commodities = response;
                    mymodal1.open();
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

        var mymodal1 = new jBox('Modal', {

            width: 1200,
            height: 500,
            blockScroll: false,
            animation: 'zoomIn',

            overlay: true,
            closeButton: true,

            content: jQuery('#cotentid5'),

        });

        $scope.COMMClick = function (C, idx) {


            if (C.Commodity != null || C.Commodity != "") {

                $scope.cat.Unspsc = C.Commodity;
                $scope.Commodities[0].ClassTitle = C.ClassTitle;
                $scope.Commodities[0].Class = C.Class
            }
            else

                $scope.cat.Unspsc = C.Class;

            mymodal1.close();


        }


            //Images


            $scope.LoadAssetImg = function (files) {
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 3000);

                $rootScope.NotifiyRes = false;
                $scope.files = files;
                if (files[0] != null) {
                    var ext = files[0].name.match(/\.(.+)$/)[1];
                    if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                    } else {

                        angular.element("input[type='file']").val(null);
                        files[0] == null;
                        $('#divNotifiy').attr('style', 'display: block');
                        $rootScope.Res = "Please Load valid Image";
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;

                    }
                }
            };
            $scope.UploadMaximoAssetImg = function (Uid) {
                $scope.Res = null;

                if ($scope.files[0] != null) {


                    $scope.ShowHide = true;
                    //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                    var formData = new FormData();
                    formData.append('image', $scope.files[0]);
                    formData.append('UniqueId', Uid);

                    $rootScope.cgBusyPromises = $http({
                        url: "/Catalogue/UploadMaximoAssetImg",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {
                        $('#divNotifiy').attr('style', 'display: block');


                        $scope.ShowHide = false;
                        if (data.includes("Error : ")) {

                            $rootScope.Res = data;
                            $rootScope.Notify = "alert-danger";
                            $rootScope.NotifiyRes = true;
                        }
                        else {
                            $rootScope.Res = data;
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                        }





                        $('.fileinput').fileinput('clear');

                    }).error(function (data, status, headers, config) {
                        $rootScope.ShowHide = false;
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;


                    });
                };

            }
            $scope.UploadAssetImg = function (Uid) {

                $scope.Res = null;

                if ($scope.files[0] != null) {


                    $scope.ShowHide = true;
                    //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                    var formData = new FormData();
                    formData.append('image', $scope.files[0]);
                    formData.append('UniqueId', Uid);

                    $rootScope.cgBusyPromises = $http({
                        url: "/Catalogue/UploadAssetImg",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {
                        $('#divNotifiy').attr('style', 'display: block');


                        $scope.ShowHide = false;
                        if (data.includes("Error : ")) {

                            $rootScope.Res = data;
                            $rootScope.Notify = "alert-danger";
                            $rootScope.NotifiyRes = true;
                        }
                        else {
                            $rootScope.Res = data;
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                        }





                        $('.fileinput').fileinput('clear');

                    }).error(function (data, status, headers, config) {
                        $rootScope.ShowHide = false;
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;


                    });
                };

            }


            $scope.RemoveMaximoAssetImg = function (fle, Uid) {

                $scope.Res = null;
                if ($window.confirm("Do you want to delete the image?")) {

                    $scope.ShowHide = true;

                    $rootScope.promise = $http({
                        method: 'GET',
                        url: '/Catalogue/DeleteMaximoAssetImg?FileName=' + fle + "&Uid=" + Uid
                    }).success(function (data) {

                        $('#divNotifiy').attr('style', 'display: block');
                        if (data.includes("Error : ")) {

                            $scope.Res = data;
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                        } else {

                            $scope.Res = data
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                        }
                    }).error(function (data, status, headers, config) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;


                    });
                }

            };
            $scope.RemoveAssetImg = function (fle, Uid) {
                $scope.Res = null;

                if ($window.confirm("Do you want to delete the image?")) {

                    $scope.ShowHide = true;

                    $rootScope.promise = $http({
                        method: 'GET',
                        url: '/Catalogue/DeleteAssetImg?FileName=' + fle + "&Uid=" + Uid
                    }).success(function (data) {

                        $('#divNotifiy').attr('style', 'display: block');
                        if (data.includes("Error : ")) {

                            $scope.Res = data;
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                        } else {

                            $scope.Res = data
                            $scope.Notify = "alert-info";
                            $scope.NotifiyRes = true;
                        }
                    }).error(function (data, status, headers, config) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;


                    });
                }

            };
    });

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