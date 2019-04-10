using System.Text.RegularExpressions;
using System.Net;
using System;
//using System.IO;
using System.Collections.Generic;

namespace Lab2
{
	class Analyzer
	{
		WebClient client;
		static SortedSet<string> visitedLinks;
		public string root { private set; get; }

		public Analyzer(string _root)
		{
			root = _root;
			client = new WebClient();
			visitedLinks = new SortedSet<string>();
		}

		static string htmlLinkToURI(string htmlLink)
		{
			//FIXME: does not work well with links without quotes
			int i = 0;
			while (htmlLink[i] != '\"')
				i++;
			i++;
			//skip a '/' character 
			if (htmlLink[i] == '/')
				i++;

			string URI = "";
			int j = 0;
			while (htmlLink[i] != '\"')
			{
				URI += htmlLink[i];
				i++;
			}
			return URI;
		}

		public string[] findLinksOnPage(string pageURI)
		{
			string page = client.DownloadString(new Uri(pageURI));
			MatchCollection matches = Regex.Matches(page, @"<a href=[""\/\w-\.:]+>");

			string[] links = new string[matches.Count];
			int size = matches.Count;
			for(int i = 0; i < size;i++)
			{
				links[i] = matches[i].ToString();
			}
			return links;
		}

		public void recSearch(string thisURI, int depth = 0)
		{
			if (depth == 5)
				return;
			else if (!visitedLinks.Contains(thisURI))
			{
				visitedLinks.Add(thisURI);

				string[] links = findLinksOnPage(thisURI);
				///////////////////////////TEST
				foreach (string item in links)
					Console.WriteLine(item);
				Console.ReadKey();
				Console.Clear();
				///////////////////////////TEST
				foreach (string link in links)
				{
					if (link == "<a href=/contact/feedback/susu/ru>")
							Console.WriteLine("ы");
					recSearch(root + htmlLinkToURI(link), depth + 1);
				}
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Analyzer a = new Analyzer( "https://www.susu.ru/");
			a.recSearch(a.root); 
		}
	}
}
