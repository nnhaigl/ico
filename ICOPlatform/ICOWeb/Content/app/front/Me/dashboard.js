//mainModule.controller("dashboardCtrl", function ($scope, $http, $sce, $q, $window, $filter) {
mainModule.controller('dashboardCtrl', ['$scope', '$http', '$sce', '$q', '$window', '$filter', '$timeout'
    , function ($scope, $http, $sce, $q, $window, $filter, $timeout) {

    $scope.promise = null;
    $scope.dashboardInfo = {};

    $scope.init = function () {
        $scope.getDashboardInfo();
        //new PNotify({
        //    title: '<b>Announcement</b>',
        //    text: "We're so sorry for re-calculating the profit algorithm .The previous GH has been rejected .You can re-GH from now on. </br> <b> New Coin Club wish you a happy day</b> !",
        //    addclass: 'bg-danger'
        //});
    };

    $scope.getDashboardInfo = function () {
        $scope.promise = $http.get("/bizzApi/user/dashboard/me")
         .success(function (data) {
             if (data.IsSuccess) {
                 $('#divDashboardCtrl').show();
                 $scope.dashboardInfo = data.Value;
             }
         });
    };

}]);
