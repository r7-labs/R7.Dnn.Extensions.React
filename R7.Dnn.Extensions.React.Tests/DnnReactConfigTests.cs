//
//  DnnReactConfigTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using R7.Dnn.Extensions.Configuration;
using Xunit;

namespace R7.Dnn.Extensions.React.Tests
{
    public class DnnReactConfigTests
    {
        [Fact]
        public void DnnReactConfigTest ()
        {
            var config = new ExtensionYamlConfig<DnnReactConfig> (
                Path.GetFullPath ("../../React.yml"), cfg => {
                    return cfg;
                }
            );

            Assert.NotNull (config.GetInstance (0).JavaScriptEngine);
            Assert.Equal ("JintJsEngine", config.GetInstance (0).JavaScriptEngine.DefaultEngineName);
            Assert.Equal (1, config.GetInstance (0).JavaScriptEngine.StartEngines);
            Assert.Equal (50, config.GetInstance (0).JavaScriptEngine.MaxEngines);
            Assert.Equal (true, config.GetInstance (0).JavaScriptEngine.ReuseEngines);
            Assert.Equal (false, config.GetInstance (0).DisableServerSideRendering);
            Assert.Equal (false, config.GetInstance (0).Babel.LoadBabel);
        }

        [Fact]
        public void DefaultDnnReactConfigTest ()
        {
            var config = new ExtensionYamlConfig<DnnReactConfig> (
                Path.GetFullPath ("SomeNonExistentConfigFile.yml"), cfg => {
                    return cfg;
                }
            );

            Assert.NotNull (config.GetInstance (0).JavaScriptEngine);
            Assert.Equal ("JurassicJsEngine", config.GetInstance (0).JavaScriptEngine.DefaultEngineName);
            Assert.Equal (10, config.GetInstance (0).JavaScriptEngine.StartEngines);
            Assert.Equal (25, config.GetInstance (0).JavaScriptEngine.MaxEngines);
            Assert.Equal (true, config.GetInstance (0).JavaScriptEngine.ReuseEngines);
            Assert.Equal (false, config.GetInstance (0).DisableServerSideRendering);
            Assert.Equal (false, config.GetInstance (0).Babel.LoadBabel);
        }
    }
}
