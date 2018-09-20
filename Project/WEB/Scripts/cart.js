

$(document).ready(function () {
    GetCartItem();
    $('#add-cart').click(function () {
        var product = $('select[name=select-product]').val();

        var list = $('.imgChoiced');
        $.each(list, function (key, val) {
            if ($(val).hasClass('image-checkbox-checked')) {
                var formData = new FormData();
                formData.append("image", $(val).attr('src'));
                formData.append("product",product);
                AddToCart(formData);
            }
        });
        GetCartItem();
    });
});

function AddToCart(data) {
    $.ajax({
        url: '/Upload/AddCart',
        data: data,
        type: 'POST',
        async: false,
        processData: false,
        contentType: false
    }).success(function (result) {

    }).error(function (xhr, status) {
    });
}

function GetCartItem() {
    $.ajax({
        url: '/Upload/GetCartUpload',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#list-cart').html(result);
    }).error(function (xhr, status) {
    });
}
