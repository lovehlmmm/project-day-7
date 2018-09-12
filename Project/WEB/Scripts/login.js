
$("#formLogin").submit(function () {
    
    var username = $("input[name=username]").val().trim();
    var password = $("input[name=password]").val().trim();
    var remember = $("input[name=remember]:checked").length > 0;
    if (username === '') {
        $("input[name=username]").addClass("border border-danger");
        return false;
    } else {
        $("input[name=username]").removeClass("border border-danger");
    }
    if (password === '') {
        $("input[name=password]").addClass("border border-danger");
        return false;
    } else {
        $("input[name=password]").removeClass("border border-danger");
    }
    var user = { Username: username, Password: password, Remember: remember };
    $.ajax({
        url: '/LoginUser/CheckLogin',
        type: 'POST',   
        dataType: 'json',
        data: {loginUser:user},
        async: true,
        success: function (data) {
            if (data.status === true) {
                swal("Success", "Login Success", "success");
            } else {
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
