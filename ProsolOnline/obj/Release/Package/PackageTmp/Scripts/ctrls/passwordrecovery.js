var pr_body = angular.module("pr_app", ['cgBusy']);

pr_body.controller('pr_controller',
    function ($scope, $window, $http) {
        $scope.date = new Date();

        $scope.sendemail_forPR = function () {
            if ($scope.emaill === "") {
                $scope.Error1 = "Enter registered email id";
            }
            else{
            $scope.promise1 = $http({
                method: 'GET',
                url: '/passwordrecovery/sendemail_forPR',
                params :{email: $scope.emaill}
            }).success(function (data) {

                if (data.result === true) {
                    $scope.Error1 = "New password link has been sent, check your email";

                }
                else {
                   // alert("Invalid Email");
                   $scope.Error1 = "Enter registered email id";
                }

            }).error(function (result) {

            });
            }
        }
    }
    )


pr_body.controller('pru_controller',
    function ($scope, $window, $http,$location) {
        $scope.date = new Date();
        $scope.error123 = "";
      
        $scope.updatepasswordpage = function () {
            
            $scope.error123 = "";
            if ($scope.pw == $scope.cpw) {
              
                $scope.promise1 = $http({
                    method: 'GET',
                    url: '/passwordrecovery/updatepassword',
                    params: { pswd: $scope.pw, userid: $location.search().num$123, rndm: $location.search().num$coda }
                    //?pswd=' + $scope.pw + '&userid=' + $location.search().num$123 + '&rndm=' + $location.search().num$coda //'userid=' + $location.search().userid
                }).success(function (data) {

                    if (data.result === true) {
                        alert("Password has been updated");
                        window.location = "/login/Index";
                    }
                    else {
                        alert("Invalid request for changing your password");
                        window.location = "/passwordrecovery/updatepasswordpage";
                    }

                }).error(function (result) {

                });
            }
            else
            {
               
                $scope.error123 = "Password missmatch";
            }
           
        }          
    }
    )
