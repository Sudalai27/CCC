(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['datatables']);



    app.controller('PlantController', function ($scope, $http,$rootScope, $timeout) {
            $scope.BindList = function () {
                $http({
                    method: 'GET',
                    url: '/Master/GetDataListplnt'
                }).success(function (response) {
                    $scope.Plant = response;
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
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));
                $http({
                    url: "/Master/InsertDataplnt",
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
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                            $scope.BindList();
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
                        url: '/Master/DelDataplnt',
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
                angular.forEach($scope.Plant, function (lst) {
                    $('#plnt' + i).attr("style", "");
                    i++;
                });
                $('#plnt' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Plantcode = x.Plantcode;
                $scope.obj.Plantname = x.Plantname;
            };
            $scope.updateData = function () {
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));

                $http({
                    url: "/Master/InsertDataplnt",
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
                            $rootScope.Notify = "alert-info";
                            $rootScope.NotifiyRes = true;
                            $scope.BindList();
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
            $scope.DisablePlant = function (idx, enable, _id) {
               
                if (enable == false) {

                    if (confirm("Are you sure, disable this record?")) {

                        $http({
                            method: 'GET',
                            url: '/Master/DisablePlant',
                            params :{id: _id ,Islive: enable}
                        }).success(function (response) {
                            $rootScope.Res = "Plant disabled";
                            $rootScope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.BindList();
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    } else $scope.Plant[idx].Islive = true;

                } else {
                    if (confirm("Are you sure, enable this record?")) {

                        $http({
                            method: 'GET',
                            url: '/Master/DisablePlant',
                            params: { id: _id, Islive: enable }
                        }).success(function (response) {
                            $rootScope.Res = "Plant enabled";
                            $rootScope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.BindList();
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    } else $scope.Plant[idx].Islive = false;


                }

            };
        });
    app.controller('ProfitcenterController', function ($scope, $http,$rootScope, $timeout) {

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
        $rootScope.onclickProfit = function () {
            $scope.BindPlantList();
            $scope.BindList();
        }
        
            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params :{label:'Profit center'}
                }).success(function (response) {
                    $scope.Profits = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
           
            $scope.reset = function () {

                $scope.form.$setPristine();
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Profit center";
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
                         
                        }
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
                angular.forEach($scope.Profits, function (lst) {
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
                $scope.obj.Plantcode = x.Plantcode;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Profit center";
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
                    } else $scope.Profits[idx].Islive = true;

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
                    } else $scope.Profits[idx].Islive = false;


                }

            };
        });
    app.controller('StoragelocationController', function ($scope, $http,$rootScope, $timeout) {

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
        $rootScope.onclickStorage = function () {
            $scope.BindPlantList();
            $scope.BindList();
        }

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params :{label:'Storage location'}
                }).success(function (response) {
                    $scope.Storagelocs = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
           // $scope.BindList();
            $scope.reset = function () {

                $scope.form.$setPristine();
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Storage location";
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));

                $http({
                    url: "/Master/InsertDatawithplant",
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
                angular.forEach($scope.Storagelocs, function (lst) {
                    $('#strloc' + i).attr("style", "");
                    i++;
                });
                $('#strloc' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
                $scope.obj.Plantcode = x.Plantcode;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Storage location";
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));

                $http({
                    url: "/Master/InsertDatawithplant",
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
                    } else $scope.Storagelocs[idx].Islive = true;

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
                    } else $scope.Storagelocs[idx].Islive = false;


                }

            };
        });
    app.controller('StoragebinController', function ($scope, $http, $rootScope, $timeout) {

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
        $rootScope.onclickStoragebin = function () {
            $scope.BindPlantList();
            $scope.BindList();
        }
            $scope.BindList = function () {
                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params:{label:'Storage bin'}
                }).success(function (response) {
                    $scope.Storagebins = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params:{label:'Storage location'}
                }).success(function (response) {
                    $scope.strloc = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });

            };
           // $scope.BindList();
            $scope.reset = function () {
                $scope.form.$setPristine();
            }

           

            //$scope.getstoragelocation = function()
            //{
            //    alert($scope.obj.Plantcode);
            //    $scope.strloc = [];
            //    if ($scope.obj.Plantcode != '' && $scope.obj.Plantcode != undefined && $scope.obj.Plantcode != null && $scope.obj.Plantcode != "") {
            //        $http({
            //            method: 'GET',
            //            url: '/Master/getstoragelocation?plant=' + $scope.obj.Plantcode
            //        }).success(function (response) {
            //            $scope.storage = response;
            //            alert(angular.toJson($scope.storage))
            //            angular.forEach($scope.storage, function (lst) {
            //                $scope.strloc.push(lst);
            //            })
            //            alert(angular.toJson($scope.strloc))
            //        }).error(function (data, status, headers, config) {
            //        })
            //    }
            //}

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.createData = function () {
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Storage bin";
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
                angular.forEach($scope.Storagebins, function (lst) {
                    $('#strbin' + i).attr("style", "");
                    i++;
                });
                $('#strbin' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
                $scope.obj.Storagelocationcode = x.Storagelocationcode;
                $scope.obj.Plantcode = x.Plantcode;
            };

            $scope.updateData = function () {
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Storage bin";
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
                    } else $scope.Storagebins[idx].Islive = true;

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
                    } else $scope.Storagebins[idx].Islive = false;


                }

            };
        });
    app.controller('ValuationclassController', function ($scope, $http,$rootScope, $timeout) {

            // DataTables configurable options
        $rootScope.onclickValuation = function () {           
            $scope.BindList();
        }

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
          //  alert(angular.toJson($scope.MasterList))

        };
        $scope.BindMaster();
            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params :{label:'Valuation class'}
                }).success(function (response) {
                    $scope.Valuationclasss = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
            //$scope.BindList();
            $scope.reset = function () {

                $scope.form.$setPristine();
            }

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Valuation class";
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
                angular.forEach($scope.Valuationclasss, function (lst) {
                    $('#valcls' + i).attr("style", "");
                    i++;
                });
                $('#valcls' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
            };
            $scope.updateData = function () {
                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Valuation class";
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
                            params:{id: _id ,Islive:enable}
                        }).success(function (response) {
                            $rootScope.Res = "Data disabled";
                            $rootScope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.BindList();
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    } else $scope.Valuationclasss[idx].Islive = true;

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
                    } else $scope.Valuationclasss[idx].Islive = false;


                }

            };
        });
    app.controller('PricecontrolController', function ($scope, $http,$rootScope, $timeout) {

            // DataTables configurable options
        $rootScope.onclickPrice = function () {
            $scope.BindList();
        }

            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params  :{label:'Price control'}
                }).success(function (response) {
                    $scope.Pricecontrls = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
           // $scope.BindList();
            $scope.reset = function () {

                $scope.form.$setPristine();
            }

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Price control";
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
                angular.forEach($scope.Pricecontrls, function (lst) {
                    $('#prcntrl' + i).attr("style", "");
                    i++;
                });
                $('#prcntrl' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Price control";
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
                    } else $scope.Pricecontrls[idx].Islive = true;

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
                    } else $scope.Pricecontrls[idx].Islive = false;


                }

            };
        });
    app.controller('ValuationcatagoryController', function ($scope, $http,$rootScope, $timeout) {

        $rootScope.onclickValuationcatagory = function () {
            $scope.BindList();
        }
          

            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params :{label:'Valuation catagory'}
                }).success(function (response) {
                    $scope.Valcatgrys = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
           // $scope.BindList();

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.reset = function () {

                $scope.form.$setPristine();
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Valuation catagory";
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
                angular.forEach($scope.Valcatgrys, function (lst) {
                    $('#valcat' + i).attr("style", "");
                    i++;
                });
                $('#valcat' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Valuation catagory";
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
                    } else $scope.Valcatgrys[idx].Islive = true;

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
                    } else $scope.Valcatgrys[idx].Islive = false;


                }

            };
        });
    app.controller('VariencekeyController', function ($scope, $http,$rootScope, $timeout) {

        $rootScope.onclickVariencekey = function () {
            $scope.BindList();
        }
            

            $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataList',
                    params :{label:'Varience key'}
                }).success(function (response) {
                    $scope.Varkeys = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

           // $scope.BindList();
            $scope.reset = function () {

                $scope.form.$setPristine();
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Varience key";
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
                angular.forEach($scope.Varkeys, function (lst) {
                    $('#varkey' + i).attr("style", "");
                    i++;
                });
                $('#varkey' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Code = x.Code;
                $scope.obj.Title = x.Title;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
                $scope.obj.Label = "Varience key";
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
                    } else $scope.Varkeys[idx].Islive = true;

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
                    } else $scope.Varkeys[idx].Islive = false;


                }

            };
        });
    app.controller('DepartmentController', function ($scope, $http,$rootScope, $timeout) {

        $rootScope.onclickDepartment = function () {
            $scope.BindList();
        }
        $scope.BindList = function () {

                $http({
                    method: 'GET',
                    url: '/Master/GetDataListdept'
                }).success(function (response) {
                    $scope.Departments = response;
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            };
            //$scope.BindList();

            $rootScope.NotifiyResclose = function () {
                $('#divNotifiy').attr('style', 'display: none');
            }

            $scope.reset = function () {

                $scope.form.$setPristine();
            }
            $scope.createData = function () {

                //if (!$scope.form.$invalid) {               

                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
              //  $scope.obj.Label = "Varience key";
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));

                $http({
                    url: "/Master/InsertDatawithdept",
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
                        url: '/Master/DelDatadept',
                        params: { id: _id, Islive: enable }
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
                angular.forEach($scope.Departments, function (lst) {
                    $('#dept' + i).attr("style", "");
                    i++;
                });
                $('#dept' + idx).attr("style", "background-color:lightblue");
                $scope.obj = {};
                $scope.btnSubmit = false;
                $scope.btnUpdate = true;
                $scope.obj._id = x._id;
                $scope.obj.Departmentname = x.Departmentname;
            };
            $scope.updateData = function () {

                //if (!$scope.form.$invalid) { 
                $timeout(function () { $scope.NotifiyRes = false; }, 30000);
               // $scope.obj.Label = "Varience key";
                var formData = new FormData();
                formData.append("data", angular.toJson($scope.obj));

                $http({
                    url: "/Master/InsertDatawithdept",
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
            $scope.DisableDept = function (idx, enable, _id) {

                if (enable == false) {

                    if (confirm("Are you sure, disable this record?")) {

                        $http({
                            method: 'GET',
                            url: '/Master/DisableDept',
                            params: { id: _id, Islive: enable }
                        }).success(function (response) {
                            $rootScope.Res = "Department disabled";
                            $rootScope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.BindList();
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    } else $scope.Departments[idx].Islive = true;

                } else {
                    if (confirm("Are you sure, enable this record?")) {

                        $http({
                            method: 'GET',
                            url: '/Master/DisableDept',
                            params: { id: _id, Islive: enable }
                        }).success(function (response) {
                            $rootScope.Res = "Department enabled";
                            $rootScope.Notify = "alert-info";
                            $('#divNotifiy').attr('style', 'display: block');
                            $scope.BindList();
                        }).error(function (data, status, headers, config) {
                            // alert("error");
                        });
                    } else $scope.Departments[idx].Islive = false;


                }

            };
    });
    //app.controller('DistributionController', function ($scope, $http, $rootScope, $timeout) {

    //    $rootScope.onclickDistribution = function () {
    //        $scope.BindList();
    //    }


    //    $scope.BindList = function () {

    //        $http({
    //            method: 'GET',
    //            url: '/Master/GetDataList',
    //            params: { label: 'Varience key' }
    //        }).success(function (response) {
    //            $scope.Distributions= response;
    //        }).error(function (data, status, headers, config) {
    //            // alert("error");
    //        });
    //    };

    //    $rootScope.NotifiyResclose = function () {
    //        $('#divNotifiy').attr('style', 'display: none');
    //    }

    //    // $scope.BindList();
    //    $scope.reset = function () {

    //        $scope.form.$setPristine();
    //    }
    //    $scope.createData = function () {

    //        //if (!$scope.form.$invalid) {               

    //        $timeout(function () { $scope.NotifiyRes = false; }, 30000);
    //        $scope.obj.Label = "Distribution Channel";
    //        var formData = new FormData();
    //        formData.append("data", angular.toJson($scope.obj));

    //        $http({
    //            url: "/Master/InsertData",
    //            method: "POST",
    //            headers: { "Content-Type": undefined },
    //            transformRequest: angular.identity,
    //            data: formData
    //        }).success(function (data, status, headers, config) {

    //            if (data.success === false) {

    //            }
    //            else {
    //                if (data === false)
    //                    $rootScope.Res = "Data already exists";
    //                else {
    //                    $rootScope.Res = "Data created successfully";
    //                    $scope.BindList();
    //                }
    //                $scope.reset();
    //                $scope.obj = null;

    //                $rootScope.Notify = "alert-info";
    //                $rootScope.NotifiyRes = true;
    //                $('#divNotifiy').attr('style', 'display: block');
    //            }
    //        }).error(function (data, status, headers, config) {

    //        });

    //        // }
    //    };
    //    $scope.btnSubmit = true;
    //    $scope.btnUpdate = false;
    //    $scope.DataDel = function (_id) {
    //        if (confirm("Are you sure, delete this record?")) {
    //            $http({
    //                method: 'GET',
    //                url: '/Master/DelData',
    //                params: { id: _id }
    //            }).success(function (response) {
    //                $rootScope.Res = "Data deleted";
    //                $rootScope.Notify = "alert-info";
    //                $rootScope.NotifiyRes = true;
    //                $scope.BindList();
    //                $('#divNotifiy').attr('style', 'display: block');
    //            }).error(function (data, status, headers, config) {
    //                // alert("error");
    //            });
    //        }
    //    };
    //    $scope.DataEdit = function (x, idx) {
    //        var i = 0;
    //        angular.forEach($scope.Distributions, function (lst) {
    //            $('#Distributions' + i).attr("style", "");
    //            i++;
    //        });
    //        $('#varkey' + idx).attr("style", "background-color:lightblue");
    //        $scope.obj = {};
    //        $scope.btnSubmit = false;
    //        $scope.btnUpdate = true;
    //        $scope.obj._id = x._id;
    //        $scope.obj.Code = x.Code;
    //        $scope.obj.Title = x.Title;
    //    };
    //    $scope.updateData = function () {

    //        //if (!$scope.form.$invalid) { 
    //        $timeout(function () { $scope.NotifiyRes = false; }, 30000);
    //        $scope.obj.Label = "Distribution Channel";
    //        var formData = new FormData();
    //        formData.append("data", angular.toJson($scope.obj));

    //        $http({
    //            url: "/Master/InsertData",
    //            method: "POST",
    //            headers: { "Content-Type": undefined },
    //            transformRequest: angular.identity,
    //            data: formData
    //        }).success(function (data, status, headers, config) {

    //            if (data.success === false) {

    //            }
    //            else {
    //                if (data === false)
    //                    $rootScope.Res = "Data already exists";
    //                else {
    //                    $rootScope.Res = "Data updated successfully";
    //                    $scope.BindList();
    //                }
    //                $scope.reset();
    //                $scope.obj = null;
    //                $rootScope.Notify = "alert-info";
    //                $rootScope.NotifiyRes = true;
    //                $('#divNotifiy').attr('style', 'display: block');
    //            }
    //        }).error(function (data, status, headers, config) {

    //        });

    //        $scope.btnSubmit = true;
    //        $scope.btnUpdate = false;
    //    };
    //    $scope.ClearFrm = function () {

    //        $scope.obj = null;
    //        $scope.btnSubmit = true;
    //        $scope.btnUpdate = false;
    //        $scope.reset();
    //    }
    //    $scope.DisableData = function (idx, enable, _id) {

    //        if (enable == false) {

    //            if (confirm("Are you sure, disable this record?")) {

    //                $http({
    //                    method: 'GET',
    //                    url: '/Master/DisableData',
    //                    params: { id: _id, Islive: enable }
    //                }).success(function (response) {
    //                    $rootScope.Res = "Data disabled";
    //                    $rootScope.Notify = "alert-info";
    //                    $('#divNotifiy').attr('style', 'display: block');
    //                    $scope.BindList();
    //                }).error(function (data, status, headers, config) {
    //                    // alert("error");
    //                });
    //            } else $scope.Varkeys[idx].Islive = true;

    //        } else {
    //            if (confirm("Are you sure, enable this record?")) {

    //                $http({
    //                    method: 'GET',
    //                    url: '/Master/DisableData',
    //                    params: { id: _id, Islive: enable }
    //                }).success(function (response) {
    //                    $rootScope.Res = "Data enabled";
    //                    $rootScope.Notify = "alert-info";
    //                    $('#divNotifiy').attr('style', 'display: block');
    //                    $scope.BindList();
    //                }).error(function (data, status, headers, config) {
    //                    // alert("error");
    //                });
    //            } else $scope.Varkeys[idx].Islive = false;


    //        }

    //    };
    //});
})();