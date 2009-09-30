using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPMessagerNet.UI.Controls.Config
{
	public partial class NotifyConfigPanel : IPMessagerNet.API.ConfigPanelBase
	{
		public NotifyConfigPanel()
		{
			InitializeComponent();
		}

		private void NotifyConfigPanel_Load(object sender, EventArgs e)
		{
			switch (Env.HostConfig.HostOnlineTip)
			{
				case IPMessagerNet.Config.HostBallonTip.None:
					rbOTNone.Checked = true;
					break;
				case IPMessagerNet.Config.HostBallonTip.All:
					rbOTAll.Checked = true;
					break;
				case IPMessagerNet.Config.HostBallonTip.Special:
					rbOTSpec.Checked = true;
					break;
			}
			btnOTSpec.Enabled = rbOTSpec.Checked;

			switch (Env.HostConfig.HostOfflineTip)
			{
				case IPMessagerNet.Config.HostBallonTip.None:
					rbFTNone.Checked = true;
					break;
				case IPMessagerNet.Config.HostBallonTip.All:
					rbFTAll.Checked = true;
					break;
				case IPMessagerNet.Config.HostBallonTip.Special:
					rbFTSpec.Checked = true;
					break;
			}
			btnFTSpec.Enabled = rbFTSpec.Checked;

			rbOTNone.CheckedChanged += rbOTNone_CheckedChanged;
			rbOTAll.CheckedChanged += rbOTNone_CheckedChanged;
			rbOTSpec.CheckedChanged += rbOTNone_CheckedChanged;
			rbFTNone.CheckedChanged += rbFTNone_CheckedChanged;
			rbFTAll.CheckedChanged += rbFTNone_CheckedChanged;
			rbFTSpec.CheckedChanged += rbFTNone_CheckedChanged;
		}

		//Handle checkbox checked changed events.
		void rbOTNone_CheckedChanged(object sender, EventArgs e)
		{
			if (sender == rbOTAll) SwitchOT(IPMessagerNet.Config.HostBallonTip.All);
			else if (sender == rbOTNone) SwitchOT(IPMessagerNet.Config.HostBallonTip.None);
			else SwitchOT(IPMessagerNet.Config.HostBallonTip.Special);
		}

		//Handle checkbox checked changed events.
		void rbFTNone_CheckedChanged(object sender, EventArgs e)
		{
			if (sender == rbFTAll) SwitchFT(IPMessagerNet.Config.HostBallonTip.All);
			else if (sender == rbFTNone) SwitchFT(IPMessagerNet.Config.HostBallonTip.None);
			else SwitchFT(IPMessagerNet.Config.HostBallonTip.Special);
		}

		//选择上线提示类型
		void SwitchOT(IPMessagerNet.Config.HostBallonTip type)
		{
			btnOTSpec.Enabled = type == IPMessagerNet.Config.HostBallonTip.Special;
		}

		//选择下线提示类型
		void SwitchFT(IPMessagerNet.Config.HostBallonTip type)
		{
			btnFTSpec.Enabled = type == IPMessagerNet.Config.HostBallonTip.Special;
		}
	}
}
