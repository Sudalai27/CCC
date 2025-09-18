(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables']);



    app.controller('MrptypeController', function ($scope, $http, $timeout, $rootScope,DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);
   
        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Mrp type'}
            }).success(function (response) {
                $scope.Mrptyps = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };

       

        $scope.BindList();
        $scope.reset = function () {

            $scope.form.$setPristine();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Mrp type";
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
                    $scope.Res = data.errors;
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Mrptyps, function (lst) {
                $('#mrpt' + i).attr("style", "");
                i++;
            });
            $('#mrpt' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Mrp type";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.NotifiyRes = true;
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                        params :{id:_id,Islive: enable}
                    }).success(function (response) {
                        $rootScope.Res = "Data disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Mrptyps[idx].Islive = true;

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
                } else $scope.Mrptyps[idx].Islive = false;


            }

        };
    });
    app.controller('MrpconController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

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
                params :{label:'Mrp controller'}
            }).success(function (response) {
                $scope.Mrpcons = response;
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
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Mrp controller";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.NotifiyRes = true;
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.NotifiyRes = true;
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Mrpcons, function (lst) {
                $('#mrpc' + i).attr("style", "");
                i++;
            });
            $('#mrpc' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Mrp controller";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.NotifiyRes = true;
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.NotifiyRes = true;
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Mrpcons[idx].Islive = true;

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
                } else $scope.Mrpcons[idx].Islive = false;


            }

        };
    });
    app.controller('LotsizeController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Lot size'}
            }).success(function (response) {
                $scope.Lotsizes = response;
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

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Lot size";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Lotsizes, function (lst) {
                $('#lot' + i).attr("style", "");
                i++;
            });
            $('#lot' + idx).attr("style", "background-color:lightblue");

            $scope.obj = {};

            $scope.btnSubmit = false;
            $scope.btnUpdate = true;

            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;


        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Lot size";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Lotsizes[idx].Islive = true;

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
                } else $scope.Lotsizes[idx].Islive = false;


            }

        };
    });
    app.controller('ProcurementController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Procurement type'}
            }).success(function (response) {
                $scope.Procures = response;
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

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            $scope.obj.Label = "Procurement type";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Procures, function (lst) {
                $('#pro' + i).attr("style", "");
                i++;
            });
            $('#pro' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Procurement type";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Procures[idx].Islive = true;

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
                } else $scope.Procures[idx].Islive = false;


            }

        };
    });
    app.controller('PlanningController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

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
                params :{label:'Planning strgy'}
            }).success(function (response) {
                $scope.Planstragys = response;
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

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Planning strgy";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: '/Master/InsertData',
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                  
                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Planstragys, function (lst) {
                $('#plnstr' + i).attr("style", "");
                i++;
            });
            $('#plnstr' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Planning strgy";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Planstragys[idx].Islive = true;

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
                } else $scope.Planstragys[idx].Islive = false;


            }

        };
    });
    app.controller('AvalibleController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

        // DataTables configurable options
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDisplayLength(10)
            .withOption('bLengthChange', true);

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params :{label:'Avali check'}
            }).success(function (response) {
                $scope.Avalibles = response;
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

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Avali check";
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
                    $rootScope.Res = data.errors;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Avalibles, function (lst) {
                $('#ava' + i).attr("style", "");
                i++;
            });
            $('#ava' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $scope.obj.Label = "Avali check";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Avalibles[idx].Islive = true;

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
                } else $scope.Avalibles[idx].Islive = false;


            }

        };
    });
    app.controller('SchduleController', function ($scope, $http, $timeout,$rootScope, DTOptionsBuilder) {

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
                params :{label:'Schedule Margin'}
            }).success(function (response) {
                $scope.Schedules = response;
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

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            $scope.obj.Label = "Schedule Margin";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: '/Master/InsertData',
                method: "POST",
                headers: { "Content-Type": undefined },
                transformRequest: angular.identity,
                data: formData
            }).success(function (data, status, headers, config) {

                if (data.success === false) {
                }
                else {
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data created successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
            angular.forEach($scope.Schedules, function (lst) {
                $('#sch' + i).attr("style", "");
                i++;
            });
            $('#sch' + idx).attr("style", "background-color:lightblue");
            $scope.obj = {};
            $scope.btnSubmit = false;
            $scope.btnUpdate = true;
            $scope.obj._id = x._id;
            $scope.obj.Code = x.Code;
            $scope.obj.Title = x.Title;
        };
        $scope.updateData = function () {

            //if (!$scope.form.$invalid) { 
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 30000);
            $scope.obj.Label = "Schedule Margin";
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
                    if (data === false) {
                        $rootScope.Res = "Data already exists";
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    else {
                        $rootScope.Res = "Data updated successfully";
                        $scope.BindList();
                        $rootScope.Notify = "alert-info";
                        $rootScope.NotifiyRes = true;
                        $('#divNotifiy').attr('style', 'display: block');
                    }
                    $scope.reset();
                    $scope.obj = null;
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
                } else $scope.Schedules[idx].Islive = true;

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
                } else $scope.Schedules[idx].Islive = false;


            }

        };
    });
  


})();