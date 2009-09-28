using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPMessagerNet.Core;

namespace IPMessagerNet.UI.Base
{
	public class DialogBase : FormBase
	{
		public DialogBase()
		{
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

			if (Env.ClientConfig != null)
			{
				this.Icon = this.GetThemeIcon();

				if (Environment.OSVersion.Version.Major < 5)
				{
					FSLib.Windows.Components.Effect.FadeEffectComponent fec = new FSLib.Windows.Components.Effect.FadeEffectComponent()
					{
						ParentForm = this
					};
				}
			}
		}
	}
}
