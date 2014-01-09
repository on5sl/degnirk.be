//Na het inladen van de html en css beginnen we met de javascript te verwerken
//We laden de afbeeldingen in op het header element
$('header').backstretch(images, { fade: 'slow', duration: '10000' });
//var backgroundCssTop;
//Nadat de afbeelding is ingeladen wordt deze functie opgeroepen
$(window).on("backstretch.after", function (e, instance, index) {
    //De code in de if wordt enkel uitgevoerd na de eerste afbeelding, zodat een slideshow later geen problemen geeft
    if (index === 0) {
        $('header').css({
            "box-shadow": "inset 0 -10px 20px 0 #000"
        });
        //TODO: Dit kan beter vervangen door een reeks animaties zodat de tekst pas erin komt nadat het logo centraal staat
        $('#logoHolder').animate({ top: "0", opacity: "1" }, { duration: 1000 });
        $('#subnav').fadeIn(2000);
        //backgroundCssTop = parseFloat(instance.$img.css('top'), 10);
    }
});

$(document).ready(function () {
    $('#navbar').hide();

    $('header').height($(window).height());

    $(window).resize(function () {
        $('header').height($(window).innerHeight);
    });
    var temp = 0;
    function test() {
        $('header').backstretch("pause");
        var scrollTop = $(window).scrollTop();
        var coords = (scrollTop / 2) + 'px';
        $('header img').css({
            transform: 'translate3d(0,' + coords + ',0)',
            '-webkit-transform': 'translate3d(0,' + coords + ',0)'
        });

        if (scrollTop >= $('#content').offset().top - ($('#navbar').height())) {
            $('#navbar').show();
        } else {
            $('#navbar').hide();
        }
    }

    document.addEventListener('gesturechange', function () {
        //alert('gesturechagne');
        test();
    });

    document.addEventListener("touchmove", function () {
        //alert('touchmove');
        test();
    });

    $(window).scroll(function () {
        test();
    });

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

    // Scroll on click
    $(document).on('click', 'header .scrollToHref', function (e) {
        if (!($('html body').is('.error404'))) {
            e.preventDefault();
            var href = e.target.attributes['href'].nodeValue;
            var distance = $(href).offset().top - $('#navbar').height() - 20;
            $('html,body').animate({ scrollTop: distance }, { duration: 1000 });
        }
    });

    //https://api.foursquare.com/v2/venues/4d7be2a0f260a093e61f30ba/herenow?client_id=3UFSGCLAXPLGASVZIWDYEF3NL24SDC1RYYS1DMKDTGFJQJ1L&client_secret=XK4WTKZHT2HTUP3OTBCX1G55IST1X2ZQHQNG1GDVYBB2KHWH&v=20130827


    //$("#crown").rotate(-15);
});