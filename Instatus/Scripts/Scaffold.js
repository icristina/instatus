$(function () {
    // re-theme min markup to have Bootstrap containers and styles
    var primaryNavigation = $('body').children('nav');
    var primaryNavigationList = primaryNavigation.find('ul');

    $('.current').addClass('active');

    primaryNavigation
        .prependTo('body')
        .wrap('<div class="navbar navbar-fixed-top"><div class=navbar-inner><div class=container></div></div></div>')
        .find('ul')
            .addClass('nav')
            .first()
                .append('<li class=divider-vertical>​</li>');

    $('body')
        .children('form, section, table')
            .wrap('<div class=container></div>')
        .end()
        .find('h1')
            .wrap('<div class=page-header></div>');

    $('body')
        .find('> form, div > form')
            .addClass('form-horizontal')
            .wrap('<div class=contains><div class=row></div></div>')
            .find('fieldset > div, > div')
                .addClass('control-group')
            .end()
            .find('label').addClass('control-label')
                .next(':input')
                    .wrap('<div class=controls></div>')
                .end()
            .end()
            .children('button[type=submit]')
                .wrap('<div class=form-actions></div>')
                .addClass('btn btn-primary')
            .end()
            .find('input[type=text], textarea')
                .addClass('input-xlarge')
            .end();

    $('table')
        .addClass('table table-striped')
        .find('a, button')
            .addClass('btn');

    $('#actions')
        .find('a')
            .addClass('btn btn-primary');

    $('#user')
        .prepend('<i class="icon-user icon-white"></i> ');

    $('#paging')
        .find('ul')
            .addClass('pagination')
        .end()
        .find('li.disabled')
            .wrapInner('<a></a>')
        .end()
        .find('b')
            .wrap('<a></a>')
            .closest('li')
                .addClass('active');
});