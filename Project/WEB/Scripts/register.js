

$.validator.setDefaults({
    submitHandler: function () {
        var username = $("input[name=username]").val().trim();
        var password = $("input[name=password]").val().trim();
        var email = $("input[name=email]").val().trim();
        var name = $("input[name=name]").val().trim();
        var gender = $('input[name=gender]:checked', '#form-register').val();
        var user = { Username: username, Password: password, Email: email };
        var cus = { CustomerName: name, Gender: gender };
        $('#loading').show();

        $.ajax({
            url: '/RegisterUser/Register',
            type: 'POST',
            dataType: 'json',
            data: { user: user, customer: cus },
            async: true,
            success: function (data) {
                if (data.status === true) {
                    $('#loading').hide();
                    swal("Success", "Register Success", "success");
                } else {
                    $('#loading').hide();
                    swal({
                        type: 'error',
                        title: 'Register Fail',
                        text: 'Something Wrong !'
                    });
                }
            }
        });
    }
});

jQuery.validator.addMethod("checkexist", function (value, element) {   
    var checkAcc;
    $.ajax({
        url: '/RegisterUser/CheckExistAccount?username=' + value,
        type: 'GET',
        async: false,
        success: function (data) {
            if (data.status === true) {
                checkAcc = true;
            } else {
                checkAcc = false;
            }
        }
    });
    if (!checkAcc) {
        return false;  // FAIL validation when REGEX matches
    } else {
        return true;   // PASS validation otherwise
    }
});
    

$(document).ready(function () {
    //$("input[name=username]").on('input', function () {

    //    $.ajax({
    //        url: '/RegisterUser/CheckExist?username=' + $(this).val(),
    //        type: 'GET',
    //        async: true,
    //        success: function (data) {
    //            if (data.status === true) {
    //                swal("Success", "Account Not Exist", "success");
    //            } else {
    //                swal({
    //                    type: 'error',
    //                    title: 'Register Fail',
    //                    text: 'Something Wrong !'
    //                });
    //            }
    //        }
    //    });
    //});
    $("#form-register").validate({
        rules: {
            name: "required",
            username: {
                required: true,
                minlength: 5,
                checkexist: true
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
                minlength: "Your username must consist of at least 5 characters",
                checkexist: "Account already exists"
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

            } else if (element.prop('type') === 'radio') {
                error.appendTo(element.parent().parent().parent());
            } else {
                error.insertAfter(element);
            }
        }
    });
});
