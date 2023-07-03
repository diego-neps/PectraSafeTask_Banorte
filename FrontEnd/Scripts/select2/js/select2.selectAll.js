$.fn.select2.amd.define("SelectAllAdapter", [
    "select2/utils",
    "select2/dropdown",
    "select2/dropdown/attachBody",
    "select2/dropdown/attachContainer"
],
    function (Utils, Dropdown, AttachBody, AttachContainer) {

        // Decorate Dropdown with Search functionalities
        var dropdownWithActionBox = Utils.Decorate(Dropdown, AttachContainer);
        dropdownWithActionBox.prototype.render = function () {
            var $rendered = Dropdown.prototype.render.call(this);
            var self = this;
            if (self.options.options.actionsBox) {
                var actionsbox = $(
                    '<div class="s2-actionsbox" style="padding: 4px 8px;">' +
                    '<div class="btn-group btn-group-sm btn-block">' +
                    '<button type="button" id="s2-select-all" class="actions-btn s2-select-all btn btn-default">' +
                    'Seleccionar todos' +
                    '</button>' +
                    '<button type="button" id="s2-deselect-all" class="actions-btn s2-deselect-all btn btn-default">' +
                    'Ninguno' +
                    '</button>' +
                    '</div>' +
                    '</div>');

                $rendered.prepend(actionsbox);

                actionsbox.on('click', '.actions-btn', function (e) {

                    if ($(this).hasClass('s2-select-all')) {
                        var $results = $rendered.find('.select2-results__option[aria-selected=false]');

                        // Get all results that aren't selected
                        $results.each(function () {
                            // Get the data object for it
                            var data = Utils.GetData(this, 'data');

                            // Trigger the select event
                            self.trigger('select', {
                                data: data
                            });
                        });
                    } else {
                        // Trigger value changed with null value
                        self.$element.val(null);
                        self.$element.trigger('change');
                    }

                    self.trigger('close');
                });

            }
            return $rendered;
        };

        // Decorate the dropdown+search with necessary containers
        let adapter = Utils.Decorate(dropdownWithActionBox, AttachContainer);
        adapter = Utils.Decorate(adapter, AttachBody);
        return adapter;
    });