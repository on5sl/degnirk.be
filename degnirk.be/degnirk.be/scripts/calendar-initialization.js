$('document').ready(function () {
    var options = {
        events_source: '/umbraco/Surface/Calendar/GetEvents',
        language: 'nl-NL',
        tmpl_path: '../Content/',
        tmpl_cache: false,
        onAfterViewLoad: function (view) {
            $('h4.calendar-header').text(this.getTitle());
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