window.instatus = {
    visible: function (el, condition) {
        if (condition) {
            el.removeAttr('hidden').show();
        } else {
            el.attr('hidden', 'hidden').hide();
        }
    },
    ajax: function (event) {
        event.preventDefault();

        var config = $.extend({
            target: 'body',
            empty: true
        }, event.data);

        var el = $(this);
        var target = $(config.target);
        var container = config.container ? $(config.container) : target;

        var insert = function (html) {
            if (config.empty)
                container.empty();

            container.append(html);
            instatus.visible(target, true);
        };

        var deferred;

        if (el.is('form')) {
            deferred = $.post(el.attr('action'), el.serialize())
                .done(insert);
        } else {
            deferred = $.get(el.attr('href'))
                .done(insert);
        }

        if (config.addCallbacks)
            config.addCallbacks(deferred);
    }
};