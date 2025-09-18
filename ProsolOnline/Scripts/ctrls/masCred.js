
(function () {
    'use strict';

    var rootApp = angular.module('rootApp', ['ProsolApp']);

    rootApp.run(function ($rootScope, $http) {
        //  var pages = '@Request.RequestContext.HttpContext.Session["access"]';

        //$rootScope.p1 = false;
        //$rootScope.p2 = false;
        //$rootScope.p3 = false;
        //$rootScope.p4 = false;
        //$rootScope.p5 = false;
        //$rootScope.p6 = false;
        //$rootScope.p7 = false;
        //$rootScope.p8 = false;
        // $rootScope.p9 = false;


       
       
        var pages = $("#myId").val();       
        var response = pages.split(",");

        if (response.length > 0) {
            if (response.indexOf("Import") > -1) {
                $rootScope.p1c1 = true;
              //  $rootScope.p1 = true;

            }
            if (response.indexOf("Export") > -1) {
                $rootScope.p1c2 = true;
               // $rootScope.p1 = true;
            }
            if (response.indexOf("Assign Work") > -1) {
                $rootScope.p1c3 = true;
               // $rootScope.p1 = true;
            }
            if (response.indexOf("Search by REF") > -1) {
                $rootScope.p2c1 = true;
               // $rootScope.p2 = true;
            }
            if (response.indexOf("Search by DESC") > -1) {
                $rootScope.p2c2 = true;
                //$rootScope.p2 = true;
            }
            if (response.indexOf("Request Item") > -1) {
                $rootScope.p3c1 = true;
               // $rootScope.p3 = true;
            }
            if (response.indexOf("Request Log") > -1) {
                $rootScope.p3c2 = true;
              //  $rootScope.p3 = true;
            }
            if (response.indexOf("Approve Item") > -1) {
                $rootScope.p3c3 = true;
              //  $rootScope.p3 = true;
            }
            if (response.indexOf("Catalogue") > -1) {
                $rootScope.p4c1 = true;
               // $rootScope.p4 = true;
            }
            if (response.indexOf("Review") > -1) {
                $rootScope.p4c2 = true;
                //$rootScope.p4 = true;
            }
            if (response.indexOf("Release") > -1) {
                $rootScope.p4c3 = true;
               // $rootScope.p4 = true;
            }
            if (response.indexOf("General") > -1) {
                $rootScope.p5c1 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("Plant") > -1) {
                $rootScope.p5c2 = true;
              //  $rootScope.p5 = true;
            }
            if (response.indexOf("Mrp data") > -1) {
                $rootScope.p5c3 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("Sales & Others") > -1) {
                $rootScope.p5c4 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("Vendor Details") > -1) {
                $rootScope.p5c5 = true;
                //$rootScope.p5 = true;
            }
            if (response.indexOf("General Settings") > -1) {
                $rootScope.p5c6 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("Sequence Master") > -1) {
                $rootScope.p5c7 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("UOM Settings") > -1) {
                $rootScope.p5c8 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("Add Logics") > -1) {
                $rootScope.p5c9 = true;
               // $rootScope.p5 = true;
            }
            if (response.indexOf("UNSPSC Master") > -1) {
                $rootScope.p5c10 = true;
              //  $rootScope.p5 = true;
            }
            if (response.indexOf("View") > -1) {
                $rootScope.p6c1 = true;
               // $rootScope.p6 = true;
            }
            if (response.indexOf("Create") > -1) {
                $rootScope.p6c2 = true;
               // $rootScope.p6 = true;;
            }
            if (response.indexOf("Value Standardization") > -1) {
                $rootScope.p7c1 = true;
               // $rootScope.p7 = true;
            }
            if (response.indexOf("Repository cleansing") > -1) {
                $rootScope.p7c2 = true;
               // $rootScope.p7 = true;
            }
            if (response.indexOf("User") > -1) {
                $rootScope.p8c1 = true;
               // $rootScope.p8 = true;
            }
            if (response.indexOf("Access permissions") > -1) {
                $rootScope.p8c2 = true;
               // $rootScope.p8 = true;
            }
            if (response.indexOf("Tracking") > -1) {
                $rootScope.p9c1 = true;
               // $rootScope.p9 = true;
            }

        };

        //$http.get('/User/getUserinfo').success(function (response) {
        //    $rootScope.usrname = response.FirstName + ' ' + response.LastName;
        //});         

    });
   
})();