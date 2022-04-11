﻿
var opts2 = {
    lines: 12 // The number of lines to draw
var opts3 = {
    lines: 12 // The number of lines to draw



//----------------------------弹窗相关


$(function() {

    BindbtnCancelClick();   //弹窗里面的 关闭按钮

    BindloginOutClick();            //退出登录

    BindconfirmBtnCLick();          //确认弹窗的 确定按钮

    BindconfirmZzClick();             //转账弹窗 确认按钮

    BindbtnCancelForCompleteInfoClick();            //添加银行卡  完善个人资料 点取消

});


function BindbtnCancelForCompleteInfoClick() {
    $('.btnCancelForCompleteInfo').live('click',function () {
        
        //返回 上一个页面
        ChangeLocation('#/bank');
        LoadBankData();
    });
}


function BindconfirmZzClick() {

    document.getElementById('confirmZz').addEventListener('tap', function () {
        LoadtransferData2();
    });
}




function BindconfirmBtnCLick() {
    
    document.getElementById('confirmBtn').addEventListener('tap', function () {

        ShowLoading();
        var about = $(this).attr('about');

        if (about == 'login') {
            $.post('/Home/LoginOut', function() {
                location.href = '/home/index'; //刷新
            });
        }
        else if (about == 'delHasReadMsg') {
            $.post('/Home/DelHasReadMsg', function () {
                ShowTanMin('删除成功');
                setTimeout('LoadNoticeCenter(2)', 500);
            });
        }
        else if (about == 'yijianzhuanzhang') {
            var cid = $(this).attr('cid');
            $.post('/Home/QuickTransferMoney', { cid: cid }, function () {
                ShowTanMin('转入成功');
                setTimeout('LoadquickTransferData();', 500);
            });
        }

        CloseLoading();
    });
}


function BindloginOutClick() {

    
    if (!JudgeIDExist('loginOut')) return;
    
    document.getElementById('loginOut').addEventListener('tap', function() {
        ShowTanConfirm('您确定退出该帐号吗?', 'login');
    });

    //$('header').on('tap','.loginOut',function (){
    //    ShowTanConfirm('您确定退出该帐号吗?', 'login');
    //});
}


function BindbtnCancelClick() {

    //$('div[win]').on('tap', '.btnCancel', function () {
    $('.btnCancel').click(function() {
        $(this).parents('div[win]').hide();
    });
}