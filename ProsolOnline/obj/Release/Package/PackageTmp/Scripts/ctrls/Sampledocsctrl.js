(function () {
    'use strict';

    var app = angular.module('ProsolApp', ['cgBusy']);

    app.controller('SampledocsctrlController', function ($scope, $http, $timeout, $window, $rootScope, $filter) {
        $scope.viewadd = true;
        $scope.viewclose = false;
        $scope.shinsertupdate = false
        $scope.showfields = function () {
            $scope.shinsertupdate = true;
            $scope.viewadd = false;
            $scope.viewclose = true;

        };
        $scope.hidefields = function () {
            $scope.shinsertupdate = false;
            $scope.viewadd = true;
            $scope.viewclose = false;
           

        };
        $scope.ShowHide = false;
        $scope.files = [];

        $scope.LoadFileData = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
           
            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'pdf' || angular.lowercase(ext) === 'docx') {
                } else {
                   
                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid PDF/WORD file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;
                   
                }
            }
        };
        $scope.LoadFileData1 = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.LoadFileData2 = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.LoadFileData3 = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.LoadFileData4= function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.LoadFileData5 = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.LoadFileData6 = function (files) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);

            $rootScope.NotifiyRes = false;
            $scope.files = files;
            if (files[0] != null) {
                var ext = files[0].name.match(/\.(.+)$/)[1];
                if (angular.lowercase(ext) === 'xls' || angular.lowercase(ext) === 'xlsx') {
                } else {

                    angular.element("input[type='file']").val(null);
                    files[0] == null;
                    $('#divNotifiy').attr('style', 'display: block');
                    $rootScope.Res = "Please Load valid excel file";
                    $rootScope.Notify = "alert-danger";
                    $rootScope.NotifiyRes = true;

                }
            }
        };
        $scope.getuserinfo = function () {
         
            $http({
                method: 'GET',
                url: '/User/getusrinfo'
            }).success(function (response) {
           
                if (response.length==0)
                {
                    $scope.showupload = false;
               }
                else {
               
                   $scope.showupload = true;
               }
            
            });
        }
        $scope.getuserinfo();
        $scope.UploadFile = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);
        
            if ($scope.files[0] != null) {


                $scope.ShowHide = true;
                $timeout(function () { $scope.NotifiyRes = false; }, 5000);

                var formData = new FormData();
                formData.append('image', $scope.files[0]);

                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/Upload",
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: angular.identity,
                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles()

                    $scope.ShowHide = false;
                    if (data == 2 || 0) {

                        $rootScope.Res = "File already exists"
                        $rootScope.Notify = "alert-danger";
                        $rootScope.NotifiyRes = true;
                    }
                    else {
                        $rootScope.Res = " File uploaded successfully"
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
      
        $scope.UploadFileBulk = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles1();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles1 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist1",
            }).success(function (response) {
                $scope.Folderfiles1 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles1();

        $scope.UploadFileBulk1 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk1",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles2();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles2 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist2",
            }).success(function (response) {
                $scope.Folderfiles2 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles2();
     
        $scope.UploadFileBulk2 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk2",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles3();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles3 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist3",
            }).success(function (response) {
                $scope.Folderfiles3 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles3();
    
        $scope.UploadFileBulk3 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk3",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles4();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles4 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist4",
            }).success(function (response) {
                $scope.Folderfiles4 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles4();
      
        $scope.UploadFileBulk4 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk4",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles5();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles5 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist5",
            }).success(function (response) {
                $scope.Folderfiles5 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles5();
        
        $scope.UploadFileBulk5 = function () {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 5000);            if ($scope.files[0] != null) {
                $scope.ShowHide = true;                $timeout(function () { $scope.NotifiyRes = false; }, 5000);                var formData = new FormData();                formData.append('image', $scope.files[0]);                $rootScope.cgBusyPromises = $http({
                    url: "/Dashboard/UploadBulk5",                    method: "POST",                    headers: { "Content-Type": undefined },                    transformRequest: angular.identity,                    data: formData
                }).success(function (data, status, headers, config) {
                    $('#divNotifiy').attr('style', 'display: block');                    $scope.getfiles()                    $scope.ShowHide = false;                    if (data == 2 || 0) {
                        $rootScope.Res = "File already exists"                        $rootScope.Notify = "alert-danger";                        $rootScope.NotifiyRes = true;
                    }                    else {
                        $rootScope.Res = " File uploaded successfully"                        $rootScope.Notify = "alert-info";                        $rootScope.NotifiyRes = true;
                        $scope.getfiles6();
                    }                    $('.fileinput').fileinput('clear');
                }).error(function (data, status, headers, config) {
                    $rootScope.ShowHide = false;                    $rootScope.Res = data;                    $rootScope.Notify = "alert-danger";                    $rootScope.NotifiyRes = true;
                });
            };
        }
        $scope.getfiles6 = function () {
            $rootScope.promise1 = $http({
                method: 'GET',                url: "/Dashboard/fileslist6",
            }).success(function (response) {
                $scope.Folderfiles6 = response;
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.getfiles6();
      
     
        $scope.getfiles = function () {

          
            $rootScope.promise1 = $http({
                    method: 'GET',
                    url: "/Dashboard/fileslist",
                }).success(function (response) {
                    $scope.Folderfiles = response;
                }).error(function (data, status, headers, config) {

                });


        };
       
        $scope.getfiles()
      
        $scope.Remove = function (x) {
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile?FileName=' + x
            }).success(function (response) {
                if(response == 1)
                {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });
            
        };
        $scope.Remove1 = function (fle) {
          
            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile1?FileName=' + fle
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
        $scope.Remove2 = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile2?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles2()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
        $scope.Remove3 = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile3?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles3()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
        $scope.Remove4 = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile4?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles4()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
        $scope.Remove5 = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile5?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles5()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
        $scope.Remove6 = function (fle) {

            $timeout(function () {
                $('#divNotifiy').attr('style', 'display: none');
            }, 3000);
            $rootScope.promise = $http({
                method: 'GET',
                url: '/Dashboard/Deletefile6?FileName=' + fle
            }).success(function (response) {
                if (response == 1) {
                    $('#divNotifiy').attr('style', 'display: block');
                    $scope.getfiles6()
                    $rootScope.Res = "File Deleted";
                    $rootScope.Notify = "alert-info";
                    $rootScope.NotifiyRes = true;
                }
            });

        };
        
    });

})();