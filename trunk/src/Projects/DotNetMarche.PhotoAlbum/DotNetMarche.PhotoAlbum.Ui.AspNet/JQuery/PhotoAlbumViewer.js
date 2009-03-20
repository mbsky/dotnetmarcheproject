/// <reference path="jquery.js">
$(function() {

   $('#thumbstrip input').log('Thumbs')
   .click(function() {

      var newPhoto = $(this).attr('photoname');
      $('#thephoto img')
      .fadeOut(1000,
         function() {
   
            $(this).show().log();
            this.src = newPhoto;
         })
      .parent()
      .css('background', 'url(' + newPhoto + ')');
      //      $('#thephoto img')
      //       .hide();
      //       .after('<img src="' + $(this).attr('photoname') + '" />');

      return false;
   });
});