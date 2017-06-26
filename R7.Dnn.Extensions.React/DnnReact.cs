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
    /// Handles initial React configuration for DNN extensions
    /// </summary>
    public static class DnnReact
    {
        static readonly object dnnReactSyncRoot = new object ();

        public static DnnReactConfig Config;

        static void Configure ()
        {
            Config = LoadDnnReactConfig ();

            // HACK: Preferred engine should be the first one
            AddJsEngineByNameHack (JsEngineSwitcher.Instance, Config.JavaScriptEngine.EngineName);
            // FIXME: Should be sufficent, but it's not, see https://github.com/reactjs/React.NET/pull/413
            JsEngineSwitcher.Instance.DefaultEngineName = Config.JavaScriptEngine.EngineName;

            var reactConfig = ReactSiteConfiguration.Configuration;
            reactConfig.SetLoadBabel (false);
            reactConfig.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver ();
            reactConfig.ReuseJavaScriptEngines = true;

            reactConfig.SetStartEngines (Config.JavaScriptEngine.StartEngines);
            reactConfig.SetMaxEngines (Config.JavaScriptEngine.MaxEngines);
        }

        static DnnReactConfig LoadDnnReactConfig ()
        {
            // TODO: ExtensionYamlConfig<T> should allow direct deserialization
#if DEBUG
            var configFileName = "React.Debug.yml";
            if (!File.Exists (Path.Combine (Globals.ApplicationMapPath, configFileName))) {
                configFileName = "React.yml";
            }
#else
            var configFileName = "React.yml";
#endif
            return new ExtensionYamlConfig<DnnReactConfig> (
                Path.Combine (Globals.ApplicationMapPath, configFileName), cfg => {
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

        public static void AddScriptWithoutTransform (string fileName)
        {
            EnsureConfigured ();

            lock (dnnReactSyncRoot) {
                ReactSiteConfiguration.Configuration.AddScriptWithoutTransform (fileName);
            }
        }

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

        #region Public render methods

        public static IHtmlString React<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.React (null, componentName, props, htmlTag, containerId, Config.Rendering.GetEffectiveClientOnly (clientOnly), Config.Rendering.GetEffectiveServerOnly (serverOnly), containerClass);
        }

        public static IHtmlString ReactWithInit<T> (string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, string containerClass = null)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactWithInit (null, componentName, props, htmlTag, containerId, Config.Rendering.GetEffectiveClientOnly (clientOnly), containerClass);
        }

        public static IHtmlString ReactInitJavaScript (bool clientOnly = false)
        {
            return global::React.Web.Mvc.HtmlHelperExtensions.ReactInitJavaScript (null, Config.Rendering.GetEffectiveClientOnly (clientOnly));
        }

        #endregion
    }
}
