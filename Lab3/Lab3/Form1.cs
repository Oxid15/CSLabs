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
			gmailClient.auth();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{

		}
	}
}
