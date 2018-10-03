 
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
        //$('#loading').show();
        var ajaxTime = new Date().getTime();
        var notice = new PNotify({
            text: "Please Wait",
            type: 'info',
            icon: 'fa fa-spinner fa-spin',
            hide: false,
            delay:8000,
            buttons: {
                closer: false,
                sticker: false
            },
            shadow: false,
            width: "170px"
        });
        $.each(list, function (key, val) {
            var cartItem = { Image: $(val).attr('src'),ImageTitle: $(val).attr('title'), Quantity: quantity};
            var formData = new FormData();
            formData.append('item', JSON.stringify(cartItem));
            formData.append("productId", product);
            formData.append("materialId", material);
            formData.append("option", option);
            AddToCart(formData);
        });
        //$('#loading').hide();
        var totalTime = new Date().getTime() - ajaxTime;
        dyn_notice(totalTime, notice);
        GetCartItem(totalTime);
        //swal("Success", "Add Success", "success");
        
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

function GetCartItem(time) {
    $.ajax({
        url: '/Upload/GetCartUpload',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        setTimeout(function () {
            $('#list-cart').html(result);
            UpdateItem();
            $('.deleteitem').click(function () {
                var deleteId = $(this).data('id');
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
                        GetCartItem();
                        swal("Success", "Delete item success!", "success");
                    }
                }).error(function (xhr, status) {
                });

            });
        }, time);
    }).error(function (xhr, status) {
    });
}

function UpdateItem() {
    $('.updateitem').click(function () {
        var index = $(this).closest('tr').index();
        var cartItem = $('.cart-item')[index];
        var productitem = $(cartItem).find('.productitem').val();
        var materialitem = $(cartItem).find('.materialitem').val();
        var optionsitem = $(cartItem).find('.optionsitem').val();
        var quantityitem = $(cartItem).find('.quantityitem').val();
        var cartId = $(this).data('id');
        if (productitem === '0') {
            swal("Warning", "Please choose size", "error");
            return;
        }
        if (materialitem === '0') {
            swal("Warning", "Please choose material", "error");
            return;
        }
        if (quantityitem < 1) {
            swal("Warning", "Please enter a number greater than 0", "error");
            return;
        }
        var product = { ProductId: productitem };
        var material = { Id: materialitem };
        var cart = { Product: product, Material: material, Quantity: quantityitem, Option: optionsitem, Id: cartId }
        var data = new FormData();
        data.append('cartItem', cart);
        $('#loading').show();
        $.ajax({
            url: '/Upload/UpdateItem',
            type: 'POST',
            data: { cartItem: cart },
            async: false,
            dataType:'json'
        }).success(function (result) {
            if (result.status) {
                $('#loading').hide();
                GetCartItem();
                swal("Success", "Update item success!", "success");
            }
        }).error(function (xhr, status) {
        });
    }) 
}
function dyn_notice(time, notice) {
    var percent = 0;
    setTimeout(function () {
        notice.update({
            title: false
        });
        var interval = setInterval(function () {
            percent += 2;
            var options = {
                text: percent + "% complete."
            };
            if (percent >= 100) {
                window.clearInterval(interval);
                options.title = "Done!";
                options.type = "success";
                options.hide = true;
                options.buttons = {
                    closer: true,
                    sticker: true
                };
                options.icon = 'fa fa-check';
                options.shadow = true;
                options.width = PNotify.prototype.options.width;
            }
            notice.update(options);
        }, 50);
    }, time/2);
}