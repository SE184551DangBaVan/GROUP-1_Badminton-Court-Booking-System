$(document).ready(function () {
    $(window).scroll(function () {
        if (this.scrollY > 40) {
            $('nav').addClass("sticky");
        } else {
            $('nav').removeClass("sticky");
        }
    
        if (this.scrollY > 40) {
            $('.mega-box').addClass("sticky");
        } else {
            $('.mega-box').removeClass("sticky");
        }
    
        if (this.scrollY > 40) {
            $('.account-actions .drop-menu').addClass("sticky");
        } else {
            $('.account-actions .drop-menu').removeClass("sticky");
        }
    
        if (this.scrollY > 150) {
            $('.Quote').addClass("flow");
        } else {
            $('.Quote').removeClass("flow");
        }
    
        if (this.scrollY > 200) {
            $('.heading h2').addClass("float");
        } else {
            $('.heading h2').removeClass("float");
        }
    
        if (this.scrollY > 220) {
            $('.welc-images').addClass("flew");
        } else {
            $('.welc-images').removeClass("flew");
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
    const slideWidth = document.querySelector('.court-card').offsetWidth + 50; // Adjust margin
    const maxIndex = slide.children.length - Math.floor(slide.parentElement.offsetWidth / slideWidth);
    let isUserInteracted = false;
    let autoScrollInterval;
    let idleTimeout;
    let direction = 1;

    // Added for smooth sliding
    slide.style.transition = 'transform 0.5s ease-in-out';

    function startAutoScroll() {
        autoScrollInterval = setInterval(function () {
            if (currentIndex >= maxIndex +0.2 && direction === 1) {
                direction = -1;
            } else if (currentIndex <= 0 && direction === -1) {
                direction = 1;
            }
            currentIndex += direction*0.01;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
        }, 120); // Adjust scroll speed here
    }

    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }

    function resetIdleTimeout() {
        clearTimeout(idleTimeout);
        idleTimeout = setTimeout(function () {
            if (!isUserInteracted) {
                startAutoScroll();
            }
        }, 1000); // Adjust idle time here
    }

    nextButton.addEventListener('click', function () {
        stopAutoScroll();
        isUserInteracted = true;
        if (currentIndex < maxIndex) {
            currentIndex++;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
        }
        setTimeout(function () {
            isUserInteracted = false;
            resetIdleTimeout();
        }, 5000); // Adjust delay time here
    });

    prevButton.addEventListener('click', function () {
        stopAutoScroll();
        isUserInteracted = true;
        if (currentIndex > 0) {
            currentIndex--;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
        }
        setTimeout(function () {
            isUserInteracted = false;
            resetIdleTimeout();
        }, 5000); // Adjust delay time here
    });

    // Start auto scrolling initially
    startAutoScroll();

    // Stop auto scrolling and reset idle timeout on any user interaction
    ['click', 'mousemove', 'keydown', 'touchstart'].forEach(event => {
        document.addEventListener(event, function () {
            stopAutoScroll();
            resetIdleTimeout();
        });
    });
});