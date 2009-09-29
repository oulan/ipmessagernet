using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSLib.IPMessager.Services;

namespace IPMessagerNet.UI.Controls.Config
{
	public partial class SecurityConfigPanel : IPMessagerNet.API.ConfigPanelBase
	{
		public SecurityConfigPanel()
		{
			InitializeComponent();

			this.VisibleChanged += SecurityConfigPanel_VisibleChanged;
			this.Load += SecurityConfigPanel_Load;
		}

		void SecurityConfigPanel_Load(object sender, EventArgs e)
		{
			BindServiceState();
		}

		void SecurityConfigPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				BindServiceState();
			}
		}

		void BindServiceState()
		{
			isInBinding = true;

			CheckServiceInstalled(InnerService.BlackListBlocker, chkBlackList);
			CheckServiceInstalled(InnerService.MessageFilterService, chkFilter, txtFilterWords);
			CheckServiceInstalled(InnerService.RSAEncryptionComponent, chkEncrypt);
			BindFilterEditor();

			isFirstLoad = false;
			isInBinding = false;
		}

		/// <summary>
		/// 绑定关键字过滤
		/// </summary>
		private void BindFilterEditor()
		{
			ServiceInfo iflp = Env.IPMClient.Config.Services.Find(s => s.TypeName == ServiceManager.InnerServiceTypeList[InnerService.MessageFilterService]);
			if (iflp != null)
			{
				List<string> words = iflp.ServiceProvider.ProviderConfig as List<string>;
				if (words != null) txtFilterWords.Text = string.Join(",", words.ToArray());
				if (!isFirstLoad)
				{
					txtFilterWords.LostFocus += (s, e) =>
					{
						List<string> words1 = iflp.ServiceProvider.ProviderConfig as List<string>;
						if (words1 == null) iflp.ServiceProvider.ProviderConfig = words1 = new List<string>();
						words1.Clear();
						words1.AddRange(txtFilterWords.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
						iflp.ServiceProvider.ReloadConfig();
					};
				}
			}
		}
		bool isFirstLoad = true;	//是否是第一次加载
		bool isInBinding = false;	//是否正在重新绑定中

		/// <summary>
		/// 绑定服务状态，并绑定事件
		/// </summary>
		/// <param name="service"></param>
		/// <param name="args"></param>
		void CheckServiceInstalled(FSLib.IPMessager.Services.InnerService service, params Control[] args)
		{
			ServiceInfo iflp = Env.IPMClient.Config.Services.Find(s => s.TypeName == ServiceManager.InnerServiceTypeList[service]);
			if (iflp == null) Array.ForEach(args, s => s.Enabled = false);
			else
			{
				Array.ForEach(args, s =>
				{
					if (s is CheckBox)
					{
						CheckBox ck = s as CheckBox;
						ck.Checked = iflp.Enabled;

						if (!isFirstLoad) return;

						ck.CheckedChanged += (a, b) =>
						{
							if (isInBinding) return;	//如果是在绑定中，那么就让这次的事件触发无效。
							CheckBox ckb = a as CheckBox;
							iflp.Enabled = ckb.Checked;
							if (ckb.Checked)
							{
								if (!Env.StartupServiceProvider(iflp)) Infomation("无法启动插件，可能需要重新启动设置才能生效。");
							}
							else
							{
								if (!Env.ShutdownServiceProvider(iflp)) Infomation("无法停止插件，可能需要重新启动设置才能生效。");
							}

						};
					}
				}
				);
			}
		}
	}
}
