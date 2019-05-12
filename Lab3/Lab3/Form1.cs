using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Lab3
{
	public partial class Form1 : Form
	{
		Client gmailClient;

		public Form1()
		{
			InitializeComponent();
			gmailClient = new Client("token.json");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if(gmailClient.auth() == 1)
				button4.Visible = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var list = gmailClient.requestMessageLabels();
			if(list != null)
			{
				LabelsForm f = new LabelsForm();
				f.Show();
				f.showLabelsList(list);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var profile = gmailClient.requestUserInfo();
			if (profile != null)
			{
				UserInfoForm f = new UserInfoForm();
				f.Show();
				f.showInfo(profile);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			var messages = gmailClient.requestMsg();
			if (messages != null)
			{
				SnippetForm f = new SnippetForm();
				f.Show();
				f.showMsgInfo(messages);
			}
		}
	}
}
