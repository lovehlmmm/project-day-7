$("#form-register").submit(function () {

    var username = $("input[name=username]").val().trim();
    var password = $("input[name=password]").val().trim();
    var email = $("input[name=email]").val().trim();
    var name = $("input[name=name]").val().trim();
    var gender = $('input[name=gender]:checked', '#form-register').val();
     if (username === '' || password === '') {
        alert('fail');
        return false;
    }
    var user = { Username: username, Password: password, Email: email };
    var cus = { CustomerName: name, Gender: gender };
    $.ajax({
        url: '/RegisterUser/Register',
        type: 'POST',
        dataType: 'json',
        data: { user: user, customer: cus },
        async: true,
        success: function (data) {
            if (data.status === true) {
                swal("Success", "Register Success", "success");
            } else {
                swal({
                    type: 'error',
                    title: 'Register Fail',
                    text: 'Something Wrong !'
                });
            }
        }
    });
    return false;
});

$.validator.setDefaults({
    submitHandler: function () {
        alert("submitted!");
    }
});

$(document).ready(function () {
    $("#form-register").validate({
        rules: {
            name: "required",
            username: {
                required: true,
                minlength: 2
            },
            password: {
                required: true,
                minlength: 5
            },
            confirm_password: {
                required: true,
                minlength: 5,
                equalTo: "#password"
            },
            email: {
                required: true,
                email: true
            },
            gender: "required"
        },
        messages: {
            name: "Please enter your name",
            username: {
                required: "Please enter a username",
                minlength: "Your username must consist of at least 2 characters"
            },
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
            confirm_password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long",
                equalTo: "Please enter the same password as above"
            },
            email: "Please enter a valid email address",
            gender: "Please choose you gender !"
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
 
        } else if(element.prop('type') === 'radio') {
                error.appendTo(element.parent().parent().parent());
            } else {
                error.insertAfter(element);
            }
        },
    });
});