var loginModule = angular.module('loginApp', ["cgBusy"]);

//loginModule.controller("userLoginCtrl", function ($scope, $http, $sce, $q, $window) {

loginModule.controller('userLoginCtrl', ['$scope', '$http', '$sce', '$q', '$window', '$filter', '$timeout'
    , function ($scope, $http, $sce, $q, $window, $filter, $timeout) {

        $scope.returnUrl = '';
        $scope.init = function () {
        };

        $scope.username = '';
        $scope.message = ""
        $scope.pass = '';
        $scope.isLoginFailed = null;
        $scope.promise = null; // show loading
        $scope.init = function () {
            //$scope.returnUrl = '@returnUrl';
        };

        $scope.loginKeyPress = function (keyEvent) {
            if (keyEvent.which === 13)
                $scope.login();
        };

        $scope.login = function () {
            $scope.message = "";
            $scope.isLoginFailed = false;

            var g_recaptcha_response = $('#g-recaptcha-response').val();
            //var x = document.getElementsByName("g-recaptcha-response")[0];

            if ($scope.username == "" || $scope.pass == "") {
                $scope.isLoginFailed = true;
                $scope.message = "Please fill in form !";
                return;
            }

            $scope.promise = $http.post("/user/login", {
                Username: $scope.username,
                Password: $scope.pass,
                g_recaptcha_response: g_recaptcha_response
            })
                .success(function (data) {
                    if (data.IsSuccess) {
                        //localStorageSvc.setItem(_SESSION_STORAGE.USER, $scope.username);
                        $window.localStorage.setItem(_SESSION_STORAGE.USER, angular.toJson($scope.username));
                        $window.location.href = '/me/dashboard';
                        //alert($window.localStorage.getItem(_SESSION_STORAGE.USER));

                        //if ($scope.returnUrl != '' && $scope.returnUrl != '/')
                        //    $window.location.href = $scope.returnUrl;
                        //else
                        //    $window.location.href = '/me/dashboard';
                    } else {
                        $scope.isLoginFailed = true;

                        var errorCapcha = data.CaptchaErrorMess;
                        if (errorCapcha != null && errorCapcha != "") {
                            $scope.message = "Please confirm captcha !";
                        } else {
                            $scope.message = "Login failed !";
                        }
                    }
                })
                .error(function (data, status, headers, config) {
                    new PNotify({
                        title: 'Error',
                        text: 'Đã có lỗi xảy ra, vui lòng liên hệ ban quản trị',
                        icon: 'icon-blocked',
                        type: 'error'
                    });
                });
        };


        $scope.renderHtml = function (html_code) {
            return $sce.trustAsHtml(html_code);
        };

    }]);