// Закрытие прелоудера
$(window).on("load", function () {
    $('#bgPreloader').fadeOut("slow");
    window.onbeforeunload = function () { $('#bgPreloader').fadeIn("slow"); }
});

$(".modalBody").click(function (e) {
    if (e.target.className.indexOf("modalBody") > -1) {
        closeModal();
    }
});

$(".modalClose").click(function () {
    closeModal();
});

$(".closeModalBtn").click(function () {
    closeModal();
});

function openModal(modal) {
    $(".modalBody").css("display", "flex");
    $(modal).css("display", "block").addClass("fadeInDown");
}

// Закрытие модального окна
function closeModal() {
    var modal = $(".modalBody").children(".fadeInDown");
    modal.addClass("fadeOutUp").removeClass("fadeInDown");
    setTimeout(function () {
        $(".modalBody").fadeOut(500);
        modal.css("display", "none").removeClass("fadeOutUp");
    }, 500);
}

function scrollTop(time = 1000, top = 0) {
    $('html, body').animate({
        scrollTop: top
    }, time);
}

// Глушилка
function globalError() {
    alert("Что-то пошло не так");
}