// javascript Document
$(function () {

    /******************** 注册 js ********************/
    /*注册隐藏显示密码*/
    $(".has_theme_conyc").click(function () {
        $(this).toggleClass("conyc_current");
    });

    /******************** 彩票游戏 js ********************/
    //alert($(document).height());
    //var gao = $(window).height()-125;
    //alert(gao);
    //$(".dfh_ionic_scroll").height(gao);
    /*即时注单*/
    //$(document).click(function(event) {
    //	if($(event.target).is(".dfh_cq_tu")) 
    //	{
    //		$(".dfh_cq_rul").toggle();
    //	}
    //	else{
    //		$(".dfh_cq_rul").hide();
    //	}
    //});

    /******************** 重庆时时彩 js ********************/
    /*即时注单*/
    //$(document).click(function(event) {
    //	if($(event.target).is(".dfh_if1rtu")) 
    //	{
    //		$(".dfh_clrt_if1ul").toggle();
    //	}
    //	else{
    //		$(".dfh_clrt_if1ul").hide();
    //	}
    //});



    /******************** 信息中心 js ********************/
    $(".Information_nav li").click(function () {
        var IF = $(this).index();
        $(this).addClass("current").siblings().removeClass("current");
        $(".Information_text .Information_info").eq(IF).show().siblings().hide();
    });


    /******************** 路珠 js ********************/
    $(".luzhu_nav li").click(function () {
        var lzzhi = $(this).index();
        $(this).addClass("current").siblings().removeClass("current");
        $(this).parent().parent().parent().siblings(".luzhu_kcon").children(".kcon_info").eq(lzzhi).show().siblings().hide();
    });






    //------------------------------------------------------
    /*mui(".ifk1_tb").on('tap', 'td', function (event) {
        this.click();
    });
    
    mui(".mui_scroll_nav").on('tap', 'li', function (event) {
        this.click();
    });
    
    mui("body").on('tap', 'h3', function (event) {
        this.click();
    });*/



    /******************** 资金管理 js ********************/
    



});



