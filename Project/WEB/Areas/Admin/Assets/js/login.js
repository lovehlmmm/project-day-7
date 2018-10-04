
$("#login_form").submit(function () {

    var username = $("input[name=username]").val().trim();
    var password = $("input[name=password]").val().trim();
    var remember = $("input[name=remember]:checked").length > 0;
    var user = { username: username, password: password, remember: remember };
    $('#loading').show();
    $.ajax({
        url: '/Admin/Login/CheckLogin',
        type: 'POST',
        dataType: 'json',
        data: { user: JSON.stringify(user) },
        async: true,
        success: function (data) {
            if (data.status === true) {
                $('#loading').hide();
                window.location = data.url;
            } else {
                $('#loading').hide();
                swal({
                    type: 'error',
                    title: 'Login Fail',
                    text: 'Username password incorrect!'
                });
            }
        }
    });
    return false;
}); 
