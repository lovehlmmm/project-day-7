$(document).ready(function() {
    $('#nav-orders-tab').click(function() {
        GetData();
    });

    $('.showcredit').click(function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/user/GetCreditCard?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    $('#showCardNum').val('**** **** **** ' + response.data.CreditNumber);
                    $('#showCardExpire').val(response.data.Expire);
                    $('#showCardCVC').val(response.data.CVC);
                    $('#creditModal').modal();
                } else {
                    alert('faill');
                }
            }
        });
    });

});