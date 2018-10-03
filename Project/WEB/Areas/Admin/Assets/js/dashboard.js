var page = 5; // start at 6th record (assumes first 5 included in initial view)
var takeCount = 1; // return new 5 records
var hasMoreRecords = true;
$("#order-pending").scroll(function () {
    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
        GetAllNotification();
        takeCount++;
    }
});
$(document).ready(function () {
    $('.pending-table').addClass('loading');
    GetAllNotification();
    takeCount++;
    $('#refresh-pending').click(function () {
        page = 5;
        takeCount = 1;
        $('.order-pending').html("");
        GetAllNotification();
    });
});
function GetAllNotification() {
    if (!hasMoreRecords) {
        return;
    }
    $.ajax({
        url: '/Dashbroad/GetOrderPending',
        data: { num: takeCount, page: page },
        dataType: 'html',
        success: function (data) {
            $('.pending-table').removeClass('loading');
            $('.order-pending').append(data);
        }
    });
}

