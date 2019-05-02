using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
	class Client
	{
		UserCredential credential;
		string credPath;
		string applicationName;

		public Client(string _credPath)
		{
			credPath = _credPath;
		}

		public void auth()
		{
			string[] Scopes = { GmailService.Scope.GmailReadonly };

			using (var stream =  new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
								GoogleClientSecrets.Load(stream).Secrets,
								Scopes,
								"user",
								CancellationToken.None,
								new FileDataStore(credPath, true)).Result;
			}
		}

		public void query1()
		{
			
		}
	}


	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.Run(new Form1());
		}
	}
}