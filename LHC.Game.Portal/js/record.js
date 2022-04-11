

$(function () {


    //var email = 'sdfdsfsd@qq.com';

    //alert(email.indexOf('@'));



    var href = location.href.toLowerCase();

    if (href.indexOf('lottery') != -1) {
        BindbetRecordClick2();

        if (href.indexOf('#/week') != -1) {
            LoadWeekData2();
        }
        else if (href.indexOf('#/daydetail') != -1) {
            LoadDayDetailData();
        }
        else if (href.indexOf('#/day') != -1) {
            LoadDayData2();
        }
    }
    else {
        BindbetRecordClick();

        if (href.indexOf('#/week') != -1) {
            LoadWeekData();
        }
        else if (href.indexOf('#/day') != -1) {
            LoadDayData();
        }
        else if (href.indexOf('#/rptgame') != -1) {
            LoadRPTGameData();
        }
        else if (href.indexOf('#/rptdetail') != -1) {
            LoadRPTDetailData();
        }
        else if (href.indexOf('#/center') != -1) {
            LoadUserCenter();
        }
        else if (href.indexOf('#/info') != -1) {
            LoadUserInfo();
        }
        else if (href.indexOf('#/apply') != -1) {
            LoadProxyApply();
        }
        else if (href.indexOf('#/proxy') != -1) {
            //LoadProxyApply();
            LoadTeamMana();
        }
        else if (href.indexOf('#/addnext') != -1) {
            LoadAddNext();
        }
        else if (href.indexOf('#/linkregister') != -1) {
            LoadLinkRegister();
        }
        else if (href.indexOf('#/addlink') != -1) {
            LoadAddLink();
        }
        else if (href.indexOf('#/nextmana') != -1) {
            LoadNextMana();
        }
        else if (href.indexOf('#/nextrecharge') != -1) {
            LoadNextRecharge();
        }
        else if (href.indexOf('#/nextprofitloss') != -1) {
            LoadNextProfitLoss();
        }
        else if (href.indexOf('#/nextbet') != -1) {
            LoadNextBet();
        }
        else if (href.indexOf('#/nextbetdetail') != -1) {
            LoadNextBetDetail();
        }
        else if (href.indexOf('#/nextusers') != -1) {
            LoadNextUsers();
        }
        else if (href.indexOf('#/gereninfo') != -1) {
            LoadGeRenInfo();
        }
        else if (href.indexOf('#/uprebate') != -1) {
            LoadUpRebate();
        }
        else if (href.indexOf('#/teamrecharge') != -1) {
            LoadTeamRecharge();
        }
        else if (href.indexOf('#/teamreport') != -1) {
            LoadTeamReport();
        }
        else if (href.indexOf('#/teamtixian') != -1) {
            LoadTeamTixian();
        }
        else if (href.indexOf('#/overview') != -1) {
            LoadOverview();
        }
        else if (href.indexOf('#/teambetdetail') != -1) {
            LoadTeamBetDetail();
        }
        else if (href.indexOf('#/teamprofitloss') != -1) {
            LoadTeamProfitLoss();
        }
        else if (href.indexOf('#/teambet') != -1) {
            LoadTeamBet();
        }
        else if (href.indexOf('#/notice') != -1) {
            LoadNoticeCenter();
        }
        else if (href.indexOf('#/sendmsg') != -1) {
            LoadSendMsgData();
        }
        else if (href.indexOf('#/mdfpwd') != -1) {
            LoadModifyPwd();
        }
        else if (href.indexOf('#/bank') != -1) {
            LoadBankData();
        }
        else if (href.indexOf('#/addbank') != -1) {
            LoadAddBankData();
        }
        else if (href.indexOf('#/editbank') != -1) {
            LoadBankData();
        }
        else if (href.indexOf('#/transfer') != -1) {
            LoadtransferData();
        }
        else if (href.indexOf('#/quicktransfer') != -1) {
            LoadquickTransferData();
        }
        else if (href.indexOf('#/fund') != -1) {
            LoadFundData();
        }
        else if (href.indexOf('#/recharge') != -1) {
            LoadRechargeData();
        }
        else if (href.indexOf('#/activity') != -1) {
            LoadActivityData();
        }
        else if (href.indexOf('#/rechargesd') != -1) {
            LoadRechargeSDData();
        }
        else if (href.indexOf('#/rechargeauto') != -1) {
            //LoadRechargeAutoData();
            LoadFundData();
        }
        else if (href.indexOf('#/profitloss') != -1) {

            LoadProfitLossData();
        }




    }





    BindjszdCLick();   //即时注单

    BindopenResultCLick();  //开奖结果


    BindkjjgLTypeChange();          //开奖结果中的 彩种切换

    BindkjjgDateChange();           //开奖结果中的 日期切换

    BinduCenterCLick();                //点击用户名


    BindlmchanglongCLick();            //两面长龙

    BindChangLongTypeChange();              //两面长龙中的  彩种切换

    BindbetLimitClick();                   //投注限额

    BindbetLimitlTypeChange();              //投注限额 切换彩种

    BindtransferClick();               //首页转账

    Bindtransfer2Click();               //List页面的 转账

    BindfundClick();                    //资金管理

    BindRechargeClick();                //充值

    BindActivityClick();                //优惠活动

    BindconfirmBtnClick();              //弹窗的确认按钮

});


function Bindtransfer2Click() {

    if (!JudgeIDExist('transfer2')) return;

    document.getElementById('transfer2').addEventListener('tap', function () {

        LoadtransferData2();
    });
}

function BindtransferClick() {

    if (!JudgeIDExist('transfer')) return;

    document.getElementById('transfer').addEventListener('tap', function () {

        ChangeLocation('#/transfer');
        transferFrom = 1;       //标志从哪里跳转过去的
        LoadtransferData();

    });
}

function BindfundClick() {

    if (!JudgeIDExist('fund')) return;

    document.getElementById('fund').addEventListener('tap', function () {

        if (JudgeIsTry()) return;   //判断试玩账号

        ChangeLocation('#/fund');
        fundFrom = 1;       //标志从哪里跳转过去的
        LoadFundData();

    });
}


function BindRechargeClick() {

    if (!JudgeIDExist('recharge')) return;

    document.getElementById('recharge').addEventListener('tap', function () {

        if (JudgeIsTry()) return;   //判断试玩账号

        ChangeLocation('#/recharge');
        rechargeFrom = 1;       //标志从哪里跳转过去的
        LoadRechargeData();

    });
}


function BindActivityClick() {

    if (!JudgeIDExist('activity')) return;

    document.getElementById('activity').addEventListener('tap', function () {

        //if (JudgeIsTry()) return;   //判断试玩账号

        ChangeLocation('#/activity');
        rechargeFrom = 1;       //标志从哪里跳转过去的
        LoadActivityData();

    });
}




function BindbetLimitlTypeChange() {
    $('#betLimitlType').live('change', function () {
        var cid = $(this).val();
        $('.KJJG_content[lType=' + cid + ']').show().siblings().hide();
    });
}

function BindbetLimitClick() {

    if (!JudgeIDExist('betLimit')) return;

    document.getElementById('betLimit').addEventListener('tap', function () {

        LoadbetLimitData();
    });
}

function BindChangLongTypeChange() {

    $('#changLongType').live('change', function () {
        var t = $(this).val();
        LoadChangLongData(t);
    });
}

function BindlmchanglongCLick() {

    if (!JudgeIDExist('lmchanglong')) return;

    document.getElementById('lmchanglong').addEventListener('tap', function () {

        LoadChangLongData(lType);
    });
}

function BinduCenterCLick() {

    if (!JudgeIDExist('uCenter')) return;

    document.getElementById('uCenter').addEventListener('tap', function () {

        ChangeLocation('#/center');
        LoadUserCenter();
    });
}

function BindopenResultCLick() {

    if (!JudgeIDExist('openResult')) return;

    document.getElementById('openResult').addEventListener('tap', function () {

        LoadOpenData(lType);
    });
}

function BindkjjgLTypeChange() {

    $('#kjjgLType').live('change', function () {
        //彩种
        var type = $(this).val();
        //日期
        var date = $('#kjjgDate').val();

        LoadOpenData(type, date);
    });
}

function BindkjjgDateChange() {

    $('#kjjgDate').live('change', function () {

        //彩种
        var type = $('#kjjgLType').val();
        //日期
        var date = $(this).val();

        LoadOpenData(type, date);
    });
}

function BindjszdCLick() {

    if (!JudgeIDExist('jszd')) return;

    document.getElementById('jszd').addEventListener('tap', function () {

        LoadJSZDData();
    });
}

function BindbetRecordClick() {

    if (!JudgeIDExist('betRecord')) return;

    document.getElementById('betRecord').addEventListener('tap', function () {

        if (JudgeIsTry()) return;   //判断试玩账号

        ChangeLocation('#/week');
        bettingRecordFrom = 1;          //标志从哪里跳转过去的  
        LoadWeekData();
    });
}

function BindbetRecordClick2() {

    if (!JudgeIDExist('betRcd')) return;

    document.getElementById('betRcd').addEventListener('tap', function () {

        ChangeLocation('#/week');

        LoadWeekData2();
    });

}


function LoadCunQuData() {
    $.post('/Record/CunQuRecord', function (data) {
        CloseLoading();
        $('.GL_Tinfo:eq(1)').find('.TK_info:eq(0)').html(data);
    });
}


function LoadZhuanZhangData() {
    $.post('/Record/ZhuanZhangRecord', function (data) {
        CloseLoading();
        $('.GL_Tinfo:eq(1)').find('.TK_info:eq(1)').html(data);
    });
}

function LoadZengJinFanDianData() {

    $.post('/Record/ZengJinFanDianRecord', function (data) {
        CloseLoading();
        $('.GL_Tinfo:eq(1)').find('.TK_info:eq(2)').html(data);
    });

}

function LoadquickTransferData() {
    $.post('/Home/QuickTransfer', function (data) {
        $('#quickTransferPage').html(data).show().siblings().hide();
    });
}

function LoadtransferData2() {
    $.post('/Lottery/Transfer', function (data) {
        $('#transferPage').html(data).show().siblings().hide();
    });
}

function LoadFundData(isRecord) {

    $.post('/Home/Fund', function (data) {

        if (JudgeReturnIsLogin(data)) { //返回Login
            return;
        }

        $('#fundPage').html(data).show().siblings().hide();



        //特殊情况- 从手动充值返回
        if (isRecord) {

            $('.GL_nav li').eq(1).addClass('current').siblings().removeClass('current');
            $('.GL_Tinfo').eq(1).show().siblings().hide();
            LoadCunQuData();
        }
        else {

            CheckBankInfo();
            CheckLiuShui();
        }


    });
}


function LoadRechargeData() {

    $.post('/Home/RechargePage', function (data) {

        if (JudgeReturnIsLogin(data)) { //返回Login
            return;
        }

        $('#rechargePage').html(data).show().siblings().hide();

        //InitBank(3);

    });
}






function LoadProfitLossData(time1, time2) {

    $.post('/Record/ProfitLossPage', { date1: time1, date2: time2 }, function (data) {

        $('#activityPage').html(data).show().siblings().hide();


        //InitBank(3);

    });
}


function LoadActivityData() {
    $.post('/Home/ActivityPage', function (data) {

        //if (JudgeReturnIsLogin(data)) { //返回Login
        //    return;
        //}

        $('#activityPage').html(data).show().siblings().hide();
    });
}


function LoadActivityDetailData(acId) {
    $.post('/Activity/Activity' + acId, function (data) {

        $('#activityDetailPage').html(data).show().siblings().hide();
    });
}







function LoadtransferData() {

    ShowLoading();
    $.post('/Home/Transfer', function (data) {

        CloseLoading();
        JudgeReturnIsLogin(data);   //判断登录

        $('#transferPage').html(data).show().siblings().hide();
    });
}


function LoadbetLimitData() {
    $.post('/Record/BetLimit', function (data) {
        $('#betLimitPage').html(data).show().siblings().hide();
    });
}

function LoadSendMsgData() {
    $.get('/Home/SendMsg', function (data) {
        $('#sendMsgPage').html(data).show().siblings().hide();
    });
}

function LoadNoticeDetailData(cid, type) {
    $.post('/Home/NoticeDetail', { cid: cid, type: type }, function (data) {
        $('#noticeDetailPage').html(data).show().siblings().hide();
    });
}

function LoadEditBankData(cid) {
    $.post('/Home/EditBank', { cid: cid }, function (data) {
        $('#editBankPage').html(data).show().siblings().hide();
    });
}

function LoadAddBankData() {
    $.post('/Home/AddBank', function (data) {

        $('#addBankPage').html(data).show().siblings().hide();
    });
}

function LoadChangLongData(lType) {
    ShowLoading();
    $.post('/Record/TwoAsideChangLong', { lType: lType }, function (data) {
        $('#changLongPage').html(data).show().siblings().hide();
        CloseLoading();
    });
}

function LoadBankData() {

    $.post('/Home/BankInfo', function (data) {

        if (data == 'norealname') {
            ShowTanMin2('请先在个人资料绑定真实姓名');
        }
        else {
            $('#bankInfo').html(data).show().siblings().hide();
        }


    });
}


//LoadRechargeSDData
function LoadRechargeSDData() {

    $.post('/Home/RechargeSDPage', function (data) {
        $('#rechargeSDPage').html(data).show().siblings().hide();
    });
}

function LoadRechargeAutoData() {
    $.post('/Home/RechargeAutoPage', function (data) {

        $('#rechargeAutoPage').html(data).show().siblings().hide();

        //更换标头
        $('#rechargeTitle').html(GetRechargeTypeName(rechargeType));

        //更换限额
        $('#limit1').html(limit1);
        $('#limit2').html(limit2);

    });
}





function LoadModifyPwd() {

    $.post('/Home/ModifyPwd', function (data) {
        $('#modifyPwd').html(data).show().siblings().hide();
    });
}

function LoadUserInfo() {

    $.post('/Home/Info', function (data) {
        $('#userInfo').html(data).show().siblings().hide();
    });
}

function LoadNextMana(cid) {

    $.post('/Home/NextMana', { cid: cid }, function (data) {
        $('#userInfo').html(data).show().siblings().hide();
    });
}


function LoadTeamMana() {

    $.post('/Home/TeamMana', function (data) {

        if (JudgeReturnIsLogin(data)) { //返回Login
            return;
        }

        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadProxyApply() {

    $.post('/Home/ProxyApply', function (data) {

        if (JudgeReturnIsLogin(data)) { //返回Login
            return;
        }

        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadTeamReport(userId, userType, time1, time2) {

    $.post('/Team/TeamReport', { userId: userId, userType: userType, date1: time1, date2: time2 }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadNextRecharge(cid) {

    $.post('/Home/NextRecharge', { cid: cid }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadNextProfitLoss(cid, type, time1, time2) {

    $.post('/Team/NextProfitLoss', { cid: cid, type: type, date1: time1, date2: time2 }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadNextBet(cid, type, time1, time2) {

    $.post('/Team/NextBet', { cid: cid, lType: type, date1: time1, date2: time2 }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadNextBetDetail(cid) {

    $.post('/Team/NextBetDetail', { cid: cid }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadNextUsers(userId) {

    $.post('/Record/NextUsers', { userId: userId }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadNextUsers2(time1, time2, key) {

    $.post('/Record/NextUsers', { date1: time1, date2: time2, key: key }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}



function LoadGeRenInfo(cid) {

    $.post('/Team/GeRenInfo', { cid: cid }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadUpRebate(cid) {

    $.post('/Team/UpRebate', { cid: cid }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}




function LoadOverview() {

    $.post('/Team/Overview', function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadTeamProfitLoss(type, time1, time2, username) {

    $.post('/Team/TeamProfitLoss', { type: type, date1: time1, date2: time2, key: username }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadTeamBet(lType, time1, time2, username) {

    $.post('/Team/TeamBet', { lType: lType, date1: time1, date2: time2, key: username }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadTeamBetDetail(cid) {

    $.post('/Team/TeamBetDetail', { cid: cid }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}





function LoadTeamRecharge(state, time1, time2, username) {

    $.post('/Team/TeamRecharge', { state: state, date1: time1, date2: time2, key: username }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadTeamTixian(state, time1, time2, username) {

    $.post('/Team/TeamTixian', { state: state, date1: time1, date2: time2, key: username }, function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadAddNext() {

    $.post('/Home/AddNext', function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadLinkRegister() {

    $.post('/Team/LinkRegisterPage', function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}


function LoadAddLink() {

    $.post('/Team/AddLinkPage', function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}




function LoadUserCenter() {

    $.post('/Home/Center', function (data) {
        $('#userCenter').html(data).show().siblings().hide();
    });
}

function LoadNoticeCenter(cid) {

    $.post('/Home/NoticeCenter', function (data) {
        $('#noticePage').html(data).show().siblings().hide();

        if (cid == 2) {
            $(".Information_nav li:eq(1)").addClass('current').siblings().removeClass('current');
            $(".Information_text .Information_info").eq(1).show().siblings().hide();
            $('.Information_dela').show();
        }
    });
}


function LoadJSZDData() {
    ShowLoading();
    $.post('/Lottery/JSZD', function (data) {
        $('#jszdPage').html(data).show().siblings().hide();
        CloseLoading();
    });
}

function LoadJSZDDetailData(lType) {
    ShowLoading();
    $.post('/Lottery/JSZDDetail', { lType: lType }, function (data) {
        $('#jszdDetailPage').html(data).show().siblings().hide();
        CloseLoading();
    });
}

function LoadWeekData(isHome) {
    ShowLoading();
    $.post('/Record/Week', function (data) {
        CloseLoading();


        if (JudgeReturnIsLogin(data)) { //返回Login
            return;
        }

        $('#weekData').html(data).show().siblings().hide();

        //标识  从主页跳转过来的 还是个人中心跳转过来的
        if (isHome != undefined) {
            $('#weekBack').attr('isHome', isHome);
        }
    });
}

function LoadWeekData2() {
    ShowLoading();
    $.post('/Record/Week2', function (data) {
        CloseLoading();

        $('#weekData2').html(data).show().siblings().hide();
    });
}

function LoadDayData(date) {
    ShowLoading();
    $.post('/Record/Day', { date: date }, function (data) {
        CloseLoading();
        $('#dayData').html(data);
    });

    $('#dayData').show().siblings().hide();
}

function LoadDayData2(date) {
    ShowLoading();
    $.post('/Record/Day2', { date: date }, function (data) {
        CloseLoading();
        $('#dayData2').html(data).show().siblings().hide();
    });
}

function LoadRPTGameData(date) {
    ShowLoading();
    $.post('/Record/RPTGame', { date: date }, function (data) {
        CloseLoading();
        $('#rptgameData').html(data);
    });

    $('#rptgameData').show().siblings().hide();
}

function LoadRPTDetailData(lType, date) {
    ShowLoading();
    $.post('/Record/RPTDetail', { lType: lType, date: date }, function (data) {
        CloseLoading();
        $('#rptdetailData').html(data);
    });

    $('#rptdetailData').show().siblings().hide();
}

function LoadDayDetailData(lType, date) {
    ShowLoading();
    $.post('/Record/DayDetail', { lType: lType, date: date }, function (data) {
        $('#dayDetailData').html(data).show().siblings().hide();
        CloseLoading();
    });
}

function LoadOpenData(lType, date) {
    ShowLoading();
    $.post('/Record/OpenRecord', { lType: lType, date: date }, function (data) {
        $('#kjjgPage').html(data).show().siblings().hide();
        CloseLoading();
    });
}



//确认按钮
function BindconfirmBtnClick() {
    $('.confirmBtn').live('click', function () {

        if (confirmEvent == 'nextUsers') {

            var time1 = $('.time1').val();
            var time2 = $('.time2').val();
            var username = $('#username').val();

            LoadNextUsers2(time1, time2, username);
        }
        else if (confirmEvent == 'teamBet') {

            var time1 = $('.time1').val();
            var time2 = $('.time2').val();
            var username = $('#username').val();
            var lTypeForSea = $('#lType').val();


            LoadTeamBet(lTypeForSea, time1, time2, username);
        }
        else if (confirmEvent == 'teamProfitLoss') {

            var time1 = $('.time1').val();
            var time2 = $('.time2').val();
            var username = $('#username').val();
            var type = $('#type').val();


            LoadTeamProfitLoss(type, time1, time2, username);
        }
        else if (confirmEvent == 'teamRecharge') {

            var time1 = $('.time1').val();
            var time2 = $('.time2').val();
            var username = $('#username').val();
            var state = $('#state').val();


            LoadTeamRecharge(state, time1, time2, username);
        }
        else if (confirmEvent == 'teamTiXian') {

            var time1 = $('.time1').val();
            var time2 = $('.time2').val();
            var username = $('#username').val();
            var state = $('#state').val();

            LoadTeamTixian(state, time1, time2, username);
        }


    });
}
