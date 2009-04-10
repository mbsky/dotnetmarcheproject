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