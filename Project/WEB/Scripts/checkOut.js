 $('#showAddress').click(function () {
      GetModalAddress();
    });
 

function GetModalAddress() {
    $.ajax({
        url: '/CheckOut/GetModalAddress',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#modalAddress').html(result);
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