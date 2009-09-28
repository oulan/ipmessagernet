using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
			
		}

		void SecurityConfigPanel_VisibleChanged(object sender, EventArgs e)
		{
			
		}
	}
}
