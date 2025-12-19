(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap']);
    //app.filter('myFilter', function () {
    //    return function (inputs, filterValues) {
    //        var output = [];
    //        angular.forEach(inputs, function (input) {
    //            if (filterValues.indexOf(input.Value) !== -1)
    //                output.push(input);
    //        });
    //        return output;
    //    };
    //});
    app.controller('NMController', function ($scope, $http, $timeout, $rootScope, $filter) {
        // $scope.NM = [];
        $rootScope.resdwn = false;
        $rootScope.resdwn1 = false;
        $rootScope.resdwn2 = false;
        $rootScope.resdwn3 = false;

        $scope.BindGroupinx = function (inx) {
            // $scope.showlb = $scope.showlb;
            if ($scope.valueSrch != null && $scope.valueSrch != "" && $scope.valueSrch != undefined) {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetValueListforcreate_tempsearch',
                    //?currentPage=' + inx + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Noun=' + $scope.NM.Noun + '&Modifier=' + $scope.NM.Modifier + '&srchtxt=' + $scope.valueSrch
                    params: { currentPage: inx, maxRows: 25, Name: $scope.showlb, Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier, srchtxt: $scope.valueSrch }
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
                                url: '/Dictionary/GetAttributesDetail',
                                params: { Name: $scope.showlb }
                                //?Name=' + $scope.showlb
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
                    url: '/Dictionary/GetValueListforcreate_temp',
                    //?currentPage=' + inx + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Noun=' + $scope.NM.Noun + '&Modifier=' + $scope.NM.Modifier
                    params: { currentPage: inx, maxRows: 25, Name: $scope.showlb, Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier }
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
                                url: '/Dictionary/GetAttributesDetail',
                                params: { Name: $scope.showlb }
                                //?Name=' + $scope.showlb
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
        $http({
            method: 'GET',
            url: '/User/roles'
        }).success(function (response) {
            $scope.role = response;

        });
        // uom

        $scope.Binduom = function () {
            $http({
                method: 'GET',
                url: '/GeneralSettings/getlistuom'
            }).success(function (response) {

                if (response != '') {

                    $scope.UOMs = response;
                }
                else $scope.UOMs = null;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
        $scope.Binduom();
        var uomlist = [];
        $scope.checkUom = function (id, indx) {

            if ($('#chku1' + indx).is(':checked')) {

                uomlist.push(id);

            }
            else {

                var index = uomlist.indexOf(id);
                uomlist.splice(index, 1);
            }

        };

        $scope.BindGroupinxsearch = function () {
            // alert(
            //  "in");
            $http({
                method: 'GET',
                url: '/Dictionary/GetValueListforcreate_tempsearch',
                //?currentPage=' + 1 + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Noun=' + $scope.NM.Noun + '&Modifier=' + $scope.NM.Modifier + '&srchtxt=' + $scope.valueSrch
                params: { currentPage: 1, maxRows: 25, Name: $scope.showlb, Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier, srchtxt: $scope.valueSrch }
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
                            url: '/Dictionary/GetAttributesDetail',
                            params: { Name: $scope.showlb }
                            //?Name=' + $scope.showlb

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



        $scope.selectValue = [];
        $scope.checkValue = function (id, indx) {
            // alert(angular.toJson(id));


            if ($('#chk' + id).is(':checked')) {

                $scope.selectValue.push(id);
                $scope.ValueList
            }
            else {
                var index = $scope.selectValue.indexOf(id);
                $scope.selectValue.splice(index, 1);


            }
            //  alert(angular.toJson($scope.selectValue));

        };


        $scope.ShowValue = function (Name, Noun, Modifier) {
            
            $scope.showlb = Name;
            $http({
                method: 'GET',
                url: '/Dictionary/GetValueListforcreate_temp',
                params: { currentPage: 1, maxRows: 25, Name: $scope.showlb, Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier }
                //?currentPage=' + 1 + '&maxRows=' + 25 + '&Name=' + $scope.showlb + '&Noun=' + $scope.NM.Noun + '&Modifier=' + $scope.NM.Modifier
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
                            url: '/Dictionary/GetAttributesDetail',
                            params: { Name: Name }
                            //?Name=' + Name

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
                                    $scope.ValueList = $filter('orderBy')($scope.ValueList, 'Checked');
                                   // $scope.ValueList = $filter('orderBy')($scope.ValueList, 'Checked')
                                   // $scope.ValueList = $filter('orderBy')($scope.ValueList, 'Checked');
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
                            url: '/Dictionary/GetAttributesDetailfromcharacteristicvalues',
                            params: { Name: Name, Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier }
                            //?Name=' + Name + '&Noun=' + $scope.NM.Noun + '&Modifier=' + $scope.NM.Modifier
                        }).success(function (response) {
                            // alert(angular.toJson(response));
                            if (response._id != null) {
                                if (response.ValueList != null) {
                                    $scope.selectValue = response.ValueList;
                                    angular.forEach($scope.ValueList, function (lst) {
                                        if (response.ValueList.indexOf(lst._id) !== -1) {
                                            lst.Checked = 1;
                                            $('#chk' + lst._id).prop('disabled', false);
                                        } else {
                                            lst.Checked = 0;
                                            $('#chk' + lst._id).prop('disabled', false);
                                        }
                                    });
                                    //$scope.ValueList.sort(function (a, b) {
                                    //    if (a.Checked ==  b.Checked) return 1;
                                    //    if (b.Checked == a.Checked) return -1;
                                    //    return 0;
                                    //});
                               
                                }
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    }
                }
            }).error(function (data, status, headers, config) {
            });




        };

        $scope.bindAttriUOM = function () {

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUOMList'
            }).success(function (response) {
                $scope.UOMs = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
             
        }
        $scope.bindAttriUOM();
        $scope.ShowUOM = function (Name) {
          
            $scope.showlb1 = Name;
            $http({
                method: 'GET',
                url: '/Dictionary/GetNMUOMList',
                params: { Noun: $scope.NM.Noun, Modifier: $scope.NM.Modifier, Attribute: Name }

            }).success(function (response) {

                if (response != null) {
                    $('#divUOMAdd').attr('style', 'display: block');                   
                    $scope.CustUOM = response;


                }
            }).error(function (data, status, headers, config) {
            });
        };


        $scope.changeSeq = function () {
            console.log($scope.rows);
            $scope.rows.sort(function (a, b) {
                return parseInt(a.Squence) - parseInt(b.Squence);
            });
            console.log($scope.rows);
        };

        $scope.toNumber = function (item) {
            return parseInt(item.Squence) || 0;
        };


        $scope.saveData = function () {

            var obj = $.grep($scope.rows, function (lst) {
                return lst.Characteristic == $scope.showlb;
            })[0];
            var inx = $scope.rows.indexOf(obj)
            $scope.rows[inx].Values = $scope.selectValue;
            $('#divValAdd').attr('style', 'display: none');

        };
        $scope.saveData1 = function () {

            var obj = $.grep($scope.rows, function (lst) {
                return lst.Characteristic == $scope.showlb1;
            })[0];
            var inx = $scope.rows.indexOf(obj)
            var templist = [];
            angular.forEach($scope.CustUOM, function (lst) {
                if (lst.UOMname == '1') {
                    templist.push(lst.Unitname);
                   
                }
            });
            $scope.rows[inx].Uom = templist;
          
            $('#divUOMAdd').attr('style', 'display: none');

        };
        $scope.closeData = function () {

            $('#divValAdd').attr('style', 'display: none');

        };
        $scope.closeData1 = function () {

            $('#divUOMAdd').attr('style', 'display: none');

        };

        
        $scope.BindAttributesList = function () {
            $http({
                method: 'GET',
                url: '/Dictionary/GetAttributesList'
            }).success(function (response) {
                $scope.AttributesList = response;
                //  alert(angular.toJson($scope.AttributesList))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindAttributesList();
        //$scope.getchar = function () {
        //    $http({
        //        method: 'GET',
        //        url: '/GeneralSettings/GetAttributesList'
        //    }).success(function (response) {
        //        $scope.AttributesList= response

        //    }).error(function (data, status, headers, config) {

        //    });
        //};
        $scope.files = [];
        $scope.removeindexes = [];

        $scope.LoadFileData = function (files) {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

            $scope.files = files;
            var reader = new FileReader();
            if (files[0] != null) {

                var flg = 0;
                var ext = files[0].name.match(/\.(.+)$/)[1];

                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg' || angular.lowercase(ext) === 'png') {
                    flg = 1;

                }
                if (flg == 1) {


                    reader.onload = function (event) {
                        $scope.image_source = event.target.result
                        $scope.$apply()

                    }
                    if (files[0] == null) {

                        $scope.image_source = null;
                        $scope.$apply()
                    } else {
                        reader.readAsDataURL(files[0]);
                    }

                }
                else {

                    $rootScope.Res = "Please upload valid image file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    angular.element("input[type='file']").val(null);
                    $scope.image_source = null;
                    $scope.$apply()
                    files[0] = null;
                }
            } else {
                $scope.image_source = null;
                $scope.$apply()

            }

        };
        $scope.KeyClear = function () {

            $scope.uomlist = [];
            $scope.Binduom();
            if ($scope.NM.Noun === undefined) {
                $scope.NM.Nounabv = "";
                $scope.NM.NounDefinition = "";
            }


        };
        $scope.KeyClear1 = function () {
            $scope.uomlist = [];
            $scope.Binduom();
            if ($scope.NM.Modifier === undefined) {
                $scope.NM.Nounabv = "";
            }
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.Listmodifer = function () {
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: $scope.NM.Noun }
                //?Noun='+$scope.noun
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.GetCharc = function (Noun, Modifier) {
            $scope.NM.Modifier = Modifier.toUpperCase();
            if (Modifier != undefined && Modifier != "") {
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params: { Noun: Noun, Modifier: Modifier }
                    //?Noun='+$scope.noun
                }).success(function (response) {
                   
                    if (response == "") {
                        $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                    }
                    else {
                        $scope.NM = response.One_NounModifier;
                    
                        if (response.ALL_NM_Attributes.length > 0) {
                            $scope.rows = response.ALL_NM_Attributes;
                            console.log("Before:", $scope.rows)
                            $scope.rows = $scope.rows.filter(function (value, index, self) {
                                return value.Definition === 'MM';
                            }).sort();
                            console.log("After:", $scope.rows)
                        }
                        else {
                            $scope.NM.NounDefinition = "";
                            $scope.NM.Modifierabv = "";
                            $scope.NM.ModifierDefinition = "";
                            $scope.rows = [{ 'Squence': 10, 'ShortSquence': 10, 'Remove': 0 }];
                        }
                        if (response.One_NounModifier.uomlist.length > 0) {
                            var uomlist = response.One_NounModifier.uomlist;
                            
                            if (uomlist != null) {
                                angular.forEach($scope.UOMs, function (lst) {

                                    if (uomlist.indexOf(lst._id) !== -1) {
                                        lst.Checked = '1';
                                    } else {
                                        lst.Checked = '0';
                                    }
                                });
                            }
                        }
                    }
                });
            }
            else {
                $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
            }
        }
        $scope.changeCheck = function () {
            var characterList = [];
            $scope.charcheck = false;
            angular.forEach($scope.rows, function (lst) {

                if (characterList.indexOf(lst.Characteristic) !== -1) {
                    $scope.charcheck = true;
                }
                characterList.push(lst.Characteristic);


            });
        };
        $scope.createNM = function () {

            $scope.NM.uomlist = uomlist
            var arrList_Long = [];
            var arrList_Short = [];
            $scope.Seqcheck1 = false;
            $scope.Seqcheck2 = false;

            var characterList = [];
            $scope.charcheck = false;
            console.log(angular.toJson($scope.rows));

            angular.forEach($scope.rows, function (lst) {

                if (characterList.indexOf(lst.Characteristic) !== -1) {
                    $scope.charcheck = true;
                }
                characterList.push(lst.Characteristic);


            });

            angular.forEach($scope.rows, function (lst) {

                if (arrList_Long.indexOf(parseInt(lst.Squence)) !== -1 || parseInt(lst.Squence) < 1) {
                    $scope.Seqcheck1 = true;
                }
                arrList_Long.push(parseInt(lst.Squence));


            });
            //  alert(angular.toJson($scope.Seqcheck1));

            //for (var i = 1; i <= arrList_Long.length; i++) {
            //    if (arrList_Long.indexOf(i) == -1 ) {
            //        $scope.Seqcheck1 = true;
            //    }
            //}

            angular.forEach($scope.rows, function (lst) {

                if (arrList_Short.indexOf(parseInt(lst.ShortSquence)) !== -1 || parseInt(lst.ShortSquence) < 1) {
                    $scope.Seqcheck2 = true;
                }
                arrList_Short.push(parseInt(lst.ShortSquence));

            });
            //for (var i = 1; i <= arrList_Short.length; i++) {
            //    if (arrList_Short.indexOf(i) == -1) {
            //        $scope.Seqcheck2 = true;
            //    }
            //}





            //if (!$scope.form.$invalid) {               
            if ($scope.Seqcheck2 == false && $scope.charcheck == false) {

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append("NM", angular.toJson($scope.NM));
                formData.append('image', $scope.files[0]);
                formData.append('CHA', angular.toJson($scope.rows));

                $http({
                    url: "/Dictionary/addNM",
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
                        $scope.uomlist = [];
                        $scope.Binduom();
                        $scope.reset();
                        $scope.NM = null;
                        $scope.rows = null;
                        uomlist = [];
                        $scope.image_source = null;
                        $scope.rows = [{ Squence: 1, ShortSquence: 1, 'Remove': 0 }];

                        //angular.element("input[type='file']").val(null);

                        $rootScope.Res = "Dictionary created successfully"
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('.fileinput').fileinput('clear');
                    }


                }).error(function (data, status, headers, config) {

                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                });
            }
            // }
        };


        $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
        $scope.addRow = function () {
            //var flg = 0;
            //angular.forEach($scope.rows, function (value, key) {               
            //    if (value.Characteristic == null) {                  
            //        flg = 1;
            //    }
            //});
            //if (flg == 0) {
            var characterList = [];
            $scope.charcheck = false;
            angular.forEach($scope.rows, function (lst) {
                //alert(angular.toJson(lst.Characteristic));
                if (lst.Definition === 'MM') {
                    if (characterList.indexOf(lst.Characteristic) !== -1) {
                        $scope.charcheck = true;
                    }
                    characterList.push(lst.Characteristic);
                }
            });
            if (!$scope.charcheck) {
               
                console.log($scope.rows);
                var seq = ($scope.rows.length + 1).toString() + "0";
                console.log("Adding new row with sequence:", seq);

                $scope.rows.push({
                    'Squence': parseInt(seq, 10),
                    'ShortSquence': parseInt(seq, 10),
                    'Remove': 0,
                    'Definition':'MM'
                });

                console.log("New row added. Updated rows:", $scope.rows);
            } else {
                console.log("Duplicate characteristic found. No new row added.");
            }


        };
        $scope.RemoveRow = function () {
            // Create an array to hold rows that should not be removed
            var filteredRows = [];

            // Iterate through rows and filter out the ones with Remove = 1
            angular.forEach($scope.rows, function (value, key) {
                if (value.Remove === 0) {
                    filteredRows.push(value); // Keep the row if Remove is 0
                }
            });

            // Now, we will reset the sequence numbers for the remaining rows
            var cunt = 10; // Start the sequence count from 10

            // Update sequence for each row in the filtered list
            angular.forEach(filteredRows, function (value, key) {
                value.Squence = cunt;
                value.ShortSquence = cunt; // Assuming ShortSquence should also follow the same pattern
                cunt += 10; // Increment by 10 for each row
                value.Remove = 0; // Ensure Remove flag is reset to 0
            });

            // Update the rows array to contain only the filtered rows
            $scope.rows = filteredRows;

            // Log the updated rows to console for debugging
            console.log("Rows after removal and re-sequencing:", $scope.rows);
        };

        //$scope.RemoveRow = function () {

        //    $scope.ar = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
        //    angular.forEach($scope.rows, function (value, key) {
        //        if (value.Remove == 0) {
        //            $scope.ar.push(value);
        //        }
        //    });

        //    $scope.ar.splice(0, 1);
        //    var cunt = 1;
        //    angular.forEach($scope.ar, function (value, key) {
        //        value.Squence = cunt;
        //        value.ShortSquence = cunt++;
        //        value.Remove = 0;
        //    });

        //    $scope.rows = $scope.ar;
        //    // alert(angular.toJson($scope.rows));
        //};

        $scope.identifyremove_rows = function (index) {
            if ($scope.rows[index].Remove == 0) {
                $scope.rows[index].Remove = 1;
            } else {
                $scope.rows[index].Remove = 0;
            }

            //   alert(angular.toJson($scope.rows));

        };

    });


    app.controller('CODELOGIC', function ($scope, $http, $rootScope, $timeout) {

        $http({
            method: 'GET',
            url: '/Dictionary/loadversion'
        }).success(function (response) {
            $scope.Versionddl = response;

        }).error(function (data, status, headers, config) {
        });


        $http({
            method: 'GET',
            url: '/Dictionary/Showdata'
        }).success(function (response) {
            if (response.CODELOGIC == "UNSPSC Code") {
                $scope.obj = { "CODELOGIC": "UNSPSC Code", "Version": response.Version };
                $scope.showunspscversion();
            }
            else if (response.CODELOGIC == "Customized Code") {
                $scope.obj = { "CODELOGIC": "Customized Code", "Version": response.Version };
                $scope.hideversion();
            }
            else {
                $scope.obj = { "CODELOGIC": "Item Code", "Version": response.Version };
                $scope.hideversion();
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

        $scope.createcodelogic = function () {

         //   $timeout(function () { $('#divNotifiy').attr('style', 'display: none'); }, 3000);
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            var formData = new FormData();
            formData.append("obj", angular.toJson($scope.obj));
            //  alert(angular.toJson($scope.obj));
            $http({
                method: "POST",
                url: "/Dictionary/CodeSave",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                $rootScope.Res = "CodeLogic has been changed"
                $rootScope.Notify = "alert-info";
                $rootScope.NotifiyRes = true;
               // $('#divNotifiy').attr('style', 'display: block');
            }).error(function (data, status, headers, config) {
            });
        };
    });





    app.controller('BulkNMController', function ($scope, $http, $timeout, $rootScope) {


        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
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

        $scope.BulkNM = function () {

            if ($scope.files[0] == null) {
                $rootScope.Res = "Please Select File First"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Dictionary/NM_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0) {
                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }
                    else if (data === -1) {
                        $rootScope.Res = "upload valid excel file";
                        $rootScope.Notify = "alert-danger";
                    }

                    else {
                        $rootScope.Notify = "alert-info";
                        $rootScope.Res = data + " Records uploaded successfully"
                    }



                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

    });


    app.controller('BulkCharacteristicController', function ($scope, $http, $timeout, $rootScope) {



        $scope.ShowHide1 = false;

        $scope.files = [];

        $scope.LoadFileData = function (files) {

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

        $scope.BulkCharacteristic = function () {
            if ($scope.files[0] == null) {
                $rootScope.Res = "Please Select File First"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }

            if ($scope.files[0] != null) {

                $scope.ShowHide1 = true;

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Dictionary/CH_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $scope.ShowHide1 = false;
                    if (data === 0) {

                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }

                    else if (data === -1) {
                        $rootScope.Res = "Please upload Noun and Modifier"
                        $rootScope.Notify = "alert-danger";
                    }

                    else if (data === -2) {
                        $rootScope.Res = "upload valid excel file"
                        $rootScope.Notify = "alert-danger";
                    }

                    else {
                        $rootScope.Notify = "alert-info";
                        $rootScope.Res = data + " Records uploaded successfully"
                    }


                    $rootScope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide1 = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
    });
    app.controller('BulkValueController', function ($scope, $http, $timeout, $rootScope) {
        $scope.ShowHide2 = false;

        $scope.files = [];

        $scope.LoadFileData1 = function (files) {

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

        $scope.BulkValue = function () {

            if ($scope.files[0] == null) {
                $rootScope.Res = "Please Select File First"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }
            if ($scope.files[0] != null) {

                $scope.ShowHide2 = true;

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Dictionary/Value_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $scope.ShowHide2 = false;
                    if (data === 0) {
                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }
                    else if (data === -1) {
                        $rootScope.Res = "Upload valid excel file"
                        $rootScope.Notify = "alert-danger";
                    }
                    else {
                        $rootScope.Res = data + " Records uploaded successfully"
                        $rootScope.Notify = "alert-info";

                    }
                    $rootScope.NotifiyRes = true;

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
    app.controller('BulkuomController', function ($scope, $http, $timeout, $rootScope) {
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
                    $scope.$rootScope = "Load valid excel file";
                    $scope.$rootScope = "alert-danger";
                    $scope.$rootScope = true;

                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.$apply();
                }
            }
        };

        $scope.BulkUom = function () {

            if ($scope.files[0] == null) {
                $rootScope.Res = "Please Select File First"
                $rootScope.Notify = "alert-danger";
                $rootScope.NotifiyRes = true;
            }
            if ($scope.files[0] != null) {

                $scope.ShowHide2 = true;

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.cgBusyPromises = $http({
                    url: "/Dictionary/Uom_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $scope.ShowHide2 = false;
                    if (data === 0) {
                        $rootScope.Res = "Records already exists"
                        $rootScope.Notify = "alert-danger";
                    }
                    else if (data === -1) {
                        $rootScope.Res = "Upload valid excel file"
                        $rootScope.Notify = "alert-danger";
                    }
                    else {
                        $rootScope.Res = data + " Records uploaded successfully"
                        $rootScope.Notify = "alert-info";

                    }
                    $rootScope.NotifiyRes = true;

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

    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteNoun",
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
                        scope.NM.Noun = selectedItem.item.value;


                        $.get("/Dictionary/GetNounDetail", { Noun: scope.NM.Noun }).success(function (response) {

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

    app.factory("AutoCompleteService1", ["$http", function ($http) {
        return {
            search: function (Noun, term) {

                return $http({
                    url: "/Dictionary/AutoCompleteModifier",
                    params: { term: term, Noun: Noun },

                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });


                // return $http.get("AutoCompleteModifier?term=" + term + "&Noun=" + Noun).then(function (response) {
                //  return response.data;
                // });
            }
        };
    }]);
    app.directive("autoComplete1", ["AutoCompleteService1", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(scope.NM.Noun, searchTerm.term).success(function (autocompleteResults) {
                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Modifier,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem) {
                        scope.NM.Modifier = selectedItem.item.value;

                        $.get("/Dictionary/GetNounModifier", { Noun: scope.NM.Noun, Modifier: scope.NM.Modifier }).success(function (response) {
                            scope.NM = response.One_NounModifier;
                            var uomlist = response.One_NounModifier.uomlist;

                            if (uomlist != null) {
                                angular.forEach(scope.UOMs, function (lst) {

                                    if (uomlist.indexOf(lst._id) !== -1) {
                                        lst.Checked = '1';
                                    } else {
                                        lst.Checked = '0';
                                    }
                                });
                            }
                            //angular.forEach(scope.UOMs, function (value1, key) {

                            //    angular.forEach(scope.uomlist, function (value2, key) {


                            //        if (value1._id == value2._id)
                            //        {
                            //            alert("1")
                            //            $('#chku' + _id).Checked = true;

                            //        }

                            //    });


                            //});

                            if (response.ALL_NM_Attributes.length > 0) {
                                scope.rows = response.ALL_NM_Attributes;
                                //  alert(angular.toJson(scope.rows))
                                //angular.forEach(scope.rows, function (lst) {

                                //    $.get("/Dictionary/GetAttributesDetail?Name=" + lst.Characteristic).success(function (response) {
                                //        if (response != null) {
                                //            var i = 0;
                                //            if (response.ValueList == null) {

                                //                angular.forEach($scope.ValueList, function (lst) {
                                //                  $scope.selectValue.push(lst._id);

                                //                });
                                //            } 
                                //        }

                                //    })
                                //});

                                //  alert(angular.toJson(scope.rows));
                            }
                            else {
                                scope.NM.NounDefinition = "";
                                scope.NM.Modifierabv = "";
                                scope.NM.ModifierDefinition = "";
                                scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                            }
                            scope.$apply();
                            event.preventDefault();
                        });


                        //$.ajax({
                        //      url: 'GetNounModifier?Noun=' + scope.NM.Noun + '&Modifier=' + scope.NM.Modifier,                           
                        //    type: 'GET',                       
                        //    success: function (response) {
                        //       // alert(JSON.stringify(response.ALL_NM_Attributes));
                        //        scope.NM = response.One_NounModifier;
                        //        scope.rows = response.ALL_NM_Attributes;
                        //        scope.$apply();
                        //        event.preventDefault();
                        //    },
                        //    error: function (xhr, ajaxOptions, thrownError) {
                        //        $scope.Res = thrownError;

                        //    }
                        //});
                    }
                });


            }
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

})();



