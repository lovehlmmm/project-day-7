


$(document).ready(function () {
    GetCartItem();
    $('#add-cart').click(function () {
        var product = $('select[name=select-product]').val();
        var material = $('select[name=select-material]').val();
        var option = $('select[name=select-option]').val();
        var list = $('.image-checkbox-checked');
        if (list.length === 0) {
            swal("Warning", "Please choose your images", "error");
            return;
        }
        if (product === '0') {
            swal("Warning", "Please choose size", "error");
            return;
        }
        if (material === '0') {
            swal("Warning", "Please choose material", "error");
            return;
        }
        var quantity = $('#quantity').val();
        if (quantity < 1) {
            swal("Warning", "Please enter a number greater than 0", "error");
            return;
        }
        $.each(list, function (key, val) {
            var cartItem = { Image: $(val).attr('src'),ImageTitle: $(val).attr('title'), Quantity: quantity};
            var formData = new FormData();
            formData.append('item', JSON.stringify(cartItem));
            formData.append("productId", product);
            formData.append("materialId", material);
            formData.append("option", option);
            AddToCart(formData);
        });
        swal("Success", "Add Success", "success");
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
