using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using IPMessagerNet.UI.Controls.HostTreeView;

namespace IPMessagerNet
{
	static class Program
	{

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] cmd)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (cmd.Length > 0)
			{
				if (cmd[0] == "/wait" && cmd.Length >= 2)
				{
					try
					{
						System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(int.Parse(cmd[1]));
						if (p != null) p.WaitForExit();	//等待终止
					}
					catch (Exception) { }
				}
			}

			//捕捉异常
			FSLib.Windows.Dialogs.ThreadException.SettingUpForm();

			//加载配置
			Env.ClientConfig = Core.ProfileManager.LoadConfig<Config.ClientConfig>();
			if (Env.ClientConfig == null)
			{
				Env.ClientConfig = GetDefaultConfig();
				Env.ClientConfig.Save();
			}
			Env.ClientConfig.IPMClientConfig.VersionInfo = "飞鸽传书.Net " + Application.ProductVersion + "，BY 木鱼";

			//初始化IPM客户端
			Env.InitializeIPMClient();

			if (Env.IPMClient.IsInitialized)
			{
				Application.Run(new UI.Forms.FrameContainer());
			}
			else
			{
				//失败
				Application.Run(new UI.Dialogs.Notify.InitializeError());
			}
		}

		/// <summary>
		/// 重启客户端
		/// </summary>
		public static void Restart()
		{
			System.Diagnostics.Process.Start(Application.ExecutablePath, String.Format("/wait {0}", System.Diagnostics.Process.GetCurrentProcess().Id));
			UI.Forms.FrameContainer.ContainerForm.ShutdownIPM();
		}

		/// <summary>
		/// 生成默认的设置
		/// </summary>
		/// <returns></returns>
		static Config.ClientConfig GetDefaultConfig()
		{
			return new IPMessagerNet.Config.ClientConfig()
			{
				Themes = "Default",
				HostListViewConfig = new ListConfig()
				{
					FirstGroupType = ListConfig.GroupType.None,
					SecondGroupType = ListConfig.GroupType.None,
					FirstOrder = ListConfig.SortOrder.State,
					SecondOrder = ListConfig.SortOrder.IP
				},
				IPMClientConfig = new FSLib.IPMessager.Entity.Config()
				{
					Port = 2425,
					GroupName = Environment.MachineName,
					NickName = Environment.UserName,
					ForceOldContract = true,
					IgnoreNoAddListFlag = false,
					EnableHostNotifyBroadcast = false,
					HostName = Environment.MachineName,
					AutoReplyWhenAbsence = true,
					AutoDetectVersion = true,
					BanedHost = new List<string>(),
					KeepedHostList = new List<string>(),
					BindedIP = System.Net.IPAddress.Any,
					VersionInfo = "飞鸽传书.Net " + Application.ProductVersion + "，BY 木鱼",
					AbsenceSuffix = " [离开]",
					Services = FSLib.IPMessager.Services.ServiceManager.GetServices()
				},
				AbsenceMessage = new List<string>()
				{
					"我有事暂时不在，稍后联系你",
					"我吃饭去了！",
					"我玩游戏去了！",
					"还钱请电话，借钱请关机！"
				},
				FrameContainerConfig = new IPMessagerNet.UI.Forms.FrameContainerConfig()
				{
					HostListContainerTopMost = true,
					HostListFloat = false,
					MinimizeToTray = true,
					WindowState = -1,
					HostListContainerWindowState = -1
				},
				HostGroupConfig = new FSLib.IPMessager.Entity.SerializableDictionary<string, string>(),
				HostInfo = new IPMessagerNet.Config.HostInfoConfig()
				{
					DisplayStyle = IPMessagerNet.Config.HostNameDisplayStyle.MemoBeforeName,
					HostMemo = new FSLib.IPMessager.Entity.SerializableDictionary<string, string>(),
					HostOfflineTip = IPMessagerNet.Config.HostBallonTip.All,
					HostOnlineTip = IPMessagerNet.Config.HostBallonTip.All,
					OfflineTip = new List<string>(),
					OnlineTip = new List<string>()
				},
				ChatConfig = new IPMessagerNet.Config.ChatAreaConfig()
				{
					EnableCtrlEnterShortKey = true
				},
				FunctionConfig = new IPMessagerNet.Config.ClientFunctionConfig()
				{
					Share_CalculateFolderSize = true
				}
			};
		}
	}
}
