(function () {
    'use strict';
    var app = angular.module('ProsolApp',['cgBusy']);


    app.controller('Exportcontroller', function ($scope, $http, $timeout, $window) {
        

        $("#txtFrom").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.Fromdate = $('#txtFrom').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                $("#txtTo").datepicker("option", "minDate", dt);
            }
        });
        $("#txtTo").datepicker({ maxDate: new Date() });
        $("#txtFrom1").datepicker({
            numberOfMonths: 1,
            onSelect: function (selected) {
                $scope.Fromdate1 = $('#txtFrom1').val();
                var dt = new Date(selected);
                dt.setDate(dt.getDate());
                $("#txtTo1").datepicker("option", "minDate", dt);
            }
        });
        $("#txtTo1").datepicker({ maxDate: new Date() });
        //$("#txtTo").datepicker({
        //    numberOfMonths: 1,

        //    //onSelect: function (selected) {
        //    //    var dt = new Date(selected);
        //    //    dt.setDate(dt.getDate() );
        //    //    $("#txtFrom").datepicker("option", "maxDate", dt);
        //    //}
        //});

        //$scope.dtOptions = DTOptionsBuilder.newOptions()
        //.withDisplayLength(10)
        //.withOption('bLengthChange', true);

        /*Option items*/
        var array = [];
        //$scope.options = [{ 'Name': 'Noun', 'Status': false }, { 'Name': 'Modifier', 'Status': false }, { 'Name': 'Legacy', 'Status': false }, { 'Name': 'Longdesc', 'Status': false }, { 'Name': 'Manufacturer', 'Status': false }, { 'Name': 'Shortdesc', 'Status': false }, { 'Name': 'Legacy2', 'Status': false }, { 'Name': 'Plants', 'Status': false }, { 'Name': 'Characteristics', 'Status': false }];
        $scope.options = [{ 'Name': 'Noun,Modifier', 'Status': false }, { 'Name': 'UOM', 'Status': false }, { 'Name': 'Legacy', 'Status': false }, { 'Name': 'Longdesc', 'Status': false }, { 'Name': 'Vendorsuppliers', 'Status': false }, { 'Name': 'Equipment', 'Status': false }, { 'Name': 'Shortdesc', 'Status': false }, { 'Name': 'Characteristics', 'Status': false }, { 'Name': 'Additionalinfo', 'Status': false }];
        $scope.check1 = function () {
            $scope.selection = [];
            angular.forEach($scope.options, function (value, key) {

                if (value.Status == true) {
                    if (value.Name != "Noun,Modifier") {
                        $scope.selection.push(value.Name);

                    }
                    else {
                        $scope.selection.push("Noun");
                        $scope.selection.push("Modifier");
                    }
                //if (value.Status == true) {
                    //    $scope.selection.push(value.Name);

                    if (value.Name === "Vendor Details") {
                        var lll = 0;
                        angular.forEach($scope.selection, function (valuee, key) {
                            if(valuee === "Vendorsuppliers")
                            {
                                lll = 1;
                            }
                        });
                        if (lll != 1) {
                            $scope.selection.push("Vendorsuppliers");
                        }
                           
                          
                    }

                
                }
            });
        }
       
       // $scope.statusoptions = [{ 'Name': 'Request', 'Name': 'Request', 'Status': false }, { 'Name': 'Approve', 'Status': false }, { 'Name': 'Catalogue', 'Status': false }, { 'Name': 'QA', 'Status': false }, { 'Name': 'QC', 'Status': false }, { 'Name': 'Released', 'Status': false }];
         $scope.statusoptions = [{ 'Vale': 'Requests Created', 'Name': 'Request', 'Status': false }, { 'Vale': 'Accepted SPIR/CRF', 'Name': 'Approve', 'Status': false }, { 'Vale': 'In progress', 'Name': 'Catalogue', 'Status': false }, { 'Vale': 'Ready for load', 'Name': 'QC', 'Status': false }, { 'Vale': 'Uploaded in SAP', 'Name': 'Released', 'Status': false }];
       
	   $scope.checks = function () {
            $scope.statusselection = [];
            angular.forEach($scope.statusoptions, function (value, key) {

                if (value.Status == true) {
                        $scope.statusselection.push(value.Name);
                }
            });
        }
      //  export_images

        //$scope.export_images = function () {
        //   // alert(angular.toJson($scope.mat_code));
        //  //  var mat_res = $scope.mat_code.split(/["\n\t ,"]+/);

        //   // alert(angular.toJson(mat_res));
          
        //    $window.open('/Catalogue/Downloadimages?mat_code=' + $scope.mat_code);
         
        //  //  $window.open('/Catalogue/Downloadfile?fileName=' + fileName + '&type=' + type + '&imgId=' + imgId);

        //};



        /*Load data of export*/
        $scope.load = function () {
           
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
          
            if ($scope.selection == undefined)
            {
                $scope.selection = [];
                angular.forEach($scope.options, function (x) {
                    $scope.selection.push(x.Name);
                });
            }
            if ($scope.selection != undefined)
            {
                var formData = new FormData();

                formData.append("selection", JSON.stringify($scope.selection));
                formData.append("Where", "UserName");
                formData.append("Value", $scope.value);
              
                if ($scope.Fromdate != undefined) {
                    formData.append("Fromdate", $scope.Fromdate);
                   
                }
                if ($scope.Todate != undefined) {
                    formData.append("Todate", $scope.Todate);
                   
                }                  
               
                formData.append("Role", $scope.role);
                formData.append("Status", $scope.status);

              

                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Report/loadexport',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {                                
                   
                    if (response != "" && response.length <= 100) {
                        $scope.tables = [];
                        $scope.tables = response;
                        $scope.len = response.length;

                        $scope.Res = "Loaded successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                       
                    } else if (response != "" && response.length > 100) {
                        $scope.tables = [];
                        $scope.len = response.length;
                        $scope.Res = "100 records only load here, You can export more than 100 records";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    } else if (response == "") {
                        $scope.len = null;
                        $scope.tables = [];
                        $scope.Res = "No data avaliable for load";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    }
                }).error(function (data, status, headers, config) {

                });// to get the access of user

            }
            else if ($scope.role == undefined || $scope.role=='') {
                $scope.Res = "Select Role";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }           
            else if ($scope.selection == undefined) {
                $scope.Res = "Select Fields";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }else {
                
            }
        }

        $scope.loadstatus = function () {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);

            if ($scope.statusselection == undefined) {
                $scope.statusselection = [];
                angular.forEach($scope.statusoptions, function (x) {
                    $scope.statusselection.push(x.Name);
                });
            }
            if ($scope.statusselection != undefined) {
                var formData = new FormData();

                formData.append("selection", JSON.stringify($scope.statusselection));
                if ($scope.Fromdate1 != undefined) {
                    formData.append("Fromdate", $scope.Fromdate1);

                }
                if ($scope.Todate1 != undefined) {
                    formData.append("Todate", $scope.Todate1);

                }

               

                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Report/loadstatusexport',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    if (response != "" && response.length <= 100) {
                        $scope.tables1 = [];
                        $scope.tables1 = response;
                        $scope.len1 = response.length;

                        $scope.Res = "Loaded successfully";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');


                    } else if (response != "" && response.length > 100) {
                        $scope.tables1 = [];
                        $scope.len1 = response.length;
                        $scope.Res = "100 records only load here, You can export more than 100 records";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    } else if (response == "") {
                        $scope.len1 = null;
                        $scope.tables1 = [];
                        $scope.Res = "No data avaliable for load";
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');

                    }
                }).error(function (data, status, headers, config) {

                });// to get the access of user

            }
           else {
                $scope.Res = "Select Status";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        }
        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.exportstatus = function () {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            if ($scope.statusselection == undefined) {
                $scope.statusselection = [];
                angular.forEach($scope.statusoptions, function (x) {
                    $scope.statusselection.push(x.Name);
                });
            }
            if ($scope.statusselection != undefined) {

                var fdate = null, tdate = null;
                if ($scope.Fromdate1 != undefined) {
                    fdate = $scope.Fromdate1;
                }
                if ($scope.Todate1 != undefined) {
                    tdate = $scope.Todate1;
                }

                $window.location = '/Report/StatusDownload?selection=' + JSON.stringify($scope.statusselection) + '&Fromdate=' + fdate + '&Todate=' + tdate;
                

            }
            else if ($scope.statusselection == undefined) {
                $scope.Res = "Select Status";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            } else {

            }

        }
        /*Downloading the Load data of export*/
        $scope.export = function () {
            
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            if ($scope.selection == undefined) {
                $scope.selection = [];
                angular.forEach($scope.options, function (x) {
                    $scope.selection.push(x.Name);
                });
            }
            if ($scope.selection != undefined && $scope.role != undefined) {               

                var fdate = null, tdate = null;
                if ($scope.Fromdate != undefined) {                  
                    fdate = $scope.Fromdate;
                }
                if ($scope.Todate != undefined) {                   
                    tdate = $scope.Todate;
                }
                
                if ($scope.status == "Clarification") {

                    $window.location = '/Report/DownloadOverall?selection=' + JSON.stringify($scope.selection) + '&Where=UserName&Value=' + $scope.value + '&Role=' + $scope.role + '&Status=' + $scope.status + '&Fromdate=' + fdate + '&Todate=' + tdate;
                }
                else
                {
                    $window.location = '/Report/Download?selection=' + JSON.stringify($scope.selection) + '&Where=UserName&Value=' + $scope.value + '&Role=' + $scope.role + '&Status=' + $scope.status + '&Fromdate=' + fdate + '&Todate=' + tdate;
                }
               
            }
            else if ($scope.role == undefined || $scope.role == '') {
                $scope.Res = "Select Role";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }            
            else if ($scope.selection == undefined) {
                $scope.Res = "Select Fields";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            } else {

            }



            //if ($scope.gettext != undefined && $scope.gettext.length > 0) {

            //    var blob1 = new Blob([document.getElementById('tbl').innerHTML], {
            //        type: "application/octet-stream"
            //        //type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            //    });
            //    if ($scope.role == "Cataloguer")
            //        $scope.file = "Cat";
            //    if ($scope.role == "Releaser")
            //        $scope.file = "Rel";
            //    if ($scope.role == "Reviewer")
            //        $scope.file = "Rev";
            //    $scope.filename = $scope.file + $scope.value;
             
            //    saveAs(blob1, $scope.filename + ".xls");

            //    $scope.Res = "Exported successfully";
            //    $scope.Notify = "alert-info";
            //    $scope.NotifiyRes = true;
            //    $('#divNotifiy').attr('style', 'display: block');
            //}else
            //{
            //    $scope.Res = "No data avaliable for download";
            //    $scope.Notify = "alert-danger";
            //    $scope.NotifiyRes = true;
            //    $('#divNotifiy').attr('style', 'display: block');
            //}
        }
        
        $scope.change = function (chk1) {
           if(chk1 === true)
           {
               $scope.code = null;
                $scope.cde = true;
              
           }
           else {
               $scope.cde = false;
           }
         
        }
        ////
        $scope.change1 = function (chk2) {
            if (chk2 === true) {
                $scope.cat = null;
                $scope.cde1 = true;
                $scope.char.$setPristine();
            }
            else {
                $scope.cde1 = false;
            }

        }


        $scope.roleChange = function () {
            $scope.findcontrol();
        }
        $scope.exportvendor = function () {
        
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("Code", $scope.code);
            formData.append("Chk", $scope.chk);

            if ($scope.code != undefined || $scope.chk === true) {
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Report/Expv',
                    params :{Code: $scope.code ,Chk:$scope.chk}
                }).success(function (response) {
                 
                    if (response > 0)
                    {
                        
                        $window.location = '/Report/Expv1?Code=' + $scope.code + '&Chk=' + $scope.chk
                    }
                    else
                    {
                        $scope.Res = "No Data Found";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                     
                    //}
                    //else if ($scope.chk === true  )
                    //{
                    //    $window.location = '/Report/Expv?Code=' + "undefined" + '&Chk=' + $scope.chk
                })
            }
            else {
                $scope.Res = "Please Give Input For Report";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
              
            }
           
         
        
        } 
        /*Get Userlist or Code the Load data of export*/
        $scope.findcontrol = function () {
          
        

                $scope.listItms = [];                
                 
                  
                        $http({
                            method: 'GET',
                            url: '/Report/getuser',
                            params: { role: $scope.role }

                            //?role=' + $scope.role
                            // data: $scope.role
                        }).success(function (response) {
                           // $scope.getuser = response;                           
                            angular.forEach(response, function (value, key) {
                                $scope.listItms.push(value.UserName);                              
                            });
                           
                        });
                   
                   
            
              
         
        }

        function BindControls() {
            $('#value').autocomplete({
                source: array,
                minLength: 0,
                scroll: true
            }).focus(function () {
                $(this).autocomplete("search", "");
            });
        }


        //MODIFIER

        $scope.loadmodifier = function (noun) {
            $scope.cat.Noun = $scope.cat.Noun.toUpperCase();
            // alert("mod");
            //alert(angular.toJson(noun));
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: noun }
                //?Noun=' + noun
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }

        //CHARACTERSTIC DOWNLOAD
        $scope.exportchar = function () {
            // alert('HH');
            //   alert(angular.toJson($scope.cat));
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("cat", $scope.cat);
            formData.append("Chk1", $scope.chk1);

            if ($scope.cat != undefined) {             

                 

                        $window.location = '/Report/EXPORTCHAR1?cat=' + $scope.cat.Noun + '&cat1=' + $scope.cat.Modifier
                   
               
            }
            else if ($scope.chk1 == true)
            {
                
               // $scope.promise = $http({
                 //   method: 'POST',
                  //  url: '/Report/EXPORTCHAR?Chk1=' + $scope.chk1
               // }).success(function (response) {


                $window.location = '/Report/EXPORTCHAR1?Chk1=' + $scope.chk1

                    //}
                    //else if ($scope.chk === true  )
                    //{
                    //    $window.location = '/Report/Expv?Code=' + "undefined" + '&Chk=' + $scope.chk
              //  })
            }

            else {
                $scope.Res = "Please Give Input For Report";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            }



        }

        //$scope.exportchar = function () {
        //   // alert('HH');
        // //   alert(angular.toJson($scope.cat));
        //    $timeout(function () {
        //        $('#divNotifiy').attr('style', 'display: none');
        //    }, 30000);
        //    var formData = new FormData();
        //    formData.append("cat", $scope.cat);
        //    formData.append("Chk1", $scope.chk1);
           
        //    if ($scope.cat != undefined ) {
        //        $scope.promise = $http({
        //            method: 'POST',
        //            url: '/Report/EXPORTCHAR?cat=' + $scope.cat.Noun + '&cat1=' + $scope.cat.Modifier 
        //        }).success(function (response) {
                   
                        
        //            $window.location = '/Report/EXPORTCHAR1?cat=' + $scope.cat.Noun + '&cat1=' + $scope.cat.Modifier
                  
        //            //}
        //            //else if ($scope.chk === true  )
        //            //{
        //            //    $window.location = '/Report/Expv?Code=' + "undefined" + '&Chk=' + $scope.chk
        //        })
        //    }
        //    else if( $scope.chk1 === true)


        //       {
        //        $scope.promise = $http({
        //        method: 'POST',
        //        url: '/Report/EXPORTCHAR?Chk1=' + $scope.chk1
        //    }).success(function (response) {
                   
                        
        //            $window.location = '/Report/EXPORTCHAR1?Chk1=' + $scope.chk1
                  
        //        //}
        //        //else if ($scope.chk === true  )
        //        //{
        //        //    $window.location = '/Report/Expv?Code=' + "undefined" + '&Chk=' + $scope.chk
        //    })
        //    }
            
        //    else {
        //        $scope.Res = "Please Give Input For Report";
        //        $scope.Notify = "alert-danger";
        //        $scope.NotifiyRes = true;
        //        $('#divNotifiy').attr('style', 'display: block');

        //    }



        //}


    });




    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteNoun",
                    params:{term: term},
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

})();