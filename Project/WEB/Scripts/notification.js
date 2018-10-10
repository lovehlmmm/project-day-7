﻿var page = 5; // start at 6th record (assumes first 5 included in initial view)
var takeCount = 1; // return new 5 records
var hasMoreRecords = true;
$("#notification").click(function () {
    GetAllNotification();
    takeCount++;
    $('.count-notification-load').text($('.notification-box').length);
});
$("#showmenu").scroll(function () {
    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
        GetAllNotification();
        takeCount++;
        $('.count-notification-load').text($('.notification-box').length);
    }
});
hub.client.broadcaastNotif = function (noti, count) {
    GetAllNotification();
};
function GetAllNotification() {
    if (!hasMoreRecords) {
        return;
    }
    $.ajax({
        url: '/Notification/GetNotification',
        data: { page: page, takeCount: takeCount },
        dataType: 'html',
        success: function (data) {
            $('#showmenu').append(data);
            $('.notification-box').click(function () {
                window.location.href = '' + $(this).data('url');
            });
        }
    });
}

function  ReadNotification(id) {
    
}