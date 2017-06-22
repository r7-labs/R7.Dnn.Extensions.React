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

// TODO: Add React.NET licence: https://github.com/reactjs/React.NET/blob/master/LICENSE

using System.Web;
using React;
using React.Exceptions;
using React.TinyIoC;

namespace R7.MiniGallery.React
{
    public static class ReactRenderer
    {
        static IReactEnvironment Environment {
            get {
                IReactEnvironment current;
                try {
                    current = ReactEnvironment.Current;
                }
                catch (TinyIoCResolutionException innerException) {
                    throw new ReactNotInitialisedException ("ReactJS.NET has not been initialised correctly.", innerException);
                }
                return current;
            }
        }

        public static IHtmlString React<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            IHtmlString result;
            try {
                IReactComponent reactComponent = Environment.CreateComponent (componentName, props, containerId, clientOnly);
                if (!string.IsNullOrEmpty (htmlTag)) {
                    reactComponent.ContainerTag = htmlTag;
                }
                if (!string.IsNullOrEmpty (containerClass)) {
                    reactComponent.ContainerClass = containerClass;
                }
                result = new HtmlString (reactComponent.RenderHtml (clientOnly, serverOnly));
            }
            finally {
                Environment.ReturnEngineToPool ();
            }
            return result;
        }

        public static IHtmlString ReactInitJavaScript (bool clientOnly = false)
        {
            IHtmlString result;
            try {
                result = new HtmlString ("<script type=\"text/javascript\">" + Environment.GetInitJavaScript (clientOnly) + "</script>");
            }
            finally {
                Environment.ReturnEngineToPool ();
            }
            return result;
        }

        public static IHtmlString ReactWithInit<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            IHtmlString result;
            try {
                var reactComponent = Environment.CreateComponent (componentName, props, containerId, clientOnly);
                if (!string.IsNullOrEmpty (htmlTag)) {
                    reactComponent.ContainerTag = htmlTag;
                }
                if (!string.IsNullOrEmpty (containerClass)) {
                    reactComponent.ContainerClass = containerClass;
                }
                var html = reactComponent.RenderHtml (clientOnly, false);
                var tag = "<script type=\"text/javascript\">" + reactComponent.RenderJavaScript () + "</script>";
                result = new HtmlString (html + System.Environment.NewLine + tag);
            }
            finally {
                Environment.ReturnEngineToPool ();
            }
            return result;
        }
    }
}
