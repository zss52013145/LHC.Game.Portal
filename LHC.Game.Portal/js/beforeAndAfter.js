//提现
function beginTiXian() {
    var money = $('input[name=money]').val();

    if (!JudgeZS(money)) {
        ShowTanMin('金额必须是正整数');
        return false;
    }
    else if ($('select[name=bank]').val() == 0) {
        ShowTanMin('请选择银行');
        return false;
    }
    else if ($('input[name=payWord]').val() == '') {
        ShowTanMin('请输入取款密码');
        return false;
    }

    return true;
}


function afterTiXian(data) {
    if (data != 'ok') {
        ShowTanMin2(data);
    }
    else {
        ShowTanMin('提交成功');
    }
}

//用户注册
function beginRegisterUserInfo() {

    var username = $("#addUser-userName").val();
    var password = $("#addUser-password").val();
    var payword = $("#addUser-payword").val();
    var realname = $("#addUser-realname").val();
    var mobile = $("#addUser-mobile").val();
    
    var vcode = $('#addUser_vcode').val();

    if (username == "") {
        ShowTanMin('账户不能为空');
        return false;
    }
    else if (username.length < 6 || username.length > 15) {
        ShowTanMin2('账户长度需在6-15位之间');
        return false;
    }
    else if (!LimitDigitAndEng(username)) {
        ShowTanMin2('账户只能包含数字或字母');
        return false;
    }
    else if (!LimitMustDigitAndEng(username)) {
        ShowTanMin2("账户必须同时包含数字和字母");
        return false;
    }
    else if (password == "") {
        ShowTanMin("密码不能为空");
        return false;
    }
    else if (password.length < 6 || password.length > 20) {
        ShowTanMin2("密码长度需在6-20位之间");
        return false;
    }
    else if (realname == "") {
        ShowTanMin("真实姓名不能为空");
        return false;
    }
    else if (realname.length < 2 || realname.length > 5) {
        ShowTanMin2('真实姓名长度需在2-5位之间');
        return false;
    }
    else if (mobile == "") {
        ShowTanMin("手机号码不能为空");
        return false;
    }
    else if (mobile.length != 11) {
        ShowTanMin2('手机号码长度需等于11');
        return false;
    }
    else if (!LimitDigit(mobile)) {
        ShowTanMin2('手机号码需是全数字');
        return false;
    }
    //else if (!LimitMustIsZhongWen(realname)) {
    //    ShowTanMin2("真实姓名必须是中文");
    //    return false;
    //}
    else if (payword == "") {
        ShowTanMin("取款密码不能为空");
        return false;
    }
    else if (payword.length < 6 || payword.length > 20) {
        ShowTanMin2("取款密码长度需在6-20位之间");
        return false;
    }
    else if (vcode == "") {
        ShowTanMin("验证码不能为空");
        return false;
    }
    else if ($(':checkbox:checked').size() == 0) {
        ShowTanMin("请先同意协议");
        return false;
    }
    else {
        ShowLoading();
        return true;
    }
}

function afterRegisterUserInfo(data) {

    CloseLoading();

    if (data == "ok") {
        ShowTanMin("注册成功!");
        setTimeout("location.href = '/home/index';", 1000);
    }
    else {
        ShowTanMin2(data);
        $('.vimg').attr("src", "/home/getvcode?" + Math.random());   //改变验证码
    }
}


//代理申请
function beginApplyProxy() {

    var username = $("input[name=username]").val()

    if (username == "") {
        ShowTanMin('账户不能为空');
        return false;
    }
    else if (username.length < 6 || username.length > 15) {
        ShowTanMin2('账户长度需在6-15位之间');
        return false;
    }
    else if (!LimitDigitAndEng(username)) {
        ShowTanMin2('账户只能包含数字或字母');
        return false;
    }
    else if (!LimitMustDigitAndEng(username)) {
        ShowTanMin2("账户必须同时包含数字和字母");
        return false;
    }
    else {
        ShowLoading();
        return true;
    }
}


function afterApplyProxy(data) {

    CloseLoading();

    if (data != "ok") {
        ShowTanMin(data);
    }
    else {
        ShowTanMin("添加成功");
    }
}



//代理申请
function beginApplyProxy2() {
    if ($("input[name=mobile]").val() == "") {
        ShowTanMin('联系电话不能为空');
        return false;
    }
    else if ($("input[name=qq]").val() == "") {
        ShowTanMin('QQ不能为空');
        return false;
    }
    else if ($("textarea[name=reason]").val() == "") {
        ShowTanMin('申请理由不能为空');
        return false;
    }
    else {
        ShowLoading();
        return true;
    }
}


function afterApplyProxy2(data) {

    CloseLoading();

    if (data != "ok") {
        ShowTanMin(data);
    }
    else {
        ShowTanMin("申请成功");
    }
}



//用户登陆
function beforeLogin() {
    if ($("input[name=username]").val() == "") {
        ShowTanMin('用户名不能为空');
        return false;
    }
    else if ($("input[name=password]").val() == "") {
        ShowTanMin('密码不能为空');
        return false;
    }
    else {
        //ShowLoading();
        return true;
    }
}

function afterLogin(data) {

    //CloseLoading();

    if (data == "error" || data == "dongjie") {
        ShowTanMin('用户名或密码错误');
        //$(".vimg").attr("src", "/home/getvcode?" + Math.random());
    }
    else if (data == "yzmgq") {
        ShowTanMin('验证码已过期');
        //$(".vimg").attr("src", "/home/getvcode?" + Math.random());
    }
    else if (data == "yzmerr") {
        ShowTanMin('验证码错误');
        //$(".vimg").attr("src", "/home/getvcode?" + Math.random());
    }
    else if (data == "ok") {

        ShowTanMin('登录成功');

        setTimeout('location.href = "/home/index";', 2000);
        
    }
}




//修改登陆密码
function beginModifyPwd() {

    var password = $("#newPwd").val();

    var type = $("#type").val();

    if (type == 0) {
        ShowTanMin("请选择密码类型");
        return false;
    }
    else if ($("#oldPwd").val() == "") {
        ShowTanMin("请输入旧密码");
        return false;
    }
    else if (password == "") {
        ShowTanMin("请输入新密码");
        return false;
    }
    else if (password.length < 6 || password.length > 20) {
        ShowTanMin("密码长度需在6-20位之间");
        return false;
    }
        //else if (!/[a-zA-Z0-9]/.test(password)) {
        //    showTanMin("密码只能包含数字或字母");
        //    return false;
        //}
    else if (!checkInputIsAll(password)) {
        ShowTanMin("密码必须同时包含数字和字母");
        return false;
    }
    else if ($("#newPwd2").val() == "") {
        ShowTanMin("请输入确认密码");
        return false;
    }
    else if ($("#newPwd").val() != $("#newPwd2").val()) {
        ShowTanMin("两次输入密码不一致");
        return false;
    }
    else {
        return true;
    }
}

function afterModifyPwd(data) {
    if (data == "pwdErr") {
        ShowTanMin("旧密码错误");
    } else {
        ShowTanMin("修改成功");
    }
}



//完善用户资料
function beforeCompleteUserInfo() {

    var pwd = $("input[name=payWord]").val();

    if ($("input[name=realName]").val() == "") {
        ShowTanMin("请输入真实姓名");
        return false;
    }
    else if (pwd == "") {
        ShowTanMin("请输入取款密码");
        return false;
    }
    else if (!checkInputIsAll(pwd)) {
        ShowTanMin2("密码必须同时包含数字和字母");
        return false;
    }
    else if (pwd.length < 6 || pwd.length > 20) {
        ShowTanMin("密码长度需在6-20");
        return false;
    }
    else {
        return true;
    }
}


function afterCompleteUserInfo() {
    ShowTanMin("修改成功");
    CloseTan('completeUserInfo');
}


//增加银行卡
function beginAddBankInfo() {

    if ($("select[name=bankName]").val() == "选择银行") {
        ShowTanMin("请选择开户行");
        return false;
    }
    else if ($("select[name=provience]").val() == "选择省份") {
        ShowTanMin("请选择省份");
        return false;
    }
    else if ($("select[name=city]").val() == "选择城市") {
        ShowTanMin("请选择城市");
        return false;
    }
    else if ($("input[name=zhiHangName]").val() == "") {
        ShowTanMin("请填写开户网点");
        return false;
    }
    else if ($("input[name=bankNum]").val() == "") {
        ShowTanMin("请填写开户账号");
        return false;
    }

    return true;
}

function afterAddBankInfo(data) {

    if (data != "ok") {
        ShowTanMin(data);
    } else {
        ShowTanMin('添加成功');

        ChangeLocation('#/bank');
        LoadBankData();
    }
}


function afterEditBankInfo(data) {

    if (data != "ok") {
        ShowTanMin(data);
    } else {
        ShowTanMin('修改成功');

        ChangeLocation('#/bank');
        LoadBankData();
    }
}


function beginSendMessage() {

    if ($('select[name=type]').val() == '问题类型') {
        ShowTanMin('请选择问题类型');
        return false;
    }
    else if ($('textarea[name=content]').val() == '') {
        ShowTanMin('请输入内容');
        return false;
    } else {
        return true;
    }
}


function afterSendMessage() {
    ShowTanMin('发送成功');
    setTimeout('LoadNoticeCenter(2)', 500);
}


//---------------------------------------------------------提交前后

function beginSetNick() {
    var nick = $("#newNick").val();
    if (nick == null) {
        showTanMin("不能为空");
        return;
    }
}


function afterSetNick(data) {

    if (data == "repeat") {
        showTanMin("昵称重复，请重新输入");
    }
    else {
        showTanMin("设置成功");
    }
}





//管理员登陆
function beforeAdminLogin() {
    if ($("#admin-username").val() == "") {
        showTanMin("请输入用户名");
        return false;
    }
    else if ($("#admin-password").val() == "") {
        showTanMin("请输入密码");
        return false;
    }
}

function afterAdminLogin(data) {
    if (data == "err") {
        showTanMin("用户名或密码错误");
    } else {
        location.href = "/Admin/Index";
    }
}

//提现
function beginTixian() {
    //提现次数判断
    var times = $("#tixianCount").val();

    if (times <= 0) {
        showTan("min-win", "今天提现次数已满");
        return false;
    }

    //提现银行
    var bank = $("#tixianBank").val();
    if (bank == "") {
        showTan("min-win", "先去安全中心绑定银行卡");
        return false;
    }

    //提现金额
    var money = $("#tixianMoney").val();
    if (money == "") {
        showTan("min-win", "提现金额不能为空");
        return false;
    }
    else if (money < 100) {
        showTan("min-win", "单笔提现不得小于100");
        return false;
    }
    else if (money > 100000) {
        showTan("min-win", "单笔提现不得超过10万");
        return false;
    }
    //资金密码
    var pwd = $("#fundPwd").val();
    if (pwd == "") {
        showTan("min-win", "资金密码不能为空");
        return false;
    }
    else {
        showTan("jdtHanding");
        return true;
    }
}

function afterTixian(data) {
    closeTan("jdtHanding");
    if (data == "timeErr") {
        showTan("min-win", "提款时间为11:00-23:59");
    }
    else if (data == "pwdErr") {
        showTan("min-win", "资金密码错误");
    }
    else if (data == "yuebuzu") {
        showTan("min-win", "余额不足");
    }
    else if (data == "tixiancountbuzu") {
        showTan("min-win", "今天提现次数已用完");
    }
    else if (data == "limitErr") {
        showTan("min-win", "提现额度错误");
    }
    else {
        showTan("min-win", "提交成功！等待客服处理");
    }
}

//充值
function beginRecharge() {
    var m = $(".rechargeMoney").val();
    if (m == "") {
        showTan("min-win", "金额不能为空");
        return false;
    }
    else if (/\D/.test(m)) {
        showTan("min-win", "金额不能为非数字");
        return false;
    } else {
        return true;
    }
}

function afterRecharge() {
    closeTan();
    //打开第二个弹窗
    var bank = $("#bankName").val();
    $("#bankLink").html(bank);
    if (bank == "工商银行") {
        $("#bankNum").html("lishenggne@163.com");
        $("#bankLink").attr("href", "https://mybank.icbc.com.cn/icbc/perbank/index.jsp");
    } else {
        $("#bankNum").html("6217003490000928766");
        $("#bankLink").attr("href", "https://ibsbjstar.ccb.com.cn/app/V5/CN/STY1/login.jsp");
    }
    showTan("chongzhi-ok");
}

//添加用户
function beginAddUserInfo() {

    var userRebate = parseFloat($("#userRebates").val());
    var rebate = parseFloat($("#addUser-rebate").val());

    var _rebate = rebate + "";
    while (_rebate.indexOf('.') != -1) {
        _rebate = _rebate.replace('.', '6');
    }

    var username = $("#addUser-userName").val();
    var password = $("#addUser-password").val();
    var payword = $("#addUser-payword").val();
    var nickname = $("#addUser-nick").val();


    if (username == "") {
        $("#addUserErrMsg").html("账户不能为空").show();
        return false;
    }
    else if (username.length < 6 || username.length > 16) {
        $("#addUserErrMsg").html("账户长度需在6-16位之间").show();
        return false;
    } else if (!/[a-zA-Z0-9]/.test(username)) {
        $("#addUserErrMsg").html("账户只能包含数字或字母").show();
        return false;
    } else if (!checkInputIsAll(username)) {
        $("#addUserErrMsg").html("账户必须同时包含数字和字母").show();
        return false;
    }
    else if (password == "") {
        $("#addUserErrMsg").html("密码不能为空").show();
        return false;
    }
    else if (password.length < 6 || password.length > 16) {
        $("#addUserErrMsg").html("密码长度需在6-16位之间").show();
        return false;
    }
    else if (!/[a-zA-Z0-9]/.test(password)) {
        $("#addUserErrMsg").html("密码只能包含数字或字母").show();
        return false;
    }
    else if (payword == "") {
        $("#addUserErrMsg").html("资金密码不能为空").show();
        return false;
    }
    else if (payword.length < 6 || payword.length > 16) {
        $("#addUserErrMsg").html("资金密码长度需在6-16位之间").show();
        return false;
    }
        //else if (!checkInputIsAll(password)) {
        //    $("#addUserErrMsg").html("密码必须同时包含数字和字母").show();
        //    return false;
        //}
        //else if ($("#addUser-password").val() != $("#addUser-password2").val()) {
        //    $("#addUserErrMsg").html("两次密码不一致").show();
        //    return false;
        //}
    else if ($("#addUser-rebate").val() == "") {
        $("#addUserErrMsg").html("返点不能为空").show();
        return false;
    } else if (/\D/.test(_rebate)) {
        $("#addUserErrMsg").html("返点不能包含非数字").show();
        return false;
    }
    else if (rebate != 12.8 && rebate != 0 && rebate == userRebate) {
        $("#addUserErrMsg").html("返点不能等于上级").show();
        return false;
    }
    else if (rebate > userRebate) {
        $("#addUserErrMsg").html("返点不能大于上级").show();
        return false;
    }
    else if (nickname == "") {
        $("#addUserErrMsg").html("昵称不能为空").show();
        return false;
    }
    else {
        $("#addUserErrMsg").hide();
        showTan("jdtHanding");
        return true;
    }
}

function afterAddUserInfo(data) {
    closeTan("jdtHanding");
    if (data != "ok") {
        $("#addUserErrMsg").html(data).show();
    } else {
        showTanMin("添加成功");
        setTimeout(function () {
            closeTan("min-win");
            closeTan("add-user");
            $.post("/Lottery/UserManager", function (data2) {
                $("#user-list").html(data2);
            });
        }, 1000);
    }
}



//修改用户
function beginModifyUserInfo() {
    var editRebate = $("#edit-rebate").val();
    var userRebate = $("#userRabate").val();

    //处理小数点 
    var _rebate = editRebate + "";
    while (_rebate.indexOf('.') != -1) {
        _rebate = _rebate.replace('.', '6');
    }

    if (editRebate == "") {
        $("#errMsg").html("返点不能为空");
        return false;
    }
    else if (/\D/.test(_rebate)) {
        $("#errMsg").html("返点不能为非数字");
        return false;
    }
    else if (editRebate != 12.8 && editRebate != 0 && editRebate == userRebate) {
        $("#errMsg").html("返点必须小于上级");
        return false;
    }
    else if (editRebate > userRebate) {
        $("#errMsg").html("返点必须小于上级");
        return false;
    } else {
        showTan("jdtHanding");
        return true;
    }
}

function afterModifyUserInfo(data) {
    closeTan("jdtHanding");
    if (data != "ok") {
        $("#errMsg").html(data);
    } else {
        showTanMin("修改成功");
        setTimeout(function () {
            closeTan("min-win");
            closeTan("edit-user");
            $.post("/Lottery/UserManager", function (data) {
                $("#user-list").html(data);
            });
        }, 1000);

    }
}


//修改支付密码
function beginModifyPayPwd() {
    var password = $("#newPayWord").val();
    if ($("#oldPayWord").val() == "") {
        showTanMin("请输入旧密码");
        return false;
    }
    else if (password == "") {
        showTanMin("请输入新密码");
        return false;
    }
    else if (password.length < 6 || password.length > 16) {
        showTanMin("密码长度需在6-16位之间");
        return false;
    } else if (!/[a-zA-Z0-9]/.test(password)) {
        showTanMin("密码只能包含数字或字母");
        return false;
    } else if (!checkInputIsAll(password)) {
        showTanMin("密码必须同时包含数字和字母");
        return false;
    }
    else if ($("#newPayWord2").val() == "") {
        showTanMin("请输入确认密码");
        return false;
    }
    else if (password != $("#newPayWord2").val()) {
        showTanMin("两次输入密码不一致");
        return false;
    } else {
        return true;
    }
}

function afterModifyPayPwd(data) {
    if (data == "pwdErr") {
        showTanMin("旧密码错误");
    } else {
        showTanMin("修改成功");
    }
}

//设置支付密码
function beginSetPayPwd() {
    if ($("#bankInfo-fundPwd").val() == "") {
        showTanMin("请输入资金密码");
        return false;
    } else {
        return true;
    }
}

function afterSetPayPwd(data) {
    if (data == "pwdErr") {
        showTanMin("资金密码不正确请重新输入");
    } else {
        //密码验证对了 
        //加载银行卡信息
        $.post("/Lottery/BankList", function (data2) {
            $("#bankInfo-list").html(data2);
        });
        $(".bankInfo-second").show();
    }
}



//申请上庄
function beforeApplySZ() {

    //showTanMin("上庄功能维护中");
    //return false;

    if ($(":radio:checked[name=szType]").size() == 0) {
        showTanMin("请选择上庄类型");
        return false;
    }
    else if ($(".limtMoney:visible :radio:checked").size() == 0) {
        showTanMin("请选择上庄额度");
        return false;
    } else {
        return true;
    }
}

function afterApplySZ(data) {
    if (data == "weihu") {
        showTanMin("上庄功能维护中");
    }
    else if (data == "nobank") {
        showTanMin("请先在账户管理下面的银行资料里绑定银行资料");
    }
    else if (data == "onlyOne") {
        showTanMin("每个用户只能同时坐一个庄");
    }
    else if (data == "xuni") {
        showTanMin("该账号不能使用上庄功能");
    }
    else if (data == "yuebuzu") {
        showTanMin("余额不足");
    }
    else if (data == "dataErr") {
        showTanMin("数据错误");
    }
    else if (data == "repeat") {
        showTanMin("同类型的庄请勿重复申请");
    }
    else {

        if (data == 1) {
            showTanMin("上庄申请成功");
        } else {
            showTanMin("上庄排队中");
        }
    }
}

