define([
// epi
    "epi-cms/contentediting/editors/TinyMCEEditor",

// templates
    "dojo/text!./templates/ExtendedTinyMCEEditor.html"
],

function (
// epi
    TinyMCEEditor,

// templates
    template

) {

    return declare([TinyMCEEditor], {
        // summary:
        //      Widget for the tinyMCE editor.

        templateString: template,

        postCreate: function () {

            this.inherited(arguments);

            this.connect(this.inheritButton, "onclick", "onInheritButtonClick");
            this.connect(this.inheritFromParentRadioButton, "onclick", "onInheritRadioButtonClick");
            this.connect(this.useLocalValueRadioButton, "onclick", "onInheritRadioButtonClick");
        },

        onInheritButtonClick: function () {
            var text = this.divParentValue.innerHTML;
            var ed = this.getEditor();
            if (ed && ed.initialized) {
                ed.setContent(text);
            } else {
                this.editorFrame.value = text;
            }
            this._onChange(text);
        },

        onInheritRadioButtonClick: function () {
            var ed = this.getEditor();
            if (ed && ed.initialized) {
                this._onChange(ed.getContent(), true);
            }
        },

        _setValueAttr: function (newValue) {
            //summary:
            //    Value's setter
            //
            // tags:
            //    protected

            var isInheritting = editableValue.charAt(0) == "1",
                editableValue = this._unencodedValue(newValue || "");

            if (isInheritting) {
                this.inheritFromParentRadioButton.checked = true;
                this.useLocalValueRadioButton.checked = false;
            } else {
                this.useLocalValueRadioButton.checked = true;
                this.inheritFromParentRadioButton.checked = false;
            }

            this.inherited(arguments, [editableValue]);

        },

        _onChange: function (val, forcechange) {
            // summary:
            //    Raised when the editor's content is changed.
            //
            // val:
            //    The editor's changed value
            //
            // tags:
            //    callback public


            forcechange = !!forcechange;

            var hasChanged = this.get("_editorValue") !== val || forcechange === true;
            if (hasChanged) {
                this.set("_editorValue", val);
                val = this._encodedValue(val);
                this._set("value", val);
                if (this.validate()) {
                    this.set("_hasPendingChanges", false);
                    this.onChange(val);
                }
            }
        },

        _encodedValue: function (orgValue) {
            var useLocal = this.useLocalValueRadioButton.checked;

            return (useLocal ? "0:" : "1:") + orgValue;

        },

        _unencodedValue: function (orgValue) {
            var isSet = orgValue != null && orgValue !== undefined && orgValue.length >= 2 && orgValue.charAt(1) == ":";
            return isSet ? orgValue.substring(2) : orgValue;
        },

    });
});