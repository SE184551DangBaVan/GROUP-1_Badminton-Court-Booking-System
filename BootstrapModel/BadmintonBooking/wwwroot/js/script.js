$(document).ready(function () {
    function applyScrollClasses(scrollY) {
        if (scrollY > 40) {
            $('.navBar').addClass("sticky");
            $('.mega-box').addClass("sticky");
            $('.account-actions .drop-menu').addClass("sticky");
        } else {
            $('.navBar').removeClass("sticky");
            $('.mega-box').removeClass("sticky");
            $('.account-actions .drop-menu').removeClass("sticky");
        }

        if (scrollY > 150) {
            $('.Quote').addClass("flow");
            $('.return-to-top-button i').addClass("appear");
        } else {
            $('.Quote').removeClass("flow");
            $('.return-to-top-button i').removeClass("appear");
        }

        if (scrollY > 180) {
            $('.heading h2').addClass("headingfloat");
        } else {
            $('.heading h2').removeClass("headingfloat");
        }

        if (scrollY > 1100) {
            $('.court-card').each(function (index) {
                $(this).css('transition-delay', index * 50 + 'ms').addClass("float");
            });
        } else {
            $('.court-card').each(function () {
                $(this).css('transition-delay', '0ms').removeClass("float");
            });
        }
    }
    
    function handleScroll() {
        const scrollY = $(window).scrollTop();
        applyScrollClasses(scrollY);
    }

    // Apply classes on initial load
    handleScroll();

    $(window).scroll(function () {
        handleScroll();
    });
    
    $(window).on('load', function () {
        $(window).scrollTop($(window).scrollTop() + 10);
    });

    window.onpopstate = function () {
        handleScroll();
    };
    $(window).on('popstate', function () {
        handleScroll();
    });
    
    var typed = new Typed(".typing", {
        strings: ["Greatest", "Cleanest", "Prestigious", "Most Reliable"],
        typeSpeed: 105,
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
    const maxIndex = slide.children.length - visibleCardsCount;
    let isUserInteracted = false;
    let autoScrollInterval;
    let idleTimeout;
    let direction = 1;

    slide.style.transition = 'transform 0.5s ease-in-out';

    function updateBackgroundImage() {
        const currentCard = courtCards[Math.round(currentIndex)];
        const newBgUrl = `/images/${currentCard.getAttribute('data-bg-url')}`;
        document.querySelector('.BookingType').style.background = `url(${newBgUrl}) center no-repeat`;
    }

    function startAutoScroll() {
        autoScrollInterval = setInterval(function () {
            if (currentIndex >= maxIndex + 2 && direction === 1) {
                direction = -1;
            } else if (currentIndex <= 0 && direction === -1) {
                direction = 1;
            }
            currentIndex += direction * 0.02;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateBackgroundImage();
        }, 120); // Adjust scroll speed here
    }

    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }

    function resetScrollPosition() {
        currentIndex = 0;
        slide.style.transform = `translateX(0px)`;
    }

    function resetIdleTimeout() {
        clearTimeout(idleTimeout);
        idleTimeout = setTimeout(function () {
            if (!isUserInteracted) {
                startAutoScroll();
            }
        }, 3000); // Adjust idle time here
    }

    nextButton.addEventListener('click', function () {
        stopAutoScroll();
        isUserInteracted = true;
        if (currentIndex < maxIndex + 2) {
            currentIndex++;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateBackgroundImage();
        } else {
            currentIndex = maxIndex;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateBackgroundImage();
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
            updateBackgroundImage();
        } else {
            currentIndex = 0;
            slide.style.transform = `translateX(-${slideWidth * currentIndex}px)`;
            updateBackgroundImage();
        }
        setTimeout(function () {
            isUserInteracted = false;
            resetIdleTimeout();
        }, 5000); // Adjust delay time here
    });

    // Stop auto scrolling on hover
    courtCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            stopAutoScroll();
            card.classList.add('hovered');
            isUserInteracted = true;
        });
        card.addEventListener('mouseleave', function () {
            card.classList.remove('hovered');
            isUserInteracted = false;
            resetIdleTimeout();
        });
    });

    // Stop auto scrolling on hover over buttons
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

    updateBackgroundImage();
});

document.addEventListener('DOMContentLoaded', function () {
    var suspendButtons = document.querySelectorAll('.manager-buttons-suspend');

    suspendButtons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            var confirmation = confirm('Are you sure you want to suspend this court?');
            if (confirmation) {
                var card = button.closest('.card');
                card.classList.toggle('suspended');
            } else {
                event.preventDefault();
            }
        });
    });
});
