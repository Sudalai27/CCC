(function () {
    'use strict';


    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter', 'datatables']);


  
  
    app.controller('AssetCustomerrController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {
        $scope.desctab = "active";
        $scope.desctab1 = "active";
        $scope.dis = false;
        $scope.sts1 = false;

        $scope.LoadData = function () {

            

                $rootScope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/FAR/getHierarchy',
                    params: { Bis: "", Major: "", FunStr: "" }
                }).success(function (response) {

                    $scope.AssetdataList = response;
                   

                }).error(function (data, status, headers, config) {

                });

         
        }
        $scope.LoadData();
        $scope.GetUserList = function () {
            $http({
                method: 'GET',
                url: '/Catalogue/showall_user'
            }).success(function (response) {
                $scope.UserList = response;

                //  alert(angular.toJson($scope.UserList))
            }).error(function (data, status, headers, config) {
                // alert("error");
            });



        };
        $scope.GetUserList();
        $("#warantydate").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.asset.Warranty_ExpiryDate = $('#warantydate').val();

                // $scope.Todate = $('#txtTo').val();
            }
        });

        $("#Transacdate").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.asset.Transaction_date = $('#Transacdate').val();

                // $scope.Todate = $('#txtTo').val();
            }
        });

        $("#Depdate").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.asset.Depreciation_Startdate = $('#Depdate').val();

                // $scope.Todate = $('#txtTo').val();
            }
        });
     
        $scope.BindBusinessList = function () {
            $http({
                method: 'GET',                url: '/FAR/GetBusiness'
            }).success(function (response) {
                $scope.getbusiness = response.Businesses;
                $scope.getmajorCls = response.MajorClasses;
                $scope.getminorcls = response.MinorClasses;
                $scope.getRegion = response.Regions;
                $scope.getcity = response.Cities;
                $scope.getarea = response.Areas;
                $scope.getSub = response.SubAreas;
                $scope.getloc = response.Locations;
                $scope.getidenti = response.Identifiers;
                $scope.getequipclass = response.EquipmentClasses;
                $scope.getequiptype = response.EquipmentTypes;


            }).error(function (data, status, headers, config) {                //alert("error");            });
        };        $scope.BindBusinessList();     
      

      
        $scope.reset = function () {

            $scope.form.$setPristine();
        }

       

        new jBox('Tooltip', {
            attach: '#showstatus',
            //width: 400,
            //height: 500,                   
            closeButton: true,
            //animation: 'zoomIn',
            theme: 'TooltipBorder',
            trigger: 'click',
            width: 200,
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
     
        $scope.openClfPOP = function (UniqueId,ind) {
         
            new jBox('Modal', {
               // attach: '#clfremark' + ind,
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 500,
              
                adjustTracker: true,
                closeOnClick: 'body',
                closeOnEsc: true,
                animation: 'move',
                //position: {
                //    x: 'center',
                //    y: 'center'
                //},
                outside: 'y',
                content: jQuery('#conId' + UniqueId+ ind),
            
                overlay: false,
                reposition: false,
                repositionOnOpen: false,                
            }).open();
          
        }
     
        $scope.RowOnClick = function (cat1, idx) {
            $scope.NotifiyRes = false;
          
            var i = 0;
            angular.forEach($scope.AssetdataList, function (lst) {
                $('#' + i).attr("style", "");
                i++;
            });
            $('#' + idx).attr("style", "background-color:lightblue");         
           

            $scope.show = true;
            $scope.hindx = idx;
            $scope.hBusiness = cat1.Business;
            $scope.hMajor = cat1.MajorClass;
            $scope.hMinor = cat1.MinorClass;
            $scope.LoadGrid(cat1.Business, cat1.MajorClass, cat1.MinorClass);
        
        }
        $scope.LoadGrid = function (busi,major,minor) {
            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/FAR/getHierarchy',
                params: { Bis: busi, Major: major, FunStr: minor }

            }).success(function (response) {

                if (response.length > 0) {

                    $scope.AssetMinorList = response;


                    //// Setup - add a text input to each footer cell
                    //$('#example tfoot th').each(function () {
                    //    var title = $(this).text();
                    //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
                    //});

                    // DataTable
                    //var table = $("#tblFar").DataTable({
                    //    initComplete: function () {
                    //        // Apply the search
                    //        this.api()
                    //            .columns()
                    //            .every(function () {
                    //                var that = this;

                    //                $('input', this.footer()).on('keyup change clear', function () {
                    //                    if (that.search() !== this.value) {
                    //                        that.search(this.value).draw();
                    //                    }
                    //                });
                    //            });
                    //    },
                    //});

                }
                else {

                    $scope.Noitem = true;
                    $scope.show = true;
                    $scope.Result = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }

            })
        }
        $scope.getBOMList = function (Uid) {
          
            $http({
                method: 'GET',                url: '/FAR/getBomList',
                params: { UniqueId: Uid }
            }).success(function (response) {                             $scope.BomList = response;
            }).error(function (data, status, headers, config) {                //alert("error");            });
        };
      
     
        $scope.SaveFields=function()
        {
            

            var formData = new FormData()
            formData.append("disFields", angular.toJson($scope.rows));

            $http({
                url: "/FAR/InsertFields",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
              
                $scope.Res = "Saved successfully";
                $scope.NotifiyRes = true;
                $scope.Notify = "alert-info";
                $('#divNotifiy').attr('style', 'display: block');

            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
            });

        }
        $scope.getFieldsList = function () {

            $http({
                method: 'GET',                url: '/FAR/getFieldsList',
              
            }).success(function (response) {
                             
                if (response == '') {
                    $scope.rows = [{
                        'ColName': 'Barcode', 'Display': 'Barcode', 'Visible': true
                    },
                  {
                      'ColName': 'NewTagNo', 'Display': 'NewTagNo', 'Visible': true
                  },
                   {
                       'ColName': 'SiteId', 'Display': 'SiteId', 'Visible': true
                   },
                  {
                      'ColName': 'SiteName', 'Display': 'SiteName', 'Visible': true
                  },
                  {
                      'ColName': 'SiteType', 'Display': 'SiteType', 'Visible': true
                  },
                  {
                      'ColName': 'CompanyCode', 'Display': 'CompanyCode', 'Visible': true
                  },
                 {
                     'ColName': 'SAP_Equipment', 'Display': 'SAP_Equipment', 'Visible': true
                 },
                 {
                     'ColName': 'OldTagNo', 'Display': 'OldTagNo', 'Visible': true
                 },
                 {
                     'ColName': 'AssetNo', 'Display': 'AssetNo', 'Visible': true
                 },
                  {
                      'ColName': 'AssetSubNo', 'Display': 'AssetSubNo', 'Visible': true
                  },
                  {
                      'ColName': 'ObjectId', 'Display': 'ObjectId', 'Visible': true
                  },
                  {
                      'ColName': 'ObjectType', 'Display': 'ObjectType', 'Visible': true
                  },

                  {
                      'ColName': 'Business', 'Display': 'Business', 'Visible': true
                  },
                  {
                      'ColName': 'MajorClass', 'Display': 'MajorClass', 'Visible': true
                  },
                 {
                     'ColName': 'MinorClass', 'Display': 'MinorClass', 'Visible': true
                 },
                  {
                      'ColName': 'Region', 'Display': 'Region', 'Visible': true
                  },
                  {
                      'ColName': 'City', 'Display': 'City', 'Visible': true
                  },
                  {
                      'ColName': 'Area', 'Display': 'Area', 'Visible': true
                  },
                  {
                      'ColName': 'SubArea', 'Display': 'SubArea', 'Visible': true
                  },
                  {
                      'ColName': 'Identifier', 'Display': 'Identifier', 'Visible': true
                  },
                  {
                      'ColName': 'OldFunLoc', 'Display': 'OldFunLoc', 'Visible': true
                  },
                 {
                     'ColName': 'FLOC_Code', 'Display': 'FLOC_Code', 'Visible': true
                 },
                  {
                      'ColName': 'FuncLocDesc', 'Display': 'FuncLocDesc', 'Visible': true
                  },
                 {
                     'ColName': 'EquipmentDesc', 'Display': 'EquipmentDesc', 'Visible': true
                 },
                  {
                      'ColName': 'EquipmentClass', 'Display': 'EquipmentClass', 'Visible': true
                  },
                  {
                      'ColName': 'EquipmentType', 'Display': 'EquipmentType', 'Visible': true
                  }
                    ];
                }
                else {
                    $scope.rows = response;
                }
            }).error(function (data, status, headers, config) {                //alert("error");            });
        }
        $scope.getFieldsList();

        
        $scope.approveAll = function () {
           
            $scope.ApprovedItem = [];
            if ($scope.Appselected) {
              
                angular.forEach( $scope.AssetMinorList, function (lst) {
                    lst.SerialNo = true;
                 
                   
                });
              
            }
            else {
              
                angular.forEach( $scope.AssetMinorList, function (lst) {
                    lst.SerialNo = false;
                });
              
            }

        }
        $scope.Approvedata = function () {

          
            var formData = new FormData()
            formData.append("ApprovedItems", angular.toJson($scope.AssetMinorList));
            formData.append("ApproveRemark", $scope.Remark);
       
            $http({
                url: "/FAR/ApproveAll",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                $scope.Res = data+" Items Approved successfully";
                $scope.NotifiyRes = true;
                $scope.Notify = "alert-info";
                $('#divNotifiy').attr('style', 'display: block');
                $scope.LoadGrid($scope.hBusiness, $scope.hMajor, $scope.hMinor);
                $scope.LoadData();
                var i = 0;
                angular.forEach($scope.AssetdataList, function (lst) {
                    $('#' + i).attr("style", "");
                    i++;
                });
                $('#' + $scope.hindx).attr("style", "background-color:lightblue");



            }).error(function (data, status, headers, config) {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
            });

        }
        $scope.saveRemarks = function () {

           
                var formData = new FormData()
                formData.append("ApprovedItems", angular.toJson($scope.AssetMinorList));
                formData.append("CLFremark", $scope.Remark);
             
                $http({
                    url: "/FAR/ApproveAll",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    $scope.Res = data + " Items sent for Clarification";
                    $scope.NotifiyRes = true;
                    $scope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.LoadGrid($scope.hBusiness, $scope.hMajor, $scope.hMinor);
                    $scope.LoadData();
                    var i = 0;
                    angular.forEach($scope.AssetdataList, function (lst) {
                        $('#' + i).attr("style", "");
                        i++;
                    });
                    $('#' + $scope.hindx).attr("style", "background-color:lightblue");

                    $scope.Remark = null;

                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });

            
        }

        $scope.LoadGrid = function (busi, major, minor) {


            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/FAR/getHierarchy',
                params: { Bis: busi, Major: major, FunStr: minor }

            }).success(function (response) {

                if (response.length > 0) {                    
                    var colArr = [];
                 
                
                        angular.forEach($scope.rows, function (lst) {
                          
                                colArr.push(lst.Display);
                               
                           
                        });
                   
                    $scope.AssetMinorList = response;

                   
                   
                }
                else {

                    $scope.Noitem = true;
                    $scope.show = true;
                    $scope.Result = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }

            })
        }
        $scope.GetItemsList = function () {
            $http.get('/MaterialRequest/getplant').success(function (response) {

                $scope.Business = response

                if (response == "GRASIM") {

                    $scope.data = [{ 'Source Short': '', 'UOM': '', 'Source Long': '', 'Remark': '', 'MaterialCode': '', 'PlantCode': '', 'Priority': '', 'MaterialType': '', 'Make': '', 'Model': '', 'EquipmenName': '', 'Equipmentmfr': '', 'EquipmentTagNo': '', 'EquipmentModelNo': '', 'EquipmentSerialNo': '', 'EquipmentInfo': '', 'MaterialGroup': '', 'Div': '', 'ABC_ID': '', 'ALT_UNIT': '', 'Pur_valkey': '', 'HSN': '', 'Purchase_Group': '', 'Profit_CTR': '', 'STGE_LOC': '', 'Moving_PR': '', 'Val_Class': '', 'SALES_ORG': '', 'Distr_Chan': '', 'QualityInspection': '' }];
                    for (var i = 2; i <= 20; i++) {
                        $scope.data.push({ 'Source Short': '', 'UOM': '', 'Source Long': '', 'Remark': '', 'MaterialCode': '', 'PlantCode': '', 'Priority': '', 'MaterialType': '', 'Make': '', 'Model': '', 'EquipmenName': '', 'Equipmentmfr': '', 'EquipmentTagNo': '', 'EquipmentModelNo': '', 'EquipmentSerialNo': '', 'EquipmentInfo': '', 'MaterialGroup': '', 'Div': '', 'ABC_ID': '', 'ALT_UNIT': '', 'Pur_valkey': '', 'HSN': '', 'Purchase_Group': '', 'Profit_CTR': '', 'STGE_LOC': '', 'Moving_PR': '', 'Val_Class': '', 'SALES_ORG': '', 'Distr_Chan': '', 'QualityInspection': '' });;
                    }
                  
                }
                else {

                    $scope.data = [{ 'Source Short': '', 'UOM': '', 'Source Long': '', 'Remark': '', 'MaterialCode': '', 'Priority': '', 'MaterialType': '', 'UnitPrice': '', 'Make': '', 'Model': '', 'EquipmenName': '', 'Equipmentmfr': '', 'EquipmentTagNo': '', 'EquipmentModelNo': '', 'EquipmentSerialNo': '', 'EquipmentInfo': '' }];
                    for (var i = 2; i <= 20; i++) {
                        $scope.data.push({ 'Source Short': '', 'UOM': '', 'Source Long': '', 'Remark': '', 'MaterialCode': '', 'Priority': '', 'MaterialType': '', 'UnitPrice': '', 'Make': '', 'Model': '', 'EquipmenName': '', 'Equipmentmfr': '', 'EquipmentTagNo': '', 'EquipmentModelNo': '', 'EquipmentSerialNo': '', 'EquipmentInfo': '' });
                    }
                    var $container = $("#myHandsonTable");
                    $container.handsontable({
                        data: $scope.data,
                        startRows: 20,
                        startCols: 7,
                        colHeaders: true,
                        rowHeaders: true,
                        contextMenu: true,
                        manualColumnResize: true,
                        manualRowResize: true,
                        dropdownMenu: true,
                        filters: true,
                        colWidths: 180,

                        //stretchH: 'all',
                        colHeaders: ["Source Short", "UOM", "Source Long", "Remark", "Material/SAP Code", "Priority(LOW,MEDIUM,HIGH,URGENT)", "Material Type", " Moving Price", "Item Make", "Item Model", "Equipmen Name", "Equipment mfr", "Equipment TagNo.", "Equipment ModelNo.", "Equipment SerialNo.", "Equipment Additional Info"]
                        // fixedColumnsLeft: 1,


                    });
                }

            });
        }
        $scope.LoadExcel = function (busi, major, minor) {


            $rootScope.cgBusyPromises = $http({

                method: 'GET',
                url: '/FAR/getHierarchy',
                params: { Bis: busi, Major: major, FunStr: minor }

            }).success(function (response) {

                if (response.length > 0) {
                    var colArr = [];


                    angular.forEach($scope.rows, function (lst) {

                        colArr.push(lst.Display);


                    });

                    $scope.AssetMinorList = response;

                    //var $container = $("#myHandsonTable");
                    //$container.handsontable({
                    //    data: response,
                    //    startRows:10,
                    //    startCols: 10,
                    //    colHeaders: true,
                    //    rowHeaders: true,
                    //    contextMenu: true,
                    //    ColName: true,
                    //    manualColumnResize: true,
                    //    manualRowResize: true,
                    //    dropdownMenu: true,
                    //    filters: true,
                    //    colWidths: 180,
                    //    //stretchH: 'all',
                    //    colHeaders: colArr,
                    //    // fixedColumnsLeft: 1,


                    //});
                    //var data = [
                    //    [
                    //        false,
                    //        "Tagcat",
                    //        "United Kingdom",
                    //        "Classic Vest",
                    //        "11/10/2020",
                    //        "01-2331942",
                    //        true,
                    //        "172",
                    //        2,
                    //        2
                    //    ],
                    //    [
                    //        true,
                    //        "Zoomzone",
                    //        "Indonesia",
                    //        "Cycling Cap",
                    //        "03/05/2020",
                    //        "88-2768633",
                    //        true,
                    //        "188",
                    //        6,
                    //        2
                    //    ],
                    //    [
                    //        true,
                    //        "Meeveo",
                    //        "United States",
                    //        "Full-Finger Gloves",
                    //        "27/03/2020",
                    //        "51-6775945",
                    //        true,
                    //        "162",
                    //        1,
                    //        3
                    //    ],
                    //    [
                    //        false,
                    //        "Buzzdog",
                    //        "Philippines",
                    //        "HL Mountain Frame",
                    //        "29/08/2020",
                    //        "44-4028109",
                    //        true,
                    //        "133",
                    //        7,
                    //        1
                    //    ],
                    //    [
                    //        true,
                    //        "Katz",
                    //        "India",
                    //        "Half-Finger Gloves",
                    //        "02/10/2020",
                    //        "08-2758492",
                    //        true,
                    //        "87",
                    //        1,
                    //        3
                    //    ],
                    //    [
                    //        false,
                    //        "Jaxbean",
                    //        "China",
                    //        "HL Road Frame",
                    //        "28/09/2020",
                    //        "84-3557705",
                    //        false,
                    //        "26",
                    //        8,
                    //        1
                    //    ],
                    //    [
                    //        false,
                    //        "Wikido",
                    //        "Brazil",
                    //        "HL Touring Frame",
                    //        "24/06/2020",
                    //        "20-9397637",
                    //        false,
                    //        "110",
                    //        4,
                    //        1
                    //    ],
                    //    [
                    //        false,
                    //        "Browsedrive",
                    //        "United States",
                    //        "LL Mountain Frame",
                    //        "13/03/2020",
                    //        "36-0079556",
                    //        true,
                    //        "50",
                    //        4,
                    //        4
                    //    ]];
                    //var $container = $("#myHandsonTable");
                    //$container.handsontable({
                    //    data: response,
                    //    height: 450,
                    //    //colWidths: [140, 126, 192, 100, 100, 90, 90, 110, 97],
                    //    colHeaders: colArr,
                    //    //columns: [
                    //    //    { data: 1, type: "text" },
                    //    //    { data: 2, type: "text" },
                    //    //    {
                    //    //        data: 3,
                    //    //        type: "text"
                    //    //    },
                    //    //    {
                    //    //        data: 4,
                    //    //        type: "date",
                    //    //        allowInvalid: false
                    //    //    },
                    //    //    { data: 5, type: "text" },
                    //    //    {
                    //    //        data: 6,
                    //    //        type: "checkbox",
                    //    //        className: "htCenter"
                    //    //    },
                    //    //    {
                    //    //        data: 7,
                    //    //        type: "numeric"
                    //    //    },
                    //    //    {
                    //    //        data: 8,
                    //    //        // renderer: progressBarRenderer,
                    //    //        readOnly: true,
                    //    //        className: "htMiddle"
                    //    //    },
                    //    //    {
                    //    //        data: 9,
                    //    //        //  renderer: starRenderer,
                    //    //        readOnly: true,
                    //    //        className: "star htCenter"
                    //    //    }
                    //    //],
                    //    dropdownMenu: true,
                    //    hiddenColumns: {
                    //        indicators: true
                    //    },
                    //    contextMenu: true,
                    //    multiColumnSorting: true,
                    //    filters: true,
                    //    rowHeaders: true,
                    //    manualRowMove: true,
                    //    // afterGetColHeader: alignHeaders,
                    //    //  afterOnCellMouseDown: changeCheckboxCell,
                    //    //  beforeRenderer: addClassesToRows,
                    //    licenseKey: "non-commercial-and-evaluation"
                    //});

                }
                else {

                    $scope.Noitem = true;
                    $scope.show = true;
                    $scope.Result = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }

            })
        }
    })

    app.filter("unique", function () {        // we will return a function which will take in a collection        // and a keyname        return function (collection, keyname) {            // we define our output and keys array;            var output = [],                keys = [];            // we utilize angular's foreach function            // this takes in our original collection and an iterator function            angular.forEach(collection, function (item) {                // we check to see whether our object exists                var key = item[keyname];                // if it's not already part of our keys array                if (keys.indexOf(key) === -1) {                    // add it to our keys array                    keys.push(key);                    // push this item to our final output array                    output.push(item);                }            });            // return our array which should be devoid of            // any duplicates            return output;        };    });

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