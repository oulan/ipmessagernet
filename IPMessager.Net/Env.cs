using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSLib.IPMessager.Services;

namespace IPMessagerNet
{
	/// <summary>
	/// 工作的环境类
	/// </summary>
	static class Env
	{
		/// <summary>
		/// 客户端配置
		/// </summary>
		public static Config.ClientConfig ClientConfig { get; set; }

		/// <summary>
		/// IPM客户端
		/// </summary>
		public static FSLib.IPMessager.IPMClient IPMClient { get; set; }

		/// <summary>
		/// 当前工作的主窗口
		/// </summary>
		public static UI.Forms.FrameContainer MainForm { get; internal set; }

		/// <summary>
		/// 聊天设置
		/// </summary>
		public static Config.ChatAreaConfig ChatConfig
		{
			get
			{
				if (ClientConfig == null) return null;
				else return ClientConfig.ChatConfig;
			}
		}

		/// <summary>
		/// 主机信息配置
		/// </summary>
		public static Config.HostInfoConfig HostConfig
		{
			get
			{
				if (ClientConfig == null) return null;
				else return ClientConfig.HostInfo;
			}
		}

		/// <summary>
		/// 主机列表视图配置
		/// </summary>
		public static UI.Controls.HostTreeView.ListConfig HostListConfig
		{
			get
			{
				return ClientConfig == null ? null : ClientConfig.HostListViewConfig;
			}
		}

		/// <summary>
		/// 框架窗口视图配置
		/// </summary>
		public static UI.Forms.FrameContainerConfig FrameConfig
		{
			get
			{
				return ClientConfig == null ? null : ClientConfig.FrameContainerConfig;
			}
		}

		/// <summary>
		/// 初始化通信客户端
		/// </summary>
		public static void InitializeIPMClient()
		{
			if (IPMClient == null && ClientConfig != null)
			{
				IPMClient = new FSLib.IPMessager.IPMClient(ClientConfig.IPMClientConfig);

				//初始化插件
				IPMClient.Config.Services.ForEach(s =>
				{
					HandleServiceConfigLoad(s.ServiceProvider);
				});
			}
		}

		/// <summary>
		/// 绑定插件的配置加载保存事件
		/// </summary>
		/// <param name="provider"></param>
		public static void HandleServiceConfigLoad(FSLib.IPMessager.Services.IServiceProvider provider)
		{
			if (provider == null) return;

			provider.RequireLoadConfig += (s, e) =>
			{
				if (!e.IsHandled) { e.ConfigObject = Core.ProfileManager.LoadConfig(e.Type, s.GetType().Name); e.IsHandled = true; }
			};
			provider.ReuqireSaveConfig += (s, e) =>
			{
				if (!e.IsHandled) { Core.ProfileManager.SaveConfig(s.GetType().Name, e.ConfigObject); e.IsHandled = true; }
			};
		}

		/// <summary>
		/// 尝试启动指定插件
		/// </summary>
		/// <param name="si"></param>
		/// <returns></returns>
		public static bool StartupServiceProvider(ServiceInfo si)
		{
			if (si.EnsureLoadAssembly() && si.CreateProviderInstance() && si.InitialzingServiceProvider(Env.IPMClient))
			{
				if (!si.LoadService()) return false;
				else { Env.HandleServiceConfigLoad(si.ServiceProvider); return true; }
			}
			return si.State != ServiceState.Running;
		}

		/// <summary>
		/// 卸载指定插件
		/// </summary>
		/// <param name="si"></param>
		/// <returns></returns>
		public static bool ShutdownServiceProvider(ServiceInfo si)
		{
			return si.State == ServiceState.Unload || si.ShutDown();
		}
	}
}
