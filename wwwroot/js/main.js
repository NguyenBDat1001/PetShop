(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            const spinnerElement = $('#spinner');
            if (spinnerElement.length > 0) {
                spinnerElement.removeClass('show');
            }
        }, 800); // Tăng thời gian để spinner hiển thị rõ hơn
    };
    spinner();

    // Fixed Navbar
    $(window).on('scroll', function () {
        const isSmallScreen = $(window).width() < 992;
        const scrollPosition = $(this).scrollTop();
        const navbar = $('.fixed-top');

        if (scrollPosition > 55) {
            navbar.addClass('shadow').css('top', isSmallScreen ? '' : -55);
        } else {
            navbar.removeClass('shadow').css('top', isSmallScreen ? '' : 0);
        }
    });

    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });

    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        items: 4, // Hiển thị 4 mục
        autoplayHoverPause: true, // Dừng khi hover
        autoplay: true, // Tự động chạy
        smartSpeed: 2000,
        center: false,
        dots: true,  // Ẩn dấu chấm
        loop: true, // Lặp lại carousel
        margin: 25, // Khoảng cách giữa các mục
        nav: true, // Hiển thị nút điều hướng
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: { items: 1 }, // 1 mục trên màn hình nhỏ
            576: { items: 2 }, // 2 mục trên màn hình vừa
            768: { items: 3 }, // 3 mục trên màn hình lớn hơn
            992: { items: 4 } // 4 mục trên màn hình lớn
        }
    });

    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav: true,
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });

    // Modal Video
    $('.btn-play').on('click', function () {
        const $videoSrc = $(this).data("src");
        if ($videoSrc) {
            $('#videoModal').on('shown.bs.modal', function () {
                $("#video").attr('src', $videoSrc + "?autoplay=1&modestbranding=1&showinfo=0");
            });
        }
    });

    $('#videoModal').on('hide.bs.modal', function () {
        $("#video").attr('src', '');
    });

    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });

    // Chọn hình ảnh và container
    const imageContainer = document.querySelector('.image-container');
    if (imageContainer) {
        const image = imageContainer.querySelector('.img-prod');
        imageContainer.addEventListener('mousemove', (e) => {
            const { left, top, width, height } = imageContainer.getBoundingClientRect();
            const x = ((e.clientX - left) / width) * 100;
            const y = ((e.clientY - top) / height) * 100;

            image.style.transformOrigin = `${x}% ${y}%`;
            image.style.transform = 'scale(2)';
        });

        imageContainer.addEventListener('mouseleave', () => {
            image.style.transform = 'scale(1)';
        });
    }
    $(document).ready(function () {
        toastr.options = {
            "closeButton": true,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-center", // Vị trí hiển thị (top-right, top-left, bottom-right, bottom-left)
            "preventDuplicates": true,
            "timeOut": "5000", // Thời gian tồn tại (ms)
        };
    });
})(jQuery);