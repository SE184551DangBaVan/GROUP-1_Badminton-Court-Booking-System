$(document).ready(function () {
    $(window).scroll(function () {
        if (this.scrollY > 40) {
            $('nav').addClass("sticky");
        } else {
            $('nav').removeClass("sticky");
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
        if (this.scrollY > 45) {
            $('.welc-images').addClass("fly");
        } else {
            $('.welc-images').removeClass("fly");
        }
    })

    var typed = new Typed(".typing", {
        strings: ["Greatest", "Nicest", "Cleanest", "Safest"],
        typeSpeed: 100,
        backSpeed: 80,
        loop: true
    });
});


const select = document.querySelectorAll('.selectBtn');
const option = document.querySelectorAll('.option');
let index = 1;

select.forEach(a => {
    a.addEventListener('click', b => {
        const next = b.target.nextElementSibling;
        next.classList.toggle('toggle');
        next.style.zIndex = index++;
    })
})
option.forEach(a => {
    a.addEventListener('click', b => {
        b.target.parentElement.classList.remove('toggle');
        const parent = b.target.closest('.select').children[0];

        parent.setAttribute('data-type', b.target.innerHTML);

        parent.innerHTML = b.target.innerHTML;
    })
});
$(function () {
    $("#sourcedatepicker").datepicker();
    $("#destinationdatepicker").datepicker();
});