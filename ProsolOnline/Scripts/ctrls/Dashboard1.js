(function () {
    'use strict';

    var app = angular.module('ProsolApp', ['cgBusy']);

    app.controller('DashboardController', function ($scope, $http, $timeout, $window, $filter) {

        $scope.monthList = [
            { month: "January" }, { month: "February" }, { month: "March" }, { month: "April" }, { month: "May" }, { month: "June" }, { month: "July" }, { month: "August" }, { month: "September" }, { month: "October" }, { month: "November" }, { month: "December" }
        ];
        var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        $scope.yearList = [
            { year: "2022" },
            { year: "2023" },
            { year: "2024" },
            { year: "2025" },
            { year: "2026" },
            { year: "2027" },
            { year: "2028" },
            { year: "2029" },
            { year: "2030" },
            { year: "2031" },
            { year: "2032" },
            { year: "2033" },
            { year: "2034" },
            { year: "2035" },
            { year: "2036" },
            { year: "2037" },
            { year: "2038" },
            { year: "2039" },
            { year: "2040" },
            { year: "2041" }];

        //Pie Chart

        $scope.bindItems = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getRepPurgItems'
            }).success(function (response) {

                $scope.countList = response;

                var labels = $scope.countList.map(item => item.purgCode);
                var data = $scope.countList.map(item => parseInt(item.itmsCount));
                //$scope.total = 
                var pieChart = document.getElementById('pie').getContext('2d');

                var myPieChart = new Chart(pieChart, {
                    type: 'pie',
                    data: {
                        labels: labels,
                        datasets: [{
                            data: data,
                            backgroundColor: [
                                "#0000FF", "#00008B", "#0000CD", "#0000FA",
                                "#000080", "#191970", "#1E90FF", "#20B2AA",
                                "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                                "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                                "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                                "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                                "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                                "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                                "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                                "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                                "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                                "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                            ],
                            borderColor: [
                                "#0000FF", "#00008B", "#0000CD", "#0000FA",
                                "#000080", "#191970", "#1E90FF", "#20B2AA",
                                "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                                "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                                "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                                "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                                "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                                "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                                "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                                "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                                "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                                "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                            ],
                            borderWidth: 1
                        }],
                    },
                    options: {
                        animation: {
                            duration: 1500,
                            easing: 'easeInOutQuart'
                        },
                        plugins: {
                            legend: false,
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem, data) {
                                        var dataset = data.datasets[tooltipItem.datasetIndex];
                                        var value = dataset.data[tooltipItem.index];
                                        return dataset.label + ': ' + value;
                                    }
                                }
                            }
                        }
                    }
                });

            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindItems();


        //Donut Chart

        $scope.bindOpenItems = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getAllOpenReqs'
            }).success(function (response) {

                $scope.itmsList = response;

                var labels = $scope.itmsList.map(item => item.req);
                var data = $scope.itmsList.map(item => item.itmsCount);

                var donutChart = document.getElementById('donut').getContext('2d');

                var myDonutChart = new Chart(donutChart, {
                    type: 'doughnut',
                    data: {
                        labels: labels,
                        datasets: [{
                            data: data,
                            backgroundColor: [
                                "#0000FF", "#00008B", "#0000CD", "#0000FA",
                                "#000080", "#191970", "#1E90FF", "#20B2AA",
                                "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                                "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                                "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                                "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                                "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                                "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                                "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                                "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                                "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                                "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                            ],
                            borderColor: [
                                "#0000FF", "#00008B", "#0000CD", "#0000FA",
                                "#000080", "#191970", "#1E90FF", "#20B2AA",
                                "#4169E1", "#4682B4", "#6495ED", "#87CEEB",
                                "#87CEFA", "#00BFFF", "#00CED1", "#00F5FF",
                                "#33A1C9", "#6699CC", "#6D9BC3", "#728FCE",
                                "#7A89B8", "#7EB3D5", "#7EC0EE", "#85A3D4",
                                "#89CFF0", "#8A2BE2", "#8A73D6", "#8B008B",
                                "#8DB6CD", "#8EE5EE", "#ADD8E6", "#B0E0E6",
                                "#BFEFFF", "#C6DEFF", "#B2DFEE", "#BCD4E6",
                                "#BDD4E7", "#C2DFFF", "#C3E6F9", "#C6E2FF",
                                "#CAE1FF", "#CEE3F6", "#D0E0E3", "#D4E2FC",
                                "#D8E2DC", "#DBE9F4", "#DDA0DD", "#DEE5E5",
                            ],
                            borderWidth: 1
                        }],
                    },
                    options: {
                        animation: {
                            duration: 1500,
                            easing: 'easeInOutQuart'
                        },
                        plugins: {
                            legend: false,
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem, data) {
                                        var dataset = data.datasets[tooltipItem.datasetIndex];
                                        var value = dataset.data[tooltipItem.index];
                                        var label = data.labels[tooltipItem.index];
                                        return dataset.label + ': ' + value;
                                    }
                                }
                            }
                        }
                    }
                });
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindOpenItems();

        //Progress Delay Chart

        var currentDate = new Date();
        $scope.pendYear = currentDate.getFullYear().toString();
        $scope.pendList = [];
        $scope.bindPendItems = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/GetPendItems',
                params: { year: $scope.pendYear }
            }).success(function (response) {

                $scope.pendList = response.data;
                var currentDate = new Date();
                var currentMonth = currentDate.getMonth() + 1;

                //filteredData = $scope.pendList.filter(item => {
                //    var itemMonth = item.month;
                //    return itemMonth === $scope.slocMonth;
                //});

                var yearsArray = response.yearsList.map(item => item.fyear);
                var uniqueYears = yearsArray.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                }).sort();

                $scope.yearList = uniqueYears.map(function (year) {
                    return { year: year };
                });
                console.log($scope.yearList)
                var labels = $scope.pendList.map(item => item.month);
                var data = $scope.pendList.map(item => parseInt(item.itmsCount));

                var progChart = document.getElementById('prog').getContext('2d');
                if ($window.progress != undefined)
                    $window.progress.destroy();
                $window.progress = new Chart(progChart, {
                    type: 'line',
                    data: {
                        labels: labels,
                        datasets: [{
                            label: 'Data',
                            data: data,
                            borderColor: '#306897',
                            backgroundColor: 'lightblue',
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                position: 'top',
                            },
                            title: {
                                display: true,
                                text: 'Chart.js Line Chart'
                            }
                        }
                    },
                });
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindPendItems();


        // Bar Average Chart

        var currentDate = new Date();
        $scope.slocMonth = months[currentDate.getMonth()];
        $scope.slocYear = currentDate.getFullYear().toString();
        var filteredData = [];
        $scope.bindSlocItems = function () {
            var data1 = [];
            var data2 = [];
            var data3 = [];
            var data4 = [];
            var data5 = [];
            var data6 = [];
            var datasets = [];
            var slocList = [];
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getTotalPendReqs',
                params: { year: $scope.slocYear }
            }).success(function (response) {

                slocList = response.data;
                console.log($scope.slocList)
                var currentDate = new Date();
                var currentMonth = currentDate.getMonth() + 1;
                var yearsArray = response.yearsList.map(item => item.fyear);
                var uniqueYears = yearsArray.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                }).sort();

                $scope.yearList = uniqueYears.map(function (year) {
                    return { year: year };
                });
                //var monthsArray = $scope.slocList.map(item => item.month);
                //var uniqueMonths = monthsArray.filter(function (value, index, self) {
                //    return self.indexOf(value) === index;
                //}).sort();

                //$scope.monthList = uniqueMonths.map(function (month) {
                //    return { month: month };
                //});
                //filteredData = slocList.filter(item => {
                //    var itemMonth = item.month;
                //    return itemMonth === $scope.slocMonth;
                //});
                //alert(angular.toJson(filteredData))

                var monthLabels = [$scope.slocMonth];

                data1 = filteredData.map(item => item.locationCounts["RWT1"]);
                data2 = filteredData.map(item => item.locationCounts["KW1M"]);
                data3 = filteredData.map(item => item.locationCounts["KY1M"]);
                data4 = filteredData.map(item => item.locationCounts["KC1M"]);
                data5 = filteredData.map(item => item.locationCounts["KW1X"]);
                data6 = filteredData.map(item => item.locationCounts["KW1I"]);

                datasets = [
                    { label: 'RWT1', data: data1, backgroundColor: '#306897', borderColor: '#306897', borderWidth: 1 },
                    { label: 'KW1M', data: data2, backgroundColor: '#191970', borderColor: '#191970', borderWidth: 1 },
                    { label: 'KY1M', data: data3, backgroundColor: '#4169E1', borderColor: '#4169E1', borderWidth: 1 },
                    { label: 'KC1M', data: data4, backgroundColor: '#6495ED', borderColor: '#6495ED', borderWidth: 1 },
                    { label: 'KW1X', data: data5, backgroundColor: 'lightblue', borderColor: 'lightblue', borderWidth: 1 },
                    { label: 'KW1I', data: data6, backgroundColor: '#AFEEEE', borderColor: '#AFEEEE', borderWidth: 1 }
                ];

                datasets = datasets.filter(dataset => dataset.data.some(value => value !== null));

                //var average = data.reduce((acc, val) => acc + val, 0) / data.length;

                var combinedData = parseInt(data1, 10) + parseInt(data2, 10) + parseInt(data3, 10) + parseInt(data4, 10) + parseInt(data5, 10) + parseInt(data6, 10);
                var combinedAverage = combinedData / 6;
                var baravg1Chart = document.getElementById('baravg1').getContext('2d');
                if ($window.myBaravg1Chart != undefined)
                    $window.myBaravg1Chart.destroy();
                $window.myBaravg1Chart = new Chart(baravg1Chart, {
                    type: 'bar',
                    data: {
                        labels: monthLabels,
                        datasets: datasets
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        },
                        plugins: {
                            legend: false
                        }
                    }

                });
                myBaravg1Chart.update();
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindSlocItems();

        //All Open Requests

        $scope.bindAllItems = function () {
            $http({
                method: 'GET',
                url: '/Dashboard/getAllOpenReqs'
            }).then(function (response) {
                var rawData = response.data;
                console.log(rawData);
                var summarizedData = {};
                rawData.forEach(function (item) {
                    var req = item.req;
                    if (!summarizedData[req]) {
                        summarizedData[req] = 0;
                    }
                    summarizedData[req] += parseInt(item.itmsCount);
                });

                $scope.summaryList = [];
                for (var req in summarizedData) {
                    var formattedCount = summarizedData[req].toLocaleString();
                    $scope.summaryList.push({ req: req, itmsCount: formattedCount });
                }
                console.log($scope.summaryList)
                $scope.cat = $scope.summaryList[0].itmsCount;
                $scope.clf = $scope.summaryList[1].itmsCount;
                $scope.qc = $scope.summaryList[2].itmsCount;
                $scope.qa = $scope.summaryList[3].itmsCount;
                $scope.rel = $scope.summaryList[4].itmsCount;
                $scope.pvpend = $scope.summaryList[5].itmsCount;
                $scope.total = $scope.summaryList[6].itmsCount;
            }).catch(function (error) {
            });
        };

        $scope.bindAllItems();

        $scope.bindItemsAvg = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getRlsdItems'
            }).success(function (response) {

                $scope.avgList = response;
                $scope.repAvg = response[0].repAvg;
                $scope.nonrepAvg = response[0].nonrepAvg;
                if ($scope.repAvg <= 0) {
                    $scope.repAvg = "<1";
                }
                if ($scope.nonrepAvg <= 0) {
                    $scope.nonrepAvg = "<1";
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindItemsAvg();

        $scope.bindModAvg = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getModRlsdItems'
            }).success(function (response) {

                $scope.modAvg = response;
                if (response <= 0) {
                    $scope.modAvg = "<1";
                }
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.bindModAvg();

        //Bar Chart

        var currentDate = new Date();
        $scope.totalYear = currentDate.getFullYear().toString();
        $scope.totalList = [];
        $scope.bindTotalItems = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getTotalReqs',
                params: { tyear: $scope.totalYear }
            }).success(function (response) {
                $scope.totalList = response.data;

                var yearsArray = response.yearsList.map(item => item.year);
                var uniqueYears = yearsArray.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                }).sort();

                $scope.yearList = uniqueYears.map(function (year) {
                    return { year: year };
                });
                console.log(yearsArray)
                var monthLabels = $scope.totalList.map(item => item.month);
                var data1 = $scope.totalList.map(item => item.rep);
                //var data2 = $scope.totalList.map(item => item.nonrep);
                //var average = $scope.totalList.map(item => item.avg);

                var data1Total = data1.reduce(function (accumulator, currentValue) {
                    return accumulator + parseInt(currentValue);
                }, 0);

                var average1 = data1Total / data1.length;

                var initialDisplayMonths = 5;
                var visibleMonthLabels = monthLabels.slice(-initialDisplayMonths);

                var baravgChart = document.getElementById('baravg').getContext('2d');

                //var myBaravgChart = new Chart(baravgChart, {
                //    type: 'bar',
                //    data: {
                //        labels: monthLabels,
                //        datasets: [
                //            {
                //                label: 'Replenishment',
                //                data: data1,
                //                backgroundColor: '#306897',
                //                borderColor: '#306857',
                //                borderWidth: 1
                //            },
                //            {
                //                label: 'NonReplenishment',
                //                data: data2,
                //                backgroundColor: 'lightblue',
                //                borderColor: 'lightblue',
                //                borderWidth: 1
                //            },
                //            {
                //                type: 'line',
                //                label: 'Average',
                //                data: Array(monthLabels.length).fill(average1),
                //                borderColor: 'rgba(0, 0, 0, 0.8)',
                //                borderWidth: 2,
                //                fill: false
                //            }
                //        ]
                //    },
                //    options: {
                //        scales: {
                //            x: {
                //             display: false
                //            },
                //            y: {
                //                grid: {
                //                    borderDash: [2], 
                //                    color: 'rgba(0, 0, 0, 0.2)' 
                //                }
                //            }
                //        },
                //        plugins: {
                //            legend: {
                //                display: true,
                //                position: 'bottom'
                //            }
                //        }
                //    }
                //});
                if ($window.myBaravgChart != undefined)
                    $window.myBaravgChart.destroy();
                $window.myBaravgChart = new Chart(baravgChart, {
                    type: 'bar',
                    data: {
                        labels: monthLabels,
                        datasets: [
                            {
                                label: 'Data',
                                data: data1,
                                backgroundColor: '#306897'
                                //},
                                //{
                                //    label: 'NonReplenishment',
                                //    data: data2,
                                //    backgroundColor: '#191970'
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });

                //myBaravgChart.data.datasets.push({
                //    type: 'line',
                //    label: 'Average',
                //    data: average,
                //    borderColor: 'lightblue',
                //    borderWidth: 2,
                //    fill: false,
                //    borderDash: [5, 5]
                //});

                myBaravgChart.update();
            }).error(function (data, status, headers, config) {
                // handle error
            });
        };
        $scope.bindTotalItems();

        var currentDate = new Date();
        $scope.totalYear = currentDate.getFullYear().toString();
        $scope.totalList = [];
        $scope.bindTotalPendItems = function () {
            $scope.promise = $http({
                method: 'GET',
                url: '/Dashboard/getTotalPendReqs',
                params: { tyear: $scope.totalYear }
            }).success(function (response) {
                $scope.totalList = response.data;

                var yearsArray = response.yearsList.map(item => item.year);
                var uniqueYears = yearsArray.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                }).sort();

                $scope.yearList = uniqueYears.map(function (year) {
                    return { year: year };
                });
                console.log(yearsArray)
                var monthLabels = $scope.totalList.map(item => item.month);
                var data1 = $scope.totalList.map(item => item.rep);
                //var data2 = $scope.totalList.map(item => item.nonrep);
                //var average = $scope.totalList.map(item => item.avg);

                var data1Total = data1.reduce(function (accumulator, currentValue) {
                    return accumulator + parseInt(currentValue);
                }, 0);

                var average1 = data1Total / data1.length;

                var initialDisplayMonths = 5;
                var visibleMonthLabels = monthLabels.slice(-initialDisplayMonths);

                var baravgChart1 = document.getElementById('baravg1').getContext('2d');

                if ($window.myBaravgChart1 != undefined)
                    $window.myBaravgChart1.destroy();
                $window.myBaravgChart1 = new Chart(baravgChart1, {
                    type: 'bar',
                    data: {
                        labels: monthLabels,
                        datasets: [
                            {
                                label: 'Data',
                                data: data1,
                                backgroundColor: '#191970'
                                //},
                                //{
                                //    label: 'NonReplenishment',
                                //    data: data2,
                                //    backgroundColor: '#191970'
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });

                //myBaravgChart.data.datasets.push({
                //    type: 'line',
                //    label: 'Average',
                //    data: average,
                //    borderColor: 'lightblue',
                //    borderWidth: 2,
                //    fill: false,
                //    borderDash: [5, 5]
                //});

                myBaravgChart1.update();
            }).error(function (data, status, headers, config) {
                // handle error
            });
        };
        $scope.bindTotalPendItems();

    });

})();