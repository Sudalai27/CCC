var reqcont = angular.module('ProsolApp', ['cgBusy']);

reqcont.controller('valuestandardizationcontroller', function ($scope, $http, $timeout) {


        $scope.NounOptions = [];
        $scope.ModifierOptions = [];
        $scope.AttributesOptions = [];
        $scope.values = [];
        $scope.values1 = [];
        $scope.BtnNounmodel = 'Select Noun  ';
        $scope.BtnModifiermodel = 'Select Modifier  ';
        $scope.BtnAttributesmodel = 'Select Attributes  ';
        $scope.ngdis = false;
        $scope.dis_table = false;
        $scope.serchFilter = "";
        $scope.serchFilter1 = "";
        $scope.serchFilter2 = "";

        // Loading Noun on pageload
        $http({
            method: 'GET',
            url:'/ValueStandardisation/GetNoun'
        }).success(function (result) {
            $scope.NounOptions = result;
        });

        $scope.selectEntity = function (noun) {
            $scope.BtnNounmodel = noun;

            $scope.BtnModifiermodel = 'Select Modifier  ';
            $scope.BtnAttributesmodel = 'Select Attributes  ';
            $scope.AttributesOptions = [];

            $http({
                method: 'GET',
                url: '/ValueStandardisation/GetModifier',
                params:{noun: noun}
            }).success(function (result) {
                $scope.serchFilter = "";
                if (result.length > 0) {
                    $scope.ModifierOptions = result;
                }
            });
        };

        $scope.selectEntity1 = function (noun,modifier) {
            $scope.BtnModifiermodel = modifier;
            $scope.BtnAttributesmodel = 'Select Attributes  ';
           // alert(noun);
            $http({
                method: 'GET',
                url: '/ValueStandardisation/GetAttributes',
                params :{noun: noun ,modifier: modifier}
            }).success(function (result) {
                $scope.serchFilter1 = "";
                if (result.length > 0) {
                    $scope.AttributesOptions = result;
                }
            });
        };

        $scope.selectEntity2 = function (attributes) {
            $scope.BtnAttributesmodel = attributes;
            $scope.serchFilter2 = "";
        };

        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {




            $http({
                method: 'GET',
                url: '/Catalogue/CheckValue',
                params :{Noun: $scope.BtnNounmodel,Modifier:$scope.BtnModifiermodel ,Attribute: $scope.BtnAttributesmodel ,Value:Value}

            }).success(function (response) {


                if (response === "false") {
                    
                    $scope.values[indx].Source = " ";
                    $timeout(function () { $scope.NotifiyRes = false; }, 10000);
                    $scope.Res = "No value found in value master";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }
                else {
                    $scope.values[indx].Source = response;


                }

            }).error(function (data, status, headers, config) {


            });





        };
        $scope.load_values = function () {
            $http({
                method: 'GET',
                url: '/ValueStandardisation/mandatory',
                params: { noun: $scope.BtnNounmodel, modifier: $scope.BtnModifiermodel, attribute: $scope.BtnAttributesmodel }
            }).success(function (result1) {
                $scope.showdelete = result1;
            })
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/ValueStandardisation/load_values',
                params : {noun: $scope.BtnNounmodel, modifier: $scope.BtnModifiermodel , attribute: $scope.BtnAttributesmodel}
            }).success(function (result) {
                if (result.length > 0) {
                    $scope.dis_table = true;
                    $scope.values = result;
                   
                }
                else {
                    $scope.dis_table = false;
                    $timeout(function () { $scope.NotifiyRes = false; }, 10000);
                    $scope.Res = "No Records Found For Your Search";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                }
            })
        };



        $scope.update_values = function (index) {
          //  alert("hai");
            if ($scope.values[index].Valuee === $scope.values[index].Value && $scope.values[index].UOM1 === $scope.values[index].UOM) {
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);
                $scope.Res = "Value must be edited before update";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
            }
            else {
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);
                //$scope.ngdis = true;
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/ValueStandardisation/update_values',
                    params :{noun: $scope.BtnNounmodel ,modifier:$scope.BtnModifiermodel ,attribute:$scope.BtnAttributesmodel,value: $scope.values[index].Valuee ,newvalue: $scope.values[index].Value ,UOM:$scope.values[index].UOM1 ,newUOM: $scope.values[index].UOM}
                    //?noun=' + $scope.BtnNounmodel + '&modifier=' + $scope.BtnModifiermodel + '&attribute=' + $scope.BtnAttributesmodel + '&value=' + $scope.values[index].Valuee + '&newvalue=' + $scope.values[index].Value + '&UOM=' + $scope.values[index].UOM1 + '&newUOM=' + $scope.values[index].UOM
                }).success(function (data) {
                  //  alert(data);
                    if (data < 1) {
                        $scope.Res = "0 items has been updated successfully";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $scope.ngdis = false;
                    }
                    else {
                        $scope.Res = data + " item(s) has been updated successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $scope.ngdis = false;
                   }

                }).error(function () {
                    $scope.Res = "Error occures";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                    $scope.show_loading_multiple = false;
                });
            }
        };
        $scope.Delete_values = function (index) {
            if (confirm("Are you sure, Delete this value?")) {
                $timeout(function () { $scope.NotifiyRes = false; }, 10000);
                $scope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/ValueStandardisation/Delete_values',
                    params: { noun: $scope.BtnNounmodel, modifier: $scope.BtnModifiermodel, attribute: $scope.BtnAttributesmodel, value: $scope.values[index].Value }
                }).success(function (data) {
                    $scope.Res = data + " item(s) has been updated successfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.load_values();
                }).error(function () {
                    $scope.Res = "Error occures";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.ngdis = false;
                    $scope.show_loading_multiple = false;
                });
            }
            
        };
        $scope.clear_page = function () {
          //  alert("in");
            $scope.ModifierOptions = [];
            $scope.AttributesOptions = [];
            $scope.values = [];
            $scope.values1 = [];
            $scope.BtnNounmodel = 'Select Noun  ';
            $scope.BtnModifiermodel = 'Select Modifier  ';
            $scope.BtnAttributesmodel = 'Select Attributes  ';
            $scope.ngdis = false;
            $scope.dis_table = false;
            $scope.serchFilter = "";
            $scope.serchFilter1 = "";
            $scope.serchFilter2 = "";

        };

    });