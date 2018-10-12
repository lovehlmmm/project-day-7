$(document).ready(function() {
    $('#edit-email').click(function() {
        $('.code-email').hide();
        $('#modalMail').modal();
    });
    $("#save-email").click(function() {
        var email = $('input[name=email]').val();
        var password = $('input[name=password]').val();
        var host = $('input[name=host]').val();
        var emailCode = $('input[name=email-code]').val();
        var port = $('input[name=port]').val();
        var code = $('input[name=code]').val();
        var emailApp = { Email: email, Password: password, Port: port, Stmp: host };
        var data = new FormData();
        data.append('emailApp', JSON.stringify(emailApp));
        data.append('email', emailCode);
        data.append('key', code);
        $('#loading').show();
        $.ajax({
            url: '/Setting/UpdateEmailSetting',
            type: 'POST',
            async: false,
            data: data,
            processData: false,
            contentType: false,
            success: function(result) {
                if (result.status) {
                    $('#loading').hide();
                    $('.code-email').show();
                } else {
                    $('#loading').hide();
                }
            }
        });
    });
});