

//alert(play_特码B);


var lType = -1;

var bigPlayName = '特码';
var smallPlayName = '特码A';

var betContent = '';

var betCount = 1;

var isBigPlayClick = false;
var danMaCount = 0;               //是否有胆码

var thisYearSX = '虎';

var money = 0;


var weijiemingxi1 = '';

$(function () {

    lType = $('#lType').val();  //赋初始值


    //alert(lType);

    setInterval('StartOrEnd()', 5000);          //控制封盘或者开盘 显示赔率


    BindBigitleClick();             //大玩法

    Bindlh_subtitleClick();         //小玩法

    Init();


    BindopenResultClick();          //开奖结果

    Binddialog_closeClick();        //弹窗关闭

    BindruleClick();                //玩法规则

    BindmodifyPwdClick();           //修改密码

    BindresetClick();               //重置

    BindconfirmClick();             //修改密码确认

    BindcaiSelectClick();           //彩种选择

    BindloginOutClick();            //退出

    BindgsitemClick();              //玩法块

    BindrightBtnClick();            //快速投注和长龙

    BindplayInfoClick();                //个人信息

    BinddialogcloseClick();             //确认订单弹窗 取消按钮

    BindbetClick();                     //投注

    BindmoneyKeyUp();                   //金额输入框

    BindbetResetClick();                //选号重置

    BindmoneySelClick();                //筹码选择

    BindRightNumSelCLick();             //快速下注 号码选择

    Bindresetright1CLick();             //右边 重置

    Bindsubmitright1Click();            //右边确认


    BindRightNumTdClick();                 //右侧号码选择

    BindconfirmBetClick();              //确认投注

    BindelradioClick();                     //单式/复式/胆拖

    BindelcheckboxClcik();                      //六肖 家禽 野兽选择

    BindnotOpenDetailCLick();          //未结明细

    BindweijieDetailCLick();            //未结明细-第二层


    BindwjmxBackCLick();                //未结明细-第二层     返回按钮

    BindFundDetailCLick();              //资金明细

    BindtodaySettlementCLick();         //今日结算

    BindhistoryReportCLick();           //历史报表

    BindsettlementDetailCLick();                //今日结算详情

    //setInterval('LoadLastBet();', 3000);

});


function Init() {

    //二级玩法 默认显示第一个
    $('.lh_subtitle_container').eq(0).css('display', 'flex');

    //alert($('.lh_subtitle_container').eq(0).find('.lh_subtitle:eq(0)'));

    //$('.lh_subtitle_container').eq(0).find('.lh_subtitle:eq(0)').remove();

    //$('.lh_subtitle_container').eq(0).find('.lh_subtitle:eq(0)').click();

    $('#tab li:eq(0)').click();

    //开奖数据
    $.post('/lottery/OpenData', { lType: lType }, function (data) {

        var last = data[0];

        //最新一期 数据
        $('#lTypeName').html(last.ShowTypeName);

        $('#currentIssue').html(last.Issue);

        $('#currentNum').empty();

        var arr = last.ShowOpenNumInfo.split(',');

        for (var i = 0; i < arr.length; i++) {

            var arr2 = arr[i].split('-');

            if (i == 6) {
                $('#currentNum').append('<li data-v-c60dea02="" data-v-2db11f03="" class="item lh_item" style="width: 60px;"><span data-v-c60dea02="" data-v-2db11f03="">+</span> <span data-v-c60dea02="" data-v-2db11f03="" class="number number-' + arr2[2] + '">' + arr2[0] + '</span> <span data-v-c60dea02="" data-v-2db11f03="">' + arr2[1] + '</span></li>');
            }
            else {
                $('#currentNum').append('<li data-v-c60dea02="" data-v-2db11f03="" class="item lh_item"><span data-v-c60dea02="" data-v-2db11f03="" class="number number-' + arr2[2] + '">' + arr2[0] + '</span> <span data-v-c60dea02="" data-v-2db11f03="">' + arr2[1] + '</span></li>');
            }
        }




    });



    //期号 时间
    $.post('/lottery/GetCurrentIssueAndTime', { lType: lType }, function (data) {

        if (data != '已封盘') {

            $('.c_time').eq(0).hide();
            $('.c_time').eq(1).show();


            var arr = data.split('|');


            //1.
            $('#nextIssue').html(arr[0]);


            //2.2时:7分:22秒
            var arr2 = arr[1].split('&');
            var fengpantime = '<i id="hour">' + parseInt(arr2[0]) + '</i>时:<i id="minute">' + parseInt(arr2[1]) + '</i>分:<i id="second">' + parseInt(arr2[2]) + '</i>秒';
            $('#fenpanTime').html(fengpantime);

            //3.
            var arr3 = arr[2].split('&');
            $('#openTime').html('<span data-v-eb4ccf6e="" class="tx"><i id="second2">' + parseInt(arr3[2]) + '</i>秒</span><span data-v-eb4ccf6e="" class="tx"><i id="minute2">' + parseInt(arr3[1]) + '</i>分:</span><span data-v-eb4ccf6e="" class="tx"><i id="hour2">' + parseInt(arr3[0]) + '</i>时:</span><span data-v-eb4ccf6e="" class="tx" style="color: black;">&nbsp;距开奖:</span>');

        }

    })


}





function BindsettlementDetailCLick() {
    $('#settlementDetail').live('click', function () {

        $.post('/record/TodaySettlementDetail', { lType: lType }, function (data) {
            $('#messageBox').html(data).show();
        })
    })
}


function BindtodaySettlementCLick() {
    $('#todaySettlement').click(function () {

        $.post('/record/todaySettlement', { lType: lType }, function (data) {
            $('#messageBox').html(data).show();
        })
    })
}

function BindhistoryReportCLick() {
    $('#historyReport').click(function () {

        $.post('/record/historyReport', function (data) {
            $('#messageBox').html(data).show();
        })
    })
}




function BindFundDetailCLick() {
    $('#fundDetail').click(function () {

        $.post('/record/fundDetail', function (data) {
            $('#messageBox').html(data).show();
        })
    })
}




function BindwjmxBackCLick() {
    $('#wjmxBack').live('click', function () {

        $('#messageBox').html(weijiemingxi1);

    })
}


function BindweijieDetailCLick() {
    $('.weijieDetail').live('click', function () {

        var cid = $(this).attr('cid');

        weijiemingxi1 = $('#messageBox').html();  //保留第一层数据

        $.post('/lottery/NotOpenRecordDetail', { lType: cid }, function (data) {

            $('#messageBox').html(data);
        })
    })
}


function BindnotOpenDetailCLick() {
    $('#notOpenDetail').click(function () {

        $.post('/lottery/NotOpenRecord', { lType: lType }, function (data) {
            $('#messageBox').html(data).show();
        })
    })
}

function LoadLastBet() {

    $('.current-list li').remove();

    $.post('/lottery/LastBetRecord', { lType: lType }, function (data) {
        $('.current-list').append(data);
    })
}


function BindelcheckboxClcik() {

    $('.el-checkbox:visible').live('click', function (e) {

        e.stopPropagation();
        e.preventDefault();


        $(this).toggleClass('is-checked');
        $(this).find('.el-checkbox__input').toggleClass('is-checked');


        var index = $('.el-checkbox:visible').index($(this));


        if (index == 0) {
            $('.g_s-item i').eq(1).toggleClass('active');
            $('.g_s-item i').eq(6).toggleClass('active');
            $('.g_s-item i').eq(7).toggleClass('active');
            $('.g_s-item i').eq(9).toggleClass('active');
            $('.g_s-item i').eq(10).toggleClass('active');
            $('.g_s-item i').eq(11).toggleClass('active');
        }
        else {
            $('.g_s-item i').eq(0).toggleClass('active');
            $('.g_s-item i').eq(2).toggleClass('active');
            $('.g_s-item i').eq(3).toggleClass('active');
            $('.g_s-item i').eq(4).toggleClass('active');
            $('.g_s-item i').eq(5).toggleClass('active');
            $('.g_s-item i').eq(8).toggleClass('active');

        }



    });


}



function BindelradioClick() {

    $('.el-radio').live('click', function () {

        //改变样式
        $(this).find('.el-radio__input').addClass('is-checked');

        $(this).siblings().find('.el-radio__input').removeClass('is-checked');



        //清除已选择
        $('.g_s-item i').filter('.active').removeClass('active');
        $('.g_dantuo_img:visible').remove();

        //判断是不是选的胆拖

        if ($('.el-radio-group:visible .el-radio').index($(this)) == 1) {

            danMaCount = 0;
        }


    });


}

//确认投注
function BindconfirmBetClick() {
    $('.confirmBet').click(function () {

        $.post('/betting/BetNormal', { lType: lType, bigPlayName: bigPlayName, smallPlayName: smallPlayName, betInfo: betContent, betCount: betCount, money: money }, function (data) {

            if (data != 'ok') {
                ShowTanMin(data);
            }
            else {

                $('#money').val('');
                $('.g_s-item input').val('').removeClass('active');
                $('#confirmOrderDialog,#confirmOrderDialog2').hide();


                ShowTanMin('恭喜您，下注成功！');

                LoadLastBet();

            }
        });
    })
}


function BindRightNumTdClick() {
    $('#right_num tr:lt(10) td').click(function () {
        $(this).toggleClass('number-active');
    })
}

//右边确认
function Bindsubmitright1Click() {
    $('#submit_right1').click(function () {


        betContent = '';

        if (bigPlayName != '特码' && bigPlayName != '正码' && bigPlayName != '正特') {

            ShowTanMin2('此玩法不支持快速投注');
            return;
        }




        money = $('#right_amount').val();

        if (money == '') {
            ShowTanMin2('下注错误，未输入金额');
            return;
        }

        var pl = 42.35;

        //获取赔率
        $.post('/lottery/GetPeilv', { lType: lType, bigType: bigPlayName, smallType: smallPlayName }, function (data) {

            pl = data;

            //异步处理
            $('#right_num .number-active b').each(function () {

                betContent += $(this).html() + '-';
                betContent += pl + '-' + money + '|';
            })


            if (betContent == '') {
                ShowTanMin2('请至少选择一个号码');
                return;
            }

            betContent = trimEnd(betContent);

            //alert(betContent);

            HandOrderDetail(betContent, money);

            //清空选择
            $('#right_num tr:lt(10) td').removeClass('number-active');
            $('#right_amount').val('');



        });



    })


    $('#submit_right2').click(function () {

        betContent = '';

        if (bigPlayName != '特码' && bigPlayName != '正码' && bigPlayName != '正特') {

            ShowTanMin2('此玩法不支持快速投注');
            return;
        }

        money = $('#right_amount').val();

        if (money == '') {
            ShowTanMin2('下注错误，未输入金额');
            return;
        }


        var con = $('textarea').val();

        if (con == '') {
            ShowTanMin2('请至少输入一个号码');
            return;
        }


        var pl = 42.35;


        //获取赔率
        $.post('/lottery/GetPeilv', { lType: lType, bigType: bigPlayName, smallType: smallPlayName }, function (data) {

            pl = data;


            var arr = con.split(' ');

            var num = 0;

            for (var i = 0; i < arr.length; i++) {

                num = parseInt(arr[i]);

                if (isNaN(num) || num < 1 || num > 49 || arr[i].length != 2) {
                    continue;
                }

                betContent += arr[i] + '-';
                betContent += pl + '-' + money + '|';
            }


            if (betContent == '') {
                ShowTanMin2('投注内容或订单金额有误');
                return;
            }

            betContent = trimEnd(betContent);

            //alert(betContent);

            HandOrderDetail(betContent, money);

            //清空选择
            $('textarea').val('');
            $('#right_amount').val('');

        });



    })
}



//快速下注 号码选择
function BindRightNumSelCLick() {
    $('.first_tr b').click(function () {

        var txt = $(this).html().trim();

        //alert(txt);
        var num = 0;
        var num2 = 0;
        var a = 0;
        var b = 0;
        var red = "01，02，07，08，12，13，18，19，23，24，29，30，34，35，40，45，46";
        var blue = "03，04，09，10，14，15，20，25，26，31，36，37，41，42，47，48";
        var green = "05，06，11，16，17，21，22，27，28，32，33，38，39，43，44，49";


        if (txt == '单') {

            $('#right_num tr:even td').addClass('number-active');
        }
        else if (txt == '双') {

            $('#right_num tr:odd td').addClass('number-active');
        }
        else if (txt == '大') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '小') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '合双') {

            $('#right_num b').each(function () {
                num = $(this).html();

                num = parseInt(num.substr(0, 1)) + parseInt(num.substr(1, 1));

                if (num % 2 == 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '合单') {

            $('#right_num b').each(function () {
                num = $(this).html();

                num = parseInt(num.substr(0, 1)) + parseInt(num.substr(1, 1));

                if (num % 2 != 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '大单') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num % 2 != 0 && num >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '大双') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num % 2 == 0 && num >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '小单') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num % 2 != 0 && num <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '小双') {

            $('#right_num b').each(function () {
                num = parseInt($(this).html());

                if (num % 2 == 0 && num <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '红波') {

            $('#right_num b').each(function () {
                num = $(this).html();

                if (red.indexOf(num) != -1) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '红单') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (red.indexOf(num) != -1 && num2 % 2 != 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '红双') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (red.indexOf(num) != -1 && num2 % 2 == 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '红大') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (red.indexOf(num) != -1 && num2 >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '红小') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (red.indexOf(num) != -1 && num2 <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '蓝波') {

            $('#right_num b').each(function () {
                num = $(this).html();

                if (blue.indexOf(num) != -1) {
                    $(this).parent().addClass('number-active');
                }
            })
        }

        else if (txt == '绿波') {

            $('#right_num b').each(function () {
                num = $(this).html();

                if (green.indexOf(num) != -1) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '蓝单') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (blue.indexOf(num) != -1 && num2 % 2 != 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '蓝双') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (blue.indexOf(num) != -1 && num2 % 2 == 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '蓝大') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (blue.indexOf(num) != -1 && num2 >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '蓝小') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (blue.indexOf(num) != -1 && num2 <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '绿单') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (green.indexOf(num) != -1 && num2 % 2 != 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '绿双') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (green.indexOf(num) != -1 && num2 % 2 == 0) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '绿大') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (green.indexOf(num) != -1 && num2 >= 25) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if (txt == '绿小') {

            $('#right_num b').each(function () {
                num = $(this).html();
                num2 = parseInt(num);

                if (green.indexOf(num) != -1 && num2 <= 24) {
                    $(this).parent().addClass('number-active');
                }
            })
        }
        else if ("鼠 牛 虎 兔 龙 蛇 马 羊 猴 鸡 狗 猪".indexOf(txt) != -1) {


            var sx = '';

            if (txt == "龙") {
                sx = "11,23,35,47";
            }
            else if (txt == "蛇") {
                sx = "10,22,34,46";
            }
            else if (txt == "马") {
                sx = "09,21,33,45";
            }
            else if (txt == "羊") {
                sx = "08,20,32,44";
            }
            else if (txt == "猴") {
                sx = "07,19,31,43";
            }
            else if (txt == "鸡") {
                sx = "06,18,30,42";
            }
            else if (txt == "狗") {
                sx = "05,17,29,41";
            }
            else if (txt == "猪") {
                sx = "04,16,28,40";
            }
            else if (txt == "鼠") {
                sx = "03 15 27 39";
            }
            else if (txt == "牛") {
                sx = "02,14,26,38";
            }
            else if (txt == "虎") {
                sx = "01,13,25,37,49";
            }
            else if (txt == "兔") {
                sx = "12,24,36,48";
            }

            $('#right_num b').each(function () {
                num = $(this).html();

                if (sx.indexOf(num) != -1) {
                    $(this).parent().addClass('number-active');
                }
            })


        }

    });
}

//右边 重置
function Bindresetright1CLick() {
    $('#reset_right1').click(function () {

        $('#right_num tr td').removeClass('number-active');

        $('#right_amount').val('');

    });

    $('#reset_right2').click(function () {

        $('textarea').val('');
        $('#right_amount').val('');

    });
}





function BindmoneySelClick() {
    $('#moneySel li').click(function () {

        var m = parseInt($(this).attr('data-chip'));
        var m2 = $('#money').val();

        if (m2 != '') {
            m2 = parseInt(m2);
        }

        var total = m + m2;

        $('#money').val(total);

        //给选中的input 填充金额
        $('.g_s-item:visible').each(function () {

            var input = $(this).find('input');
            if (input.hasClass('active')) {

                input.val(total);
            }
        })


    });
}


//选号重置
function BindbetResetClick() {
    $('#betReset').click(function () {

        $('#money').val('');

        $('.g_s-item:visible').find('input').removeClass('active');

        $('.g_s-item:visible').find('input').val('');


        $('.g_s-item:visible').find('i').removeClass('active');

        $('.g_dantuo_img:visible').remove();            //胆码

        //六肖、
        $('.el-checkbox,.el-checkbox__input').removeClass('is-checked');

        danMaCount = 0;

    })
}

//投注
function BindbetClick() {
    $('#bet').click(function () {

        betContent = '';

        money = $('#money').val();

        if (money == '') {
            ShowTanMin('请填写金额');
            return;
        }


        if (contains(bigPlayName, '连') || bigPlayName == '合肖' || bigPlayName == '特平中' || bigPlayName == '中一' || bigPlayName == '自选不中') {

            var txt = $('.el-radio__input').filter('.is-checked').next().html();


            if (typeof (txt) != 'undefined' && contains(txt, '胆拖')) {         //胆拖玩法

                //胆拖玩法

                //1.找胆

                $('.g_dantuo_img:visible').each(function () {

                    betContent += $(this).parents('.g_s-item').find('span:eq(1)').html() + ',';
                })


                betContent = trimEnd(betContent) + '拖';


                $('i:visible').filter('.active').each(function () {


                    if ($(this).parents('.g_s-item').find('.g_dantuo_img').size() == 0) {

                        betContent += $(this).parents('.g_s-item').find('span:eq(0)').html() + ',';
                    }

                })



            }
            else {
                //  常规玩法
                $('i:visible').filter('.active').each(function () {

                    betContent += $(this).parents('.g_s-item').find('span:eq(0)').html() + ',';
                })
            }

            betContent = trimEnd(betContent);


            if (betContent == '') {
                ShowTanMin('未选中订单或订单金额有误');
                return;
            }

            HandOrderDetail2(betContent, money);



        }
        else {

            //收集投注信息
            $('input:visible').filter('.active').each(function () {

                if (bigPlayName == '特肖首尾' || bigPlayName == '一肖尾数' || bigPlayName == '半波') {
                    betContent += $(this).parents('.g_s-item').find('div:eq(0)').find('span').html() + '-';
                    betContent += $(this).parents('.g_s-item').find('div:eq(2)').html() + '-' + money + '|';
                }
                else {
                    betContent += $(this).parents('.g_s-item').find('div:eq(0)').find('span').html() + '-';
                    betContent += $(this).parents('.g_s-item').find('div:eq(1)').html() + '-' + money + '|';
                }

            })


            betContent = trimEnd(betContent);


            if (betContent == '') {
                ShowTanMin('未选中订单或订单金额有误');
                return;
            }

            HandOrderDetail(betContent, money);


        }






    })
}

//普通
function HandOrderDetail(betContent, money) {

    var arr = betContent.split('|');

    $('#orderDetail tr:gt(0)').remove();

    for (var i = 0; i < arr.length; i++) {
        var arr2 = arr[i].split('-');

        $('#orderDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + smallPlayName + ' ' + arr2[0] + '</span></td><td data-v-c187c242="">' + arr2[1] + '</td><td data-v-c187c242="">' + arr2[2] + '</td><td data-v-c187c242=""><input data-v-a475f800="" data-v-c187c242="" type="checkbox" checked="checked" value="2550"></td></tr>');
    }

    $('.betCount').html(arr.length);
    $('.betMoney').html(arr.length * money);

    $('#confirmOrderDialog').show();
}

//组合
function HandOrderDetail2(betContent, money) {

    $('#numSeled').html(betContent + '；组合如下：');


    var peilv = 0;

    $.post('/lottery/GetPeilv', { lType: lType, bigType: bigPlayName, smallType: smallPlayName }, function (data) {
        peilv = data;

        //alert(peilv);

        var arr = peilv.split('/');


        $('#zuheDetail').find('tr:gt(0)').remove();

        var labelTxt = $('.el-radio__input').filter('.is-checked').next().html();

        if (smallPlayName == '二肖连中' || smallPlayName == '二肖连不中' || smallPlayName == '二尾连中' || smallPlayName == '二尾连不中' || smallPlayName == '二全中' || smallPlayName == '二中特' || smallPlayName == '二中特X' || smallPlayName == '特串') {

            //#region 



            if (contains(labelTxt, '拖')) {

                var arr2 = betContent.split('拖');

                var len = arr2.length;


                if (len < 2) {
                    ShowTanMin('最少选择两个号码');
                    return;
                }

                var arr3 = arr2[1].split(',');

                betCount = arr3.length;


                for (var i = 0; i < arr3.length; i++) {

                    var t = arr2[0] + ',' + arr3[i];

                    if (smallPlayName == '二全中' || smallPlayName == '特串') {

                    }
                    else if (smallPlayName == '二中特' || smallPlayName == '二中特X') {
                        peilv = arr[0];

                    }
                    else if (contains(smallPlayName, '尾连')) {

                        if (contains(t, '0尾')) {
                            peilv = arr[1];
                        }
                        else {
                            peilv = arr[0];
                        }
                    }
                    else {
                        if (contains(t, thisYearSX)) {
                            peilv = arr[0];
                        }
                        else {
                            peilv = arr[1];
                        }
                    }


                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                }

            }
            else {

                var arr2 = betContent.split(',');

                var len = arr2.length;


                if (len < 2) {
                    ShowTanMin('最少选择两个号码');
                    return;
                }

                betCount = JieCheng(len) / (JieCheng(2) * JieCheng(len - 2));

                for (var i = 0; i < arr2.length; i++) {
                    for (var j = i + 1; j < arr2.length; j++) {

                        var t = arr2[i] + ',' + arr2[j];

                        if (smallPlayName == '二全中' || smallPlayName == '特串') {

                        }
                        else if (smallPlayName == '二中特' || smallPlayName == '二中特X') {
                            peilv = arr[0];

                        }
                        else if (contains(smallPlayName, '尾连')) {

                            if (contains(t, '0尾')) {
                                peilv = arr[1];
                            }
                            else {
                                peilv = arr[0];
                            }
                        }
                        else {
                            if (contains(t, thisYearSX)) {
                                peilv = arr[0];
                            }
                            else {
                                peilv = arr[1];
                            }

                        }



                        $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '三肖连中' || smallPlayName == '三肖连不中' || smallPlayName == '三尾连中' || smallPlayName == '三尾连不中' || smallPlayName == '三全中' || smallPlayName == '三中二' || smallPlayName == '三中二X') {

            //#region 


            if (contains(labelTxt, '拖')) {

                var arr2 = betContent.split('拖');

                console.log(arr2);

                var len = arr2.length;

                if (arr2[0].length < 3) {
                    ShowTanMin('最少选择2个胆码');
                    return;
                }
                else if (arr2.length != 2) {
                    ShowTanMin('最少选择3个号码');
                    return;
                }

                var arr3 = arr2[1].split(',');

                betCount = arr3.length;


                for (var i = 0; i < arr3.length; i++) {

                    var t = arr2[0] + ',' + arr3[i];

                    if (smallPlayName == '三全中') {

                    }
                    else if (smallPlayName == '三中二' || smallPlayName == '三中二X') {
                        peilv = arr[0];

                    }
                    else if (contains(smallPlayName, '尾连')) {

                        if (contains(t, '0尾')) {
                            peilv = arr[1];
                        }
                        else {
                            peilv = arr[0];
                        }
                    }
                    else if (contains(t, thisYearSX)) {
                        peilv = arr[0];
                    }
                    else {
                        peilv = arr[1];
                    }

                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                }

            }
            else {

                var arr2 = betContent.split(',');

                var len = arr2.length;


                if (len < 3) {
                    ShowTanMin('最少选择3个号码');
                    return;
                }

                betCount = JieCheng(len) / (JieCheng(3) * JieCheng(len - 3));

                for (var i = 0; i < arr2.length; i++) {
                    for (var j = i + 1; j < arr2.length; j++) {

                        for (var k = j + 1; k < arr2.length; k++) {


                            var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k];


                            if (smallPlayName == '三全中') {

                            }
                            else if (smallPlayName == '三中二' || smallPlayName == '三中二X') {
                                peilv = arr[0];

                            }
                            else if (contains(smallPlayName, '尾连')) {

                                if (contains(t, '0尾')) {
                                    peilv = arr[1];
                                }
                                else {
                                    peilv = arr[0];
                                }
                            }
                            else if (contains(t, thisYearSX)) {
                                peilv = arr[0];
                            }
                            else {
                                peilv = arr[1];
                            }

                            $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');


                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '四肖连中' || smallPlayName == '四肖连不中' || smallPlayName == '四尾连中' || smallPlayName == '四尾连不中') {

            //#region 


            if (contains(labelTxt, '拖')) {

                var arr2 = betContent.split('拖');

                var len = arr2.length;

                if (arr2[0].length < 5) {
                    ShowTanMin('最少选择3个胆码');
                    return;
                }
                else if (arr2.length != 2) {
                    ShowTanMin('最少选择4个号码');
                    return;
                }

                var arr3 = arr2[1].split(',');

                betCount = arr3.length;


                for (var i = 0; i < arr3.length; i++) {

                    var t = arr2[0] + ',' + arr3[i];


                    if (contains(smallPlayName, '尾连')) {

                        if (contains(t, '0尾')) {
                            peilv = arr[1];
                        }
                        else {
                            peilv = arr[0];
                        }
                    }
                    else if (contains(t, thisYearSX)) {
                        peilv = arr[0];
                    }
                    else {
                        peilv = arr[1];
                    }

                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                }

            }
            else {

                var arr2 = betContent.split(',');

                var len = arr2.length;


                if (len < 4) {
                    ShowTanMin('最少选择4个号码');
                    return;
                }

                betCount = JieCheng(len) / (JieCheng(4) * JieCheng(len - 4));

                for (var i = 0; i < arr2.length; i++) {
                    for (var j = i + 1; j < arr2.length; j++) {

                        for (var k = j + 1; k < arr2.length; k++) {

                            for (var m = k + 1; m < arr2.length; m++) {


                                var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m];

                                if (contains(smallPlayName, '尾连')) {

                                    if (contains(t, '0尾')) {
                                        peilv = arr[1];
                                    }
                                    else {
                                        peilv = arr[0];
                                    }
                                }
                                else if (contains(t, thisYearSX)) {
                                    peilv = arr[0];
                                }
                                else {
                                    peilv = arr[1];
                                }

                                $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');



                            }



                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '五肖连中' || smallPlayName == '五肖连不中' || smallPlayName == '五尾连中' || smallPlayName == '五尾连不中') {

            //#region 


            if (contains(labelTxt, '拖')) {

                var arr2 = betContent.split('拖');

                var len = arr2.length;

                if (arr2[0].length < 7) {
                    ShowTanMin('最少选择4个胆码');
                    return;
                }
                else if (arr2.length != 2) {
                    ShowTanMin('最少选择5个号码');
                    return;
                }

                var arr3 = arr2[1].split(',');

                betCount = arr3.length;


                for (var i = 0; i < arr3.length; i++) {

                    var t = arr2[0] + ',' + arr3[i];


                    if (contains(smallPlayName, '尾连')) {

                        if (contains(t, '0尾')) {
                            peilv = arr[1];
                        }
                        else {
                            peilv = arr[0];
                        }
                    }
                    else if (contains(t, thisYearSX)) {
                        peilv = arr[0];
                    }
                    else {
                        peilv = arr[1];
                    }

                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                }

            }
            else {

                var arr2 = betContent.split(',');

                var len = arr2.length;


                if (len < 5) {
                    ShowTanMin('最少选择5个号码');
                    return;
                }

                betCount = JieCheng(len) / (JieCheng(5) * JieCheng(len - 5));

                for (var i = 0; i < arr2.length; i++) {
                    for (var j = i + 1; j < arr2.length; j++) {

                        for (var k = j + 1; k < arr2.length; k++) {

                            for (var m = k + 1; m < arr2.length; m++) {

                                for (var n = m + 1; n < arr2.length; n++) {

                                    var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n];


                                    if (contains(smallPlayName, '尾连')) {

                                        if (contains(t, '0尾')) {
                                            peilv = arr[1];
                                        }
                                        else {
                                            peilv = arr[0];
                                        }
                                    }
                                    else if (contains(t, thisYearSX)) {
                                        peilv = arr[0];
                                    }
                                    else {
                                        peilv = arr[1];
                                    }

                                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                                }




                            }



                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '六肖') {

            //#region 


            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 6) {
                ShowTanMin('请最少选择6个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(6) * JieCheng(len - 6));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var p = n + 1; p < arr2.length; p++) {


                                    var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[p];


                                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');


                                }


                            }




                        }



                    }



                }


            }


            //#endregion

        }
        else if (smallPlayName == '一粒任中') {

            //#region 


            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 1) {
                ShowTanMin('请选择号码');
                return;
            }

            betCount = len;

            for (var i = 0; i < arr2.length; i++) {

                $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + arr2[i] + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

            }


            //#endregion

        }
        else if (smallPlayName == '五中一' || smallPlayName == '五不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 5) {
                ShowTanMin('最少选择5个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(5) * JieCheng(len - 5));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n];

                                $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                            }




                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '六中一' || smallPlayName == '六不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 6) {
                ShowTanMin('最少选择6个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(6) * JieCheng(len - 6));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var o = n + 1; o < arr2.length; o++) {
                                    var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[o];

                                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                                }


                            }




                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '七中一' || smallPlayName == '七不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 7) {
                ShowTanMin('最少选择7个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(7) * JieCheng(len - 7));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var o = n + 1; o < arr2.length; o++) {

                                    for (var p = o + 1; p < arr2.length; p++) {

                                        var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[o] + ',' + arr2[p];

                                        $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                                    }


                                }


                            }




                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '八中一' || smallPlayName == '八不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 8) {
                ShowTanMin('最少选择8个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(8) * JieCheng(len - 8));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var o = n + 1; o < arr2.length; o++) {

                                    for (var p = o + 1; p < arr2.length; p++) {

                                        for (var q = p + 1; q < arr2.length; q++) {
                                            var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[o] + ',' + arr2[p] + ',' + arr2[q];

                                            $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');

                                        }


                                    }


                                }


                            }




                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '九中一' || smallPlayName == '九不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 9) {
                ShowTanMin('最少选择9个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(9) * JieCheng(len - 9));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var o = n + 1; o < arr2.length; o++) {

                                    for (var p = o + 1; p < arr2.length; p++) {

                                        for (var q = p + 1; q < arr2.length; q++) {
                                            for (var r = q + 1; r < arr2.length; r++) {
                                                var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[o] + ',' + arr2[p] + ',' + arr2[q] + ',' + arr2[r];

                                                $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');


                                            }



                                        }


                                    }


                                }


                            }




                        }


                    }
                }


            }


            //#endregion

        }
        else if (smallPlayName == '十中一' || smallPlayName == '十不中') {

            //#region 

            var arr2 = betContent.split(',');

            var len = arr2.length;


            if (len < 10) {
                ShowTanMin('最少选择10个号码');
                return;
            }

            betCount = JieCheng(len) / (JieCheng(10) * JieCheng(len - 10));

            for (var i = 0; i < arr2.length; i++) {
                for (var j = i + 1; j < arr2.length; j++) {

                    for (var k = j + 1; k < arr2.length; k++) {

                        for (var m = k + 1; m < arr2.length; m++) {

                            for (var n = m + 1; n < arr2.length; n++) {

                                for (var o = n + 1; o < arr2.length; o++) {

                                    for (var p = o + 1; p < arr2.length; p++) {

                                        for (var q = p + 1; q < arr2.length; q++) {

                                            for (var r = q + 1; r < arr2.length; r++) {

                                                for (var s = r + 1; s < arr2.length; s++) {

                                                    var t = arr2[i] + ',' + arr2[j] + ',' + arr2[k] + ',' + arr2[m] + ',' + arr2[n] + ',' + arr2[o] + ',' + arr2[p] + ',' + arr2[q] + ',' + arr2[r] + ',' + arr2[s];

                                                    $('#zuheDetail').append('<tr data-v-c187c242=""><td data-v-c187c242=""><span data-v-a475f800="" data-v-c187c242="">' + t + '</span></td><td data-v-c187c242="">' + peilv + '</td><td data-v-c187c242="">' + money + '</td></tr>');


                                                }



                                            }



                                        }


                                    }


                                }


                            }




                        }


                    }
                }


            }


            //#endregion

        }

        $('.betCount').html(betCount);
        $('.betMoney').html(betCount * money);

        $('#confirmOrderDialog2').show();


    })

}


function BindmoneyKeyUp() {
    $('#money,#right_amount').keyup(function (e) {


        //alert(e.srcElement.id);

        money = $(this).val();


        if (money == '') {
            ShowTanMin('未选中订单或订单金额有误');
            return;
        }
        else if (!/^\+?[1-9][0-9]*$/.test(money)) {          //判断是否是正整数

            ShowTanMin('金额不能为非数字');

            $('.s_item-auto input:visible').val('');
            $(this).val('');
            return;
        }
        else {
            money = parseInt(money);
        }


        if (e.srcElement.id == 'money') {           //判断事件源是不是左边的 金额

            //给选中的input 填充金额
            $('.g_s-item:visible').each(function () {

                var input = $(this).find('input');
                if (input.hasClass('active')) {

                    input.val(money);
                }
            })

        }

    })
}

function BinddialogcloseClick() {
    $('.dialog_close,.cancelBtn').click(function () {

        $(this).parents('.dialog_wrap').hide();
    })
}



function BindplayInfoClick() {
    $('#playInfo').click(function () {

        $.post('/lottery/PanKouInfo', { lType: lType }, function (data) {
            $('#messageBox').html(data).show();
        })
    });
}


function BindrightBtnClick() {

    $('.rightBtn').click(function () {

        $(this).addClass('active').siblings().removeClass('active');

        var txt = $(this).html();

        if (txt == '快速下注') {
            $('#ksxz').show();

            $('#clpage').remove();
        }
        else {
            $('#ksxz').hide();

            $.post('/lottery/changlongPage', { lType: lType }, function (data) {
                $('#rightPart').append(data);
            })

        }

    });
}




//玩法块
function BindgsitemClick() {

    $('.g_s-item').live('click', function () {


        if ($(this).hasClass('disabled')) return;


        //连码最多选10个
        if (bigPlayName == '连码') {
            if ($('i:visible').filter('.active').size() >= 10) {

                ShowTanMin('最多选择10个号码');
                return;
            }
        }



        $(this).find('input').toggleClass('active');
        $(this).find('i').toggleClass('active');

        var m = $('#money').val();

        if (m != '') {
            $(this).find('input').val(m);
        }



        //取消胆码选择
        if (!$(this).find('i').hasClass('active')) {

            if ($(this).find('.g_dantuo_img').size() == 1) {

                $(this).find('.g_dantuo_img').remove();

                danMaCount--;

            }
        }






        if (bigPlayName.indexOf('连') != -1) {

            //第一步 确认上面 是否选择了 胆码 模式
            if ($('.el-radio-group  .el-radio__input:visible').eq(1).hasClass('is-checked')) {            //处理胆码样式


                //第二步 确认当前是否为选中
                if ($(this).find('i').hasClass('active')) {

                    //确认当前胆码个数够不够
                    if ((smallPlayName.indexOf('特串') != -1 && danMaCount < 1) || (smallPlayName.indexOf('二') != -1 && danMaCount < 1) || (smallPlayName.indexOf('三') != -1 && danMaCount < 2) || (smallPlayName.indexOf('四') != -1 && danMaCount < 3) || (smallPlayName.indexOf('五') != -1 && danMaCount < 4)) {

                        $(this).prepend('<span data-v-cbc0e39c="" class="g_dantuo_img"></span>');
                        danMaCount++;

                    }

                }
            }
        }






    });
}




function BindloginOutClick() {

    $('#loginOut').click(function () {
        $.post('/home/loginout', function () {
            location.href = '/home/login';
        });
    });
}

function BindcaiSelectClick() {
    $('.caiSelect').click(function () {

        var cid = $(this).attr('cid');

        lType = cid;

        $(this).addClass('router-link-active').siblings().removeClass('router-link-active');

        Init();     //重新初始化彩种
    })
}


function BindconfirmClick() {
    $('.confirm').live('click', function () {

        var pwd = $('input[name="pwd"]').val();
        var pwd2 = $('input[name="pwd2"]').val();
        var pwd3 = $('input[name="pwd3"]').val();

        if (pwd == '') {
            ShowTanMin('原密码不能为空');
        }
        else if (pwd2 == '') {
            ShowTanMin('新密码不能为空');
        }
        else if (pwd3 == '') {
            ShowTanMin('确认新密码不能为空');
        }
        else if (pwd2 != pwd3) {
            ShowTanMin('两次密码不一致');
        }
            //else if (pwd2.length < 8) {
            //    ShowTanMin('密码长度不能小于8');
            //}
        else {
            $.post('/home/modifyPwd', { pwd: pwd, pwd2: pwd2 }, function (data) {
                if (data == 'ok') {
                    ShowTanMin('修改成功');
                    $('#messageBox').hide();

                }
                else {
                    ShowTanMin(data);

                }
            });
        }

    })
}


function BindresetClick() {
    $('.reset').live('click', function () {

        $('input[type=password]').val('');
    })
}


function BindmodifyPwdClick() {
    $('#modifyPwd').click(function () {
        $.get('/home/modifyPwd', { lType: lType }, function (data) {

            $('#messageBox').html(data).show();
        })
    })
}

//rule

function BindruleClick() {
    $('#rule').click(function () {
        $.post('/lottery/Rule', { lType: lType }, function (data) {

            $('#messageBox').html(data).show();
        })
    })
}



function Binddialog_closeClick() {
    $('.dialog_close').live('click', function () {

        $('#messageBox').hide();
    })
}

function BindopenResultClick() {
    $('#openResult').click(function () {

        $.post('/lottery/OpenResult', { lType: lType }, function (data) {
            $('#messageBox').html(data).show();

        })

    })
}





function StartOrEnd() {

}





//大玩法
function BindBigitleClick() {

    $('#tab li').click(function () {

        isBigPlayClick = true;



        $(this).addClass('active').siblings().removeClass('active');


        var index = $('#tab li').index($(this));

        $('.lh_subtitle_container').hide();

        $('.lh_subtitle_container').eq(index).css('display', 'flex');


        $('.lh_subtitle_container').eq(index).find('.lh_subtitle:eq(0)').click();



        //如果没有二级玩法  直接切换玩法内容
        var title = $('.lh_subtitle_container').eq(index).html();


        if (title == '') {
            $('.component-page').children('div:visible:gt(0)').remove();    //清除旧的

            bigPlayName = $("[class='t_item lh_t_item active']").html();
            var playName = 'play_' + bigPlayName;

            $('.component-page').append(eval(playName));            //加玩法内容

            LoadPeiLV(bigPlayName, '');                  //加赔率


            //没有小玩法的
            if (bigPlayName == '特肖首尾') {
                smallPlayName = '特肖';
            }
            else if (bigPlayName == '总肖') {
                smallPlayName = '总肖单双';
            }
            else if (bigPlayName == '半波') {
                smallPlayName = '半波';
            }
        }




    });

}

//小玩法
function Bindlh_subtitleClick() {
    $('.lh_subtitle').click(function (e) {



        if (e.originalEvent) {     //不是JS点击  手动鼠标点击
            isBigPlayClick = false;
        }


        //alert($(this).html());

        $(this).addClass('li_subtitle_active').siblings().removeClass('li_subtitle_active');


        //
        bigPlayName = $("[class='t_item lh_t_item active']").html();

        //alert(bigPlayName);


        var playName = 'play_' + $(this).html();

        $('.component-page').children('div:visible:gt(0)').remove();    //清除旧的


        $('.component-page').append(eval(playName));


        smallPlayName = $(this).html();

        //加载赔率
        LoadPeiLV(bigPlayName, smallPlayName);



        //$('.s_item-rate:visible').addClass('change');    //黄色背景



    });
}


//改背景
function ChangeBG() {

    if (isBigPlayClick) return;         //默认第一个

    if (smallPlayName == '特码A' || smallPlayName == '特码B') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (smallPlayName == '正码A' || smallPlayName == '正码B') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (smallPlayName == '一肖中' || smallPlayName == '尾数中' || smallPlayName == '尾数不中') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (bigPlayName == '连肖' || bigPlayName == '连尾' || bigPlayName == '连码') {

        if (smallPlayName != '五尾连不中') {

            $('.s_item-rate:visible:lt(49)').addClass('change');
        }
    }
    else if (smallPlayName == '五不中' || smallPlayName == '六不中' || smallPlayName == '七不中' || smallPlayName == '八不中' || smallPlayName == '九不中' || smallPlayName == '十不中') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (smallPlayName == '六肖') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (smallPlayName == '一粒任中') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }
    else if (bigPlayName == '中一' && smallPlayName != '四中一') {
        $('.s_item-rate:visible:lt(49)').addClass('change');
    }

    setTimeout("$('.s_item-rate:visible:lt(49)').removeClass('change');", 5000);
}

//加载赔率
//function LoadPeiLV(bigPlayName, smallPlayName) {
function LoadPeiLV() {


    var d = new Date();

    var hour = d.getHours();
    var min = d.getMinutes();


    //if ((hour >= 17 && hour < 21) || (hour == 21 && min < 20)) {
    if (true) {

        //开盘之后


        //1.处理禁用
        $('.g_s-item:visible').removeClass('disabled');

        $('input:visible').removeAttr('disabled');

        //2.获取玩法赔率
        $.post('/lottery/peilvdata', { lType: lType, playBigType: bigPlayName }, function (data) {


            var arr = data.data;

            var j = 49;

            if (bigPlayName == '特码') {
                if (smallPlayName == '特码A') {

                    $('.s_item-rate:visible:lt(49)').html(arr[0].PeiLv);
                }
                else if (smallPlayName == '特码B') {
                    $('.s_item-rate:visible:lt(49)').html(arr[1].PeiLv);

                }

                for (var i = 2; i < arr.length; i++) {

                    $('.s_item-rate:visible').eq(j).html(arr[i].PeiLv);
                    j++;
                }
            }
            else if (bigPlayName == '正码') {

                if (smallPlayName == '正码A') {

                    $('.s_item-rate:visible:lt(49)').html(arr[0].PeiLv);
                }
                else if (smallPlayName == '正码B') {
                    $('.s_item-rate:visible:lt(49)').html(arr[1].PeiLv);

                }

                for (var i = 2; i < arr.length; i++) {

                    $('.s_item-rate:visible').eq(j).html(arr[i].PeiLv);
                    j++;
                }
            }
            else if (bigPlayName == '正特') {

                $('.s_item-rate:visible:lt(49)').html(arr[0].PeiLv);

                for (var i = 1; i < arr.length; i++) {

                    $('.s_item-rate:visible').eq(j).html(arr[i].PeiLv);
                    j++;
                }
            }
            else if (bigPlayName == '特肖首尾') {

                //特肖
                if (arr[0] != '停') {

                    var tArr = arr[0].PeiLv.split('/');

                    for (var i = 0; i < 12; i++) {

                        if (i == 2) {
                            $('.s_item-rate:visible').eq(i).html(tArr[0]);
                        }
                        else {
                            $('.s_item-rate:visible').eq(i).html(tArr[1]);

                        }
                    }
                }
                else {
                    $('.s_item-rate:visible:lt(12)').html(arr[0]);
                }

                //特尾
                $('.s_item-rate:visible:gt(11):lt(21)').html(arr[1].PeiLv);

                if (arr[1].PeiLv == '停') {
                    $('.s_item-rate:visible:gt(11):lt(21)').each(function () {
                        $(this).parent().addClass('disabled');
                        $(this).next().find('input').attr('disabled', 'disabled');
                    })
                }

                //头数
                $('.s_item-rate:visible:gt(21):lt(26)').html(arr[2].PeiLv);

                if (arr[2].PeiLv == '停') {
                    $('.s_item-rate:visible:gt(21):lt(26)').each(function () {
                        $(this).parent().addClass('disabled');
                        $(this).next().find('input').attr('disabled', 'disabled');
                    })
                }


            }
            else if (bigPlayName == '一肖尾数') {

                var info = '';
                if (smallPlayName == '一肖中') {

                    info = arr[0].PeiLv;
                }
                else if (smallPlayName == '一肖不中') {

                    info = arr[1].PeiLv;
                }
                else if (smallPlayName == '尾数中') {

                    info = arr[2].PeiLv;
                }
                else if (smallPlayName == '尾数不中') {

                    info = arr[3].PeiLv;
                }


                if (info != '停') {

                    var tArr = info.split('/');

                    for (var i = 0; i < 12; i++) {


                        if (smallPlayName == '尾数中') {
                            if (i == 0) {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);

                            }
                        }
                        else if (smallPlayName == '尾数不中') {
                            if (i == 0) {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);

                            }
                        }
                        else {
                            if (i == 2) {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);

                            }
                        }


                    }
                }
                else {
                    $('.s_item-rate:visible').html('停');


                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        $(this).next().find('input').attr('disabled', 'disabled');
                    });


                }



            }
            else if (bigPlayName == '连肖') {

                var info = '';

                if (smallPlayName == '二肖连中') {
                    info = arr[0];
                }
                else if (smallPlayName == '三肖连中') {
                    info = arr[1];
                }
                else if (smallPlayName == '四肖连中') {
                    info = arr[2];
                }
                else if (smallPlayName == '五肖连中') {
                    info = arr[3];
                }
                if (smallPlayName == '二肖连不中') {
                    info = arr[4];
                }
                else if (smallPlayName == '三肖连不中') {
                    info = arr[5];
                }
                else if (smallPlayName == '四肖连不中') {
                    info = arr[6];
                }
                else if (smallPlayName == '五肖连不中') {
                    info = arr[7];
                }



                if (info != '停') {

                    var tArr = info.PeiLv.split('/');

                    for (var i = 0; i < 12; i++) {

                        if (contains(smallPlayName, '不')) {
                            if (i == 2) {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);

                            }

                        }
                        else {
                            if (i == 2) {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);

                            }
                        }


                    }
                }
                else {
                    $('.s_item-rate:visible').html('停');
                }


            }
            else if (bigPlayName == '连尾') {

                var info = '';

                if (smallPlayName == '二尾连中') {
                    info = arr[0];
                }
                else if (smallPlayName == '三尾连中') {
                    info = arr[1];
                }
                else if (smallPlayName == '四尾连中') {
                    info = arr[2];
                }
                else if (smallPlayName == '五尾连中') {
                    info = arr[3];
                }
                if (smallPlayName == '二尾连不中') {
                    info = arr[4];
                }
                else if (smallPlayName == '三尾连不中') {
                    info = arr[5];
                }
                else if (smallPlayName == '四尾连不中') {
                    info = arr[6];
                }
                else if (smallPlayName == '五尾连不中') {

                    info = arr[7];

                    console.log(info);
                }


                if (info.PeiLv != '停') {

                    var tArr = info.PeiLv.split('/');

                    for (var i = 0; i < 12; i++) {


                        if (smallPlayName.indexOf('不') != -1) {

                            if (i == 0) {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);

                            }
                        }
                        else {

                            if (i == 0) {
                                $('.s_item-rate:visible').eq(i).html(tArr[1]);
                            }
                            else {
                                $('.s_item-rate:visible').eq(i).html(tArr[0]);

                            }
                        }


                    }
                }
                else {


                    $('.s_item-rate:visible').html('停');

                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        //$(this).next().find('input').attr('disabled', 'disabled');
                    });
                }


            }


            else if (bigPlayName == '连码') {

                var info = '';

                if (smallPlayName == '三全中') {
                    info = arr[0];
                }
                else if (smallPlayName == '三中二') {
                    info = arr[1];
                }
                else if (smallPlayName == '三中二X') {
                    info = arr[2];
                }
                else if (smallPlayName == '二全中') {
                    info = arr[3];
                }
                else if (smallPlayName == '二中特') {
                    info = arr[4];
                }
                else if (smallPlayName == '二中特X') {
                    info = arr[5];
                }
                else if (smallPlayName == '特串') {
                    info = arr[6];
                }


                if (info != '停') {

                    $('.s_item-rate:visible').html(info.PeiLv);
                }
                else {
                    $('.s_item-rate:visible').html('停');
                }


            }
            else if (bigPlayName == '自选不中') {

                var info = '';

                if (smallPlayName == '五不中') {
                    info = arr[0];
                }
                else if (smallPlayName == '六不中') {
                    info = arr[1];
                }
                else if (smallPlayName == '七不中') {
                    info = arr[2];
                }
                else if (smallPlayName == '八不中') {
                    info = arr[3];
                }
                else if (smallPlayName == '九不中') {
                    info = arr[4];
                }
                else if (smallPlayName == '十不中') {
                    info = arr[5];
                }
                else if (smallPlayName == '十一不中') {
                    info = arr[6];
                }
                else if (smallPlayName == '十二不中') {
                    info = arr[7];
                }



                if (info.PeiLv != '停') {

                    $('.s_item-rate:visible').html(info.PeiLv);
                }
                else {
                    $('.s_item-rate:visible').html('停');

                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        //$(this).next().find('input').attr('disabled', 'disabled');
                    });

                }


            }
            else if (bigPlayName == '总肖') {

                $('.s_item-rate:visible').eq(6).html(arr[0].PeiLv);
                $('.s_item-rate:visible').eq(7).html(arr[1].PeiLv);
                $('.s_item-rate:visible:lt(6)').html(arr[2].PeiLv);

                if (arr[2].PeiLv == '停') {
                    $('.s_item-rate:visible:lt(6)').each(function () {

                        $(this).parents('.g_s-item').addClass('disabled');
                        $(this).next().find('input').attr('disabled', 'disabled');
                    });
                }

            }
            else if (bigPlayName == '合肖') {

                var info = '';

                if (smallPlayName == '二肖') {
                    info = arr[0];
                }
                else if (smallPlayName == '三肖') {
                    info = arr[1];
                }
                else if (smallPlayName == '四肖') {
                    info = arr[2];
                }
                else if (smallPlayName == '五肖') {
                    info = arr[3];
                }
                else if (smallPlayName == '六肖') {
                    info = arr[4];
                }




                if (info.PeiLv != '停') {

                    $('.s_item-rate:visible').html(info.PeiLv);
                }
                else {
                    $('.s_item-rate:visible').html('停');

                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        //$(this).next().find('input').attr('disabled', 'disabled');
                    });

                }


            }
            else if (bigPlayName == '特平中') {

                var info = '';

                if (smallPlayName == '一粒任中') {
                    info = arr[0];
                }
                else if (smallPlayName == '二粒任中') {
                    info = arr[1];
                }
                else if (smallPlayName == '三粒任中') {
                    info = arr[2];
                }
                else if (smallPlayName == '四粒任中') {
                    info = arr[3];
                }
                else if (smallPlayName == '五粒任中') {
                    info = arr[4];
                }




                if (info.PeiLv != '停') {

                    $('.s_item-rate:visible').html(info.PeiLv);
                }
                else {
                    $('.s_item-rate:visible').html('停');

                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        //$(this).next().find('input').attr('disabled', 'disabled');
                    });

                }


            }
            else if (bigPlayName == '中一') {

                var info = '';

                if (smallPlayName == '四中一') {
                    info = arr[0];
                }
                else if (smallPlayName == '五中一') {
                    info = arr[1];
                }
                else if (smallPlayName == '六中一') {
                    info = arr[2];
                }
                else if (smallPlayName == '七中一') {
                    info = arr[3];
                }
                else if (smallPlayName == '八中一') {
                    info = arr[4];
                }
                else if (smallPlayName == '九中一') {
                    info = arr[5];
                }
                else if (smallPlayName == '十中一') {
                    info = arr[6];
                }


                if (info.PeiLv != '停') {

                    $('.s_item-rate:visible').html(info.PeiLv);
                }
                else {
                    $('.s_item-rate:visible').html('停');

                    $('.s_item-rate:visible').each(function () {

                        $(this).parent().addClass('disabled');
                        //$(this).next().find('input').attr('disabled', 'disabled');
                    });

                }


            }
            else if (bigPlayName == '半波') {

                for (var i = 0; i < 12; i++) {
                    $('.s_item-rate:visible').eq(i).html(arr[i].PeiLv);
                }
            }
            else if (bigPlayName == '五行') {

                for (var i = 0; i < 5; i++) {
                    $('.s_item-rate:visible').eq(i).html(arr[i].PeiLv);

                    if (arr[i].PeiLv == '停') {
                        $('.s_item-rate:visible').eq(i).parents('.g_s-item').addClass('disabled');
                        $('.s_item-rate:visible').eq(i).next().find('input').attr('disabled', 'disabled');
                    }

                }


            }


            //改背景
            ChangeBG(bigPlayName, smallPlayName);






        });


    }
}



//---------------公共方法------------------------

//trim方法
function trimEnd(str) {
    return str.substr(0, str.length - 1);
}


function contains(str1, str2) {

    if (str1.indexOf(str2) != -1) {
        return true;
    }

    return false;

}


//计算阶乘的方法
function JieCheng(num) {
    var result = 1;
    for (var i = 1; i <= num; i++) {
        result *= i;
    }
    return result;
}
