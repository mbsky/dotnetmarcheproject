/*!
* jQuery JavaScript Library v1.3.2
* http://jquery.com/
*
* Copyright (c) 2009 John Resig
* Dual licensed under the MIT and GPL licenses.
* http://docs.jquery.com/License
*
* Date: 2009-02-19 17:34:21 -0500 (Thu, 19 Feb 2009)
* Revision: 6246
*/
(function() {
    //
    // current presentation
    //
    window.slideData = null;

    // startup presentation (embedded)
    var demo = {
        "slides": [{
            "title": "Hello Blackboard",
            "background_image": "blackboard.jpg",
            "color": "white",
            "items": [{
                "type": "span",
                "text": "The Blackboard",
                "font_size": "36pt",
                "color": "white",
                "left": 194,
                "top": 92
            },
		{
		    "type": "span",
		    "text": "project",
		    "font_size": "20pt",
		    "color": "gray",
		    "left": 495,
		    "top": 173
		},
		{
		    "type": "img",
		    "src": "/content/libs/img/chalkboard.png",
		    "left": 123,
		    "top": 261
}]
}],
            "author": "andrea balducci"
        };

        //
        // Template for a new span
        //
        var spanTemplate = {
            "type": "span",
            "text": "Double click me!",
            "font_size": "large",
            "color": "white",
            "left": 283,
            "top": 268
        };

        //
        // Template for a new image
        //
        var imgTemplate = {
            "type": "img",
            "src": "/content/libs/img/edit.png",
            "left": 200,
            "top": 200
        };

        //
        // Slide template
        // 
        var slideTemplate = {
            "title": "Hello Blackboard",
            "background_image": "blackboard.jpg",
            "color": "white",
            "items": [{
                "type": "span",
                "text": "New slide",
                "font_size": "36pt",
                "color": "white",
                "left": 333,
                "top": 247
}]
            };


            // Array Remove - By John Resig (MIT Licensed)
            Array.prototype.remove = function(from, to) {
                var rest = this.slice((to || from) + 1 || this.length);
                this.length = from < 0 ? this.length + from : from;
                return this.push.apply(this, rest);
            };

            //
            // multiselect
            //
            var initPos = false;
            var collection = false;

            //
            // Instrument slide item
            //
            jQuery.fn.instrument = function() {
                return this.each(function() {
                    var node = $(this);
                    node.draggable(
                    {
                        helper: 'original',
                        start: function(e, ui) {
                            if ($(this).is('.ui-selected')) {
                                initPos = { x: parseInt(this.style.left, 10), y: parseInt(this.style.top, 10) };
                                collection = jQuery('.ui-selected:visible').not(this);
                                if (collection.size() == 0) {
                                    initPos = false;
                                    collection = false;
                                }
                            }
                        },
                        drag: function(e, ui) {
                            if (collection) {
                                var x = ui.position.left;
                                var y = ui.position.top;
                                collection.each(
                                       function() {
                                           this.style.left = parseInt(this.style.left, 10) + x - initPos.x + 'px';
                                           this.style.top = parseInt(this.style.top, 10) + y - initPos.y + 'px';
                                       }
                                   );
                                initPos = { x: x, y: y };
                            }
                        },
                        stop: function(e, ui) {
                            initPos = false;
                            collection = false;
                            var data = $(this).data('state');
                            data.left = ui.position.left;
                            data.top = ui.position.top;
                        },
                        containment: 'parent'
                    }
		    )
		    .dblclick(function(event) {
		        var self = $(this);
		        //$('#textColorPicker').ColorPickerSetColor(self.css('color'));
		        $('#spanPropertyText').val(self.text());
		        $('#ddlFontSize').val(self.css('font-size').replace('px', ''));
		        $('#textColorPicker').ColorPickerSetColor(self.css('color'));

		        $('#propertiesDialog').data('current', self).dialog('open');
		        event.stopPropagation();
		    });

                });
            };

            $(document).ready(function() {

                //
                // common controls..
                //
                var slideViewerCtrl = $('.slide-viewer');
                var navigatorCtrl = $('#navigator');

                //
                // setup Slide Viewer
                //
                slideViewerCtrl.dblclick(function(event) {
                    $('#btnPlay').click();
                    event.stopPropagation();
                }).data('slidemode', 'edit');

                //
                // Setup slider
                //
                navigatorCtrl.slider({
                    value: 0,
                    min: 0,
                    max: 10,
                    step: 1,
                    slide: function(event, ui) {
                        updateSlideNumber(ui.value, false);
                    }
                });

                //
                // Align left
                //
                $('#btnAlignLeft').click(function() {
                    var left = -1;

                    $('.ui-selected:visible').each(function() {
                        if (left == -1)
                            left = parseInt(this.style.left, 10);
                        else
                            left = Math.min(left, parseInt(this.style.left, 10));
                    }).each(function() {
                        this.style.left = left + 'px';
                        $(this).data('state').left = this.style.left;
                    });
                });

                //
                // Go to prev slide
                //
                $('#btnPrev').click(function() {
                    var value = navigatorCtrl.slider('option', 'value');
                    var min = navigatorCtrl.slider('option', 'min');
                    log('next min ' + min + ' value ' + value);
                    if (value > min) {
                        value = value - 1;
                        navigatorCtrl.slider('option', 'value', value);
                        updateSlideNumber(value, false);
                    }
                });

                //
                // Go to next slide
                //
                function NextSlide(animate) {
                    log('next');
                    var value = navigatorCtrl.slider('option', 'value');
                    var max = navigatorCtrl.slider('option', 'max');
                    if (value < max) {
                        value = value + 1;
                        log('call to update slide');
                        navigatorCtrl.slider('option', 'value', value);
                        updateSlideNumber(value, animate);
                    }
                    else if (value == max && isPlaying()) {
                        log('Stop');
                        // stop 
                        $('#btnPlay').click();
                        log('end stop');
                    } else {
                        log('Ignore');
                    }
                    log('end next slide');
                }

                //
                // Go to next slide without animation
                //
                $('#btnNext').click(function() {
                    NextSlide(false);
                });

                //
                //
                //
                $('#btnToggleLock').click(function() {
                    if ($(this).hasClass('ui-state-active')) {
                    }
                    else {
                    }
                });

                //
                //
                //
                $('#accordion').accordion({ fillSpace: true, header: 'h3', autoHeight: false, collapsible: true });

                //
                // Update slide number
                //
                function updateSlideNumber(value, animate) {

                    if (value === undefined) {
                        value = navigatorCtrl.slider('option', 'value');
                    };

                    if (window && window.log) log("updating slide " + value);

                    $("#slidenumber").text(value + 1);
                    createSlide(window.slideData.slides, value, animate);
                };


                //
                // Start / stop presentation
                //
                $('#btnPlay').click(function() {

                    var slidemode = slideViewerCtrl.data('slidemode');

                    $('#toolbar-pane > div').slideToggle('slow');
                    $('#navigator-pane').slideToggle('slow');
                    $('#title > h1').toggle('slow');

                    log('current play mode is ' + slidemode);

                    if (slidemode != 'play') {
                        slideViewerCtrl.data('slidemode', 'play');
                        slideViewerCtrl.focus();
                        log('new play mode is play');
                    }
                    else {
                        slideViewerCtrl.data('slidemode', 'edit');
                        log('new play mode is edit');
                    }
                });

                //
                // Create new span item
                //
                function createNewSpan(data) {
                    return $(document.createElement('span'))
	                        .attr('class', 'slide-element')
	                        .text(data.text)
	                        .css('font-size', data.font_size)
	                        .css('color', data.color)
	                        .css('left', data.left)
	                        .css('top', data.top)
	                        .data('state', data);
                }

                //
                // Create new image item
                //
                function createNewImage(data) {
                    return $(document.createElement('img'))
	                        .attr('class', 'slide-element')
	                        .attr('src', data.src)
	                        .css('left', data.left)
	                        .css('top', data.top)
	                        .data('state', data);
                }

                //
                // Update current slide
                //
                function createSlide(slides, index, animate) {
                    var data = slides[index];
                    var currentSlide = slideViewerCtrl.children('div:first');

                    if (window && window.log) log("updating slide " + index + " with data " + data);

                    var imgpath = 'url(' + serverUrls['images'] + data.background_image + ')';

                    var newContent = $(document.createElement('div'))
	            .attr('class', 'slide')
                .css('background-image', imgpath)
                .css('color', data.color)
	            .css('z-index', 1000 + index)
	            .attr('id', 'slideNr' + index)
	            .selectable({ delay: 30 })
	            .dblclick(function(event) {
	                $('#btnPlay').click();
	                event.stopPropagation();
	            });

                    if (data.items !== undefined) {
                        jQuery.each(data.items, function() {
                            if (this.type == 'span') {
                                createNewSpan(this).appendTo(newContent);
                            } else if (this.type == 'img') {
                                createNewImage(this).appendTo(newContent);
                            }
                        });
                    }

                    if (animate == true) {
                        log('animating');
                        newContent.hide().appendTo(slideViewerCtrl);

                        currentSlide.fadeOut(2000, function() {
                            $(this).remove();
                            newContent.fadeIn(2000);
                        });
                    }
                    else {
                        slideViewerCtrl.append(newContent);
                        currentSlide.remove();
                    }

                    // instrument!
                    newContent.children().instrument();

                    // set current slide...
                    slideViewerCtrl.data('current', data);
                }


                //
                // add new element to current slide
                //
                function addToSlide(elem) {
                    var currentSlide = slideViewerCtrl.data('current');

                    if (currentSlide.items === undefined) {
                        currentSlide.items = new [];
                    }

                    currentSlide.items.push(elem);
                }


                //
                // Load slides (json)
                //
                window.loadSlides = function(data) {
                    window.slideData = data;
                    updateSlideNavigator();
                    updateSlideNumber(0);
                }

                function updateSlideNavigator() {
                    navigatorCtrl.slider('option', 'max', window.slideData.slides.length - 1);
                    navigatorCtrl.slider('option', 'value', 0);
                }

                //
                // Check if the viewer is in playmode
                //
                function isPlaying() {
                    return slideViewerCtrl.data('slidemode') == 'play';
                }

                //
                // Handle presentation mode - Enter -> next slide
                //
                $(window).keydown(function(event) {
                    if (!isPlaying())
                        return;

                    switch (event.keyCode) {
                        case 13:
                            {
                                event.stopPropagation();
                                NextSlide(true);
                            }
                            break;
                    }
                });

                //
                // Delete selected items
                //
                $('#btnDeleteSelected').click(function() {
                    var currentSlide = slideViewerCtrl.data('current');

                    $('.ui-selected').each(function() {
                        var self = $(this);
                        var itemData = self.data('state');
                        // remove from slide object
                        for (var i = 0; i < currentSlide.items.length; i++) {
                            if (currentSlide.items[i] == itemData) {
                                currentSlide.items.remove(i);
                                break;
                            }
                        }

                        // remove from dom
                        self.remove();
                    });
                });

                //
                //
                //
                function cloneJSON(o) {
                    return $.json.deserialize($.json.serialize(o));
                };

                //
                // Add text element
                //
                $('#btnAddText').click(function() {
                    var newSpan = cloneJSON(spanTemplate);
                    createNewSpan(newSpan)
                .appendTo(slideViewerCtrl.children('div:first'))
                .instrument();

                    addToSlide(newSpan);
                });

                //
                // add an image
                //
                function addImage(src) {
                    var newImage = cloneJSON(imgTemplate);
                    newImage.src = src;

                    createNewImage(newImage)
                .appendTo(slideViewerCtrl.children('div:first'))
                .instrument();

                    addToSlide(newImage);
                }

                //
                // Image entry handlers
                //
                $('.imageentry').live('mouseover', function() {
                    $(this).addClass('imageentry-hover');
                }).live('mouseout', function() {
                    $(this).removeClass('imageentry-hover');
                }).live('click', function() {
                    $('#btnAddImage').asyncdialog('close');
                    var src = $(this).children('img').attr('src');

                    addImage(src);
                });

                //
                // Color  picker
                //
                $('#textColorPicker').ColorPicker({
                    flat: true,
                    onSubmit: function(hsb, hex, rgb) {
                        window.selectedColor = hex;
                    }
                });

                //
                // btnAddSlide
                //
                $('#btnAddSlide').click(function() {
                    var newSlide = cloneJSON(slideTemplate);
                    window.slideData.slides.push(newSlide);
                    updateSlideNavigator();

                    var lastSlideIdx = window.slideData.slides.length - 1;
                    newSlide.items[0].text = 'Slide ' + (lastSlideIdx+1);

                    navigatorCtrl.slider('option', 'value', lastSlideIdx);
                    updateSlideNumber(lastSlideIdx, false);
                });


                //
                // start!
                //
                window.loadSlides(demo);

                window.log = function(text) {
                    if (window.console && window.console.log) {
                        window.console.log(text);
                    }
                }
            }
);
        })();