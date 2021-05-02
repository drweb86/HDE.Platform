using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using HDE.Platform.AspectOrientedFramework.Services;
using HDE.Platform.Collections;
using HDE.Platform.Logging;

namespace HDE.Platform.AspectOrientedFramework.WinForms
{
    public abstract class ShellBaseController<TModel, TMainWindow> : BaseController<TModel>
        where TModel : class, new()
        where TMainWindow: Form, IMainFormView, new()
    {
        protected List<ITool> Tools { get; set; }

        protected ShellBaseController()
        {
            RegisterShell();
        }

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

            var binFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var configurationFile = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".xml";
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

                var loadedToolAssembly = Assembly.LoadFile(Path.Combine(binFolder, $"{activatorInfo[1]}.dll"));
                var tool = (ITool)loadedToolAssembly.CreateInstance(activatorInfo[0]);
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

        protected virtual void RegisterShell()
        {
            UiFactory.Register<IMainFormView, TMainWindow>();
        }

        protected virtual IMainFormView CreateShell()
        {
            var type = UiFactory.Get(typeof(IMainFormView));
            var result = Activator.CreateInstance(type);
            var typedResult = (IMainFormView)result;

            typedResult.SetController(this);

            return typedResult;
        }

        public virtual void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportIssue(Log, e.ExceptionObject);
            Application.ThreadException += (s, e) => ReportIssue(Log, e.Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var result = CreateShell();
            Configure(result);
            result.SetController(this);
            Application.Run((Form)result);
            ((Form)result).FormClosing += OnMainWindowClosed;
        }

        private void OnMainWindowClosed(object sender, FormClosingEventArgs e)
        {
            TearDownTools();

            ((Form)sender).FormClosing -= OnMainWindowClosed;
        }

        private static void ReportIssue(ILog log, object unhandledException)
        {
            var typedExc = unhandledException as Exception;
            if (typedExc != null)
            {
                log.Error(typedExc);
                MessageBox.Show(typedExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
