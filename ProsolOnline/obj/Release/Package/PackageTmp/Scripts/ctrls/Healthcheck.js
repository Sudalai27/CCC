(function () {
    'use strict';
    var app = angular.module('ProsolApp', ['cgBusy','datatables']);


    app.controller('HealthcheckController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {


        $scope.resdwn = false;
        $scope.ShowHide = false;
        $scope.files = [];

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
                    $scope.NotifiyRes = true;
                    $scope.$apply();
                }
            }
        };
      
        $scope.successFlag = false;
        $scope.checkdata = function () {
            $scope.successFlag = true;
            var formData = new FormData();
            formData.append('batch_file', $scope.files[0]);

            $.ajax({
                type: "post",
                // url: 'https://ml.prosolonline.com/health_check.php',
                url: 'https://coda.infony.in/health_check.php',
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: function () {
                    $("#loading_div").show();
                    $("#result_div").hide();
                },
                success: function (response) {
                    var result = JSON.parse(response);
                 
                   // console.log(result);
                    $scope.fetch_output(result);
                    $("#loading_div").hide();

                    $("#result_div").show();
                  
                }
            });

        }
        //New

        $scope.fetch_output = function (result) {

            console.log(result);
            var Total_Count = result.Total_Count;
            document.getElementById("total_items").innerHTML = Total_Count;

            var material_code_duplicate = result.material_code_duplicate;
            document.getElementById("duplicate_m_c").innerHTML = material_code_duplicate
            var short_desc_duplicate = result.short_desc_duplicate;
            document.getElementById("duplicate_short").innerHTML = short_desc_duplicate;
            var long_desc_duplicate = result.long_desc_duplicate;
            document.getElementById("duplicate_long").innerHTML = long_desc_duplicate;
            var Sourceduplicatecount = result.Sourceduplicatecount;
            document.getElementById("source_duplicate_count").innerHTML = Sourceduplicatecount;

            var short_0 = result.short_0;
            document.getElementById("short_0").innerHTML = short_0;
            var short_0_15 = result.short_0_15;
            document.getElementById("short_0_15").innerHTML = short_0_15;
            var short_15_40 = result.short_15_40;
            document.getElementById("short_15_40").innerHTML = short_15_40;
            var short_40 = result.short_40;
            document.getElementById("short_40").innerHTML = short_40;

            var long_0 = result.long_0;
            document.getElementById("long_0").innerHTML = long_0;
            var long_0_40 = result.long_0_40;
            document.getElementById("long_0_40").innerHTML = long_0_40;
            var long_40_100 = result.long_40_100;
            document.getElementById("long_40_100").innerHTML = long_40_100;
            var long_100 = result.long_100;
            document.getElementById("long_100").innerHTML = long_100;


            var total_manufacturer = result.total_manufacturer;
            document.getElementById("total_manu").innerHTML = total_manufacturer;

            var res_manufacturer_name = result.manufacturer_name;
            var res_manufacturer_count = result.manufacturer_count;
            var manu_html = '';
            manu_html += '<tr><th>Manufacture List</th><th>Approx. Count</th></tr>';

            for (var i = 0; i < res_manufacturer_name.length; i++) {
                manu_html += '<tr><td>' + res_manufacturer_name[i] + '</td><td>' + res_manufacturer_count[i] + '</td></tr>';
            }
            document.getElementById('manu_list_html').innerHTML = manu_html;

            var total_partno = result.total_partno;
            document.getElementById("total_pn").innerHTML = total_partno;
            var total_refno = result.total_refno;
            document.getElementById("total_refno").innerHTML = total_refno;
            var total_drgno = result.total_drgno;
            document.getElementById("total_drgno").innerHTML = total_drgno;


            var uom_count = result.uom_count;
            form_ind_count(uom_count, 'uom_data_html');

            var noun_count = result.noun_count;
            form_ind_count(noun_count, 'top_noun_html');

            var modifier_count = result.modifier_count;
            form_ind_count(modifier_count, 'top_modifier_html');

            var material_type_count = result.material_type_count;
            form_ind_count(material_type_count, 'material_type_html');

            var material_group_count = result.material_group_count;
            form_ind_count(material_group_count, 'material_group_html');

            var plant_count = result.plant_count;
            form_ind_count(plant_count, 'plant_html');

            var vendor_count = result.vendor_count;
            form_ind_count(vendor_count, 'vendor_html');

        }
        function form_ind_count(data, html_id) {
            var html = '';
            for (var i = 0; i < data.length; i++) {
                html += '<tr><td>' + data[i].Term + '</td><td>' + data[i].Count + '</td></tr>';
            }
            document.getElementById(html_id).innerHTML = html;
        }

        function show_div() {
            var x = document.getElementById('detail_view');
            if (x.style.display === "block") {
                x.style.display = "none";
            } else {
                x.style.display = "block";
            }
        }

        $scope.show_result = function (id, dis_type) {
            var x = document.getElementById(id);
            if (x.style.display === dis_type) {
                x.style.display = "none";
            } else {
                x.style.display = dis_type;
            }
        }
    }]);


})();