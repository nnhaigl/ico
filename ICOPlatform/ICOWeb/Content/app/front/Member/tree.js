var _data = null;
var _initUsername = null;
var _previousUsername = null;
var _currentUsername = null;
var _parentUserName = null;

// TODO : Hàm để tìm kiếm phần tử trong node
var __findNode = function (value, _trees, _parentId, _isLeftNode) {

    //if (_trees.id == value) {
    //    //console.logog("Found");
    //    return;
    //}

    //if (_trees.children == null || _trees.children.length == 0)
    //{
    //    return null;
    //}

    if (_trees != null /*&& _trees.id != 0*/) {
        if (value > 0) {
            if (_trees.id == value) {
                // đã tìm thầy phần tử , do something here
                //console.logog("Found ");
                return 0;
            } else {
                if (_trees.children != null && _trees.children.length > 0) {
                    //// -- left node
                    if (_trees.children[0] != null) {
                        if (_trees.children[0].id == value) {
                            // đã tìm thầy phần tử , do something here
                            //console.logog("Found on left node");
                            return 0;
                        } else if (_trees.children[0].id != 0) {
                            // tìm kiếm tiếp nhánh bên trái nếu nhánh trái khác null
                            ////console.logog("_trees.children[0].id = " + _trees.children[0].id);
                            __findNode(value, _trees.children[0], _parentId);
                        }
                    }

                    //// -- right node
                    if (_trees.children[1] != null) {
                        if (_trees.children[1].id == value) {
                            // đã tìm thầy phần tử , do something here
                            //console.logog("Found on right node");
                            return 0;
                        } else if (_trees.children[1].id != 0) {
                            // tìm kiếm tiếp nhánh bên phải nếu nhánh phải khác null
                            ////console.logog("_trees.children[1].id = " + _trees.children[1].id);
                            __findNode(value, _trees.children[1], _parentId);
                        }
                    }
                }
            }
        } else {
            // ------------------ tìm kiếm matched condition với value < 0 && parentId == node.ParentId
            if (_trees.id == value) {
                // đã tìm thầy phần tử , do something here
                //console.logog("Found " + value + " parentId = " + _parentId);
                return 0;
            } else {
                if (_trees.children != null && _trees.children.length > 0) {
                    //// -- left node
                    if (_trees.children[0] != null) {
                        if (_trees.children[0].id == value && _trees.children[0].ParentId == _parentId && _isLeftNode) {
                            // đã tìm thầy phần tử , do something here
                            //console.logog("Found on left node xxx");
                            //_trees.children = [];
                            ////console.logog(_trees.children);
                            ////console.logog(_trees.children[0]);
                            //_trees.children[0] = testObject;
                            //_trees.children[0].push(testObject);
                            rebuildTree2();
                            return 0;
                        } else if (_trees.children[0].id != 0) {
                            // tìm kiếm tiếp nhánh bên trái nếu nhánh trái khác null
                            ////console.logog("_trees.children[0].id = " + _trees.children[0].id);
                            __findNode(value, _trees.children[0], _parentId);
                        }
                    }

                    //// -- right node
                    if (_trees.children[1] != null) {
                        if (_trees.children[1].id == value && _trees.children[1].ParentId == _parentId && !_isLeftNode) {
                            // đã tìm thầy phần tử , do something here
                            //console.logog("Found on right node xxx");
                            //_trees.children = [];
                            ////console.logog(_trees.children);
                            //_trees.children[1].children = testObject;
                            ////console.logog(_trees.children[1]);
                            //_trees.children[1] = testObject; /////////////////
                            //_trees.children = testObject;
                            //_trees.children.push(null);
                            rebuildTree2();
                            return 0;
                        } else if (_trees.children[1].id != 0) {
                            // tìm kiếm tiếp nhánh bên phải nếu nhánh phải khác null
                            ////console.logog("_trees.children[1].id = " + _trees.children[1].id);
                            __findNode(value, _trees.children[1], _parentId);
                        }
                    }
                }
            }
        }
    }
};

var triggerEvent = function () {
    $('.trcont').click(function (e) {
        e.stopPropagation();
        //$(".tree").on("click", ".trcont", function () {
        var liTag = $(this).parent();
        var nodeId = liTag.attr("id"); // li tag
        var _username = liTag.attr("data-username"); // li tag

        ////console.logog(username);
        var isLoadChild = liTag.attr("isLoaded");
        ////console.logog(isLoadChild);

        ////console.logog(__jsonStructureObject);

        var topNode = _data[0];
        var parentListTag = liTag.parent().parent().parent();
        var parentIdOfClickedNode = parentListTag.attr("id");
        var dataId = liTag.attr("data-id");
        var isLeftNode = dataId == 0 ? true : false;
        ////console.logog("parentIdOfClickedNode = " + parentIdOfClickedNode);

        //__findNode(nodeId, topNode, parentIdOfClickedNode, isLeftNode);

        if (!isLoadChild) {
            // node đã load child
            liTag.attr("isLoaded", true);

            // request server de load node con
            if (nodeId != 0) {

            } else {

            }
        }
        if (_username != null && _username != "null" && _username != "") {
            //$('span#backUsername').html(_username);
            $('#divBackTree').css('display', 'block');
            rebuildTree(_username);
        }

    });
};

var setTreeStyle = function () {
    //$('div.ui-widget-content').html('<img src="../../Content/global/images/avatar_placeholder.jpg" class="img-circle img-sm" alt="">');

    $('div.ui-widget-content').each(function () {
        var _this = $(this);
        var tnode = $(this).closest('li.tnode');
        var gender = tnode.attr('data-gender');
        var username = tnode.attr('data-username');

        if (username == null || username == 'null') {
            _this.html('<img src="../../Content/global/images/avatar_placeholder.jpg" class="img-circle img-sm" alt="">');
        } else {
            if (gender == true || gender == "true") {
                _this.html('<img src="../../Content/global/images/man.png" class="img-circle img-sm" alt="">');
            } else {
                _this.html('<img src="../../Content/global/images/woman.png" class="img-circle img-sm" alt="">');
            }
        }
    });


    $('.ui-widget-header').remove();

    $('div.ui-widget-content ui-corner-bl ui-corner-br').removeClass('ui-widget-content ui-corner-bl ui-corner-br');
    $('div.ui-widget-content').removeClass('ui-widget-content');

    $('span.ui-icon').remove();
    $('div.funcbtnb').remove();

    //$('div.ui-widget-header').each(function () {
    //    var content = $(this).html();
    //    var tnode = $(this).closest('li.tnode');
    //    var TotalPHDownside = tnode.attr('data-totalphdownside');
    //    var newContent = '<span>' + content + '</span> <br>' + '<span class="ph-info">' + TotalPHDownside + ' PH </span>';
    //    $(this).html(newContent);
    //});

    $('div.ui-corner-bl').each(function () {
        var tnode = $(this).closest('li.tnode');
        var username = tnode.attr('data-username');
        if (username == 'null' || username == null || username == undefined) {
            username = '-- None --';
        }
        var TotalPHDownside = tnode.attr('data-totalphdownside');
        var newContent = '<br/> <span>' + username + '</span> <br>' + '<span class="ph-info">' + TotalPHDownside + ' PH </span>';
        $(this).append(newContent);
    });


};

//mainModule.controller("memberTreeCtrl", function ($rootScope, $scope, $http, $sce, $q, $window, $timeout, $filter) {
mainModule.controller('memberTreeCtrl', ['$scope', '$http', '$sce', '$q', '$window', '$filter', '$timeout', 'localStorageSvc'
    , function ($scope, $http, $sce, $q, $window, $filter, $timeout, localStorageSvc) {

        $scope.promise = null;
        $scope.treeValues = {};
        $scope.currentUsername = '';
        $scope.init = function () {
            $scope.currentUsername = localStorageSvc.getItem(_SESSION_STORAGE.USER);
            $scope.initTree();
            _initUsername = $scope.currentUsername;
        };

        $scope.initTree = function () {
            $scope.promise = $http.get("/bizzApi/user/member/Tree/" + $scope.currentUsername, { extra: '' })
             .success(function (data) {
                 //console.logog("tree", data);
                 if (data.IsSuccess) {
                     $scope.treeValues = data.Value.Trees;
                     _data = $scope.treeValues;
                     _currentUsername = $scope.currentUsername;
                     $('#divMemberTreeCtrl').show('slow');

                     $("#tree").nadyTree({
                         callType: 'obj',
                         structureObj: $scope.treeValues
                     });
                     $("#newTree").innerHTML = data.Message;
                     // $scope.buildTreeNew(data.Value.Trees);
                     triggerEvent();
                     setTreeStyle();
                 }
             });
        };

        $scope.BindHTML = function (input) {
            return $sce.trustAsHtml(input);
        };
        $scope.buildTreeNew = function (data) {
            var aaa = "<ul><li>";
            aaa += getHTML(data[0]);
            aaa += NewBuild(data[0]);
            aaa += "</li></ul>";
            $(".network-tree-stucture").html(aaa);
        };
        function NewBuild(input) {
            var strReturn = "";
            if (input.children != null && input.children.length > 0) {
                strReturn += "<ul>";
                input.children.forEach(function (item) {

                    strReturn += "<li>";
                    strReturn += getHTML(item);
                    strReturn += NewBuild(item);
                    strReturn += "</li>";
                });
                strReturn += "</ul>";
            }
            return strReturn;
        }
        var LevelCode = ["C-M", "C-Silver", "C-Gold", "C-Daimond", "C-Platinum", "C-King"];
        var LevelCodeClass = ["user-type-41", "user-type-3", "user-type-4", "user-type-5", "user-type-60", "status-22"];
        function getHTML(item) {
            if (LevelCode.indexOf(item.level) >= 0) {
                return '<span class="inline-block relative"> <a href="javascript:BuildNew(\'' + item.username + '\')" class="' + LevelCodeClass[LevelCode.indexOf(item.level)] + ' product-6 user-packet"><span class="user-name">' + item.username + '</span></a> <span class="tooltip-icon" id="tooltip' + item.username + '" onclick="ClickToolTip(\'' + item.username + '\')" data-toggle="tooltip" data-tooltip="ABC 123" data-tooltip-position="r" data-tooltip-class="tooltip-tree-style">i<div  id="tooltip1498055224298" class="tooltip tooltip-tree-style tooltip-r" style="position: absolute;display:none; width:150px; transform: translateZ(0px) translate3d(0px, 0px, 0px); opacity: 1; left: 23px; top: -51px;"><h5>typhuhn <span class="close-tooltip pull-right pointer visible-xs">X</span></h5><div><strong>Sponsored:</strong> thinhtyphu <br>          <strong>Package:</strong> Gold <br>          <strong>Left Point:</strong> 80 <br>          <strong>Right Point:</strong> 189680 <br>          <strong>Country:</strong> item.CountryCode <br>          <strong>Status:</strong> <span class="green">Active</span><br></div></div></span></span>';
            }
            else {
                return '<span class="inline-block relative"> <a  class=" product-6 user-packet"><span class="user-name"></span></a> ';
            }
        }
        function BuildNew(username) {
            debugger;
            $scope.promise = $http.get("/bizzApi/user/member/Tree/" + username)
             .success(function (data) {
                 //console.logog("tree", data);
                 if (data.IsSuccess) {
                     $scope.treeValues = data.Value.Trees;
                     _data = $scope.treeValues;
                     _currentUsername = $scope.currentUsername;
                     $('#divMemberTreeCtrl').show('slow');

                     $("#tree").nadyTree({
                         callType: 'obj',
                         structureObj: $scope.treeValues
                     });
                     //console.logog(data, data.Message);
                     $("#newTree").innerHTML = data.Message;
                     $scope.newTree = data.Message;
                     //$scope.buildTreeNew(data.Value.Trees);
                     triggerEvent();
                     setTreeStyle();
                 }
             });
        };
        function ClickToolTip(username) {
            alert(username);
        }
    }]); // end module

var showToolTip = 0, oldToolTip = 0;
function ClickToolTip(username) {
    console.log(username);
    $(".tooltip-tree-style").css("display", "none");
    $("#tooltip" + username).find("div").css("display", "");
    showToolTip++;
}
$(".network-tree-stucture").click(function () {
    console.log(showToolTip);
    if (showToolTip > 0) {

        $(".tooltip-tree-style").css("display", "none"); showToolTip = 0;
    }
});
function ShowDetail(item) {
    $(item).find("div").css("display", "");
}
function HiddenDetail() {
    $(".tooltip-tree-style").css("display", "none");
}
$(".tooltip-icon").hover(function () {
    $(this).find("div").css("display", "");
}, function () {
    $(".tooltip-tree-style").css("display", "none");
});
var backTree = function () {
    var backUsername = _previousUsername;
    if (backUsername) {
        rebuildTree(backUsername);
    }
};
var previousUsername = localStorage._sess_user.substr(1, localStorage._sess_user.length - 2);
var listParent = [];
function BuildNew(username) {

    if (listParent.length > 0) {
        $("#topOne").css("display", "block");
    }
    else {
        $("#topOne").css("display", "none");
    }
    $.getJSON("/bizzApi/user/member/Tree/" + username, function (data) {
        var dtaaa = { "Value": { "Trees": [{ "head": "admin", "id": 1, "username": "admin", "contents": "admin", "country": "Viet Nam", "totalleft": 70, "totalright": 24, "parent": "", "children": [{ "head": "newcoin2", "id": 19, "username": "newcoin2", "contents": "newcoin2", "country": "Viet Nam", "totalleft": 69, "totalright": 0, "parent": "admin", "children": [], "ParentId": 1, "LeftPosId": 21, "RightPosId": 0, "TotalPHDownside": 34.9, "Gender": false, "level": "C-Platinum" }, { "head": "newcoin2", "id": 19, "username": "newcoin2", "contents": "newcoin2", "country": "Viet Nam", "totalleft": 69, "totalright": 0, "parent": "admin", "children": [], "ParentId": 1, "LeftPosId": 21, "RightPosId": 0, "TotalPHDownside": 34.9, "Gender": false, "level": "C-Platinum" }, { "head": "daigiabitcoin", "id": 18, "username": "daigiabitcoin", "contents": "daigiabitcoin", "country": "Viet Nam", "totalleft": 4, "totalright": 19, "parent": "admin", "children": [], "ParentId": 1, "LeftPosId": 42, "RightPosId": 41, "TotalPHDownside": 4.74645, "Gender": false, "level": "C-King" }], "ParentId": 0, "LeftPosId": 19, "RightPosId": 18, "TotalPHDownside": 40.64645, "Gender": false, "level": "C-King" }], "PreviousLevel": null }, "IsSuccess": true, "Message": null, "PropertyErrors": [], "ResultCode": 0, "IsValid": true };
        buildTreeNew(dtaaa.Value.Trees);
    });
};
function BuildNewTree(username) {
    var item = $("#" + username).parent().parent().parent();
    var arrayTemp = [];
    if ($(item).attr("class") != "network-tree-stucture") {
        arrayTemp.push($($(item).children()[0]).find("a span").text());

        while (true) {
            var item = $("#" + arrayTemp[arrayTemp.length - 1]).parent().parent().parent();
            console.log(item);
            if ($(item).attr("class") != "network-tree-stucture") {
                arrayTemp.push($($(item).children()[0]).find("a span").text());
            }
            else {
                listParent = arrayTemp.concat(listParent);
                BuildNew(username);
                break;
            }
        }
    }
}
function Back() {
    BuildNew(listParent[0]);
    listParent.shift();
}
function Right() {
    $(".network-tree-stucture").html("");
    listParent = [];
    var username = localStorage._sess_user.substr(1, localStorage._sess_user.length - 2);
    var item = null;
    var Loop = function () {
        $.getJSON("/bizzApi/user/member/Tree/" + username, function (data) {
            if (data.Value.Trees[0].children == null || data.Value.Trees[0].children.length == 0) {

                BuildNew(data.Value.Trees[0].username);
                return;
            }
            else {
                debugger;
                var item = data.Value.Trees[0];
                listParent.unshift(item.username);
                if (item.children != null && item.children.length == 2) {
                    item = item.children[1];
                    username = item.username;
                    if (username != null && username != undefined) {
                        Loop();
                    } else {
                        BuildNew(listParent[0]);
                        listParent.shift();
                    }
                }
                else {
                    username = listParent[0];
                    console.log("ended ");
                    BuildNew(username);
                }
            }
        });
    }
    Loop();
}
function Top() {
    $(".network-tree-stucture").html("");
    listParent = [];
    BuildNew(localStorage._sess_user.substr(1, localStorage._sess_user.length - 2));
}
function Left() {

    $(".network-tree-stucture").html("");
    listParent = [];
    var username = localStorage._sess_user.substr(1, localStorage._sess_user.length - 2);
    var item = null;
    var Loop = function () {
        $.getJSON("/bizzApi/user/member/Tree/" + username, function (data) {
            if (data.Value.Trees[0].children == null || data.Value.Trees[0].children.length == 0) {

                BuildNew(data.Value.Trees[0].username);
                return;
            }
            else {
                debugger;
                var item = data.Value.Trees[0];
                listParent.unshift(item.username);
                if (item.children != null && item.children.length >= 1) {
                    item = item.children[0];
                    username = item.username;
                    if (username != null && username != undefined) {
                        Loop();
                    } else {
                        BuildNew(listParent[0]);
                        listParent.shift();
                    }
                }
                else {
                    username = listParent[0];
                    console.log("ended ");
                    BuildNew(username);
                }
            }
        });
    }
    Loop();
}
function BuildBackNew(username) {
    alert("back");
    $.getJSON("/bizzApi/user/member/BackTree/" + username, function (data) {
        buildTreeNew(data.Value.Trees);
    });
};
var LevelCode = ["C-M", "C-Silver", "C-Gold", "C-Daimond", "C-Platinum", "C-King"];
var LevelCodeClass = ["user-type-41", "user-type-3", "user-type-4", "user-type-5", "user-type-60", "status-22"];
var LevelCodeTitel = ["STAR", "PREMIER", "RUBY", "EMERALD", "DIAMOND", "BLUE DIAMOND"];
function getHTML(item) {
    if (LevelCode.indexOf(item.level) >= 0) {
        return '<span class="inline-block relative"> <a href="javascript:BuildNew(\'' + item.username + '\')" class="' + LevelCodeClass[LevelCode.indexOf(item.level)] + ' product-6 user-packet"><span class="user-name">' + item.username + '</span></a> <span class="tooltip-icon" id="tooltip' + item.username + '" onclick="ClickToolTip(\'' + item.username + '\')" onmouseover="ShowDetail(this)" onmouseout="HiddenDetail()" data-toggle="tooltip" data-tooltip="ABC 123" data-tooltip-position="r" data-tooltip-class="tooltip-tree-style">i<div  id="tooltip1498055224298" class="tooltip tooltip-tree-style tooltip-r" style="position: absolute;display:none; width:150px; transform: translateZ(0px) translate3d(0px, 0px, 0px); opacity: 1; left: 23px; top: -51px;"><h5>' + item.username + ' <span class="close-tooltip pull-right pointer visible-xs">X</span></h5><div><strong>Sponsored:</strong> ' + item.parent + ' <br>          <strong>Package:</strong> ' + LevelCodeTitel[LevelCode.indexOf(item.level)] + ' <br>          <strong>Left Point:</strong> ' + item.totalleft + ' <br>          <strong>Right Point:</strong> ' + item.totalright + '<br>          <strong>Country:</strong> ' + item.country + '<br></div></div></span></span>';
    }
    else {
        return '<span class="inline-block relative"> <a  class=" product-6 user-packet"><span class="user-name"></span></a> ';
    }

}
function buildTreeNew(data) {
    var aaa = "<ul><li>";
    aaa += getHTML(data[0]);
    aaa += NewBuild(data[0]);
    aaa += "</li></ul>";
    $(".network-tree-stucture").html(aaa);
};
BuildNew(localStorage._sess_user.substr(1, localStorage._sess_user.length - 2));
function NewBuild(input) {
    var strReturn = "";
    if (input.children != null && input.children.length > 0) {
        strReturn += "<ul>";
        input.children.forEach(function (item) {

            strReturn += "<li>";
            strReturn += getHTML(item);
            strReturn += NewBuild(item);
            strReturn += "</li>";
        });
        strReturn += "</ul>";
    }
    return strReturn;
}
var reInitTree = function () {
    rebuildTree(_initUsername);
    $('#divBackTree').css('display', 'none');
};

var previousLevelClick = function () {
    if (_initUsername != _currentUsername) {
        rebuildTree(_parentUserName);
    }
};

var rebuildTree = function (username) {
    if (username == null || username == "") {
        return;
    }

    $("div.zmrcntr").remove();
    $("#tree").html("");
    $('#divBackTree').css('display', 'block');

    _previousUsername = _currentUsername;
    _currentUsername = username;
    $('span#backUsername').html(_previousUsername);

    if (_initUsername == _currentUsername) {
        $('#btnBackToTop').css('display', 'none');
        $('#divBackTree').css('display', 'none');
    } else {
        $('#btnBackToTop').css('display', 'block');
    }

    $.getJSON("/bizzApi/user/member/Tree/" + username, function (data) {
        // //console.logog(data);
    })
          .done(function (data) {
              //console.logog(data);
              //----------------------
              _data = data.Value.Trees;

              var parentNode = data.Value.PreviousLevel;
              if (parentNode) {
                  _parentUserName = parentNode.head;
                  $('#previousTree').css('display', 'block');
              } else {
                  $('#previousTree').css('display', 'none');
              }

              if (_initUsername == _currentUsername) {
                  $('#previousTree').css('display', 'none');
              } else {
                  $('#previousTree').css('display', 'block');
              }

              $("#tree").nadyTree({
                  callType: 'obj',
                  structureObj: _data,
                  nodeDropComplete: function (event, data) {
                  }
              });

              triggerEvent();
              setTreeStyle();

          })
          .fail(function (err) {
              var status = err.status;
              var statusText = err.statusText;
              showServerError();
              ////console.logog(err);
          });

};