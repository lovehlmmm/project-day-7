
/* jQuery Pre loader
 -----------------------------------------------*/
$(window).load(function () {
    $('.preloader').fadeOut(1000); // set duration in brackets    
});


$(document).ready(function () {


    /* Hide mobile menu after clicking on a link
      -----------------------------------------------*/
    $('.navbar-collapse a').click(function () {
        $(".navbar-collapse").collapse('hide');
    });


    /* home slider section
    -----------------------------------------------*/
    $(function () {
        jQuery(document).ready(function () {
            $('#home').backstretch([
                "../Assets/images/bgxx.jpg",
                "../Assets/images/bgnew.png",
            ], { duration: 5000, fade: 750 });
        });
    })

    /* Owl Carousel
      -----------------------------------------------*/
    $(document).ready(function () {
        $("#owl-testimonial").owlCarousel({
            autoPlay: 5000,
            items: 2,
            itemsDesktop: [1199, 1],
            itemsDesktopSmall: [979, 2],
        });
    });


    /* Parallax section
      -----------------------------------------------*/
    function initParallax() {
        $('#home').parallax("100%", 0.1);
        $('#service').parallax("100%", 0.3);
        $('#about').parallax("100%", 0.2);
        $('#team').parallax("100%", 0.3);
        $('#portfolio').parallax("100%", 0.1);
        $('#blog').parallax("100%", 0.2);
        $('#faq').parallax("100%", 0.1);
        $('#testimonial').parallax("100%", 0.3);
        $('#contact').parallax("100%", 0.1);

    }
    initParallax();


    /* Nivo lightbox
      -----------------------------------------------*/
    $('#portfolio .col-md-4 a').nivoLightbox({
        effect: 'fadeScale',
    });


    /* wow
    -------------------------------*/
    new WOW({ mobile: false }).init();

});

// image gallery
// init the state from the input
$(".image-checkbox").each(function () {
    if ($(this).find('input[type="checkbox"]').first().attr("checked")) {
        $(this).addClass('image-checkbox-checked');
    }
    else {
        $(this).removeClass('image-checkbox-checked');
    }
});

// sync the state to the input
$(".image-checkbox").on("click", function (e) {
    $(this).toggleClass('image-checkbox-checked');
    var $checkbox = $(this).find('input[type="checkbox"]');
    $checkbox.prop("checked", !$checkbox.prop("checked"))

    e.preventDefault();
});


//Upload preview image
function previewImages() {

    var preview = document.querySelector('#preview');

    if (this.files) {
        [].forEach.call(this.files, readAndPreview);
    }

    function readAndPreview(file) {

        // Make sure `file.name` matches our extensions criteria
        if (!/\.(jpe?g|png|gif)$/i.test(file.name)) {
            return alert(file.name + " is not an image");
        } // else...

        var reader = new FileReader();

        reader.addEventListener("load", function () {
            var div1 = document.createElement("div");
            div1.style = "position:relative;display: inline-block";

            $(div1).addClass("form-group text-center");
            $(div1).append('<a href="#" class="btn btn-danger close" onClick="removeDiv(this)" >x</a>');
            var image = new Image();
            image.height = 150;
            image.width = 200;
            image.title = file.name;
            image.src = this.result;
           
 
            image.className = "imgChoiced";
            image.onclick = function clickImg() {
                $(this).toggleClass("image-checkbox-checked");
             }

            image.style = "margin:10px;box-shadow: 1px 1px 5px 0 #a2958a;max-width:200px;";


            div1.append(image);
            preview.append(div1);
        }, false);

        reader.readAsDataURL(file);

    }

}

function removeDiv(e) {
    $(e).closest("div").remove();

 
      //$(this).parent('div.form-group text-center').remove();
}
 


document.querySelector('#file-input').addEventListener("change", previewImages, false);

function toggle(data) {
    var mode = $(data).attr("data-mode")
    if (mode === 'check') {
        $(".imgChoiced").each(function (index) {
            if (!$(this).hasClass("image-checkbox-checked")) {
                $(this).toggleClass("image-checkbox-checked");
            }
            $(data).attr("data-mode", "uncheck");
            $(data).text("Deselect All")
        });
    }
    else if (mode === 'uncheck') {
        $(".imgChoiced").each(function (index) {
            if ($(this).hasClass("image-checkbox-checked")) {
                $(this).toggleClass("image-checkbox-checked");
            }
            $(data).attr("data-mode", "check")
            $(data).text("Select All")
        })
    }
}

