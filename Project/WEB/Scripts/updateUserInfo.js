$.validator.setDefaults({
    submitHandler: function () {
        var phonenumber = $('#id_phone').val();
        var addr = $('.addressmuti');
        var gender = $('input[name=checkgender]:checked').val();
        var dateofbirth = $('#id_dateofbirth').val();
        var customerName = $('#id_name').val();
        var password = $('input[name=newpassword]').val();
        var username = $('#id_username').val();
        var address = [];
        $.each(addr, function (key, val) {
            var Address = { AddressId: $(val).data('id'), AddressDetails: $(val).val()}
            address.push(Address);
        });
        var cvc = $('.id_cvc').val();
        var creditexpire = $('.id_creditexpire').val();
        var creditnumber = $('.id_creditnumber').val();
        var credit = { CreditNumber: creditnumber, Expire: creditexpire, CVC: cvc };
        
        var customer = { CustomerName: customerName, Addresses: address, PhoneNumber: phonenumber, Gender: gender, DateOfBirth: dateofbirth}
        var user = { Username: username, Password: password, Customer: customer }
        $('#loading').show();
        $.ajax({
            url: '/UserInfo/UpdateUser',
            type: 'POST',
            dataType: 'json',
            data: { userUpdate: user, creditCard: credit },
            success: function (response) {
                if (response.status) {
                    $('#loading').hide();
                    swal("Success", "You Update success!", "success");
                    
                } else {
                    $('#loading').hide();
                    alert('faill');
                }
            }
        });
    }
  });


$(document).ready(function () {
    $('.showcredit').click(function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/UserInfo/GetCreditCard?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    $('#showCardNum').val('**** **** **** ' + response.data.CreditNumber);
                    $('#showCardExpire').val(response.data.Expire);
                    $('#showCardCVC').val(response.data.CVC);
                    $('#creditModal').modal();
                } else {
                    alert('faill');
                }
            }
        });
    });
    $('.deleteCre').click(function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/UserInfo/DeleteCredit?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    $('#loading').hide();
                    swal("Success", "Delete success!", "success");
                    $(this).parent('#removeview').remove();

                 } else {
                    $('#loading').hide();
                    alert('faill');
                }
            }
        });
    });


    $('.deleteaddr').click(function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/UserInfo/Delete?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    $('#loading').hide();
                    swal("Success", "Delete success!", "success");
                    $(this).parent('.div_id_interval').remove();
                } else {
                    $('#loading').hide();
                    alert('faill');
                }
            }
        });
    });

    $("#form-user").validate({
        rules: {
            name: {
                required: true,
                minlength: 5

            },
            PhoneNumber: {
                required: true,
                minlength: 10
            },
            newpassword: {
                required: false,
                minlength: 5
            },
            confirm_password: {
                required: false,
                minlength: 5,
                equalTo: "#newpassword"
            },
            birthday: {
                required: false
            },
            cardNumber: {
                required: true,
                cardNumber: true
            },
            cardExpiry: {
                required: true,
                cardExpiry: true
            },
            cardCVC: {
                required: true,
                cardCVC: true
            }
        },
        messages: {
            name: "Please enter your name",
            PhoneNumber: {
                required: "Please enter a phone number",
                minlength: "Your phonenumber must consist of at least 5 characters"
            },
            newpasswordord: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
            confirm_password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long",
                equalTo: "Please enter the same password as above"
            },
            birthday: "Please provide a datetime"
        },
        highlight: function (element) {
            $(element).closest('.reg_error').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.reg_error').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    })
});

jQuery.validator.addMethod("cardNumber", function (value, element) {
    return this.optional(element) || Stripe.card.validateCardNumber(value);
}, "Please specify a valid credit card number.");

jQuery.validator.addMethod("cardExpiry", function (value, element) {
    /* Parsing month/year uses jQuery.payment library */
    value = $.payment.cardExpiryVal(value);
    return this.optional(element) || Stripe.card.validateExpiry(value.month, value.year);
}, "Invalid expiration date.");

jQuery.validator.addMethod("cardCVC", function (value, element) {
    return this.optional(element) || Stripe.card.validateCVC(value);
}, "Invalid CVC.");
