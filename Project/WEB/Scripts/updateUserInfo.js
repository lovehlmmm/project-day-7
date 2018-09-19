$(document).ready(function () {
var customer = $('#id_name').val();
var phonenumber = $('#id_phone').val();
var address   = $('.addressmuti').val();

var dateofbirth = $('id_dateofbirth').val();
var username = $('#id_username').val();
var password = $('changepasswd').val();
 
var customer = {CustomerName:customerName,Address:address}


var user = {Username:username,Password:password,Customer:customer}



});
function Update(data, id) {
    $.ajax({
        url: '/UserInfo/UpdateUser',
        type: 'POST',
        dataType: 'json',
        data: { product: data, id: id },
        success: function (response) {
            if (response.status) {
                swal("Success", "You Update success!", "success");
                 GetData();
            } else {
                alert('fail');
            }
        }
    });
}