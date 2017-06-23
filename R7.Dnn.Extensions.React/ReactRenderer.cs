//
//  ReactRenderer.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Web;

namespace R7.Dnn.Extensions.React
{
    /// <summary>
    /// Renders react components and initialization code
    /// </summary>
    public static class ReactRenderer
    {
        public static IHtmlString React<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.React (null, componentName, props, htmlTag, containerId, clientOnly, serverOnly, containerClass);
        }

        public static IHtmlString ReactWithInit<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactWithInit (null, componentName, props, htmlTag, containerId, clientOnly, containerClass);
        }

        public static IHtmlString ReactInitJavaScript (bool clientOnly = false)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactInitJavaScript (null, clientOnly);
        }
    }
}
