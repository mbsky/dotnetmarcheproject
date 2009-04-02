/*
* jQuery asyncDialog
* version: 1.0 
* Depends:
*	ui.dialog.js
*/
(function($) {
    $.widget('ui.asyncdialog', {
        _init: function() {

            var self = this,
			    options = this.options;

            this.element.click(function() { self.popup(); });
        },

        destroy: function() {
            this.element.unbind('click');
            this.valueDiv.remove();
            $.widget.prototype.destroy.apply(this, arguments);
        },

        title: function(newTitle) {
            arguments.length && this._setData("title", newTitle);
            return this._title();
        },

        _setData: function(key, value) {
            switch (key) {
                case 'title':
                    this.options.title = value;
                    this._refreshValue();
                    this._trigger('changetitle', null, {});
                    break;
            }

            $.widget.prototype._setData.apply(this, arguments);
        },

        _title: function() {
            var val = this.options.title;
            return val;
        },

        popup: function() {
            var tempDiv = $('<div title=\'' + this.title() + '\'><div class=\'ajaxloader\'></div></div>');
            var buttons = {};
            if (this.options.showbuttons == true) {
                buttons = {
                    "Cancel": function() {
                        $(this).dialog("close");
                    },
                    "Ok": function() {
                        $(this).dialog("close");
                    }
                };
            }

            tempDiv.dialog({
                bgiframe: true,
                modal: true,
                height: this.options.height,
                width: this.options.width,
                buttons: buttons,
                overlay: { backgroundColor: "#000", opacity: 0.7 },
                close: function(ev, ui) { $(this).remove(); }
            })
            .load(this.options.url, {}, this.options.onready);

            $(this).data('dialog', tempDiv);
        },

        close: function() {
            var dlg = $(this).data('dialog');

            if (dlg !== undefined) {
                dlg.dialog("close");
            }

            $(this).data('dialog', null);
        }
    });

    $.ui.asyncdialog.getter = "title";

    $.extend($.ui.asyncdialog, {
        version: "1.6rc5",
        defaults: {
            title: 'Dialog',
            url: '',
            height: 300,
            width: 460,
            onready: null,
            showbuttons: true
        }
    });

})(jQuery);
