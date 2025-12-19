
(function () {
    'use strict';


    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter', 'datatables']);
  
    app.controller('CatalogueController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {

        $scope.cat = {};
        $scope.cat.Specification = "";

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
                        else {
                            $scope.erp.ReOrderPoint_ = $scope.erp.Quantity_;
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
        $scope.Compare = function (Legacy1, Legacy) {


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
                url: '/Dictionary/GetNounModifier2',
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
            angular.forEach($scope.DataList, function (value, key) {
                if (value.ItemStatus == 1) {
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


                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier2',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;

                        $scope.Characteristics = response.ALL_NM_Attributes;

                        //Autofill

                        //console.log($scope.cat.Noun, $scope.cat.exNoun);
                        //console.log($scope.cat.Modifier, $scope.cat.exModifier);
                        //if ($scope.cat.Noun == $scope.cat.exNoun && $scope.cat.Modifier == $scope.cat.exModifier && $scope.cat.Characteristics == null) {
                        //    $scope.lstCharateristics1 = [];

                        //    //if ($scope.cat.exCharacteristics !== null) {
                        //    //    angular.forEach($scope.cat.exCharacteristics, function (pattri) {
                        //    //        var AttrMdl = {
                        //    //            Characteristic: pattri.Characteristic,
                        //    //            Value: pattri.Value,
                        //    //            UOM: pattri.UOM,
                        //    //            Squence: pattri.Squence,
                        //    //            ShortSquence: pattri.ShortSquence,
                        //    //            Source: pattri.Source,
                        //    //            SourceUrl: pattri.SourceUrl
                        //    //        };

                        //    //        $scope.lstCharateristics1.push(AttrMdl);
                        //    //    });
                        //    //}
                        //    //$scope.cat.Characteristics = $scope.lstCharateristics1;
                        //    $scope.Characteristics = $scope.cat.exCharacteristics;
                        //    console.log($scope.cat.Characteristics)
                        //}

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
                //  data: formData,
                params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
            }).success(function (response) {

                if (response != null && response.length > 0) {
                    // angular.forEach(response, function (obj) {
                    //     var cnt = 0.0;
                    //     var cnt1 = 0.0;
                    //     angular.forEach($scope.cat.Characteristics, function (value1, key1) {
                    //         if (value1.Value != null && value1.Value != '')
                    //         {
                    //             cnt1++;
                    //         }
                    //     angular.forEach(obj.Characteristics, function (value2, key2) {

                    //         if (value1.Characteristic === value2.Characteristic && value1.Value === value2.Value) {
                    //             cnt++;
                    //         }
                    //     });
                    //     });

                    //     //calc
                    //     var res = ((cnt / cnt1) * 100);
                    //     obj.batch = res;
                    //     //alert(cnt)
                    //     //alert(cnt1)
                    //     //alert(res)

                    // });
                    //// $scope.checkSimilar();

                    $scope.LstsimItems = response;
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



            if ($scope.sNoun == null) {

                $http({
                    method: 'GET',
                    url: '/Catalogue/GetDataList'
                }).success(function (response) {
                    $scope.DataList = response;
                    console.log(response)

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


                    $scope.saveditems = ($filter('filter')($scope.DataList, { 'ItemStatus': '1' })).length;
                    $scope.balanceitems = ($filter('filter')($scope.DataList, { 'ItemStatus': '0' })).length;


                    $scope.checkSubmit();

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
            else {

                $scope.searchMaster();
            }

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

            $window.open('/Catalogue/Downloadfile?ItemId=' + imgId + '&fName=' + fileName );

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
            //alert(sts);
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

            //newly added fields
            //  $scope.erp.TaxClassification2_ = $scope.erp.TaxClassification2 != null ? $("#ddltaxclass2").find("option:selected").text() : null;
            $scope.erp.DeliveringPlant_ = $scope.erp.DeliveringPlant != null ? $("#ddldelplant").find("option:selected").text() : null;
            $scope.erp.TransportationGroup_ = $scope.erp.TransportationGroup != null ? $("#ddltransgrp").find("option:selected").text() : null;
            $scope.erp.LoadingGroup_ = $scope.erp.LoadingGroup != null ? $("#ddlloadgrp").find("option:selected").text() : null;
            $scope.erp.OrderUnit_ = $scope.erp.OrderUnit != null ? $("#ddlorderunit").find("option:selected").text() : null;
            $scope.erp.AutomaticPO_ = $scope.erp.AutomaticPO != null ? $("#ddlatmpo").find("option:selected").text() : null;


            // formData.append('files', $scope.attachment);

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
                    $('#divNotifiy').attr('style', 'display: block');

                }
                else {

                    if (data > -1) {

                        if (data == 8) {

                            $scope.Notify = "alert-danger";
                            $scope.Res = "Please add vendor in vendor master"
                            $scope.NotifiyRes = true;
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.dis = false;
                        } else if (data == 9) {

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

            if ($scope.cat.RevRemarks == undefined || $scope.cat.RevRemarks == null || $scope.cat.RevRemarks == "" || $scope.cat.RevRemarks == "CATALOGUER-REMARK:") {
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
           
            // alert("mod");
            //alert(angular.toJson(noun));


            //$scope.Type = null;
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
            $scope.cat.Noun = noun.toUpperCase();
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params :{Noun: noun}
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }

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

        $scope.editAction = true;
        $scope.RowClick = function (lst, idx) {


            //var values = input_string.join('').replace(/","/g, '\n');

            //$scope.values = values;
            //alert($scope.values)
            //var array = ["PUMP,PARTS/ACCESSORIES,", "GASKET,MATERIAL:STAINLESS STEEL + GRAPHITE,MANUFACTURER:NUOVO PIGNONE,PART NUMBER:KFZ239680144,", " ADDITIONAL INFORMATION:DRAWING NUMBER:SOA31509/3, POSITION NUMBER:17A ,", " EQUIPMENT NAME:PUMP,", " EQUIPMENT SERIALNO:P15186 & 87,", ""];

            //var resultString = "";

            //array.forEach(function (item) {
            //    resultString += item + '\n';
            //});

            //alert(resultString);


            $scope.attcount1 = false;
            var usr = $('#usrHid').val();
            $scope.currentUsr = usr;
            $scope.showlist = true;
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

        

            //  alert(angular.toJson($scope.tempPlace));

            var i = 0;
            angular.forEach($scope.DataList, function (lst1) {
                $('#' + i).attr("style", "");
                i++;
            });
            $('#' + idx).attr("style", "background-color:lightblue");

            //   alert(angular.toJson(lst));

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
            $scope.ItmCode = lst.Itemcode;
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
                // $scope.Equ.ENS = '1';
                //   $scope.Equ.EMS = '1';
                // alert(angular.toJson($scope.Equ));
                if ((response.Catalogue != null && response.Catalogue.Name == usr) && (response.ItemStatus == 0 || response.ItemStatus == 1 || response.ItemStatus == -1 || response.ItemStatus == 13))
                    $scope.editAction = true;
                else $scope.editAction = false;

              

             

                var tst = response.Vendorsuppliers;
                if (tst != null && tst != '') {
                    $scope.rows = response.Vendorsuppliers;
                    angular.forEach($scope.rows, function (rw) {
                        rw.l = '1';
                    });
                } else {
                    $scope.rows = null;
                    $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                }


                console.log($scope.rows)
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

        $scope.ddlusrChange =function () {          
            if ($scope.usr === undefined || $scope.usr === null) {
                $scope.revshow = true;              
            }
            else {
                $scope.revshow = false;          
                
            }
        };


      

        $scope.SubmitData = function () {          
              
                if ($filter('filter')($scope.DataList, { 'bu': '1' }).length >= 1) {

                    $scope.DataList = $filter('filter')($scope.DataList, { 'bu': '1' })

                }

                $scope.DataList = $filter('filter')($scope.DataList, { 'ItemStatus': '1' });

            //  alert(angular.toJson($scope.DataList));
            console.log($scope.DataList)
            if ($scope.DataList != "" && $scope.DataList != undefined) {
                if ($window.confirm("Do you want to submit the (Count:" + $scope.DataList.length + ") data?")) {
                        $timeout(function () {

                            $('#divNotifiy').attr('style', 'display: none');
                        }, 5000);

                        var formData = new FormData();

                        formData.append("DataList", angular.toJson($scope.DataList));
                        
                    $scope.cgBusyPromises = $http({
                        url: "/Catalogue/InsertDataList",
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
                            $scope.showlist = false;
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
                }
                else
                {
                        alert("Select saved items to submit");
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
        $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1','Name':'' }];
        $scope.addRow = function () {

            //var flg = 0;
            //($scopeangular.forEach.rows, function (value, key) {
                
            //    if ((value.Name == null || value.Name == '') && ( value.RefNo == null || value.RefNo == '')) {
                   
            //        flg = 1;
            //    }
            //});
            //if (flg == 0) {
            //    $scope.rows.push({ 'slno': $scope.rows.length + 1,'s':'0','l':'0' });
               
            //}
            $scope.rows.push({ 'slno': $scope.rows.length + 1, 's': '1', 'l': '1' });
            // alert(angular.toJson($scope.rows));

        };

        $scope.RemoveRow = function (indx) {

           // alert("hi")
            //if ($scope.rows[indx].s != 1)
            //{
                if ($scope.rows.length > 1)
                {
                    $scope.rows.splice(indx, 1);
                    //  $scope.rows = [{ 'slno': 1, 's': '0', 'l': '1' }];
                }

                if ($scope.rows.length === 1 && indx === 0)
                {
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
                    params : {Partno : $scope.rows[index].RefNo , icode:  $scope.cat.Itemcode , Flag :$scope.rows[index].Refflag}
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
        $scope.curlchk === 0;
        $scope.checkDuplicate = function (form) {
         
           // alert("hai");
            //  if ($scope.myres1 != true) {
          //alert(form)
            if (form != false) {
                $scope.dis = true;
                $scope.createData('No');
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
        $scope.docheck = function () {
            
           // alert("hai");
            var formData = new FormData();
            formData.append("cat", angular.toJson($scope.cat));
          
            formData.append("attri", angular.toJson($scope.Characteristics));
            formData.append("vendorsupplier", angular.toJson($scope.rows));
            formData.append("Equ", angular.toJson($scope.Equ));
            console.log($scope.cat.Unspsc);
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
                    $rootScope.cgBusyPromises=    $http({
                        url: "/Catalogue/checkDuplicate",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (response) {
                      //  alert(angular.toJson(response));
                      //  alert(response)

                        $scope.createData('No');
                        //if (response != '') {
                        //    $scope.listptnodup1 = response;

                        //    $('#divduplicate').attr('style', 'display: block');
                        //}
                        //else {
                          
                        //    //  alert('hi')
                        //    $scope.createData('No');

                        //    $('#divduplicate').attr('style', 'display: none');
                        //    $scope.listptnodup1 = response;
                        //}


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
                        url: "/Catalogue/checkDuplicate",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (response) {
                      //  alert(angular.toJson(response));
                        // alert(response)
                        $scope.createData('No');
                        //if (response != '') {

                        //    $scope.listptnodup1 = response;

                        //    $('#divduplicate').attr('style', 'display: block');
                        //}
                        //else {
                        //    // alert('hi');
                        //    $scope.createData('No');

                        //    $('#divduplicate').attr('style', 'display: none');
                        //    $scope.listptnodup1 = response;
                        //}


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
                        url: "/Catalogue/checkDuplicate",
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
                 
                    $scope.desctab = "";
                   
                    $scope.planttab = "";
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
            formData.append("exattri", angular.toJson($scope.cat.exCharacteristics));
            console.log(angular.toJson($scope.cat.exCharacteristics))
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
                    //alert($scope.cat.exMissingValue)
                    $scope.cat.RepeatedValue = response[6];
                    // alert(angular.toJson($scope.RepeatedValue));
                    $scope.Res = "Short and long generated.";
                    $scope.NotifiyRes = true;
                    $scope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
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


        $scope.fillData=function(ls){          

           // $scope.cat = {};
            $http({
                method: 'GET',
                url: '/Catalogue/GetSingleItem',
                params :{itemcode:ls.Itemcode}
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
                    params : {Noun : $scope.cat.Noun , Modifier :$scope.cat.Modifier}
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
                    params : {Noun : $scope.cat.Noun}
                }).success(function (response) {
                    $scope.Modifiers = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });



                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier2',
                    params : {Noun : $scope.cat.Noun , Modifier :$scope.cat.Modifier }
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
                        select:function(event,ui){
                            $scope.Characteristics[inx].Value = ui.item.label;
                        }
                    }).autocomplete("search", "");
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
              

         
          
             //  $("#values" + inx).click(function () { $(this) });
             
              
        };
      
      
        
        
        $scope.SelectEquip = function (Name) {


            $http({
                method: 'GET',
                url: '/Catalogue/GetEquip',
                params : {EName :Name}
            }).success(function (response) {
                $scope.Equips = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };

        var microSign = "\u00B5";
        $scope.convertMicroSymbol = function (Value) {

            if (Value.toUpperCase().includes("MICRO")) {
                Value = Value.replace(/MICRO/gi, microSign);
                console.log(Value);
            }

        };

        $scope.valChange = function (value, idx) {
            if (value == null || value == "" || value == undefined) {
                $scope.Characteristics[idx].UOM = "";
            }
        }

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx, Uom) {
            //  Value = Value.replace('&', '***');
            console.log(Uom);
            if (Value == null || Value == "" || Value == undefined) {
                $scope.Characteristics[indx].UOM = "";
            }
            if (Value != null && Value != '') {
                if (Value.includes("Micro")) {
                    Value = Value.replace("Micro", microSign);
                }
                if (Value.includes("micro")) {
                    Value = Value.replace("micro", microSign);
                }
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
                    params: { Noun: Noun, Modifier:Modifier,Attribute: Attribute, Value: Value }

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
                            url: '/Catalogue/CheckValue1' ,
                            params: { Noun: Noun, Modifier:Modifier,Attribute: Attribute, Value: Value }

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
                        params :{Value:Value}
                    }).success(function (response) {
                        if (response != null) {
                            $scope.Characteristics[indx].UOM = Uom;
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
      

        $scope.AddValue = function (Noun, Modifier, Attribute, Value, abb, indx) {

           // Value = Value.replace('&', '***');
           // abb = abb.replace('&', '***');
          
            $http({
                method: 'GET',
                url: '/Catalogue/AddValue',
                params : {Noun : Noun ,Modifier:Modifier , Attribute : Attribute , Value : Attribute , abb:abb }
            }).success(function (response) {   
                
            
                    $('#btnabv' + indx).attr('style','display:none');
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
     
        $scope.Clarbtn = function () {
            $scope.sts3 = false;
            $scope.sts2 = true;
            $scope.sts4 = true;
            $scope.ven = false;

        }
        $scope.remarksChange = function () {
            if ($scope.cat.RevRemarks == undefined || $scope.cat.RevRemarks == null || $scope.cat.RevRemarks == "" || $scope.cat.RevRemarks == "CATALOGUER-REMARK:") {
                $scope.isClf = true;
            }
            else {
                $scope.isClf = false;
            }
        }
        //reject
        $scope.dis1 = false;
        $scope.isClf = false;
        $scope.isClfClicked = false;
        $scope.RejectData = function (usr) {
            $scope.isClfClicked = true;
            // alert(angular.toJson($scope.cat));
            var formData = new FormData();
            if ($scope.cat.RevRemarks == undefined || $scope.cat.RevRemarks == null || $scope.cat.RevRemarks == "" || $scope.cat.RevRemarks == "CATALOGUER-REMARK:")
            {
                $scope.isClf = true;
                alert(angular.toJson("Please Provide Remarks for Clarification"));
            }
            else {

               
                if (confirm("Are you sure, send to clarification this record?")) {
                    $scope.dis1 = true;
                    console.log($scope.cat)
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
                    $scope.isClfClicked = false;
                    $scope.isClf = false;
                       
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
            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sShort != undefined && $scope.sShort != '') || ($scope.sLong != undefined && $scope.sLong != '') || ($scope.sNoun != undefined && $scope.sNoun != '') || ($scope.sModifier != undefined && $scope.sModifier != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sType != undefined && $scope.sType != '') || ($scope.sStatus != undefined && $scope.sStatus != ''))
            {

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
                        else
                        {
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

        //HSN


        $scope.Searchhsn = function (hsn) {

            $scope.promise = $http({
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

        //Autofill
        $scope.autoFill = function () {

            //$http({
            //    method: 'GET',
            //    url: '/Catalogue/GetSingleItem',
            //    params: { itemcode: $scope.ItmCode }
            //}).success(function (response) {

            //    $scope.cat = response;
            //    console.log($scope.cat)
            //}).error(function (data, status, headers, config) {
            //    // alert("error");

            //});

            //alert($scope.cat.Noun, $scope.cat.exNoun);
            //alert($scope.cat.Modifier, $scope.cat.exModifier);
            console.log($scope.cat.exCharacteristics);
            if (
                $scope.cat.Noun == $scope.cat.exNoun &&
                $scope.cat.Modifier == $scope.cat.exModifier
                //&&($scope.cat.Characteristics == null || $scope.cat.Characteristics.length == 0)
            ) {
                //$scope.lstCharateristics1 = [];
                //$scope.Characteristics = $scope.Characteristics.filter(i => i.Definition == "MM"));
                if ($scope.cat.exCharacteristics !== null && $scope.cat.exCharacteristics.length > 0) {
                    angular.forEach($scope.cat.exCharacteristics, function (ex) {
                        angular.forEach($scope.Characteristics, function (ne) {
                            if (ex.Characteristic === ne.Characteristic) {
                                console.log('Match found:', ex.Characteristic, '->', 'Old Value:', ex.Value, 'New Value:', ne.Value);
                                return ne.Value = ex.Value;
                            }
                        });
                    });
                }
                //if ($scope.cat.exCharacteristics !== null && $scope.cat.exCharacteristics.length > 0) {
                //    angular.forEach($scope.cat.Characteristics, function (ex) {
                //        angular.forEach($scope.cat.exCharacteristics, function (ne) {
                //            if (ex.Characteristic === ne.Characteristic) {
                //                console.log('Match found:', ex.Characteristic, '->', 'Old Value:', ex.Value, 'New Value:', ne.Value);
                //                return ex.Value = ne.Value;
                //            }
                //        });
                //    });
                //}
                else {
                    $scope.Res = "Existing details are empty."
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                //var mergedList = [];

                //if ($scope.cat.exCharacteristics.length > 0 && $scope.Characteristics.length > 0) {
                //    var commonKeys = Object.keys($scope.cat.exCharacteristics[0]).filter(key => $scope.Characteristics[0].hasOwnProperty(key));

                //    if (commonKeys.length > 0) {
                //        mergedList = $scope.Characteristics.map(item => {
                //            var newItem = {};
                //            commonKeys.forEach(key => {
                //                newItem[key] = $scope.cat.exCharacteristics[0][key];
                //            });
                //            Object.keys(item).forEach(key => {
                //                if (!commonKeys.includes(key)) {
                //                    newItem[key] = item[key];
                //                }
                //            });
                //            return newItem;
                //        });
                //    }
                //} else {
                //    // Handle the case where either dumblist or list is empty
                //}

                //console.log(mergedList);


                //$scope.Characteristics = $scope.lstCharateristics1;
                console.log($scope.Characteristics);
            } else {
                $scope.Res = "Noun and Modifier don't match";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }

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
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4) + ( autocompleteResult.Acquiredby == null ? '' :' (Acquired by '+ autocompleteResult.AcquiredCompanyName+')'),
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
                                    label: autocompleteResult.Name + (autocompleteResult.Name2 = autocompleteResult.Name2 == null ? '' : autocompleteResult.Name2) + (autocompleteResult.Name3 = autocompleteResult.Name3 == null ? '' : autocompleteResult.Name3) + (autocompleteResult.Name4 = autocompleteResult.Name4 == null ? '' : autocompleteResult.Name4) + ( autocompleteResult.Acquiredby == null ? '' : ' (Acquired by ' + autocompleteResult.AcquiredCompanyName + ')'),
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