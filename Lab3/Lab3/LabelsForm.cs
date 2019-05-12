using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
	public partial class LabelsForm : Form
	{
		public LabelsForm()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			
		}

		public void showLabelsList(IList<Google.Apis.Gmail.v1.Data.Label> list)
		{
			foreach(var item in list)
				Labels.Items.Add(item.Name);
		}
	}
}
