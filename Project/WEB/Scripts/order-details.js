

$(document).ready(function () {
    var x = setInterval(function () {

        // Get todays date and time
        var oderdate = $('#order-date').data('id');
        var dateOrder = new Date(oderdate);
        dateOrder.setHours(dateOrder.getHours() + 24);

        var now = new Date().getTime();
        var distance = dateOrder.getTime() - now;

        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        $("#clock").text(hours + "h "+ minutes + "m " + seconds + "s ");
        if (distance <= 0) {
            clearInterval(x);
            $("#clock").text("EXPIRED");
        }
    }, 1000);
    
    alert(dateOrder.toLocaleString());

});