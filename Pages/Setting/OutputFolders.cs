using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SmartLocationApp.Source;

namespace SmartLocationApp.Pages.Setting
{
	// Token: 0x02000034 RID: 52
	public partial class outputFolders : Form
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0001DB69 File Offset: 0x0001BD69
		public outputFolders()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0001DB78 File Offset: 0x0001BD78
		internal void init(BarcodePrintSettings _BarcodePrintSettingsForm, int _index)
		{
			this.BarcodePrintSettingsForm = _BarcodePrintSettingsForm;
			this.index = _index;
			this.outputs = new TextBox[]
			{
				this.outputFolder0,
				this.outputFolder1,
				this.outputFolder2,
				this.outputFolder3,
				this.outputFolder4,
				this.outputFolder5,
				this.outputFolder6,
				this.outputFolder7,
				this.outputFolder8,
				this.outputFolder9
			};
			this.prefix = new TextBox[]
			{
				this.textBoxPrefix0,
				this.textBoxPrefix1,
				this.textBoxPrefix2,
				this.textBoxPrefix3,
				this.textBoxPrefix4,
				this.textBoxPrefix5,
				this.textBoxPrefix6,
				this.textBoxPrefix7,
				this.textBoxPrefix8,
				this.textBoxPrefix9
			};
			this.cycles = new CheckBox[]
			{
				this.cycle0,
				this.cycle1,
				this.cycle2,
				this.cycle3,
				this.cycle4,
				this.cycle5,
				this.cycle6,
				this.cycle7,
				this.cycle8,
				this.cycle9
			};
			this.amounts = new NumericUpDown[]
			{
				this.amount0,
				this.amount1,
				this.amount2,
				this.amount3,
				this.amount4,
				this.amount5,
				this.amount6,
				this.amount7,
				this.amount8,
				this.amount9
			};
			this.actives = new CheckBox[]
			{
				this.active0,
				this.active1,
				this.active2,
				this.active3,
				this.active4,
				this.active5,
				this.active6,
				this.active7,
				this.active8,
				this.active9
			};
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001DD9C File Offset: 0x0001BF9C
		private void outputFoldersSave_Click(object sender, EventArgs e)
		{
			List<OutputFoldersStructure> opFolders = new List<OutputFoldersStructure>();
			for (int i = 0; i < 10; i++)
			{
				opFolders.Add(new OutputFoldersStructure
				{
					outputFolder = this.outputs[i].Text,
					prefixValue = this.prefix[i].Text,
					cycle = this.cycles[i].Checked,
					amount = (int)this.amounts[i].Value,
					active = this.actives[i].Checked,
					queue = (i == 0),
					loop = 0
				});
			}
			this.BarcodePrintSettingsForm.opFolders[this.index] = opFolders;
			base.Hide();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001DE60 File Offset: 0x0001C060
		public void fillOutputFolders(List<OutputFoldersStructure> configs, bool prefix_status)
		{
			int i = 0;
			foreach (OutputFoldersStructure item in configs)
			{
				this.outputs[i].Text = item.outputFolder;
				this.prefix[i].Text = item.prefixValue;
				this.prefix[i].Enabled = prefix_status;
				this.cycles[i].Checked = item.cycle;
				this.amounts[i].Value = item.amount;
				this.actives[i].Checked = item.active;
				i++;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001DF24 File Offset: 0x0001C124
		private void openFolderDialog(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string name = ((Button)sender).Name;
				int index = int.Parse(name.Substring(name.Length - 1));
				this.outputs[index].Text = dialog.SelectedPath;
			}
		}

		// Token: 0x040002CD RID: 717
		private TextBox[] outputs;

		// Token: 0x040002CE RID: 718
		private TextBox[] prefix;

		// Token: 0x040002CF RID: 719
		private CheckBox[] cycles;

		// Token: 0x040002D0 RID: 720
		private NumericUpDown[] amounts;

		// Token: 0x040002D1 RID: 721
		private CheckBox[] actives;

		// Token: 0x040002D2 RID: 722
		private int index;

		// Token: 0x040002D3 RID: 723
		private BarcodePrintSettings BarcodePrintSettingsForm;
	}
}
