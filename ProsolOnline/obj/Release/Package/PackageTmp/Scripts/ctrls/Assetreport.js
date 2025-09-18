var Asset = angular.module('ProsolApp', ['cgBusy']);


Asset.controller('Assetreportcontroller', function ($scope, $http, $timeout, $window, $filter) {



    $scope.desctab = "active";
    $scope.codetab = "";

    $scope.getpvusers = function () {
        // GetPVUsers
       
        $http({
            method: 'GET',
            url: '/FAR/GetPVusers'
        }).success(function (response) {

            $scope.pvuserslist = response;

        }).error(function (data, status, headers, config) {
            // alert("error");
        });

      
    }
    $scope.getpvusers();

    

    $scope.BindBusinessList = function (far) {

        $http({
            method: 'GET',
            url: '/FAR/GetFarMaster'
        }).success(function (response) {

            $scope.FAR_Master = [];
            $scope.FAR_Master = response;
            $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
            console.log(response)
            console.log(far)
            console.log($scope.FAR_Master)
            if (far != null && far != "" && far != undefined) {
                $scope.BtnFARmodel = far;
                $scope.asset.FARId = far;
                console.log($scope.FAR_Master);

                $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.Region)));
                $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': $scope.asset.FARId }).map(i => i.AssetDesc)));

                $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
                $scope.asset.Region = $scope.RegionMaster.join('');

                console.log($scope.asset.FARId);
                console.log($scope.RegionMaster);
                console.log($scope.AssetDescMaster);
                $scope.searchFar = "";
            }

        }).error(function (data, status, headers, config) {
            //alert("error");
        });
    };
    $scope.BindBusinessList();

    $("#txtFrom").datepicker({
        numberOfMonths: 1,
        onSelect: function (selected) {
            $scope.Fromdate = $('#txtFrom').val();
            var dt = new Date(selected);
            dt.setDate(dt.getDate());
            $("#txtTo").datepicker("option", "minDate", dt);
            // $scope.Todate = $('#txtTo').val();
        }
    });
    $scope.NotifiyResclose = function () {
        $('#divNotifiy').attr('style', 'display: none');
    }

    $("#txtTo").datepicker({ maxDate: new Date() });


    $scope.LoadMaster = function () {
        $http({
            method: 'GET',            url: '/FAR/GetBusiness'
        }).success(function (response) {

            $scope.getBusiness = response.Businesses;
            $scope.getMajorClass = response.MajorClasses;
            $scope.getMinorClass = response.MinorClasses;
            $scope.getRegion = response.Regions;
            $scope.getcity = response.Cities;
            $scope.getarea = response.Areas;
            $scope.getSub = response.SubAreas;
            //$scope.getloc = response.Locations;
            //$scope.getidenti = response.Identifiers;
            //$scope.getequipclass = response.EquipmentClasses;
            //$scope.getequiptype = response.EquipmentTypes;


        }).error(function (data, status, headers, config) {            //alert("error");        });
    };
    $scope.LoadMaster();

    $scope.findcontrol = function () {
        $scope.UserName = "";
        $scope.reassigngetuser = "";
        $scope.get_reassigndata = "";
        if ($scope.role != "" && $scope.role != undefined) {

            $http({
                method: 'GET',
                url: '/FAR/getuser',
                params: { role: $scope.role }

            }).success(function (response) {
                $scope.getuser = response;
               // $scope.PVreassigngetuser = response;
            });
        } else {
            $scope.getuser = "";
            $scope.reassigngetuser = "";
            $scope.show2 = false;
        }
    }
    // Track the load data......
    $scope.PV = false;
    $scope.trackingload = function () {

      
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 30000);



        var formData = new FormData();
        formData.append("FARId", $scope.BtnFARmodel);
        //formData.append("MajorClass", $scope.MajorClass);
        //formData.append("MinorClass", $scope.MinorClass != undefined ? $scope.MinorClass : "");
        //formData.append("SubArea", $scope.SubArea);
        formData.append("Role", $scope.role);
        formData.append("User", $scope.user != undefined ? $scope.user : "");
        formData.append("Status", $scope.status != undefined ? $scope.status:"");
        formData.append("From", $scope.Fromdate != undefined ? $scope.Fromdate : "");
        formData.append("To", $scope.Todate != undefined ? $scope.Todate : "");
        $scope.cgBusyPromises = $http({
            method: 'POST',
            url: '/FAR/TrackloadAsset',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,
        }).success(function (response) {

            if (response != "") {

                $scope.tables = [];
                $scope.tables = response;

                $scope.len = response.length;

                $scope.Res = "Loaded successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            } else {
                $scope.Res = "No data found";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        });
    }

    $scope.Attr = 0;

    //$scope.attrValue = function (chkAttr) {
    //    alert($scope.Attr)
    //};

    $scope.exportAsset = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);
        

        $scope.status = $scope.status != undefined ? $scope.status : "";
        $scope.user = $scope.user != undefined ? $scope.user : "";
        $scope.Fromdate = $scope.Fromdate != undefined ? $scope.Fromdate : "";
        $scope.Todate = $scope.Todate != undefined ? $scope.Todate : "";
        $scope.MinorClass=$scope.MinorClass != undefined ? $scope.MinorClass : ""

        $window.location = '/FAR/DownloadAssetMulticode?FARId=' + $scope.BtnFARmodel + '&Role=' + $scope.role + '&User=' + $scope.user + '&Status=' + $scope.status + '&Fromdate=' + $scope.Fromdate + '&Todate=' + $scope.Todate + '&Attr=' + $scope.Attr;

     
        }
       
    $scope.assetno = "";

    //Code Search

    $scope.exportAssetCodes = function () {


        if ($scope.assetno !== null && $scope.assetno !== "") {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            if ($scope.assetno != "" && $scope.assetno != null && $scope.assetno != undefined) {
                var formData = new FormData();
                formData.append("assetno", $scope.assetno);
                $scope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/FAR/DownloadTrackCodes',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {
                    console.log(response);
                    if (response > 0) {
                        $window.location = '/FAR/TrackloadAssetCodes';
                        $scope.assetno = "";
                    }
                    else {
                        $scope.Res = "Please enter valid asset no (or) unique id, if more than one code separate by comma(,)";
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                });
            }
        };
    }


});


Asset.controller('Assetsearchcontroller', function ($scope, $http, $timeout, $window, $filter) {

    $scope.sResult = null;
    $scope.cat = {};
    $scope.attr = {};

    //Search

    $scope.SearchItem = function () {
        $timeout(function () { $scope.NotifiyRes = false; }, 5000);
        //  $timeout(function () { $scope.NotifiyRes = false; }, 30000);

        $scope.sResult = null;
        $scope.cgBusyPromises = $http({
            method: 'GET',
            url: '/Search/AssetSearchResult',
            params: { sKey: $scope.searchkey, sBy: $scope.search }
        }).success(function (response) {
            if (response != '') {
                $scope.sResult = response;
                //console.log($scope.sResult)
                $scope.Res = "Search results : " + response.length + " items."
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
            } else {
                $scope.sResult = null;
                $scope.Res = "No item found"
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });

    };

    //Single Item


    $scope.ClearItem = function () {

        $scope.searchkey = null;
        $scope.cat = null;
        $scope.erp = null;
        $scope.sResult = null;
        $scope.reset();
    }
    $scope.clickToOpen = function (UniqueId) {

        $scope.cgBusyPromises = $http({
            method: 'GET',
            url: '/FAR/GetAssetinfo',
            params: { UniqueId: UniqueId }
        }).success(function (response) {
            if (response != '') {
                $scope.cat = response;
            } else {
                $scope.cat = null;

            }

        }).error(function (data, status, headers, config) {
            // alert("error");
        });
        $scope.attr = {};
        $http({
            method: 'GET',
            url: '/FAR/GetAttributeinfo',
            params: { UniqueId: UniqueId }
        }).success(function (response) {
            if (response != '') {
                $scope.attr = response;
                console.log($scope.attr)

            } else {
                $scope.attr = null;

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


    $scope.reset = function () {

        $scope.form.$setPristine();
    }

});

Asset.controller('FarDashboardcontroller', function ($scope, $http, $timeout, $window, $filter) {


    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    $scope.yearList = [
        { year: "2022" },
        { year: "2023" },
        { year: "2024" },
        { year: "2025" },
        { year: "2026" },
        { year: "2027" },
        { year: "2028" },
        { year: "2029" },
        { year: "2030" },
        { year: "2031" },
        { year: "2032" },
        { year: "2033" },
        { year: "2034" },
        { year: "2035" },
        { year: "2036" },
        { year: "2037" },
        { year: "2038" },
        { year: "2039" },
        { year: "2040" },
        { year: "2041" }];
  
    $scope.getDashboard = function () {
        // GetPVUsers

        $http({
            method: 'GET',
            url: '/FAR/GetFarMaster'
        }).success(function (response) {

            $scope.FarInfo = response;

            const uniqueFARIds = new Set($scope.FarInfo.map(item => item.FARId));
            const uniqueRegions = new Set($scope.FarInfo.map(item => item.Region));
            const uniqueAssetDescs = new Set($scope.FarInfo.map(item => item.AssetDesc));

            $scope.Far = uniqueFARIds.size;
            $scope.Region = uniqueRegions.size;
            $scope.AssetDesc = uniqueAssetDescs.size;


        }).error(function (data, status, headers, config) {
            // alert("error");
        });


    }
    $scope.getDashboard();


    $scope.findcontrol = function () {
        $scope.UserName = "";
        $scope.reassigngetuser = "";
        $scope.get_reassigndata = "";
        if ($scope.role != "" && $scope.role != undefined) {

            $http({
                method: 'GET',
                url: '/FAR/getuser',
                params: { role: $scope.role }

            }).success(function (response) {
                $scope.getuser = response;
                // $scope.PVreassigngetuser = response;
            });
        } else {
            $scope.getuser = "";
            $scope.reassigngetuser = "";
            $scope.show2 = false;
        }
    }
    // Track the load data......
    $scope.PV = false;
    $scope.trackingload = function () {


        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 30000);



        var formData = new FormData();
        formData.append("MajorClass", $scope.MajorClass);
        formData.append("SubArea", $scope.SubArea);
        formData.append("Role", $scope.role);
        formData.append("User", $scope.user != undefined ? $scope.user : "");
        formData.append("Status", $scope.status != undefined ? $scope.status : "");
        formData.append("From", $scope.Fromdate != undefined ? $scope.Fromdate : "");
        formData.append("To", $scope.Todate != undefined ? $scope.Todate : "");
        $scope.cgBusyPromises = $http({ 
            method: 'POST',
            url: '/FAR/TrackloadAsset',
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity,
            data: formData,
        }).success(function (response) {

            if (response != "") {

                $scope.tables = [];
                $scope.tables = response;

                $scope.len = response.length;

                $scope.Res = "Loaded successfully";
                $scope.Notify = "alert-info";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');

            } else {
                $scope.Res = "No data found";
                $scope.Notify = "alert-danger";
                $scope.NotifiyRes = true;
                $('#divNotifiy').attr('style', 'display: block');
            }
        });
    }
    $scope.exportAsset = function () {
        $timeout(function () {
            $('#divNotifiy').attr('style', 'display: none');
        }, 5000);


        $scope.status = $scope.status != undefined ? $scope.status : "";
        $scope.user = $scope.user != undefined ? $scope.user : "";
        $scope.Fromdate = $scope.Fromdate != undefined ? $scope.Fromdate : "";
        $scope.Todate = $scope.Todate != undefined ? $scope.Todate : "";

        $window.location = '/FAR/DownloadAssetMulticode?Major=' + $scope.MajorClass + '&SubArea=' + $scope.SubArea + '&Role=' + $scope.role + '&User=' + $scope.user + '&Status=' + $scope.status + '&Fromdate=' + $scope.Fromdate + '&Todate=' + $scope.Todate + '&Attr=' + $scope.Attr;


    }

    //Pie Chart

    //$scope.bindItems = function () {
    //    $scope.promise = $http({
    //        method: 'GET',
    //        url: '/FAR/GetFarMaster'
    //    }).success(function (response) {

    //        $scope.Master = response;
    //        $scope.FARMaster = Array.from(new Set(response.map(i => i.FARId)));
    //        $scope.RegionMaster = Array.from(new Set(response.map(i => i.Region)));
    //        $scope.AssetDescMaster = Array.from(new Set(response.map(i => i.AssetDesc)));
    //        //$scope.countList = $scope.countList.filter(item => item.itmsCount !== "0");
    //        $scope.FARCount = $scope.FARMaster.length;
    //        $scope.RegionCount = $scope.RegionMaster.length;
    //        $scope.AssetDescCount = $scope.AssetDescMaster.length;
    //        console.log($scope.FARCount)
    //        console.log($scope.RegionCount)
    //        console.log($scope.AssetDescCount)

    //        var labels = ["FAR ID", "Region", "Asset Description"];
    //        var data = [$scope.FARCount, $scope.RegionCount, $scope.AssetDescCount];
    //        //$scope.total = 
    //        var pieChart = document.getElementById('pie').getContext('2d');

    //        var myPieChart = new Chart(pieChart, {
    //            type: 'pie',
    //            data: {
    //                labels: labels,
    //                datasets: [{
    //                    data: data,
    //                    backgroundColor: [
    //                        "#0000FF", "#00008B", "#0000CD", "#0000FA",
    //                        "#000080", "#191970", "#1E90FF", "#20B2AA",
    //                        "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
    //                        "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
    //                        "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
    //                        "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
    //                        "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
    //                        "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
    //                        "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
    //                        "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
    //                        "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
    //                        "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
    //                    ],
    //                    borderColor: [
    //                        "#0000FF", "#00008B", "#0000CD", "#0000FA",
    //                        "#000080", "#191970", "#1E90FF", "#20B2AA",
    //                        "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
    //                        "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
    //                        "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
    //                        "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
    //                        "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
    //                        "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
    //                        "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
    //                        "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
    //                        "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
    //                        "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
    //                    ],
    //                    borderWidth: 1
    //                }],
    //            },
    //            options: {
    //                animation: {
    //                    duration: 1500,
    //                    easing: 'easeInOutQuart'
    //                },
    //                plugins: {
    //                    legend: false,
    //                    tooltip: {
    //                        callbacks: {
    //                            label: function (tooltipItem, data) {
    //                                var dataset = data.datasets[tooltipItem.datasetIndex];
    //                                var value = dataset.data[tooltipItem.index];
    //                                return dataset.label + ': ' + value;
    //                            }
    //                        },
    //                        custom: function (tooltipModel) {
    //                            var tooltipEl = document.getElementById('custom-tooltip');
    //                            if (!tooltipEl) {
    //                                tooltipEl = document.createElement('div');
    //                                tooltipEl.id = 'custom-tooltip';
    //                                tooltipEl.classList.add('custom-tooltip');
    //                                document.body.appendChild(tooltipEl);
    //                            }
    //                            if (tooltipModel.opacity === 0) {
    //                                tooltipEl.style.opacity = 0;
    //                                return;
    //                            }

    //                            tooltipEl.innerHTML = tooltipModel.body[0].lines[0];
    //                            tooltipEl.style.fontSize = '16px';
    //                            tooltipEl.style.left = tooltipModel.caretX + 'px';
    //                            tooltipEl.style.top = tooltipModel.caretY + 'px';
    //                            tooltipEl.style.opacity = 1;
    //                        }

    //                    }
    //                }
    //            }
    //        });

    //    }).error(function (data, status, headers, config) {

    //    });
    //};
    //$scope.bindItems();



    // Bar Average Chart

    //Progress Delay Chart

    var currentDate = new Date();
    $scope.pendYear = currentDate.getFullYear().toString();
    $scope.pendList = [];
    $scope.bindPendItems = function () {
        $scope.promise = $http({
            method: 'GET',
            url: '/FAR/GetPendItems',
            params: { year: $scope.pendYear }
        }).success(function (response) {

            $scope.pendList = response.data;
            var currentDate = new Date();
            var currentMonth = currentDate.getMonth() + 1;

            //filteredData = $scope.pendList.filter(item => {
            //    var itemMonth = item.month;
            //    return itemMonth === $scope.slocMonth;
            //});

            var yearsArray = response.yearsList.map(item => item.fyear);
            var uniqueYears = yearsArray.filter(function (value, index, self) {
                return self.indexOf(value) === index;
            }).sort();

            $scope.yearList = uniqueYears.map(function (year) {
                return { year: year };
            });
            console.log($scope.yearList)
            var labels = $scope.pendList.map(item => item.month);
            var data = $scope.pendList.map(item => parseInt(item.itmsCount));

            var progChart = document.getElementById('prog').getContext('2d');
            if ($window.progress != undefined)
                $window.progress.destroy();
            $window.progress = new Chart(progChart, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Data',
                        data: data,
                        borderColor: '#306897',
                        backgroundColor: 'lightblue',
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Chart.js Line Chart'
                        }
                    }
                },
            });
        }).error(function (data, status, headers, config) {

        });
    };
    $scope.bindPendItems();

    //Donut Chart

    $scope.bindOpenItems = function () {
        $scope.promise = $http({
            method: 'GET',
            url: '/FAR/getAllOpenReqs'
        }).success(function (response) {

            $scope.itmsList = response;

            var labels = $scope.itmsList.map(item => item.req);
            var data = $scope.itmsList.map(item => item.itmsCount);

            var donutChart = document.getElementById('donut').getContext('2d');

            var myDonutChart = new Chart(donutChart, {
                type: 'doughnut',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: [
                            "#0000FF", "#00008B", "#0000CD", "#0000FA",
                            "#000080", "#191970", "#1E90FF", "#20B2AA",
                            "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                            "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                            "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                            "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                            "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                            "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                            "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                            "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                            "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                            "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                        ],
                        borderColor: [
                            "#0000FF", "#00008B", "#0000CD", "#0000FA",
                            "#000080", "#191970", "#1E90FF", "#20B2AA",
                            "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                            "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                            "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                            "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                            "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                            "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                            "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                            "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                            "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                            "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                        ],
                        borderWidth: 1
                    }],
                },
                options: {
                    animation: {
                        duration: 1500,
                        easing: 'easeInOutQuart'
                    },
                    plugins: {
                        legend: false,
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem, data) {
                                    var dataset = data.datasets[tooltipItem.datasetIndex];
                                    var value = dataset.data[tooltipItem.index];
                                    var label = data.labels[tooltipItem.index];
                                    return dataset.label + ': ' + value;
                                }
                            }
                        }
                    }
                }
            });
        }).error(function (data, status, headers, config) {

        });
    };
    $scope.bindOpenItems();


    //Bar Chart

    var currentDate = new Date();
    $scope.totalYear = currentDate.getFullYear().toString();
    $scope.totalList = [];
    $scope.bindTotalItems = function () {
        $scope.promise = $http({
            method: 'GET',
            url: '/FAR/getTotalReqs',
            params: { tyear: $scope.totalYear }
        }).success(function (response) {
            $scope.totalList = response.data;

            var yearsArray = response.yearsList.map(item => item.year);
            var uniqueYears = yearsArray.filter(function (value, index, self) {
                return self.indexOf(value) === index;
            }).sort();

            $scope.yearList = uniqueYears.map(function (year) {
                return { year: year };
            });
            console.log(yearsArray)
            var monthLabels = $scope.totalList.map(item => item.month);
            var data1 = $scope.totalList.map(item => item.rep);
            //var data2 = $scope.totalList.map(item => item.nonrep);
            //var average = $scope.totalList.map(item => item.avg);

            var data1Total = data1.reduce(function (accumulator, currentValue) {
                return accumulator + parseInt(currentValue);
            }, 0);

            var average1 = data1Total / data1.length;

            var initialDisplayMonths = 5;
            var visibleMonthLabels = monthLabels.slice(-initialDisplayMonths);

            var baravgChart = document.getElementById('baravg').getContext('2d');

            //var myBaravgChart = new Chart(baravgChart, {
            //    type: 'bar',
            //    data: {
            //        labels: monthLabels,
            //        datasets: [
            //            {
            //                label: 'Replenishment',
            //                data: data1,
            //                backgroundColor: '#306897',
            //                borderColor: '#306857',
            //                borderWidth: 1
            //            },
            //            {
            //                label: 'NonReplenishment',
            //                data: data2,
            //                backgroundColor: 'lightblue',
            //                borderColor: 'lightblue',
            //                borderWidth: 1
            //            },
            //            {
            //                type: 'line',
            //                label: 'Average',
            //                data: Array(monthLabels.length).fill(average1),
            //                borderColor: 'rgba(0, 0, 0, 0.8)',
            //                borderWidth: 2,
            //                fill: false
            //            }
            //        ]
            //    },
            //    options: {
            //        scales: {
            //            x: {
            //             display: false
            //            },
            //            y: {
            //                grid: {
            //                    borderDash: [2], 
            //                    color: 'rgba(0, 0, 0, 0.2)' 
            //                }
            //            }
            //        },
            //        plugins: {
            //            legend: {
            //                display: true,
            //                position: 'bottom'
            //            }
            //        }
            //    }
            //});
            if ($window.myBaravgChart != undefined)
                $window.myBaravgChart.destroy();
            $window.myBaravgChart = new Chart(baravgChart, {
                type: 'bar',
                data: {
                    labels: monthLabels,
                    datasets: [
                        {
                            label: 'Data',
                            data: data1,
                            backgroundColor: '#306897'
                            //},
                            //{
                            //    label: 'NonReplenishment',
                            //    data: data2,
                            //    backgroundColor: '#191970'
                        }
                    ]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            //myBaravgChart.data.datasets.push({
            //    type: 'line',
            //    label: 'Average',
            //    data: average,
            //    borderColor: 'lightblue',
            //    borderWidth: 2,
            //    fill: false,
            //    borderDash: [5, 5]
            //});

            myBaravgChart.update();
        }).error(function (data, status, headers, config) {
            // handle error
        });
    };
    $scope.bindTotalItems();

    //All Open Requests

    $scope.bindAllItems = function () {
        $http({
            method: 'GET',
            url: '/FAR/getAllOpenReqs'
        }).then(function (response) {
            var rawData = response.data;
            console.log(rawData);
            var summarizedData = {};
            rawData.forEach(function (item) {
                var req = item.req;
                if (!summarizedData[req]) {
                    summarizedData[req] = 0;
                }
                summarizedData[req] += parseInt(item.itmsCount);
            });

            $scope.summaryList = [];
            for (var req in summarizedData) {
                var formattedCount = summarizedData[req].toLocaleString();
                $scope.summaryList.push({ req: req, itmsCount: formattedCount });
            }
            console.log($scope.summaryList)
            $scope.pen = $scope.summaryList[0].itmsCount;
            $scope.pv = $scope.summaryList[1].itmsCount;
            $scope.cat = $scope.summaryList[2].itmsCount;
            $scope.qc = $scope.summaryList[3].itmsCount;
            $scope.rlsd = $scope.summaryList[4].itmsCount;
            //$scope.total = $scope.summaryList[5].itmsCount;
        }).catch(function (error) {
        });
    };

    $scope.bindAllItems();

});


Asset.controller('AssetBIcontroller', function ($scope, $http, $timeout, $window, $filter) {

    //New

    $scope.role = "Cataloguer";
    $scope.usersData = [];

    Chart.register(ChartDataLabels);

    $scope.changeRole = function () {
        $http.get('/FAR/GetUsersCounts', { params: { role: $scope.role } })
            .then(function (res) {
                //console.log(res);
                $scope.usersData = res.data;
            });
    }
    $scope.changeRole();
    $scope.dashboardStats = [
        { title: 'MAXIMO ASSETS', value: 124377 },
        { title: 'PV COMPLETED', value: 111569 },
        { title: 'PV YET TO COMPLETE', value: 12808 },
        { title: 'PV COMPLETED - NEW ASSETS', value: 24498 },
        { title: 'TOTAL PV COMPLETED', value: 152606 },
        { title: 'OVERALL ASSETS', value: 165414 },
        { title: 'UNIQUE ASSETS', value: 95460 },
        { title: 'SUBMITTED ASSET', value: 71714 }
    ];

    //Gauge 1

    const pvCtx = document.getElementById('gaugePVChart').getContext('2d');
    const pvPercentage = 91.24;

    const gaugePVChart = new Chart(pvCtx, {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [pvPercentage, 100 - pvPercentage],
                backgroundColor: ['#2a3d66', '#eeeeee'],
                borderWidth: 0,
                cutout: '70%',  
                circumference: 180,
                rotation: 270
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false },
                tooltip: { enabled: false },
                datalabels: false
            }
        }
    });

    document.getElementById('percentagePVLabel').textContent = pvPercentage.toFixed(2) + '%';


    const catCtx = document.getElementById('gaugeCATChart').getContext('2d');
    const catPercentage = 91.24;
    
    //Gauge 2

    const gaugeCATChart = new Chart(catCtx, {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [catPercentage, 100 - catPercentage],
                backgroundColor: ['#2a3d66', '#eeeeee'],
                borderWidth: 0,
                cutout: '70%',
                circumference: 180,
                rotation: 270
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false },
                tooltip: { enabled: false },
                datalabels: false
            }
        }
    });

    document.getElementById('percentageCATLabel').textContent = catPercentage.toFixed(2) + '%';

    //Dough-Nut

    $timeout(function () {
        var ctx = document.getElementById("catalogueChart").getContext("2d");

        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: [
                    "OVERALL ASSETS",
                    "PV COMPLETED",
                    "CATALOGUE COMPLETED",
                    "CATALOGUING IN PROGRESS"
                ],
                datasets: [{
                    data: [165943, 154177, 145109, 9068],
                    backgroundColor: [
                        "#2e4a75", 
                        "#00479d",
                        "#0060b0",
                        "#7da2bf"
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                cutoutPercentage: 70,
                responsive: false,
                plugins: {
                    datalabels: {
                        color: 'white',
                        font: {
                            weight: 'bold',
                            size: 14
                        },
                        formatter: function (value) {
                            return value;
                        }
                    },
                },
                legend: {
                    position: 'bottom',
                    labels: {
                        fontSize: 10,
                        boxWidth: 10
                    }
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var value = dataset.data[tooltipItem.index];
                            return data.labels[tooltipItem.index] + ": " + value;
                        }
                    }
                },
            }
        });
    }, 100);


    



});