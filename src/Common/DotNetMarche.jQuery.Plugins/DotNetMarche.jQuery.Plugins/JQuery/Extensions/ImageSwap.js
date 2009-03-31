/// <reference path="../jquery.js">

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

