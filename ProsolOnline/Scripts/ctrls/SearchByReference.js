var reqcont = angular.module('ProsolApp', []);

reqcont.controller('SearchByReferenceController',
    function ($scope, $http, $timeout, $filter) {


        $scope.checkCataloguer = function () {
            $scope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/checkCataloguer'
            }).success(function (response) {
                $scope.CataloguerIndicator = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        }
        $scope.checkCataloguer();


        $scope.NounOptions = [];
        $scope.ModifierOptions = [];
        $scope.IndustrySectorOptions = [];
        $scope.MaterialTypeOptions = [];
        $scope.InspectionTypeOptions = [];
        $scope.PlantOptions = [];
        $scope.ProfitCenterOptions = [];
        $scope.StorageLocationOptions = [];
        $scope.StorageBinOptions = [];
        $scope.ValuationClassOptions = [];
        $scope.PriceControlOptions = [];
        $scope.MRPTypeOptions = [];
        $scope.MRPControllerOptions = [];
        $scope.ProcurementTypeOptions = [];
        $scope.res_table = "false";
        // $scope.code = [];

        $scope.values = [];
        $scope.values1 = [];
        $scope.BtnNounmodel = 'Select';
        $scope.BtnModifiermodel = 'Select';
        $scope.BtnIndustrySectormodel = 'Select';
        $scope.BtnMaterialTypemodel = 'Select';
        $scope.BtnInspectionTypemodel = 'Select';
        $scope.BtnPlantmodel = 'Select';
        $scope.BtnProfitCentermodel = 'Select';
        $scope.BtnStorageLocationmodel = 'Select';
        $scope.BtnStorageBinmodel = 'Select';
        $scope.BtnValuationClassmodel = 'Select';
        $scope.BtnPriceControlmodel = 'Select';
        $scope.BtnMRPTypemodel = 'Select';
        $scope.BtnMRPControllermodel = 'Select';
        $scope.BtnProcurementTypemodel = 'Select';

        $scope.ngdis = false;
        $scope.dis_table = "hidden";

        $http({
            method: 'GET',
            url: '/ValueStandardisation/GetNoun'
        }).success(function (result) {
            // alert(result);
            $scope.NounOptions = result;
        });
        // alert('hi');
        $http({
            method: 'GET',
            url: '/SearchByReference/GetPlants'
        }).success(function (result) {
            // alert(result);
            $scope.PlantOptions = result;
        });



        $scope.selectEntity = function (noun) {
            $scope.BtnNounmodel = noun;

            $scope.BtnModifiermodel = 'Select';
            $http({
                method: 'GET',
                url: '/ValueStandardisation/GetModifier',
                params :{noun: noun}
            }).success(function (result) {
                $scope.serchFilter = "";
                if (result.length > 0) {
                    $scope.ModifierOptions = result;
                }
            });
        };

        $scope.clearall = function () {
            $scope.BtnNounmodel = 'Select';
            $scope.ModifierOptions = [];
            $scope.BtnModifiermodel = 'Select ';
            $scope.BtnIndustrySectormodel = 'Select';
            $scope.BtnMaterialTypemodel = 'Select';
            $scope.BtnInspectionTypemodel = 'Select';
            $scope.BtnPlantmodel = 'Select';
            $scope.BtnProfitCentermodel = 'Select';
            $scope.StorageLocationOptions = [];
            $scope.BtnStorageLocationmodel = 'Select';
            $scope.StorageBinOptions = [];
            $scope.BtnStorageBinmodel = 'Select';
            $scope.BtnValuationClassmodel = 'Select';
            $scope.BtnPriceControlmodel = 'Select';
            $scope.BtnMRPTypemodel = 'Select';
            $scope.BtnMRPControllermodel = 'Select';
            $scope.BtnProcurementTypemodel = 'Select';
            $scope.code = [];
            $scope.sResult = null;
        };

        $scope.selectEntity1 = function (noun, modifier) {
            $scope.BtnModifiermodel = modifier;

        };

        $scope.selectProcurementType = function (str1) {
            $scope.BtnProcurementTypemodel = str1;

        };
        $scope.selectMRPController = function (str1) {
            $scope.BtnMRPControllermodel = str1;

        };
        $scope.selectMRPType = function (str1) {
            $scope.BtnMRPTypemodel = str1;

        };
        $scope.selectPriceControl = function (str1) {
            $scope.BtnPriceControlmodel = str1;

        };
        $scope.selectValuationClass = function (str1) {
            $scope.BtnValuationClassmodel = str1;

        };
        $scope.selectStorageBin = function (str1) {
            $scope.BtnStorageBinmodel = str1;

        };

        $scope.selectStorageLocation = function (str1) {

            $scope.StorageBinOptions = [];
            $scope.BtnStorageBinmodel = 'Select';

            $scope.BtnStorageLocationmodel = str1;

            $http({
                method: 'GET',
                url: '/SearchByReference/GetStorageBin',
                params: { Plant: $scope.BtnPlantmodel, sl: $scope.BtnStorageLocationmodel }
                //?Plant=' + $scope.BtnPlantmodel + '&sl=' + $scope.BtnStorageLocationmodel
            }).success(function (result) {
                // alert(result);
                $scope.StorageBinOptions = result;
            });


        };

        $scope.selectProfitCenter = function (str1) {
            $scope.BtnProfitCentermodel = str1;

        };

        $scope.selectPlant = function (str1) {
            $scope.StorageLocationOptions = [];
            $scope.BtnStorageLocationmodel = 'Select';
            $scope.StorageBinOptions = [];
            $scope.BtnStorageBinmodel = 'Select';

            $scope.BtnPlantmodel = str1;
            // alert("in");
            $http({
                method: 'GET',
                url: '/SearchByReference/GetStorageLocations',
                params :{Plant: $scope.BtnPlantmodel}
            }).success(function (result) {
                // alert(result);
                $scope.StorageLocationOptions = result;
            });
        };

        $scope.selectInspectionType = function (str1) {
            $scope.BtnInspectionTypemodel = str1;

        };
        $scope.selectMaterialType = function (str1) {
            $scope.BtnMaterialTypemodel = str1;

        };
        $scope.selectIndustrySector = function (str1) {
            $scope.BtnIndustrySectormodel = str1;

        };


        $http({
            method: 'GET',
            url: '/Master/GetMaster'
        }).success(function (result) {
            $scope.IndustrySectorOptions = $filter('filter')(result, { Label: "Industry sector" });
            $scope.MaterialTypeOptions = $filter('filter')(result, { Label: "Material type" });
            $scope.InspectionTypeOptions = $filter('filter')(result, { Label: "Inspection type" });
            $scope.ProfitCenterOptions = $filter('filter')(result, { Label: "Profit center" });
            $scope.ValuationClassOptions = $filter('filter')(result, { Label: "Valuation class" });
            $scope.PriceControlOptions = $filter('filter')(result, { Label: "Price control" });
            $scope.MRPTypeOptions = $filter('filter')(result, { Label: "Mrp type" });
            $scope.MRPControllerOptions = $filter('filter')(result, { Label: "Mrp controller" });
            $scope.ProcurementTypeOptions = $filter('filter')(result, { Label: "Procurement type" });
        });


        $scope.chkandshowresults = function () {

            //Code
            if ($scope.code != undefined && $scope.code != '') {

                $http({
                    method: 'GET',
                    url: '/SearchByReference/GetResultForCode',
                    params :{code: $scope.code}
                }).success(function (result) {
                    if (result != '') {
                        $scope.sResult = result;
                        $scope.Res = "Search results : " + result.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    } else {
                        $scope.sResult = null;
                        $scope.Res = "No items found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }

                }).error(function (data, status, headers, config) {

                });

            }
            else if ($scope.BtnNounmodel != 'Select' && $scope.BtnIndustrySectormodel == 'Select' && $scope.BtnMaterialTypemodel == 'Select' && $scope.BtnInspectionTypemodel == 'Select' && $scope.BtnPlantmodel == 'Select' && $scope.BtnProfitCentermodel == 'Select' && $scope.BtnStorageLocationmodel == 'Select' && $scope.BtnStorageBinmodel == 'Select' && $scope.BtnValuationClassmodel == 'Select' && $scope.BtnPriceControlmodel == 'Select' && $scope.BtnMRPTypemodel == 'Select' && $scope.BtnMRPControllermodel == 'Select' && $scope.BtnProcurementTypemodel == 'Select') {

                $http({
                    method: 'GET',
                    url: '/SearchByReference/GetResultFornoun_modifier',
                    params: { noun: $scope.BtnNounmodel, modifier: $scope.BtnModifiermodel }
                    //?noun=' + $scope.BtnNounmodel + '&modifier=' + $scope.BtnModifiermodel
                }).success(function (result) {
                    if (result != '') {
                        $scope.sResult = result;
                        $scope.Res = "Search results : " + result.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    } else {
                        $scope.sResult = null;
                        $scope.Res = "No items found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }

                }).error(function (data, status, headers, config) {

                });

            }
            else {

                $http({
                    method: 'get',
                    url: '/searchbyreference/chkandshowresults',
                    params: { noun: $scope.BtnNounmodel, modifier: $scope.BtnModifiermodel, industry_sector: $scope.BtnIndustrySectormodel , material_type:$scope.BtnMaterialTypemodel ,inspection_type: $scope.BtnInspectionTypemodel,plant: $scope.BtnPlantmodel ,profit_center: $scope.BtnProfitCentermodel ,storage_location: $scope.BtnStorageLocationmodel ,storage_bin: $scope.BtnStorageBinmodel , valuation_class: $scope.BtnValuationClassmodel , price_control: $scope.BtnPriceControlmodel , mrp_type: $scope.BtnMRPTypemodel, mrp_controller: $scope.BtnMRPControllermodel, procurement_type: $scope.BtnProcurementTypemodel}
                    //?noun=' + $scope.BtnNounmodel + '&modifier=' + $scope.BtnModifiermodel + '&industry_sector=' + $scope.BtnIndustrySectormodel + '&material_type=' + $scope.BtnMaterialTypemodel + '&inspection_type=' + $scope.BtnInspectionTypemodel + '&plant=' + $scope.BtnPlantmodel + '&profit_center=' + $scope.BtnProfitCentermodel + '&storage_location=' + $scope.BtnStorageLocationmodel + '&storage_bin=' + $scope.BtnStorageBinmodel + '&valuation_class=' + $scope.BtnValuationClassmodel + '&price_control=' + $scope.BtnPriceControlmodel + '&mrp_type=' + $scope.BtnMRPTypemodel + '&mrp_controller=' + $scope.BtnMRPControllermodel + '&procurement_type=' + $scope.BtnProcurementTypemodel
                }).success(function (result) {

                    if (result != '') {

                        $scope.sResult = result;
                        $scope.Res = "Search results : " + result.length + " items."
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    } else {
                        $scope.sResult = null;
                        $scope.Res = "No items found"
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    }

                }).error(function (data, status, headers, config) {

                });

            }
            // }
        };


        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
            $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params :{Itmcode: Itemcode}
            }).success(function (response) {
                if (response != '') {
                    $scope.cat = response;
                    $scope.img = {};
                    $http({
                        method: 'GET',
                        url: '/Search/GetImage?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                    }).success(function (response) {
                        if (response != '') {
                            $scope.img = response;

                        } else {

                            $scope.img = null;

                        }
                    })

                } else {
                    $scope.cat = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
            $scope.erp = {};
            $http({
                method: 'GET',
                url: '/Search/GetItemERP',
                params :{Itmcode: Itemcode}
            }).success(function (response) {
                if (response != '') {
                    $scope.erp = response;


                } else {
                    $scope.cat = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });

            new jBox('Modal', {
                width: 1200,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: false,
                overlay: true,
                closeButton: true,
                content: jQuery('#srefconId'),
                reposition: false,
                repositionOnOpen: false
            }).open();


        };


    });


