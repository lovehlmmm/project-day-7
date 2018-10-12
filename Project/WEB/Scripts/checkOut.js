

$('#showAddress').click(function () {
    GetModalAddress();
});
$('#newAddress').on('click', function () {
    $('#form-new-address').submit();

});

$('#saveChooseAddress').click(function () {
    $('input[name=chooseAddress]:checked').data('id');

});

$('#addphone').click(function () {
    var addphone = $('input[name=txtaddphone]').val();
    var phoneno = /(09|01[2|6|8|9])+([0-9]{8})\b/g
    if (addphone.match(phoneno)) {
        $('.showPhone').text(addphone);
        return true;
    }
    else {
        swal("Error", "Wrong number of phone!", "error");
        return false;
    }
   

});
$(document).ready(function () {
    $('#paymentcheckout').click(function () {
        var addressId = $('.addressDetails').data('id');
        var phone = $('.showPhone').text().trim();
        var data = new FormData();
        data.append("addressId", addressId);
        data.append("phone", phone);
        CheckOut(data);
    });
});
function CheckOut(data) {
    $.ajax({
        url: '/CheckOut/Confirm',
        type: 'POST',
        data: data,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            window.location.replace('/paymentcheckout');
        } else {
            swal("Warning", result.message, "warning");
        }
    }).error(function (xhr, status) {
    });
}
$('.deleteitem').click(function () {
    var deleteId = $(this).data('id');
    var item = $(this);
    $('#loading').show();
    $.ajax({
        url: '/Upload/DeleteItem?id=' + deleteId,
        type: 'GET',
        async: false,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            $('#loading').hide();
            $(item).closest('tr').remove();
            $('.count_cart').text(result.count);
            swal("Success", "Delete item success!", "success");
        }
    }).error(function (xhr, status) {
    });
});

function GetModalAddress() {
    $.ajax({
        url: '/CheckOut/GetModalAddress',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#addAddress').show();
        $('modalAddress h5').text("Add Address")
        $('#modalAddress .modal-body').html(result);
        $('#modalAddress').modal();
        $('#saveChooseAddress').click(function () {
            var checkId = $('input[name=chooseAddress]:checked').data('id');
            if (checkId === undefined) {
                return;
            }
            $.ajax({
                url: '/CheckOut/GetAddress?id=' + checkId,
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.status === true) {
                        $('.addressDetails').text(data.address.AddressDetails);
                        $('.addressDetails').data('id', data.address.AddressId);
                        $('#modalAddress').modal('hide');
                    } else {
                        return false;

                    }
                }
            });
        })
    }).error(function (xhr, status) {
    });
}

function GetModalAddAddress() {
    $.ajax({
        url: '/CheckOut/GetModalAddAddress',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#addAddress').hide();
        $('#modalAddress h5').text("Add a new receiving address")
        $('#modalAddress .modal-body').html(result);
        $('#form-new-address').validate({
            rules: {
                newaddress: {
                    required: true
                }
            }, submitHandler: function (form) {
                var address = $('input[name=newaddress]').val();
                var data = new FormData();
                data.append('address', address);
                $.ajax({
                    url: '/CheckOut/AddAddress',
                    type: 'POST',
                    data: data,
                    async: false,
                    processData: false,
                    contentType: false
                }).success(function (result) {
                    if (result.status) {
                        GetModalAddress();
                    }
                }).error(function (xhr, status) {
                });
                return false;  // blocks regular submit since you have ajax
            }
        })
    }).error(function (xhr, status) {
    });
}