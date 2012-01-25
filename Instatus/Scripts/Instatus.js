(function ($) {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
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

    function placeholder() {
        if (window.Modernizr && Modernizr.input && Modernizr.input.placeholder)
            return;

        var el = $(this);
        var placeholder = el.attr('placeholder');

        if (!placeholder)
            return;

        el.focus(function () {
            el.removeClass('placeholder');
            if (el.val() == placeholder)
                el.val('');
        });

        var hint = function () {
            if (el.val() == '') {
                el.val(placeholder);
                el.addClass('placeholder');
            } else if (el.val() != placeholder) {
                el.removeClass('placeholder');
            }
        };

        el.blur(hint);
        hint();
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

    function aria(el, role, val) {
        el.attr('aria-' + role, val);

        if (val === true) {
            el.addClass(role);
        }
        else if (val === false) {
            el.removeClass(role);
        }
    }

    function ajax(event) {
        var s = state(event, {
            empty: true,
            track: true,
            validate: validate,
            alert: alert,
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

        var busy = function () {
            s.el.closest('form').find(':submit').each(function () {
                var button = $(this);

                if (!button.is(':has(span)'))
                    button.append('<span></span>');

                button.attr('disabled', 'disabled');
            });

            aria(s.target, 'busy', true);
        }

        var done = function () {
            s.el.closest('form').find(':submit').removeAttr('disabled');
            aria(s.target, 'busy', false);
        }

        if (!$.isFunction(s.validate) || s.validate(s)) {
            var deferred;

            if (s.el.is('form')) {
                deferred = $.post(s.uri, s.el.serialize());
            } else {
                deferred = $.get(s.uri);
            }

            busy();

            deferred
                .done(done)
                .done(insert)
                .done(track);

            if ($.isFunction(s.addCallbacks))
                s.addCallbacks(deferred);
        }
    }

    function alert(s) {
        window.alert(s.errors[0].message);
    }

    function format(formatString, val) {
        return formatString.replace('{0}', val);
    }

    function validate(s) {
        s.errors = [];
        s.el.find(':input').each(function () {
            var input = $(this);
            $.each(instatus.validator, function (name, validator) {
                if (input.is(validator.selector) && !validator.valid(input.val(), input)) {
                    s.errors.push({
                        input: input,
                        message: input.attr('title') || format(validator.message, input.attr('placeholder'))
                    });
                    input.addClass('error');
                    return false;
                } else {
                    input.removeClass('error');
                }
            });
        });
        if (s.errors.length > 0) {
            s.alert(s);
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

    window.instatus = {
        behaviour: {
            accordion: accordion,
            ajax: ajax,
            close: close,
            toggle: toggle,
            back: back,
            button: button,
            submit: submit,
            validate: validate
        },
        polyfill: {
            placeholder: placeholder
        },
        routeData: routeData,
        track: {
            view: trackPageview,
            event: trackEvent
        },
        validator: {
            required: {
                selector: '[required]',
                valid: function (val, input) { return !(val == input.attr('placeholder')) && val && (val + '').length > 0 && val != 'null'; },
                message: '{0} is required'
            },
            email: {
                selector: '[type=email]',
                valid: function (val) { return val && val.indexOf('@') != -1; },
                message: 'Email address is not valid'
            }
        }
    };
})(jQuery);