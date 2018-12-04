mainModule.directive('numberOnlyInput', function () {
    return {
        restrict: 'EA',
        template: '<input name="{{inputName}}" ng-model="inputValue" type="text" class="sm-form-control required" placeholder="Ví dụ 0.1" aria-required="true" />',
        //template: ' <input name="{{inputName}}" ng-model="inputValue" ng-keypress="suggest();" type="text" class="sm-form-control required" placeholder="Ví dụ 0.1" aria-required="true">',
        scope: {
            someCtrlFn: '&callbackFn',
            inputValue: '=',
            inputName: '='
        },
        link: function (scope) {

            scope.$watch('inputValue', function (newValue, oldValue) {
                var arr = String(newValue).split("");
                if (arr.length === 0) return;
                if (arr.length === 1 && (arr[0] == '-' || arr[0] === '.')) return;
                if (arr.length === 2 && newValue === '-.') return;
                if (isNaN(newValue)) { // không phải là số
                    scope.inputValue = oldValue;
                } else {
                    if (!isNaN(oldValue)) {
                        scope.someCtrlFn({ inputBTCNumber: scope.inputValue });
                    }
                }
            });
        }
    };
});


mainModule.directive('biClick', function ($parse) {
    return {
        compile: function ($element, attr) {
            var handler = $parse(attr.biClick);
            return function (scope, element, attr) {
                element.on('click', function (event) {
                    scope.$apply(function () {
                        var promise = handler(scope, { $event: event });
                        if (promise && angular.isFunction(promise.finally)) {
                            element.attr('disabled', true);
                            promise.finally(function () {
                                element.attr('disabled', false);
                            });
                        }
                    });
                });
            };
        }
    };
});


// prevent double submiting 
//http://jsfiddle.net/miensol/7PETf/3/
//http://miensol.pl/angularjs/2014/05/21/ngclick-and-double-post.html