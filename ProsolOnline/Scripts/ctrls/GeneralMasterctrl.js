
(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables']);


    app.controller('IndustrysectorController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        
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
                params :{label: 'Industry sector'},
            }).success(function (response) {
                $scope.Industrysectors = response;
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
            $scope.obj.Label = "Industry sector";
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
                    params: { id: _id }
                    //?id=' + _id
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
            angular.forEach($scope.Industrysectors, function (lst) {
                $('#inds' + i).attr("style", "");
                i++;
            });
            $('#inds' + idx).attr("style", "background-color:lightblue");
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
            $scope.obj.Label = "Industry sector";
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
                        //?id=' + _id + '&Islive=' + enable

                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Industrysectors[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params: { id: _id, Islive: enable }
                        //?id=' + _id + '&Islive=' + enable
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Industrysectors[idx].Islive = false;


            }

        };
    });
    app.controller('MaterialtypeController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

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
                params :{label:'Material type'}
            }).success(function (response) {
                $scope.Materials = response;
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
            $scope.obj.Label = "Material type";
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
                    params: { id: _id }
                    //?id=' + _id
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
            angular.forEach($scope.Materials, function (lst) {
                $('#mat' + i).attr("style", "");
                i++;
            });
            $('#mat' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Material type";
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
                        //?id=' + _id + '&Islive=' + enable
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
                        params: { id: _id, Islive: enable }
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
    app.controller('BaseuopController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params:{label:'Base uop'}
            }).success(function (response) {
                $scope.Baseuops = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Base uop";
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
                    params : {id: _id}
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
            angular.forEach($scope.Baseuops, function (lst) {
                $('#bas' + i).attr("style", "");
                i++;
            });
            $('#bas' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Base uop";
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
                        params : {id:_id,Islive:enable}
                        //?id=' + _id + '&Islive=' + enable
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Baseuops[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params : {id:_id,Islive:enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Baseuops[idx].Islive = false;


            }

        };
    });
    app.controller('UnitofissueController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

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
                params :{label:'Unit of issue'}
            }).success(function (response) {
                $scope.Units = response;
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
            $scope.obj.Label = "Unit of issue";
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
            angular.forEach($scope.Units, function (lst) {
                $('#unt' + i).attr("style", "");
                i++;
            });
            $('#unt' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Unit of issue";
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
                        params:{id:_id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Units[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params:{id:_id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Units[idx].Islive = false;


            }

        };
    });
    app.controller('AlternateuomController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Alternate uom'}
            }).success(function (response) {
                $scope.Alters = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; },5000);
            $scope.obj.Label = "Alternate uom";
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
            angular.forEach($scope.Alters, function (lst) {
                $('#alt' + i).attr("style", "");
                i++;
            });
            $('#alt' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Alternate uom";
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
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Alters[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                        params:{id:_id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Alters[idx].Islive = false;


            }

        };
    });
    app.controller('InspectiontypeController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Inspection type'}
            }).success(function (response) {
                $scope.Instyps = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Inspection type";
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
            angular.forEach($scope.Instyps, function (lst) {
                $('#inst' + i).attr("style", "");
                i++;
            });
            $('#inst' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Inspection type";
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
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Instyps[idx].Islive = true;

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
                } else $scope.Instyps[idx].Islive = false;


            }

        };
    });
    app.controller('InspectioncodeController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Inspection code'}
            }).success(function (response) {
                $scope.Inspcods = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Inspection code";
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
            angular.forEach($scope.Inspcods, function (lst) {
                $('#insc' + i).attr("style", "");
                i++;
            });
            $('#insc' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Inspection code";
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
                } else $scope.Inspcods[idx].Islive = true;

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
                } else $scope.Inspcods[idx].Islive = false;


            }

        };
    });
    app.controller('DivisionController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Division'}
            }).success(function (response) {
                $scope.Divisions = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Division";
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
            angular.forEach($scope.Divisions, function (lst) {
                $('#div' + i).attr("style", "");
                i++;
            });
            $('#div' + idx).attr("style", "background-color:lightblue");

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
            $scope.obj.Label = "Division";
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
                        params: { id: _id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Divisions[idx].Islive = true;

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
                } else $scope.Divisions[idx].Islive = false;


            }

        };
    });
    app.controller('SalesunitController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Sales unit'}
            }).success(function (response) {
                $scope.Salesunits = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }
        $scope.reset = function () {
            $scope.form.$setPristine();
        }
        $scope.createData = function () {
            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Sales unit";
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
                     params:{id:_id }
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
            angular.forEach($scope.Salesunits, function (lst) {
                $('#sal' + i).attr("style", "");
                i++;
            });
            $('#sal' + idx).attr("style", "background-color:lightblue");
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
            $scope.obj.Label = "Sales unit";
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
                } else $scope.Salesunits[idx].Islive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/Master/DisableData',
                         params:{id:_id ,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Salesunits[idx].Islive = false;


            }

        };
    });
})();
