/// <reference path="jquery.js">

(function($) {

   $.fn.log = function(msg) {
      console.log("%s: %o", msg, this);
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

