/* ========================================================
* bootstrap-tab.js v2.0.0
* http://twitter.github.com/bootstrap/javascript.html#tabs
* ========================================================
* Copyright 2012 Twitter, Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* ======================================================== */


!function ($) {

    "use strict"

    /* TAB CLASS DEFINITION
    * ==================== */

    var Tab = function (element) {
        this.element = $(element)
    }

    Tab.prototype = {

        constructor: Tab

  , show: function () {
      var $this = this.element
        , $ul = $this.closest('ul:not(.dropdown-menu)')
        , selector = $this.attr('data-target')
        , previous
        , $target

      if (!selector) {
          selector = $this.attr('href')
          selector = selector && selector.replace(/.*(?=#[^\s]*$)/, '') //strip for ie7
      }

      if ($this.parent('li').hasClass('active')) return

      previous = $ul.find('.active a').last()[0]

      $this.trigger({
          type: 'show'
      , relatedTarget: previous
      })

      $target = $(selector)

      this.activate($this.parent('li'), $ul)
      this.activate($target, $target.parent(), function () {
          $this.trigger({
              type: 'shown'
        , relatedTarget: previous
          })
      })
  }

  , activate: function (element, container, callback) {
      var $active = container.find('> .active')
        , transition = callback
            && $.support.transition
            && $active.hasClass('fade')

      function next() {
          $active
          .removeClass('active')
          .find('> .dropdown-menu > .active')
          .removeClass('active')

          element.addClass('active')

          if (transition) {
              element[0].offsetWidth // reflow for transition
              element.addClass('in')
          } else {
              element.removeClass('fade')
          }

          if (element.parent('.dropdown-menu')) {
              element.closest('li.dropdown').addClass('active')
          }

          callback && callback()
      }

      transition ?
        $active.one($.support.transition.end, next) :
        next()

      $active.removeClass('in')
  }
    }


    /* TAB PLUGIN DEFINITION
    * ===================== */

    $.fn.tab = function (option) {
        return this.each(function () {
            var $this = $(this)
        , data = $this.data('tab')
            if (!data) $this.data('tab', (data = new Tab(this)))
            if (typeof option == 'string') data[option]()
        })
    }

    $.fn.tab.Constructor = Tab


    /* TAB DATA-API
    * ============ */

    $(function () {
        $('body').on('click.tab.data-api', '[data-toggle="tab"], [data-toggle="pill"]', function (e) {
            e.preventDefault()
            $(this).tab('show')
        })
    })

} (window.jQuery)

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
            .find('div > label')
                .addClass('control-label')
                .next('span')
                    .addClass('controls')
                .end()
            .end()
            .find('.multiSelectList')
                .addClass('controls')
            .end()
            .children('button[type=submit]')
                .wrap('<div class=form-actions></div>')
                .addClass('btn btn-primary')
            .end()
            .find('input[type=text], textarea')
                .addClass('input-xlarge')
            .end()
            .find('.field-validation-error')
                .addClass('help-inline')
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