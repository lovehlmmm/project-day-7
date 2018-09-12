$("#form-register").submit(function () {
    
    var username = $("input[name=username]").val().trim();
    var password = $("input[name=password]").val().trim();
    var email = $("input[name=email]").val().trim();
    var name = $("input[name=name]").val().trim();
    var gender=$('input[name=gender]:checked', '#form-register').val();
    var remember = $("input[name=remember]:checked").length > 0;
    if (username === '' || password === '') {
        alert('fail');
        return false;
    }
    var user = { Username: username, Password: password, Email: email};
    var cus = { CustomerName: name, Gender: gender };
    $.ajax({
        url: '/RegisterUser/Register',
        type: 'POST',   
        dataType: 'json',
        data: {user:user,customer:cus},
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
