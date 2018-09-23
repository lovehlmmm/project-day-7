$('#showCredit').click(function () {
    //$('#modalCredit').modal();
     GetModalCredit();
});


function GetModalCredit() {
    $.ajax({
        url: '/PaymentCheckOut/GetModalCredit',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#modalCredit .modal-body').html(result);
        $('#modalCredit').modal();
        $('#saveChooseCredit').click(function () {
            var checkId = $('input[name=chooseCredit]:checked').data('id');
            if (checkId === undefined) {
                return;
            }
            $.ajax({
                url: '/PaymentCheckOut/GetCredit?id='+checkId,
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.status === true) {
                        $('.creditDetails').text(data.credit.CreditNumber);
                        $('.creditEx').text(data.credit.Expire);
                        $('.creditDetails').data('id', data.credit.CreditCardId);
                        $('#modalCredit').modal('hide');
                    } else {
                        return false;

                    }
                }
            });
        })
    }).error(function (xhr, status) {
    });
}