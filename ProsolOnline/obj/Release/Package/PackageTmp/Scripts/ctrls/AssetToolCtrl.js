(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy','datatables']);


    app.controller('ValueController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.KeyClear = function () {
            $scope.uomlist = [];
            if ($scope.NM.Noun === undefined) {
                $scope.NM.Nounabv = "";
                $scope.NM.NounDefinition = "";
            }
        };
        $scope.KeyClear1 = function () {
            $scope.uomlist = [];
            if ($scope.NM.Modifier === undefined) {
                $scope.NM.Nounabv = "";
            }
        };
        $scope.Listmodifer = function () {
            if (!$scope.NM?.Noun) return;

            $http({
                method: 'GET',
                url: '/Dictionary/GetModifier',
                params: { Noun: $scope.NM.Noun }
            }).then(function (response) {
                $scope.Modifiers = response.data;
            }).catch(function (error) {
                console.error("Error fetching modifiers:", error);
            });
        };

        $scope.GetCharc = function (Noun, Modifier) {
            $scope.NM.Modifier = Modifier?.toUpperCase();

            if (!Modifier) {
                $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                return;
            }

            $http({
                method: 'GET',
                url: '/Dictionary/GetNounModifier',
                params: { Noun: Noun, Modifier: Modifier }
            }).then(function (response) {
                const data = response.data;

                if (!data) {
                    $scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                    return;
                }

                $scope.NM = data.One_NounModifier || {};

                if (Array.isArray(data.ALL_NM_Attributes) && data.ALL_NM_Attributes.length > 0) {
                    console.log("Before:", data.ALL_NM_Attributes);
                    $scope.rows = data.ALL_NM_Attributes.filter(item => item.Definition === 'MM').sort();
                    console.log("After:", $scope.rows);
                } else {
                    $scope.NM.NounDefinition = "";
                    $scope.NM.Modifierabv = "";
                    $scope.NM.ModifierDefinition = "";
                    $scope.rows = [{ 'Squence': 10, 'ShortSquence': 10, 'Remove': 0 }];
                }

                const uomlist = data.One_NounModifier?.uomlist || [];
                if (Array.isArray(uomlist) && uomlist.length > 0 && Array.isArray($scope.UOMs)) {
                    $scope.UOMs.forEach(lst => {
                        lst.Checked = uomlist.includes(lst._id) ? '1' : '0';
                    });
                }
            }).catch(function (error) {
                console.error("Error fetching noun modifier:", error);
            });
        };


    });
    app.controller('MfrController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnFARmodel = 'Select FAR ID';
        $scope.BtnRegmodel = 'Select Region';
        $scope.BtnDescmodel = '';
        $scope.maxInitialRecords = 10;

        $scope.obj = {};

        $scope.pageSize = 10; 
        $scope.currentPage = 1; 

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/FAR/GetDataList',
                params: { label: 'Manufacturer' }
            }).success(function (response) {
                $scope.MfrLst = response;
                console.log($scope.MfrLst)
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        // Function to load the next page
        $scope.loadNextPage = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.currentPage++;
                $scope.BindList();
            }
        };

        // Function to load the previous page
        $scope.loadPreviousPage = function () {
            if ($scope.currentPage > 1) {
                $scope.currentPage--;
                $scope.BindList();
            }
        };

        // Initial load


        $scope.regionFilter = function (far) {
            return far.Region !== null && far.Region !== "";
        };
        $scope.assetDescFilter = function (far) {
            return far.Region !== null && far.AssetDesc !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnFARmodel = far;
                $scope.obj.FARId = far;
                //if()
            }
            $scope.RegionMaster_ = $filter('filter')($scope.FARMaster, function (i) {
                return i.FARId == far;
            });
            $scope.RegionList = Array.from(new Set($scope.RegionMaster_.map(i => i.Region)));
            //console.log($scope.RegionList)
        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnRegmodel = reg;
                $scope.obj.Region = reg;
                //if()
            }

        }
        $scope.reset = function () {
            $scope.obj.FARId = "";
            $scope.obj.FARId = "";
            $scope.obj.Region = "";
            $scope.obj.AssetDesc = "";
            $scope.BtnFARmodel = 'Select FAR ID';
            $scope.BtnRegmodel = 'Select Region';
            $scope.form.$setPristine();
        }
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "Manufacturer";
            var formData = new FormData();
            $scope.obj.Title = $scope.obj.Code;
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertDataBusiness",
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

        $rootScope.onclickBusiness = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }

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
        $scope.promise = $scope.MfrBulkUpload = function () {


            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/MfrBulkUpload",
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
                    alert($scope.Res)

                    $scope.Notify = "alert-info";
                    $scope.NotifiyRes = true;

                    //$('#divNotifiy').attr('style', 'display: block');
                    $('.fileinput').fileinput('clear');

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $scope.Res = "Please Valid Your Excel File";
                    $scope.Notify = "alert-danger";
                    $scope.NotifiyRes = true;


                });
            };
        }



        $scope.RemoveMfr = function (id) {

            if (confirm("Are you sure, disable this record?")) {

                $http({
                    method: 'GET',
                    url: '/FAR/RemoveMfr',
                    params: { id: id, IsActive: false }
                }).success(function (response) {
                    $rootScope.Res = "Manufacturer deleted";
                    $rootScope.Notify = "alert-info";
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.BindList();
                }).error(function (data, status, headers, config) {
                    // alert("error");
                });
            } 

        };
        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };
        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableNotes',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

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
                    $scope.BindList();
                }
                else {
                    $rootScope.Res = response;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                }
            });
        }

    });
    app.controller('TaxController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnFARmodel = 'Select FAR ID';
        $scope.BtnRegmodel = 'Select Region';
        $scope.BtnDescmodel = '';
        $scope.maxInitialRecords = 10;

        $scope.obj = {};

        $scope.pageSize = 10; 
        $scope.currentPage = 1; 

        $scope.BindList = function () {

            $http({
                method: 'GET',
                url: '/Master/GetDataList',
                params: { label: 'AdditionalNotes' }
            }).success(function (response) {
                $scope.Notes = response;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        // Function to load the next page
        $scope.loadNextPage = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.currentPage++;
                $scope.BindList();
            }
        };

        // Function to load the previous page
        $scope.loadPreviousPage = function () {
            if ($scope.currentPage > 1) {
                $scope.currentPage--;
                $scope.BindList();
            }
        };

        // Initial load


        $scope.regionFilter = function (far) {
            return far.Region !== null && far.Region !== "";
        };
        $scope.assetDescFilter = function (far) {
            return far.Region !== null && far.AssetDesc !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnFARmodel = far;
                $scope.obj.FARId = far;
                //if()
            }
            $scope.RegionMaster_ = $filter('filter')($scope.FARMaster, function (i) {
                return i.FARId == far;
            });
            $scope.RegionList = Array.from(new Set($scope.RegionMaster_.map(i => i.Region)));
            //console.log($scope.RegionList)
        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnRegmodel = reg;
                $scope.obj.Region = reg;
                //if()
            }

        }
        $scope.reset = function () {
            $scope.obj.FARId = "";
            $scope.obj.FARId = "";
            $scope.obj.Region = "";
            $scope.obj.AssetDesc = "";
            $scope.BtnFARmodel = 'Select FAR ID';
            $scope.BtnRegmodel = 'Select Region';
            $scope.form.$setPristine();
        }
        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }

        $scope.createData = function () {

            //if (!$scope.form.$invalid) {               

            $timeout(function () { $scope.NotifiyRes = false; }, 5000);
            $scope.obj.Label = "AdditionalNotes";
            var formData = new FormData();
            $scope.obj.Title = $scope.obj.Code;
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

        $rootScope.onclickBusiness = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }

        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };
        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableNotes',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

    });
    app.controller('FARController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnFARmodel = 'Select FAR ID';
        $scope.BtnRegmodel = 'Select Region';
        $scope.BtnDescmodel = '';
        $scope.maxInitialRecords = 10;

        $scope.obj = {};

        $scope.pageSize = 10; 
        $scope.currentPage = 1; 

        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetFarMaster',
                params: {
                    page: $scope.currentPage,
                    pageSize: $scope.pageSize
                }
            }).then(function (response) {
                const data = response.data;

                // Assign the full response data
                $scope.FARMaster = data.data || [];

                // Handle pagination info if available
                $scope.totalPages = data.totalPages || 0;

                // Create distinct lists using Set
                $scope.FARList = [...new Set($scope.FARMaster.map(i => i.FARId))];
                $scope.RegionList = [...new Set($scope.FARMaster.map(i => i.Region))];
                $scope.AssetDescList = [...new Set($scope.FARMaster.map(i => i.AssetDesc))];

                console.log($scope.FARMaster);
            }).catch(function (error) {
                console.error('Error fetching FAR Master:', error);
                // Optionally show error to user
            });
        };


        // Function to load the next page
        $scope.loadNextPage = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.currentPage++;
                $scope.BindList();
            }
        };

        // Function to load the previous page
        $scope.loadPreviousPage = function () {
            if ($scope.currentPage > 1) {
                $scope.currentPage--;
                $scope.BindList();
            }
        };

        // Initial load
        $scope.BindList();


        $scope.regionFilter = function (far) {
            return far.Region !== null && far.Region !== "";
        };
        $scope.assetDescFilter = function (far) {
            return far.Region !== null && far.AssetDesc !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnFARmodel = far;
                $scope.obj.FARId = far;
                //if()
            }
            $scope.RegionMaster_ = $filter('filter')($scope.FARMaster, function (i) {
                return i.FARId == far;
            });
            $scope.RegionList = Array.from(new Set($scope.RegionMaster_.map(i => i.Region)));
            //console.log($scope.RegionList)
        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnRegmodel = reg;
                $scope.obj.Region = reg;
                //if()
            }

        }
        $scope.reset = function () {
            $scope.obj.FARId = "";
            $scope.obj.Region = "";
            $scope.obj.AssetDesc = "";
            $scope.BtnFARmodel = 'Select FAR ID';
            $scope.BtnRegmodel = 'Select Region';
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
                url: "/FAR/InsertDataFar",
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

        $rootScope.onclickBusiness = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }

        $scope.DisableBuns = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableBun',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Business enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Business[idx].IsActive = false;


            }

        };

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

    });
    app.controller('SiteController', function ($scope, $http, $rootScope, $timeout, $filter) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnSitemodel = 'Select Site ID';
        $scope.BtnClusmodel = 'Select Cluster';
        $scope.BtnHLLcmodel = '';

        $scope.obj = {};

        $scope.BindList = function () {

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
        $scope.BindList();


        $scope.ClusFilter = function (far) {
            return far.Cluster !== null && far.Cluster !== "";
        };
        $scope.HLLFilter = function (far) {
            return far.HighLevelLocation !== null && far.Cluster !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnSitemodel = far;
                $scope.obj.SiteId = far;
                //if()
            }
            $scope.Master_ = $filter('filter')($scope.SiteMaster, function (i) {
                return i.SiteId == far;
            });
            console.log($scope.Master_)
            $scope.ClusList = Array.from(new Set($scope.Master_.map(i => i.Cluster)));

        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnClusmodel = reg;
                $scope.obj.Cluster = reg;
                //if()
            }

        }

        $scope.onclickSite = function () {
            $scope.obj.Label = "SiteId";
        }

        $rootScope.onclickLocation = function () {
            $scope.obj.Label = "Location";
            $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }



        $scope.reset = function () {

            $scope.obj.SiteId = "";
            $scope.obj.Cluster = "";
            $scope.BtnSitemodel = 'Select Site ID';
            $scope.BtnClusmodel = 'Select Cluster';
            $scope.form.$setPristine();

        }
        $scope.createData = function () {

            //$timeout(function () { $scope.NotifiyRes = false; }, 30000);
            //$scope.obj.Label = "MajorClass";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertDataSite",
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

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }


        $scope.DisableData = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemjr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MajorClass disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Major[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemjr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MajorClass enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Major[idx].IsActive = false;


            }

        };
    });
    app.controller('LocController', function ($scope, $http, $rootScope, $timeout) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnLocmodel = 'Select Location';
        $scope.BtnLochmodel = '';
        $scope.BtnDescmodel = '';

        $scope.obj = {};


        $scope.BindList = function () {
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
        $scope.BindList();

        $scope.LochFilter = function (far) {
            return far.LocationHierarchy !== null && far.LocationHierarchy !== "";
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnLocmodel = far;
                $scope.obj.Location = far;
                //if()
            }

        }
        $scope.reset = function () {

            $scope.obj.Location = '';
            $scope.obj.LocationHierarchy = '';
            $scope.BtnLocmodel = 'Select Location';
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
                url: "/FAR/InsertDataLoc1",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;


        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableLoc = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableLoc',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Location disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Loclist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableLoc',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Location enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Loclist[idx].IsActive = false;


            }

        };
    });
    app.controller('ATController', function ($scope, $http, $rootScope, $timeout) {

        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.BtnSitemodel = 'Select Asset Type';
        $scope.BtnClusmodel = 'Select Classification Hierarchy Description';
        $scope.BtnHLLcmodel = '';

        $scope.obj = {};

        $scope.BindList = function () {

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
        $scope.BindList();

        $scope.ClsFilter = function (far) {
            return far.ClassificationHierarchyDesc !== null && far.ClassificationHierarchyDesc !== "";
        };
        $scope.FacFilter = function (far) {
            return far.ClassificationHierarchyDesc !== null && far.FailureCode !== null;
        };

        $scope.changeFar = function (far) {

            if (far != null) {
                $scope.BtnSitemodel = far;
                $scope.obj.AssetType = far;
                //if()
            }
            $scope.Master_ = $filter('filter')($scope.ATMaster, function (i) {
                return i.AssetType == far;
            });
            $scope.ClsList = Array.from(new Set($scope.Master_.map(i => i.ClassificationHierarchyDesc)));

        }
        $scope.changeReg = function (reg) {

            if (reg != null) {
                $scope.BtnClusmodel = reg;
                $scope.obj.ClassificationHierarchyDesc = reg;
                //if()
            }

        }

        $scope.onclickSite = function () {
            $scope.obj.Label = "SiteId";
        }

        $rootScope.onclickLocation = function () {
            $scope.obj.Label = "Location";
            $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }



        $scope.reset = function () {

            $scope.obj.AssetType = "";
            $scope.obj.ClassificationHierarchyDesc = "";
            $scope.obj.FailureCode = "";
            $scope.BtnSitemodel = 'Select Asset Type';
            $scope.BtnClusmodel = 'Select Classification Hierarchy Description';
            $scope.form.$setPristine();

        }
        $scope.createData = function () {

            //$timeout(function () { $scope.NotifiyRes = false; }, 30000);
            //$scope.obj.Label = "MajorClass";
            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertDataAT",
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

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }


        $scope.DisableData = function (idx, enable, id) {

            if (enable == false) {
                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemjr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MajorClass disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Major[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemjr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MajorClass enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Major[idx].IsActive = false;


            }

        };
    });

    app.controller('MinorController', function ($scope, $http, $rootScope, $timeout) {

        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.Minor = response.MinorClasses;

                //    $scope.Majorlist = response.MajorClasses;
                $scope.Businesslist = response.Businesses;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $scope.dropdownlist = function () {
            $http({
                method: 'GET',
                url: '/FAR/dropdownasset'
            }).success(function (response) {
                //  $scope.Minor = response.MinorClasses;

                $scope.Majorlist = response.MajorClasses;
                $scope.Businesslist = response.Businesses;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.dropdownlist();
        $rootScope.onclickStorage = function () {
            $scope.BindPlantList();
            $scope.BindList();
        }

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

            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertData1",
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

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

        $scope.Disablemnr = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemnr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MinorClass disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Minor[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disablemnr',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "MinorClass enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Minor[idx].IsActive = false;


            }

        };
    });
    app.controller('IdentifierController', function ($scope, $http, $rootScope, $timeout, $window) {

        //$('#tblIden tfoot th').each(function () {
        //    var title = $(this).text();
        //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        //});

        //var table = $('#tblIden').DataTable({
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
        $scope.exportIdentifier = function () {

           
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
            $window.location = '/FAR/DownloadIdentifier';


        }

        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.Identifier = response.Identifiers;

             //   $scope.Minorlist = response.MinorClasses;
             //   $scope.Major = response.MajorClasses;

                $scope.Businesslist = response.Businesses;
               
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        $scope.dropdownlist = function () {
            $http({
                method: 'GET',
                url: '/FAR/dropdownasset'
            }).success(function (response) {
                 $scope.Minorlist = response.MinorClasses;

                $scope.Major= response.MajorClasses;
                $scope.Businesslist = response.Businesses;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.dropdownlist();

        $rootScope.onclickStorage = function () {
            $scope.BindPlantList();
            $scope.BindList();
        }

        $rootScope.NotifiyResclose = function () {
            $('#divNotifiy').attr('style', 'display: none');
        }


        // $scope.BindList();
       
        $scope.createData = function () {



            $timeout(function () { $scope.NotifiyRes = false; }, 30000);

            var formData = new FormData();
            formData.append("data", angular.toJson($scope.obj));

            $http({
                url: "/FAR/InsertDataIdent",
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
        $scope.reset = function () {

            $scope.form.$setPristine();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;


        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }

        $scope.Disableidnt = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disableidnt',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Identifier disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Identifier[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/Disableidnt',
                        params: { id: id, IsActive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Identifier enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Identifier[idx].IsActive = false;


            }

        };

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

                    $rootScope.Res = "Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.$apply();
                }
            }
        };
        $scope.ShowHide = false;

        $scope.IdentifierBulkdata = function () {

            if ($scope.files[0] != null) {
                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $scope.promise = $http({
                    url: "/FAR/IdentifierBulk_Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    if (data.includes("Error : ")) {
                        $scope.ShowHide = false;
                        $rootScope.Res = data;
                        $rootScope.Notify = "alert-danger";
                        $('#divNotifiy').attr('style', 'display: block');
                    } else {
                        $scope.ShowHide = false;
                        $rootScope.Res = data + " items uploaded successfully";

                        $rootScope.Notify = "alert-info";
                    

                        $('#divNotifiy').attr('style', 'display: block');

                        $('.fileinput').fileinput('clear');
                    }

                }).error(function (data, status, headers, config) {
                    $scope.ShowHide = false;
                    $rootScope.Res = data;
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                    $('#divNotifiy').attr('style', 'display: block');


                });
            };
        }

    });
    app.controller('RegionController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.region = response.Regions;
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
                url: "/FAR/InsertDataRegion",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
   
    
        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableReg = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableReg',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Region disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.region[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableReg',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Region enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.region[idx].IsActive = false;


            }

        };
    });
    app.controller('CityController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.region = response.Regions;
                $scope.citylist = response.Cities;
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
                url: "/FAR/InsertDataCity",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
        $scope.DataDel = function (_id) {
            if (confirm("Are you sure, delete this record?")) {
                $http({
                    method: 'GET',
                    url: '/FAR/DelDataCity',
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

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableCity = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableCity',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "City disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.citylist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableCity',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "City enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.citylist[idx].IsActive = false;


            }

        };
    });
    app.controller('AreaController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.Arealist = response.Areas;
             //   $scope.citylist = response.Cities;
               // $scope.region = response.Regions;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();

        $scope.dropdownlist = function () {
            $http({
                method: 'GET',
                url: '/FAR/dropdownasset'
            }).success(function (response) {
                $scope.citylist = response.Cities;
                $scope.region = response.Regions;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.dropdownlist();
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
                url: "/FAR/InsertDataArea",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
       

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableArea = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableArea',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Area disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Arealist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableArea',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Area enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Arealist[idx].IsActive = false;


            }

        };
    });
    app.controller('SubAreaController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
             //   $scope.Arealist = response.Areas;
                $scope.SubArealist = response.SubAreas;
            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.BindList();
        $scope.dropdownlist = function () {
            $http({
                method: 'GET',
                url: '/FAR/dropdownasset'
            }).success(function (response) {
                $scope.Arealist = response.Areas;
                $scope.citylist = response.Cities;
                $scope.region = response.Regions;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });
        };
        $scope.dropdownlist();
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
                url: "/FAR/InsertDataSubArea",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;
 

        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableSubArea = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableSubArea',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Sub Area disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Arealist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableSubArea',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Sub Area enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.Arealist[idx].IsActive = false;


            }

        };
    });
    app.controller('EquipClassController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.EquipClasslist = response.EquipmentClasses;
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
                url: "/FAR/InsertDataEquipClass",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;


        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableEquipClass = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableEquipClass',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Equipment Class disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.EquipClasslist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableEquipClass',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Equipment Class enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.EquipClasslist[idx].IsActive = false;


            }

        };
    });
    app.controller('EquipTypeController', function ($scope, $http, $rootScope, $timeout) {
        $scope.BindList = function () {
            $http({
                method: 'GET',
                url: '/FAR/GetAssetMaster'
            }).success(function (response) {
                $scope.EquipClass = response.EquipmentClasses;
                $scope.EquipTypelist = response.EquipmentTypes;

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
                url: "/FAR/InsertDataEquipType",
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
        $rootScope.onclickregion = function () {
            //$scope.BindPlantList();
            $scope.BindList();
        }
        $scope.btnSubmit = true;
        $scope.btnUpdate = false;


        $scope.ClearFrm = function () {
            $scope.obj = null;
            $scope.btnSubmit = true;
            $scope.btnUpdate = false;
            $scope.reset();
        }
        $scope.DisableEquipType = function (idx, enable, id) {

            if (enable == false) {

                if (confirm("Are you sure, disable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableEquipType',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Equipment Type disabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.EquipTypelist[idx].IsActive = true;

            } else {
                if (confirm("Are you sure, enable this record?")) {

                    $http({
                        method: 'GET',
                        url: '/FAR/DisableEquipType',
                        params: { id: id, Islive: enable }
                    }).success(function (response) {
                        $rootScope.Res = "Equipment Type enabled";
                        $rootScope.Notify = "alert-info";
                        $('#divNotifiy').attr('style', 'display: block');
                        $scope.BindList();
                    }).error(function (data, status, headers, config) {
                        // alert("error");
                    });
                } else $scope.EquipTypelist[idx].IsActive = false;


            }

        };


    });

    app.factory("AutoCompleteService", ["$http", function ($http) {
        return {
            search: function (term) {
                return $http({
                    url: "/Dictionary/AutoCompleteAssetNoun",
                    params: { term: term },
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
                        scope.NM.Noun = selectedItem.item.value;


                        $.get("/Dictionary/GetNounDetail", { Noun: scope.NM.Noun }).success(function (response) {

                            scope.NM = response;
                            scope.$apply();
                            event.preventDefault();
                        });
                    }
                });

            }

        };
    }]);
    app.factory("AutoCompleteService1", ["$http", function ($http) {
        return {
            search: function (Noun, term) {

                return $http({
                    url: "/Dictionary/AutoCompleteModifier",
                    params: { term: term, Noun: Noun },

                    method: "GET"
                }).success(function (response) {
                    return response.data;
                });


                // return $http.get("AutoCompleteModifier?term=" + term + "&Noun=" + Noun).then(function (response) {
                //  return response.data;
                // });
            }
        };
    }]);
    app.directive("autoComplete1", ["AutoCompleteService1", function (AutoCompleteService) {
        return {
            restrict: "A",
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (searchTerm, response) {

                        AutoCompleteService.search(scope.NM.Noun, searchTerm.term).success(function (autocompleteResults) {
                            response($.map(autocompleteResults, function (autocompleteResult) {
                                return {
                                    label: autocompleteResult.Modifier,
                                    value: autocompleteResult
                                }
                            }))
                        });
                    },
                    minLength: 1,
                    select: function (event, selectedItem) {
                        scope.NM.Modifier = selectedItem.item.value;

                        $.get("/Dictionary/GetNounModifier", { Noun: scope.NM.Noun, Modifier: scope.NM.Modifier }).success(function (response) {
                            scope.NM = response.One_NounModifier;
                            var uomlist = response.One_NounModifier.uomlist;

                            if (uomlist != null) {
                                angular.forEach(scope.UOMs, function (lst) {

                                    if (uomlist.indexOf(lst._id) !== -1) {
                                        lst.Checked = '1';
                                    } else {
                                        lst.Checked = '0';
                                    }
                                });
                            }
                            //angular.forEach(scope.UOMs, function (value1, key) {

                            //    angular.forEach(scope.uomlist, function (value2, key) {


                            //        if (value1._id == value2._id)
                            //        {
                            //            alert("1")
                            //            $('#chku' + _id).Checked = true;

                            //        }

                            //    });


                            //});

                            if (response.ALL_NM_Attributes.length > 0) {
                                scope.rows = response.ALL_NM_Attributes;
                                //  alert(angular.toJson(scope.rows))
                                //angular.forEach(scope.rows, function (lst) {

                                //    $.get("/Dictionary/GetAttributesDetail?Name=" + lst.Characteristic).success(function (response) {
                                //        if (response != null) {
                                //            var i = 0;
                                //            if (response.ValueList == null) {

                                //                angular.forEach($scope.ValueList, function (lst) {
                                //                  $scope.selectValue.push(lst._id);

                                //                });
                                //            } 
                                //        }

                                //    })
                                //});

                                //  alert(angular.toJson(scope.rows));
                            }
                            else {
                                scope.NM.NounDefinition = "";
                                scope.NM.Modifierabv = "";
                                scope.NM.ModifierDefinition = "";
                                scope.rows = [{ 'Squence': 1, 'ShortSquence': 1, 'Remove': 0 }];
                            }
                            scope.$apply();
                            event.preventDefault();
                        });


                        //$.ajax({
                        //      url: 'GetNounModifier?Noun=' + scope.NM.Noun + '&Modifier=' + scope.NM.Modifier,                           
                        //    type: 'GET',                       
                        //    success: function (response) {
                        //       // alert(JSON.stringify(response.ALL_NM_Attributes));
                        //        scope.NM = response.One_NounModifier;
                        //        scope.rows = response.ALL_NM_Attributes;
                        //        scope.$apply();
                        //        event.preventDefault();
                        //    },
                        //    error: function (xhr, ajaxOptions, thrownError) {
                        //        $scope.Res = thrownError;

                        //    }
                        //});
                    }
                });


            }
        };
    }]);
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