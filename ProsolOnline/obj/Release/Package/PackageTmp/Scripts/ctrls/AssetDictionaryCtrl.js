(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy', 'ui.bootstrap', 'angular.filter']);

    app.controller('AssetNMController', function ($scope, $http, $timeout, $rootScope, $filter) {
        $scope.valSearch = "";
        $scope.selectedValue = "";
        $scope.suggestions = [];
        $scope.dropdownVisible = true;
        $scope.clickedIndex = -1;

        $scope.KeyClear = function () {
            $scope.uomlist = [];
            if ($scope.NM.Noun === undefined) {
                $scope.NM.Nounabv = "";
                $scope.NM.NounDefinition = "";
            }
        };
        $scope.KeyClear1 = function () {
            $scope.uomlist = [];
            if ($scope.NM.Modifier === undefined) {
                $scope.NM.Nounabv = "";
            }
        };
        $scope.Listmodifer = function () {
            if (!$scope.NM?.Noun) return;

            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: $scope.NM.Noun }
            }).then(function (response) {
                $scope.Modifiers = response.data;
            }).catch(function (error) {
                console.error("Error fetching modifiers:", error);
            });
        };

        $scope.GetCharc = function (Noun, Modifier) {
            $scope.NM.Modifier = Modifier?.toUpperCase();

            if (!Modifier) {
                $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                return;
            }

            $http({
                method: 'GET',
                url: '/Dictionary/EquGetNounModifier',
                params: { Noun: Noun, Modifier: Modifier }
            }).then(function (response) {
                const data = response.data;
                console.log(data)

                if (!data) {
                    $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                    return;
                }

                $scope.NM = data.One_NounModifier || {};

                if (Array.isArray(data.ALL_NM_Attributes) && data.ALL_NM_Attributes.length > 0) {
                    console.log("Before:", data.ALL_NM_Attributes);
                    $scope.rows = data.ALL_NM_Attributes.filter(item => item.Definition === 'Equ').sort();
                    console.log("After:", $scope.rows);
                } else {
                    $scope.NM.NounDefinition = "";
                    $scope.NM.Modifierabv = "";
                    $scope.NM.ModifierDefinition = "";
                    $scope.rows = [{ 'Squence': 10, 'ShortSquence': 10, 'Remove': 0 }];
                }

                //const uomlist = data.One_NounModifier?.uomlist || [];
                //if (Array.isArray(uomlist) && uomlist.length > 0 && Array.isArray($scope.UOMs)) {
                //    $scope.UOMs.forEach(lst => {
                //        lst.Checked = uomlist.includes(lst._id) ? '1' : '0';
                //    });
                //}
                console.log($scope.rows)
            }).catch(function (error) {
                console.error("Error fetching noun modifier:", error);
            });
        };

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

        $scope.reset = function () {

            $scope.form.$setPristine();
        }


        $scope.createNM = function () {

            //$scope.NM.uomlist = uomlist
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


            //$scope.rows.forEach(function (rw) {
            //    if (rw.Definition == "" || rw.Definition == null || rw.Definition == "null" || rw.Definition == undefined || rw.Definition == "undefined") {
            //        rw.Definition = "Equ";
            //    }
            //})


            //if (!$scope.form.$invalid) {               
            if ($scope.Seqcheck2 == false && $scope.charcheck == false) {

                $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append("NM", angular.toJson($scope.NM));
                //formData.append('image', $scope.files[0]);
                formData.append('CHA', angular.toJson($scope.rows));

                $http({
                    url: "/Dictionary/AssetAddNM",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    console.log(data)
                    if (data !== "Success") {

                        $scope.Res = data.errors;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;

                    }
                    else {

                        $scope.Res = "Dictionary created successfully"
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        //$scope.uomlist = [];
                        //$scope.Binduom();
                        $scope.reset();
                        $scope.NM = null;
                        $scope.rows = null;
                        //uomlist = [];
                        $scope.image_source = null;
                        $scope.rows = [{ Squence: 1, ShortSquence: 1, 'Remove': 0 }];

                        //angular.element("input[type='file']").val(null);
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

        $scope.onCharacteristicChange = function (attr, value) {
            console.log(attr);
            console.log(value);
            $scope.rows.forEach(function (rw) {
                rw[attr] = value;
            }); 
            console.log($scope.rows);
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
                    'Definition': 'MM'
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

        //Values Modal

        $scope.currentValues = [];

        $scope.valueModal = new jBox('Modal', {
            width: 600,
            blockScroll: false,
            animation: 'zoomIn',

            overlay: true,
            closeButton: true,

            content: jQuery('#values-content'),

        });

        $scope.valuesPopup = function (attr) {
            new jBox('Modal', {
                width: 550,
                height: 500,
                blockScroll: true,
                animation: 'zoomIn',
                draggable: 'title',
                closeButton: true,
                content: jQuery('#values-content'),
                //content: $('#simItems').html(),
                title: attr,
                overlay: false,

                reposition: false,
                repositionOnOpen: false,
                position: {
                    x: 'right',
                    y: 'right'
                }
            }).open();
            $scope.load = false;
        }

        $scope.valuesList = [];
        $scope.isAdd = false;

        $scope.ShowValue = function (idx, attr, values) {
            console.log(idx)
            $scope.clickedIndex = idx;
            $scope.valSearch = "";
            $scope.selectedValue = "";
            $scope.suggestions = [];
            if (Array.isArray($scope.rows) && $scope.rows[$scope.clickedIndex]) {
                $scope.currentValues = $scope.rows[$scope.clickedIndex].Values;
            } else {
                console.error("Invalid index or rows not defined", $scope.rows, $scope.clickedIndex);
            }

            //$scope.valueModal.close();
            $scope.valuesPopup($scope.rows[$scope.clickedIndex].Characteristic);

        };

        $scope.onSearch = function (term) {
            if (!term || term.length < 1) {
                $scope.suggestions = [];
                $scope.isAdd = false;
                return;
            }

            if (typeof term !== 'string' || !term.trim()) {
                alert("Invalid value.");
                $scope.isAdd = false;
                return;
            }

            $http.get('/Dictionary/AutoCompleteAssetValues', { params: { term: term } })
                .then(function (res) {
                    $scope.suggestions = res.data;
                    $scope.valuesList = res.data;
                    $scope.dropdownVisible = true;

                    // Move this check here, after the data is available
                    const exists = $scope.valuesList.some(v =>
                        typeof v === 'string' && v.toLowerCase() === term.toLowerCase()
                    );

                    $scope.isAdd = !exists;
                });
        };


        $scope.selectItem = function (item) {
            console.log(item)
            $scope.valSearch = "";
            $scope.valSearch = null;
            $scope.selectedValue = item;
            console.log($scope.selectedValue)
            $scope.suggestions = [];
            $scope.isAdd = false;
        };

        $scope.AddValue = function (Noun, Modifier, Attribute, Value, abb, indx) {

            $http({
                method: 'GET',
                url: '/FAR/AddAssetValue',
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: abb }
            }).success(function (response) {


                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:block');
                $scope.onSearch(Value);

            }).error(function (data, status, headers, config) {

            });
        }

        $scope.addItem = function (item, idx, values) {
            console.log("Value:", item);
            console.log("Index:", idx);
            console.log("Current Values:", $scope.rows[$scope.clickedIndex].Values);
            console.log("All Rows:", $scope.rows);

            if (!Array.isArray($scope.rows[$scope.clickedIndex].Values)) {
                $scope.rows[$scope.clickedIndex].Values = [];
            }

            if (typeof item !== 'string' || !item.trim()) {
                alert("Invalid value.");
                return;
            }

            const exists = $scope.rows[$scope.clickedIndex].Values.some(v => typeof v === 'string' && v.toLowerCase() === item.toLowerCase());

            if (!exists) {
                $scope.selectedValue = item;
                $scope.rows[$scope.clickedIndex].Values.push(item);
                $scope.currentValues = $scope.rows[$scope.clickedIndex].Values;
                //$scope.rows[idx].Values = values;
                //$scope.ShowValue($scope.clickedIndex,$scope.rows[$scope.clickedIndex].Characteristic, $scope.rows[$scope.clickedIndex].Values);
            } else {
                alert("Value already exists.");
            }
        };

        $scope.hideDropdownWithDelay = function () {
            $timeout(function () {
                $scope.dropdownVisible = false;
            }, 200);
        };

        $scope.clearItem = function () {
            $scope.selectedItem = [];
            $scope.selectedValue = "";
        }
    });

    app.controller('DictionaryViewController', ['$scope', '$http', '$window', function ($scope, $http, $window) {

        $http({
            method: 'GET',
            url: '/Dictionary/GetAssetNoun'
        }).success(function (response) {

            $scope.Nouns = response;

        }).error(function (data, status, headers, config) {
            alert("error");

        });
        $scope.Gsearch = "";
        $scope.SelectNoun = function () {


            $scope.modifierDef = null;
            $scope.modifier = null;
            $scope.NounEqu = null;


            if ($scope.lst.Noun.toString().indexOf(',') == -1) {

                $scope.noun = $scope.lst.Noun;
                $scope.Gsearch = $scope.noun;
                var NDef = $.grep($scope.Nouns, function (lst) {
                    return lst.Noun == $scope.lst.Noun;
                })[0].NounDefinition;

                $scope.nounDef = NDef;
            }

            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: $scope.noun }
                //?Noun='+$scope.noun
            }).success(function (response) {

                $scope.Modifiers = response;
                $scope.NounSynonyms = response[0].NounEqu;




            }).error(function (data, status, headers, config) {
                // alert("error");

            });
            //$scope.NM.FileData = null;
            $scope.rows = null;
        };

        $scope.SelectModifier = function () {

            $scope.charaterDef = null;
            $scope.charater = null;

            if ($scope.lst.Modifier.toString().indexOf(',') == -1) {

                //  $scope.modifier = $scope.lst.Modifier;

                var NDef = $.grep($scope.Modifiers, function (lst) {
                    return lst.Modifier == $scope.lst.Modifier;
                })[0].ModifierDefinition;
                if ($scope.lst.Modifier != 'NO MODIFIER' && $scope.lst.Modifier != '--')
                    $scope.Gsearch = $scope.noun + ', ' + $scope.lst.Modifier;
                else $scope.Gsearch = $scope.noun;
                $scope.modifierDef = NDef;
            }

            $http({
                method: 'GET',
                url: '/Dictionary/EquGetNounModifier',
                params: { Noun: $scope.noun, Modifier: $scope.lst.Modifier }


            }).success(function (response) {

                $scope.NM = response.One_NounModifier;
                $scope.rows = response.ALL_NM_Attributes;

                $scope.nounDef = response.One_NounModifier.Nounabv;

                //  alert(angular.toJson($scope.NM))
            }).error(function (data, status, headers, config) {
                alert("error");

            });

            //get Unspsc

            $http({
                method: 'GET',
                url: '/GeneralSettings/GetUnspsc',
                params: { Noun: $scope.noun, Modifier: $scope.lst.Modifier }
            }).success(function (response) {

                if (response != '') {
                    $scope.Commodities = response;
                    if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                        $scope.Unspsc = $scope.Commodities[0].Commodity;
                    else $scope.Unspsc = $scope.Commodities[0].Class;
                }
                else {
                    $scope.Commodities = null;
                }

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

        };

        $scope.SelectCharater = function () {

            if ($scope.lst.Characteristic.toString().indexOf(',') == -1) {

                //  $scope.charater = $scope.lst.Characteristic;
                var NDef = $.grep($scope.rows, function (lst) {
                    return lst.Characteristic == $scope.lst.Characteristic;
                })[0].Definition;
                $scope.Gsearch = $scope.Gsearch + ', ' + $scope.lst.Characteristic;
                $scope.charaterDef = NDef;
            }

            $http({
                method: 'GET',
                url: '/Dictionary/GetValuesList',
                params: { Noun: $scope.noun, Modifier: $scope.lst.Modifier, Characteristic: $scope.lst.Characteristic }
                //?Noun=' + $scope.noun + '&Modifier=' + $scope.lst.Modifier + '&Characteristic=' + $scope.lst.Characteristic

            }).success(function (response) {
                $scope.Values = response;

            }).error(function (data, status, headers, config) {
                alert("error");

            });

        };
        $scope.SelectVal = function () {
            $scope.Gsearch = $scope.Gsearch + ', ' + $scope.val;
        }
        $scope.DonloadNM = function () {
            $window.open('/Dictionary/DownloadAsset');

        };

    }]);



    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteAssetNoun",
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

                        $.get("/Dictionary/EquGetNounModifier", { Noun: scope.NM.Noun, Modifier: scope.NM.Modifier }).success(function (response) {
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
                        var selection = element[0].selectionStart;
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                        element[0].selectionStart = selection;
                        element[0].selectionEnd = selection;
                    }
                    return capitalized;
                }
                modelCtrl.$parsers.push(capitalize);
                capitalize(scope[attrs.ngModel]); 
            }
        };
    });
    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteAssetNoun",
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

    //Values Auto Complete

    //app.factory("AutoCompleteValuesService", ["$http", function ($http) {
    //    return {
    //        search: function (term) {
    //            return $http({
    //                method: "GET",
    //                url: '/Dictionary/AutoCompleteAssetValues',
    //                params: { term: term }
    //            });
    //        }
    //    };
    //}]);

    //app.directive("autoComplete2", ["AutoCompleteValuesService", function (AutoCompleteService) {
    //    return {
    //        restrict: "A",
    //        link: function (scope, elem) {
    //            elem.autocomplete({
    //                source: function (searchTerm, response) {
    //                    AutoCompleteService.search(searchTerm.term).then(function (res) {
    //                        const results = res.data;
    //                        response($.map(results, function (item) {
    //                            return {
    //                                label: item,
    //                                value: item
    //                            };
    //                        }));
    //                    });
    //                },
    //                minLength: 1,
    //                select: function (event, ui) {
    //                    scope.$apply(function () {
    //                        scope.valSearch = ui.item.value;
    //                    });
    //                }
    //            });
    //        }
    //    };
    //}]);

})();