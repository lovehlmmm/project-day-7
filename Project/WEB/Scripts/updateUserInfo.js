﻿$.validator.setDefaults({
    submitHandler: function () {
        var phonenumber = $('#id_phone').val();
        var addr = $('.addressmuti');
        var gender = $('input[name=checkgender]:checked').val();
        var dateofbirth = $('#id_dateofbirth').val();
        var customerName = $('#id_name').val();
        var password = $('#inputchangepasswd').val();
        var username = $('#id_username').val();
        var address = [];
        $.each(addr, function (key, val) {
            var Address = { AddressId: $(val).data('id'), AddressDetails: $(val).val()}
            address.push(Address);
        });
        var cvc = $('.id_cvc').val();
        var creditexpire = $('.id_creditexpire').val();
        var creditnumber = $('.id_creditcard').val();

        var addcre = $('.addcredit');
        var addcredit = [];

        $.each(addcre, function (key, val) {
            var CreditCard = { CreditCardId: $(val).data('id'), CreditNumber: $(val).val(), Expire: $(val).val(), CVC: $(val).val()}
            addcredit.push(CreditCard);
        });
        var creditcard = { CreditNumber: creditnumber, Expire: creditexpire, CVC: cvc }
        var customer = { CustomerName: customerName, Addresses: address, PhoneNumber: phonenumber, Gender: gender, DateOfBirth: dateofbirth, CreditCard: creditcard }
        var user = { Username: username, Password: password, Customer: customer }
        $('#loading').show();
        $.ajax({
            url: '/UserInfo/UpdateUser',
            type: 'POST',
            dataType: 'json',
            data: { userUpdate: user },
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
    $('.deleteaddr').click(function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/UserInfo/Delete?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    $('#loading').hide();
                    swal("Success", "You Update success!", "success");
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
                maxlength : 5
            },
            PhoneNumber: {
                required: true,
                maxlength: 10
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


