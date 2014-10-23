using System.Diagnostics;
using EPiServer;
using EPiServer.Cms.Shell.Extensions;
using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.Editor.TinyMCE;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyMceInCustomProperties.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(XhtmlString), UIHint = Global.SiteUIHints.ExtendedXhtml)]
    public class ExtendedXhtmlEditorDescriptor : EditorDescriptor
    {
        private IList<string> _nonLegacyPlugins;

        public ExtendedXhtmlEditorDescriptor()
        {
            ClientEditingClass = "alloy/editors/ExtendedTinyMCEEditor";
            List<string> strs = new List<string>()
            {
                "advhr",
                "advimage",
                "advlink",
                "advlist",
                "autoresize",
                "autosave",
                "bbcode",
                "contextmenu",
                "directionality",
                "emotions",
                "epiaccesskeysremove",
                "epiautoresize",
                "epicontentfragment",
                "epidynamiccontent",
                "epieditordisable",
                "epiexternaltoolbar",
                "epifilebrowser",
                "epifloatingtoolbar",
                "epiimageeditor",
                "epiimageresize",
                "epilink",
                "epipersonalizedcontent",
                "epiquote",
                "epistylematcher",
                "epitrailing",
                "epiwindowmanager",
                "epivisualaid",
                "fullpage",
                "fullscreen",
                "iespell",
                "inlinepopups",
                "insertdatetime",
                "layer",
                "legacyoutput",
                "lists",
                "media",
                "nonbreaking",
                "noneditable",
                "pagebreak",
                "paste",
                "preview",
                "print",
                "save",
                "searchreplace",
                "spellchecker",
                "style",
                "tabfocus",
                "table",
                "template",
                "visualchars",
                "wordcount",
                "xhtmlxtras"
            };
            this._nonLegacyPlugins = strs;
        }

        private void CorrectPluginsList(TinyMCEInitOptions options)
        {
            List<string> strs = new List<string>()
            {
                "epiexternaltoolbar"
            };
            List<string> strs1 = new List<string>()
            {
                "lists",
                "epicontentfragment"
            };
            string item = options.InitOptions["plugins"] as string;
            char[] chrArray = new char[] { ',' };
            IEnumerable<string> strs2 = item.Split(chrArray).Union<string>(strs1).Except<string>(strs).Intersect<string>(this._nonLegacyPlugins);
            options.InitOptions["plugins"] = string.Join(",", strs2);
        }

        private IEnumerable<string> GetLegacyPluginsList(TinyMCEInitOptions options)
        {
            string item = options.InitOptions["plugins"] as string;
            char[] chrArray = new char[] { ',' };
            return item.Split(chrArray).Except<string>(this._nonLegacyPlugins);
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            IContent model;
            base.ModifyMetadata(metadata, attributes);
            TinyMCESettings setting = (TinyMCESettings)((PropertyData)metadata.Model).GetSetting(typeof(TinyMCESettings));
            ExtendedMetadata extendedMetadatum = ((ContentDataMetadata)metadata).FindTopMostContentMetadata();
            if (extendedMetadatum != null)
            {
                model = extendedMetadatum.Model as IContent;
            }
            else
            {
                model = null;
            }
            TinyMCEInitOptions tinyMCEInitOption = new TinyMCEInitOptions((TinyMCEInitOptions.InitType)1, setting, model);
            IEnumerable<string> legacyPluginsList = this.GetLegacyPluginsList(tinyMCEInitOption);
            this.CorrectPluginsList(tinyMCEInitOption);
            if (metadata.IsReadOnly)
            {
                tinyMCEInitOption.InitOptions["readonly"] = true;
                tinyMCEInitOption.InitOptions["body_class"] = "mceReadOnly";
            }
            if (!tinyMCEInitOption.InitOptions.ContainsKey("schema"))
            {
                tinyMCEInitOption.InitOptions["schema"] = "html5";
            }
            metadata.EditorConfiguration.Add("width", tinyMCEInitOption.InitOptions["width"]);
            metadata.EditorConfiguration.Add("height", tinyMCEInitOption.InitOptions["height"]);
            metadata.EditorConfiguration.Add("settings", tinyMCEInitOption.InitOptions);
            metadata.EditorConfiguration.Add("legacyPlugins", legacyPluginsList);
            metadata.EditorConfiguration.Add("parentValue", parentValue(model, metadata.PropertyName));
            IDictionary<string, object> editorConfiguration = metadata.EditorConfiguration;
            string[] strArrays = new string[] { "fileurl", "link", "episerver.core.icontentdata" };
            editorConfiguration["AllowedDndTypes"] = strArrays;
            Dictionary<string, object> strs = new Dictionary<string, object>()
            {
                { "theme_advanced_toolbar_location", "external" },
                { "theme_advanced_path", false },
                { "theme_advanced_statusbar_location", "none" },
                { "body_class", "epi-inline" }
            };
            metadata.CustomEditorSettings["uiParams"] = new { inlineSettings = strs };
            metadata.CustomEditorSettings["uiType"] = "epi-cms.contentediting.editors.TinyMCEInlineEditor";
            metadata.CustomEditorSettings["uiWrapperType"] = "richtextinline";
        }

        private string parentValue(IContent page, string propertyName)
        {
            if (page.ParentLink != null)
            {
                var repro = ServiceLocator.Current.GetInstance<IContentRepository>();
                IContent parent;

                if (repro.TryGet(page.ParentLink, out parent))
                {
                    return ((ContentData)parent).GetPropertyValue(propertyName) ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}