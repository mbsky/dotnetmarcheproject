/// <reference path="jquery.js">

$(function() {
   $('div[id^=photo] input').log('edit buttons').hide();

   $('div[id^=photo] span')
      .click(function() {
         //We must begin editing of the field.
         //Grab id of the photo from the name of the div.
         var photoid = $(this).parent()[0].id.substring(6);
         //Find the actual text of the photo.
         var span = $(this).hide();
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
               .show();
         editbox[0].endedit = function() {
            span.show();
            $(editbox).remove();
         };
      });
});