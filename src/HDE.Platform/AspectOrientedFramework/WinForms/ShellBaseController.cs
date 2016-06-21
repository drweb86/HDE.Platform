using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using HDE.Platform.AspectOrientedFramework.Services;
using HDE.Platform.Collections;

namespace HDE.Platform.AspectOrientedFramework.WinForms
{
    public abstract class ShellBaseController<TModel> : BaseController<TModel> 
        where TModel : class, new()
    {
        protected List<ITool> Tools { get; set; }

        protected virtual void TearDownTools()
        {
            if (Tools != null)
            {
                foreach (var tool in Tools)
                {
                    tool.Dispose();
                }
            }
        }

        protected virtual void Configure(IMainFormView mainFormView)
        {
            Tools = new List<ITool>();
            var commonServices = new Dictionary<object, object>();
            commonServices.Add(typeof(IMessagePump), new MessagePump());
            var commonServicesAssign = commonServices.ToReadonlyDictionary();

            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configurationFile = Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().Location) + ".xml";
            var configFile = Path.Combine(binFolder, configurationFile);
            var shellConfig = new XmlDocument();
            shellConfig.Load(configFile);

            var toolConfigs = shellConfig.SelectNodes(@"Configuration/Tools/Load");
            foreach (XmlNode toolConfig in toolConfigs)
            {
                var activatorInfo = toolConfig
                    .Attributes["tool"].Value
                    .Split(
                        new[] { ", " },
                        StringSplitOptions.RemoveEmptyEntries);

                var tool = (ITool)Activator.CreateInstance(activatorInfo[1], activatorInfo[0]).Unwrap();
                Tools.Add(tool);

                tool.Assign(Log,
                    toolConfig.Attributes["name"].Value,
                    mainFormView.TabControl,
                    mainFormView.MainMenu,
                    commonServicesAssign);

                var menuPaths = toolConfig.Attributes["addToMenu"].Value.Split(new[] { '/' });
                var rootItemCollection = mainFormView.MainMenu.Items;
                for (int i = 0; i < menuPaths.Length; i++)
                {
                    ToolStripMenuItem menu = null;
                    foreach (ToolStripMenuItem item in rootItemCollection)
                    {
                        if (item.Text == menuPaths[i])
                        {
                            menu = item;
                        }
                    }

                    if (menu == null)
                    {
                        menu = new ToolStripMenuItem(menuPaths[i]);
                        menu.Name = menuPaths[i];

                        rootItemCollection.Add(menu);
                    }
                    rootItemCollection = menu.DropDownItems;

                    if (i == menuPaths.Length - 1)
                    {
                        menu.Click += (s, e) => tool.Activate();
                    }
                }
            }
        }
    }
}
