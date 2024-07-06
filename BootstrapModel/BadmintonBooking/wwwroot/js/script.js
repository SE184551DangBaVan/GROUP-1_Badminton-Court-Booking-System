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
    const courtCards = document.querySelectorAll('.court-card');

    let currentIndex = 0;
    const slideWidth = document.querySelector('.court-card').offsetWidth;
    const visibleCardsCount = Math.floor(slide.parentElement.offsetWidth / slideWidth);
    const maxIndex = slide.children.length - visibleCardsCount + 2; // Recalculate the max index with buffer
    let isUserInteracted = false;
    let autoScrollInterval;
    let idleTimeout;
    let direction = 1;

    // Added for smooth sliding
    slide.style.transition = 'transform 0.5s ease-in-out';

    function updateCardScaling() {
        courtCards.forEach((card, index) => {
            const distanceFromCenter = Math.abs(currentIndex - index);
            const scale = Math.max(1.1 - (distanceFromCenter * 0.1), 0.6);
            card.style.transform = `scale(${scale})`;
            card.style.opacity = 1 - (distanceFromCenter * 0.1);
        });
    }

    function startAutoScroll() {
        autoScrollInterval = setInterval(function () {
            if (currentIndex >= maxIndex+4 && direction === 1) {
                direction = -1;
            } else if (currentIndex <= 0 && direction === -1) {
                direction = 1;
            }
            currentIndex += direction * 0.02;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateCardScaling();
        }, 120); // Adjust scroll speed here
    }

    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }

    function resetScrollPosition() {
        currentIndex = 0;
        slide.style.transform = `translateX(0px)`;
        updateCardScaling();
    }

    function resetIdleTimeout() {
        clearTimeout(idleTimeout);
        idleTimeout = setTimeout(function () {
            if (!isUserInteracted) {
                startAutoScroll();
            }
        }, 2000); // Adjust idle time here
    }

    nextButton.addEventListener('click', function () {
        stopAutoScroll();
        isUserInteracted = true;
        if (currentIndex < maxIndex+4) {
            currentIndex++;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateCardScaling();
        } else {
            currentIndex = maxIndex;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateCardScaling();
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
            updateCardScaling();
        } else {
            currentIndex = 0;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateCardScaling();
        }
        setTimeout(function () {
            isUserInteracted = false;
            resetIdleTimeout();
        }, 5000); // Adjust delay time here
    });

    // Stop auto scrolling on hover over court-card elements
    courtCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            stopAutoScroll();
            card.classList.add('hovered');
            isUserInteracted = true; // Ensure the auto-scroll does not resume
        });
        card.addEventListener('mouseleave', function () {
            card.classList.remove('hovered');
            isUserInteracted = false;
            resetIdleTimeout();
        });
    });

    // Stop auto scrolling on hover over buttons and reset scroll position
    [prevButton, nextButton].forEach(button => {
        button.addEventListener('mouseenter', function () {
            stopAutoScroll();
            isUserInteracted = true; // Ensure the auto-scroll does not resume
        });
        button.addEventListener('mouseleave', function () {
            isUserInteracted = false;
            resetIdleTimeout();
        });
    });

    // Start auto scrolling initially
    startAutoScroll();

    // Stop auto scrolling and reset idle timeout on any user interaction
    ['click', 'keydown', 'touchstart'].forEach(event => {
        document.addEventListener(event, function () {
            stopAutoScroll();
            resetIdleTimeout();
        });
    });

    // Initial card scaling
    updateCardScaling();
});
