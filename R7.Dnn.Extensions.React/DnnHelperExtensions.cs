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
    /// The <see cref="DnnHelper" /> extension methods for React rendering.
    /// </summary>
    public static class DnnHelperExtensions
    {
        /// <summary>
        /// Renders the specified React component.
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="dnnHelper">DNN helper</param>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialize the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <param name="serverOnly">Skip rendering React specific data-attributes during server side rendering. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString React<T> (this DnnHelper dnnHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            return DnnReact.React (componentName, props, htmlTag, containerId, clientOnly, serverOnly, containerClass);
        }

        /// <summary>
        /// Renders the specified React component, along with its client-side initialization code.
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="dnnHelper">DNN helper</param>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialize the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString ReactWithInit<T> (this DnnHelper dnnHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            return DnnReact.ReactWithInit (componentName, props, htmlTag, containerId, clientOnly, containerClass);
        }

        /// <summary>
        /// Renders the JavaScript required to initialize all components client-side.
        /// This will attach event handlers to the server-rendered HTML.
        /// </summary>
        /// <param name="dnnHelper">DNN helper</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <returns>JavaScript for all components</returns>
        public static IHtmlString ReactInitJavaScript (this DnnHelper dnnHelper, bool clientOnly = false)
        {
            return DnnReact.ReactInitJavaScript (clientOnly);
        }
    }
}
