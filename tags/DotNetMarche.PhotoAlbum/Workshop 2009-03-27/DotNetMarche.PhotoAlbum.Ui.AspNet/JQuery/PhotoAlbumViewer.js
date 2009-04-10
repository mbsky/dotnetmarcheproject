/// <reference path="jquery.js">
$(function() {
   var currentPhoto = 1;
   var thumbstrip = $('#thumbstrip input').log('Thumbs');
   var isInTransiction = false;
   var numOfPhoto = thumbstrip.size();

   thumbstrip.click(function() {
      if (isInTransiction) return false;
      isInTransiction = true;
      var newPhoto = $(this).attr('photoname');
      //preload
      var img = new Image();
      img.onload = function() {
         $('#thephoto img')
         .fadeOut(1000,
            function() {
               $(this).show().log();
               this.src = newPhoto;
               isInTransiction = false;
            })
         .parent()
         .css('background-image', 'url(' + newPhoto + ')');
      }
      currentPhoto = thumbstrip.index(this) + 1;
      img.src = newPhoto;
      return false;
   });

   var timerid;

   $('#viewercontrols').css('display', 'block');
   $('#viewercontrols input[id$=btnPrev]').click(
      function() { TransictionTo(currentPhoto - 1); return false; });
   $('#viewercontrols input[id$=btnNext]').click(
      function() { TransictionTo(currentPhoto + 1); return false; });
   $('#viewercontrols input[id$=btnStart]').click(
      function() {
         timerid = setInterval(function() {
            TransictionTo(currentPhoto + 1);
            if (currentPhoto == numOfPhoto)
               clearInterval(timerid);
         },
         3000);
         return false;
      });
   $('#viewercontrols input[id$=btnStop]').click(
      function() {
         clearInterval(timerid);
         return false;
      });

   $(document).keydown(function(e) {
      var newPhotoIndex;
      switch (e.keyCode) {

         case 39: //Forward
            TransictionTo(currentPhoto + 1);
            break;
         case 37:
            TransictionTo(currentPhoto - 1);
            break;
      }
   });
});

function TransictionTo(index) {

   $('#thumbstrip div:nth-child(' + index + ') input').click();
}

function MoveToNext(index) {
   TransictionTo(currentPhoto + 1);
}