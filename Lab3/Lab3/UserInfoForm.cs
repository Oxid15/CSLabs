using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Gmail.v1.Data;

namespace Lab3
{
	public partial class UserInfoForm : Form
	{
		public UserInfoForm()
		{
			InitializeComponent();
		}

		public void showInfo(Profile profile)
		{
			textBox1.Text += "email address: ";
			textBox1.Text += profile.EmailAddress + "\r\n";
			textBox1.Text += "total messages: ";
			textBox1.Text += profile.MessagesTotal;
			textBox1.Text += "\r\n";
			textBox1.Text += "total threads: ";
			textBox1.Text += profile.ThreadsTotal;
			textBox1.Text += "\r\n";
		}
	}
}
