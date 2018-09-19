

$(document).ready(function () {
    $('#saveinfo').click(function () {
        var phonenumber = $('#id_phone').val();
        var address = $('.addressmuti').val();
        var gender = $('input[name=checkgender]:checked').val();
        var dateofbirth = $('#id_dateofbirth').val();
        var customerName = $('#id_name').val();
        var password = $('#inputchangepasswd').val();
        var username = $('#id_username').val();
        var customer = { CustomerName: customerName, Address: address, PhoneNumber: phonenumber, Gender: gender, DateOfBirth: dateofbirth }
        var user = { Username: username, Password: password, Customer: customer }
        $.ajax({
            url: '/UserInfo/UpdateUser',
            type: 'POST',
            dataType: 'json',
            data: { userUpdate: user },
            success: function (response) {
                if (response.status) {
                    swal("Success", "You Update success!", "success");  
                } else {
                    alert('faill');
                }
            }
        });
    })
 });


