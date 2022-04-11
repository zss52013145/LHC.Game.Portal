//错误弹窗
function ShowTanMin(msg) {
    $('.message__content').html(msg);
    $('.notification').fadeIn(500);

    setTimeout("$('.notification').fadeOut(500);", 2000);
}


//错误弹窗2
function ShowTanMin2(msg) {
    $('.el-message__content').html(msg);
    $('.notification2').fadeIn(500);

    setTimeout("$('.notification2').fadeOut(500);", 2000);
}
