var reqlogcont = angular.module('ProsolApp', []);




// for loading image
reqlogcont.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    elm.show();
                } else {
                    elm.hide();
                }
            });
        }
    };

}]);


reqlogcont.controller('reqlog_controller',
    function ($scope, $window, $http, $timeout) {


        new jBox('Tooltip', {
            attach: '#showstatus',
            //width: 400,
            //height: 500,                   
            closeButton: true,
            //animation: 'zoomIn',
            theme: 'TooltipBorder',
            trigger: 'click',
            width: 600,
            height: 200,
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
        $scope.Loadpop = function () {
            new jBox('Tooltip', {
                attach: '#showstatus',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 200,
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

        $scope.Loadpop1 = function () {
            new jBox('Tooltip', {
                attach: '#showstatus1',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 200,
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
        $scope.Loadpop2 = function () {
            new jBox('Tooltip', {
                attach: '#showstatus2',
                //width: 400,
                //height: 500,                   
                closeButton: true,
                //animation: 'zoomIn',
                theme: 'TooltipBorder',
                trigger: 'click',
                width: 600,
                height: 200,
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

        $scope.showuserMap = function (itmcode) {

            $http({
                method: 'GET',
                url: '/User/getItemstatusMap',
                params :{itmcde: itmcode}
            }).success(function (response) {
                $scope.itemstatusLst = response;

            }).error(function (data, status, headers, config) {
                // alert("error");
            });



        }
        $scope.rows = [];
        $scope.rows_app = [];
        $scope.rows_rej = [];
        $scope.req_visible = false;

        $http({
            method: 'GET',
            url: 'requestlog/getRequested_Records'
        }).success(function (result) {
            $scope.rows = result;
            
            if ($scope.rows.length > 0) {
                $scope.load_data_to_approve(0);
            }

        });

        $http({
            method: 'GET',
            url: 'requestlog/getApproved_Records'
        }).success(function (result) {
            $scope.rows_app = result;

            if (result.length > 0) {
                $scope.load_data_of_approved(0);
            }
        });

        $http({
            method: 'GET',
            url: 'requestlog/getRejected_Records'
        }).success(function (result) {
            $scope.rows_rej = result;

            if (result.length > 0) {
                $scope.load_data_of_rejected(0);
            }
        });





        $scope.load_data_to_approve = function (index) {
            $scope.cb_approve = true;
            $scope.cb_reject = false;
            $scope.rejecttxt_show = false;

            $scope.cataloguerlist = [];
          //  $scope.getcataloguernames_id();
            var val = 0;
            if ($scope.rows.length > 0) {
                for (var res in $scope.rows) {
                    if (val === index) {
                        $scope.rows[val].itemStatus = "active";
                    }
                    else {
                        $scope.rows[val].itemStatus = "inactive";
                    }
                    val = val + 1;
                }

                $http({
                    method: 'GET',
                    url: 'requestlog/getsingle_requested_record',
                    params :{abcsony: $scope.rows[index].itemId}
                }).success(function (result) {
                    
                    $scope.Req = {};
                    $scope.Req.itemId = result[0].itemId;
                    $scope.Req.requestId = result[0].requestId;
                    $scope.Req.plant = result[1].plant;
                    $scope.Req.storage_Location = result[1].storage_Location;
                    $scope.Req.group = result[1].group;
                    $scope.Req.subGroup = result[1].subGroup;
                    $scope.Req.requester = result[1].requester;
                    $scope.Req.source = result[0].source;
                    $scope.Req.requestedOn = result[0].requestedOn;
                    $scope.Req.approver = result[0].approver;
                    $scope.req_visible = true;

                    $scope.l_item = "Item :";
                    $scope.l_req_id = "Request Id";
                    $scope.l_plant = "Plant";
                    $scope.l_sl = "Storage Location";
                    $scope.l_group = "Group";
                    $scope.l_subgroup = "Sub Group";
                    $scope.l_source = "Source";
                    $scope.l_approver = "Approver";

                });
            }
            else {
                $scope.req_visible = false;
                $scope.Req = {};
                $scope.Req.itemId = "";
                $scope.Req.requestId = "";
                $scope.Req.plant = "";
                $scope.Req.storage_Location = "";
                $scope.Req.group = "";
                $scope.Req.subGroup = "";
                $scope.Req.requester = "";
                $scope.Req.source = "";
                $scope.Req.requestedOn = "";
                $scope.Req.approver = "";
            }
        };


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
                    params : {abcsony:$scope.rows_app[index].itemId}
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
                });

            }
            else {
                $scope.app_visible = false;
            }
        };

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
                    params :{abcsony: $scope.rows_rej[index].itemId}
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
                });
            }
            else {
                $scope.rej_visible = false;
            }
        };

    });
