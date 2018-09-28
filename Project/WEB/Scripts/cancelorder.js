$('#btnCancel').click(function (){
    var id = $(this).data('id');
    swal({
        title: "Are you sure?",
        text: "Make sure ! When you cancel your order, you will receive a refund..!",
        icon: "success",
        buttons: true,
        dangerMode: true,
    })
        .then((willCancel) => {
            if (willCancel) {
                $('#loading').show();
                ConfirmCancel(id);
             }
        });
})

function ConfirmCancel(id) {
    $('#loading').show();
    $.ajax({
        url: '/OrderDetails/CancelOrder?id=' + id,
        type: 'GET',
         async: false,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            $('#loading').hide();
            swal("OK! Cancel success  !", {
                icon: "success"
            });
            window.location.replace(result.url)
        } else {
            $('#loading').hide();
            swal("Error", result.message, "error");
        }
    }).error(function (xhr, status) {
    });
}