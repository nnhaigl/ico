//mainModule.controller("profileCtrl", function ($scope, $http, $sce, $q, $window, $filter) {
mainModule.controller('profileCtrl', ['$scope', '$http', '$sce', '$q', '$window', '$filter', '$timeout'
    , function ($scope, $http, $sce, $q, $window, $filter, $timeout) {

        $scope.promise = null;
        $scope.mode = "view";
        $scope.username = '';
        $scope.country = null;
        $scope.basicInfo = {};
        $scope.userLevel = null;
        $scope.initBasicInfo = {
            DateOfBirth: new Date()
        };
        $scope.accountInfo = {
            DateOfBirth: new Date()
        };

        $scope.Genders = [
            { ID: true, Title: 'MALE' },
            { ID: false, Title: 'FEMALE' }
        ];

        // for changing password / password 2 FA
        $scope.passwordInfo = {
            OldPassword: '',
            NewPassword: '',
            ConfirmPassword: ''
        };

        $scope.passErrorMessages = null;
        $scope.basicInfoErrorMessages = null;

        $scope.init = function () {
            $scope.username = '@currUsername';
            $scope.getUserDetail();
        };

        $scope.resetPassValue = function () {
            $scope.passwordInfo.OldPassword = '';
            $scope.passwordInfo.NewPassword = '';
            $scope.passwordInfo.ConfirmPassword = '';
            $scope.passErrorMessages = null;
        };

        $scope.getUserDetail = function () {
            $scope.promise = $http.get("/bizzApi/user/detail")
                .success(function (data) {
                    if (!data.IsSuccess) {
                        showServerError();
                    } else {
                        $('#divProfileCtrl').show();

                        $scope.accountInfo = data.Value.Account;
                        $scope.basicInfo = data.Value.UserInfo;
                        $scope.country = data.Value.Country;
                        $scope.userLevel = data.Value.UserLevel;

                        $scope.initBasicInfo = angular.copy($scope.basicInfo);

                        $scope.basicInfo.DOB = $filter('date')($scope.basicInfo.DateOfBirth, 'dd/MM/yyyy');
                        $scope.initBasicInfo.DOB = $filter('date')($scope.initBasicInfo.DateOfBirth, 'dd/MM/yyyy');
                    }
                })
                .error(function () {
                    showServerError();
                });
        };

        $scope.changeMode = function (mode) {
            $scope.mode = mode;
            if (mode == 'view') {
                $scope.basicInfo = angular.copy($scope.initBasicInfo);
            }
        };

        $scope.editBasicInfo = function () {
            $scope.basicInfoErrorMessages = null;

            $scope.promise = $http.post('/bizzApi/user/editBasicInfo', $scope.basicInfo)
                .success(function (data) {
                    if (data.IsSuccess) {
                        if (!data.IsValid) {
                            $scope.basicInfoErrorMessages = data.PropertyErrors;
                        } else {
                            showSuccessDefault();
                            $scope.getUserDetail();
                            $scope.changeMode('view');
                        }
                    }
                });
        };


        $scope.showModalPassword = function (isShow) {
            $scope.resetPassValue();
            //$('#modal_changePass').modal(isShow);
            if (isShow) {
                $('#modal_changePass').modal('show');
            } else {
                $('#modal_changePass').modal('hide');
            }
        };

        $scope.showModalPassword2FA = function (isShow) {
            //$('#modal_change2FA').modal(isShow);
            if (isShow) {
                $('#modal_change2FA').modal('show');
            } else {
                $('#modal_change2FA').modal('hide');
            }
            $scope.resetPassValue();
        };

        $scope.changePassword = function (url) {
            $scope.doChangePassword('password');
        };

        $scope.changePassword2FA = function (url) {
            $scope.doChangePassword('password2FA');
        };

        $scope.doChangePassword = function (type) {
            $scope.passErrorMessages = null;

            var url = '';
            if (type == 'password')
                url = '/bizzApi/user/changePassword';
            else
                url = '/bizzApi/user/changePassword2FA';

            $scope.promise = $http.post(url, $scope.passwordInfo)
                .success(function (data) {
                    if (data.IsSuccess) {
                        if (!data.IsValid) {
                            $scope.passErrorMessages = data.PropertyErrors;
                        } else {
                            showSuccessDefault();
                            if (type == 'password2FA') {
                                // update password 2FA
                                $scope.accountInfo.Password2FA = $scope.passwordInfo.NewPassword;
                                $scope.showModalPassword2FA(false);
                            }
                            else
                                $scope.showModalPassword(false);
                        }
                    }
                });
        };

    }]);