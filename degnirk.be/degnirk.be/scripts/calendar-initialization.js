$('document').ready(function () {
    var options = {
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
        tmpl_path: '../Content/',
        tmpl_cache: false,
        onAfterViewLoad: function (view) {
            $('.page-header h3').text(this.getTitle());
            $('.btn-group button').removeClass('active');
            $('button[data-calendar-view="' + view + '"]').addClass('active');
        },
        classes: {
            months: {
                general: 'label'
            }
        }
    };

    var calendar = $('#calendar').calendar(options);

    $('.btn-group button[data-calendar-nav]').each(function () {
        var $this = $(this);
        $this.click(function () {
            calendar.navigate($this.data('calendar-nav'));
        });
    });

    $('.btn-group button[data-calendar-view]').each(function () {
        var $this = $(this);
        $this.click(function () {
            calendar.view($this.data('calendar-view'));
        });
    });
});