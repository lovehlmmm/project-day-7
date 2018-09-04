$(document).ready(function () {
    $("#btnLogin").click(function() {
        var username = $("input[name=username]").val();
        var password = $("input[name=password]").val();
        var user = { username: username, password: password, remember: true };
        $.ajax({
            url: '/Login/CheckLogin',
            type: 'POST',
            dataType: 'json',
            data: {user:JSON.stringify(user)},
            async: true,
            success: function (data) {
                if (data.status === true) {
                    alert("success");
                } else {
                    alert('fail');
                }
            }
        });
    }); 
});