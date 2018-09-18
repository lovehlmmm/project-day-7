$(document).ready(function () {
    GetCartItem();
});
function GetCartItem() {
    $.ajax({
        url: '/Upload/GetCartUpload',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        dataType: 'html'
    }).success(function (result) {
        $('#list-cart').append(result);
    }).error(function (xhr, status) {
    });
}