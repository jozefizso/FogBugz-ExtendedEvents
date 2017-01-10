using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Vereyon.Web
{
    public class UrlOrBase64DataCheckHandler : IHtmlAttributeSanitizer
    {
        public SanitizerOperation CheckAttribute(HtmlAttribute attribute)
        {
            // Check the url. We assume that there's no use in keeping for example a link tag without a href, so flatten the tag on failure.
            if (!HtmlSanitizer.AttributeUrlCheck(attribute))
            {
                if (!HtmlSanitizer.AttributeBase64Check(attribute))
                {
                    return SanitizerOperation.FlattenTag;
                }
            }

            return SanitizerOperation.DoNothing;
        }
    }
}
