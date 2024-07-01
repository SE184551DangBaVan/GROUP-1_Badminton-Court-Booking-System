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