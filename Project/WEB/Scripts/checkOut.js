 $('#showAddress').click(function () {
      GetModalAddress();
    });
 $('#newAddress').on('click',function(){
   $('#addaddress').val();
})
$('#form-new-address').validate({
rules:{
newaddress{
    required: true
}
},submitHandler: function(form) {
var address = $('input[name=newaddress]').val();
            $.ajax({
        url: '/CheckOut/AddAddress',
        contentType: 'application/json,
        data: {address:address},
        type: 'POST',
        async: false,
        dataType: 'json'
    }).success(function (result) {
        if(result.status){
        GetModalAddress();
}
    }).error(function (xhr, status) {
    });
            return false;  // blocks regular submit since you have ajax
        }
})
function GetModalAddress() {
    $.ajax({
        url: '/CheckOut/GetModalAddress',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#modalAddress .modal-body').html(result);
$('#modalAddress').modal();
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
        $('#addAddress').remove();
        $('#modalAddress h5').text("Add a new receiving address")
        $('#modalAddress .modal-body').html(result);
    }).error(function (xhr, status) {
    });
}