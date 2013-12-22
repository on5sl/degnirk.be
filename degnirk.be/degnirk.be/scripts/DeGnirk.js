//Na het inladen van de html en css beginnen we met de javascript te verwerken
//We laden de afbeeldingen in op het header element
$('header').backstretch(images, { fade: 'slow', duration: '10000' });
var backgroundCssTop;
//Nadat de afbeelding is ingeladen wordt deze functie opgeroepen
$(window).on("backstretch.after", function (e, instance, index) {
    //De code in de if wordt enkel uitgevoerd na de eerste afbeelding, zodat een slideshow later geen problemen geeft
    if (index === 0) {
        //TODO: Dit kan beter vervangen door een reeks animaties zodat de tekst pas erin komt nadat het logo centraal staat
        $('#logoHolder').animate({ top: "0", opacity: "1" }, { duration: 1000 });
        $('nav').fadeIn(2000);
        backgroundCssTop = parseFloat(instance.$img.css('top'), 10);
    }
});

$(document).ready(function () {

    $('header').height($(window).height());

    $(window).resize(function () {
        $('header').height($(window).innerHeight);
    });

    
    $(window).scroll(function () {
        $('header').backstretch("pause");
        var coords = backgroundCssTop + ($(window).scrollTop() / 2) + 'px';
        $('header img').css({
            top: coords
    });
    });

    $('body').niceScroll({ mousescrollstep: 10, cursorborder: '0px', cursorwidth: '7px', cursoropacitymax: 0.5 });

    var angle1 = 0;
    setInterval(function () {
        angle1 += 0.15;
        $("#degnirkinnerring").rotate(angle1);
    }, 150);

    var angle2 = 0;
    setInterval(function () {
        angle2 -= 0.3;
        $("#degnirkouterring").rotate(angle2);
    }, 70);




    //https://api.foursquare.com/v2/venues/4d7be2a0f260a093e61f30ba/herenow?client_id=3UFSGCLAXPLGASVZIWDYEF3NL24SDC1RYYS1DMKDTGFJQJ1L&client_secret=XK4WTKZHT2HTUP3OTBCX1G55IST1X2ZQHQNG1GDVYBB2KHWH&v=20130827


    //$("#crown").rotate(-15);
});

(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o), m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})
(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-43474239-1', 'degnirk.be');
ga('send', 'pageview');