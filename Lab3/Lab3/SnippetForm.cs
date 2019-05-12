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
	public partial class SnippetForm : Form
	{
		public SnippetForm()
		{
			InitializeComponent();
		}

		public void showMsgInfo(IList<Google.Apis.Gmail.v1.Data.Message> msg)
		{
			foreach (var mes in msg)
			{
				textBox2.Text += mes.Snippet + "\r\n\r\n";
			}
		}
	}
}
