(function () {
    'use strict';


    var app = angular.module('ProsolApp', ['cgBusy', 'angular.filter', 'datatables']);

  
    //Bulkdata
    app.controller('AssetBulkController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {


        //$scope.cat.Exceptional = false;
        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
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
                    $('#divNotifiy').attr('style', 'display: block');
                   
                    $scope.$apply();

                  
             
                }
            }
        };
        $scope.ShowHide = false;
        $scope.LoadFileData1 = function (files) {

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
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.$apply();



                }
            }
        };
        $scope.promise = $scope.AssetBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/AssetBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                     
                  

                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = "Items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.promise = $scope.AssetProxyBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/AssetProxyBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = data + " items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');


                });
            };
        }

        $scope.promise = $scope.BOMBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BOMBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');



                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = data + " items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.LoadFileData2 = function (files) {

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
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.$apply();



                }
            }
        };
        $scope.promise = $scope.AttriBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('Attribute', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/AttriBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');



                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = data + " items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }
                        
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.promise = $scope.Vendorbulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('Attribute', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/VendorBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');



                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = data + " items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }
                        
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.ShortLong = function () {
            if ($scope.files && $scope.files[0]) {
                // Reset the progress bar and time
                $scope.ShowHide = true;
                $scope.uploadProgress = 0;
                $scope.uploadTimeTaken = 0;

                var startTime = new Date().getTime(); // Track the start time

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                console.log('File being uploaded: ', $scope.files[0]);
                console.log('File size: ', $scope.files[0].size); // Log file size for debugging

                // Start the HTTP request
                $scope.promise = $http({
                    url: "/FAR/ShortLong", // Endpoint for file upload
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    data: formData,
                    transformRequest: angular.identity,
                    // Custom XHR to monitor progress
                    xhr: function () {
                        var xhr = new XMLHttpRequest();

                        // Progress event
                        xhr.upload.addEventListener("progress", function (e) {
                            if (e.lengthComputable) {
                                let percentComplete = Math.round((e.loaded / e.total) * 100);

                                // Always increase progress by +5, but cap at 100%
                                let adjustedProgress = Math.min(percentComplete + 5, 100);

                                console.log('Raw Progress:', percentComplete);
                                console.log('Adjusted Progress (+5):', adjustedProgress);

                                // Update Angular model and trigger $apply
                                $scope.uploadProgress = adjustedProgress;
                                $scope.$apply(); // Ensure the view is updated with new progress
                            } else {
                                console.log('Progress event fired, but size is not computable');
                            }
                        }, false);

                        // Event listeners for debugging
                        xhr.upload.addEventListener('loadstart', function (e) {
                            console.log('Upload started...');
                        });

                        xhr.upload.addEventListener('loadend', function (e) {
                            console.log('Upload finished.');
                        });

                        xhr.upload.addEventListener('error', function (e) {
                            console.error('Upload error', e);
                        });

                        xhr.upload.addEventListener('abort', function (e) {
                            console.error('Upload aborted', e);
                        });

                        return xhr;
                    }
                }).then(function (response) {
                    // On success, finalize the progress bar and calculate the time taken
                    const endTime = new Date().getTime();
                    $scope.uploadProgress = 100;  // Set progress to 100%
                    $scope.uploadTimeTaken = ((endTime - startTime) / 1000).toFixed(2);  // Calculate time taken in seconds

                    // Handle the server response
                    var data = response.data;
                    if (data === 0) {
                        $scope.Res = "Records already exist";
                    } else {
                        $scope.Res = data + " Records uploaded successfully";
                    }

                    // Show success notification
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear'); // Clear the file input

                    // Auto-hide the progress bar after 4 seconds
                    setTimeout(() => {
                        $scope.ShowHide = false;   // Hide progress bar
                        $scope.$apply();           // Trigger Angular digest cycle
                    }, 4000);

                }, function (error) {
                    // Handle error case
                    $scope.uploadProgress = 0;  // Reset progress bar
                    $scope.ShowHide = false;    // Hide progress bar
                    $scope.Res = "Please validate your Excel file";  // Error message
                    $scope.Notify = "alert-danger";  // Show error notification
                    $scope.NotifiyRes = true;
                });
            } else {
                console.error('No file selected');
            }
        };

        $scope.promise =  $scope.Rework = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/Rework",
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

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }

        $scope.promise = $scope.Dashboard = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BulkDashboard",
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

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.promise = $scope.Swap = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BulkSwap",
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

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.promise = $scope.Additional = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BulkAdditional",
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

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
        $scope.promise = $scope.BulkLocation = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BulkLocation",
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

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }
    })
    app.controller('AssetQRController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {

        $scope.generatedData = [],
            $scope.currentPage = 1
            , $scope.numPerPage = 10
            , $scope.maxSize = 5;

        $scope.ddlItems = function () {
            $scope.chkSelected = false;
            $scope.numPerPage = $scope.selecteditem;
            $scope.slcteditem = [];
        };

        $scope.TagNo = "";
        $scope.TagImage = "";

        //$scope.cat.Exceptional = false;
        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
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
                    $('#divNotifiy').attr('style', 'display: block');
                   
                    $scope.$apply();

                  
             
                }
            }
        };
        $scope.ShowHide = false;
        $scope.LoadFileData1 = function (files) {

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
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.$apply();



                }
            }
        };
        $scope.LoadFileData2 = function (files) {

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
                    $('#divNotifiy').attr('style', 'display: block');

                    $scope.$apply();



                }
            }
        };

        $scope.promise = $scope.AssetBulkQR = function () {
            if ($scope.files[0] != null) {

                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/BulkQR",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).then(function (response) {
                    var data = response.data;
                    const qrData = data.data || [];

                    $scope.len = qrData.length;
                    $scope.generatedData = qrData;
                    $scope.ShowHide = false;

                    if (qrData.length === 0) {
                        $scope.Res = "QR Generation Failed";
                    } else {
                        $scope.Res = qrData.length + " QR Successfully Generated";
                    }

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    $('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).catch(function (error) {
                    console.error("Error uploading file:", error);
                    $scope.ShowHide = false;
                    $scope.Res = error.data?.message || "Something went wrong. Please validate your Excel file.";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                });
            }
        };


        $scope.downloadPDF = function () {
            const { jsPDF } = window.jspdf;
            var pdf = new jsPDF('p', 'mm', 'a4');

            const pageWidth = 210;
            const pageHeight = 297;

            const columns = 4;
            const rows = 4;
            const padding = 5;

            const usableWidth = pageWidth - padding * (columns + 1);
            const usableHeight = pageHeight - padding * (rows + 1);
            const squareSize = Math.min(usableWidth / columns, usableHeight / rows); 

            const images = $scope.generatedData;

            let loaded = 0;

            images.forEach((item, index) => {
                const img = new Image();
                img.crossOrigin = "ADPORTS";

                img.onload = function () {
                    const pageIndex = Math.floor(index / (columns * rows));
                    const posInPage = index % (columns * rows);

                    const row = Math.floor(posInPage / columns);
                    const col = posInPage % columns;

                    if (index > 0 && posInPage === 0) {
                        pdf.addPage();
                    }

                    const x = padding + col * (squareSize + padding);
                    const y = padding + row * (squareSize + padding);

                    pdf.addImage(img, 'PNG', x, y, squareSize, squareSize - 10); 

                    //pdf.setFontSize(8);
                    //pdf.text("Asset: " + item.assetNo, x + 2, y + squareSize - 2);

                    loaded++;
                    if (loaded === images.length) {
                        pdf.save("adp-tags.pdf");
                    }
                };

                img.src = "/QRImages/" + item.tagURL;
            });
        };

    })
    app.controller('AssetmasterController', function ($scope, $http, $timeout, $window, $filter, $location, $rootScope) {
        $scope.cat = {};
        $scope.AssetdataList = [];
        $scope.desctab = "active";
        $scope.desctab1 = "active";
        $scope.dis = false;
        $scope.sts1 = false;
        $scope.showlist = false;
        $scope.buttons = false;
        $scope.searchFar = "";
        $scope.BtnFARmodel1 = "";
        $scope.source = "Existing";
        $scope.cat.Exceptional = false;
        //$scope.isHttpsValid = true;
        //$scope.touched = false;

        //$scope.checkHttpsUrl = function () {
        //    var url = $scope.asset.Soureurl || '';
        //    $scope.isHttpsValid = /^https:\/\//.test(url);
        //};

        $scope.equipment = {
            BOMId: "4939848374",
            Desc: "fsdfsfew",
            Quantity: "2",
            BOM: [
                {
                    Materialcode: "782472",
                    Description: "gyefgwhfbbsj,fdzb",
                    Quantity: "21",
                    open: false, 
                    children: [] 
                },
                {
                    Materialcode: "865242",
                    Description: "gyefgwhfbbsj,fdzb",
                    Quantity: "1",
                    open: false,
                    children: [] 
                }
            ]
        };

        $scope.toggle = function (item) {
            item.open = !item.open;
        };

        $scope.LoadData = function () {         

            if ($scope.sNoun == null) {

                $rootScope.cgBusyPromises =    $http({
                    method: 'GET',
                    url: '/FAR/GetAssetDataList'
                }).success(function (response) {

                    $scope.AssetdataList = response;
                    $scope.AssetNo_List = $filter('filter')($scope.AssetdataList, { 'FARId': $scope.asset.FARId });
                    console.log($scope.AssetNo_List)
                    $scope.AssetNoList = Array.from(new Set($scope.AssetNo_List.map(i => i.AssetNo)));
                    $scope.saveditems = ($filter('filter')($scope.AssetdataList, { 'ItemStatus': '5' })).length;
                    $scope.balanceitems = ($filter('filter')($scope.AssetdataList, { 'ItemStatus': '4' })).length;

                 

                    if ($scope.saveditems == 0 && $scope.balanceitems == 0) {

                        $scope.saveditems = ($filter('filter')($scope.AssetdataList, { 'ItemStatus': '3' })).length;
                        $scope.balanceitems = ($filter('filter')($scope.AssetdataList, { 'ItemStatus': '2' })).length;
                    }
                   
                }).error(function (data, status, headers, config) {
                    
                });

              
            }
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
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.$apply();
                }
            }
        };
        //Site
        $scope.BindSiteList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetSiteMaster'
            }).success(function (response) {
                $scope.SiteMaster = response;
                $scope.SiteList = Array.from(new Set(response.map(i => i.SiteId)));
                $scope.ClusList = Array.from(new Set(response.map(i => i.Cluster)));
                $scope.HLLList = Array.from(new Set(response.map(i => i.HighLevelLocation)));


            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindSiteList();
        //Location
        $scope.BindLocList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetLocMaster'
            }).success(function (response) {
                $scope.LocMaster = response;

                $scope.LocList = Array.from(new Set(response.map(i => i.Location)));
                $scope.LochList = Array.from(new Set(response.map(i => i.LocationHierarchy)));

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        //AssetType
        $scope.BindATList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetAssetTypeMaster'
            }).success(function (response) {
                $scope.ATMaster = response;
                $scope.ATList = Array.from(new Set(response.map(i => i.AssetType)));
                $scope.ClsList = Array.from(new Set(response.map(i => i.ClassificationHierarchyDesc)));
                $scope.FacList = Array.from(new Set(response.map(i => i.FailureCode)));
                console.log($scope.ClsList)


            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        //Add_Notes
        $scope.BindAddList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList?label=AdditionalNotes'
            }).success(function (response) {
                $scope.Notes = response;
                console.log($scope.Notes)
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindATList();
        $scope.BindLocList();
        $scope.BindAddList();
        $scope.ShowHide = false;
        $scope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.AssetBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
              //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/AssetBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    } else {
                        $scope.ShowHide = false;
                        $scope.Res = data + " items uploaded successfully";
                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');


                });
            };
        }


        $scope.clearFar = function () {
            //alert('Clear')
            $scope.selectedRowIndex = -1;
            $scope.showlist = false;
            $scope.BtnFARmodel = '';
            $scope.BtnFARmodel1 = '';
            $scope.asset.Region = '';
            $scope.asset.AssetDesc = '';
            $scope.searchByAsset = '';
            $scope.searchByID = '';
            $scope.LoadData();
        }

        $scope.prefileList = [];

        var uploadObj = $("#fileuploader").uploadFile({
            url: "/MaterialRequest/fileupload",
            multiple: true,
            autoSubmit: false,
            dragDrop: true,
            fileName: "myfile",
            // sequential: true,
            //  sequentialCount: 5,
            onSelect: function (files) {
                files[0].name;
                files[0].size;
                for (var i = 0; i < files.length; i++) {

                    $scope.prefileList.push(files[i]);
                }

                return true; //to allow file submission.
            },

            onCancel: function (files, pd) {
                var newFiles = [];
                for (var i = 0; i < $scope.prefileList.length; i++) {
                    if ($scope.prefileList[i].name != files)
                        newFiles.push($scope.prefileList[i]);
                }
                $scope.prefileList = newFiles;
                // $("#eventsmessage").html($("#eventsmessage").html() + "<br/>Canceled  files: " + JSON.stringify(files));
            }
        });


        $scope.shortLong = false;
        $scope.formShortLong = function () {
            var formData = new FormData();
            $scope.asset.Noun = $scope.cat.Noun;
            $scope.asset.Modifier = $scope.cat.Modifier;
            formData.append("cat", angular.toJson($scope.asset));
            $scope.Characteristics = $filter('filter')($scope.Characteristics, function (char) {
                return char.Definition == 'Equ';
            });
            formData.append("attri", angular.toJson($scope.Characteristics));
            formData.append("vendorsupplier", angular.toJson($scope.rows));
            formData.append("Equ", angular.toJson($scope.Equ));

            //  alert(angular.toJson($scope.Equ));
            $http({
                url: "/FAR/GenerateShortLong",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
                $timeout(function () {
                    $scope.shortLong = true;
                    $scope.asset.Equipment_Short = response[0];
                    $scope.asset.Equipment_Long = response[1];
                    $scope.asset.MissingValue = response[2];
                    $scope.asset.EnrichedValue = response[3];
                    $scope.asset.RepeatedValue = response[4];
                });
            }).error(function (data, status, headers, config) {
                // Handle the error
                // alert("error");
            });

        };

        $scope.SaveData = function (form) {
            //alert($scope.shortLong)
          if ($scope.shortLong != false) {
          if (form != false) {
                if (confirm("Are you sure, do you want to save ths item?")) {
                    $timeout(function () {
                        $('#divNotifiy').attr('style', 'display: none');
                    }, 5000);
                    $scope.formShortLong();
                    //alert($scope.asset.Equipment_Short)
                    $scope.asset.FixedAssetNo = $scope.asset.FARId;
                    var formData = new FormData();
                    formData.append("Asset", angular.toJson($scope.asset));
                    formData.append("Cat", angular.toJson($scope.cat));
                    console.log($scope.Characteristics)
                    formData.append("attri", angular.toJson($scope.Characteristics));
                    formData.append("AssetBOM", angular.toJson($scope.rowBOM));

                    for (var i = 0; i < $scope.prefileList.length; i++) {
                        formData.append('files', $scope.prefileList[i]);
                    }

                    $scope.cgBusyPromises =   $http({
                        url: "/FAR/InsertAssetData",
                        method: "POST",
                        headers: { "Content-Type": undefined },
                        transformRequest: angular.identity,
                        data: formData
                    }).success(function (data, status, headers, config) {

                        if (data == false) {

                            $scope.Res = data.errors;
                            $scope.Notify = "alert-danger";
                            $('#divNotifiy').attr('style', 'display: block');

                        }
                        else {
                            $scope.fromsave = 1;
                            $scope.Res = "Data saved successfully";
                            $scope.prefileList = [];
                            $scope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.reset();
                            $scope.BtnFARmodel = "";
                            $scope.BtnFARmodel1 = "";
                            $scope.clearFar();
                            uploadObj.reset();
                            $scope.asset = null;
                            $scope.cat = null;
                            $scope.shortLong == false;
                            $scope.Characteristics = null;
                            $scope.rowBOM = [{ 'Qty': 1, 'UOM': 'EA' }];
                            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sStatus != undefined && $scope.sStatus != '')) {
                                $scope.searchMaster();
                            } else {
                                $scope.LoadData();
                            }
                        }


                    }).error(function (data, status, headers, config) {
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    });
                }
          }
          else {
                $scope.Res = "Please fill the highlighted mandatory field(s)";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
            }
          }
          else {
                $scope.Res = "Please generate short and long ";
                $scope.Notify = "alert-danger";
                $('#divNotifiy').attr('style', 'display: block');
            }
        }


        $scope.asset = {};
        $scope.BtnFARmodel = '';
        $scope.BtnFARmodel1 = '';
        $scope.BtnRegmodel = '';
        $scope.BtnDescmodel = '';

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
                    $scope.BtnFARmodel1 = far;
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

        //$scope.selectFAREntity = function (far) {

            
        //}


        //$scope.selectRegEntity = function (far, reg) {
        //    $scope.BtnRegmodel = 'Select';
        //    $scope.BtnDescmodel = 'Select';

        //    //$scope.AssetDescMaster = [];
        //    $scope.BtnRegmodel = reg;
        //    $scope.asset.Region = reg;
        //    $scope.AssetDesc_Master = $filter('filter')($scope.FAR_Master, { 'FARId': far, 'Region': reg });
        //    $scope.AssetDescMaster = Array.from(new Set($scope.AssetDesc_Master.map(i => i.AssetDesc)));
        //    $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');;
        //    console.log($scope.asset.AssetDesc)
        //}

        $scope.selectDescEntity = function (desc) {

            $scope.BtnDescmodel = desc;
            $scope.asset.AssetDesc = desc;

        }

        $scope.TARList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetTarMaster'
            }).success(function (response) {

                $scope.TARMaster = response;
                $scope.FAR_Master = $filter('filter')(response, { 'Label': 'FARId' });


            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.TARList();
     
        $scope.loadCompanycode = function () {

            $http({
                method: 'GET',
                url: '/FAR/getCompanycode'
                
            }).success(function (response) {

                $scope.companycodelist = response;
            
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadCompanycode();
       
        $scope.loadBOMequipmentType = function () 
       {

            $http({
                method: 'GET',
                url: '/FAR/GetBOMEquipmentType',
               
            }).success(function (response) {

                $scope.BOMEquipmentTypelist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
      

        $scope.loadequipment = function (minor) {

            $http({
                method: 'GET',
                url: '/FAR/GetEquipment',
                params:{minorclass:minor}
            }).success(function (response) {

                $scope.Equipmentlist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadobjecttype = function () {

            $http({
                method: 'GET',
                url: '/FAR/Getobjecttype'
            }).success(function (response) {

                $scope.objecttypelist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadobjecttype();

        $scope.loadfunlochierarchy = function () {

            $http({
                method: 'GET',
                url: '/FAR/Getfunlochierarchy'
            }).success(function (response) {

                $scope.funlochierarchylist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadfunlochierarchy();
        $scope.loadsiteId = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetsiteId'
            }).success(function (response) {

                $scope.siteIdlist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadsiteId();
        $scope.loadsitename = function () {

            $http({
                method: 'GET',
                url: '/FAR/Getsitename'
            }).success(function (response) {

                $scope.sitenamelist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadsitename();
        $scope.loadsitetype= function () {

            $http({
                method: 'GET',
                url: '/FAR/Getsitetype'
            }).success(function (response) {

                $scope.sitetypelist = response;
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.treeBOMData = [];

        $scope.toggleNode = function (node) {
            node.expanded = !node.expanded;

            if (!node.expanded && node.children && node.children.length) {
                collapseChildren(node.children);
            }
        };

        function collapseChildren(children) {
            children.forEach(child => {
                child.expanded = false;
                if (child.children && child.children.length) {
                    collapseChildren(child.children);
                }
            });
        }
        //const bomData = [];
        //const assemblyData = [];
        //const subAssemblyData = [];
        //const componentData = [];
        //$scope.getAssemblyData = function (flatData) {

        //}
        $scope.mapBomData = function (flatData) {
            const rootMap = {};          // Category H (BOMs)
            const assemblyMap = {};      // Category A
            const subAssemblyMap = {};   // Category SA
            const componentMap = {};  // Category N & E
            flatData.forEach(item => {
                if (item.Category === 'H')  {
                    rootMap[item.BOMID] = item;
                }
                else if (item.Category === 'A' || (item.AssemblyId === ''))  {
                    assemblyMap[item.BOMID] = item;
                }
                else if (item.Category === 'SA' || (item.AssemblyId === ''))  {
                    assemblyMap[item.BOMID] = item;
                }
            });
        }

        function buildTree(flatData) {
            const rootMap = {};
            const assemblyMap = {};
            const subAssemblyMap = {};

            // Initialize
            flatData.forEach(item => {
                item.children = [];
                item.expanded = false;
            });

            // Step 1: Map BOMs (H)
            flatData.forEach(item => {
                if (item.Category === 'H') {
                    rootMap[item.BOMId] = {
                        BOMId: item.BOMId,
                        BOMDesc: item.BOMDesc,
                        Category: 'H',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        Materialcode: item.ComponentId,
                        children: [],
                        expanded: false
                    };
                }
            });

            // Step 6: Components (N/E) directly under BOM (no AssemblyId)
            flatData.forEach(item => {
                if (['N', 'E'].includes(item.Category) && (!item.AssemblyId || item.AssemblyId.trim() === '')) {
                    const parent = rootMap[item.BOMId];
                    if (parent) {
                        parent.children.push({
                            ComponentId: item.ComponentId,
                            ComponentDesc: item.ComponentDesc,
                            Materialcode: item.ComponentId,
                            UOM: item.UOM,
                            Quantity: item.Quantity,
                            Category: item.Category,
                            children: [],
                            expanded: false
                        });
                    }
                }
            });

            // Step 2: Map Assemblies (A) under BOM
            flatData.forEach(item => {
                if (item.Category === 'A') {
                    const parent = rootMap[item.BOMId];
                    const assemblyObj = {
                        AssemblyId: item.AssemblyId,
                        AssemblyDesc: item.AssemblyDesc,
                        Materialcode: item.ComponentId,
                        Category: 'A',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        children: [],
                        expanded: false
                    };

                    if (parent) {
                        parent.children.push(assemblyObj);
                    }

                    assemblyMap[item.AssemblyId] = assemblyObj;
                }
            });

            // Step 4: Pre-create SubAssemblies (SA) and attach components
            flatData.forEach(item => {
                if (item.Category === 'SA') {
                    subAssemblyMap[item.AssemblyId] = {
                        AssemblyId: item.AssemblyId,
                        AssemblyDesc: item.AssemblyDesc,
                        Materialcode: item.ComponentId,
                        Category: 'SA',
                        UOM: item.UOM,
                        Quantity: item.Quantity,
                        children: [],
                        expanded: false
                    };
                }
            });

            // Step 4 continued: Add components under SubAssemblies
            flatData.forEach(item => {
                if (['N', 'E'].includes(item.Category)) {
                    const parent = subAssemblyMap[item.AssemblyId];
                    if (parent) {
                        parent.children.push({
                            ComponentId: item.ComponentId,
                            ComponentDesc: item.ComponentDesc,
                            Materialcode: item.ComponentId,
                            UOM: item.UOM,
                            Quantity: item.Quantity,
                            Category: item.Category,
                            children: [],
                            expanded: false
                        });
                    }
                }
            });

            // Step 3: Attach SubAssemblies under Assemblies (based on prefix match)
            Object.values(subAssemblyMap).forEach(subAssembly => {
                const parent = Object.values(assemblyMap).find(a =>
                    subAssembly.AssemblyId.startsWith(a.AssemblyId + '-')
                );

                if (parent) {
                    parent.children.push(subAssembly);

                    // Ensure components come before subassemblies
                    parent.children.sort((a, b) => {
                        const order = { 'N': 1, 'E': 1, 'SA': 2 };
                        return (order[a.Category] || 3) - (order[b.Category] || 3);
                    });
                }
            });

            // Step 5: Components under Assembly (not in any SubAssembly)
            flatData.forEach(item => {
                if (['N', 'E'].includes(item.Category)) {
                    if (!subAssemblyMap[item.AssemblyId]) {
                        const parent = assemblyMap[item.AssemblyId];
                        if (parent) {
                            parent.children.push({
                                ComponentId: item.ComponentId,
                                Materialcode: item.ComponentId,
                                ComponentDesc: item.ComponentDesc,
                                UOM: item.UOM,
                                Quantity: item.Quantity,
                                Category: item.Category,
                                children: [],
                                expanded: false
                            });
                        }
                    }
                }
            });

            // Final Sort: Ensure all assemblies have components before subassemblies
            Object.values(assemblyMap).forEach(assembly => {
                assembly.children.sort((a, b) => {
                    const order = { 'N': 1, 'E': 1, 'SA': 2 };
                    return (order[a.Category] || 3) - (order[b.Category] || 3);
                });
            });

            return Object.values(rootMap);
        }


        $scope.getbOM = function (id) {

            $http({
                method: 'GET',
                url: '/FAR/getAssetBom',
                params: {id: id}
            }).success(function (response) {

                $scope.sitetypelist = response;

                $scope.flatData = response;
                console.log($scope.flatData);
                $scope.treeBOMData = buildTree($scope.flatData);
                console.log($scope.treeBOMData);
            }).error(function (data, status, headers, config) {
                //alert("error");
            });
        };
        $scope.loadsitetype();

        $scope.selectedRowIndex = -1;
        $scope.Lstattachment = []; 

        $scope.editAction = false;
        $scope.saveAction = false;
        $scope.tab = "data";
        var usr = "";
        $scope.currentusr = "";
        $scope.editImage = false;
        $scope.editQR = true;

        var cataloguer = "";
        var reviewer = "";
        var itmSts = 0;
        $scope.RowClick = function (lst, idx) {
            console.log(lst)
            $scope.Lstattachment = [];
            //$scope.BtnFARmodel = 'Select';
            //$scope.BtnRegmodel = 'Select';
            //$scope.BtnDescmodel = 'Select';
            $scope.editAction = false;
            usr = $('#usrHid').val();
            $scope.currentusr = $('#usrHid').val();
            if (["coda", "KHAMIZ", "Badri", "Mivinkumar", "Ajith", "Kimilson", "Falilrashith", "Yuvaraj", "Hajisyed", "Imran", "Mohamed", "Halid"].includes(usr))
                $scope.editImage = true;
            else
                $scope.editImage = false;
            if (["coda", "KHAMIZ", "Badri", "Mivinkumar", "Ajith", "Kimilson", "Falilrashith", "Yuvaraj", "Mivinkumar", "Hajisyed"].includes(usr)) {
                $scope.saveAction = true;
                $scope.editQR = false;
            }
            else {
                $scope.saveAction = false;
                $scope.editQR = true;
            }
            $scope.selectedRowIndex = idx;
            $scope.showlist = true;
            $scope.asset = {};
            $scope.cat = {};
            var dbCharacteristics = [];
            $scope.exCharacteristics = [];
            console.log(lst);
    
            $scope.loadBOMequipmentType();
            $http({
                method: 'GET',
                url: '/FAR/GetAssetinfo',
                params: { UniqueId: lst.UniqueId }
            }).success(function (response) {
                $scope.asset = response;
                if ($scope.asset.Bom != null && $scope.asset.Bom.BOMId != null && $scope.asset.Bom.BOMId != "") {
                    $scope.getbOM($scope.asset.Bom.BOMId);
                }
                $scope.tempCH = $scope.asset.ClassificationHierarchyDesc;
                //alert(angular.toJson(response.Catalogue))
                //alert(angular.toJson(response.Review))
                //alert(response.ItemStatus)
                //alert(response.Catalogue.Name)
                itmSts = response.ItemStatus;
                if (response.ItemStatus == 2 || response.ItemStatus == 3) {
                    if (response.Catalogue != null && response.Catalogue.Name == usr) {
                        cataloguer = response.Catalogue.Name;
                        $scope.editAction = true;
                    }
                    else $scope.editAction = false;
                }
                if (response.ItemStatus == 4 || response.ItemStatus == 5) {
                    if (response.Review != null && response.Review.Name == usr) {
                        //alert(response.Review.Name)
                        reviewer = response.Review.Name;
                        $scope.editAction = true;
                    }
                    else $scope.editAction = false;
                }

                if ($scope.tab == "data")
                    $scope.editAction = false;

                if (response.Attachment !== null && response.Attachment !== '') {
                    var splt = response.Attachment.split(',');
                    if (splt.length > 0) {
                        $scope.Lstattachment = splt;
                    }
                    else {
                        $scope.Lstattachment.push(response.Attachment)
                    }

                }

                if ($scope.asset.Idle_Operational != "Idle")
                    $scope.asset.Idle_Operational = "Operational";
                $scope.BtnFARmodel1 = $scope.asset.FixedAssetNo;
                $scope.asset.FARId = $scope.asset.FixedAssetNo;
                //alert($scope.asset.FixedAssetNo);
                //alert($scope.BtnFARmodel1)
                //alert(angular.toJson($scope.asset.Condition))
                //alert(angular.toJson($scope.asset.Condition.Rank))
                $scope.BtnFARmodel = response.FixedAssetNo;


                //Old
                //if ($scope.asset && $scope.asset.GIS && $scope.asset.GIS.LongitudeStart != null) {
                //    if ($scope.asset.GIS.LongitudeStart.includes('W'))
                //    $scope.asset.GIS.LongitudeStart = $scope.asset.GIS.LongitudeStart.replace('W', 'E');
                //}

                //New GIS fixing
                if ($scope.asset && $scope.asset.GIS) {
                    let lat = $scope.asset.GIS.LattitudeStart || "";
                    let lon = $scope.asset.GIS.LongitudeStart || ""; 

                    const invalidValues = ["0", "00", "000", "", null];

                    if (invalidValues.includes(lat) || invalidValues.includes(lon)) {
                        if (invalidValues.includes(lat)) {
                            $scope.asset.GIS.LattitudeStart = "";
                        }
                        if (invalidValues.includes(lon)) {
                            $scope.asset.GIS.LongitudeStart = "";
                        }
                    }
                    else if (!lat.includes("W") && !lat.includes("N") && !lon.includes("W") && !lon.includes("N")) {
                        $scope.asset.GIS.LattitudeStart = lat.startsWith("2") ? lat + "N" : lat + "E";
                        $scope.asset.GIS.LongitudeStart = lon.startsWith("2") ? lon + "N" : lon + "E";
                    }
                    else {
                        if (lat.includes("W")) {
                            $scope.asset.GIS.LattitudeStart = lat.replace("W", "E");
                        }
                        if (lon.includes("W")) {
                            $scope.asset.GIS.LongitudeStart = lon.replace("W", "E");
                        }
                    }

                    if ($scope.asset.GIS.LattitudeStart.includes("Â")) {
                        $scope.asset.GIS.LattitudeStart = $scope.asset.GIS.LattitudeStart.replace("Â", " ");
                    }
                    if ($scope.asset.GIS.LongitudeStart.includes("Â")) {
                        $scope.asset.GIS.LongitudeStart = $scope.asset.GIS.LongitudeStart.replace("Â", " ");
                    }
                }



                if ($scope.asset && $scope.asset.AssetCondition) {
                    if ($scope.asset.AssetCondition.Rank == "7") {
                        $scope.asset.AssetCondition.Rank = "1";
                    }
                    else if ($scope.asset.AssetCondition.Rank == "6") {
                        $scope.asset.AssetCondition.Rank = "2";
                        if ($scope.asset.AssetCondition.Damage == "Medium") {
                            $scope.asset.AssetCondition.Rank = "2";
                        }
                    }
                    else if ($scope.asset.AssetCondition.Rank == "4" || $scope.asset.AssetCondition.Rank == "5") {
                        $scope.asset.AssetCondition.Rank = "3";
                    }
                    else if ($scope.asset.AssetCondition.Rank == "2" || $scope.asset.AssetCondition.Rank == "3") {
                        $scope.asset.AssetCondition.Rank = "4";
                    }
                    else if ($scope.asset.AssetCondition.Rank == "1") {
                        $scope.asset.AssetCondition.Rank = "5";
                    }
                    if ($scope.asset.AssetCondition.Leakage == "Medium" ) {
                        $scope.asset.AssetCondition.Rank = "3";
                    }
                    if ($scope.asset.AssetCondition.Leakage == "Medium" && $scope.asset.AssetCondition.Vibration == "Medium" && $scope.asset.AssetCondition.Temparature == "Medium") {
                        $scope.asset.AssetCondition.Rank = "3";
                    }
                    if ($scope.asset.AssetCondition.Leakage == "Medium" && $scope.asset.AssetCondition.Vibration == "Medium" && $scope.asset.AssetCondition.Temparature == "Medium" && $scope.asset.AssetCondition.Damage == "Medium" && $scope.asset.AssetCondition.Smell == "Medium") {
                        $scope.asset.AssetCondition.Rank = "3";
                    }
                    if ($scope.asset.AssetCondition.Leakage == "Medium" && $scope.asset.AssetCondition.Vibration == "Medium" && $scope.asset.AssetCondition.Temparature == "Medium" && $scope.asset.AssetCondition.Damage == "Medium" && $scope.asset.AssetCondition.Smell == "Medium" && $scope.asset.AssetCondition.Noise == "Medium") {
                        $scope.asset.AssetCondition.Rank = "5";
                    }
                    if ($scope.asset.AssetCondition.Corrosion == "High" && $scope.asset.AssetCondition.Damage == "High") {
                        $scope.asset.AssetCondition.Rank = "5";
                    }
                    if ($scope.asset.AssetCondition.Leakage == "No" && $scope.asset.AssetCondition.Corrosion == "No" && $scope.asset.AssetCondition.Vibration == "No" && $scope.asset.AssetCondition.Temparature == "No" && $scope.asset.AssetCondition.Damage == "No" && $scope.asset.AssetCondition.Smell == "No" && $scope.asset.AssetCondition.Noise == "No") {
                        $scope.asset.AssetCondition.Rank = "1";
                    }
                    //alert($scope.asset.AssetCondition.Rank)
                }

                console.log($scope.asset.FARId)
                //if ($scope.FARMaster != null) {
                //    $scope.FARMaster = $filter('filter')($scope.FARMaster, function (i) {
                //        return i.value == $scope.asset.FARId;
                //    });
                //}
                //$scope.FARMaster = $filter('filter')($scope.FARMaster, { $scope.asset.FARId });
                console.log($scope.FARMaster)
                $scope.BindBusinessList($scope.asset.FixedAssetNo)
                $scope.checkSubmit(response.ItemStatus);
                //$scope.BindBusinessList();
                console.log($scope.FAR_Master)
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
            //$http({
            //    method: 'GET',
            //    url: '/FAR/getAssetBom',
            //    params: { equipmentId: lst.UniqueId }
            //}).success(function (response) {
            //    $scope.rowBOM = response;

            //}).error(function (data, status, headers, config) {

            //});
            $http({
                method: 'GET',
                url: '/FAR/GetAttributeinfo',
                params: { UniqueId: lst.UniqueId }
            }).success(function (response) {
                console.log(response)
                $scope.cat.Noun = response.Noun;
                $scope.loadmodifier($scope.cat.Noun)
                $scope.cat.Modifier = response.Modifier;

                //alert(angular.toJson(response))
                //alert(angular.toJson(response.exCharacterisitics))

                dbCharacteristics = response.Characterisitics;
                $scope.exCharacteristics = response.exCharacterisitics;
                //angular.forEach($scope.exCharacteristics, function (ex) {
                //    return ex.UomMandatory = 'EX';
                //});
                $scope.cat.exNoun = response.exNoun;
                $scope.cat.exModifier = response.exModifier;

                $scope.exNoun = response.exNoun;
                $scope.exModifier = response.exModifier;
               
                $http({
                    method: 'GET',
                    url: '/Dictionary/EquGetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {

                        $scope.Characteristics = response.ALL_NM_Attributes;
                       
                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;

                        }).error(function (data, status, headers, config) {
                            // 
                        });



                    }
                    else $scope.Characteristics = null;
                       
               
                    angular.forEach($scope.Characteristics, function (value1, key1) {

                        angular.forEach(dbCharacteristics, function (value2, key2) {
                           
                            if (value1.Characteristic === value2.Characteristic) {

                                value1.Value = value2.Value;
                                value1.UOM = value2.UOM;
                                value1.Source = value2.Source;
                                value1.SourceUrl = value2.SourceUrl;
                                value1.Squence = value1.Squence;
                                value1.ShortSquence = value1.ShortSquence;
                                value1.UomMandatory = value1.UomMandatory;
                                if ($scope.asset.Exchk == true)
                                    value1.Mandatory = 'No';
                                // value1.Abbrivation = value2.Abbrevated;
                                // value1.Approve = value2.Approve;
                                //value1.btnName = "Approve";
                            }
                        });
                    });

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
              


            }).error(function (data, status, headers, config) {
                // alert("error");

            });

            //var i = 0;
            //angular.forEach($scope.AssetdataList, function (lst) {
            //    $('#' + i).attr("style", "");
            //    i++;
            //});
            //$('#' + idx).attr("style", "background-color:lightblue");


        }

        $scope.newQRChange = function (qr) {
            $scope.asset.AssetQRCode = qr;
        }

        $scope.tabChange = function (tab) {
            $scope.tab = tab;
            //alert(usr)
            //alert(cataloguer)
            //alert(reviewer)
            if (tab == "data")
                $scope.editAction = false;
            else if (tab == "class") {
                $scope.editAction = true;
                if (itmSts == 2 || itmSts == 3) {
                    if ((cataloguer != null && cataloguer == usr))
                        $scope.editAction = true;
                    else $scope.editAction = false;
                }
                if (itmSts == 4 || itmSts == 5) {
                    if ((reviewer != null && reviewer == usr))
                        $scope.editAction = true;
                    else $scope.editAction = false;
                }
            }
            else if (tab == "condn")
                $scope.editAction = false;
            else if (tab == "gis")
                $scope.editAction = false;
            else if (tab == "attach")
                $scope.editAction = false;
            //alert($scope.editAction)
        }

        $scope.Downloadfile = function (RequestId, fname) {
            //alert(RequestId + "," + fname )
            $window.open('/Catalogue/Downloadfile?ItemId=' + RequestId + '&fName=' + fname);

        };

        $scope.changeFAR = function (far) {
            $scope.BtnFARmodel1 = far;
            $scope.asset.FARId = far;
            $scope.RegionMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': far }).map(i => i.Region)));
            $scope.AssetDescMaster = Array.from(new Set($filter('filter')($scope.FAR_Master, { 'FARId': far }).map(i => i.AssetDesc)));

            $scope.asset.AssetDesc = $scope.AssetDescMaster.join('');
            $scope.asset.Region = $scope.RegionMaster.join('');
        }
        $scope.isSelectedRow = function (index) {
            return $scope.selectedRowIndex === index;
        };
        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $scope.sts2 = false;
        $scope.checkSubmit = function (ItemStatus) {
         
            $scope.sts1 = false;
            $scope.sts2 = false;

            if (ItemStatus == 3 || ItemStatus == 5) {
                $scope.sts1 = true;
            }
            if (ItemStatus == 4 || ItemStatus == 5) {
                $scope.sts2 = true;
            }

        };

        $scope.Exceptional = function (Noun, Modifier) {
            console.log(Noun)
            console.log(Modifier)

            if ($scope.asset.Exchk == true) {

                angular.forEach($scope.Characteristics, function (value1) {
                    value1.Mandatory = 'No'
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/Dictionary/EquGetNounModifier',
                    params: {
                        Noun: Noun,
                        Modifier: Modifier
                    }
                }).success(function (response) {
                    console.log(response)
                    if (response != '') {

                        angular.forEach($scope.Characteristics, function (value1, key1) {

                            angular.forEach(response.ALL_NM_Attributes, function (value2, key2) {

                                if (value1.Characteristic === value2.Characteristic) {
                                    value1.Mandatory = value2.Mandatory
                                }
                            });
                        });

                    }
                    else {
                        $scope.Characteristics = null;
                        // $('#divcharater').attr('style', 'display: none');
                    }
                    //   alert(angular.toJson($scope.Characteristics));
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            }

        }

        $scope.bumu = '0';
        $scope.idlist=["Idle","Operational"]
        $scope.AssetRework = function () {

            //   alert("hai");

            //if (!$scope.form.$invalid) {               
            if (confirm("Are you sure, rework this record?")) {
                if ($scope.asset.Rework_Remarks != null) {


                    $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                    $scope.cgBusyPromises = $http({
                        method: 'GET',
                        url: '/FAR/AssetRework',
                        params: { UniqueId: $scope.asset.UniqueId, RevRemarks: $scope.asset.Rework_Remarks }
                    }).success(function (data, status, headers, config) {
                        if (data.success === false) {
                            //  alert(angular.toJson($scope.cat.RevRemarks));
                            $scope.Res = data.errors;
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                        }
                        else {

                            if (data === true) {
                                $scope.reset();

                                $scope.asset = null;
                                $scope.Characteristics = null;


                                $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                                $scope.Res = "Record sent to rework"
                                $scope.Notify = "alert-info";

                                $('#divNotifiy').attr('style', 'display: block');


                                // $scope.searchMaster();
                                $scope.LoadData();
                            } else {
                                $scope.Res = "Record sending fail to rework"
                                $scope.Notify = "alert-info";

                                $('#divNotifiy').attr('style', 'display: block');

                            }

                            $scope.asset = {};
                            $scope.showlist = false;
                        }

                    }).error(function (data, status, headers, config) {
                        $scope.Res = data;
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    });
                }
                else {
                    $scope.Res = "Please enter remarks"
                    $scope.Notify = "alert-danger";

                    $('#divNotifiy').attr('style', 'display: block');
                }
            }

        };
        $scope.PVRework = function () {

            //   alert("hai");

            //if (!$scope.form.$invalid) {               
            if (confirm("Are you sure, rework this record?")) {
                if ($scope.asset.Rework_Remarks != null) {


                    $timeout(function () { $scope.NotifiyRes = false; }, 30000);

                    $scope.cgBusyPromises = $http({
                        method: 'GET',
                        url: '/FAR/PVRework',
                        params: { UniqueId: $scope.asset.UniqueId, RevRemarks: $scope.asset.Rework_Remarks, CondRemarks: $scope.asset.assetConditionRemarks, ImageRemarks: $scope.asset.Remarks, AddRemarks: $scope.asset.AdditionalNotes }
                    }).success(function (data, status, headers, config) {
                        if (data.success === false) {
                            //  alert(angular.toJson($scope.cat.RevRemarks));
                            $scope.Res = data.errors;
                            $scope.Notify = "alert-danger";
                            $scope.NotifiyRes = true;
                        }
                        else {

                            if (data === true) {
                                $scope.reset();

                                $scope.asset = null;
                                $scope.Characteristics = null;


                                $scope.rows = [{ 'slno': 1, 's': '1', 'l': '1' }];
                                $scope.Res = "Record sent to PV rework"
                                $scope.Notify = "alert-info";

                                $('#divNotifiy').attr('style', 'display: block');


                                // $scope.searchMaster();
                                $scope.showlist = false;
                                $scope.asset = {};
                                $scope.LoadData();
                            } else {
                                $scope.Res = "Record sending fail to rework"
                                $scope.Notify = "alert-info";

                                $('#divNotifiy').attr('style', 'display: block');

                            }

                        }

                    }).error(function (data, status, headers, config) {
                        $scope.Res = data;
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    });
                }
                else {
                    $scope.Res = "Please enter remarks"
                    $scope.Notify = "alert-danger";

                    $('#divNotifiy').attr('style', 'display: block');
                }
            }

        };
        $scope.chkmuall = function () {
       
            //  alert(angular.toJson($scope.bumu));

            if ($scope.bumu === 1) {
                angular.forEach($scope.AssetdataList, function (value1, key1) {
                    //   alert(angular.toJson(value1.Materialcode));
                    //  alert(angular.toJson(value1.ItemStatus));
                    if (value1.ItemStatus === 3) {
                        if (value1.bu != 1)
                            value1.bu = '1';
                    }
                });
            }
            else {
                angular.forEach($scope.AssetdataList, function (value1, key1) {
                    if (value1.bu != 0)
                        value1.bu = '0';
                });
            }
            // alert(angular.toJson($scope.DataList));
        };
        $scope.SubmitAssetData = function () {


            if ($filter('filter')($scope.AssetdataList, { 'bu': '1' }).length >= 1) {

                $scope.AssetdataList = $filter('filter')($scope.AssetdataList, { 'bu': '1' })

            }
            var filcat = $filter('filter')($scope.AssetdataList, { 'ItemStatus': '3' });


            if (filcat != "") {
                $scope.AssetdataList = filcat;
            }
            else {
               
                $scope.AssetdataList = $filter('filter')($scope.AssetdataList, { 'ItemStatus': '5' });

            }

            console.log($scope.AssetdataList)

            if ($scope.AssetdataList != "" && $scope.AssetdataList != undefined && $scope.AssetdataList != []) {

                $timeout(function () {

                    $('#divNotifiy').attr('style', 'display: none');
                }, 5000);

                var formData = new FormData();

                $scope.AssetdataList = $scope.AssetdataList.map(function (obj) {
                    var newObj = Object.entries(obj).reduce((acc, [key, value]) => {
                        if (key !== '_id') {
                            acc[key] = value;
                        }
                        return acc;
                    }, {});
                    return newObj;
                });


                formData.append("AssetdataList", angular.toJson($scope.AssetdataList));

                console.log($scope.AssetdataList)

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/InsertAssetDataList",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {

                    if (data > 0) {
                        $scope.asset = null;

                        $scope.Res = data + " Data submitted successfully";
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.LoadData();
                        $scope.showlist = false;
                        if ($scope.asset !== null && typeof $scope.asset === 'object')
                            $scope.asset.FARId = "";
                        $scope.asset.BtnFARmodel = "";
                        $scope.reset();
                    } else {
                        $scope.Res = "Data submission failed"
                        $scope.Notify = "alert-info";

                        $('#divNotifiy').attr('style', 'display: block');
                    }



                }).error(function (data, status, headers, config) {
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $('#divNotifiy').attr('style', 'display: block');
                });
            }
            else {
                alert("Select saved items to submit");
                $scope.LoadData();
            }

        };



        $scope.rowBOM = [{ 'Qty': 1, 'UOM': 'EA'}];
        $scope.addRow = function () {
        
            var bomList = [];
            $scope.charcheck = false;
            angular.forEach($scope.rowBOM, function (lst) {
              
                //if (bomList.indexOf(lst.Characteristic) !== -1) {
                //    $scope.charcheck = true;
                //}
                bomList.push(lst.Qty);


            });
            if ($scope.charcheck == false)
                $scope.rowBOM.push({ 'Qty': 1, 'UOM': 'EA' });


        };

        $scope.RemoveRow = function (inx) {
            if ($scope.rowBOM.length > 1) {
                var newLst = [];
                var incr = 0;
                angular.forEach($scope.rowBOM, function (lst) {
                    if (inx != incr) {
                        newLst.push(lst);
                    }
                    incr++;

                });
                if (newLst.length > 0)
                    $scope.rowBOM = newLst;
                else $scope.rowBOM.push({ 'Qty': 1, 'UOM': 'EA' });
            }
        };
           
        $scope.loadmodifier = function (noun) {

            // alert("mod");
            //alert(angular.toJson(noun));


            $scope.Type = null;
            $scope.cat.Modifier = null;
            $scope.cat.Exceptional = false;
            $scope.Characteristics = null;
            $scope.cat.RevRemarks = null; $scope.cat.RelRemarks = null; $scope.cat.Remarks = null; $scope.cat.EnrichedValue = null;
            $scope.cat.MissingValue = null;
            $scope.cat.RepeatedValue = null;
            $scope.cat.Longdesc = null;
            $scope.cat.Shortdesc = null;
            $scope.cat.Additionalinfo = null;
            $scope.cat.UOM = null;
            $scope.Equ = null;
            $scope.sts4 = false;
            $scope.sts3 = false;
            $scope.Legacy1 = true;
            $scope.reset();
            if ($scope.cat && $scope.cat.Noun) 
                $scope.cat.Noun = $scope.cat.Noun.toUpperCase();
            $http({
                method: 'GET',
                url: '/Dictionary/GetAssetModifier',
                params: { Noun: noun }
            }).success(function (response) {

                $scope.Modifiers = response;
                console.log($scope.Modifiers)
            }).error(function (data, status, headers, config) {

            });


        }
        $scope.ChangeModifier = function () {
            $scope.cat.Shortdesc = "";
            $scope.cat.Longdesc = "";
            $scope.cat.MissingValue = "";
            $scope.cat.EnrichedValue = "";
            $scope.cat.RepeatedValue = "";

            $scope.dumbNoun = $scope.cat.Noun;
            $scope.dumbModifier = $scope.cat.Modifier;

            if ($scope.cat.Modifier != null && $scope.cat.Modifier != '') {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspsc',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    if (response != '') {
                        $scope.Commodities = response;
                        if (response[0].Commodity != null && response[0].Commodity != "")
                            $scope.cat.Unspsc = response[0].Commodity;
                        else $scope.cat.Unspsc = response[0].Class;
                    }
                    else {
                        $scope.Commodities = null;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetHsn?Noun=' + $scope.cat.Noun + '&Modifier=' + $scope.cat.Modifier
                }).success(function (response) {
                    if (response.HSNID != null) {
                        $scope.HSNID = response.HSNID;
                        $scope.Desc = response.Desc;
                        $scope.HSNShow = true;
                    }
                    else {
                        $scope.HSNShow = false;
                        $scope.HSNID = null;
                        $scope.Desc = null;
                    }


                });

                $http({
                    method: 'GET',
                    url: '/Dictionary/getuomlist1',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    if (response.length > 0) {
                        $scope.uomList1 = response;

                    }
                    else {

                        $http.get('/Catalogue/getuomlist').success(function (response) {
                            // alert('hi');
                            $scope.uomList1 = response;
                            //alert(angular.toJson($scope.uomList1))
                        });
                    }


                });

                //  $scope.getItem();
                $http({
                    method: 'GET',
                    url: '/Dictionary/GetForamted',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {

                    $scope.Type = response;

                    //angular.forEach($scope.rows, function (lst) {
                    //    lst.s = '1';

                    //});
                    if ($scope.Type == "OPM" || $scope.Type == "OEM") {
                        $scope.ven = true;


                    }
                    else {
                        $scope.ven = false;
                    }

                });


                $http({
                    method: 'GET',
                    url: '/Dictionary/EquGetNounModifier',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.NM1 = response.One_NounModifier;

                        $scope.Characteristics = response.ALL_NM_Attributes;
                        $scope.dumbCharacteristics = response.ALL_NM_Attributes;

                        console.log($scope.Characteristics);
                        console.log($scope.tempCH);
                        if ($scope.Characteristics != null) {
                            if ($scope.Characteristics[0].HierarchyPath != null)
                                $scope.asset.ClassificationHierarchyDesc = $scope.Characteristics[0].HierarchyPath;
                            else
                                $scope.asset.ClassificationHierarchyDesc = $scope.tempCH;
                        }

                        $http({
                            method: 'GET',
                            url: '/Catalogue/GetUnits'
                        }).success(function (response) {
                            $scope.UOMs = response;

                        }).error(function (data, status, headers, config) {
                            // 
                        });



                    }
                    else $scope.Characteristics = null;

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

            }
            else {

                // $scope.cat = null;
                $scope.Characteristics = null;
                //  $scope.equ = null;
            }
            //   $scope.getSimiliar();    

        };
        $scope.SelectCharater = function (Noun, Modifier, Attribute, inx) {

            $http({
                method: 'GET',
                url: '/FAR/GetAssetValues',

                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute }
            }).success(function (response) {

                $("#div" + inx).addClass("sample-arrow");

                $("#values" + inx).autocomplete({

                    minLength: 0,
                    source: response,
                    select: function (event, ui) {
                        $scope.Characteristics[inx].Value = ui.item.label;
                    }
                }).autocomplete("search", "");
            }).error(function (data, status, headers, config) {
                // alert("error");
            });


            angular.forEach($scope.exCharacteristics, function (ex) {
                return ex.UomMandatory = 'EX';
            });


            //  $("#values" + inx).click(function () { $(this) });


        };
        $scope.checkValue = function (Noun, Modifier, Attribute, Value, indx) {
            //  Value = Value.replace('&', '***');
            console.log($scope.Characteristics);

            if (Value != null && Value != '') {

                if (Attribute == "TYPE" || Attribute == "MATERIAL") {
                    angular.forEach($scope.Commodities, function (value1, key1) {
                        if (value1.value === Value) {
                            if (value1.Commodity != null && value1.Commodity != "")
                                return $scope.cat.Unspsc = value1.Commodity;
                            else return $scope.cat.Unspsc = value1.Class;
                            ;
                        }

                    });
                }

                var res = $filter('filter')($scope.tempPlace, { 'KeyValue': Value }, true);

                if (res != null && res != '') {

                    if (res[0].KeyAttribute === Attribute && res[0].KeyValue === Value) {


                        angular.forEach($scope.Characteristics, function (value2, key2) {

                            angular.forEach(res[0].Characteristics, function (value1, key1) {

                                if (value1.Characteristic === value2.Characteristic) {

                                    value2.Value = value1.Value;
                                    value2.UOM = value1.UOM;

                                }

                            })
                        });


                    }
                }

                $http({
                    method: 'GET',
                    url: '/Catalogue/CheckValue',
                    params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value }

                }).success(function (response) {


                    if (response === "false") {
                        $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
                        $('#checkval' + indx).attr('style', 'display:block');
                        $scope.Characteristics[indx].Abbrivation = "";


                    }
                    else {

                        $scope.Characteristics[indx].Abbrivation = response;

                        $http({
                            method: 'GET',
                            url: '/Catalogue/CheckValue1',
                            params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value }

                        }).success(function (response) {
                            if (response === "false") {

                                $('#btnabv' + indx).attr('style', 'display:block;background:#fff;border:none;color:#3e79cb;text-decoration:underline;');
                                $('#checkval' + indx).attr('style', 'display:block');

                            }
                            else {
                                $('#btnabv' + indx).attr('style', 'display:none');
                                $('#checkval' + indx).attr('style', 'display:block');
                                //  $scope.Characteristics[indx].Abbrivation = $scope.Characteristics[indx].Abbrivation;
                            }
                        }).error(function (data, status, headers, config) {
                            // alert("error");

                        });

                    }

                    //  alert(angular.toJson(response));
                    //  alert(angular.toJson(indx));

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });

                //  alert(angular.toJson($scope.Characteristics));
                $http({
                    method: 'GET',
                    url: '/Catalogue/getunitforvalue',
                    params: { Value: Value }
                }).success(function (response) {
                    if (response != null) {
                        $scope.Characteristics[indx].UOM = response;
                    }
                    // alert(response);
                    //if (response != null) {
                    //    angular.forEach($scope.UOMList, function (valueee) {
                    //        if(valueee == response)
                    //        {
                    //          //  alert("hi");
                    //            $scope.Characteristics[indx].UOM = response;
                    //        }
                    //    });

                    //}
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });




            } else if (Attribute == "TYPE" || Attribute == "MATERIAL") {
                $http({
                    method: 'GET',
                    url: '/GeneralSettings/GetUnspsc',
                    params: { Noun: $scope.cat.Noun, Modifier: $scope.cat.Modifier }
                }).success(function (response) {
                    if (response != '') {
                        $scope.Commodities = response;
                        if ($scope.Commodities[0].Commodity != null && response[0].Commodity != "")
                            $scope.cat.Unspsc = $scope.Commodities[0].Commodity;
                        else $scope.cat.Unspsc = $scope.Commodities[0].Class;
                    }
                    else {
                        $scope.Commodities = null;
                    }

                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            }

            else {

                $('#checkval' + indx).attr('style', 'display:none');
            }

            console.log($scope.Characteristics);
        };
        $scope.AddValue = function (Noun, Modifier, Attribute, Value, abb, indx) {

            // Value = Value.replace('&', '***');
            // abb = abb.replace('&', '***');

            $http({
                method: 'GET',
                url: '/Catalogue/AddValue',
                params: { Noun: Noun, Modifier: Modifier, Attribute: Attribute, Value: Value, abb: abb }
            }).success(function (response) {


                $('#btnabv' + indx).attr('style', 'display:none');
                $('#checkval' + indx).attr('style', 'display:block');

            }).error(function (data, status, headers, config) {
                // alert("error");

            });
        }
        $scope.Uomalert = function (Uom, indx) {

            if (Uom != undefined) {
                var inxx = 0;

                angular.forEach($scope.Characteristics, function (value1, key1) {
                    if (value1.UOM != undefined) {
                        var res = $filter('filter')($scope.UOMs, { 'Attribute': value1.Characteristic }, true);

                        angular.forEach(res, function (value2, key2) {
                            // alert(angular.toJson(value2.UOMList))
                            if (value2.UOMList != undefined && value2.UOMList.indexOf(Uom) != -1 && value1.UOM != Uom) {

                                $('#U' + inxx).attr('style', 'display:block;border:2px solid #f7ac02;border-radius: 10px;');
                            } else {
                                $('#U' + inxx).attr('style', 'display:block;background:none;');
                            }
                        });

                    }
                    inxx = inxx + 1;
                });
            }
        }
        $scope.Characteristics = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
        $scope.addattriRow = function () {

            var characterList = [];
            $scope.charcheck = false;
            angular.forEach($scope.Characteristics, function (lst) {
                //alert(angular.toJson(lst.Characteristic));
                if (characterList.indexOf(lst.Characteristic) !== -1) {
                    $scope.charcheck = true;
                }
                characterList.push(lst.Characteristic);


            });
            if ($scope.charcheck == false)
                $scope.Characteristics.push({ 'New':'New','Squence': $scope.Characteristics.length + 1, 'ShortSquence': $scope.Characteristics.length + 1, 'Remove': 0 });


        };

    

        $scope.identifyremove_rows = function (index) {
            if ($scope.Characteristics[index].Remove == 0) {
                $scope.Characteristics[index].Remove = 1;
            } else {
                $scope.Characteristics[index].Remove = 0;
            }

            //   alert(angular.toJson($scope.rows));

        };
        $scope.fromsave = 0;
        $scope.searchMaster = function () {
            $scope.showlist = false;
            $timeout(function () {

                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);

            var formData = new FormData();
            formData.append("sCode", $scope.sCode);
            formData.append("sSource", $scope.sSource);
            formData.append("sUser", $scope.sUser);
            formData.append("sStatus", $scope.sStatus);
            formData.append("sQR", $scope.qr);
          
            if (($scope.sCode != undefined && $scope.sCode != '') || ($scope.sSource != undefined && $scope.sSource != '') || ($scope.sUser != undefined && $scope.sUser != '') || ($scope.sStatus != undefined && $scope.sStatus != '') || ($scope.qr != undefined && $scope.qr != ''))
            {

                //$(".loaderb_div").show();
                $rootScope.cgBusyPromises = $http({
                    method: 'POST',
                    url: '/FAR/searchMaster',
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData,
                }).success(function (response) {

                    $scope.AssetdataList = response;
                    console.log($scope.AssetdataList)
                    

                    if (response != null && response.length > 0) {
                    
                        if ($scope.fromsave === 1) {
                            $scope.fromsave = 0;
                            $scope.Res = "Data Saved successfully";
                           
                        }
                        else {
                            $scope.Res = "Search result : " + response.length + " records found";
                        }

                        $scope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');


                    } else {
                        $scope.Res = "Records not found";
                        $scope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                   
                }).error(function (data, status, headers, config) {
                    // alert("error");

                });
            } else {

                $scope.LoadData();

            }

        }
        $scope.Loadnameplate = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };

        //


        $scope.getRowClass = function (cat1, index) {
            if (cat1.AdditionalNotes === 'EXISTING ASSETS (TAGGED)_GROUPING/PROXY_MAPPED') {
                return 'tr-proxy';
            }
            if (cat1.ItemStatus === 2 && (cat1.Rework === 'Cat' || cat1.Rework === 'PV')) {
                return 'trrework';
            }
            if (cat1.ItemStatus === 3 || cat1.ItemStatus === 5) {
                return 'trsaved';
            }
            if ((cat1.ItemStatus === 2 || cat1.ItemStatus === 3 || cat1.ItemStatus === 4 || cat1.ItemStatus === 5) && $scope.isSelectedRow(index)) {
                return 'selected';
            }
            return '';
        };



        $scope.UploadnameplateImage = function (Uid) {
           
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
              //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);


                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadnameplatImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                  //  $scope.getnameplateimages()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
        $scope.UploadnameplateImage2 = function (Uid) {
           
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
              //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);


                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadnameplatImg2",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                  //  $scope.getnameplateimages()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
        $scope.getnameplateimages = function () {
            $rootScope.promise1 = $http({
                method: 'GET',
                url: "/FAR/getnameplateImges",
            }).success(function (response) {

                $scope.NameplateImgs = response;

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.getnameplateimages();
        $scope.RemovenameplateImg = function (fle, Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;

                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteNPImg?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {

                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {

                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };
        $scope.RemovenameplateImg2 = function (fle, Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;

                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteNPImg2?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {

                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {

                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };

        //asset images
        $scope.LoadAssetImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
           
            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadMaximoAssetImg = function (Uid) {
           

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
              //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadMaximoAssetImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                  

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }





                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };

        }
        $scope.UploadAssetImg = function (Uid) {
           

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
              //  $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadAssetImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                  

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }





                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };

        }

        $scope.openMap = function (longitude, latitude) {
            if (longitude) {
                if (longitude.includes('W')) {
                    longitude = longitude.replace('W', 'E');
                }
            }
            var url = 'https://www.google.com/maps?q=' + longitude + ',' + latitude;
            window.open(url, '_blank');
        };

        $scope.getAssetImg = function () {
            $rootScope.promise1 = $http({
                method: 'GET',
                url: "/FAR/getAssetImg",
            }).success(function (response) {

                $scope.AssetImgs = response;

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.getAssetImg();
        $scope.RemoveMaximoAssetImg = function (fle,Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;
           
                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteMaximoAssetImg?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {
                  
                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {
                      
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };
        $scope.RemoveAssetImg = function (fle,Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;
           
                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteAssetImg?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {
                  
                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {
                      
                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };

        //oldTag
        $scope.LoadOldTagImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadOldTagImg = function (Uid) {
          

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
             

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadOldTagImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                  //  $scope.getOldTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }





                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
        $scope.getOldTagImg = function () {
            $rootScope.promise1 = $http({
                method: 'GET',
                url: "/FAR/getOldTagImg",
            }).success(function (response) {

                $scope.OldTagImgs = response;

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.getOldTagImg();
        $scope.RemoveOldTagImg = function (fle,Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;

                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteOldTagImg?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {

                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {

                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };

        //NewTag
        $scope.LoadNewTagImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadNewTagImg = function (Uid) {
          

            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
             

                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadNewTagImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                 //   $scope.getNewTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }





                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }
        $scope.getNewTagImg = function () {
            $rootScope.promise1 = $http({
                method: 'GET',
                url: "/FAR/getNewTagImg",
            }).success(function (response) {

                $scope.NewTagImgs = response;

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.getNewTagImg();
        $scope.RemoveNewTagImg = function (fle,Uid) {

            if ($window.confirm("Do you want to delete the image?")) {

                $scope.ShowHide = true;

                $rootScope.promise = $http({
                    method: 'GET',
                    url: '/FAR/DeleteNewTagImg?FileName=' + fle + "&Uid=" + Uid
                }).success(function (data) {

                    $('#divNotifiy').attr('style', 'display: block');
                    if (data.includes("Error : ")) {

                        $scope.Res = data;
                        $scope.Notify = "alert-danger";
                        $scope.NotifiyRes = true;
                    } else {

                        $scope.Res = data
                        $scope.Notify = "alert-info";
                        $scope.NotifiyRes = true;
                    }
                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = data;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            }

        };

        //Bom old tag img 

        $scope.LoadBomOldTagImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadBomOldTagImg = function (Uid) {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;


                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadBomOldTagImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    //   $scope.getNewTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

        $scope.LoadBomNamePlateImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadBomNamePlateImg = function (Uid) {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;


                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadBomNamePlateImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    //   $scope.getNewTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }

        $scope.LoadBomImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadBomImg = function (Uid) {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;


                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadBomImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    //   $scope.getNewTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }


        $scope.LoadBomBarCodeImg = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid Image";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.UploadBomBarCodeImg = function (Uid) {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;


                var formData = new FormData();
                formData.append('image', $scope.files[0]);
                formData.append('UniqueId', Uid);

                $rootScope.cgBusyPromises = $http({
                    url: "/FAR/UploadBomBarCodeImg",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    //   $scope.getNewTagImg()

                    $scope.ShowHide = false;
                    if (data.includes("Error : ")) {

                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                    }

                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;


                });
            };
        }


        //Autofill
        $scope.autoFill = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetAttributeinfo',
                params: { UniqueId: $scope.asset.UniqueId }
            }).success(function (response) {

                //alert(angular.toJson(response))
                $scope.dumbCharacteristics = response.Characterisitics;
                $scope.exCharacteristics = response.exCharacterisitics;
                //alert($scope.exCharacteristics);
                //alert($scope.dumbCharacteristics);
                $scope.dumbNoun = $scope.cat.Noun;
                $scope.dumbModifier = $scope.cat.Modifier;
                $scope.exNoun = response.exNoun;
                $scope.exModifier = response.exModifier;

                //alert($scope.dumbNoun + "," + $scope.exNoun);
                //alert($scope.dumbModifier+","+$scope.exModifier);
                //alert($scope.exCharacteristics);
                //alert($scope.dumbCharacteristics);
                if ($scope.dumbNoun == $scope.exNoun && $scope.dumbModifier == $scope.exModifier) {
                    //$scope.lstCharateristics1 = [];

                    if ($scope.exCharacteristics !== null && $scope.dumbCharacteristics !== null) {
                        //alert("Fill")
                        angular.forEach($scope.exCharacteristics, function (ex) {
                            angular.forEach($scope.dumbCharacteristics, function (ne) {
                                if (ex.Characteristic === ne.Characteristic) {
                                    //alert(ne.Value)
                                    return ne.Value = ex.Value;
                                }
                            });
                        });
                    }
                    //$scope.Characteristics = $scope.dumbCharacteristics;

                    alert(angular.toJson($scope.dumbCharacteristics));
                    $scope.Characteristics.forEach(function (exChar) {
                        $scope.dumbCharacteristics.forEach(function (char) {
                            if (exChar.Characteristic === char.Characteristic) {
                                exChar.Value = char.Value;
                                exChar.UOM = char.UOM;
                                exChar.Squence = char.Squence;
                                exChar.ShortSquence = char.ShortSquence;
                                exChar.Source = char.Source;
                                exChar.SourceUrl = char.SourceUrl;
                                exChar.Approve = char.Approve;
                                exChar.Abbrevated = char.Abbrevated;
                                exChar.Uom = char.Uom;
                                exChar.UomMandatory = char.UomMandatory;
                            }
                        });
                    });
                    //alert(angular.toJson($scope.Characteristics));
                    //$scope.Characterisitics = $scope.dumbCharacteristics;

                    //var mergedList = [];

                    //if ($scope.cat.exCharacteristics.length > 0 && $scope.Characteristics.length > 0) {
                    //    var commonKeys = Object.keys($scope.cat.exCharacteristics[0]).filter(key => $scope.Characteristics[0].hasOwnProperty(key));

                    //    if (commonKeys.length > 0) {
                    //        mergedList = $scope.Characteristics.map(item => {
                    //            var newItem = {};
                    //            commonKeys.forEach(key => {
                    //                newItem[key] = $scope.cat.exCharacteristics[0][key];
                    //            });
                    //            Object.keys(item).forEach(key => {
                    //                if (!commonKeys.includes(key)) {
                    //                    newItem[key] = item[key];
                    //                }
                    //            });
                    //            return newItem;
                    //        });
                    //    }
                    //} else {
                    //    // Handle the case where either dumblist or list is empty
                    //}

                    //console.log(mergedList);


                    //$scope.Characteristics = $scope.lstCharateristics1;
                } else {
                    $scope.Res = "Noun and Modifier don't match";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
                //alert(angular.toJson("new:" + $scope.dumbNoun + ",old:" + $scope.cat.exNoun))
                //alert(angular.toJson("new:" + $scope.dumbModifier + ",old:" + $scope.cat.exModifier))
                //alert(angular.toJson("new:" + $scope.dumbCharacteristics + ",old:" + $scope.exCharacteristics))
            }).error(function (data, status, headers, config) {
                // alert("error");

            });

        }


        //Download images

        $scope.downloadImages = function (uniqueId, imageUrls) {
            var fileCount = 1;

            angular.forEach(imageUrls, function (images, folderName) {
                if (angular.isArray(images)) {
                    angular.forEach(images, function (url) {
                        downloadImage(url);
                    });
                }
            });

            function downloadImage(url) {
                var filename = url.substring(url.lastIndexOf('/') + 1);
                var downloadLink = document.createElement('a');
                downloadLink.href = url;
                downloadLink.download = 'common_folder/' + uniqueId + '_' + (fileCount++) + '_' + filename;
                downloadLink.target = '_blank';
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            }
        };


        //Generate QR

        $scope.GenerateQR = function (uniqueId, assetNo, lst, idx) {
            const payload = {
                AssetNo: assetNo,
                UniqueId: uniqueId,
                fileName: ""
            };

            $scope.cgBusyPromises = $http.post("/FAR/GenerateQRCode", payload)
                .then(function (response) {
                    $('#divNotifiy').show();
                    $scope.ShowHide = false;

                    if (response.data.success) {
                        $rootScope.Res = "QR Image Generated";
                        $rootScope.Notify = "alert-info";
                        $scope.RowClick(lst, idx);
                    } else {
                        $rootScope.Res = response.data.error || "QR Generation Failed";
                        $rootScope.Notify = "alert-danger";
                    }

                    $rootScope.NotifiyRes = true;
                    $('.fileinput').fileinput('clear');
                })
                .catch(function (error) {
                    $scope.ShowHide = false;
                    $rootScope.Res = error?.data || "Request failed";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                });
        };

        $scope.notMfr = false;
        $scope.mfrBtn = "Add";

        $scope.checkMfr = function (term) {
            $http({
                method: "GET",
                url: '/FAR/GetMfr',
                params: { Label: "Manufacturer", Term: term }
            }).then(function (response) {
                var mfr = response.data || [];

                console.log(mfr);
                console.log($scope.asset.ItemStatus);

                if (mfr.length === 0) {
                    $scope.notMfr = true;
                    $scope.mfrBtn = "Add";
                } else if (
                    ($scope.asset.ItemStatus === 4 || $scope.asset.ItemStatus === 5) &&
                    mfr[0].Islive === false
                ) {
                    $scope.notMfr = true;
                    $scope.mfrBtn = "Approve";
                } else {
                    $scope.notMfr = false;
                }
            }).catch(function (error) {
                console.error("Error checking manufacturer:", error);
                $scope.notMfr = false;
            });
        };

        $scope.AddMfr = function (term) {
            $scope.mfr = {};
            $scope.mfr.Label = "Manufacturer";
            var formData = new FormData();
            $scope.mfr.Code = term;
            formData.append("data", angular.toJson($scope.mfr));

            return $http({
                url: "/FAR/InsertMfr",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (response) {
                if (response.includes("successfully")) {
                    $rootScope.Res = response;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.reset();
                    $scope.obj = null;
                    $scope.mfr = null;
        $scope.notMfr = false;
                }
                else {
                    $rootScope.Res = response;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            });
        }
    })

    app.directive('capitalize', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                function cleanInput(inputValue) {
                    if (!inputValue) return '';
                    return inputValue.trim().replace(/\s+/g, ' ').toUpperCase();
                }

                function transformAndRender(value) {
                    var cleaned = cleanInput(value);
                    if (cleaned !== value) {
                        const el = element[0];
                        const start = el.selectionStart;
                        const end = el.selectionEnd;

                        modelCtrl.$setViewValue(cleaned);
                        modelCtrl.$render();

                        setCursorPosition(el, start, end, value, cleaned);
                    }
                    return cleaned;
                }

                function setCursorPosition(el, start, end, oldVal, newVal) {
                    const diff = newVal.length - oldVal.length;
                    const newPos = start + diff;

                    setTimeout(() => {
                        el.setSelectionRange(newPos, newPos);
                    });
                }

                modelCtrl.$parsers.push(transformAndRender);

                element.on('blur', function () {
                    var value = element.val();
                    var cleaned = cleanInput(value);
                    scope.$applyAsync(() => {
                        modelCtrl.$setViewValue(cleaned);
                        modelCtrl.$render();
                    });
                });

                element.on('paste', function () {
                    setTimeout(function () {
                        var value = element.val();
                        var cleaned = cleanInput(value);
                        scope.$applyAsync(() => {
                            modelCtrl.$setViewValue(cleaned);
                            modelCtrl.$render();
                        });
                    }, 0);
                });

                var initialValue = scope.$eval(attrs.ngModel);
                if (initialValue) {
                    transformAndRender(initialValue);
                }
            }
        };
    });


    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteAssetNoun?term=" + term,
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

    app.factory("AutoCompleteMfrService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    method: "GET",
                    url: '/FAR/GetMfr',
                    params: { Label: "Manufacturer", Term: term }
                }).success(function (response) {
                    console.log(response);
                    return response;
                });
            }
        };
    }]);

    app.directive("autoComplete2", ["AutoCompleteMfrService", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {
                        AutoCompleteService.search(searchTerm.term).success(function (autocompleteResults) {
                            const filteredResults = autocompleteResults.filter(function (item) {
                                return item.Label === "Manufacturer" &&
                                    item.Code.toLowerCase().includes(searchTerm.term.toLowerCase());
                            });

                            response($.map(filteredResults, function (item) {
                                return {
                                    label: item.Code,
                                    value: item.Code
                                };
                            }));
                        });
                    },
                    minLength: 1
                });
            }
        };
    }]);


    app.filter('uniqueFilter', function () {
        return function (input) {
            var uniqueArray = [];
            input.forEach(function (item) {
                if (uniqueArray.indexOf(item) === -1) {
                    uniqueArray.push(item);
                }
            });
            return uniqueArray;
        };
    });

    app.directive('treeNode', function () {
        return {
            restrict: 'E',
            scope: {
                node: '='
            },
            template: `
      <li>
        <div ng-click="toggleNode(node)">
          <span ng-if="node.children.length">
            <strong>[{{ node.expanded ? '-' : '+' }}]</strong>
          </span>
          {{ node.name }} ({{ node.Category }})
        </div>

        <ul ng-if="node.expanded">
          <tree-node ng-repeat="child in node.children" node="child"></tree-node>
        </ul>
      </li>
    `,
            controller: function ($scope) {
                $scope.toggleNode = function (node) {
                    node.expanded = !node.expanded;
                };
            }
        };
    });

})();