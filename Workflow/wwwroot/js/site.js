// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function($) {
  "use strict"; // Start of use strict

  // Toggle the side navigation
  $("#sidebarToggle").on('click', function(e) {
    e.preventDefault();
    $("body").toggleClass("sidebar-toggled");
    $(".sidebar").toggleClass("toggled");
  });

  // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
  $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function(e) {
    if ($(window).width() > 768) {
      var e0 = e.originalEvent,
        delta = e0.wheelDelta || -e0.detail;
      this.scrollTop += (delta < 0 ? 1 : -1) * 30;
      e.preventDefault();
    }
  });

  // Scroll to top button appear
  $(document).on('scroll', function() {
    var scrollDistance = $(this).scrollTop();
    if (scrollDistance > 100) {
      $('.scroll-to-top').fadeIn();
    } else {
      $('.scroll-to-top').fadeOut();
    }
  });

  // Smooth scrolling using jQuery easing
  $(document).on('click', 'a.scroll-to-top', function(event) {
    var $anchor = $(this);
    $('html, body').stop().animate({
      scrollTop: ($($anchor.attr('href')).offset().top)
    }, 1000, 'easeInOutExpo');
    event.preventDefault();
  });

  // Add/remove classes when window resizes -TR
  $(document).ready(function($) {
    var changeClass = function() {
        var w = document.body.clientWidth;
        if (w < 768) {
          $('#form1').removeClass('m-3 text-right');
        } else if (w >= 768) {
           $('#form1').addClass('m-3 text-right');
        };
    };
    $(window).resize(function(){
      changeClass();
    });
    //Fire it when the page first loads:
    changeClass();
  });

})(jQuery); // End of use strict

function toggleDisplay(div_id) {
    var div = document.getElementById(div_id);
    if (div.classList.contains("hidden")) {
        div.classList.remove("hidden");
        document.getElementById("ListName").focus();
    } else {
        div.classList.add("hidden");
    }
}

function setNotesViewed(id) {
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Event/SetViewed/" + id,
            contentType: "application/json; charset=utf-8",
        }).always(function () {
            document.getElementById("notes_count").classList.add("hidden");
        });
    });
}
