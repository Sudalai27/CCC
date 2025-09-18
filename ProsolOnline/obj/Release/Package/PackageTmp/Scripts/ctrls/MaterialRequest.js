
var reqcont = angular.module('ProsolApp', ['cgBusy']);
reqcont.controller('req_load_controller', function ($scope, $window, $http, $timeout, $filter, $location) {

    $scope.hidetb1 = true;
    $scope.hidetb2 = true;
    $scope.hidetb3 = true;
    $scope.search = false;
    $scope.load = true;
    $scope.sResultbulk = [];
    $scope.sResultbulk_blktabl = [];
    $scope.Plantoptions = [];
    $scope.Storageoptions = [];
    $scope.Groupoptions = [];
    $scope.SubGroupoptions = [];
    $scope.Approveroptions = [];

    $scope.Plantoptions1 = [];
    $scope.Storageoptions1 = [];
    $scope.Groupoptions1 = [];
    $scope.SubGroupoptions1 = [];
    $scope.Approveroptions1 = [];

    $scope.add_button = true;
    $scope.update_button = false;

    $scope.tablerow1 = 0;
    $scope.ngdis = false;
    $scope.ngdis1 = false;

    $scope.show_loading_multiple = false;
    $scope.show_loading_single = false;

    $scope.fileList = [];
    $scope.attachment = [];
    $scope.imgList = [];


    $scope.LoadFileData = function (files) {
     
        $timeout(function () {
            $scope.NotifiyRes = false;
        }, 30000);
        $scope.NotifiyRes = false;
        $scope.$apply();
        $scope.fileList = files;
        if (files[0] != null) {
            var ext = files[0].name.match(/\.(.+)$/)[1];
            if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx' || angular.lowercase(ext) === 'pdf' || angular.lowercase(ext) === 'docx'
                || angular.lowercase(ext) === 'doc' || angular.lowercase(ext) === 'txt') {
            } else {
                angular.element("input[type='file']").val(null);
                files[0] = null;
                $scope.Res = "Load EXCEL/TEXT/PDF files Only";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.$apply();
                $('#divNotifiy').attr('style', 'display: block');
            }
        }

    };

    $scope.getItem = function () {
       // alert("hai")
        var url = $location.$$absUrl;
        if (url.indexOf("MaterialRequest?source") !== -1) {

            var arrId = url.split('source=');

            $scope.source1 = arrId[1];
        }
    };
    $scope.getItem();
    $scope.BindMaster = function () {
        $http({
            method: 'GET',
            url: '/Master/GetMaster'
        }).success(function (response) {

            $scope.MasterList = response;

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    };
    $scope.BindMaster();
    $scope.addImg = function () {
        $scope.attachment.push($scope.fileList[0]);

        $('.fileinput').fileinput('clear');
    };

    $scope.RemoveFile = function (indx) {
        if ($scope.imgList.length > 0) {
            $scope.attachment.splice(indx, 1);
            $scope.imgList.splice(indx, 1);
        }
    };

    // loading plant on load
    $http({
        method: 'GET',
        url: '/MaterialRequest/getplantCode_Name'
    }).success(function (result) {
        $scope.Plantoptions = result;
       
        // $scope.Plantoptions = $filter('filter')($scope.Plantoptions, { 'Islive': 'true' })
        $scope.Plantoptions1 = result;
        //  $scope.Plantoptions1 = $filter('filter')($scope.Plantoptions1, { 'Islive': 'true' })
    });

    //loading storage on plant change

    $scope.getStoragecode_name = function (plant) {
        var plantvalue = $scope.ddlplant;
        var planttitle = $.grep($scope.Plantoptions, function (plant) {
            return plant.Plantcode == plantvalue;
        })[0].Plantname;

        $http({
            method: 'GET',
            url: '/MaterialRequest/getStorageCode_Name',
            params: { Plantcode: plantvalue }
        }).success(function (result) {
            if (result.length > 0) {
                $scope.Storageoptions = result;
                // $scope.Storageoptions = $filter('filter')($scope.Storageoptions, { 'Islive': 'true' })
            }
            else {
                $scope.Storageoptions = [];
                $scope.ddlstorage = "";
            }
        });
    };

    // file control onchange
    $scope.LoadFileData1 = function (files) {
        $timeout(function () {
            $scope.NotifiyRes = false;
        }, 30000);
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
                $('#divNotifiy').attr('style', 'display: block');
            }
        }
    };


    $scope.visiable = function () {
        //  alert("hai");
        $scope.makevisiable = true;
    };

    $scope.visiable1 = function () {
        $scope.bindRejecteditems();
        $scope.makevisiable = false;
        $scope.Loadpop3();
    };
    $scope.visiable2 = function () {
        $scope.bindClarificationitems();
        $scope.makevisiable = false;
        $scope.Loadpop4();

    };
    $scope.visiable3 = function () {
        //   alert("hai");
        $scope.makevisiable = false;
        $scope.Loadpop2();
    };


    $scope.Loadfile1 = function () {

        if ($scope.files != null && $scope.files != undefined && $scope.files != "") {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $timeout(function () { $scope.NotifiyRes1 = false; }, 5000);
            var formData = new FormData();
            formData.append('file', $scope.files[0]);
            $scope.cgBusyPromises = $http({
                url: "/MaterialRequest/Load_Data",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {

                if (response == "Check File Format") {
                    $scope.Res = "Check file format";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                else if (response == "Uploaded File is Empty") {
                    $scope.Res = "Uploaded File is Empty";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                }
                else if (response != null) {
                    $scope.hidetb2 = false;
                    $scope.sResultbulk = response;

                    // alert(angular.toJson($scope.sResultbulk));

                }

            }).error(function (data, status, headers, config) {

            });

        }

        else {
            $scope.Res = "Select file to be uploaded";
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
            $('#divNotifiy').attr('style', 'display: block');
            //alert("hai");
            //$scope.files = "";

        }
    };

    // loading SL for multiple

    $scope.getStoragecode_name1 = function (plant1) {
        var plantvalue = $scope.ddlplant1;
        if (plantvalue != undefined) {
            var planttitle = $.grep($scope.Plantoptions1, function (plant1) {
                return plant1.Plantcode == plantvalue;
            })[0].Plantname;

            $http({
                method: 'GET',
                url: '/MaterialRequest/getStorageCode_Name',
                params: { Plantcode: plantvalue }
            }).success(function (result) {
                if (result.length > 0) {
                    $scope.Storageoptions1 = result;
                    // $scope.Storageoptions1 = $filter('filter')($scope.Storageoptions1, { 'Islive': 'true' })
                }
                else {
                    $scope.Storageoptions1 = [];
                    $scope.ddlstorage1 = "";
                }
            });
        }
    };

    // loading group godes
    $http({
        method: 'GET',
        url: '/MaterialRequest/getgroupCode_Name'
    }).success(function (result) {
        $scope.Groupoptions = result;
        $scope.Groupoptions1 = result;
    });

    // loading subgroup code
    $scope.getsubgroupCode_Name = function (group) {
        var groupvalue = $scope.ddlgroup;
        var grouptitle = $.grep($scope.Groupoptions, function (group) {
            return group.code == groupvalue;
        })[0].title;
        $http({
            method: 'GET',
            url: '/MaterialRequest/getsubgroupCode_Name',
            params: { groupcode: groupvalue }
        }).success(function (result) {
            if (result.length > 0) {
                $scope.SubGroupoptions = result;
            }
            else {
                $scope.SubGroupoptions = [];
                $scope.ddlsubgroup = "";
            }


        });
    };


    //loading sub group code for multiple

    $scope.getsubgroupCode_Name1 = function (group1) {
        var groupvalue = $scope.ddlgroup1;
        if (groupvalue != undefined) {
            var grouptitle = $.grep($scope.Groupoptions1, function (group1) {
                return group1.code == groupvalue;
            })[0].title;

            $http({
                method: 'GET',
                url: '/MaterialRequest/getsubgroupCode_Name',
                params: { groupcode: groupvalue }
            }).success(function (result) {
                if (result.length > 0) {
                    $scope.SubGroupoptions1 = result;
                }
                else {
                    $scope.SubGroupoptions1 = [];
                    $scope.ddlsubgroup1 = "";
                }

            });
        }
    };


    // loading approver id and names

    $http({
        method: 'GET',
        url: '/MaterialRequest/get_approvercode_name'
    }).success(function (result) {
        $scope.Approveroptions = result;
        $scope.Approveroptions1 = result;
    });


    // new request

    $scope.newRequest = function () {

        $scope.show_loading_single = true;
        $timeout(function () { $scope.NotifiyRes = false; }, 5000);
        $scope.ngdis1 = true;
        //alert("success");
        //var plantname = $scope.ddlplant;
        //var storagename = $scope.ddlstorage;
        //var groupname = $scope.ddlgroup;
        //var subgroupname = $scope.ddlsubgroup;
        //var approvername = $scope.ddlapprover;
        //var source_desc = $scope.source;
        //  alert("success");

        $scope.rows_single = [];
        $scope.rows_single.push({
            itemId: $scope.itemId, plant: $scope.ddlplant, storage_Location: $scope.ddlstorage, group: $scope.ddlgroup,
            subGroup: $scope.ddlsubgroup, source: $scope.source
        });

        var form_singlerequest = new FormData();
        form_singlerequest.append('Single_request', angular.toJson($scope.rows_single));
        form_singlerequest.append('files', $scope.fileList[0]);


        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/MaterialRequest/newRequest',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: form_singlerequest
            //?plant=' + plantname + '&storage=' + storagename + '&group=' + groupname + '&subgroup=' + subgroupname + '&approver=' + approvername + '&source=' + source_desc

        }).success(function (data) {
            if (data.success === false) {
                $scope.Res = data.errors;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.show_loading_single = false;
            }
            else {
                $('#sbtn').val('Request');
                $scope.bindRejecteditems();
                $scope.reset_reqform();
                $scope.Res = "Request sent successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.show_loading_single = false;
                $('.fileinput').fileinput('clear');
            }
        }).error(function (data) {
            $scope.Res = "Request failed";
            $scope.Notify = "alert-info";
            $scope.NotifiyRes = true;
            $scope.ngdis1 = false;
            $scope.show_loading_single = false;
        });
    };

    // Adding row to multiple values

    $scope.rows = [];
    $scope.addrowtobulktable = function (form) {
        if (form != false) {
        var planttitle = $.grep($scope.Plantoptions1, function (plant1) {
            return plant1.Plantcode == $scope.ddlplant1;
        })[0].Plantname;

        var storagetitle = $.grep($scope.Storageoptions1, function (storage1) {
            return storage1.Code == $scope.ddlstorage1;
        })[0].Title;

        if ($scope.codelogic1 == "Customized Code")
            {
        var grouptitle = $.grep($scope.Groupoptions1, function (group1) {
            return group1.code == $scope.ddlgroup1;
        })[0].title;

        var subgrouptitle = $.grep($scope.SubGroupoptions1, function (subgroup1) {
            return subgroup1.code == $scope.ddlsubgroup1;
        })[0].title;
        }
      //  alert($scope.Materialtype)
        if ($scope.Materialtype != undefined && $scope.Materialtype != null && $scope.Materialtype != "")
            {
        var materialtype = $.grep($scope.MasterList, function (x) {
            return x.Code == $scope.Materialtype;
        })[0].Title;
        }
        if ($scope.Industrysector != undefined && $scope.Industrysector != null && $scope.Industrysector != "") {
            var Industrysector = $.grep($scope.MasterList, function (x) {
                return x.Code == $scope.Industrysector;
            })[0].Title;
        }
        if ($scope.MaterialStrategicGroup != undefined && $scope.MaterialStrategicGroup != null) {
            var MaterialStrategicGroup = $.grep($scope.MasterList, function (x) {
                return x.Code == $scope.MaterialStrategicGroup;
            })[0].Title;
        }

        //var UnitPrice = $.grep($scope.MasterList, function (x) {
        //    return x.Code == $scope.UnitPrice;
        //})[0].Title;

      //  alert(materialtype)
        //var approvertitle = $.grep($scope.Approveroptions1, function (approver1) {
        //    return approver1.Userid == $scope.ddlapprover1;
        //})[0].UserName;

        $scope.attachment.push($scope.fileList[0]);
        if ($scope.fileList[0] != null) {
            $scope.rows.push({ Sequence: $scope.rows.length + 1, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, UnitPrice: $scope.UnitPrice, source: $scope.source1, hplant: planttitle, hstorage_Location: storagetitle, hgroup: grouptitle, hsubGroup: subgrouptitle, hsource: $scope.source1, hmaterialtype: materialtype, hIndustrysector: Industrysector, hMaterialStrategicGroup: MaterialStrategicGroup, hUnitPrice: $scope.UnitPrice, hAttachment: $scope.fileList[0].name, attachment: $scope.fileList[0].name });
        }
        else {
            $scope.rows.push({ Sequence: $scope.rows.length + 1, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, UnitPrice: $scope.UnitPrice, source: $scope.source1, hplant: planttitle, hstorage_Location: storagetitle, hgroup: grouptitle, hsubGroup: subgrouptitle, hmaterialtype: materialtype, hIndustrysector: Industrysector, hMaterialStrategicGroup: MaterialStrategicGroup, hUnitPrice: $scope.UnitPrice, hsource: $scope.source1, hAttachment: "", attachment: "" });
        }
        $scope.sResult = [];
        $scope.reset_reqform_multi();
    } else {
        angular.element('input.ng-invalid,select.ng-invalid').first().focus();
        alert("Please fill the highlighted mandatory field(s)");
    }

    };

    $scope.clear_fields1 = function () {
        $scope.reset();
        $scope.Plantoptions1 = "";
        $scope.Groupoptions1 = "";
        $scope.Approveroptions1 = "";
        $scope.SubGroupoptions1 = "";
        $scope.Storageoptions1 = "";
        $scope.source1 = "";
        $scope.source = "";

        $http({
            method: 'GET',
            url: '/MaterialRequest/getplantCode_Name'
        }).success(function (result) {
            $scope.Plantoptions1 = result;
            // alert("hi");
        });

        $http({
            method: 'GET',
            url: '/MaterialRequest/getgroupCode_Name'
        }).success(function (result) {
            $scope.Groupoptions1 = result;
        });

        $http({
            method: 'GET',
            url: '/MaterialRequest/get_approvercode_name'
        }).success(function (result) {
            $scope.Approveroptions1 = result;
        });
    };

    $scope.update_table = function () {
      
        var planttitle = $.grep($scope.Plantoptions1, function (plant1) {
            return plant1.Plantcode == $scope.ddlplant1;
        })[0].Plantname;

        var storagetitle = $.grep($scope.Storageoptions1, function (storage1) {
            return storage1.Code == $scope.ddlstorage1;
        })[0].Title;

        if ($scope.codelogic1 == "Customized Code") {
            var grouptitle = $.grep($scope.Groupoptions1, function (group1) {
                return group1.code == $scope.ddlgroup1;
            })[0].title;

            var subgrouptitle = $.grep($scope.SubGroupoptions1, function (subgroup1) {
                return subgroup1.code == $scope.ddlsubgroup1;
            })[0].title;
        }

        var materialtype = $.grep($scope.MasterList, function (x) {
            return x.Code == $scope.Materialtype;
        })[0].Title;

        var Industrysector = $.grep($scope.MasterList, function (x) {
            return x.Code == $scope.Industrysector;
        })[0].Title;

        var MaterialStrategicGroup = $.grep($scope.MasterList, function (x) {
            return x.Code == $scope.MaterialStrategicGroup;
        })[0].Title;

        //  $scope.rows[$scope.tablerow1] = ({ Sequence: $scope.tablerow1 + 1, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, source: $scope.source1,  hplant: planttitle, hstorage_Location: storagetitle, hgroup: grouptitle, hsubGroup: subgrouptitle, hsource: $scope.source1 });
        if ($scope.fileList[0] != null) {
            $scope.rows[$scope.tablerow1] = ({ Sequence: $scope.rows.length, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, UnitPrice: $scope.UnitPrice, source: $scope.source1, hplant: planttitle, hstorage_Location: storagetitle, hgroup: grouptitle, hsubGroup: subgrouptitle, hsource: $scope.source1, hmaterialtype: materialtype, hIndustrysector: Industrysector, hMaterialStrategicGroup: MaterialStrategicGroup, hUnitPrice: $scope.UnitPrice, hAttachment: $scope.fileList[0].name, attachment: $scope.fileList[0].name });
        }
        else {
            $scope.rows[$scope.tablerow1] = ({ Sequence: $scope.rows.length, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, UnitPrice: $scope.UnitPrice, source: $scope.source1, hplant: planttitle, hstorage_Location: storagetitle, hgroup: grouptitle, hsubGroup: subgrouptitle, hmaterialtype: materialtype, hIndustrysector: Industrysector, hMaterialStrategicGroup: MaterialStrategicGroup, hUnitPrice: $scope.UnitPrice, hsource: $scope.source1, hAttachment: "", attachment: "" });
        }
       

        $scope.update_button = false;
        $scope.add_button = true;

        $scope.reset_reqform_multi();

    };

    // removerow(indexx)
    $scope.removerow = function (index) {
        $scope.reset_reqform_multi();
        $scope.add_button = true;
        $scope.update_button = false;
        if ($scope.rows.length > 1) {
            if ($window.confirm("Please confirm to remove row?")) {
                if ($scope.rows.length > 1) {
                    $scope.rows.length
                    $scope.rows.splice(index, 1);
                }
                var cunt = 1;
                angular.forEach($scope.rows, function (value, key) {
                    value.Sequence = cunt++;
                });
            }
            else {

            }
        }
        else {
            alert("You cant delete, better update this row");
        }
    };
   
    // update row 
    $scope.updaterow = function (index) {
       // if ($window.confirm("Please confirm to load Row to update?")) {
            $scope.add_button = false;
            $scope.update_button = true;

            $scope.tablerow1 = index;
            // alert($scope.rows[index].group);
            $scope.ddlplant1 = $scope.rows[index].plant;
            $scope.ddlgroup1 = $scope.rows[index].group;

          //  $scope.getsubgroupCode_Name1();
            $scope.getStoragecode_name1();

            $scope.ddlstorage1 = $scope.rows[index].storage_Location;
            $scope.ddlsubgroup1 = $scope.rows[index].subGroup;

            $scope.Materialtype = $scope.rows[index].Materialtype;
            $scope.Industrysector = $scope.rows[index].Industrysector;
            $scope.MaterialStrategicGroup = $scope.rows[index].MaterialStrategicGroup;
            $scope.UnitPrice = $scope.rows[index].UnitPrice;


            //  $scope.ddlapprover1 = $scope.rows[index].approver;
            $scope.source1 = $scope.rows[index].source;
            // $scope.reset_reqform_multi();

        //} else {

        //}
    };
    //Code Logic
    $scope.codelogic1 = '';
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

    // requesting bulk material items

    $scope.bulk_material_request = function () {
        $scope.show_loading_multiple = true;
        $timeout(function () { $scope.NotifiyRes = false; }, 5000);
        $scope.ngdis = true;
        var formvalues = new FormData();
        formvalues.append('Mul_request', angular.toJson($scope.rows));

        for (var i = 0; i < $scope.attachment.length; i++) {
            formvalues.append('files', $scope.attachment[i]);
        }


        $scope.cgBusyPromises = $http({
            url: "/MaterialRequest/bulk_material_request",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formvalues
        }).success(function (data) {
            if (data.success === false) {
                $scope.Res = data.errors;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
                $scope.show_loading_multiple = false;
            }
            else {
                $scope.reset_reqform_multi();
                $scope.rows = [];
                $scope.Res = "Request sent successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
                $scope.show_loading_multiple = false;
            }

        }).error(function () {
            $scope.Res = "Error occured";
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
            $scope.ngdis = false;
            $scope.show_loading_multiple = false;
        });
    };

    $scope.bulkupload_material_request = function () {
        $timeout(function () { $scope.NotifiyRes = false; }, 5000);
        $scope.ngdis = true;
        var formvalues = new FormData();
        formvalues.append('sResultbulk', angular.toJson($scope.sResultbulk));
        $scope.cgBusyPromises = $http({
            url: "/MaterialRequest/bulkupload_material_request",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formvalues
        }).success(function (data) {
            if (data === false) {
                $scope.Res = data.errors;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
            }
            else if (data === true) {
                $scope.reset_reqform_multi();
                $scope.sResultbulk = [];
                $scope.Res = "Request sent successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
            }
            else {
                $scope.Res = data;
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis = false;
            }

        }).error(function () {
            $scope.Res = "Error occured";
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
            $scope.ngdis = false;
            // $scope.show_loading_multiple = false;
        });
    };

    $scope.reset_reqform_multi = function () {
        $scope.ddlplant1 = "";
        $scope.Storageoptions1 = [];
        $scope.ddlstorage1 = "";
        $scope.ddlgroup1 = "";
        $scope.SubGroupoptions1 = [];
        $scope.ddlsubgroup1 = "";
        $scope.source1 = "";
        $scope.Materialtype =null;
        $scope.Industrysector =null;
        $scope.MaterialStrategicGroup =null;
        $scope.UnitPrice = null;

        $scope.hidetb1 = true;
        $scope.hidetb2 = true;
        $scope.itemId = null;
        $scope.remark1 = "";
        $('.fileinput').fileinput('clear');

        $scope.rereq_btn = false;
        $scope.add_button = true;

        $scope.reqform_multi.$setPristine();
    };

    $scope.reset_reqform = function () {
        $scope.ddlplant = "";
        $scope.Storageoptions = [];
        $scope.ddlstorage = "";
        $scope.ddlgroup = "";
        $scope.SubGroupoptions = [];
        $scope.ddlsubgroup = "";
        $scope.source = "";
        //  $scope.ddlapprover = "";

        $scope.reqform.$setPristine();
    };


    //Rejected
    $scope.bindRejecteditems = function () {
        $http({
            method: 'GET',
            url: 'requestlog/getRejected_Records'
        }).success(function (result) {
            $scope.rows_rej = result;

            if (result.length > 0) {
                $scope.load_data_of_rejected(0);
            }
        });
    }
    $scope.bindRejecteditems();



    $scope.load_data_of_rejected = function (index) {
        // alert($scope.rows[index].requestId);
        var val = 0;
        //   alert(index);
        if ($scope.rows_rej.length > 0) {
            for (var res in $scope.rows_rej) {
                if (val === index) {
                    // alert("in");
                    $scope.rows_rej[val].itemStatus = "active";
                }
                else {
                    $scope.rows_rej[val].itemStatus = "inactive";
                }
                val = val + 1;
            }

            $http({
                method: 'GET',
                url: 'requestlog/getsingle_requested_record',
                params: { abcsony: $scope.rows_rej[index].itemId }
            }).success(function (result) {
                $scope.rej_visible = true;
                $scope.Req2 = {};
                $scope.Req2.itemId = result[0].itemId;
                $scope.Req2.requestId = result[0].requestId;
                $scope.Req2.plant = result[1].plant;
                $scope.Req2.storage_Location = result[1].storage_Location;
                $scope.Req2.group = result[1].group;
                $scope.Req2.subGroup = result[1].subGroup;
                $scope.Req2.requester = result[1].requester;
                $scope.Req2.source = result[0].source;
                $scope.Req2.requestedOn = result[0].requestedOn;
                $scope.Req2.approver = result[0].approver;
                $scope.Req2.reason_rejection = result[0].reason_rejection;
                $scope.Req2.Materialtype = result[1].Materialtype;
                $scope.Req2.Industrysector = result[1].Industrysector;
                $scope.Req2.MaterialStrategicGroup = result[1].MaterialStrategicGroup;
                $scope.Req2.UnitPrice = result[0].UnitPrice;
            });
        }
        else {
            $scope.rej_visible = false;
        }
    };

    $scope.DelRequest = function (Itemid) {

        if (confirm("Are you sure, delete this record?")) {

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);

            $http({
                method: 'GET',
                url: '/MaterialRequest/DelRequest',
                params: { Itemid: Itemid }
            }).success(function (response) {
                if (response === true) {
                    $scope.Res = Itemid + " deleted";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.bindRejecteditems();
                } else {
                    $scope.Res = Itemid + " delete failed";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }

    };

    $scope.clickToOpen = function (Itemcode) {
        //  alert(Itemcode);
        $scope.cat = {};
        $http({
            method: 'GET',
            url: '/Search/GetItemDetail',
            params: { Itmcode: Itemcode }
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
            params: { Itmcode: Itemcode }
        }).success(function (response) {
            // alert(angular.toJson(response));
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
            content: jQuery('#cotentid'),
            reposition: false,
            repositionOnOpen: false
        }).open();

    };

    $scope.clickToremoverow = function (idx) {
        $scope.sResultbulk.splice(idx, 1);
    };
    $scope.clickToOpenBulkduplicates = function (src) {

      
        $scope.hidetb3 = false;
        $scope.sResultbulk_blktabl = {};
     
        $http({
            method: 'GET',
            url: '/MaterialRequest/getpossibledup',
            params: { sKey: src.source }
        }).success(function (response) {
            
          
            if (response != '') {
                $scope.sResultbulk_blktabl = response;
               
            } else {
                $scope.res = "Data not found";
                $scope.hidetb3 = true;
            }

        })

       //alert(angular.toJson(src.table_blk));
        new jBox('Modal', {
            width: 550,
            height: 500,
            blockScroll: false,
            animation: 'zoomIn',
            draggable: 'title',
            closeButton: true,

            //width: 1200,
            //blockScroll: false,
            //animation: 'zoomIn',
            //draggable: false,
            //overlay: true,
            closeButton: true,
            content: jQuery('#cotentid123'),
            reposition: false,
            repositionOnOpen: false
        }).open();
    };

    $scope.getpossibledup = function () {
        $timeout(function () { $scope.NotifiyRes = false; }, 3000);
        if ($scope.source1 === "" || $scope.source1 == undefined) {
            $scope.Res = "Please enter value to search"
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
           // $timeout(function () { $rootScope.NotifiyRes = false; }, 3000);
        }
        else {
            $scope.search = true;
            $scope.load = false;
            //   $scope.source1 = " " + $scope.source1;
            $scope.sResult = {};
            $http({
                method: 'GET',
                url: '/MaterialRequest/getpossibledup',
                params: { sKey: $scope.source1 }
            }).success(function (response) {
                //  alert(angular.toJson(response));
                if (response != '') {
                    $scope.hidetb1 = false;
                    $scope.sResult = response;
                    $scope.Res = "Search results : " + response.length + " items."
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.search = false;
                    $scope.load = true;
                } else {
                    $scope.sResult = null;
                    $scope.Res = "No item found"
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $scope.search = false;
                    $scope.load = true;

                }

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        }
    }
    $scope.rereq_btn = false;
    $scope.editRequest = function (itemId) {
       
        $http({
            method: 'GET',
            url: 'requestlog/getsingle_requested_record',
            params: { abcsony: itemId }
        }).success(function (result) {

            $scope.rereq_btn = true;
            $scope.add_button = false;

            $('#Sig').attr('class', 'active');
            $('#a1').attr('aria-expanded', 'true');

            $('#rej').attr('class', '');
            $('#a3').attr('aria-expanded', 'false');

            $('#clari').attr('class', '');
            $('#a4').attr('aria-expanded', 'false');

            $('#Single').attr('class', 'tab-pane active');
            $('#Rejected').attr('class', 'tab-pane');
            $('#Clarification').attr('class', 'tab-pane');

          //  alert(angular.toJson(result));

            $scope.itemId = result[0].itemId;
            $scope.ddlplant1 = result[0].plant;
            $scope.getStoragecode_name1();
            $scope.ddlstorage1 = result[0].storage_Location;
            $scope.ddlgroup1 = result[0].group;
         //   $scope.getsubgroupCode_Name1();
            $scope.ddlsubgroup1 = result[0].subGroup;
            $scope.source1 = result[0].source;
            $scope.remark1 = result[0].reason_rejection;
            $scope.Materialtype = result[0].Materialtype;
            $scope.Industrysector = result[0].Industrysector;
            $scope.MaterialStrategicGroup = result[0].MaterialStrategicGroup;
            $scope.UnitPrice = result[0].UnitPrice;


        });
    }

    //Clarification
    $scope.bindClarificationitems = function () {
        $http({
            method: 'GET',
            url: 'requestlog/getClarification_Records'
        }).success(function (result) {
            $scope.rows_clr = result;

            if (result.length > 0) {
                $scope.load_data_of_clarification(0);
            }
        });
    }
    $scope.bindClarificationitems();

    $scope.load_data_of_clarification = function (index) {
        // alert($scope.rows[index].requestId);
        var val = 0;
        //   alert(index);
        if ($scope.rows_clr.length > 0) {
            for (var res in $scope.rows_clr) {
                if (val === index) {
                    // alert("in");
                    $scope.rows_clr[val].itemStatus = "active";
                }
                else {
                    $scope.rows_clr[val].itemStatus = "inactive";
                }
                val = val + 1;
            }

            $http({
                method: 'GET',
                url: 'requestlog/getsingle_requested_record',
                params: { abcsony: $scope.rows_clr[index].itemId }
            }).success(function (result) {
                $scope.clr_visible = true;
                $scope.Req2 = {};
                $scope.Req2.itemId = result[0].itemId;
                $scope.Req2.requestId = result[0].requestId;
                $scope.Req2.plant = result[1].plant;
                $scope.Req2.storage_Location = result[1].storage_Location;
                $scope.Req2.group = result[1].group;
                $scope.Req2.subGroup = result[1].subGroup;
                $scope.Req2.requester = result[1].requester;
                $scope.Req2.source = result[0].source;
                $scope.Req2.requestedOn = result[0].requestedOn;
                $scope.Req2.approver = result[0].approver;
                $scope.Req2.reason_rejection = result[0].reason_rejection;
                $scope.Req2.Materialtype = result[1].Materialtype;
                $scope.Req2.Industrysector = result[1].Industrysector;
                $scope.Req2.MaterialStrategicGroup = result[1].MaterialStrategicGroup;
                $scope.Req2.UnitPrice = result[0].UnitPrice;
            });
        }
        else {
            $scope.clr_visible = false;
        }
    };

    // re-request

    $scope.ReRequest = function () {
   
        var rows1 = [];
        $timeout(function () { $scope.NotifiyRes = false; }, 5000);
        var planttitle = $.grep($scope.Plantoptions1, function (plant1) {
            return plant1.Plantcode == $scope.ddlplant1;
        })[0].Plantname;

        var storagetitle = $.grep($scope.Storageoptions1, function (storage1) {
            return storage1.Code == $scope.ddlstorage1;
        })[0].Title;
        //var grouptitle = $.grep($scope.Groupoptions1, function (group1) {
        //    return group1.code == $scope.ddlgroup1;
        //})[0].title;
        //var subgrouptitle = $.grep($scope.SubGroupoptions1, function (subgroup1) {
        //    return subgroup1.code == $scope.ddlsubgroup1;
        //})[0].title;
        //var approvertitle = $.grep($scope.Approveroptions1, function (approver1) {
        //    return approver1.Userid == $scope.ddlapprover1;
        //})[0].UserName;


      

        $scope.attachment.push($scope.fileList[0]);
        if ($scope.fileList[0] != null) {
            rows1.push({ itemId: $scope.itemId, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, source: $scope.source1, reason_rejection: $scope.remark1, attachment: $scope.fileList[0].name });
        }
        else {
            rows1.push({ itemId: $scope.itemId, plant: $scope.ddlplant1, storage_Location: $scope.ddlstorage1, group: $scope.ddlgroup1, subGroup: $scope.ddlsubgroup1, Materialtype: $scope.Materialtype, Industrysector: $scope.Industrysector, MaterialStrategicGroup: $scope.MaterialStrategicGroup, source: $scope.source1, reason_rejection: $scope.remark1, attachment: "" });
        }

       
        var form_singlerequest = new FormData();
        form_singlerequest.append('Rerequest', angular.toJson(rows1));
        form_singlerequest.append('files', $scope.fileList[0]);


        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/MaterialRequest/ReRequest',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: form_singlerequest
            //?plant=' + plantname + '&storage=' + storagename + '&group=' + groupname + '&subgroup=' + subgroupname + '&approver=' + approvername + '&source=' + source_desc

        }).success(function (response) {
            if (response === false) {
                $scope.Res = "Request failed";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.sResult = [];
            }
            else {

                $scope.bindRejecteditems();
                $scope.bindClarificationitems();

                $scope.Res = "Request sent successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $scope.ngdis1 = false;
                $scope.sResult = [];
                $scope.reset_reqform_multi();

                $scope.rereq_btn = false;
                $scope.add_button = true;

                $('.fileinput').fileinput('clear');
            }
        }).error(function (data) {

            $scope.Res = data.errors;
            $scope.Notify = "alert-danger";
            $scope.NotifiyRes = true;
            $scope.ngdis1 = false;
            $scope.sResult = [];
        });
    };

    //approved
    $scope.rows_app = [];
    $http({
        method: 'GET',
        url: 'requestlog/getApproved_Records'
    }).success(function (result) {
        $scope.rows_app = result;

        if (result.length > 0) {
            $scope.load_data_of_approved(0);
        }
    });
    $scope.load_data_of_approved = function (index) {
        // alert($scope.rows[index].requestId);
        var val = 0;
        //   alert(index);
        if ($scope.rows_app.length > 0) {
            for (var res in $scope.rows_app) {
                if (val === index) {
                    // alert("in");
                    $scope.rows_app[val].itemStatus = "active";
                }
                else {
                    $scope.rows_app[val].itemStatus = "inactive";
                }
                val = val + 1;
            }

            $http({
                method: 'GET',
                url: 'requestlog/getsingle_requested_record',
                params: { abcsony: $scope.rows_app[index].itemId }
            }).success(function (result) {
                $scope.app_visible = true;
                $scope.Req1 = {};
                $scope.Req1.itemId = result[0].itemId;
                $scope.Req1.requestId = result[0].requestId;
                $scope.Req1.plant = result[1].plant;
                $scope.Req1.storage_Location = result[1].storage_Location;
                $scope.Req1.group = result[1].group;
                $scope.Req1.subGroup = result[1].subGroup;
                $scope.Req1.requester = result[1].requester;
                $scope.Req1.source = result[0].source;
                $scope.Req1.requestedOn = result[0].requestedOn;
                $scope.Req1.approver = result[0].approver;
                $scope.Req1.cataloguer = result[1].cataloguer;
                $scope.Req1.Materialtype = result[1].Materialtype;
                $scope.Req1.Industrysector = result[1].Industrysector;
                $scope.Req1.MaterialStrategicGroup = result[1].MaterialStrategicGroup;
                $scope.Req1.UnitPrice = result[0].UnitPrice;
            });

        }
        else {
            $scope.app_visible = false;
        }
    };
    $scope.showuserMap = function (itmcode) {

        $http({
            method: 'GET',
            url: '/User/getItemstatusMap',
            params: { itmcde: itmcode }
        }).success(function (response) {
            $scope.itemstatusLst = response;

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
    }
    $scope.Loadpop2 = function () {
        new jBox('Tooltip', {
            attach: '#showstatus1',
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
    }
    $scope.Loadpop3 = function () {
     //   alert("hai");
        new jBox('Tooltip', {
            attach: '#showstatus2',
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
    }
    $scope.Loadpop4 = function () {
        //   alert("hai");
        new jBox('Tooltip', {
            attach: '#showstatus3',
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
    }
});