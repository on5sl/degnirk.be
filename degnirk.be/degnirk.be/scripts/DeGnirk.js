//$('body').niceScroll({ mousescrollstep: 20, cursorborder: '0px', cursorwidth: '7px', cursoropacitymax: 0.5 });

$('document').ready(function () {
    var calendar = $('#calendar').calendar({
        events_source: [
            {
                "id": 293,
                "title": "Event 1",
                "url": "http://example.com",
                "class": "event-important",
                "start": 12039485678000, // Milliseconds
                "end": 1234576967000 // Milliseconds
            }
        ],
        language: 'nl-NL',
        view: 'month',
        tmpl_cache: false,
        day: '2013-03-12'
    });
});

(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o), m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})
(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-43474239-1', 'degnirk.be');
ga('send', 'pageview');