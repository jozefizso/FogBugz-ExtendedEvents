using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Vereyon.Web
{
    public class AllowAttributeHandler : IHtmlAttributeSanitizer
    {
        public SanitizerOperation CheckAttribute(HtmlAttribute attribute)
        {
            return SanitizerOperation.DoNothing;
        }
    }
}
