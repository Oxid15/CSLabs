using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Runtime.Serialization.Json;

namespace Lab3
{
	class Client
	{
		WebClient client;
		GmailService service;
		UserCredential credential;
		string credPath;
		string applicationName;

		public Client(string _credPath)
		{
			applicationName = "GmailClient";
			credPath = _credPath;
			client = new WebClient();
			service = new GmailService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = applicationName,
			});
		}

		public int auth()
		{
			try
			{
				string[] Scopes = 
				{
					GmailService.Scope.GmailReadonly,
					GmailService.Scope.MailGoogleCom,
					GmailService.Scope.GmailModify,
					GmailService.Scope.GmailCompose,
					GmailService.Scope.GmailMetadata
				};

				using (var stream =
					new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
				{
					string credPath = "token.json";
					credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
						GoogleClientSecrets.Load(stream).Secrets,
						Scopes,
						"user",
						CancellationToken.None,
						new FileDataStore(credPath, true)).Result;
				}

				service = new GmailService(new BaseClientService.Initializer()
				{
					HttpClientInitializer = credential,
					ApplicationName = applicationName,
				});

				return 1;
			}
			catch(Google.GoogleApiException e)
			{
				MessageBox.Show(e.Message);
				return 0;
			}
			
		}

		public IList<Google.Apis.Gmail.v1.Data.Label> requestMessageLabels()
		{
			try
			{
				var request = service.Users.Labels.List("me");
				IList<Google.Apis.Gmail.v1.Data.Label> labels = request.Execute().Labels;
				return labels;
			}
			catch(Google.GoogleApiException e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
		}

		public Profile requestUserInfo()
		{
			try
			{
				var request = service.Users.GetProfile("me");
				return request.Execute();
			}
			catch (Google.GoogleApiException e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
		}

		public IList<Google.Apis.Gmail.v1.Data.Message> requestMsg()
		{
			try
			{
				var request = service.Users.Messages.List("me");
				var messages = request.Execute().Messages;
				var list = new List<Google.Apis.Gmail.v1.Data.Message>();

				foreach (var message in messages)
				{
					var msgReq = service.Users.Messages.Get("me", message.Id);
					list.Add(msgReq.Execute());
				}
				return list;
			}
			catch (Google.GoogleApiException e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
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