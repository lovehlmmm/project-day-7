$(document).ready(function () {

    $(".preview-thumbnail img").click(function () {
        var data = $(this).closest('a').data('target');
        var material = $(this).closest('a').data('material');
        $('.preview-pic .tab-pane').removeClass('active');
        $('#material').val(material).attr('selected','selected');
        $('#material').val(material);
        $(data).addClass('active');
        GetMoney();
    })
    $('#material').on('change', function () {
        //var val = $(this).val();
        var data = $('#material :selected').data('target');
        $('.preview-pic .tab-pane').removeClass('active');
        $(data).addClass('active');
        GetMoney();
    });
    $('#product').on('change', function () {
        GetMoney();
    });
    function GetMoney() {
        var product = $('#product').val();
        var material = $('#material').val();
        var data = new FormData();
        data.append('materialId', material);
        data.append('productId', product);
        $.ajax({
            url: '/ProductDetails/GetMoney',
            type: 'POST',
            async: false,
            data: data,
            processData: false,
            contentType: false
        }).success(function (result) {
            if (result.status) {
                $('.price span').text(result.total +' VNĐ');
            } else {
                swal("Error", result.message, "error");
            }
        }).error(function (xhr, status) {
        });
    }
})