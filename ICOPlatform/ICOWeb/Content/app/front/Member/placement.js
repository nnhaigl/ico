
//mainModule.controller("placeCtrl", function ($rootScope, $scope, $http, $sce, $q, $window, $timeout, $filter) {
mainModule.controller('placeCtrl', ['$scope', '$http', '$sce', '$q', '$window', '$filter', '$timeout'
    , function ($scope, $http, $sce, $q, $window, $filter, $timeout) {

    $scope.promise = null;
    $scope.promise2 = null;

    $scope.subUsers = [];
    $scope.pendingUsers = [];
    $scope.activeUsers = [];
    $scope.completeUsers = [];

    $scope.placeUserInfo = null;
    $scope.errorMessages = null;

    $scope.userToPlace = {
        Position: 2,
        UserToPlace: '', // user nào sẽ được xếp vào cây
        UserToPlaceInto: '' // vị trí sẽ được xếp vào cây
    };

    $scope.positions = {
        choices: [{
            id: 1,
            text: "Left side",
            isSelected: "false"
        }, {
            id: 2,
            text: "Right side",
            isSelected: "false"
        }]
    };

    $scope.setPosition = function (choice) {
        angular.forEach($scope.positions.choices, function (c) {
            if (c.id == choice.id) {

            } else {
                c.isSelected = "false";
            }
        });

    };

    $scope.query = {
        PageIndex: 1,
        PageSize: 1000000,
    };

    $scope.init = function () {
        $scope.getPlacement();
    };

    $scope.placeUser = function (user) {
        $scope.placeUserInfo = user;
        $scope.showModalPlacement(true);
        $scope.errorMessages = null;
    };

    $scope.placeLeft = function (user) {
        $scope.userToPlace.Position = 1;
        $scope.placeUser(user);
    };

    $scope.placeRight = function (user) {
        $scope.userToPlace.Position = 2;
        $scope.placeUser(user);
    };

    $scope.doPlaceUser = function () {
        $scope.errorMessages = null;
        $scope.userToPlace.UserToPlace = $scope.placeUserInfo.Username; // user nào sẽ được xếp vào cây

        //$scope.userToPlace.Position = _.filter($scope.positions.choices, function (xx) { return xx.isSelected == "true"; });
        //console.log($scope.userToPlace.Position);

        //return;
        $scope.promise2 = $http.post("/bizzApi/user/PlaceUser", $scope.userToPlace)
         .success(function (data) {
             if (data.IsSuccess) {
                 if (!data.IsValid) {
                     $scope.errorMessages = data.PropertyErrors;
                 } else {
                     showSuccessDefault();
                     $scope.showModalPlacement(false);
                     $scope.getPlacement();
                 }
             }
         });
    };

    $scope.getPlacement = function () {
        $scope.promise = $http.post("/bizzApi/user/ReferralUsers", $scope.query)
         .success(function (data) {
             if (data.IsSuccess) {
                 $scope.subUsers = data.Data;
                 //$scope.pendingUsers = data.Data;

                 $scope.pendingUsers = _.filter($scope.subUsers, function (xx) { return xx.ReferralStatus == _REFERRAL_STATUS.PENDING; });
                 $scope.activeUsers = _.filter($scope.subUsers, function (xx) { return xx.ReferralStatus == _REFERRAL_STATUS.ACTIVE; });
                 $scope.completeUsers = _.filter($scope.subUsers, function (xx) { return xx.ReferralStatus == _REFERRAL_STATUS.COMPLETE; });

                 //_REFERRAL_STATUS
                 $('#placeCtrl').show('slow');
             }
         });
    };

    $scope.showModalPlacement = function (isShow) {
        if (isShow) {
            //$('#modal_placement').hide('slow');
            $('#modal_placement').modal('show');
        } else {
            $('#modal_placement').modal('hide');
        }
    };

}]);
