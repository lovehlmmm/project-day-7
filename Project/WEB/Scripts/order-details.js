

$(document).ready(function () {
    var x = setInterval(function () {

        // Get todays date and time
        var oderdate = $('#order-date').data('id');
        var dateOrder = new Date(oderdate);
        dateOrder.setHours(dateOrder.getHours() + 24);

        var now = new Date().getTime();
        // Find the distance between now and the count down date
        var distance = dateOrder.getTime() - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="demo"
        $("#clock").text(hours + "h "
            + minutes + "m " + seconds + "s ");

        // If the count down is over, write some text 
        if (distance < 0) {
            clearInterval(x);
            document.getElementById("demo").innerHTML = "EXPIRED";
        }
    }, 1000);
    
    alert(dateOrder.toLocaleString());

});