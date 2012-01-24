(function ($) {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };

    $.fn.bootstrap = function (selector) {
        var descendant = this.find(selector);
        if (descendant.is(':input'))
            descendant.trigger('change');
        else
            descendant.trigger('click');
        return this;
    };

    function selector(context, selector, deflt) {
        return $.isFunction(selector) ? selector(context) : selector ? $(selector) : $(deflt);
    }

    function state(event, deflt) {
        event.preventDefault();
        var s = $.extend(deflt, event.data);
        s.el = $(event.currentTarget);
        s.tagName = s.el.get(0).tagName;
        s.uri = s.el.attr('action') || s.el.attr('href') || s.el.attr('src');
        s.target = selector(s.el, s.target, s.uri && s.uri.startsWith('#') ? s.uri : event.delegateTarget);
        s.container = selector(s.el, s.container, s.target);
        return s;
    }

    function visible(el, condition) {
        if (condition) {
            el.removeAttr('hidden').show();
        } else {
            el.attr('hidden', 'hidden').hide();
        }
    }

    function close(event) {
        var s = state(event);
        visible(s.target, false);
    }

    function submit(event) {
        var s = state(event);
        s.target.submit();
    }

    function toggle(event) {
        var s = state(event, {
            container: function (el) { return el.closest('section, form'); }
        });

        visible(s.target, true);
        visible(s.container, false);
    }

    function button(event) {
        $(this).css('cursor', 'pointer');
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
        var s = state(event, {
            empty: true,
            track: true,
            validate: validate,
            hint: hint,
            addCallbacks: null
        });

        var insert = function (html) {
            if (s.empty)
                s.container.empty();

            s.container.append(html);
            visible(s.target, true);
        };

        var track = function () {
            if (s.track)
                trackPageview(s.uri);
        }

        if (!$.isFunction(s.validate) || s.validate(s)) {
            var deferred;

            if (s.el.is('form')) {
                deferred = $.post(s.uri, s.el.serialize());
            } else {
                deferred = $.get(s.uri);
            }

            deferred
                .done(insert)
                .done(track);

            if ($.isFunction(s.addCallbacks))
                s.addCallbacks(deferred);
        }
    }

    function hint(el, messages) {
        alert(messages[0]);
    }

    function message(input, prefix, suffix) {
        return input.attr('title') || prefix + ' ' + input.attr('placeholder') + ' ' + suffix;
    }

    function messages(form) {
        var messages = [];
        form.find(':input').each(function () {
            var input = $(this);
            var val = input.val();
            var m;
            if (input.is('[required]') && !required(val))
                m = message(input, '', 'is required');
            if (input.is('[type=email]') && !email(val))
                m = message(input, '', 'is not valid');

            if (m) {
                input.addClass('error');
                messages.push(m);
            }
            else {
                input.removeClass('error');
            }
        });
        return messages;
    }

    function validate(s) {
        var m = messages(s.el);
        if (m.length > 0) {
            s.hint(s.el, m);
            return false;
        } else {
            return true;
        }
    }

    function routeData() {
        var body = $('body');
        return {
            id: body.data('routeId'),
            controller: body.data('routeController'),
            action: body.data('routeAction')
        };
    }

    function trackPageview(uri) {
        if (window._gaq)
            _gaq.push(['_trackPageview', uri]);
    }

    function trackEvent(event) {
        if (window._gaq)
            _gaq.push(['_trackEvent', event.data.category, event.data.action, $(this).attr('href')]);
    }

    function back() {
        history.back();
    }

    function required(val) {
        return val && (val + '').length > 0 && val != 'null';
    }

    function email(val) {
        return val && val.indexOf('@') != -1;
    }

    window.instatus = {
        behaviour: {
            accordion: accordion,
            ajax: ajax,
            close: close,
            toggle: toggle,
            back: back,
            button: button,
            submit: submit
        },
        routeData: routeData,
        track: {
            view: trackPageview,
            event: trackEvent
        },
        validator: {
            required: required,
            email: email
        }
    };
})(jQuery);