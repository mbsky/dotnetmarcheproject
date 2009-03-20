/// <reference path="jquery.js">
$(document).ready(function() {

   $('div[id^=photo] img').log('Images Thumbnail')
      .draggable({
         helper: 'clone',
         opacity: 0.5
      })
      .droppable({
         accept: 'div[id^=photo] img',
         hoverClass: 'drophoverimg',
         drop: function(event, ui) {
//            console.log('%o %o', event, ui);
            var first = ui.draggable.parent()[0];
            var second = $(this).parent()[0];
            DotNetMarche.PhotoAlbum.Ui.AspNet.Services.PhotoManager
                  .SwapPhotoPosition(
                      first.id.substring(6),
                      second.id.substring(6),
                      function(result, context, method) {
                         if (result) {
                            //call succeeded we need to swap the image in the DOM
                            $(first).swap(second);
                         } else {
                            alert('Error during save.');
                         }
                      },
                    function(error, context, method) {
                       alert('Exception during the save.');
                    });
         }
      });
});

$(function() {
   $('div[id^=photo] input').log('edit buttons').hide();

   //Edit photo functionality
   $('div[id^=photo] span')
      .click(function() {
         //We must begin editing of the field.
         //Grab id of the photo from the name of the div.
         var photoid = $(this).parent()[0].id.substring(6);
         //Find the actual text of the photo.
         var span = $(this).fadeOut(500, function() { editbox.fadeIn(500); });
         var editbox = $('<textarea id="editCurrent" photoid="' + photoid + '">' + span.text() + '</textarea>')
               .insertAfter(this)
               .keydown(function(event) {

                  switch (event.which) {
                     case 27: //The esc button stop editing revert all changes
                        this.endedit();
                        break;
                     case 13: //Return, save the changes.
                        //Invoke webservice thanks to microsoft ajax
                        event.stopPropagation();
                        DotNetMarche.PhotoAlbum.Ui.AspNet.Services.PhotoManager
                           .ChangePhotoDescription(photoid, this.value,
                               function(result, context, method) {
                                  //call succeeded
                                  if (result) {
                                     span.text(context.value);
                                     context.endedit();
                                  } else {
                                     alert('Error during save.');
                                  }
                               },
                             function(error, context, method) {
                                alert('Exception during the save.');
                             }, this);
                        return false; //abort the result of the event.
                  }
               })
               .hide();
         editbox[0].endedit = function() {
            $(editbox).fadeOut(500, function() {
               span.fadeIn(500);
               editbox.remove();
            });
         };
      });
});

// $(function() {
//   $('#listOfPhotoAlbum').
// }  