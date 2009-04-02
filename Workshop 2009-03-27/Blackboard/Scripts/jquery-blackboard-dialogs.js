/*!
*/
(function() {
    $(document).ready(function() {
        $('#btnOpen').asyncdialog(
            {
                title: 'Apri presentazione',
                url: services['openfiledialog'],
                showbuttons: false,
                width: 600,
                height: 400,
                onready: function() {
                    // setup template
                    $('#filelist').setTemplateElement('tplFileEnty');

                    // request file list
                    jQuery.getJSON(
                        services['listfiles'],
                        {},
                        function(data) {
                            // render...
                            $('#filelist').processTemplate(data);
                        }
                    );
                }
            });



        //
        // File entry handlers
        //
        $('.fileentry').live('mouseover', function() {
            $(this).addClass('file-hover');
        }).live('mouseout', function() {
            $(this).removeClass('file-hover');
        }).live('click', function() {
            var fname = $(this).children('.fileName').text();
            jQuery.ajax({
                url: services['getfile'],
                dataType: 'text',
                data: { "name": fname },
                success: function(data) {
                    $('#btnOpen').asyncdialog('close');
                    var slides = $.json.deserialize(data);
                    window.loadSlides(slides);

                    $('#name').val(fname);
                    $('#author').val(slides.author);
                }
            });
        });

        $('#propertiesDialog').dialog({
            bgiframe: true,
            autoOpen: false,
            height: 400,
            width: 500,
            modal: false,
            buttons: {
                'Save': function() {
                    var currentSpan = $(this).data('current');
                    currentSpan.text($('#spanPropertyText').val());
                    currentSpan.css('font-size', $('#ddlFontSize').val() + 'px');
                    currentSpan.css('color', '#' + window.selectedColor);

                    var jsonSpan = currentSpan.data('state');
                    jsonSpan.text = currentSpan.text();
                    jsonSpan.font_size = currentSpan.css('font-size');
                    jsonSpan.color = '#' + window.selectedColor;

                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            },
            close: function() {

            }
        });


        $('#btnSave').click(function() {
            $('#saveDialog').dialog('open');
        });


        $('#savePresentationForm').bind('submit', function() {
            $(this).ajaxSubmit(
            {
                dataType: 'json',        // 'xml', 'script', or 'json' (expected server response type)
                success: function(response) {
                    if (response.status == 'ok') {
                        $('#savedFileSize').text(response.filesize);
                        $('#saveConfirmDialog').dialog({
                            bgiframe: true,
                            modal: true,
                            buttons: {
                                Ok: function() {
                                    $(this).dialog('close');
                                }
                            }
                        });
                    }
                }
            });

            return false;
        });

        $("#saveDialog").dialog({
            bgiframe: true,
            autoOpen: false,
            height: 300,
            modal: true,
            buttons: {
                'Save': function() {
                    var fname = $('#name');
                    var author = $('#author');

                    if (fname.val() == '') {
                        fname.addClass('error').focus();
                        return false;
                    } else {
                        fname.removeClass('error');
                    }

                    if (author.val() == '') {
                        author.addClass('error').focus();
                        return false;
                    } else {
                        author.removeClass('error');
                    }

                    window.slideData.author = author.val();
                    $('#slidesource').val($.json.serialize(window.slideData));
                    $('#savePresentationForm').submit();

                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            },
            close: function() {

            }
        }); // btnSaveDialog


        $('#btnAddImage').asyncdialog(
            {
                title: 'Brose images',
                url: services['openimagedialog'],
                showbuttons: false,
                width: 800,
                height: 600,
                onready: function() {
                    // setup template
                    $('#imgList').setTemplateElement('tplImageEnty');

                    // request file list
                    jQuery.getJSON(
                        services['listimages'],
                        {},
                        function(data) {
                            // render...
                            $('#imgList').processTemplate(data);
                        }
                    );
                }
            });

    }); // document.ready
})()