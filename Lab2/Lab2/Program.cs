//вопросы:
//1. как извлекать имена html ссылок 

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
		static Stack<string> currentPath;
		public string root { private set; get; }
		public delegate void searchResult(string result, int depth);
		public event searchResult target;

		public Analyzer(string _root)
		{
			root = _root;
			client = new WebClient();
			visitedLinks = new SortedSet<string>();
			currentPath = new Stack<string>();
		}

		static string htmlLinkToURI(string htmlLink)
		{
			//FIXME: does not work well with links without quotes
			int i = 0;
			while (i < 9)
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

		public void recSearch(string thisURI, int maxPages = 10000, int depth = 0)
		{
			if (depth == 5 || visitedLinks.Count == maxPages)
				return;
			else if (!visitedLinks.Contains(thisURI))
			{
				visitedLinks.Add(thisURI);
				currentPath.Push(thisURI);

				string[] links = findLinksOnPage(thisURI);
				foreach (string link in links)
				{
					recSearch(root + htmlLinkToURI(link), maxPages, depth + 1);
					if (currentPath.Peek() != root)
						currentPath.Pop();
				}
			}
		}
	}

	class AnalyzerHandler
	{
		//AnalyzerHandler(string csvFileName)
		//{

		//}

		//public void writeLinkCsv(string str, int depth)
		//{

		//}

		//public void writeLinkConsole(string str, int depth)
		//{

		//}
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
