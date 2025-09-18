

    var app = angular.module('ProsolApp', []);

    app.controller('BomDashboardController', function ($scope, $http, $timeout, $window, $filter) {

        $http({
            method: 'GET',
            url: '/Bom/getinrecord'
        }).success(function (response) {
          
            $scope.IN = response.IN;
            $scope.OUT = response.OUT;
            // alert(angular.toJson($scope.shwusr));

        }).error(function (data, status, headers, config) {
            // alert("error");
        });


        $scope.Chart = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Bom/Total'
            }).success(function (response) {
                $scope.count = response.count;
                $scope.A = response.A;
                $scope.B = response.B;
                $scope.C = response.C;
                $scope.AA = response.AA;
                $scope.BB = response.BB;
                $scope.CC = response.CC;
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Indicators');
                data.addColumn('number', 'count');
                data.addRows([
                ['A', response.AA],
                ['B', response.BB],
                ['C', response.CC],
                ['Uncompleted', response.sub],
              
                ]);
                var options = {
                    title: 'Equipment Details',
                };

                var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                chart.draw(data, options);
            });
                $scope.promise = $http({
                    method: 'GET',
                    url: '/Bom/Total2'
                }).success(function (response) {
                    $scope.matcount = response.matcount;
                    $scope.count2 = response.count2;
                    $scope.data = response.typemat;

               

                var data1 = new google.visualization.DataTable();
                data1.addColumn('string', 'Indicators');
                data1.addColumn('number', 'count');
                data1.addRows([
                ['Linked Material', response.count2],
                ['Not Linked', response.Sub1],

                ]);
                var options1 = {
                    title: 'Material Details',
                };

                var chart1 = new google.visualization.PieChart(document.getElementById('piechart1'));
                chart1.draw(data1, options1);
                });
            
        };

        //$scope.promise =
        //    $http({
        //    method: 'GET',
        //    url: '/Bom/getinrecord'
        //}).success(function (response) {
     
        //    $scope.Record = response;


        //});




    });
   
