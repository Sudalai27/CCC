
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables']);



    app.controller('AccountController', function ($scope, $http,$rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Acc Asignmt Category'}
            }).success(function (response) {
                $scope.Accounts = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Acc Asignmt Category";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id:_id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Acc Asignmt Category";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                   
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
              
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id , Islive:  enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Accounts[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params : {id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Accounts[idx].Islive = false;


            }

        };
    });
    app.controller('DeliveringPlantController', function ($scope, $http, $rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindPlantList = function () {
            $http({
                method: 'GET',
                url: '/Master/GetDataListplnt'
            }).success(function (response) {
                $scope.getplant = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindPlantList();
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'Delivering Plant' }
            }).success(function (response) {
                $scope.DeliveringPlnt = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickDeliveringPlant = function () {
           // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData1 = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Delivering Plant";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id }
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#DeliveringPlant' + i).attr("style", "");
                i++;
            });
            $('#DeliveringPlant' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Delivering Plant";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.DeliveringPlnt[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.DeliveringPlnt[idx].Islive = false;


            }

        };
    });
    app.controller('Taxclassification2Controller', function ($scope, $http, $rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'Tax classification 2' }
            }).success(function (response) {
                $scope.Taxclasification2= response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickTaxclassification2 = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Tax classification 2";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id }
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Taxclasification2, function (lst) {
                $('#Taxclassification2' + i).attr("style", "");
                i++;
            });
            $('#Taxclassification2' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Tax classification 2";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Taxclasification2[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Taxclasification2[idx].Islive = false;


            }

        };
    });
    app.controller('TransportationGroupController', function ($scope, $http, $rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'Transportation Group' }
            }).success(function (response) {
                $scope.TransportationGrp= response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickTransportationGroup = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Transportation Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id }
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.TransportationGrp, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Transportation Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.TransportationGrp[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.TransportationGrp[idx].Islive = false;


            }

        };
    });
    app.controller('LoadingGroupController', function ($scope, $http, $rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'Loading Group' }
            }).success(function (response) {
                $scope.LoadingGrp= response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickLoadingGroup = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Loading Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id }
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#LoadingGroup' + i).attr("style", "");
                i++;
            });
            $('#LoadingGroup' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Loading Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.LoadingGrp[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.LoadingGrp[idx].Islive = false;


            }

        };
    });
    app.controller('OrderUnitController', function ($scope, $http, $rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'Order Unit' }
            }).success(function (response) {
                $scope.OrdrUnit= response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickOrderUnit = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Order Unit";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id }
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#OrderUnit' + i).attr("style", "");
                i++;
            });
            $('#OrderUnit' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Order Unit";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {

                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {

            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.OrdrUnit[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.OrdrUnit[idx].Islive = false;


            }

        };
    });
    app.controller('TaxController', function ($scope, $http,$rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Tax Classification Group'}
            }).success(function (response) {
                $scope.Taxes = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickTax = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Tax Classification Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                  
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id: _id}
                }).success(function (response) {
                    $scope.Res = "Data deleted";
                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {
            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Tax Classification Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                  
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id:_id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Taxes[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Taxes[idx].Islive = false;


            }

        };
    });
    app.controller('ItemController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Item Category Group'}
            }).success(function (response) {
                $scope.Items = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickItemCategory = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Item Category Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
                
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id:_id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Item Category Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Items[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Items[idx].Islive = false;


            }

        };
    });
    app.controller('SalesController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }


        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Sales Organization'}
            }).success(function (response) {
                $scope.Sales = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickSales = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Sales Organization";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                    
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id: _id}

                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Sales Organization";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                  
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
                
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Sales[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Sales[idx].Islive = false;


            }

        };
    });
    app.controller('DistributionController', function ($scope, $http,$rootScope, $timeout, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Distribution Channel'}
            }).success(function (response) {
                $scope.Distributions = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        $scope.BindMaster = function () {
         
            $http({
                method: 'GET',
                url: '/Master/GetMaster'
            }).success(function (response) {

                $scope.MasterList = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
            //sl
            //$http({
            //    method: 'GET',
            //    url: '/Master/GetDataList?label=Storage location'
            //}).success(function (response) {
            //    $scope.strloc = response;
            //}).error(function (data, status, headers, config) {
            //    // alert("error");
            //});

        };
        $scope.BindMaster();
        $rootScope.onclickDistribution = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Distribution Channel";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                  
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
                
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id:_id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Distribution Channel";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                   
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });

            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            // }

        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Distributions[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params :{id: _id ,Islive:enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Distributions[idx].Islive = false;


            }

        };
    });
    app.controller('MaterialController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Material Strategic Group'}
            }).success(function (response) {
                $scope.Materials = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickMaterial = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Material Strategic Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                   
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
             
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id: _id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Material Strategic Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                   
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                }
            }).error(function (data, status, headers, config) {
              
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {

            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params : {id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Materials[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params : {id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Materials[idx].Islive = false;


            }

        };
    });
    app.controller('PurchasingController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Purchasing Group'}
            }).success(function (response) {
                $scope.Purchasings = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickPurchasingGroup = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Purchasing Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                   
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;

                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                }
            }).error(function (data, status, headers, config) {
               
            });

            // }
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params :{id:_id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');

                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {

            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Purchasing Group";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');

                }
            }).error(function (data, status, headers, config) {
              
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params : {id: _id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Purchasings[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Purchasings[idx].Islive = false;


            }

        };
    });
    app.controller('ValueKeyController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Purchasing Value Key'}
            }).success(function (response) {
                $scope.ValueKeys = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.onclickPurchasing = function () {
            // $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {
            $scope.form.$setPristine();
        }
        $scope.createData = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Purchasing Value Key";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));
            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {
                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
               
            });
        };
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/Master/DelData',
                    params: { id: _id}
                }).success(function (response) {
                    $rootScope.Res = "Data deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $scope.BindList();
                    $('#divNotifiy').attr('style', 'display: block');
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            }
        };
        $scope.DataEdit = function (x, idx) {
            var i = 0;
            angular.forEach($scope.Accounts, function (lst) {
                $('#acc' + i).attr("style", "");
                i++;
            });
            $('#acc' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Purchasing Value Key";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/Master/InsertData",
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                }
                else {
                    if (data === false)
                        $rootScope.Res = "Data already exists";
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                    }
                    $scope.reset();
                    $scope.obj = null;
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            }).error(function (data, status, headers, config) {
            
            });
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
        };
        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableData = function (idx, enable, _id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.ValueKeys[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.ValueKeys[idx].Islive = false;


            }

        };
    });
})();