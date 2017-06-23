//
//  DnnHelperExtensions.cs
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
using DotNetNuke.Web.Mvc.Helpers;

namespace R7.Dnn.Extensions.React
{
    /// <summary>
    /// The <see cref="DotNetNuke.Web.Mvc.Helpers.DnnHelper" /> extension methods for React rendering
    /// </summary>
    public static class DnnHelperExtensions
    {
        public static IHtmlString React<T> (this DnnHelper dnnHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            return ReactRenderer.React (componentName, props, htmlTag, containerId, clientOnly, serverOnly, containerClass);
        }

        public static IHtmlString ReactWithInit<T> (this DnnHelper dnnHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            return ReactRenderer.ReactWithInit (componentName, props, htmlTag, containerId, clientOnly, containerClass);
        }

        public static IHtmlString ReactInitJavaScript (this DnnHelper dnnHelper, bool clientOnly = false)
        {
            return ReactRenderer.ReactInitJavaScript (clientOnly);
        }
    }
}
