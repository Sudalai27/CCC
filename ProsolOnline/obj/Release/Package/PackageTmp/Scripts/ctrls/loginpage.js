var loginbody = angular.module('loginctrl', []);

loginbody.controller('logincontroller',
function($scope,$window,$http){
    
    $scope.date = new Date();

    $scope.checklogin_details = function () {
       
            $http({
                method: 'GET',
                url: '/login/checklogin_details',
                params :{UserName: $scope.login.UserName , Password: $scope.login.Password}
            }).success(function (data) {
              
                if (data.result === 'Active')
                {
                    window.location = "/Dashboard/HomeIndex";
                } else if (data.result === 'InActive') {
                    $scope.Error1 = "Access denied";
                }
                else if (data.result === 'Expired') {
                    $scope.Error1 = "Software has expired, please contact administrator";
                } else {
                    $scope.Error1 = "Invalid Username or Password";
                }

            }).error(function (result) {

            });
        
       
      
    }






});



loginbody.directive('passwordValidate', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {

                scope.pwdValidLength = (viewValue && viewValue.length >= 8 ? 'valid' : undefined);
                scope.pwdHasLetter = (viewValue && /[A-z]/.test(viewValue)) ? 'valid' : undefined;
                scope.pwdHasNumber = (viewValue && /\d/.test(viewValue)) ? 'valid' : undefined;

                if (scope.pwdValidLength && scope.pwdHasLetter && scope.pwdHasNumber) {
                    ctrl.$setValidity('pwd', true);
                    return viewValue;
                } else {
                    ctrl.$setValidity('pwd', false);
                    return undefined;
                }

            });
        }
    };
});

