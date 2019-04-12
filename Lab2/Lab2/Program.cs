//вопросы:
//1. как извлекать имена html ссылок 

using System.Text.RegularExpressions;
using System.Net;
using System;
using System.IO;
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
		public event searchResult onTarget;

		public Analyzer(string _root)
		{
			root = _root;
			client = new WebClient();
			visitedLinks = new SortedSet<string>();
			currentPath = new Stack<string>();
		}

		bool isLinkInternal(string link)//FIXME
		{
			if (link[0] == 'h')
				return false;
			else if (link[0] == '<')
				return true;
			else
				return true;
		}

		//causes the nonexpected missing of first characters in some links
		static string htmlLinkToURI(string htmlLink)
		{
			//passes the <a href="/
			int i = 0;
			while (i < 9)
				i++;
			i++;
			string URI = "";
			
			while (htmlLink[i] != '>')
			{
				if (htmlLink[i] == '\"')
					return URI;
				URI += htmlLink[i];
				i++;
			}
			return URI;
		}

		public string[] findLinksOnPage(string pageURI)
		{
			try
			{
				string page = client.DownloadString(new Uri(pageURI));
				MatchCollection matches = Regex.Matches(page, @"<a href=[""\/\w-\.:]+>");

				string[] links = new string[matches.Count];
				int size = matches.Count;
				for (int i = 0; i < size; i++)
				{
					links[i] = matches[i].ToString();
				}
				return links;
			}
			catch(WebException ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public void recSearch(string thisURI, int maxPages = 1000, int depth = 0)
		{
			if (depth == 5 || visitedLinks.Count == maxPages)
				return;
			else if (!visitedLinks.Contains(thisURI))
			{
				visitedLinks.Add(thisURI);
				currentPath.Push(thisURI);

				string[] links = findLinksOnPage(thisURI);
				if (links != null)
				{
					foreach (string link in links)
					{
						if (isLinkInternal(link))
							recSearch(root + htmlLinkToURI(link), maxPages, depth + 1);
						else
							onTarget(link, depth);
					}
				}
			}
		}

		public void fileOutput(string fileName)
		{
			File.WriteAllLines(fileName, visitedLinks);
		}
	}

	class AnalyzerHandler
	{
		string fileName;

		public AnalyzerHandler(string _csvFileName)
		{
			fileName = _csvFileName;
		}

		public void writeLinkCsv(string str, int depth)
		{
			string[] strA = new string[1];
			strA[0] = str;

			File.WriteAllLines(fileName, strA);
		}

		public void writeLinkConsole(string str, int depth)
		{
			Console.WriteLine(str + " - "+ depth.ToString());
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Analyzer a = new Analyzer( "https://www.susu.ru/");
			AnalyzerHandler h = new AnalyzerHandler("links.csv");
			a.onTarget += h.writeLinkConsole;
			a.recSearch(a.root);
			a.fileOutput("links.csv");
		}
	}
}
