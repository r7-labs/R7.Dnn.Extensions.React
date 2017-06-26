//
//  DnnReactConfig.cs
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

using YamlDotNet.Serialization;
          
namespace R7.Dnn.Extensions.React
{
    /// <summary>
    /// <see cref="R7.Dnn.Extensions.React" /> configuration.
    /// </summary>
    public class DnnReactConfig
    {
        /// <summary>
        /// Disables server-side rendering. This is useful when debugging your scripts.
        /// </summary>
        public bool DisableServerSideRendering { get; set; } = false;

        /// <summary>
        /// Babel configuration section.
        /// </summary>
        public BabelConfig Babel { get; set; } = new BabelConfig ();

        /// <summary>
        /// JavaScript engine configuration section.
        /// </summary>
        [YamlMember (typeof (JavaScriptEngineConfig), Alias = "javascript-engine")]
        public JavaScriptEngineConfig JavaScriptEngine { get; set; } = new JavaScriptEngineConfig ();
    }

    /// <summary>
    /// JavaScript engine configuration.
    /// </summary>
    public class JavaScriptEngineConfig
    {
        /// <summary>
        /// JavaScript engine name to use.
        /// </summary>
        public string DefaultEngineName { get; set; } = "JurassicJsEngine";

        /// <summary>
        /// Starting number of JavaScript engines in the pool.
        /// </summary>
        public int StartEngines { get; set; } = 10;

        /// <summary>
        /// Maximum number of JavaScript engines in the pool.
        /// </summary>
        public int MaxEngines { get; set; } = 25;

        /// <summary>
        /// Whether JavaScript engines should be reused across requests.
        /// </summary>
        public bool ReuseEngines { get; set; } = true;
    }

    /// <summary>
    /// Babel configuration.
    /// </summary>
    public class BabelConfig
    {
        /// <summary>
        /// Whether Babel should be loaded to transform JSX to JavaScript.
        /// </summary>
        public bool LoadBabel { get; set; } = false;
    }
}
