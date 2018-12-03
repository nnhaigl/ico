//var mainModule = angular.module('frApp', ["ui.bootstrap", "cgBusy","ngClipboard"]); // ngAnimate

// angular cbBusy loading indicator
var mainModule = angular.module('frApp', ["ui.bootstrap", "cgBusy"]).value('cgBusyDefaults', {
    message: 'Loading...',
    backdrop: true,
    //templateUrl: 'my_custom_template.html',
    delay: 0, // khoảng thời gian trễ để show loading khi bắt đầu promise
    minDuration: 500 // khoảng thời gian trễ sau khi promise được hoàn thành để hủy show loading
    //wrapperClass: 'my-class my-class2'
});

