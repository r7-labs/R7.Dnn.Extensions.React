//
//  DnnReact.cs
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

using System.IO;
using System.Web;
using DotNetNuke.Common;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using JavaScriptEngineSwitcher.Jurassic;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.V8;
using Newtonsoft.Json.Serialization;
using R7.Dnn.Extensions.Configuration;
using React;

namespace R7.Dnn.Extensions.React
{
    /// <summary>
    /// Handles initial React configuration for DNN extensions and React components rendering.
    /// </summary>
    public static class DnnReact
    {
        static readonly object dnnReactSyncRoot = new object ();

        /// <summary>
        /// Current <see cref="R7.Dnn.Extensions.React" /> configuration.
        /// </summary>
        public static DnnReactConfig Config;

        static void Configure ()
        {
            Config = LoadDnnReactConfig ();

            // HACK: Preferred engine should be the first one
            AddJsEngineByNameHack (JsEngineSwitcher.Instance, Config.JavaScriptEngine.EngineName);
            // FIXME: Should be sufficent, but it's not, see https://github.com/reactjs/React.NET/pull/413
            JsEngineSwitcher.Instance.DefaultEngineName = Config.JavaScriptEngine.EngineName;

            var reactConfig = ReactSiteConfiguration.Configuration;
            reactConfig.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver ();
            reactConfig.SetReuseJavaScriptEngines (Config.JavaScriptEngine.ReuseEngines);
            reactConfig.SetStartEngines (Config.JavaScriptEngine.StartEngines);
            reactConfig.SetMaxEngines (Config.JavaScriptEngine.MaxEngines);
            reactConfig.SetLoadBabel (Config.LoadBabel);

            if (Config.DisableServerSideRendering) {
                reactConfig.DisableServerSideRendering ();
            }
        }

        static DnnReactConfig LoadDnnReactConfig ()
        {
            // TODO: Deserialize config w/o calling GetInstance(0)?
            return new ExtensionYamlConfig<DnnReactConfig> (
                Path.Combine (Globals.ApplicationMapPath, "R7.Dnn.Extensions.React.yml"), cfg => {
                    return cfg;
                }
            ).GetInstance (0);
        }

        static void AddJsEngineByNameHack (JsEngineSwitcher engineSwitcher, string engineName)
        {
            engineSwitcher.EngineFactories.Clear ();

            switch (engineName) {
                case JurassicJsEngine.EngineName:
                    engineSwitcher.EngineFactories.AddJurassic ();
                    break;
                case JintJsEngine.EngineName:
                    engineSwitcher.EngineFactories.AddJint ();
                    break;
                case MsieJsEngine.EngineName:
                    engineSwitcher.EngineFactories.AddMsie ();
                    break;
                case ChakraCoreJsEngine.EngineName:
                    engineSwitcher.EngineFactories.AddChakraCore ();
                    break;
                case V8JsEngine.EngineName:
                    engineSwitcher.EngineFactories.AddV8 ();
                    break;
            }
        }

        static void EnsureConfigured ()
        {
            if (Config == null) {
                lock (dnnReactSyncRoot) {
                    if (Config == null) {
                        Configure ();
                    }
                }
            }
        }

        #region Public members

        /// <summary>
        /// Register single precompiled script.
        /// </summary>
        /// <param name="fileName">Script file name.</param>
        public static void AddScriptWithoutTransform (string fileName)
        {
            EnsureConfigured ();

            lock (dnnReactSyncRoot) {
                ReactSiteConfiguration.Configuration.AddScriptWithoutTransform (fileName);
            }
        }

        /// <summary>
        /// Registers multiple precompiled scripts.
        /// </summary>
        /// <param name="fileNames">File names.</param>
        public static void AddScriptsWithoutTransform (params string [] fileNames)
        {
            EnsureConfigured ();

            lock (dnnReactSyncRoot) {
                foreach (var fileName in fileNames) {
                    ReactSiteConfiguration.Configuration.AddScriptWithoutTransform (fileName);
                }
            }
        }

        #endregion

        #region Public rendering methods

        /// <summary>
        /// Renders the specified React component.
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialize the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <param name="serverOnly">Skip rendering React specific data-attributes during server side rendering. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString React<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.React (null, componentName, props, htmlTag, containerId, clientOnly, serverOnly, containerClass);
        }

        /// <summary>
        /// Renders the specified React component, along with its client-side initialization code.
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialize the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString ReactWithInit<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactWithInit (null, componentName, props, htmlTag, containerId, clientOnly, containerClass);
        }

        /// <summary>
        /// Renders the JavaScript required to initialize all components client-side.
        /// This will attach event handlers to the server-rendered HTML.
        /// </summary>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialization code. Defaults to <c>false</c></param>
        /// <returns>JavaScript for all components</returns>
        public static IHtmlString ReactInitJavaScript (bool clientOnly = false)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactInitJavaScript (null, clientOnly);
        }

        #endregion
    }
}
