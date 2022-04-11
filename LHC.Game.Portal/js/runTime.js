
$(function () {
    //初始化彩种

    setInterval("handTimeRun()", 1000);

    //更新期号 每分钟更新一次
    //setInterval('updateIssue()', 5000);  //新一期开奖之后 更新内容



});

var payTypeState = "";



//时间跑完时做的操作
function timeOver() {

    CloseTan('bet');
    ClearSelectState();

    $('#fenpanTime').html('已封盘');
    $('#fengpan').show();
}

function updateIssue() {

    $.post("/Lottery/CheckLastIssue", { lType: lType }, function (data) {

        //当前开奖期号的span
        var currIssue = $('.kaijiang[lType=' + lType + ']').find('div:eq(0)').find('span:eq(0)');

        if (data != trimEnd(currIssue.html())) {

            if (lType % 2 == 0) {

                $.post('/Lottery/GetLastOpenReocrd2', { lType: lType }, function (data) {

                    if (parseInt(data.split('|')[3]) == 1) {
                        FillLastOpenNum(data);
                    }
                });
            }
            else {
                $.post('/Lottery/GetLastOpenReocrd', { lType: lType }, function (data) {

                    FillLastOpenNum(data);
                });
            }

        }


    });
}


//处理时间倒计时
function handTimeRun() {

    var d = new Date();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var sec = d.getSeconds();

    //停止投注
    if (lType == 1 && (hour == 1 && minute == 54 && sec == 30)) {
        location.reload();
    }


    //判断是否已经停止投注
    var nextIssue = $("#nextIssue").html();
    if (nextIssue == null) {
        startAgain();
        return;
    }

    ////重新开始投注
    //if (lType == 11) { //带小时的
    //    timeRun2();
    //}
    //else {
    //    timeRun();
    //}

    timeRun2();

}

//不带小时
function timeRun() {

    var d = new Date();

    var second = $("#second").html();

    if (second == undefined) {

        var sec2 = d.getSeconds();

        if (sec2 % 2 == 0) {            //2秒更新一次
            $.post('/Lottery/GetRemainingTime', { lType: lType }, function (data) {

                if (data != '已封盘' && data != '00&00&00') {
                    var arr3 = data.split('&');
                    $('#fenpanTime').html('<t id="minute">' + arr3[1] + '</t>:<t id="second">' + arr3[2] + '</t>');
                    $('#fengpan').hide();

                    $.post('/Lottery/GetCurrentIssue', { lType: lType }, function (data) {
                        $('#nextIssue').html(data + '期');
                    });
                }
            });
        }

        return;
    }

    //临时变量
    var sec = -1;
    var min = -1;
    //将显示秒数转换为数字
    if (second.substr(0, 1) == "0") {
        sec = parseInt(second.substr(1, 1));
    } else {
        sec = parseInt(second);
    }
    //秒数减一
    sec = sec - 1;

    //更新时间
    if (sec == 5 || sec == 15 || sec == 25 || sec == 35 || sec == 45 || sec == 55) {
        updateTime(lType);
    }


    if (sec < 10) {


        if (sec == -1) {
            //秒数用完了 取分钟数
            var minute = $("#minute").html();
            //将显示分钟数转化为数字
            if (minute.substr(0, 1) == "0") {
                min = parseInt(minute.substr(1, 1));
            } else {
                min = parseInt(minute);
            }

            //判断分钟数是否为0
            if (min - 1 > -1) {
                //还有剩余分钟
                if (min - 1 < 10) {
                    $("#minute").html("0" + (min - 1));
                }
                else {
                    $("#minute").html(min - 1);
                }
                $("#second").html(59);
            }
            else {
                //分钟数用完 重置时间  ---------------延迟30秒


                var hour = d.getHours();
                var yanchi = 0;

                if (lType == 1) {
                    if (hour >= 10 && hour < 22) {
                        yanchi = 60000;
                    } else {
                        yanchi = 30000;
                    }
                }
                else if (lType == 21) {
                    yanchi = 120000;
                }
                else if (lType == 9) {
                    yanchi = 120000;
                }
                else if (lType == 7 || lType == 9 || lType == 23 || lType % 2 == 0) {
                    yanchi = 30000;
                }
                else if (lType == 17 || lType == 15) {
                    yanchi = 90000;
                }
                else if (lType == 13) {
                    yanchi = 75000;
                }


                setTimeout('resetTime(' + lType + ')', yanchi);



                //时间用完后的处理
                timeOver();
            }
        }
        else {
            $("#second").html("0" + (second - 1));
        }
    }
    else {

        $("#second").html(second - 1);
    }
}

//带小时的处理
function timeRun2() {
    var second = $("#second").html();

    if (second == undefined) return;

    //临时变量
    var sec = -1;
    var min = -1;
    var hour = -1;
    //将显示秒数转换为数字
    if (second.substr(0, 1) == "0") {
        sec = parseInt(second.substr(1, 1));
    } else {
        sec = parseInt(second);
    }
    //秒数减一
    sec = sec - 1;

    //更新时间
    if (sec == 5 || sec == 35) {
        updateTime(lType);
    }

    if (sec < 10) {
        if (sec == -1) {
            //秒数用完了 取分钟数
            var minute = $("#minute").html();
            //将显示分钟数转化为数字
            if (minute.substr(0, 1) == "0") {
                min = parseInt(minute.substr(1, 1));
            } else {
                min = parseInt(minute);
            }

            //判断分钟数是否为0
            if (min - 1 > -1) {
                //还有剩余分钟
                if (min - 1 < 10) {
                    $("#minute").html("0" + (min - 1));
                } else {
                    $("#minute").html(min - 1);
                }
                $("#second").html(59);
            } else {
                //分钟数用完了 取小时数
                var hours = $("#hour").html();
                //将显示小时数转化为数字
                if (hours.substr(0, 1) == "0") {
                    hour = parseInt(hours.substr(1, 1));
                } else {
                    hour = parseInt(hours);
                }

                //判断小时数是否为0
                if (hour - 1 > -1) {
                    //还有剩余小时
                    if (hour - 1 < 10) {
                        $("#hour").html("0" + (hour - 1));
                    } else {
                        $("#hour").html(hour - 1);
                    }
                    $("#minute").html(59);
                    $("#second").html(59);
                }
                else {
                    //小时数用完
                    //分钟数用完 重置时间

                    var yanchi = 0;
                    if (lType == 11) {
                        yanchi = 900000;
                    }

                    setTimeout('resetTime2();', yanchi);   //封盘 15分钟 然后重置时间



                    //时间用完后的处理
                    timeOver();
                }
            }
        } else {
            $("#second").html("0" + (second - 1));
        }
    } else {
        $("#second").html(second - 1);
    }
}


//重置时间
function resetTime(lType) {

    //隐藏封单
    $('#fengpan').hide();

    //updateTime(lType);


    //改变下注期号
    var nextIssue = $("#nextIssue").html();
    $("#nextIssue").html(parseInt(nextIssue) + 1);


    var d = new Date();
    var hour = d.getHours();

    //分钟数
    if (lType == 1) {
        if (hour >= 9 && hour < 22) {
            $('#fenpanTime').html('<t id="minute">08</t>:<t id="second">59</t>');
        }
        else {
            $('#fenpanTime').html('<t id="minute">04</t>:<t id="second">29</t>');
        }
    }
    else if (lType == 21) {
        $('#fenpanTime').html('<t id="minute">07</t>:<t id="second">59</t>');
    }
    else if (lType == 9) {
        $('#fenpanTime').html('<t id="minute">02</t>:<t id="second">59</t>');
    }
    else if (lType == 7 || lType == 9) {
        $('#fenpanTime').html('<t id="minute">04</t>:<t id="second">29</t>');
    }
    else if (lType == 17 || lType == 15) {
        $('#fenpanTime').html('<t id="minute">08</t>:<t id="second">29</t>');
    }
    else if (lType == 13) {
        $('#fenpanTime').html('<t id="minute">08</t>:<t id="second">45</t>');
    }
    else if (lType == 62) {
        $('#fenpanTime').html('<t id="minute">02</t>:<t id="second">29</t>');
    }
    else if (lType % 2 == 0) {
        $('#fenpanTime').html('<t id="minute">00</t>:<t id="second">59</t>');
    }


}

function resetTime2() {

    if (lType == 11) {
        $("#hour").html('23');
        $("#minute").html('44');
        $("#second").html('59');
    }


}


//暂停下注之后到时间了重新启动
function startAgain() {
    var date = new Date();
    var hour = date.getHours();
    var minute = date.getminute();
    var sec = date.getsecond();

    //console.log(lType + "," + hour + "," + minute + "," + sec);

    if (lType == 1 && hour == 10 && minute == 0 && sec == 0) {
        location.reload();
    }
    else if ((lType == 1 || lType == 5 || lType == 6 || (lType >= 7 && lType < 12)) && hour == 8) {
        location.reload();
    }
    else if (lType == 2 && hour == 9) {
        location.reload();
    }
    else if (lType == 3 && hour == 10) {
        location.reload();
    }
    else if (lType == 4 && hour == 10) {
        location.reload();
    }
}





//处理期号最后几位的长度
function handIssueLength(issueCount, lType) {
    if (lType < 4 || (lType >= 12 && lType < 14)) {
        if (issueCount.length == 1) {
            return "00" + issueCount;
        }
        else if (issueCount.length == 2) {
            return "0" + issueCount;
        }
        else {
            return issueCount;
        }
    }
    else if (lType == 14) {
        if (issueCount.length == 1) {
            return "000" + issueCount;
        }
        else if (issueCount.length == 2) {
            return "00" + issueCount;
        }
        else if (issueCount.length == 3) {
            return "0" + issueCount;
        }
        else {
            return issueCount;
        }
    }
    else {
        if (issueCount.length == 1) {
            return "0" + issueCount;
        }
        else {
            return issueCount;
        }
    }

}




//5s和35s的时候更新时间
function updateTime() {

    //InitIssueAndTime();           ---4-11
}
