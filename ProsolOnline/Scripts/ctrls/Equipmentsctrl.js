
(function () {
    'use strict';


    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter', 'datatables']);

    app.controller('EquipmentController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {

        $("#txtFrom").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.cat.Quantity_as_on_Date = $('#txtFrom').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                //  $("#txtTo").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });

        $("#txtFrom1").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.cat.Expired_Date = $('#txtFrom1').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                //  $("#txtTo").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });

        $("#txtFrom2").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.cat.GR_Date = $('#txtFrom2').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                //$("#txtTo").datepicker("option", "minDate", dt);
                // $scope.Todate = $('#txtTo').val();
            }
        });
        $scope.attcount1 = false;
        $scope.FetchNMRelation = function () {
            $http({
                method: 'GET',
                url: '/Catalogue/FetchNMRelation',
                params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
            }).success(function (response) {

                if (response != '') {
                    $scope.tempPlace = response;
                    angular.forEach($scope.Characteristics, function (value1, key1) {

                        var res = $filter('filter')($scope.tempPlace, { 'KeyAttribute': value1.Characteristic }, true);
                        if (res.length > 0) {

                            value1.style = "Yes";
                        }
                        else {
                            value1.style = "No";
                        }
                    });
                }
                else {
                    $scope.tempPlace = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }


        $scope.desctab = "active";
        $scope.desctab1 = "active";
        $scope.dis = false;
        $scope.RepeatedValue = "";

        function RestrictSpace() {
            if (event.keyCode == 32) {
                return false;
            }
        }

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
                    $scope.rows[index].s = '1';
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
            //  alert(index)

            if ($scope.rows[index].Type != "" && $scope.rows[index].Type != null && $scope.rows[index].Type != undefined) {
                $scope.rows[index].red = "";
                if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined)
                    $scope.rows[index].redname = "";
                else
                    $scope.rows[index].redname = "red";
            }
            else {
                //if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined) {                          
                //}
                //else if ($scope.rows[index].s == '1') {


                //    $scope.rows[index].l = '1';

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
            ///NEW CODE FOR VENDORMASTER

            //if ($scope.rows[index].Name != "" && $scope.rows[index].Name != null && $scope.rows[index].Name != undefined && $scope.rows[index].s != '0') {
            //    $http({
            //        method: 'GET',
            //        url: '/GeneralSettings/FINDVENDORMASTER?mfr=' + $scope.rows[index].Name
            //    }).success(function (response) {
            //        //alert(response);
            //        if (response == false) {
            //            $scope.myres1 = true;
            //            } else {
            //            $scope.myres1 = false;
            //            }

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
        //clone
        $scope.pop = false;
        $scope.N = null;
        $scope.M = null;
        $scope.identifyAdd_rows = function (index) {
            $('input:checkbox').click(function () {
                if ($(this).is(':checked')) {

                    $('#Submit').prop("disabled", false);
                } else {

                    if ($('.chk').filter(':checked').length < 2) {
                        $('#Submit').attr('disabled', true);
                    }
                    if ($('.chk').filter(':checked').length < 1) {
                        $scope.N = null;
                        $scope.M = null;
                    }
                }
            });
            if ($scope.N == null) {
                $scope.N = $scope.sResult1[index].Noun;
                $scope.M = $scope.sResult1[index].Modifier;
            }

            if ($scope.sResult1[index].add === 0) {
                if ($scope.sResult1[index].Noun == $scope.N && $scope.sResult1[index].Modifier == $scope.M) {
                    $scope.sResult1[index].add = 1;
                    $scope.add = false;
                }
                else {

                    alert("Please Select Same Noun and Modifier")
                    angular.forEach($scope.sResult1, function (value, key) {
                        if (key == index) {
                            value.isChecked = false;
                        }
                    });
                }

            }
            else {
                $scope.sResult1[index].add = 0;
            }


        };
        //$scope.pop = false;
        //$scope.identifyAdd_rows = function (index) {

        //    $('input:checkbox').click(function () {
        //        if ($(this).is(':checked')) {
        //            $('#Submit').prop("disabled", false);
        //        } else {
        //            if ($('.chk').filter(':checked').length < 2) {
        //                $('#Submit').attr('disabled', true);
        //            }
        //        }
        //    });

        //    if ($scope.sResult1[index].add === 0) {
        //        $scope.sResult1[index].add = 1;
        //        $scope.add = false;
        //    } else {
        //        $scope.sResult1[index].add = 0;
        //    }
        //};

        var listArray = [];
        var listArray1 = [];
        $scope.list = [];
        $scope.Compare = function (leg) {

            var listArray = [];

            angular.forEach($scope.sResult1, function (value, key) {
                if (value.add === 1) {
                    if (listArray.length == 0) {
                        listArray.push({
                            "Materialcode": "Materialcode",
                            "Noun": "Noun",
                            "Modifier": "Modifier",
                            "Legacy": "Legacy",
                            "Longdesc": "LongDesc",
                            "Additionalinfo": "Additional information",
                            "Characteristics": value.Characteristics
                        })
                    }
                    listArray.push(value);
                }

            });

            angular.forEach(listArray, function (src, idx) {
                src.rem = 0;
            });

            $scope.list = listArray;
            //  $scope.char =$scope.list[0].Characteristics;


            if ($scope.list.length > 1) {

                mymodalcompare.open();
                mymodalcompare.setTitle('<strong>' + "Legacy : " + '</strong>' + Legacy + '<br>' + '<strong>' + "Search key : " + '</strong>' + Legacy1);
                $scope.pop = true;
                $scope.N = null;
                $scope.M = null;
                angular.forEach($scope.sResult1, function (value, key) {
                    value.add = 0;
                    value.isChecked = false;
                });
            }
            else {
                alert("Please select two or more items to compare")
            }

        };
        var mymodalcompare = new jBox('Modal', {

            width: 2500,
            height: 650,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#compare'),

            // content: jQuery('#cotentid3'),

        });
        $scope.Remove = function (index) {
            var listArray1 = [];
            if ($scope.list[index].rem === 0) {
                $scope.list[index].rem = 1;
                $scope.remove = false;
            } else {
                $scope.list[index].rem = 0;
            }
            angular.forEach($scope.list, function (value, key) {
                if (value.rem === 0) {

                    listArray1.push(value);
                }
            });
            $scope.list = listArray1;
            if (listArray1.length == 1) {

                mymodalcompare.close();
            }
        };

        var mymodalnew = new jBox('Modal', {

            width: 1200,
            height: 500,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentidnew'),

            // content: jQuery('#cotentid3'),

        });
        $scope.nounmodifier = true;
        $scope.searchby = function (Key) {


            if (Key == "Noun-Modifier") {
                $scope.search = true;
                $scope.nounmodifier = false;
                mymodalNM.open();
            }
            else {
                $scope.search = false;
            }
        }
        var mymodalNM = new jBox('Modal', {

            width: 800,
            height: 500,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#nmpopup'),

            // content: jQuery('#cotentid3'),

        });
        $http({
            method: 'GET',
            url: '/Dictionary/GetNoun'
        }).success(function (response) {

            $scope.Nounsrch = response;

        }).error(function (data, status, headers, config) {
            alert("error");

        });

        $scope.SelectNoun1 = function () {


            $scope.modifierDef1 = null;
            $scope.pop.Modifier = null;
            //if ($scope.list.Noun.toString().indexOf(',') == -1) {

            //    $scope.getnoun = $scope.list.Noun;

            //    var NDef = $.grep($scope.Nounsrch, function (list) {
            //        return list.Noun == $scope.list.Noun;
            //    })[0].NounDefinition;

            //    $scope.nounDef = NDef;
            //}

            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier?Noun=' + $scope.pop.Noun
            }).success(function (response) {

                $scope.Modifiersrch = response;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

        };

        $scope.SelectModifier1 = function () {

            $scope.charaterDef = null;
            $scope.charater = null;

            if ($scope.pop.Modifier.toString().indexOf(',') == -1) {

                var NDef = $.grep($scope.Modifiersrch, function (pop) {
                    return pop.Modifier == $scope.pop.Modifier;
                })[0].ModifierDefinition;

                $scope.modifierDef1 = NDef;
            }

        };
        $scope.getnm = function (Noun, Modifier) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $scope.promise = $http({
                method: 'GET',
                url: '/Search/Getitem?Noun=' + Noun + '&Modifier=' + Modifier
            }).success(function (response) {

                if (response != '') {
                    $scope.sResult1 = response;

                    angular.forEach($scope.sResult1, function (src, idx) {
                        src.add = 0;
                        src.rem = 0;
                        src.isChecked = false;
                    });
                    $scope.Res = "Search results : " + response.length + " items."
                    $scope.Notify = "alert-info";

                    $scope.add = true;

                    mymodalnew.open();
                    mymodalnew.setTitle("Noun-Modifier : " + Noun + "," + Modifier);

                } else {
                    $scope.sResult1 = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                alert("error");

            });

        }
        $scope.clickToOpen = function (Legacy1, Legacy, Itemcode) {

            $scope.leg = Legacy;
            $scope.Item = Itemcode;
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if (Legacy1 != " " && Legacy1 != undefined && Legacy1 != null && Legacy1 != '') {
                $scope.sResult = {};
                $rootScope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Search/GetSearch',
                    params: { sKey: Legacy1 }

                }).success(function (response) {

                    $('#divNotifiy').attr('style', 'display: block');
                    if (response != '') {

                        $scope.sResult1 = response;

                        angular.forEach($scope.sResult1, function (src, idx) {
                            src.add = 0;
                            src.rem = 0;
                            src.isChecked = false;
                        });
                        $scope.Res = "Search results : " + response.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.add = true;

                        mymodalnew.open();
                        mymodalnew.setTitle('<strong>' + "Legacy : " + '</strong>' + Legacy + '<br>' + '<strong>' + "Search key : " + '</strong>' + Legacy1);

                    } else {
                        $scope.sResult1 = null;
                        $scope.Res = "No item found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }

                }).error(function (data, status, headers, config) {

                });
            }
        };
        //clone
        $scope.Clone = function (src1) {

            // alert("hai");
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: src1.Noun }
            }).success(function (response) {
                var flg = 0;
                $scope.Modifiers = response;
            });


            $scope.cat.Noun = src1.Noun;
            $scope.cat.Modifier = src1.Modifier;
            $scope.cat.Shortdesc = src1.Shortdesc;
            $scope.cat.Longdesc = src1.Longdesc;

            $scope.cat.Additionalinfo = src1.Additionalinfo;
            $scope.cat.Junk = src1.Junk;
            $scope.cat.Manufacturer = src1.Manufacturer;
            $scope.cat.Application = src1.Application;
            $scope.cat.Drawingno = src1.Drawingno;
            $scope.cat.Referenceno = src1.Referenceno;
            $scope.cat.Remarks = src1.Remarks;
            $scope.cat.Characteristics = src1.Characteristics;
            $scope.cat.UOM = src1.UOM;
            $scope.cat.Maincode = $scope.cat.Maincode;
            $scope.getSubcode();
            $scope.cat.Subcode = $scope.cat.Subcode;
            $scope.getSubsubcode();
            $scope.cat.Subsubcode = $scope.cat.Subsubcode;





            //Equipment
            $scope.Equ = src1.Equipment;



            var tst = src1.Vendorsuppliers;
            if (tst != null && tst != '') {
                $scope.rows = src1.Vendorsuppliers;

            } else {
                $scope.rows = null;
                $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
            }


            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: src1.Noun }
            }).success(function (response) {
                var flg = 0;
                $scope.Modifiers = response;

                angular.forEach($scope.Modifiers, function (obj) {
                    if (obj.Modifier == src1.Modifier)
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
                url: '/Dictionary/GetNounModifier',
                params: { Noun: src1.Noun, Modifier: src1.Modifier }
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
                        // 
                    });




                    angular.forEach($scope.Characteristics, function (value1, key1) {

                        angular.forEach($scope.cat.Characteristics, function (value2, key2) {

                            if (value1.Characteristic === value2.Characteristic) {

                                value1.Value = value2.Value;
                                value1.UOM = value2.UOM;
                                value1.Source = value2.Source;
                                value1.SourceUrl = value2.SourceUrl;
                                value1.Squence = value2.Squence;
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

            mymodalcompare.close();
            mymodalnew.close();
        };

        //mapduplicate
        $scope.Mapdup = function (Itemcode) {
            //  alert("hai");

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if (confirm("Are You sure, Duplicate this Itemcode '" + Itemcode + "' ?")) {

                $http({
                    method: 'GET',
                    url: '/Search/savemapduplicate',
                    params: { new_code: $scope.Item, existing_code: Itemcode }
                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: Block');
                    if (response == true) {
                        $scope.reset();
                        $scope.Res = "Itemcode : '" + $scope.Item + "' is Mapped to itemcode '" + Itemcode + "'"
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.LoadData();
                        mymodalnew.close();
                        mymodalcompare.close();
                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;

                        $scope.Desc = null;
                        //$scope.sts4 = false;
                        //$scope.sts3 = false;
                    }
                    else {
                        $scope.Res = "Not Mappped"
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {

                });

            }


        }


        $scope.getItem = function () {
            var url = $location.$$absUrl;
            if (url.indexOf("Catalogue/Index?itemId") !== -1) {

                var arrId = url.split('itemId=');

                $scope.cat = {};

                $http({
                    method: 'GET',
                    url: '/Catalogue/getItem',
                    params: { itemId: arrId[1] }
                }).success(function (response) {
                    $http({
                        method: 'GET',
                        url: '/GeneralSettings/GetUnspsc',
                        params: { Noun: response.Noun, Modifier: response.Modifier }
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

                    $scope.cat = response;
                    $scope.Equ = response.Equipment;

                    $scope.cat.Maincode = $scope.cat.Maincode;

                    if ($scope.cat.Maincode != null) {
                        $scope.getSubcode();
                    }

                    //   $scope.getSubcode();
                    $scope.cat.Subcode = $scope.cat.Subcode;
                    //  $scope.getSubsubcode();
                    if ($scope.cat.Subcode != null) {
                        $scope.getSubsubcode();
                    }
                    $scope.cat.Subsubcode = $scope.cat.Subsubcode;

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
                        params: { Noun: $scope.cat.Noun }
                    }).success(function (response) {
                        var flg = 0;
                        $scope.Modifiers = response;

                        //angular.forEach($scope.Modifiers, function (obj) {
                        //    if (obj.Modifier == lst.Modifier)
                        //        flg = 1;
                        //});
                        //if (flg == 0) {
                        //    $scope.cat.Modifier = '';

                        //}


                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });

                    ///  $scope.getSimiliar();

                    $http({
                        method: 'GET',
                        url: '/Dictionary/GetNounModifier',
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
                            $http({
                                method: 'GET',
                                url: '/Dictionary/GetForamted',
                                params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                            }).success(function (response) {

                                $scope.Type = response;
                                angular.forEach($scope.rows, function (lst) {
                                    lst.s = '1';

                                });
                                if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                                    $scope.ven = true;

                                }
                                else {
                                    $scope.ven = false;
                                }


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
            }

        }
        $scope.getItem();
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

        $scope.showuserMap = function (itmcode) {

            $http({
                method: 'GET',
                url: '/User/getItemstatusMap',
                params: { itmcde: itmcode }
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

        $scope.NMLoad = function () {
            $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Catalogue/GetDBNounList'
            }).success(function (response) {
                $scope.NounList = response;
                // $scope.sNoun = "";

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.GetUserList = function () {
            $scope.NMLoad();
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
            var indx = 0;
            angular.forEach($scope.Characteristics, function (value1, key1) {

                angular.forEach(LstObj, function (value2, key2) {

                    if (value1.Characteristic === value2.Characteristic) {

                        value1.Value = value2.Value;
                        value1.UOM = value2.UOM;
                        $('#btnabv' + indx).attr('style', 'display:none');
                        $('#checkval' + indx).attr('style', 'display:none');
                        value1.Abbrivation = "";
                    }

                });
                indx++;
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
            //sl
            $http({
                method: 'GET',
                url: '/Master/GetDataList?label=Storage location'
            }).success(function (response) {
                $scope.strloc = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        $scope.BindMaster();
        //sb
        //bincode
        $scope.getbincode = function () {

            // alert(angular.toJson($scope.cat.StorageLocation1));
            // $http.get('/Master/getbincode?StorageLocation=' + $scope.erp.StorageLocation 
            $http.get('/Master/getbincode?label= Storage bin' + '&StorageLocation=' + $scope.cat.StorageLocation1
             ).success(function (response) {
                 $scope.MasterList1 = response
                 // alert(angular.toJson($scope.getsubgroup));
             }).error(function (data, status, headers, config) {
             });
        };

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.sts1 = false;
        $scope.checkSubmit = function () {

            $scope.sts1 = false;
            angular.forEach($scope.DataList1, function (value, key) {
                if (value.ItemStatus == 1 || value.ItemStatus == 3) {
                    $scope.sts1 = true;
                }
            })
        };
        $scope.tempPlace = [];
        $scope.tempRelation = [];
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

        $scope.getSimiliar = function () {

            $rootScope.cgBusyPromises = $http({
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
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        //$scope.balanceitems = 0;
        //$scope.saveditems = 0;      

        $scope.LoadData = function () {

            $http({
                method: 'GET',
                url: '/Equipment/GetDataList'
            }).success(function (response) {
                $scope.DataList1 = response;

                $scope.DataList1 = $filter('filter')($scope.DataList1, { 'category': 'Equipment' });

                angular.forEach($scope.DataList1, function (lst) {
                    lst.bu = '0';

                });

                // $scope.saveditems = ($filter('filter')($scope.DataList1, { 'ItemStatus': '1' })).length;
                // $scope.balanceitems = ($filter('filter')($scope.DataList1, { 'ItemStatus': '0' })).length;

                $scope.checkSubmit();
            }).error(function (data, status, headers, config) {
                // alert("error");
            });          

        };

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
                url: '/Catalogue/getReviewerList'
            }).success(function (response) {
                $scope.UserList = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };
        // $scope.GetUserList();
        $scope.LoadData();
        $scope.LoadSpares = function (itmcde) {

            $http({
                method: 'GET',
                url: '/Equipment/GetSpareList?itemCode=' + itmcde
            }).success(function (response) {
                $scope.DataList = response;

                //$scope.DataList1 = $filter('filter')($scope.DataList1, { 'Type': 'Equipment' });
                //angular.forEach($scope.DataList1, function (lst) {
                //    lst.bu = '0';

                //});

                //$scope.saveditems = ($filter('filter')($scope.DataList1, { 'ItemStatus': '1' })).length;
                //$scope.balanceitems = ($filter('filter')($scope.DataList1, { 'ItemStatus': '0' })).length;

                //$scope.checkSubmit();
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        };

        //Attachment
        $scope.fileList = [];
        $scope.attachment = [];
        $scope.imgList = [];

        $scope.LoadFileData = function (files) {
            $scope.fileList = files;
            //  alert(angular.toJson(files));

        };
        $scope.addImg = function () {

            if ($scope.imgList === null) {
                $scope.imgList = [];
            }


            $scope.attachment.push($scope.fileList[0]);

            var bytes = $scope.fileList[0].size;;
            var k = 1024; //Or 1 kilo = 1000
            var sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB"];
            var i = Math.floor(Math.log(bytes) / Math.log(k));
            var size = parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];


            $scope.imgList.push({ "_id": "0", 'Title': $scope.Title == null ? '' : $scope.Title, 'FileName': $scope.fileList[0].name, 'FileSize': size, 'ContentType': $scope.fileList[0].ContentType });



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
        //uom

        $http.get('/Catalogue/getuomlist').success(function (response) {
            // alert('hi');
            $scope.uomList1 = response;
            //alert(angular.toJson($scope.uomList1))
        });
        $scope.getmultiplecoderesult = function (code) {
            //alert("hai");
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

        $scope.createData = function (sts) {
            //  alert("hai");
            // alert(angular.toJson($scope.myres1));

            //if (Characteristics.uo.Value != null && Characteristics.uo.UOM == null)
            //{
            //    alert (confirm("Are you sure, send to clarification this record?"))
            //}
            //else
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
            $scope.cat.Equipments = $scope.Equipments;

            angular.forEach($scope.rows1, function (obj) {
                if (obj.RefNo != null && obj.Refflag != null) {
                    $scope.rows.push(obj);
                }

            });

            formData.append("itemsts", "1");
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
            //formData.append("type", angular.toJson($scope.Type));
            //alert(angular.toJson($scope.Type));

            $rootScope.cgBusyPromises = $http({
                url: "/Equipment/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');

                }
                else {

                    if (data > -1) {

                        if (data == 9) {

                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please add all values"
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.dis = false;
                        } else if (data == 10) {
                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please approve/add values in value master"
                            $('#divNotifiy').attr('style', 'display: block');
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

                            $scope.imgList = null;
                            $scope.Title = null;
                            $scope.AtttachmentList = null;

                            if ($scope.Commodities != null && $scope.Commodities.length > 0) {
                                $scope.Commodities[0].Class = null;
                                $scope.Commodities[0].ClassTitle = null;
                                $scope.Commodities = null;
                            }

                           
                            $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                            $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];


                            $scope.LoadData();
                            if ($scope.Equipments != null && $scope.Equipments != '' && $scope.Equipments != undefined) {
                                $scope.LoadSpares($scope.Equipments);
                            }


                            //  $scope.LoadData();


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
                            $scope.Notify = "alert-info";
                            if (data == 3) {
                                $scope.Notify = "alert-danger";
                                $scope.Res = "Duplicate data not saved"
                                $scope.searchMaster($scope.sCode, $scope.sSource, $scope.sNoun, $scope.sModifier, $scope.sUser);

                            }

                            $('#divNotifiy').attr('style', 'display: block');
                        }

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

            //   }
        };

        $scope.createData1 = function (sts) {


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
            formData.append("PVStatus", "Yes");
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


        };

        $scope.loadmodifier = function (noun) {

            // alert("mod");
            //alert(angular.toJson(noun));


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
            $scope.cat.Noun = noun.toUpperCase();
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: noun }
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }

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

        $scope.editAction = true;
        $scope.RowClick1 = function (lst, idx) {

            $scope.attcount1 = false;
            var usr = $('#usrHid').val();

            $scope.red_commodity = "";
            $scope.Maincode3 = "";


            $scope.codeforreject = lst.Itemcode;
            $scope.sts2 = false;
            $scope.sts3 = true;
            $scope.sts5 = true;
            $scope.sts4 = false;
            $scope.Legacy1 = true;
            $scope.Partnum = false;
            $('#divPartnum').attr('style', 'display: none');
            $scope.listptnodup = null;

            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;

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
         
            $http({
                method: 'GET',
                url: '/Equipment/FetchNMRelation',
                params: { Noun: lst.Noun, Modifier: lst.Modifier }
            }).success(function (response) {

                if (response != '') {
                    $scope.tempPlace = response;

                }
                else {
                    $scope.tempPlace = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

            //  alert(angular.toJson($scope.tempPlace));

            var i = 0;
            angular.forEach($scope.DataList1, function (lst1) {
                $('#' + i).attr("style", "");
                i++;
            });
            $('#' + idx).attr("style", "background-color:lightblue");

            //   alert(angular.toJson(lst));

            $http({
                method: 'GET',
                url: '/Equipment/GetERPInfo',
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
                url: '/Equipment/GetAttachment?itemcode=' + lst.Itemcode
                //  url: '/Catalogue/GetAttachment',
                //params :{itemcode: lst.Materialcode}
            }).success(function (response) {
                $scope.AtttachmentList = response;
                if (response.length > 0) {
                    $scope.attcount = response.length;
                    $scope.attcount1 = true;
                }

                //alert(angular.toJson(response));
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            $scope.cat = {};

            $http({
                method: 'GET',
                url: '/Equipment/GetSingleItem',
                params: { itemcode: lst.Itemcode }
            }).success(function (response) {

                $scope.cat = response;

                if (response.category == "Equipment")
                    $scope.Equipments = response.Equipments;

                $scope.cat.category = "Equipment";

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
                // $scope.Equ.ENS = '1';
                //   $scope.Equ.EMS = '1';
                // alert(angular.toJson($scope.Equ));
                if (response.ItemStatus == 0 || response.ItemStatus == 1 || response.ItemStatus == -1 || response.ItemStatus == 13 || response.ItemStatus == 2 || response.ItemStatus == 3)
                    $scope.editAction = true;
                else $scope.editAction = false;

                $scope.cat.Maincode = $scope.cat.Maincode;
                if ($scope.cat.Maincode != null) {
                    $scope.getSubcode();
                }
                $scope.cat.Subcode = $scope.cat.Subcode;
                if ($scope.cat.Subcode != null) {
                    $scope.getSubsubcode();
                }

                $scope.cat.Subsubcode = $scope.cat.Subsubcode;

                //var tst = response.Vendorsuppliers;
                //if (tst != null && tst != '') {
                //    $scope.rows = response.Vendorsuppliers;
                //} else {
                //    $scope.rows = null;
                //    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                //}


                $scope.rows = [];
                $scope.rows1 = [];
                if (response.Vendorsuppliers.length > 0 && response.Vendorsuppliers != '' && response.Vendorsuppliers != null && response.Vendorsuppliers != 'null') {

                    angular.forEach(response.Vendorsuppliers, function (obj) {

                        if (obj.Type === "MANUFACTURER") {

                            $scope.rows.push(obj);
                        }
                        if ((obj.Type === null || obj.Type === '') && obj.RefNo != null && obj.Refflag != '') {

                            $scope.rows1.push(obj);
                        }

                    });
                    if ($scope.rows.length == 0) {
                        $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];

                    }
                    if ($scope.rows1.length == 0) {
                        1
                        $scope.rows1 = [{ 'slno': 1, 's': '0', 'l': '1' }];
                    }


                } else {
                    $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];
                    $scope.rows1 = [{ 'slno': 1, 's': '0', 'l': '1' }];
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
                            url: '/Equipment/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;

                        }).error(function (data, status, headers, config) {
                            // 
                        });



                        $http({
                            method: 'GET',
                            url: '/Dictionary/GetForamted',
                            params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                        }).success(function (response) {

                            $scope.Type = response;
                            $scope.ven = true;
                            //angular.forEach($scope.rows, function (lst) {
                            //    lst.s = '1';

                            //});
                            if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                                $scope.ven = true;

                                // $scope.thii = false;
                            }
                            else {
                                $scope.ven = false;
                                //  $scope.thii = true;
                            }

                        });


                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach($scope.cat.Characteristics, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {
                                    value1.Value = value2.Value;
                                    value1.UOM = value2.UOM;
                                    value1.Source = value2.Source;
                                    value1.SourceUrl = value2.SourceUrl;
                                    value1.Squence = value1.Squence;
                                    value1.ShortSquence = value1.ShortSquence;
                                    value1.UomMandatory = value1.UomMandatory;
                                    if ($scope.cat.Exchk == true)
                                        value1.Mandatory = 'No';
                                    // value1.Abbrivation = value2.Abbrevated;
                                    // value1.Approve = value2.Approve;
                                    //value1.btnName = "Approve";
                                }
                            });
                        });

                    }
                    else {
                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }
                    //   
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
                //pvdata
                if ($scope.cat.Quantity_as_on_Date != null)
                    $scope.cat.Quantity_as_on_Date = new Date(parseInt($scope.cat.Quantity_as_on_Date.replace('/Date(', ''))).toLocaleDateString();
                if ($scope.cat.Expired_Date != null)
                    $scope.cat.Expired_Date = new Date(parseInt($scope.cat.Expired_Date.replace('/Date(', ''))).toLocaleDateString();
                if ($scope.cat.GR_Date != null)
                    $scope.cat.GR_Date = new Date(parseInt($scope.cat.GR_Date.replace('/Date(', ''))).toLocaleDateString();

            }).error(function (data, status, headers, config) {
                // alert("error");

            });



            $scope.LoadSpares(lst.Itemcode);


        };
        $scope.RowClick = function (lst, idx) {

            $scope.attcount1 = false;
            var usr = $('#usrHid').val();

            $scope.red_commodity = "";
            $scope.Maincode3 = "";


            $scope.codeforreject = lst.Itemcode;
            $scope.sts2 = false;
            $scope.sts3 = true;
            $scope.sts5 = true;
            $scope.sts4 = false;
            $scope.Legacy1 = true;
            $scope.Partnum = false;
            $('#divPartnum').attr('style', 'display: none');
            $scope.listptnodup = null;

            $('#divduplicate').attr('style', 'display: none');
            $scope.listptnodup1 = null;

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
                url: '/Equipment/FetchNMRelation',
                params: { Noun: lst.Noun, Modifier: lst.Modifier }
            }).success(function (response) {

                if (response != '') {
                    $scope.tempPlace = response;

                }
                else {
                    $scope.tempPlace = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

            //  alert(angular.toJson($scope.tempPlace));


            var i = 0;
            angular.forEach($scope.DataList, function (lst1) {
                $('#sp' + i).attr("style", "");
                i++;
            });
            $('#sp' + idx).attr("style", "background-color:lightblue");


            //   alert(angular.toJson(lst));

            $http({
                method: 'GET',
                url: '/Equipment/GetERPInfo',
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
                url: '/Equipment/GetAttachment?itemcode=' + lst.Itemcode
                //  url: '/Catalogue/GetAttachment',
                //params :{itemcode: lst.Materialcode}
            }).success(function (response) {
                $scope.AtttachmentList = response;
                if (response.length > 0) {
                    $scope.attcount = response.length;
                    $scope.attcount1 = true;
                }

                //alert(angular.toJson(response));
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            $scope.cat = {};

            $http({
                method: 'GET',
                url: '/Equipment/GetSingleItem',
                params: { itemcode: lst.Itemcode }
            }).success(function (response) {

                $scope.cat = response;

                if (response.category == "Equipment")
                    $scope.Equipments = response.Equipments;

                // $scope.cat.Type = "Spare";

                angular.forEach(response.EquipLists, function (obj) {
                    if (obj.Itemcode == $scope.Equipments)
                        $scope.cat.PartQty = obj.PartQty;
                });


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
                // $scope.Equ.ENS = '1';
                //   $scope.Equ.EMS = '1';
                // alert(angular.toJson($scope.Equ));
                if (response.ItemStatus == 0 || response.ItemStatus == 1 || response.ItemStatus == -1 || response.ItemStatus == 13||response.ItemStatus == 2 || response.ItemStatus == 3 )
                    $scope.editAction = true;
                else $scope.editAction = false;

                $scope.cat.Maincode = $scope.cat.Maincode;
                if ($scope.cat.Maincode != null) {
                    $scope.getSubcode();
                }
                $scope.cat.Subcode = $scope.cat.Subcode;
                if ($scope.cat.Subcode != null) {
                    $scope.getSubsubcode();
                }

                $scope.cat.Subsubcode = $scope.cat.Subsubcode;

                //var tst = response.Vendorsuppliers;
                //if (tst != null && tst != '') {
                //    $scope.rows = response.Vendorsuppliers;
                //} else {
                //    $scope.rows = null;
                //    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                //}

                $scope.rows = [];
                $scope.rows1 = [];
                if (response.Vendorsuppliers.length > 0 && response.Vendorsuppliers != '' && response.Vendorsuppliers != null && response.Vendorsuppliers != 'null') {

                    angular.forEach(response.Vendorsuppliers, function (obj) {
                      
                        if (obj.Type != null && obj.Type != '')  {

                            $scope.rows.push(obj);
                        }
                        if (obj.RefNo != null && obj.Refflag != '') {
                            
                            $scope.rows1.push(obj);
                        }

                    });
                    if ($scope.rows.length == 0) {
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];

                    }
                    if ($scope.rows1.length == 0) {
                        1
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
                    }

                } else {
                    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                    $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
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
                            url: '/Equipment/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;

                        }).error(function (data, status, headers, config) {
                            // 
                        });



                        $http({
                            method: 'GET',
                            url: '/Dictionary/GetForamted',
                            params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                        }).success(function (response) {

                            $scope.Type = response;
                            $scope.ven = true;
                            //angular.forEach($scope.rows, function (lst) {
                            //    lst.s = '1';

                            //});
                            if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                                $scope.ven = true;

                                // $scope.thii = false;
                            }
                            else {
                                $scope.ven = false;
                                //  $scope.thii = true;
                            }

                        });


                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach($scope.cat.Characteristics, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {
                                    value1.Value = value2.Value;
                                    value1.UOM = value2.UOM;
                                    value1.Source = value2.Source;
                                    value1.SourceUrl = value2.SourceUrl;
                                    value1.Squence = value1.Squence;
                                    value1.ShortSquence = value1.ShortSquence;
                                    value1.UomMandatory = value1.UomMandatory;
                                    if ($scope.cat.Exchk == true)
                                        value1.Mandatory = 'No';
                                    // value1.Abbrivation = value2.Abbrevated;
                                    // value1.Approve = value2.Approve;
                                    //value1.btnName = "Approve";
                                }
                            });
                        });

                    }
                    else {
                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }
                    //   
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
                //pvdata
                if ($scope.cat.Quantity_as_on_Date != null)
                    $scope.cat.Quantity_as_on_Date = new Date(parseInt($scope.cat.Quantity_as_on_Date.replace('/Date(', ''))).toLocaleDateString();
                if ($scope.cat.Expired_Date != null)
                    $scope.cat.Expired_Date = new Date(parseInt($scope.cat.Expired_Date.replace('/Date(', ''))).toLocaleDateString();
                if ($scope.cat.GR_Date != null)
                    $scope.cat.GR_Date = new Date(parseInt($scope.cat.GR_Date.replace('/Date(', ''))).toLocaleDateString();

            }).error(function (data, status, headers, config) {
                // alert("error");

            });






        };
        $scope.ddlusrChange = function () {
            if ($scope.usr === undefined || $scope.usr === null) {
                $scope.revshow = true;
            }
            else {
                $scope.revshow = false;

            }
        };
        var mpop = new jBox('Modal', {
            width: 500,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#map-pop'),
            reposition: false,
            repositionOnOpen: false
        })
        $scope.ganFun = function () {

          

            $timeout(function () {

                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            if ($scope.Equipments != "" && $scope.Equipments != undefined) {

                mpop.open();
            }
            else {
                alert("Select the equipment to submit");
                $scope.LoadData();
            }

        };
    
        $scope.SubmitData1 = function (fcode) {

 
          
            $timeout(function () {

                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.Equipments != "" && $scope.Equipments != undefined) {

                if ($scope.cat.ItemStatus == '3') {
                    if (fcode != "" && fcode != undefined) {


                        var formData = new FormData();
                        $scope.cat.Junk = fcode;
                        formData.append("DataList", angular.toJson($scope.cat));

                        $rootScope.cgBusyPromises = $http({
                            url: "/Equipment/InsertDataList",
                            method: "POST",
                            headers: { "Content-Type": undefined },
                            transformRequest: angular.identity,
                            data: formData
                        }).success(function (data, status, headers, config) {

                            if (data > 0) {

                                $scope.reset();

                                $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];
                                $scope.rows1 = [{ 'slno': 1, 's': '0', 'l': '1' }];


                                $scope.Type = null;
                                $scope.cat = null;
                                $scope.Characteristics = null;
                                $scope.Equ = null;
                                mpop.close();
                                $scope.Res = data + " Data submitted successfully";
                                $scope.Notify = "alert-info";
                                $scope.Legacy1 = true;
                                $('#divNotifiy').attr('style', 'display: block');
                                $scope.LoadData();
                                $scope.DataList = null;

                            } else {
                                $scope.Res = "Invalid function location"
                                $scope.Notify = "alert-info";

                                $('#divNotifiy').attr('style', 'display: block');
                            }



                        }).error(function (data, status, headers, config) {
                            $scope.Res = data;
                            $scope.Notify = "alert-danger";
                            $('#divNotifiy').attr('style', 'display: block');
                        });
                    }
                    else {
                        alert("Please enter function location");
                                          }
                }
                else {
                    $scope.Res = "Select saved items to submit";
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');

                }
            }
            else {
              
                $scope.Res = "Select the equipment to submit";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
              
            }

        };
        $scope.SubmitData = function () {



            $timeout(function () {

                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            if ($scope.cat.ItemStatus == '1') {
                var formData = new FormData();

                formData.append("DataList", angular.toJson($scope.cat));

                $rootScope.cgBusyPromises = $http({
                    url: "/Equipment/InsertDataList",
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
                        $scope.Res = data + " Data submitted successfully";
                        $scope.Notify = "alert-info";
                        $scope.Legacy1 = true;
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                    } else {
                        $scope.Res = "Data submission failed"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                $scope.Res = "Select saved items to submit";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');

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
       
        $scope.GetRP = function () {
            //   alert(angular.toJson($scope.cat.Noun))
            if ($scope.cat.Noun != "" && $scope.cat.Noun != null) {
                //      alert(angular.toJson($scope.cat.Noun))
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetRP?Noun=' + $scope.cat.Noun
                }).success(function (response) {

                    $scope.RP = response;
                 
                    if ($scope.RP == "CI") {
                        if ($scope.erp.Quantity_ != "") {
                            $scope.erp.ReOrderPoint_ = Math.round($scope.erp.Quantity_ / 2);
                        }                       
                        $scope.erp.MaxStockLevel_ = $scope.erp.Quantity_;
                    }
                    else if ($scope.RP == "RI") {
                        $scope.erp.ReOrderPoint_ = '0';
                        $scope.erp.MaxStockLevel_ = '0';
                    }
                    else if ($scope.RP == "WI") {
                        $scope.erp.ReOrderPoint_ = $scope.erp.Quantity_;
                        $scope.erp.MaxStockLevel_ = $scope.erp.Quantity_;
                    }
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }
        }


        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1', 'Name': '' }];
        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1', 'Refflag': '' }];
        $scope.addRow = function () {

            $scope.rows.push({ 'slno': $scope.rows.length + 1, 's': '1', 'l': '1' });

            //$scope.rows[$scope.rows.length-1].Type = $scope.rows[$scope.rows.length - 2].Type;          
            //$scope.rows[$scope.rows.length - 1].Name = $scope.rows[$scope.rows.length - 2].Name;
            //$scope.rows[$scope.rows.length - 1].Refflag = $scope.rows[$scope.rows.length - 2].Refflag;
            //$scope.rows[$scope.rows.length - 1].RefNo = $scope.rows[$scope.rows.length - 2].RefNo;



        };
        $scope.addRow1 = function () {

            $scope.rows1.push({ 'slno': $scope.rows1.length + 1, 's': '1', 'l': '1' });

            //  $scope.rows1[$scope.rows.length - 1].Type = $scope.rows1[$scope.rows.length - 2].Type;
            // $scope.rows1[$scope.rows.length - 1].Name = $scope.rows1[$scope.rows.length - 2].Name;
            $scope.rows1[$scope.rows1.length - 1].Refflag = $scope.rows1[$scope.rows1.length - 2].Refflag;
            $scope.rows1[$scope.rows1.length - 1].RefNo = $scope.rows1[$scope.rows1.length - 2].RefNo;



        };
        $scope.RemoveRow = function (indx) {

           // if ($scope.rows[indx].s != 1) {
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
            //}
            //else {
            //    $timeout(function () {
            //        $('#divNotifiy').attr('style', 'display: none');
            //    }, 5000);

            //    $scope.Res = "You can't remove this row, because it will appear in Short Desc.";
            //    $scope.Notify = "alert-danger";
            //    $('#divNotifiy').attr('style', 'display: block');

            //}
        };
        $scope.RemoveRow1 = function (indx) {

           // if ($scope.rows1[indx].s != 1) {
                if ($scope.rows1.length > 1) {
                    $scope.rows1.splice(indx, 1);
                    //  $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];
                }

                if ($scope.rows1.length === 1 && indx === 0) {
                    $scope.rows1.splice(indx, 1);
                    $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
                }

                var cunt = 1;
                angular.forEach($scope.rows1, function (value, key) {
                    value.slno = cunt++;

                });
            //}
            //else {
            //    $timeout(function () {
            //        $('#divNotifiy').attr('style', 'display: none');
            //    }, 5000);

            //    $scope.Res = "You can't remove this row, because it will appear in Short Desc.";
            //    $scope.Notify = "alert-danger";
            //    $('#divNotifiy').attr('style', 'display: block');

            //}
        };


        $scope.checkPartno = function (index) {



            //if ($scope.rows[index].Refflag != "" && $scope.rows[index].Refflag != null && $scope.rows[index].Refflag != undefined) {
            //    $scope.rows[index].redrefflag = "";
            //    if ($scope.rows[index].RefNo != "" && $scope.rows[index].RefNo != null && $scope.rows[index].RefNo != undefined) {

            //        if ($scope.rows[index].Refflag === "DRAWING & POSITION NUMBER") {
            //            var hh = $scope.rows[index].RefNo;
            //            $scope.rows[index].RefNo = hh.replace(/[\s]/g, '');

            //            var comma = $scope.rows[index].RefNo.slice(-1);

            //            if (comma === ',') {
            //                $scope.rows[index].redrefno = "red";
            //                //$scope.rows[index].RefNo = $scope.rows[index].RefNo.slice(0, -1);
            //            }
            //            else {
            //                $scope.rows[index].redrefno = "";
            //            }
            //            if ($scope.rows[index].RefNo.indexOf(',') === -1) {
            //                $scope.rows[index].redrefno = "red";
            //            }

            //            if (($scope.rows[index].RefNo.split(",").length - 1) > 1) {
            //                $scope.rows[index].redrefno = "red";
            //            }
            //        }
            //        else
            //            $scope.rows[index].redrefno = "";
            //    }
            //    else
            //        $scope.rows[index].redrefno = "red";
            //}
            //else {
            //    $scope.rows[index].redrefflag = "";
            //    $scope.rows[index].redrefno = "";
            //}




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

        $scope.curlchk === 0;
        $scope.checkDuplicate1 = function (form) {          
            if (form != false) {
                $scope.dis = true;
                $scope.docheck();
            } else {
                angular.element('input.ng-invalid').first().focus();
            }

        };
        $scope.docheck = function () {

            // alert("hai");
            var formData = new FormData();
            formData.append("cat", angular.toJson($scope.cat));
            formData.append("attri", angular.toJson($scope.Characteristics));
            formData.append("vendorsupplier", angular.toJson($scope.rows));
            formData.append("Equ", angular.toJson($scope.Equ));
            //  alert(angular.toJson($scope.cat));
            //alert(angular.toJson($scope.cat.Type));
            if ($scope.codelogic1 != "UNSPSC Code") {
                //$scope.cat.Maincode = "Border: 1px solid #a94442";
                // $scope.cat.Subcode = "Border: 1px solid #a94442";
                // $scope.cat.Subsubcode = "Border: 1px solid #a94442";
                if ($scope.codelogic1 == "Item Code") {
                    //  $scope.red_commodity = "";
                    $scope.red_commodity = "";
                    $scope.Maincode3 = "";


                    // alert("hi");
                    $rootScope.cgBusyPromises = $http({
                        url: "/Equipment/checkDuplicate",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (response) {
                        //  alert(angular.toJson(response));
                        //  alert(response)

                        if (response != '') {
                            $scope.listptnodup1 = response;

                            $('#divduplicate').attr('style', 'display: block');
                        }
                        else {
                            //  alert('hi')
                            $scope.createData('No');

                            $('#divduplicate').attr('style', 'display: none');
                            $scope.listptnodup1 = response;
                        }


                    }).error(function (data, status, headers, config) {
                        // alert("error");

                    });
                }
                else {
                    //  alert(angular.toJson($scope.cat.Maincode));



                    $scope.red_commodity = "";
                    $scope.Maincode3 = "";
                    $scope.Subcode3 = "";
                    $scope.Subsubcode3 = "";

                    // alert("hi");
                    $rootScope.cgBusyPromises = $http({
                        url: "/Equipment/checkDuplicate",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (response) {
                        //  alert(angular.toJson(response));
                        // alert(response)
                        if (response != '') {

                            $scope.listptnodup1 = response;

                            $('#divduplicate').attr('style', 'display: block');
                        }
                        else {
                            // alert('hi');
                            $scope.createData('No');

                            $('#divduplicate').attr('style', 'display: none');
                            $scope.listptnodup1 = response;
                        }


                    }).error(function (data, status, headers, config) {
                        // alert("error");

                    });

                }

            }
                //else {
                //    
                //}

            else {
                if ($scope.cat.Unspsc != "" && $scope.cat.Unspsc != null && $scope.cat.Unspsc != undefined) {
                    $scope.red_commodity = "";
                    $scope.Maincode3 = "";
                    $scope.Subcode3 = "";
                    $scope.Subsubcode3 = "";
                    // $scope.red_maincode = "Border: 1px solid #a94442";
                    // alert("hi");
                    $rootScope.cgBusyPromises = $http({
                        url: "/Equipment/checkDuplicate",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (response) {
                        //alert(angular.toJson(response));
                        // alert(response)
                        if (response != '') {
                            $scope.listptnodup1 = response;
                            // $scope.red_maincode = "Border: 1px solid #a94442";
                            $('#divduplicate').attr('style', 'display: block');
                        }
                        else {
                            // alert('hi')
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
                    $scope.Unspsc1 = "active";
                    $scope.desctab = "";
                    $scope.desctab1 = "";
                    $scope.red_commodity = "Border: 1px solid #a94442";
                    // $scope.red_commodity = "";
                    $scope.Maincode3 = "";
                    $scope.Subcode3 = "";
                    $scope.Subsubcode3 = "";

                }
            }
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
            formData.append("vendorsupplier", angular.toJson($scope.rows));
            formData.append("Equ", angular.toJson($scope.Equ));

            //  alert(angular.toJson($scope.Equ));
            $http({
                url: "/Equipment/GenerateShortLong",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
                //  alert(angular.toJson(response))
                $scope.cat.Shortdesc = response[0];
                $scope.cat.Longdesc = response[1];
                $scope.cat.MissingValue = response[2];
                $scope.cat.EnrichedValue = response[3];
                $scope.cat.RepeatedValue = response[4];
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
            $scope.Partnum = false;
            $('#divPartnum').attr('style', 'display: none');
            $scope.listptnodup = null;
        };

        $scope.closeData1 = function () {
            $scope.dis = false;
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
                    url: '/Dictionary/GetNounModifier',
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



        $scope.NotifiyResclose = function () {

            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.SelectCharater = function (Noun, Modifier, Attribute) {


            $http({
                method: 'GET',
                url: '/Catalogue/GetValues',

                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute }
            }).success(function (response) {
                $scope.Values = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };
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


      

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {
            //  Value = Value.replace('&', '***');

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

                var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value }, true);

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
                    params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value }

                }).success(function (response) {


                    if (response === "false") {
                        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
                        $('#checkval' + indx).attr('style', 'display:block');
                        $scope.Characteristics[indx].Abbrivation = "";


                    }
                    else {

                        $scope.Characteristics[indx].Abbrivation = response;

                        $http({
                            method: 'GET',
                            url: '/Catalogue/CheckValue1',
                            params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value }

                        }).success(function (response) {
                            if (response === "false") {

                                $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
                                $('#checkval' + indx).attr('style', 'display:block');

                            }
                            else {
                                $('#btnabv' + indx).attr('style', 'display:none');
                                $('#checkval' + indx).attr('style', 'display:block');
                                //  $scope.Characteristics[indx].Abbrivation = $scope.Characteristics[indx].Abbrivation;
                            }
                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });

                    }

                    //  alert(angular.toJson(response));
                    //  alert(angular.toJson(indx));

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                //  alert(angular.toJson($scope.Characteristics));
                $http({
                    method: 'GET',
                    url: '/Catalogue/getunitforvalue',
                    params: { Value: Value }
                }).success(function (response) {
                    if (response != null) {
                        $scope.Characteristics[indx].UOM = response;
                    }
                    // alert(response);
                    //if (response != null) {
                    //    angular.forEach($scope.UOMList, function (valueee) {
                    //        if(valueee == response)
                    //        {
                    //          //  alert("hi");
                    //            $scope.Characteristics[indx].UOM = response;
                    //        }
                    //    });

                    //}
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });




            } else if (Attribute == "TYPE" || Attribute == "MATERIAL") {
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
            }

            else {

                $('#checkval' + indx).attr('style', 'display:none');
            }

        };
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

            // Value = Value.replace('&', '***');
            // abb = abb.replace('&', '***');

            $http({
                method: 'GET',
                url: '/Catalogue/AddValue',
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: abb }
            }).success(function (response) {


                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:block');

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }

        $scope.CancelValue = function (indx) {
            $('#checkval' + indx).attr('style', 'display:none');
            $scope.Characteristics[indx].Abbrivation = null;
        }

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

            $http.get('/GeneralSettings/GetSubGroupcodeList1', { params: { MainGroupCode: $scope.cat.Maincode } }
           ).success(function (response) {
               $scope.getsubgroup = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }

        $scope.getSubsubcode = function () {

            if ($scope.cat.Subcode != null)
                $scope.Subcode3 = "";

            $http.get('/GeneralSettings/GetSubsubGroupcodeList', { params: { SubGroupCode: $scope.cat.Subcode } }
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

        $scope.Clarbtn = function () {
            $scope.sts3 = false;
            $scope.sts2 = true;
            $scope.sts4 = true;
            $scope.ven = false;

        }
        //reject
        $scope.dis1 = false;
        $scope.RejectData = function (usr) {
            // alert(angular.toJson($scope.cat));
            var formData = new FormData();
            if ($scope.cat.Remarks == undefined || $scope.cat.Remarks == null || $scope.cat.Remarks == "" || $scope.cat.Remarks == "CATALOGUER-REMARK:") {
                alert(angular.toJson("Please Provide Remarks for Clarification"));
            }
            else {


                if (confirm("Are you sure, send to clarification this record?")) {
                    $scope.dis1 = true;

                    $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 5000);
                    formData.append("cat", angular.toJson($scope.cat));
                    formData.append("attri", angular.toJson($scope.Characteristics));
                    formData.append("Equ", angular.toJson($scope.Equ));
                    formData.append("vendorsupplier", angular.toJson($scope.rows));
                    // $scope.createData('Yes');
                    // $scope.checkDuplicate();
                    // $rootScope.cgBusyPromises = $http({
                    //     method: 'GET',
                    ////     url: '/Catalogue/GetRejectCode?Itemcode=' + $scope.codeforreject + '&Remarks=' + $scope.cat.Remarks + '&usr=' + $scope.usr
                    //     url: '/Catalogue/GetRejectCode',
                    //     params: { Itemcode: $scope.codeforreject, Remarks: $scope.cat.Remarks, usr: $scope.usr, Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                    $rootScope.cgBusyPromises = $http({
                        method: "POST",
                        url: '/Catalogue/GetRejectCode',
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData

                    }).success(function (response) {
                        $scope.Res = "Record sent to Clarification"
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        //  alert(angular.toJson($scope.checkSubmit));
                        // $scope.checkSubmit();

                        $scope.reset();
                        $scope.dis1 = false;
                        $scope.Type = null;
                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.LoadData();
                        // $scope.reset1();

                        $scope.sts2 = false;
                        $scope.sts3 = false;
                        $scope.sts4 = false;

                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                }
            }



        };

        $scope.bumu = '0';

        $scope.chkmuall = function () {
            //  alert(angular.toJson($scope.bumu));

            if ($scope.bumu === 1) {
                angular.forEach($scope.DataList, function (value1, key1) {
                    //   alert(angular.toJson(value1.Materialcode));
                    //  alert(angular.toJson(value1.ItemStatus));
                    if (value1.ItemStatus === 1) {
                        if (value1.bu != 1)
                            value1.bu = '1';
                    }
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



        $scope.chkmuone = function (indx) {
            //  alert(angular.toJson($scope.bumu));


            //  alert(angular.toJson($scope.DataList[indx].ItemStatus));
            if ($scope.DataList[indx].ItemStatus === 0) {
                //   alert('in');
                $scope.DataList[indx].bu = 0;
            }
            // if ($scope.DataList[indx].bu != 1)

            //}
            //});

            //else {
            //    angular.forEach($scope.DataList, function (value1, key1) {
            //    if ($scope.DataList[indx].bu != 0)
            //        $scope.DataList[indx].bu = '0';
            //    });
            //}
            // alert(angular.toJson($scope.DataList));
        };


        $scope.fromsave = 0;
        //Search 

        $scope.searchMaster = function (sCode, sSource, sNoun, sModifier, sUser) {

            $timeout(function () {

                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
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
                $rootScope.cgBusyPromises = $http({
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

                $scope.LoadData();

            }

        }

        //Equipment

        $scope.addEquip = function () {

            $scope.reset();

            $scope.cat = {};
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

            $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
            $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
            $scope.Equipments = null;
            $scope.DataList = null;

            $scope.cat.category = "Equipment";
        }
        $scope.addSpare = function () {

            $scope.reset();

            $scope.cat = {};
            $scope.Characteristics = null;
            $scope.gen = null;
            $scope.plant = null;
            $scope.mrpdata = null;
            $scope.sales = null;
            $scope.Equ = null;
            $scope.rows = null;
            $scope.rows1 = null;
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

            $scope.rows = [];
            $scope.rows1 = [];
            $http({
                method: 'GET',
                url: '/Equipment/GetSingleItem?itemcode=' + $scope.Equipments
            }).success(function (response) {

                if (response.Vendorsuppliers.length > 0 && response.Vendorsuppliers != '' && response.Vendorsuppliers != null && response.Vendorsuppliers != 'null') {

                    angular.forEach(response.Vendorsuppliers, function (obj) {

                        if (obj.Type === "MANUFACTURER") {

                            $scope.rows.push(obj);
                        }


                    });
                    if ($scope.rows.length == 0) {
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                    }

                    $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
                } else {
                    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                    $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];
                }



            }).error(function (data, status, headers, config) {


            });


            $scope.cat.category = null;
        }
        $scope.RemoveEquip = function () {

            if ($filter('filter')($scope.DataList1, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList1, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();

                formData.append("DataList", angular.toJson($scope.DataList2));


                $scope.promise = $http({
                    url: "/Equipment/RemoveEquipment",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data > 0) {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data + " Data removed successfully";
                        $scope.Notify = "alert-info";

                        $scope.DataList = null;
                        $scope.Equipments = null;
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                    } else {
                        $scope.Res = "Data removing failed"
                        $scope.Notify = "alert-danger";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to copy");
                //$scope.LoadData();
            }
        }
        $scope.PotentialEquip = function () {

            if ($filter('filter')($scope.DataList1, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList1, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();
                formData.append("DataList1", angular.toJson($scope.DataList1));
                formData.append("DataList", angular.toJson($scope.DataList2));


                $scope.promise = $http({
                    url: "/Equipment/PotentialEquip",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data != "") {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-info";
                        $scope.rows = null;
                        $scope.DataList = null;
                        $scope.Equipments = null;
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                    } else {
                        $scope.Res = "No potential duplicates"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to check potential duplicate");
                //$scope.LoadData();
            }
        }
        $scope.CopyEquip = function () {

            if ($filter('filter')($scope.DataList1, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList1, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();

                formData.append("DataList", angular.toJson($scope.DataList2));


                $scope.promise = $http({
                    url: "/Equipment/CopyEquip",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data != "") {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data + " equipments created successfully";
                        $scope.Notify = "alert-info";
                        $scope.rows = null;
                        $scope.DataList = null;
                        $scope.Equipments = null;
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                    } else {
                        $scope.Res = "No equipments created"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to Remove");
                //$scope.LoadData();
            }
        }

        $scope.RemoveSpare = function () {

            if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();

                formData.append("DataList", angular.toJson($scope.DataList2));
                formData.append("Equip", $scope.Equipments);

                $scope.promise = $http({
                    url: "/Equipment/RemoveSpare",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data > 0) {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data + " Data removed successfully";
                        $scope.Notify = "alert-info";


                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');

                        $scope.LoadSpares($scope.Equipments);
                    } else {
                        $scope.Res = "Data removing failed"
                        $scope.Notify = "alert-danger";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to remove");
                //$scope.LoadData();
            }
        }
        $scope.PotentialSpare = function () {

            if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();
                formData.append("DataList1", angular.toJson($scope.DataList));
                formData.append("DataList", angular.toJson($scope.DataList2));


                $scope.promise = $http({
                    url: "/Equipment/PotentialEquip",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data != "") {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-info";
                        $scope.rows = null;
                        $scope.DataList = null;
                        $scope.Equipments = null;
                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                    } else {
                        $scope.Res = "No potential duplicates"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to check potential duplicate");
                //$scope.LoadData();
            }
        }
        $scope.CopySpare = function () {

            if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                $scope.DataList2 = $filter('filter')($scope.DataList, { 'bu': 1 })

            }

            if ($scope.DataList2 != "" && $scope.DataList2 != undefined) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);

                // $scope.DataList2 = $filter('filter')($scope.DataList2, { 'ItemStatus': '1' })

                var formData = new FormData();

                formData.append("DataList", angular.toJson($scope.DataList2));
                formData.append("Equip", $scope.Equipments);

                $scope.promise = $http({
                    url: "/Equipment/CopyEquip",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data != "") {

                        $scope.reset();

                        $scope.cat = null;
                        $scope.Characteristics = null;
                        $scope.Equ = null;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                        $scope.sts4 = false;
                        $scope.sts3 = false;
                        $scope.Res = data + " spares created successfully";
                        $scope.Notify = "alert-info";

                        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                        $scope.rows1 = [{ 'slno': 1, 's': '1', 'l': '1' }];

                        $('#divNotifiy').attr('style', 'display: block');

                        $scope.LoadSpares($scope.Equipments);

                    } else {
                        $scope.Res = "No spares created"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select an items to Remove");
                //$scope.LoadData();
            }
        }


        //search func loc

        $scope.SearchFL = function () {
            $scope.FLall = 'false';
            if ($scope.funloc != '' && $scope.funloc != null) {
                $http({
                    method: 'GET',
                    url: '/Equipment/GetFunLoc?srch=' + $scope.funloc
                }).success(function (response) {
                    $scope.funlocList = response;
                    angular.forEach($scope.funlocList, function (lst) {
                        lst.bu = 0;

                    });

                    if ($scope.funlocList != undefined && $scope.funlocList != null && $scope.funlocList != '') {
                        $scope.ShowFL();
                    } else {
                        $scope.Res = "No data found";
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');

                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } else {
                $scope.Res = "Please enter Functional Location/Model NO./Serial NO.";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');


            }

        }
        $scope.ShowFL = function () {

            new jBox('Modal', {
                width: 550,
                height: 500,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: 'title',
                closeButton: true,
                content: jQuery('#FLcontent'),
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
        $scope.AssignFL = function () {

            $scope.funlocList1 = null;

            if ($filter('filter')($scope.funlocList, { 'bu': 1 }).length >= 1) {

                $scope.funlocList1 = $filter('filter')($scope.funlocList, { 'bu': 1 })

            }

            if ($scope.funlocList1 != null) {
                var modelFlg = 0, serialFlg = 0;
                angular.forEach($scope.funlocList, function (lst) {

                    if (lst.Modelno != null && modelFlg == 0) {
                        if ($scope.rows1[$scope.rows1.length - 1].Refflag != null) {
                            $scope.rows1.push({ 'slno': $scope.rows1.length + 1, 's': '0', 'l': '1' });
                        }
                        $scope.rows1[$scope.rows1.length - 1].Refflag = "MODEL NUMBER";
                        $scope.rows1[$scope.rows1.length - 1].RefNo = lst.Modelno;
                        modelFlg = 1;

                    }
                    if (lst.ManufSerialNo != null && serialFlg == 0) {
                        if ($scope.rows1[$scope.rows1.length - 1].Refflag != null) {
                            $scope.rows1.push({ 'slno': $scope.rows1.length + 1, 's': '0', 'l': '1' });
                        }
                        $scope.rows1[$scope.rows1.length - 1].Refflag = "SERIAL NUMBER";
                        $scope.rows1[$scope.rows1.length - 1].RefNo = lst.ManufSerialNo;
                        serialFlg = 1;

                    }

                    if (lst.FunctLocation != null) {
                        if ($scope.rows1[$scope.rows1.length - 1].Refflag != null) {
                            $scope.rows1.push({ 'slno': $scope.rows1.length + 1, 's': '0', 'l': '1' });
                        }
                        $scope.rows1[$scope.rows1.length - 1].Refflag = "TAG NUMBER";
                        $scope.rows1[$scope.rows1.length - 1].RefNo = lst.FunctLocation;


                    }


                });
                $scope.Res = "Assigned successfully";
                $scope.Notify = "alert-info";
                $('#divNotifiy').attr('style', 'display: block');

            } else {
                $scope.Res = "Please select atleast one item";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');

            }

        }
        $scope.chkFLall = function () {

            if ($scope.FLall == 1) {
                angular.forEach($scope.funlocList, function (lst) {
                    lst.bu = 1;

                });
            } else {
                angular.forEach($scope.funlocList, function (lst) {
                    lst.bu = 0;

                });
            }
        }

        $scope.addExist = function () {

            if ($scope.PMIcode != null && $scope.PMIcode != undefined) {
                $http({
                    method: 'GET',
                    url: '/Equipment/FetchExist?Srchcode=' + $scope.PMIcode
                }).success(function (response) {

                    if (response != '') {
                        $scope.pmiList = response;
                        angular.forEach($scope.pmiList, function (lst) {
                            lst.bu = '0';

                        });
                    } else {
                        $scope.Res = "Enter valid part number/material code/item code";
                        $scope.Notify = "alert-danger";

                        $('#divNotifiy').attr('style', 'display: block');

                    }
                }).error(function (data, status, headers, config) {

                });
            } else {
                $scope.Res = "Enter valid part number/material code/item code";
                $scope.Notify = "alert-danger";

                $('#divNotifiy').attr('style', 'display: block');

            }
        }

        $scope.addExistDB = function () {

            if ($filter('filter')($scope.pmiList, { 'bu': '1' }).length >= 1) {

                $scope.pmiList1 = $filter('filter')($scope.pmiList, { 'bu': 1 })

            }

            if ($scope.pmiList1 != "" && $scope.pmiList1 != undefined) {

                $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 30000);

                $scope.pmiList1[0].Equipments = $scope.Equipments;

                var formData = new FormData();
                formData.append("SparesLst", angular.toJson($scope.pmiList1));

                $scope.promise = $http({
                    url: "/Equipment/AddexistSpares",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data > 0) {

                        $scope.Res = data + " Items added successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadSpares($scope.Equipments);
                        $scope.pmiList = null;
                    } else {
                        $scope.Res = "Data adding failed"
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select items to add");

            }

        }

        //preview
        $scope.PreviewPopup = function () {
          
            $scope.equip = {};

            $http({
                method: 'GET',
                url: '/Equipment/GetSingleItem',
                params: { itemcode: $scope.Equipments }
            }).success(function (response) {
                $scope.equip = response;

                $http({
                    method: 'GET',
                    url: '/Equipment/GetSpareList',
                    params: { itemCode: $scope.Equipments }
                }).success(function (response) {
                    $scope.spareList = response;


                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

             
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
          

         

            new jBox('Modal', {
                width: 900,
                height: 1000,
                blockScroll: true,
                animation: 'zoomIn',
                draggable: false,
                overlay: true,
                closeButton: true,
                content: jQuery('#cotentid7'),
                reposition: false,
                repositionOnOpen: false
            }).open();
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
                    url: "/Dictionary/AutoCompleteNoun?term=" + term,
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

                        $.get("/Dictionary/GetModifier", { Noun: selectedItem.item.value }).success(function (response) {
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
                    url: "/Catalogue/AutoCompleteVendor?term=" + term,
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
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4) + (autocompleteResult.Acquiredby == null ? '' : ' (Acquired by ' + autocompleteResult.AcquiredCompanyName + ')'),
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {

                        //if (selectedItem.item.value.AcquiredCompanyName == null) {
                        scope.cat.Manufacturer = selectedItem.item.value.Name + (selectedItem.item.value.Name2 = selectedItem.item.value.Name2 == null ? '' : selectedItem.item.value.Name2) + (selectedItem.item.value.Name3 = selectedItem.item.value.Name3 == null ? '' : selectedItem.item.value.Name3) + (selectedItem.item.value.Name4 = selectedItem.item.value.Name4 == null ? '' : selectedItem.item.value.Name4);

                        // } else {

                        //  scope.cat.Manufacturer = selectedItem.item.value.AcquiredCompanyName;

                        // }

                        //scope.cat.Manufacturer = selectedItem.item.value.Name + (selectedItem.item.value.Name2 = selectedItem.item.value.Name2 == null ? '' : selectedItem.item.value.Name2) + (selectedItem.item.value.Name3 = selectedItem.item.value.Name3 == null ? '' : selectedItem.item.value.Name3) + (selectedItem.item.value.Name4 = selectedItem.item.value.Name4 == null ? '' : selectedItem.item.value.Name4) + (selectedItem.item.value.AcquiredCompanyName == null ? '' : selectedItem.item.value.AcquiredCompanyName);
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
                    url: "/Catalogue/AutoCompleteVendor?term=" + term,
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
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4) + (autocompleteResult.Acquiredby == null ? '' : ' (Acquired by ' + autocompleteResult.AcquiredCompanyName + ')'),
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem, http) {
                        //if (selectedItem.item.value.AcquiredCompanyName == null) {
                        scope.rw.Name = selectedItem.item.value.Name + (selectedItem.item.value.Name2 = selectedItem.item.value.Name2 == null ? '' : selectedItem.item.value.Name2) + (selectedItem.item.value.Name3 = selectedItem.item.value.Name3 == null ? '' : selectedItem.item.value.Name3) + (selectedItem.item.value.Name4 = selectedItem.item.value.Name4 == null ? '' : selectedItem.item.value.Name4);
                        scope.rw.Code = selectedItem.item.value.Code;
                        // } else {

                        //  scope.rw.Name = selectedItem.item.value.AcquiredCompanyName;
                        //  scope.rw.Code = selectedItem.item.value.Acquiredby;
                        // }

                        //scope.uo.UOM = selectedItem.item.value.Unitname;
                        scope.$apply();
                        event.preventDefault();
                    }
                });

            }

        };
    }]);



})();