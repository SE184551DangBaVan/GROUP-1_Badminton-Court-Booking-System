$(document).ready(function () {
    $(window).scroll(function () {
        if (this.scrollY > 40) {
            $('nav').addClass("sticky");
        } else {
            $('nav').removeClass("sticky");
        }
    })

    $(window).scroll(function () {
        if (this.scrollY > 40) {
            $('.mega-box').addClass("sticky");
        } else {
            $('.mega-box').removeClass("sticky");
        }
    })

    $(window).scroll(function () {
        if (this.scrollY > 150) {
            $('.Quote').addClass("flow");
        } else {
            $('.Quote').removeClass("flow");
        }
    })

    $(window).scroll(function () {
        if (this.scrollY > 200) {
            $('.heading h2').addClass("float");
        } else {
            $('.heading h2').removeClass("float");
        }
    })

    $(window).scroll(function () {
        if (this.scrollY > 220) {
            $('.welc-images').addClass("flew");
        } else {
            $('.welc-images').removeClass("flew");
        }
    })

    $(window).scroll(function () {
        if (this.scrollY > 250) {
            $('.courtlist .court-card').addClass("pop");
        } else {
            $('.courtlist .court-card').removeClass("pop");
        }
    })

    var typed = new Typed(".typing", {
        strings: ["Greatest", "Nicest", "Cleanest", "Safest"],
        typeSpeed: 100,
        backSpeed: 80,
        loop: true
    });
    


});

document.addEventListener('DOMContentLoaded', function () {
    const slide = document.getElementById('slide');
    const prevButton = document.getElementById('prev');
    const nextButton = document.getElementById('next');

    let currentIndex = 0;
    const slideWidth = document.querySelector('.court-card').offsetWidth + 50; // Adjust for margin

    // Add transition for smooth sliding
    slide.style.transition = 'transform 0.5s ease-in-out';

    nextButton.addEventListener('click', function () {
        const maxIndex = slide.children.length - Math.floor(slide.parentElement.offsetWidth / slideWidth);
        if (currentIndex < maxIndex) {
            currentIndex++;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
        }
    });

    prevButton.addEventListener('click', function () {
        if (currentIndex > 0) {
            currentIndex--;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
        }
    });
});