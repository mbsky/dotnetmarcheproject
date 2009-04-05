/// <reference path="jquery.js">
$(document).ready(function() {
   EnableDragAndDropOperations();
});

$(function() {
   EnableEditInPlaceOperations();
   $('#photolist').ajaxComplete(function(request, settings) {
      EnableDragAndDropOperations();
      EnableEditInPlaceOperations();
   });
});



function EnableDragAndDropOperations() {
   $('div[id^=photo] img').log('Images Thumbnail')
      .draggable({
         helper: 'clone',
         opacity: 0.5
      })
      .droppable({
         accept: 'div[id^=photo] img',
         hoverClass: 'drophoverimg',
         drop: function(event, ui) {

            var first = ui.draggable.parent()[0];
            var second = $(this).parent()[0];
            $('#photolist').setwait();
            $.ajax({
               url: "/PhotoManager/SwapPhotoPosition",
               type: "POST",
               data: { photoId1: first.id.substring(6), photoId2: second.id.substring(6) },
               success: function(data, textStatus) {
                  console.log("%o - %o", data, textStatus);
                  if (data) {
                     $(first).swap(second);
                  } else {
                     alert('Error during save.');
                  }
               },
               error: function(XMLHttpRequest, textStatus, errorThrown) {
                  alert('Exception during the save.');
               },
               complete: function(XMLHttpRequest, textStatus) {
                  $('#photolist').clearwait();
               }
            });
         }
      });
}

function EnableEditInPlaceOperations() {

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
                        $(this).parent().setwait();

                        //Now making call to the right action in asp.netmvc page.
                        $.ajax({
                           url: "/PhotoManager/ChangePhotoDescription",
                           type: "POST",
                           data: { photoId: photoid, newPhotoDescription: this.value },
                           success: function(data, textStatus) {
                              console.log("%o - %o", data, textStatus);
                              if (data) {
                                 span.text(this.mycontext.value);
                                 this.mycontext.endedit();
                              } else {
                                 alert('Error during save.');
                              }
                           },
                           error: function(XMLHttpRequest, textStatus, errorThrown) {
                              alert('Exception during the save.');
                           },
                           complete: function(XMLHttpRequest, textStatus) {
                              $(this.mycontext).parent().clearwait();
                           },
                           mycontext: this
                        });

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
}