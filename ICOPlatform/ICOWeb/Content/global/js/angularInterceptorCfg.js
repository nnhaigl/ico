(function () {
    'use strict';

    //intercept API error responses
    mainModule.config(["$httpProvider", "$provide", function ($httpProvider, $provide) {

        $httpProvider.interceptors.push(["$q", "$window", "$location", "sessionStorageSvc", "localStorageSvc",
                                                    function ($q, $window, $location, sessionStorageSvc, localStorageSvc) {
            return {
                'request': function (config) {
                  
                    var httpMethod = config.method;
                    var timestamp = new Date().ToDateTime();
                    var uri = config.url;
                    var requestBodyJson = config.data;
                    var hashedPassword = "37ddec7688d3774fa239bcc2f6416a59"

                    var requestBodyString = "";
                    var paramString = getAllParametersFromUrl(uri);

                    $.each(requestBodyJson, function (key, val) {
                        requestBodyString = requestBodyString + key + "=" + val + "&";
                    });

                    if (requestBodyString.length > 0) {
                        requestBodyString = requestBodyString.substring(0, requestBodyString.length - 1);
                    }

                    var message = httpMethod + "\n";
                    message = message + timestamp + "\n";
                    message = message + uri + "\n";
                    if (paramString) {
                        //message = message + paramString + "&";
                        message = message + paramString + "n";
                    }
                    //message = message + requestBodyString + "\n";

                    //console.log("message = " + message);

                    var signatureArr = sha256.hmac(hashedPassword, message); // dạng Uint8Arrays 
                    //var signature = nacl.util.decodeUTF8(signatureArr);
                    var signature = ""; //new TextDecoder("utf-8").decode(signatureArr);
                    //for (var i = 0; i < signatureArr.byteLength; i++) {
                    //    signature += String.fromCharCode(signatureArr[i])
                    //}

                    //console.log("signature = ");
                    //console.log(signature);   
                    // signature của user
                    config.headers['Timestamp'] = timestamp;
                    config.headers['Username'] = localStorageSvc.getItem(_SESSION_STORAGE.USER); //$window.sessionStorage[_SESSION_STORAGE.USER]; // sessionStorageSvc.getStorageKey(_SESSION_STORAGE.USER);  //'@BizzWebCore.Context.ApplicationContext.CurrentUsername';
                    config.headers['Authentication'] = 'admin:' + signature; // {username}:{signature}
                    
                    //console.log(config);
                    // thêm header để server recognize ajax request
                    config.headers['X-Requested-With'] = 'XMLHttpRequest';
                    return config;
                },

                'response': function (response) {
                    //console.log("status code from module/config.js response callback : " + response.status);
                    if (response.data) {

                        if (response.data.IsSuccess === true) {
                            //notificationProvider.notify(response.data);
                        }
                        if (response.data.IsSuccess === false) {
                            showServerError();
                            //if (response.data.Message)
                            //    console.log(response.data.Message);
                            //else
                            //    console.log("Server Error");
                        }
                    }

                    return response;
                },
                'responseError': function (rejection) {
                    showServerError();
                    console.log(" INTERCEPTOR : status code from module/config.js responseError callback " + rejection.status);
                    //console.log(rejection.config.url + "<br />" + rejection.data.Message);
                    //if (rejection.status == 401) {
                    //    console.log("status code 401");
                    //    //$location.url("/Authentication/login");
                    //    $window.location.href = '/Authentication/login';
                    //} else if (rejection.status == 403) {
                    //    console.log("status code 403");
                    //    //$location.url("/global/global/accessdenied");
                    //    $window.location.href = '/global/Redirect/accessdenied';
                    //} else if (rejection.status == 404) {
                    //    console.log("status code 404");
                    //    //$location.url("/global/global/notFound");
                    //    $window.location.href = '/global/Redirect/notFound';
                    //} else if (rejection.status == 500) {
                    //    console.log("status code 500");
                    //    //$location.url("/global/global/internalServerError");
                    //    $window.location.href = '/global/Redirect/internalServerError';
                    //}

                    return $q.reject(rejection);
                }
            };
        }]);
        
        $provide.decorator("$exceptionHandler", ['$delegate', function ($delegate) {
            return function (exception, cause) {
                console.error(exception);
            };
        }]);
        
    }]);
})();