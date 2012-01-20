(function () {
    function visible(el, condition) {
        if (condition) {
            el.removeAttr('hidden').show();
        } else {
            el.attr('hidden', 'hidden').hide();
        }
    }

    function toggle(event) {
        event.preventDefault();

        var config = $.extend({
            target: null,
            container: null
        }, event.data);

        var el = $(this);
        var href = el.attr('href');
        var target = config.target ? $(config.target) : $(href);
        var container = config.container ? $(config.container) : el.closest('section, form');

        visible(target, true);
        visible(container, false);
    }

    function accordion(event) {
        var el = $(this);
        var tagName = el.get(0).tagName;
        var content = el.next('div');
        var hidden = el.siblings().not(tagName).not(content);

        visible(hidden, false);
        visible(content, true);
    }

    function ajax(event) {
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
            visible(target, true);
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

    window.instatus = {
        behaviours: {
            accordion: accordion,
            ajax: ajax,
            toggle: toggle
        }
    };
})();