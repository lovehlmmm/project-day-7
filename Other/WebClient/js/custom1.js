
 /* jQuery Preloader
  -----------------------------------------------*/
$(window).load(function(){
    $('.preloader').fadeOut(1000); // set duration in brackets    
});


$(document).ready(function() {

  /* Hide mobile menu after clicking on a link
    -----------------------------------------------*/
    $('.navbar-collapse a').click(function(){
        $(".navbar-collapse").collapse('hide');
    });


   /* home slider section
  -----------------------------------------------*/
  $(function(){
    jQuery(document).ready(function() {
    $('#home').backstretch([
       "images/home-bg-slider-img1.jpg", 
       "images/home-bg-slider-img2.jpg",
        ],  {duration: 2000, fade: 750});
    });
  }) 

  /* Parallax section
    -----------------------------------------------*/
  function initParallax() {
    $('#home').parallax("100%", 0.1);
    $('#blog').parallax("100%", 0.3);

  }
  initParallax();


  /* wow
  -------------------------------*/
  new WOW({ mobile: false }).init();

  });

