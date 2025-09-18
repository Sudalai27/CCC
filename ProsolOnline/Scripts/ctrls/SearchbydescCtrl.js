
(function () {
    'use strict';
  

    var app = angular.module('ProsolApp', ['cgBusy']);
  
    app.controller('SearchController', function ($scope, $http, $timeout, $rootScope, $sce) {

        $scope.Resdup = "";
        $scope.mapdupcode = "";
        $scope.showmapdup = false;
        
        $scope.Pushtosap = function (Mat) {
            $http({
                method: 'GET',
                url: '/Search/Pushtosap',
                params: { Matcode: Mat }
            }).success(function (response) {

                if (response == "go")
                { $window.location = '/Search/SAPResponse' }

            }).error(function (data, status, headers, config) {
                alert("error");
            });

        }

        $scope.checkCataloguer = function () {
            
          $http({
                method: 'GET',
                url: '/Search/checkCataloguer'
            }).success(function (response) {
                
                $scope.CataloguerIndicator = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        }
        //$scope.highlight = function (search) {
            
        //    var cellText = '';
        //    $('#resulttable').each(function () {
        //        cellText = $(this).html();
        //    });
        //  //  alert(cellText)
        //    if (!search) {
              
        //        return $sce.trustAsHtml(cellText);
               
        //    }
        //    return $sce.trustAsHtml(cellText.replace(new RegExp(search, 'gi'), '<span class="highlightedText">$&</span>'));
        //};
     

        $scope.checkCataloguer();
        $scope.SearchItem = function () {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            //  $timeout(function () { $scope.NotifiyRes = false; }, 30000);

            $scope.sResult = null;
           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetSearchResult',
                params: { sKey: $scope.searchkey, sBy: $scope.search }
                //?sKey=' + $scope.searchkey + '&sBy=' + $scope.search
            }).success(function (response) {
                if (response != '') {
                    $scope.sResult = response;

                    //angular.forEach($scope.sResult, function (src, idx) {
                    //    src.add = 0;
                    //    src.rem = 0;
                    //    src.isChecked = false;
                    //});
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                } else {
                    $scope.sResult = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
           //$scope.erp = {};
           //$http({
           //    method: 'GET',
           //    url: '/Search/GetSearcherpResult',
           //    params: { sKey: $scope.searchkey, sBy: $scope.search }
           //}).success(function (response) {
           //    if (response != '') {
           //        $scope.erpresult = response;
           //        angular.forEach($scope.erpresult, function (src, idx) {
           //            src.add = 0;
           //            src.rem = 0;
           //            src.isChecked = false;
           //        });

           //    } else {
           //        $scope.cat = null;

           //    }

           //}).error(function (data, status, headers, config) {
           //    // alert("error");
           //});

        };
    
        
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
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
             
                $scope.N = $scope.sResult[index].Noun;
                $scope.M = $scope.sResult[index].Modifier;
            }
        
            if ($scope.sResult[index].add == 0) {
               
                if ($scope.sResult[index].Noun == $scope.N && $scope.sResult[index].Modifier == $scope.M) {
                  
                    $scope.erpresult[index].add = 1;
                    $scope.add = false;
                   
                }
                else {

                    alert("Please Select Same Noun and Modifier")
                    angular.forEach($scope.erpresult, function (value, key) {
                        if (key == index) {
                            value.isChecked = false;
                        }
                    });
                }

            }
            else {
                $scope.erpresult[index].add = 0;
            }


        };
        var listArray = [];
        var listArray1 = [];
        $scope.list = [];
        $scope.Compare = function () {
         
            var listArray = [];

            angular.forEach($scope.erpresult, function (value, key) {
               
                if (value.add === 1) {
                    
                    if (listArray.length == 0) {
                        listArray.push({
                           
                            "Itemcode": "Itemcode",
                           
                            "Materialtype_":"Materialtype",
                            "BaseUOP_":"Base UOP",
                            "Unit_issue_":"Unit Issue",
                            "AlternateUOM_":"Alternate UOM",
                            "Oldmaterialno_":"Old Material No",
                            "Division_":"Division",
                            "Plant":"Plant",
                            "StorageLocation":"StorageLocation",
                            "StorageBin":"StorageBin",
                            "PriceControl":"PriceControl",
                            "MRPType":"MRPType",
                            "LOTSize":"LOTSize",
                            "ProcurementType":"ProcurementType",
                            "MaxStockLevel_":"Max Stock Level",
                            "MinStockLevel_":"Min Stock Level",
                            "MaterialStrategicGroup":"MaterialStrategicGroup",
                            "PurchasingGroup":"PurchasingGroup",
                            "Quantity_":"Quantity",
                            "PurchaseOrderText":"PurchaseOrderText",
                            "PlannedDeliveryTime":"PlannedDeliveryTime",
                            "DeliveringPlant_":"DeliveringPlant",
                            "OrderUnit_":"OrderUnit"
                        })
                     
                    
                        }
                
             
                    listArray.push(value);
                }
               
            });

            angular.forEach(listArray, function (src, idx) {
                src.rem = 0;
              //  alert(angular.toJson(src))
            });

            $scope.list = listArray;
        
            //  $scope.char =$scope.list[0].Characteristics;


            if ($scope.list.length > 1) {

                mymodalcompare.open();
             
               $scope.pop = true;
                $scope.N = null;
                $scope.M = null;
                angular.forEach($scope.erpresult, function (value, key) {
                    value.add = 0;
                    value.isChecked = false;
                });
            }
            //else
            //{
            //    alert("kk")
            //}
           

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
        $scope.Reqmaterialpopup = function (itemcode) {

            $scope.itmcde = itemcode;

            $scope.from = "support@codasol.com";
            $scope.subject = "Material Request - ItemCode(" + $scope.itmcde + ")";
            $http({
                method: 'GET',
                url: '/Search/Getitemdetails',
                params: { itemcode: $scope.itmcde }
            }).success(function (response) {

                $scope.itmdets = response;


            })
            $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params: { Itmcode: itemcode }
            }).success(function (response) {
                
                    $scope.cat = response;
                    
                })
            mymodalcontent1.open();
        }
      
        var mymodalcontent1 = new jBox('Modal', {
            width: 1000,
            // height:350,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#contentmailid'),
            reposition: false,
            repositionOnOpen: false
        })
       
        $scope.Sendmail = function () {
            $timeout(function () { $Scope.NotifiyRes = false; }, 5000);
            $scope.subjects = $scope.itmcde + ":" + $scope.subject;
            $http({
                method: 'GET',
                url: '/Search/SendMail?from=' + $scope.from + '&To=' + $scope.To + '&subject=' + $scope.subjects + '&msgbox=' + $scope.msgbox + '&mailtable=' + $scope.mailtable
            }).success(function (response) {
                if (response != false) {
                  
                   
                    $scope.Result = "Email Sent Successfully";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.msgbox = null;
                    $scope.from = null;
                    $scope.subjects = null;
                    // $
                } else {
                    
                    $scope.Result = "Email sending failed";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                }

            }).error(function (data, status, headers, config) {
               
               
            });

        };
        $scope.files = [];
        $scope.LoadFileData = function (files) {
            alert(angular.toJson(files))
         
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];

            }
           
        };
        $scope.UploadAttachment = function () {
           
            //alert(angular.tojson($scope.files[0]))            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);           // if ($scope.files[0] != null) {
             
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $http({
                    url: "/Search/UploadMailAttach",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    //$('#divNotifiy').attr('style', 'display: block');                    $scope.getattach()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getattach();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    alert("err")
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
           // };
        }
      
        $scope.getattach = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Search/attachfiles",
            }).success(function (response) {
                $scope.Attachments = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.RemoveAttachment = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Search/DeleteAttachment?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles1()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
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
 
        $scope.ClearItem = function () {

            $scope.searchkey = null;
            $scope.cat = null;
            $scope.erp = null;
            $scope.sResult = null;
            $scope.reset();
        }
        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params :{Itmcode: Itemcode}
            }).success(function (response) {
                if (response != '') {
                    $scope.cat = response;
                    $scope.img = {};
                    $http({
                        method: 'GET',
                        url: '/Search/GetImage',
                        params : {Noun: $scope.cat.Noun ,Modifier: $scope.cat.Modifier}
                        //?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
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
                params:{Itmcode: Itemcode}
            }).success(function (response) {
                if (response != '') {
                    $scope.erp = response;


                } else {
                    $scope.cat = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });


             mymodal.open();

        };
         
    var mymodal =   new jBox('Modal', {
        width: 1200,
        blockScroll: false,
        animation: 'zoomIn',
        draggable: false,
        overlay: true,
        closeButton: true,
        content: jQuery('#cotentid'),
        reposition: false,
        repositionOnOpen: false
    });
    $scope.Print = function () {
        var print_div = document.getElementById("cotentid").innerHTML;
        var print_area = window.open();
        print_area.document.write(print_div);
        print_area.document.close();
        print_area.focus();
        print_area.print();
        print_area.close();
        
        }
        $scope.clickToOpen1 = function (Itemcode) {
           // alert("hyia");
            $scope.mapdupcode = Itemcode;

            new jBox('Modal', {
                width: 500,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: false,
                overlay: true,
                closeButton: true,
                content: jQuery('#map-pop'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
        $scope.unmap1 = function (Itemcode) {
            //  alert("hai");
            //  alert(angular.toJson(Itemcode));
            // $scope.mapdupcode = Itemcode;
            if (confirm("Are you sure, Unmap this record?")) {


               $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Search/unmapcode',
                    params :{Itemcode: Itemcode}
                    // url: '/Search/Mapdulpicateitem?Itemcode=' + code1
                }).success(function (response) {

                    $scope.unmap = response;
                    $rootScope.Res = "Item unmap successfully"
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;




                }).error(function (data, status, headers, config) {

                });




            }
        };
        $scope.getduplicatelist = function (code) {
            // alert(angular.toJson(code));

           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/Mapdulpicateitem',
                paramsv :{Itemcode:code}

            }).success(function (response) {
                if (response != "") {
                    $scope.sResult = response;
                    $scope.Resdup = "Item code exits";
                    $scope.clrdup = "green";
                    $scope.showmapdup = false;
                    //  $scope.Notify = "alert-info";
                    // $scope.NotifiyRes = true;
                    // $scope.code = response;
                }
                else {
                    $scope.Resdup = "Item code isn't exits";
                    $scope.clrdup = "red";
                    $scope.showmapdup = true;

                }
            }).error(function (data, status, headers, config) {

            });

        }
        $scope.mapitem = function (code) {

           $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Search/savemapduplicate',
                params :{new_code: code,existing_code:$scope.mapdupcode}
                // url: '/Search/Mapdulpicateitem?Itemcode=' + code1
            }).success(function (response) {
                if (response != false) {
                    $scope.sResult = response;
                    $scope.Resdup = "Duplicate item code " + code + " has been created for " + $scope.mapdupcode + "";
                    $scope.clrdup = "green";
                    $scope.showmapdup = false;
                  //  $scope.sResult = null;
                    // $scope.code = "";


                }
                else {
                    $scope.Resdup = "Duplicate code isn't created";
                    $scope.clrdup = "red";
                    $scope.showmapdup = true;
                    //$scope.mapdupcode = "";
                }



            }).error(function (data, status, headers, config) {

            });

        }
    
    });
    app.filter('highlight', function ($sce) {
        return function (text, phrase) {
          
            if (phrase) text = text.replace(new RegExp('(' + phrase + ')', 'gi'),
              '<span class="highlighted">$1</span>')

            return $sce.trustAsHtml(text)
        }
    });
    app.controller('NMSearchController', function ($scope, $http, $timeout, $rootScope) {
        $http({
            method: 'GET',
            url: '/Dictionary/GetNoun'
        }).success(function (response) {
           
            $scope.Nounsrch = response;

        }).error(function (data, status, headers, config) {
            alert("error");

        });
        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
            $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params : {Itmcode: Itemcode}
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
                params :{Itmcode:Itemcode}
            }).success(function (response) {
                if (response != '') {
                    $scope.erp = response;


                } else {
                    $scope.cat = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            mymodal.open();

        };
        var mymodal =  new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid1'),
            reposition: false,
            repositionOnOpen: false
        });
        $scope.SelectNoun = function () {
          
            $scope.modifierDef = null;
            $scope.modifier = null;
               $scope.noun = $scope.lst.Noun;
               $http({
                   method: 'GET',
                   url: '/Dictionary/GetModifier',
                   params :{Noun: $scope.noun}
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {
                // alert("error");

            });

        };

        $scope.SelectModifier = function () {

            $scope.charaterDef = null;
            $scope.charater = null;

            if ($scope.lst.Modifier.toString().indexOf(',') == -1) {

                //  $scope.modifier = $scope.lst.Modifier;

                var NDef = $.grep($scope.Modifiers, function (lst) {
                    return lst.Modifier == $scope.lst.Modifier;
                })[0].ModifierDefinition;

                $scope.modifierDef = NDef;
            }
            $scope.getiteam($scope.lst.Noun, $scope.lst.Modifier)
        };
        $scope.getiteam = function (Noun, Modifier) {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/Getitem',
                params :{Noun: Noun,Modifier: Modifier}
            }).success(function (response) {
               
                if (response != '') {
                    $scope.LIST = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                } else {
                   
                    $scope.LIST = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                alert("error");

            });

        }




    });
    app.controller('ATTSearchController', function ($scope, $http, $timeout, $rootScope) {
        $http({
            method: 'GET',
            url: '/Dictionary/GetNoun'
        }).success(function (response) {

            $scope.Nouns = response;

        }).error(function (data, status, headers, config) {
            alert("error");

        });

        $scope.s = true;
        $scope.t = true;
        $scope.ChangeModifier = function () {

            if ($scope.cat.Modifier != null && $scope.cat.Modifier != '') {
                $scope.getiteam($scope.cat.Noun, $scope.cat.Modifier)
               $rootScope.cgBusyPromises = $http({
                    method: 'GET',
                    url: '/Dictionary/GetNounModifier',
                    params :{Noun: $scope.cat.Noun ,Modifier: $scope.cat.Modifier}
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;
                        $scope.Characteristics = response.ALL_NM_Attributes;

                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;
                            $scope.s = false;
                            $scope.t = false;
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });

                    }
                    else $scope.Characteristics = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

            }
            else {
                $scope.s = true;
                $scope.t = true;;
                // $scope.cat = null;
                $scope.Characteristics = null;
                //  $scope.equ = null;
            }
            //   $scope.getSimiliar();           

        };
        $scope.loadmodifier = function (noun) {
            $scope.cat.Noun = $scope.cat.Noun.toUpperCase();
            // alert("mod");
            //alert(angular.toJson(noun));
            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params :{Noun: noun}
            }).success(function (response) {

                $scope.Modifiers = response;

            }).error(function (data, status, headers, config) {

            });


        }
        $scope.getiteam = function (Noun, Modifier) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
           $rootScope.cgBusyPromises = $http({
               
                method: 'GET',
                url: '/Search/Getitem',
                params :{Noun: Noun ,Modifier: Modifier}
            }).success(function (response) {
              
                $('#divNotifiy').attr('style', 'display: block');
                if (response != '') {
                    $scope.LIST1 = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                } else {
                    $scope.LIST1 = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                alert("error");

            });

        }


        $scope.Check = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("cat", angular.toJson($scope.cat));
            formData.append("attri", angular.toJson($scope.Characteristics));

           $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Search/Attri',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
                $('#divNotifiy').attr('style', 'display: block');
                if (response != '') {
                   
                    $scope.LIST1 = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                } else {
                   
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }


            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params :{Noun: Noun ,Modifier: Modifier}
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
                content: jQuery('#cotentid2'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
        $scope.ClearItemss = function () {

            angular.forEach($scope.Characteristics, function (obj) {

                obj.Value = "";
            });

            $scope.LIST1 = null;
        }

        $scope.SelectCharater = function (Noun, Modifier, Attribute) {


            $http({
                method: 'GET',
                url: '/Catalogue/GetValues',
                params : {Noun: Noun ,Modifier: Modifier ,Attribute: Attribute}
            }).success(function (response) {
                $scope.Values = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };
    });
    app.controller('logicSearch', function ($scope, $http, $timeout, $rootScope) {
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

            $http.get('/GeneralSettings/GetSubGroupcodeList1?MainGroupCode=' + $scope.cat.Maincode
           ).success(function (response) {
               $scope.getsubgroup = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }

        $scope.getSubsubcode = function () {

            if ($scope.cat.Subcode != null)
                $scope.Subcode3 = "";

            $http.get('/GeneralSettings/GetSubsubGroupcodeList?SubGroupCode=' + $scope.cat.Subcode
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



        $scope.CheckCode = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("cat", angular.toJson($scope.cat));


           $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Search/Codalogic',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {

                if (response != '') {
                    $scope.LIST4 = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                } else {
                    $scope.LIST4 = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }


            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.ClearItems1 = function () {

            $scope.cat = "";
            $scope.LIST4 = null;
        }
        $scope.clickToOpen2 = function (Itemcode) {


           $rootScope.cgBusyPromises = $http({
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
                url: '/Search/GetItemERP?Itmcode=' + Itemcode
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
                content: jQuery('#cotentid4'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };

        $scope.BindUnspscList = function () {

            $http({
                method: 'GET',
                url: '/Search/Segment'
            }).success(function (response) {
                $scope.Segment = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $scope.BindUnspscList();
        $scope.getFamily = function () {


            if ($scope.u.Segment != null)
                $scope.Segment3 = "";

            $http.get('/Search/Family?Segment=' + $scope.u.Segment
           ).success(function (response) {
               $scope.Family = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }

        $scope.getClass = function () {

            if ($scope.u.Family != null)
                $scope.Family3 = "";

            $http.get('/Search/Class?Family=' + $scope.u.Family
           ).success(function (response) {
               $scope.Class = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }
        $scope.getCommodity = function () {

            if ($scope.u.Class != null)
                $scope.Class3 = "";

            $http.get('/Search/Commodity?Class=' + $scope.u.Class
           ).success(function (response) {
               $scope.Commodity = response
               // alert(angular.toJson($scope.getsubgroup));
           }).error(function (data, status, headers, config) {
           });
        }
        $scope.Commoditychange = function () {
            if ($scope.u.Commodity != null)
                $scope.Commodity3 = "";
            // else

        };
        $scope.Checkunspsc = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            var formData = new FormData();
            formData.append("U", angular.toJson($scope.u));
           $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Search/Unspsclogic',
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {

                if (response != '') {
                    $scope.LIST3 = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;

                } else {
                    $scope.LIST3 = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }


            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.ClearItems2 = function () {

            $scope.u = "";
            $scope.LIST3 = null;
        }
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
                content: jQuery('#cotentid3'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
    });
    app.controller('ManufactureSearch', function ($scope, $http, $timeout, $rootScope) {
        $scope.req = true
        $scope.SelectEquip = function (key) {


            $http({
                method: 'GET',
                url: '/Search/GetEquip',
                params :{EName:key}
            }).success(function (response) {
                $scope.Equips = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


        };
        $scope.SelectEquip()
        $scope.SearchMan = function (key) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            //   var formData = new FormData();
            //formData.append("key", angular.toJson($scope.key));

            if (key == null || key == "" || key == undefined) {
                $scope.req = true
            }
            else {

               $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Search/SearchMan',
                    params :{Name: key}

                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: block');
                    if (response != '') {
                        $scope.LIST6 = response;
                        $rootScope.Res = "Search results : " + response.length + " items."
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;

                    } else {
                        $scope.LIST6 = null;
                        $rootScope.Res = "No item found"
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;

                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }

        }

        $scope.ClearItems = function () {
            $scope.key = "";

            $scope.LIST6 = null;
        }

        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
           $rootScope.cgBusyPromises = $http({
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
                content: jQuery('#cotentid6'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
    });
    app.controller('RefSearch', function ($scope, $http, $timeout, $rootScope) {
        $scope.req = true

        $scope.SearchRef = function (key) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            //   var formData = new FormData();
            //formData.append("key", angular.toJson($scope.key));

            if (key == null || key == "" || key == undefined) {
                $scope.req = true
            }
            else {

               $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Search/SearchRef',
                    params :{Name:key}

                }).success(function (response) {
                    $('#divNotifiy').attr('style', 'display: block');
                    if (response != '') {
                        $scope.LIST7 = response;
                        $rootScope.Res = "Search results : " + response.length + " items."
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;

                    } else {
                        $scope.LIST7 = null;
                        $rootScope.Res = "No item found"
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;

                    }


                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }

        }

        $scope.ClearItems = function () {
            $scope.key = "";

            $scope.LIST7 = null;
        }

        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
           $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params :{Itmcode:Itemcode}
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
                content: jQuery('#cotentid7'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
    });
    app.controller('MLSearchController', function ($scope, $http, $timeout, $rootScope, $sce) {

        $scope.Resdup = "";
        $scope.mapdupcode = "";
        $scope.showmapdup = false;

        $scope.Pushtosap = function (Mat) {
            $http({
                method: 'GET',
                url: '/Search/Pushtosap',
                params: { Matcode: Mat }
            }).success(function (response) {

                if (response == "go")
                { $window.location = '/Search/SAPResponse' }

            }).error(function (data, status, headers, config) {
                alert("error");
            });

        }

        $scope.checkCataloguer = function () {

            $http({
                method: 'GET',
                url: '/Search/checkCataloguer'
            }).success(function (response) {

                $scope.CataloguerIndicator = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });

        }
        //$scope.highlight = function (search) {

        //    var cellText = '';
        //    $('#resulttable').each(function () {
        //        cellText = $(this).html();
        //    });
        //  //  alert(cellText)
        //    if (!search) {

        //        return $sce.trustAsHtml(cellText);

        //    }
        //    return $sce.trustAsHtml(cellText.replace(new RegExp(search, 'gi'), '<span class="highlightedText">$&</span>'));
        //};


        $scope.checkCataloguer();
        $scope.SearchItem = function () {
            $timeout(function () { $rootScope.NotifiyRes = false; }, 5000);
            if ($scope.search == "Search")
            { $scope.showtab = "Search"; }
            else { $scope.showtab = "Analysis"; }
                
                

            $scope.sResult = null;
            $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetAPIResult',
                params: { sKey: $scope.searchkey, sBy: $scope.search }
                //?sKey=' + $scope.searchkey + '&sBy=' + $scope.search
            }).success(function (response) {
                if (response != '') {
                   
                    $scope.sResult = response;
                    $rootScope.Res = "Search results : " + response.length + " items."
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                } else {
                    $scope.sResult = null;
                    $rootScope.Res = "No item found"
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.ClearItem = function () {

            $scope.searchkey = null;
            $scope.cat = null;
            $scope.erp = null;
            $scope.sResult = null;
            $scope.reset();
        }
        $scope.clickToOpen = function (Itemcode) {

            $scope.cat = {};
            $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/GetItemDetail',
                params: { Itmcode: Itemcode }
            }).success(function (response) {
                if (response != '') {
                    $scope.cat = response;
                    $scope.img = {};
                    $http({
                        method: 'GET',
                        url: '/Search/GetImage',
                        params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                        //?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
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
                params: { Itmcode: Itemcode }
            }).success(function (response) {
                if (response != '') {
                    $scope.erp = response;


                } else {
                    $scope.cat = null;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            mymodal.open();

        };

        var mymodal = new jBox('Modal', {
            width: 1200,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: false,
            overlay: true,
            closeButton: true,
            content: jQuery('#cotentid'),
            reposition: false,
            repositionOnOpen: false
        });
        $scope.Print = function () {
            var print_div = document.getElementById("cotentid").innerHTML;
            var print_area = window.open();
            print_area.document.write(print_div);
            print_area.document.close();
            print_area.focus();
            print_area.print();
            print_area.close();

        }
        $scope.clickToOpen1 = function (Itemcode) {
            // alert("hyia");
            $scope.mapdupcode = Itemcode;

            new jBox('Modal', {
                width: 500,
                blockScroll: false,
                animation: 'zoomIn',
                draggable: false,
                overlay: true,
                closeButton: true,
                content: jQuery('#map-pop'),
                reposition: false,
                repositionOnOpen: false
            }).open();

        };
        $scope.unmap1 = function (Itemcode) {
            //  alert("hai");
            //  alert(angular.toJson(Itemcode));
            // $scope.mapdupcode = Itemcode;
            if (confirm("Are you sure, Unmap this record?")) {


                $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/Search/unmapcode',
                    params: { Itemcode: Itemcode }
                    // url: '/Search/Mapdulpicateitem?Itemcode=' + code1
                }).success(function (response) {

                    $scope.unmap = response;
                    $rootScope.Res = "Item unmap successfully"
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;




                }).error(function (data, status, headers, config) {

                });




            }
        };
        $scope.getduplicatelist = function (code) {
            // alert(angular.toJson(code));

            $rootScope.cgBusyPromises = $http({
                method: 'GET',
                url: '/Search/Mapdulpicateitem',
                paramsv: { Itemcode: code }

            }).success(function (response) {
                if (response != "") {
                    $scope.sResult = response;
                    $scope.Resdup = "Item code exits";
                    $scope.clrdup = "green";
                    $scope.showmapdup = false;
                    //  $scope.Notify = "alert-info";
                    // $scope.NotifiyRes = true;
                    // $scope.code = response;
                }
                else {
                    $scope.Resdup = "Item code isn't exits";
                    $scope.clrdup = "red";
                    $scope.showmapdup = true;

                }
            }).error(function (data, status, headers, config) {

            });

        }
        $scope.mapitem = function (code) {

            $rootScope.cgBusyPromises = $http({
                method: 'POST',
                url: '/Search/savemapduplicate',
                params: { new_code: code, existing_code: $scope.mapdupcode }
                // url: '/Search/Mapdulpicateitem?Itemcode=' + code1
            }).success(function (response) {
                if (response != false) {
                    $scope.sResult = response;
                    $scope.Resdup = "Duplicate item code " + code + " has been created for " + $scope.mapdupcode + "";
                    $scope.clrdup = "green";
                    $scope.showmapdup = false;
                    //  $scope.sResult = null;
                    // $scope.code = "";


                }
                else {
                    $scope.Resdup = "Duplicate code isn't created";
                    $scope.clrdup = "red";
                    $scope.showmapdup = true;
                    //$scope.mapdupcode = "";
                }



            }).error(function (data, status, headers, config) {

            });

        }

    });
    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteNoun",
                    params :{term: term},
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

})();