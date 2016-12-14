using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Vereyon.Web
{
    public interface IHtmlAttributeSanitizer
    {
        SanitizerOperation CheckAttribute(HtmlAttribute attribute);
    }
}
