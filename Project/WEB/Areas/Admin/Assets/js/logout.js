$('#btnLogout').click(function () {
    $.ajax({
        url: '/admin/Login/Logout',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                window.location = data.url;
            } else {
                swal({
                    type: 'error',
                    title: 'Logout Fail',
                    text: 'Logout Fail! Please try again ♥'
                });
            }
        }
    });

});