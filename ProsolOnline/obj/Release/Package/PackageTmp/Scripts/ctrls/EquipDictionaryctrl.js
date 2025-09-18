
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['angular.filter']);

    app.controller('DictionaryViewController', ['$scope', '$http', '$window', '$timeout', function ($scope, $http, $window, $timeout) {

        $http({
            method: 'GET',
            url: '/Equipment/GetEqu_Noun'
        }).success(function (response) {

            $scope.Nouns = response;

        }).error(function (data, status, headers, config) {
            alert("error");

        });

        $scope.SelectNoun = function () {
     
           
            $scope.modifierDef = null;
            $scope.modifier = null;
            $scope.NounEqu = null;
            $scope.sparelist = null;

            if ($scope.lst.Noun.toString().indexOf(',') == -1) {

                $scope.noun = $scope.lst.Noun;

                //var NDef = $.grep($scope.Nouns, function (lst) {
                //    return lst.Noun == $scope.lst.Noun;
                //})[0].NounDefinition;

               // $scope.nounDef = NDef;
            }

            $http({
                method: 'GET',
                url: '/Equipment/GetEqu_Modifier',
                params: { Noun: $scope.noun }
                //?Noun='+$scope.noun
            }).success(function (response) {

                $scope.Modifiers = response;
              //  $scope.NounSynonyms = response[0].NounEqu;
             
            }).error(function (data, status, headers, config) {
               // alert("error");

            });
            $scope.NM.FileData = null;
            $scope.rows = null;
        };
        $scope.sparelist = null;
        $scope.SelectModifier = function () {

            $scope.charaterDef = null;
            $scope.charater = null;
            $scope.sparelist = null;
            if ($scope.lst.Modifier.toString().indexOf(',') == -1) {

              //  $scope.modifier = $scope.lst.Modifier;

                var NDef = $.grep($scope.Modifiers, function (lst) {
                    return lst.Modifier == $scope.lst.Modifier;
                })[0].ModifierDefinition;

                $scope.modifierDef = NDef;
            }
            $http({
                method: 'GET',
                url: '/Equipment/GetEquipsparelist',
                params: { Noun: $scope.noun, Modifier: $scope.lst.Modifier }
            }).success(function (response) {
                $scope.sparelist = response;
              
            }).error(function (data, status, headers, config) {
               
                alert("error");

            });
            $http({
                method: 'GET',
                url: '/Dictionary/GetNounModifier',
                params:{Noun: $scope.noun , Modifier: $scope.lst.Modifier}
                //?Noun=' + $scope.noun + '&Modifier=' + $scope.lst.Modifier

            }).success(function (response) {
               //  alert(JSON.stringify(response.ALL_NM_Attributes));
                $scope.NM = response.One_NounModifier;
                $scope.rows = response.ALL_NM_Attributes;

                $scope.nounDef = response.One_NounModifier.Nounabv;

              //  alert(angular.toJson($scope.NM))
            }).error(function (data, status, headers, config) {
                alert("error");

            });
            

        };

        $scope.SelectCharater = function () {

            if ($scope.lst.Characteristic.toString().indexOf(',') == -1) {

              //  $scope.charater = $scope.lst.Characteristic;
                var NDef = $.grep($scope.rows, function (lst) {
                    return lst.Characteristic == $scope.lst.Characteristic;
                })[0].Definition;

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

        $scope.DonloadNM = function () {
            $window.open('/Dictionary/Download');           

        };
        $scope.resdwn = false;
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {

            $scope.NotifiyRes = false;
            $scope.$apply();
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {
                    angular.element("input[type='file']").val(null);
                    files[0] = null;

                    $scope.Res = "Load valid excel file";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.$apply();
                }
            }
        };

        $scope.BulkDictionary = function () {
          
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);
              
                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $http({
                    url: "/Equipment/BulkDictionary",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    //  alert(data);
                    $scope.ShowHide = false;
                    if (data === 0)
                        $scope.Res = "Records already exists"
                    else $scope.Res = data + " Records uploaded successfully"


                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.files = [];
        $scope.GetLD = "";
        $scope.load = true;
        $scope.scrub = true;
        $scope.imp = true;



        $scope.filteredassign = [],
   $scope.currentPage = 1
, $scope.numPerPage = 10
, $scope.maxSize = 5;

        $scope.ddlItems = function () {
            $scope.chkSelected = false;
            $scope.numPerPage = $scope.selecteditem;
            $scope.slcteditem = [];
        };
    }]);   

})();