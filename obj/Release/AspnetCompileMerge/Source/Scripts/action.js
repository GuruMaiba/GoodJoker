/*
 * Слайдер
 * --------------------------------------------------
 */
var slider = {};                                // Создаём объект

var slideWidth = $('.slideBlock').width();      // Ширину слайдера
var slideHeight = $('.slideBlock').height();    // Высоту слайдера
var intervalMoveSlider = 4000;                  // Время интревала
var speedMoveSlider = 400;                      // Скорость анимации
var sliderTimer;                                // Таймер
var currentSlide = 0;                           // Счётчик

// функция ресайза слайдера
slider.resize = function () {
    var winHgt = $(window).height();
    var winWdh = $(window).width();
    slideWidth = $('.slideBlock').width();

    if (winWdh > winHgt + 400 && winHgt > winWdh - 400) {
        slideHeight = winHgt - 56;
    } else {
        slideHeight = winWdh * 0.45;
    }

    $(".slideBlock").height(slideHeight);
    $('.slide').width(slideWidth);
    $('.slide').height(slideHeight);
    $('.slider').width($('.slider').children().length * slideWidth);

};

// функция движения слайдера
slider.moveSlider = function () {
    
    // интервал через сколько показывать следующий слайд
    slider.clear();
    sliderTimer = setInterval(slider.nextSlide, intervalMoveSlider);

    $('.slideBlock').mouseover(function () {    // действия по наведению
        slider.clear();                         // функция очищения таймера
    });

    $('.slideBlock').mouseout(function () {                                 // действие по отведению
        slider.clear();                                                     // функция очищения таймера
        sliderTimer = setInterval(slider.nextSlide, intervalMoveSlider);    // задаём таймер
    });

};

// функция следующего слайда
slider.nextSlide = function () {
    // Увеличиваем счётчик на один
    currentSlide++;

    // Если счётчик больше или равен количеству слайдов
    if (currentSlide >= $('.slider').children().length)
        currentSlide = 0; // то обнуляем счётчик

    // Анимируем смещение блока
    $('.slider').animate({ left: -currentSlide * slideWidth }, speedMoveSlider);

}

// функция предыдущего слайда
slider.prevSlide = function () {
    // Уменьшаем счётчик на один
    currentSlide--;

    // Если счётчик меньше 0
    if (currentSlide < 0)
        currentSlide = $('.slider').children().length - 1; // то задаём счётчику максимальное значение

    // Анимируем смещение блока
    $('.slider').animate({ left: -currentSlide * slideWidth }, speedMoveSlider);

}

// функция очищения таймера
slider.clear = function () {
    clearInterval(sliderTimer);         // останавливаем таймер
}

// действия по нажатию на кнопку next
$('#nextSlide').click(function (e) {    // событие нажатия
    e.preventDefault();                 // отмена действия
    slider.nextSlide();                 // запуск функции следующего слайда
});

// действия по нажатию на кнопку prev
$('#prevSlide').click(function (e) {    // событие нажатия
    e.preventDefault();                 // отмена действия
    slider.prevSlide();                 // запуск функции предыдущего слайда
});

/*
 * --------------------------------------------------
 * Слайдер
 */

/*
 * Анимация частиц
 * --------------------------------------------------
 */

// Запрос покадровой анимации
(function () {
    var lastTime = 0;
    var vendors = ['ms', 'moz', 'webkit', 'o'];
    for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
        window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
        window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame']
            || window[vendors[x] + 'CancelRequestAnimationFrame'];
    }

    if (!window.requestAnimationFrame)
        window.requestAnimationFrame = function (callback, element) {
            var currTime = new Date().getTime();
            var timeToCall = Math.max(0, 16 - (currTime - lastTime));
            var id = window.setTimeout(function () { callback(currTime + timeToCall); },
                timeToCall);
            lastTime = currTime + timeToCall;
            return id;
        };

    if (!window.cancelAnimationFrame)
        window.cancelAnimationFrame = function (id) {
            clearTimeout(id);
        };
}());

initAnimateBg('bgPrevImg', 'canvasBG', 0, 0.5, 0.00005, 20);
initAnimateBg('topName', 'canvasTopName', 600, 0.1, 0.00005, 100);
initAnimateBg('topMask', 'canvasTopMask', 800, 0.01, 0.00005, 500);

// Анимация
function initAnimateBg(shellId, canvasId, zoom, countElem, alphaSize, scaleSize) {

    var width, height, header, canvas, ctx, circles, target, animateHeader = true;

    // Main
    initHeader();
    addListeners();

    function initHeader() {
        width = window.innerWidth + zoom;
        height = window.innerHeight + zoom;
        target = { x: 0, y: height };

        header = document.getElementById(shellId);
        header.style.height = height + 'px';
        header.style.width = width + 'px';

        canvas = document.getElementById(canvasId);
        canvas.width = width;
        canvas.height = height;
        ctx = canvas.getContext('2d');

        // create particles
        circles = [];
        for (var x = 0; x < width * countElem; x++) {
            var c = new Circle();
            circles.push(c);
        }
        animate();
    }

    // Event handling
    function addListeners() {
        window.addEventListener('scroll', scrollCheck); // проверка скрола
        window.addEventListener('resize', resize);
    }

    function scrollCheck() {
        if (document.body.scrollTop > height) animateHeader = false;
        else animateHeader = true;
    }

    function resize() {
        width = window.innerWidth + zoom;
        height = window.innerHeight + zoom;
        header.style.height = height + 'px';
        canvas.width = width;
        canvas.height = height;
    }

    function animate() {
        if (animateHeader) {
            ctx.clearRect(0, 0, width, height);
            for (var i in circles) {
                circles[i].draw();
            }
        }
        requestAnimationFrame(animate);
    }

    // Canvas manipulation
    function Circle() {
        var _this = this;

        // constructor
        (function () {
            _this.pos = {};
            init();
        })();

        function init() {
            _this.pos.x = Math.random() * width;
            _this.pos.y = height + Math.random() * 100;
            _this.alpha = 0.1 + Math.random() * 0.3;
            _this.scale = 0.1 + Math.random() * 0.3;
            _this.velocity = Math.random();
        }

        this.draw = function () {
            if (_this.alpha <= 0) {
                init();
            }
            _this.pos.y -= _this.velocity;
            _this.alpha -= alphaSize;
            ctx.beginPath();
            ctx.arc(_this.pos.x, _this.pos.y, _this.scale * scaleSize, 0, 2 * Math.PI, false);
            ctx.fillStyle = 'rgba(21, 219, 219,' + _this.alpha + ')';
            ctx.fill();
        };
    }

};

/*
 * --------------------------------------------------
 * Анимация частиц
 */