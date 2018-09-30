function GetDataEdit(id) {

    $.ajax({
        url: '/Order/Get/' + id,
        type: 'GET',
        success: function (response) {
            $('#body-modal-order-details').html(response);
            $('#modal-order-details').modal();
        }
    });
}