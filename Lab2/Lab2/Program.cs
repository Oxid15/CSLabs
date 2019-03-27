using System.Text.RegularExpressions;
using System.Net;
using System;
//using System.IO;
//using System.Collections.Generic;

namespace Lab2
{
	class Analyzer
	{
		WebClient client;

		public Analyzer()
		{
			client = new WebClient();
		}

		public void findLinks(string pageURI)
		{
			string page = client.DownloadString(new Uri(pageURI));
			MatchCollection items = Regex.Matches(page, @"<a href=[""\/\w-\.:]+>");
			
		}

		//public void findLinks(string pageURI, string fileName)
		//{
		//	string page = client.DownloadString(new Uri(pageURI));
		//	MatchCollection items = Regex.Matches(page, @"<a href=[""\/\w-\.:]+>");

		//	TextWriter writer = new StreamWriter(fileName);
		//	foreach (var item in items)
		//	{
		//		writer.WriteLine(item);
		//	}
		//	writer.Close();
		//}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Analyzer a = new Analyzer();
			a.findLinks("http://www.susu.ru/");
		}
	}
}
