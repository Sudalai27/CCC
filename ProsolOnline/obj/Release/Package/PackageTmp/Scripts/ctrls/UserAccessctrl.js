
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables', 'cgBusy']);
 
    app.controller('homeController', ['$scope', '$http','$timeout', function ($scope, $http, $timeout, DTOptionsBuilder) {
        
        $scope.cgBusyPromises = $http.get('/User/Getpages').success(function (data) {
            $scope.gett = data            
            var uniqueMainmenu = [];
            var lst1 = [];
            var lst2 = [];
            for (var i = 0; i < data.length; i++) {
                if (uniqueMainmenu.indexOf(data[i].Mainmenu) === -1) {
                        uniqueMainmenu.push(data[i].Mainmenu);
                }
            }
            angular.forEach(uniqueMainmenu, function (lst) {
             
                if (lst == "Service Master" || lst == "Asset Master" || lst == "Bill Of Material" || lst == "User Module" || lst=="Business Partner") {
                    lst1.push(lst);
                }
                else {
                    lst2.push(lst);
                }
            })
            $scope.gett1 = lst1;
            $scope.gett2 = lst2;
          
        });//to get pages

        $scope.bindPlant = function () {
            $http.get('/User/getplant').success(function (response) {           
                $scope.PlantList = response
                $scope.Plant = response[0].Plantcode;
                $scope.selection1 =[];
                $scope.selection1.push(response[0].Plantcode);
               
                $http.get('/User/getuser', { params: { plants: angular.toJson($scope.selection1) } }).success(function (response) {
                    $scope.listItms = [];
                    $scope.getuser = response
                });

            });
        }
        $scope.bindPlant();
        $scope.changePlant=function(){
            $scope.selection1 = [];
            $scope.selection1.push($scope.Plant);
            $http.get('/User/getuser', { params: { plants: angular.toJson($scope.selection1) } }).success(function (response) {
                $scope.listItms = [];
                $scope.getuser = response
            });
        }

        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.shw = function () {
            $http.get('/User/Getpages').success(function (data) {
                $scope.gett = data
            });//to get pages
        }

       
        $scope.reset = function () {

            $scope.form4.$setPristine();
        }
        $scope.add = function () {
            var i;
            angular.forEach($scope.gett, function (lst) {
                if (lst.Status == true) {
                    lst.Status = "1";
                } if (lst.Status == false) {
                    lst.Status = null;

                }
            })

            angular.forEach($scope.gett, function (lst) {
                if (i != "1") {
                    if (lst.Status == "1") {
                        i = "1";
                    }
                    else {
                        i = "0";
                    }
                }
            })
            if ($scope.UserName1 != undefined && $scope.UserName1 != '') {
                $timeout(function () {
                    $('#divNotifiy').attr('style', 'display: none');
                }, 30000);
                $http.get('/User/AutoCompleteUSN', { params: { term: $scope.UserName1 }}).success(function (response) {
                    if (response != '') {
                        $scope.view = false;
                        if ($scope.UserName1 != undefined && $scope.UserName1 != '' && i === "1") {
                            

                            $scope.cgBusyPromises = $http({
                                method: 'POST',
                                url: '/User/submit',
                                params :{term :$scope.UserName1},
                                data: $scope.gett
                            }).success(function (data, status, headers, config) {
                                $scope.Res = "User permission has been updated"
                                $scope.Notify = "alert-info";
                                $scope.NotifiyRes = true;
                                $scope.UserName1 = ""
                                $scope.shw();
                                $scope.view = true;
                                $('#divNotifiy').attr('style', 'display: block');
                                $scope.reset();
                            }).error(function (data, status, headers, config) {
                               
                            });


                        } else {
                            if ($scope.UserName1 === "") {
                                $scope.Res = "Select user";
                                $scope.Notify = "alert-danger";
                                $scope.NotifiyRes = true;
                                $('#divNotifiy').attr('style', 'display: block');
                            } else if (i === "0") {
                                $scope.Res = "Select pages";
                                $scope.Notify = "alert-danger";
                                $scope.NotifiyRes = true;
                                $scope.view = true;
                                $('#divNotifiy').attr('style', 'display: block');
                            } else {

                            }
                        }
                    }
                    else {
                        $scope.Res = "Invalid user"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }//
                });
            } else {
                $scope.Res = "Select user";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }

        /*To load accesspermissions based on user of access page*/
        $scope.keyup = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
         
                if ($scope.UserName1 != '' && $scope.UserName1 != null && $scope.UserName1 != "" && $scope.UserName1 != undefined) {
                $http.get('/User/AutoCompleteUSN?term=' + $scope.UserName1).success(function (response) {
                    if (response.length > 0) {
                        $http({
                            method: 'GET',
                            url: '/User/search',
                            params :{term: $scope.UserName1}
                        }).success(function (response) {
                           
                            if (response.length > 0) {
                                $scope.gett = response
                                angular.forEach($scope.gett, function (lst) {
                                    if (lst.Status == "1") {
                                        lst.Status = true;

                                    }
                                });
                                $scope.view = true;
                            } else {
                                $scope.shw();
                                $scope.Res = "Permission has been assign to " + $scope.UserName1;
                                $scope.Notify = "alert-danger";
                                $scope.NotifiyRes = true;
                                $scope.view = true;
                                $('#divNotifiy').attr('style', 'display: block');
                            }

                        }).error(function (data, status, headers, config) {

                        });// to get the access of user
                    }
                    else {
                        $scope.Res = "Invalid user"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $scope.shw();
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                });

                } else {
                    $scope.shw();
                }
           
        }

            
            

           
       
        /*not using anywhere*/
        $scope.search = function () {
            if ($scope.selectuser.Userid === undefined || $scope.selectuser.Userid !=="") {
                $scope.shw();
            } else {
                $http({
                    method: 'GET',
                    url: '/User/search',
                    params :{id: $scope.selectuser.Userid}
                }).success(function (data, status, headers, config) {
                    if (data === null) {
                        $scope.shw();
                    } else {
                        $scope.gett = data
                    }

                }).error(function (data, status, headers, config) {
                   
                });// to get the access of user
            }
        }
    }]);

    //app.factory("AutoCompleteService", ["$http", function ($http) {

    //    return {
    //        search: function (term) {

    //            return $http.get("AutoCompleteUSN?term=" + term).then(function (response) {
    //                return response.data;

    //            });
    //        }
    //    };
    //}]);
    //app.directive("autoComplete", ["AutoCompleteService", function (AutoCompleteService) {
    //    return {
    //        restrict: "A",
    //        link: function (scope, elem, attr, ctrl) {
    //            elem.autocomplete({
    //                source: function (searchTerm, response) {

    //                    AutoCompleteService.search(searchTerm.term).then(function (autocompleteResults) {
    //                        response($.map(autocompleteResults, function (autocompleteResult) {
    //                            return {
    //                                label: autocompleteResult.UserName,
    //                                value: autocompleteResult.UserName,
    //                            }
    //                        }))
    //                    });
    //                },
    //                minLength: 1,
    //                select: function (event, selectedItem, http) {
                       
    //                    scope.UserName1 = selectedItem.item.value;
    //                    scope.$apply();
    //                    event.preventDefault()
    //                    $.ajax({
    //                        url: '/User/search?term=' + selectedItem.item.value,
    //                        type: 'GET',
    //                        success: function (response) {                               

    //                            scope.gett = response;
    //                            var uniqueMainmenu = [];
    //                            for (var i = 0; i < response.length; i++) {
    //                                if (uniqueMainmenu.indexOf(response[i].Mainmenu) === -1) {
    //                                    uniqueMainmenu.push(response[i].Mainmenu);
    //                                }
    //                            }
    //                            scope.gett1 = uniqueMainmenu;
                              
    //                            angular.forEach(scope.gett, function (lst) {
    //                                if (lst.Status == "1") {                                       
    //                                    lst.Status = true;
                                        
    //                                }
    //                            })                              

    //                            scope.$apply();
    //                            event.preventDefault();
    //                        },
    //                        error: function (xhr, ajaxOptions, thrownError) {
    //                            $scope.Res = thrownError;

    //                        }
    //                    });


    //                }
    //            });

    //        }

    //    };

    //}]);


})();