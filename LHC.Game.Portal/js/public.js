





//----------------------------变量


var lType = 1;
var rebates = 1.982;

var transferFrom = 1;
var bettingRecordFrom = 1;
var fundFrom = 1;
var rechargeFrom = 1;
var bankFrom = 1;


//充值相关
var limit1 = 100;                     //充值额度下限
var limit2 = 50000;                 //充值额度上限
var rechargeType = 1;               //充值类型      1.微信    2.QQ    3.支付宝   4.银行转账  5.在线网银  6.微信APP支付   7.支付宝APP支付
var rechargeLine = 1;               //充值通道      2.智汇付   2.威富通
var bankId = 1;                     //转账银行ID



var isTry = 0;   //是否是试玩用户

var leftPlayName = '';          //左边的玩法

var bankType = 1;

var confirmEvent = "";




var provience = ['北京市', '广东', '上海', '天津', '重庆', '辽宁', '江苏', '湖北', '四川', '陕西', '河北', '山西', '河南', '吉林', '黑龙江', '内蒙古', '山东', '安徽', '浙江',
    '福建', '湖南', '广西', '江西', '贵州', '云南', '西藏', '海南', '甘肃', '宁夏', '青海', '新疆', '香港', '澳门', '台湾', '海外', '其它'];


var city = ['海淀区、东城区、西城区、宣武区、丰台区、朝阳区、崇文区、大兴区、石景山区、门头沟区、房山区、通州区、顺义区、怀柔区、昌平区、平谷区、密云县、延庆县',
    '广州、深圳、汕头、惠州、珠海、揭阳、佛山、河源、阳江、茂名、湛江、梅州、肇庆、韶关、潮州、东莞、中山、清远、江门、汕尾、云浮',
    '黄浦区、卢湾区、徐汇区、长宁区、静安区、普陀区、闸北区、杨浦区、虹口区、闵行区、宝山区、嘉定区、浦东新区、金山区、松江区、青浦区、南汇区、奉贤区、崇明县',
    '和平区、河西区、河北区、河东区、南开区、红桥区、北辰区、津南区、武清区、塘沽区、西青区、汉沽区、大港区、宝坻区、东丽区、蓟县、静海县、宁河县',
    '渝中区、大渡口区、江北区、沙坪坝区、九龙坡区、南岸区、北碚区、万盛区、双桥区、渝北区、巴南区、万州区、涪陵区、黔江区、长寿区、江津区、永川区、南川区、綦江县、潼南县、铜梁县、大足县、荣昌县、璧山县、垫江县、武隆县、丰都县、城口县、梁平县、开县、巫溪县、巫山县、奉节县、云阳县、忠县、石柱土家族自治县、彭水苗族土家族自治县、酉阳苗族自治县、秀山土家族苗族自治县',
    '沈阳、大连、金州、鞍山、抚顺、本溪、丹东、锦州、营口、阜新、辽阳、盘锦、铁岭、朝阳、葫芦岛',
    '南京、镇江、常州、无锡、苏州、徐州、连云港、淮安、盐城、扬州、泰州、南通、宿迁',
    '武汉、襄樊、宜昌、黄石、鄂州、随州、荆州、荆门、十堰、孝感、黄冈、咸宁',
    '成都、绵阳、德阳、广元、自贡、攀枝花、乐山、南充、内江、遂宁、广安、泸州、达州、眉山、宜宾、雅安、资阳',
    '西安、咸阳、铜川、延安、宝鸡、渭南、汉中、安康、商洛、榆林',
    '石家庄、唐山、邯郸、秦皇岛、保定、张家口、承德、廊坊、沧州、衡水、邢台',
    '太原、大同、忻州、阳泉、长治、晋城、朔州、晋中、运城、临汾、吕梁',
    '郑州、洛阳、开封、漯河、安阳、新乡、周口、三门峡、焦作、平顶山、信阳、南阳、鹤壁、濮阳、许昌、商丘、驻马店',
    '长春、吉林、四平、辽源、通化、白山、松原、白城',
    '哈尔滨、大庆、齐齐哈尔、佳木斯、鸡西、鹤岗、双鸭山、牡丹江、伊春、七台河、黑河、绥化 加格达奇',
    '呼和浩特、包头、乌海、赤峰、通辽、鄂尔多斯、呼伦贝尔、巴彦淖尔、乌兰察布',
    '济南、青岛、淄博、枣庄、东营、烟台、潍坊、济宁、泰安、威海、日照、莱芜、临沂、德州、聊城、菏泽、滨州 ',
    '合肥、蚌埠、芜湖、淮南、亳州、阜阳、淮北、宿州、滁州、安庆、巢湖、马鞍山、宣城、黄山、池州、铜陵',
    '杭州、嘉兴、湖州、宁波、金华、温州、丽水、绍兴、衢州、舟山、台州',
    '福州、厦门、泉州、三明、南平、漳州、莆田、宁德、龙岩',
    '长沙、株洲、湘潭、衡阳、岳阳、郴州、永州、邵阳、怀化、常德、益阳、张家界、娄底',
    '南宁、柳州、桂林、梧州、北海、崇左、来宾、贺州、玉林、百色、河池、钦州、防城港、贵港',
    '南昌、九江、赣州、吉安、鹰潭、上饶、萍乡、景德镇、新余、宜春、抚州',
    '贵阳、六盘水、遵义、安顺',
    '昆明、曲靖、玉溪、保山、昭通、丽江、普洱、临沧',
    '拉萨',
    '海口、三亚',
    '兰州、天水、平凉、酒泉、嘉峪关、金昌、白银、武威、张掖、庆阳、定西、陇南',
    '银川、石嘴山、吴忠、固原、中卫',
    '西宁',
    '乌鲁木齐、克拉玛依19县级市-石河子、阿拉尔市、图木舒克、五家渠、哈密、吐鲁番、阿克苏、喀什、和田、伊宁、塔城、阿勒泰、奎屯、博乐、昌吉、阜康、库尔勒、阿图什、乌苏',
    '中西区、东区、九龙城区、观塘区、南区、深水埗区、黄大仙区、湾仔区、油尖旺区、离岛区、葵青区、北区、西贡区、沙田区、屯门区、大埔区、荃湾区、元朗区',
    '花地玛堂区、圣安多尼堂区（花王堂区）、望德堂区、大堂区、风顺堂区（圣老楞佐堂区）、离岛、氹仔、路环',
    '台北、台中、基隆、高雄、台南、新竹、嘉义'
];



//----------------------------------


$(function () {


    BindfuzhiClick();               //复制银行卡号

    BindVimgClick();

    BindsomesubClick();

    BindtryCLick();     //试玩




    BindSelectprovienceChange();            //省市级联切换

    var href = location.href.toLowerCase();
    if (href.indexOf('lottery') != -1) {

        //JudgeHasLotteryMoey();          //判断彩票游戏有没有钱

        setInterval('updateLotteryMoney()', 5000);
    }

    JudgeIsTryUser();          //判断是不是试玩用户 赋值给 isTry 变量

    BindwinthemClick(); //弹窗  统一调用


});




function BindwinthemClick() {
    $('*[win-them]').live('click', function () {

        var them = $(this).attr('win-them');

        confirmEvent = $(this).attr('event');

        ShowTan(them);

    });
}




function BindfuzhiClick() {
    $('.fuzhi').live('click', function () {
        $(this).html('复制成功');
    });
}


function GetRechargeTypeName() {

    //.微信    2.QQ    3.支付宝   4.在线网银  5.银行转账 
    var result = '';
    if (rechargeType < 3) {
        result = '微信支付';
    }
    else if (rechargeType >= 3 && rechargeType < 5) {
        result = 'QQ钱包支付';
    }
    else if (rechargeType >= 6 && rechargeType < 8) {
        result = '支付宝支付';
    }
    else if (rechargeType == 8) {
        result = '在线网银';
    }
    else if (rechargeType == 5) {
        result = '银行卡转账';
    }


    return result;
}


function BindSelectprovienceChange() {
    $('select[name=provience]').live('change', function () {

        var p = $(this).val();
        var index = provience.indexOf(p);
        var c = city[index];

        var arr = c.split('、');
        $('select[name=city]').empty();

        for (var i = 0; i < arr.length; i++) {
            $('select[name=city]').append('<option>' + arr[i] + '</option>');
        }

    });
}


function JudgeHasLotteryMoey() {
    $.post('/Lottery/GetLotteryMoney', function (data) {

        //判断登录
        if (JudgeReturnIsLogin(data)) {
            return;
        }


        $('#lotteryMoney').html(data);
        if (parseFloat(data) == 0 && location.href.toLowerCase().indexOf('lottery') != -1) {
            ShowTan('zz');
        }
    });
}


function updateLotteryMoney() {
    $.post('/Lottery/GetLotteryMoney', function (data) {

        //判断登录
        if (JudgeReturnIsLogin(data)) {
            return;
        }

        $('#lotteryMoney').html(data);
    });
}


function BindtryCLick() {

    if (!JudgeIDExist('try')) return;

    document.getElementById('try').addEventListener('click', function () {
        ShowLoading();
        $.post('/Home/Login', { username: 'shiwanaccount' }, function () {
            CloseLoading();
            ShowTanMin('登陆成功');

            location.href = '/home/index';


            //$('.dfh_idx_Sign a:eq(2)').removeAttr('id').html('试玩账号');
            //$('.dfh_idx_Sign a:eq(1)').remove();
            //$('.dfh_idx_Sign a').css('text-decoration','underline');
        });
    });

}


function BindVimgClick() {
    $('#vimgChange').click(function () {
        $('.vimg').attr("src", "/home/getvcode?" + Math.random());
    });
}


function BindsomesubClick() {
    $('form').on('tap', '.somesub', function () {

        $(this).parents('form').submit();
    });
}



//----------------------------方法

//只能包含数字字母 限制
function LimitDigitAndEng(con) {

    var reg = /^[0-9a-zA-Z]+$/;

    if (!reg.test(con)) {
        return false;
    }
    return true;
}

//只能包含数字
function LimitDigit(con) {

    var reg = /^[0-9]+$/;

    if (!reg.test(con)) {
        return false;
    }
    return true;
}


//必须同时包含数字字母 限制
function LimitMustDigitAndEng(con) {

    var reg = /^(([a-z]+[0-9]+)|([0-9]+[a-z]+))[a-z0-9]*$/i;  //判断字符串是否为数字和字母组合     //判断正整数 /^[1-9]+[0-9]*]*$/    

    return reg.test(con);
}

//必须是纯中文 限制
function LimitMustIsZhongWen(con) {

    var reg = /^[\u4E00-\u9FA5]+$/;

    return reg.test(con);
}





function JudgeIsTryUser() {

    //判断是否是试玩 只判断一次
    if (isTry == 0) {
        var tryId = $('#tryId').val();

        if (tryId != undefined && tryId != '') {
            isTry = true;
        } else {
            isTry = false;
        }
    }
}



function InitLoadingForMoney(opts, index) {
    var target = $('.loading')[index];
    new Spinner(opts).spin(target);
}

function ShowLoadingForMoney() {
    $('.loading').show();
}

function CloseLoadingForMoney() {
    $('.loading').hide();
}

//今日输赢
function LoadTodayWinMoney() {
    $.post('/Lottery/GetTodayWinMoney', function (data) {
        CloseLoadingForMoney();
        $('#todayWinMoney').html('(' + parseFloat(data).toFixed(2) + ')').show();
    });
}

//获取即时注单的金额
function LoadJSZDMoney() {
    $.post('/Lottery/GetJSZDMoney', function (data) {
        CloseLoadingForMoney();
        $('#jszdMoney').html('(' + parseFloat(data).toFixed(2) + ')').show();
    });
}

function JudgeIsTry() {
    if (isTry) {
        ShowTanMin2('试玩账号无权访问，请注册');
        return true;
    }
}

//显示加载
function ShowLoading() {

    $('#loading').show();
}


function CloseLoading() {

    $('#loading').hide();
}

//判断id是否存在
function JudgeIDExist(id) {
    if ($('#' + id).size() > 0) {
        return true;
    } else {
        return false;
    }
}


//改变锚点
function ChangeLocation(target) {
    var href = location.href;
    var index = href.indexOf('#');

    location.href = href.substr(0, index) + target;
}


//检查非法输入--数字和字母
function checkInputIsAll(str) {
    var count1 = 0;
    var count2 = 0;
    for (var i = 0; i < str.length; i++) {
        var s = str[i];
        if (/[0-9]/.test(s)) {
            count1++;
        } else {
            count2++;
        }
    }
    if (count1 == 0 || count2 == 0) {
        return false;
    } else {
        return true;
    }
}

//根据id获取彩种名称
function GetLotteryNameById(lType) {

    var result = '';
    if (lType == 1) {
        result = '重庆时时彩';
    }
    else if (lType == 85) {
        result = '河内五分彩';
    }
    else if (lType == 61) {
        result = '腾讯分分彩';
    }
    else if (lType == 81) {
        result = '澳洲幸运5';
    }
    else if (lType == 25) {
        result = '360分分彩';
    }
    else if (lType == 27) {
        result = '天津时时彩';
    }
    else if (lType == 2) {
        result = '快速时时彩';
    }
    else if (lType == 3) {
        result = '澳门六合彩';
    }
    else if (lType == 4) {
        result = '快速六合彩';
    }
    else if (lType == 5) {
        result = '七星彩';
    }
    else if (lType == 6) {
        result = '快速七星彩';
    }
    else if (lType == 7) {
        result = '北京PK10';
    }
    else if (lType == 8) {
        result = '快速赛车';
    }
    else if (lType == 62) {
        result = '澳门赛车';
    }
    else if (lType == 9) {
        result = '幸运飞艇';
    }
    else if (lType == 10) {
        result = '快速飞艇';
    }
    else if (lType == 83) {
        result = '澳洲幸运10';
    }
    else if (lType == 11) {
        result = '3D';
    }
    else if (lType == 12) {
        result = '快速3D';
    }
    else if (lType == 13) {
        result = '广东快乐十分';
    }
    else if (lType == 29) {
        result = '山西快乐十分';
    }
    else if (lType == 31) {
        result = '湖南快乐十分';
    }
    else if (lType == 33) {
        result = '天津快乐十分';
    }
    else if (lType == 14) {
        result = '快速快乐十分';
    }
    else if (lType == 15) {
        result = '广东11选5';
    }
    else if (lType == 35) {
        result = '河北11选5';
    }
    else if (lType == 37) {
        result = '浙江11选5';
    }
    else if (lType == 39) {
        result = '上海11选5';
    }
    else if (lType == 41) {
        result = '山东11选5';
    }
    else if (lType == 43) {
        result = '江苏11选5';
    }
    else if (lType == 45) {
        result = '黑龙江11选5';
    }
    else if (lType == 47) {
        result = '山西11选5';
    }
    else if (lType == 16) {
        result = '快速11选5';
    }
    else if (lType == 17) {
        result = '幸运农场';
    }
    else if (lType == 18) {
        result = '快速农场';
    }
    else if (lType == 19) {
        result = '排列三';
    }
    else if (lType == 20) {
        result = '快速排列三';
    }
    else if (lType == 21) {
        result = '江苏快三';
    }
    else if (lType == 49) {
        result = '湖北快三';
    }
    else if (lType == 51) {
        result = '安徽快三';
    }
    else if (lType == 53) {
        result = '河北快三';
    }
    else if (lType == 55) {
        result = '上海快三';
    }
    else if (lType == 57) {
        result = '广西快三';
    }
    else if (lType == 59) {
        result = '吉林快三';
    }
    else if (lType == 22) {
        result = '快速快三';
    }
    else if (lType == 23) {
        result = '北京28';
    }
    else if (lType == 24) {
        result = '幸运28';
    }

    return result;
}

//计算阶乘的方法
function JieCheng(num) {
    var result = 1;
    for (var i = 1; i <= num; i++) {
        result *= i;
    }
    return result;
}


//trim方法
function trimEnd(str) {
    return str.substr(0, str.length - 1);
}

function WanfaSplit(wanfa) {

    var arr2 = new Array();
    var arr = wanfa.split('【');
    arr2[0] = arr[0];
    arr2[1] = arr[1].substr(0, arr[1].length - 1);

    return arr2;
}


function ShowTanMin(content) {

    $('div[win=ts] h3').html(content);
    $('div[win=ts]').show();

    setTimeout("$('div[win=ts]').hide()", 2000);
}

function CloseShowTanMin() {
    $('div[win=ts]').hide();
}

//不自动关闭
function ShowTanMin2(content) {

    $('div[win=ts] h3').html(content);
    $('div[win=ts]').show();

    //setTimeout("$('div[win=ts]').hide()", 1000);
}

function ShowTanMin2(content) {

    $('div[win=ts2] h3').html(content);
    $('div[win=ts2]').show();

    setTimeout("$('div[win=ts2]').hide()", 2000);
}


function ShowTanMin2ForSecond(content, seconds) {

    $('div[win=ts2] h3').html(content);
    $('div[win=ts2]').show();

    setTimeout("$('div[win=ts2]').hide()", seconds);
}

function ShowTanMin3(content) {

    $('div[win=ts3] h3').html(content);
    $('div[win=ts3]').show();

    setTimeout("$('div[win=ts3]').hide()", 1000);
}


function ShowTanConfirm(con, about) {
    $('#confirmInfo').html(con);
    $('#confirmBtn').attr('about', about);
    $('div[win=confirm]').show();
}

function ShowTan(id) {
    $('div[win=' + id + ']').show();
}

function CloseTan(id) {
    $('div[win=' + id + ']').hide();
}


function InitIssueAndTime() {

    $.post('/Lottery/GetCurrentIssueAndTime', { lType: lType }, function (data) {

        HandIssueAndTime(data);
    });
}






function HandIssueAndTime(data) {

    var arr = data.split('|');

    $('#nextIssue').html(arr[0]);       //期号

    if (arr[1] == '已封盘') {
        $('#fenpanTime').html('已封盘');
        $('#fengpan').show();               //底部的封盘遮罩
    }
    else {

        $('#fengpan').hide();               //底部的封盘遮罩

        var arr2 = arr[1].split('&');

        if (lType == 3 || lType == 5 || lType == 11 || lType == 19) {
            $('#fenpanTime').html('<t id="hour">' + arr2[0] + '</t>:<t id="minute">' + arr2[1] + '</t>:<t id="second">' + arr2[2] + '</t>');
        }
        else {
            $('#fenpanTime').html('<t id="minute">' + arr2[1] + '</t>:<t id="second">' + arr2[2] + '</t>');
        }

    }

    if (arr[2] == '正在开奖') {
        $('#openTime').html('正在开奖');
    }
    else {
        var arr3 = arr[2].split('&');
        if (lType == 3 || lType == 5 || lType == 11 || lType == 19) {
            $('#openTime').html('<t id="hour2">' + arr3[0] + '</t>:<t id="minute2">' + arr3[1] + '</t>:<t id="second2">' + arr3[2] + '</t>');
        } else {
            $('#openTime').html('<t id="minute2">' + arr3[1] + '</t>:<t id="second2">' + arr3[2] + '</t>');
        }

    }
}

function FillLastOpenNum(data) {

    var arr = data.split('|');

    //期号
    $('.kaijiang[lType=' + lType + ']').find('div').eq(0).find('span').html(arr[0] + '期');


    if (lType == 1 || lType == 2 || lType == 25 || lType == 27 || lType == 61 || lType == 81 || lType == 85) {

        //开奖号码
        var lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        var arr2 = arr[1].split(',');
        for (var i = 0; i < arr2.length; i++) {
            lastNum.append('<span>' + arr2[i] + '</span> ');
        }

        //额外信息
        var info = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(1);
        info.empty();
        var arr3 = arr[2].split(',');
        for (i = 0; i < arr3.length; i++) {
            info.append('<span>' + arr3[i] + '</span> ');
        }

    }
    else if (lType == 3 || lType == 4) {

        //开奖号码
        lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        arr2 = arr[1].split(',');

        liuhecaiNum = arr[1];  //保存值 给 切换生肖的时候用

        for (i = 0; i < arr2.length; i++) {

            var color = GetColorByDigit(arr2[i]);

            if (i == 6) {
                lastNum.append('<span class="clrt_rLjia"></span>');
                lastNum.append('<span class="clrt_rLqiu clrt_rLqiu' + color + '">' + arr2[i] + '</span> ');
            }
            else {
                lastNum.append('<span class="clrt_rLqiu clrt_rLqiu' + color + '">' + arr2[i] + '</span> ');
            }
        }

    }
    else if (lType == 7 || lType == 8 || lType == 62 || lType == 9 || lType == 10 || lType == 83) {

        //开奖号码
        lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        arr2 = arr[1].split(',');
        for (i = 0; i < arr2.length; i++) {
            lastNum.append('<span class="clrt_baseIfsp' + arr2[i] + '">' + arr2[i] + '</span> ');
        }


        //额外信息
        info = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(1);
        info.empty();
        arr3 = arr[2].split(',');
        for (i = 0; i < arr3.length; i++) {
            info.append('<span class="resultData resultData-width">' + arr3[i] + '</span> ');
        }
    }
    else if (lType == 11 || lType == 12 || lType == 19 || lType == 20) {

        //开奖号码
        lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        arr2 = arr[1].split(',');
        for (i = 0; i < arr2.length; i++) {
            lastNum.append('<span class="clrt_rLPCqiu">' + arr2[i] + '</span> ');
        }
    }
    else if (lType == 21 || lType == 49 || lType == 51 || lType == 53 || lType == 55 || lType == 57 || lType == 59 || lType == 22 || lType == 23 || lType == 24) {

        //开奖号码
        lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        arr2 = arr[1].split(',');

        //for (i = 0; i < arr2.length; i++) {
        //    lastNum.append('<span class="clrt_rLPCqiu">' + arr2[i] + '</span> ');
        //}

        var sum = parseInt(arr2[0]) + parseInt(arr2[1]) + parseInt(arr2[2]);

        lastNum.append('<span class="clrt_rLPCqiu">' + arr2[0] + '</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">+</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">' + arr2[1] + '</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">+</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">' + arr2[2] + '</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">=</span> ');

        lastNum.append('<span class="clrt_rLPCqiu">' + sum + '</span> ');

    }
    else if (lType == 5 || lType == 6 || lType == 13 || lType == 29 || lType == 31 || lType == 33 || lType == 14 || lType == 15 || lType == 35 || lType == 37 || lType == 39 || lType == 41 || lType == 43 || lType == 45 || lType == 47 || lType == 16 || lType == 17) {

        //开奖号码
        lastNum = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(0);
        lastNum.empty();
        arr2 = arr[1].split(',');
        for (i = 0; i < arr2.length; i++) {
            lastNum.append('<span class="clrt_rLPCqiu">' + arr2[i] + '</span> ');
        }

        //额外信息
        if (lType == 13 || lType == 29 || lType == 31 || lType == 33 || lType == 14 || lType == 15 || lType == 35 || lType == 37 || lType == 39 || lType == 41 || lType == 43 || lType == 45 || lType == 47 || lType == 16 || lType == 17) {
            info = $('.kaijiang[lType=' + lType + ']').find('div').eq(1).find('div').eq(1);
            info.empty();
            arr3 = arr[2].split(',');
            for (i = 0; i < arr3.length; i++) {
                info.append('<span>' + arr3[i] + '</span> ');
            }
        }
    }

}

function JudgeReturnIsLogin(data) {

    if (data.indexOf('登录') != -1) {
        window.location = '/home/login';
        return true;
    }
}


//判断输入是不是正整数
function JudgeZS(num) {
    if (num == '') {
        return false;
    }
    else if (!/^\d+$/.test(num)) {
        return false;
    }
    else if (parseFloat(num) <= 0) {
        return false;
    }
    else {
        return true;
    }
}


//
function GetNewUrl() {
    var href = location.href;
    var arr = href.split('/');

    var result = '';
    for (var i = 0; i < arr.length - 1; i++) {
        result += arr[i] + '/';
    }

    result += lType;
    return result;
}


function CheckBankInfo() {
    $.post('/Home/CheckBankInfo', function (data) {
        if (data == "0") {
            $('.GL_Tinfo:eq(0)').find('.GL_Tqk:eq(0)').show().siblings().hide();
        } else {
            $('.GL_Tinfo:eq(0)').find('.GL_Tqk:eq(1)').show().siblings().hide();

            var data2 = $.parseJSON(data);

            //填充数据
            $('.GL_Tinfo:eq(0)').find('.GL_Tqk:eq(1)').find('select').empty();


            for (var o in data2) {
                var d = data2[o];
                $('.GL_Tinfo:eq(0)').find('.GL_Tqk:eq(1)').find('select').append('<option value=' + d.Id + '>' + d.BankName + '  ' + d.BankNum + '</option>');
            }
        }
    });
}

//判断流水
function CheckLiuShui() {

    $.post('/Home/GetLiuShui', function (data) {
        if (data != '1') { //流水不够

            var arr = data.split('|');
            $('#txNot1').html('存款额：' + arr[0] + '，当前流水：' + arr[1] + ' 差流水：' + arr[2]);

            $('#txNot1,#txNot2').show();
        }
        else {
            $('#txNot1,#txNot2').hide();
        }
    });
}



//-----------------------初始化 银行卡转账


function InitBank(index) {



    $.post('/Home/RechargeSDBankData', function (data) {
        data = $.parseJSON(data);

        if (data.length > 0) {

            var arr = new Array();

            $('.GL_Tiftext .GL_Tiful:eq(' + index + ') .GL_YHK').remove();

            for (var i = 0; i < data.length; i++) {

                var d = data[i];

                FillBankData(d, index);
            }



            //for (var i = 0; i < data.length; i++) {

            //    var d = data[i];

            //    if (d.BankName == '微信支付') {
            //        FillBankData(d, index);
            //    }
            //    else {
            //        arr.push(d);
            //    }
            //}

            //var len = arr.length;

            //var id = parseInt($('#userIdForBank').val());

            //if (len == 1) {
            //    FillBankData(arr[0], index);
            //}
            //else if (len == 2) {
            //    if (id % 2 == 0) {
            //        FillBankData(arr[0], index);
            //    }
            //    else {
            //        FillBankData(arr[1],index);
            //    }
            //}
            //else if (len == 3) {
            //    if (id < 4) {
            //        FillBankData(arr[0], index);
            //    }
            //    else if (id < 7) {
            //        FillBankData(arr[1], index);
            //    }
            //    else {
            //        FillBankData(arr[2], index);
            //    }
            //}
            //else if (len == 4) {
            //    if (id % 2 == 0) {
            //        FillBankData(arr[0], index);
            //        FillBankData(arr[1], index);
            //    }
            //    else {
            //        FillBankData(arr[2], index);
            //        FillBankData(arr[3], index);
            //    }
            //}


        }


    });

    //var clipboard = new Clipboard('.fuzhi');
}


function FillBankData(d, index) {



    if (d.BankName == '银行扫码') {
        $('.GL_Tiftext .GL_Tiful:eq(' + index + ')').prepend('<div bankId="' + d.Id + '" class="GL_YHK">' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款银行</p>' +
            '<p class="YHK_KpR">' + d.BankName + '</p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款账号</p>' +
            '<p class="YHK_KpR"><img src="/images/bank/' + d.BankNum + '" width="200" height="240"/></p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款人</p>' +
            '<p class="YHK_KpR">' + d.RealName + '</p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款支行</p>' +
            '<p class="YHK_KpR">' + d.ZhiHangName + '</p>' +
            '</div>' +
            '<i class="YHK_icon">' +
            '<img src="/images/48_1.png" class="YHK_icontu1">' +
            '<img src="/images/48_2.png" class="YHK_icontu2">' +
            '</i>' +
            '</div>');

    }
    else {
        $('.GL_Tiftext .GL_Tiful:eq(' + index + ')').prepend('<div bankId="' + d.Id + '" class="GL_YHK">' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款银行</p>' +
            '<p class="YHK_KpR">' + d.BankName + '</p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款账号</p>' +
            '<p class="YHK_KpR">' + d.BankNum + '<font class="fuzhi" data-clipboard-text="' + d.BankNum + '">复制</font></p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款人</p>' +
            '<p class="YHK_KpR">' + d.RealName + '<font class="fuzhi" data-clipboard-text="' + d.RealName + '">复制</font></p>' +
            '</div>' +
            '<div class="YHK_K">' +
            '<p class="YHK_KpL f-l">收款支行</p>' +
            '<p class="YHK_KpR">' + d.ZhiHangName + '</p>' +
            '</div>' +
            '<i class="YHK_icon">' +
            '<img src="/images/48_1.png" class="YHK_icontu1">' +
            '<img src="/images/48_2.png" class="YHK_icontu2">' +
            '</i>' +
            '</div>');
    }



}


//修改赔率
function ModifyPeiLv(v) {


    if (lType == 3 || lType == 4) {
        return;
    }




    //$('.scroll_info:visible .ifk1_tb_sp2').each(function () {

    //    var v1 = parseFloat($(this).html());
    //    $(this).html((v1 * v / 1.982).toFixed(3));

    //});



    $('.scroll_info').each(function () {

        $(this).find('.ifk1_tb_sp2').each(function () {

            var v1 = parseFloat($(this).html());


            //if (v1 == 9.85) {


            //    alert(v);

            //    alert(v1);
            //}
           

            

            $(this).html((v1 * v / 1.982).toFixed(3));
        });

    });




    //var arr1 = $('.scroll_info');

    //for (var i = 0; i < arr1.length; i++) {

    //    var t = $(arr1[i]);

    //    var arr2 = t.find('.ifk1_tb_sp2');

    //    for (var j = 0; j < arr2.length; j++) {

    //        var t2 = $(arr2[j]);

    //        var v1 = parseFloat(t2.html());
    //        t2.html((v1 * v / 1.982).toFixed(3));
    //    }

    //}










    if (lType == 3 || lType == 4) {

        var v1 = (1.96 / 1.982 * rebates).toFixed(3);
        $('.scroll_infok1:eq(10)').find('th').html('赔率:' + v1);
    }

}