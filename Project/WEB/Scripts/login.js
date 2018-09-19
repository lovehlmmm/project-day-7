

$.validator.setDefaults({
    submitHandler: function () {
        var username = $("input[name=username]").val().trim();
        var password = $("input[name=password]").val().trim();
        var remember = $("input[name=remember]:checked").length > 0;
        var url = $('#urlRedirect').val();
        //if (username === '') {
        //    $("input[name=username]").addClass("border border-danger");
        //    return false;
        //} else {
        //    $("input[name=username]").removeClass("border border-danger");
        //}
        //if (password === '') {
        //    $("input[name=password]").addClass("border border-danger");
        //    return false;
        //} else {
        //    $("input[name=password]").removeClass("border border-danger");
        //}
        var user = { Username: username, Password: password, Remember: remember };
        $('#loading').show();

        $.ajax({
            url: '/LoginUser/CheckLogin',
            type: 'POST',
            dataType: 'json',
            data: { loginUser: user,url:url},
            async: true,
            success: function (data) {
                if (data.status === true) {
                    $('#loading').hide();
                    swal("Success", "Login Success", "success");
                    if (data.url!==null) {
                        window.location.replace(data.url);
                    }
                    
                } else {
                    $('#loading').hide();
                    swal("Error", data.message, "error");
                }
            }
        });
    }
});

$(document).ready(function () {

    $.ajax({
        url: '/LoginUser/ConfirmSuccess',
        type: 'GET',
        dataType: 'json',
        async: true,
        success: function (data) {
            if (data.status === true) {

                $("#activeModal").modal();
            } else {
                return false;

            }
        }
    });




    $("#formLogin").validate({
        rules: {
 
            username: {
                required: true,
                minlength: 2
            },
            password: {
                required: true,
                minlength: 5
            },
 
            agree: "required"
        },
        messages: {
            
            username: {
                required: "Please enter a username",
                minlength: "Your username must consist of at least 2 characters"
            },
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },

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
            } else {
                error.insertAfter(element);
            }
        },
    });
});



