/// <reference path="jquery.js">

(function($) {

   $.fn.log = function(msg) {
      if (typeof (console) == "undefined") {
         console = { log: function() { } };
      }
      if (console) {
         console.log("%s: %o", msg, this);
      }
      return this;
   }
})(jQuery);

(function($) {

   $.fn.swap = function(b) {
      b = $(b)[0];
      var a = this[0];
      var t = a.parentNode.insertBefore(document.createTextNode(''), a);
      b.parentNode.insertBefore(a, b);
      t.parentNode.insertBefore(b, t);
      t.parentNode.removeChild(t);
      return this;
   };
})(jQuery);

(function($) {

   $.fn.setwait = function(options) {
      var settings = $.extend({
         slidecss: 'waitindicatormasx',
         imagecss: 'waitindicator',
         waitoffset: 200
      }, options || {});
      var context = this;
      //      debugger;
      context[0].timer = setTimeout(function() {
         var position = context.position();
         var thediv = context;
         var innerslide = $('<div style="width:' + thediv.width() + 'px; height:' + thediv.height() + 'px" class="' + settings.slidecss + '" />')
            .css('opacity', 0.5);
         var progress = $('<div style="width:' + thediv.width() + 'px; height:' + thediv.height() + 'px" class="' + settings.imagecss + '"/>');
         thediv.prepend(innerslide)
         thediv.prepend(progress);
         context[0].innerslide = innerslide;
         context[0].progress = progress;
      }, settings.waitoffset);
      return this;
   };
})(jQuery);

(function($) {

   $.fn.clearwait = function() {
      var context = this[0];
      if (context.timer) {
         clearTimeout(context.timer);
      }
      if (context.innerslide) {
         $(context.innerslide).remove();
         $(context.progress).remove();
      }
      return this;
   };
})(jQuery);

//Transictionto, plugin to make funny transiction between images
(function($) {

   $.fn.transictionto1 = function(options) {
      var settings = $.extend({
         type: 'fadein',
         time: 1000
      }, options || {});

      //wrap into div if no div is present.
      $(this).each(function() {
         if ($(this).parent('div').size() == 0) {
            $(this).wrap('<div></div>')
         }

         var position = $(this).position();
         var transiction;
         switch (settings.type) {
            case 'fadein':
               transiction = {
                  opacity: 'hide'
               };
               break;
            case 'puff':

               transiction = {
                  opacity: 'hide',
                  width: $(this).width() * 1.5,
                  height: $(this).height() * 1.5,
                  top: position.top - ($(this).height() * 1.5 / 2),
                  left: position.left - ($(this).width() * 1.5 / 2)
               };
               break;
            case 'wipeleft':
               transiction = {
                  opacity: 'hide',
                  left: 1000
               };
            case 'wipedown':
               transiction = {
                  opacity: 'hide',
                  top: 1000
               };
         }

         //now swap with background trick, first of all keep track of some original values
         var height = $(this).height();
         var width = $(this).width();
         var actualpos = $(this).css('position');
         $(this)
         .parent()
            .height(height)
            .width(width)
            .css('background-image', 'url(' + settings.destinationImage + ')')
            .css('background-repeat', 'no-repeat')
         .end()
         .css('position', 'absolute')
         .animate(transiction,
                options.time,
                'linear',
                function() {
                   this.src = settings.destinationImage;
                   $(this).show();
                   $(this).height(height);
                   $(this).width(width);
                   $(this).css('position', actualpos);
                });
      });

   };
})(jQuery);
