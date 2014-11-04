using EPiServer;
using EPiServer.Cms.Shell.Extensions;
using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.Editor.TinyMCE;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyMceInCustomProperties.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(XhtmlString), EditorDescriptorBehavior = EditorDescriptorBehavior.ExtendBase, UIHint = Global.SiteUIHints.ExtendedXhtml)]
    public class ExtendedXhtmlEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            ExtendedMetadata extendedMetadatum = metadata.FindTopMostContentMetadata();

            IContent model;
            if (extendedMetadatum != null)
            {
                model = extendedMetadatum.Model as IContent;
            }
            else
            {
                model = null;
            }
            metadata.ClientEditingClass = "alloy/editors/ExtendedTinyMCEEditor";
            metadata.EditorConfiguration.Add("parentValue", parentValue(model, metadata.PropertyName));
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