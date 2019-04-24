using System.Text.RegularExpressions;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;

namespace Lab2
{
	class Link
	{
		public bool isHttps { get; set; }
		public string rootURI { get; set; } // "www.sitename" or "sitename"
		public string bodyURI { get; set; } // "/page_1/page_2/.../page_n"
		public string domain { get; set; }  // ".ru"

		public Link()
		{
			isHttps = false;
			rootURI = "";
			domain = "";
		}

		//public Link(string link)
		//{
		//	if (link.Contains("https"))
		//		isHttps = true;
		//	else
		//		isHttps = false;

		//	domain = obtainDomain(link);


		//}

		static public string obtainDomain(string link)
		{
			int dotIndex = link.Length - 1;
			while (link[dotIndex] != '.')
			{
				dotIndex--;
			}
			int i = dotIndex + 1;
			List<char> domain = new List<char>();
			while (link[i] != '/')
			{
				domain.Add(link[i]);
				i++;
			}
				

			string result = new string(domain.ToArray());
			return result;
		}

		public string getLink()
		{
			if(isHttps)
			{
				return "https://";
			}
			else
			{
				return "http://";
			}
		}
	}

	class Analyzer
	{
		WebClient client;
		static SortedSet<string> visitedLinks;
		static Stack<string> currentPath;
		private string currentPage;
		public string root { set; get; }
		public delegate void searchResult(string[] result, int depth);
		public event searchResult onTarget;

		public Analyzer(string _root)
		{
			root = _root;
			client = new WebClient();
			visitedLinks = new SortedSet<string>();
			currentPath = new Stack<string>();
		}

		string[] strArrayReverse(string[] arr)
		{
			string[] reversed = new string[arr.Length];
			for (int i = 0, j = arr.Length; i < arr.Length; i++, j--)
			{
				reversed[i] = arr[j];
			}
			return reversed;
		}

		bool isLinkExternal(string link)
		{
			int len = link.Length;
			if (!link.Contains("http"))
				return false;
			else
			{
				string _root = root;

				if (_root.Contains("www."))
					_root.Remove(_root.IndexOf("www."), 4);

				if (link.Contains("www."))
					link.Remove(link.IndexOf("www."), 4);

				int _rootLen = _root.Length;
				for (int i = 0; i < _rootLen; i++)
				{
					if (_root[i] == link[i])
						continue;
					else
						return true;
				}
				return false;
			}
		}

		//deletes http(s)://.../
		static string cutHttp_s_rootURI(string link)
		{
			if (!link.Contains("http"))
				return link;

			int i = link.IndexOf("//") + 2;
			while (link[i] != '/')
				i++;
			return link.Remove(0, i + 1);
		}

		static string htmlLinkToURI(string htmlLink)
		{
			//passes the <a href="/
			int i = 0;
			while (i < 9)
				i++;
			if (htmlLink[i] == '/')	i++;

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
				currentPage = client.DownloadString(new Uri(pageURI));
				MatchCollection matches = Regex.Matches(currentPage, @"<a href=[""\/\w-\.:]+>");

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

		private string[] findExternalLinks(string[] links)
		{
			List<string> external = new List<string>();
			foreach (string link in links)
				if (isLinkExternal(link))
					external.Add(link);
			if(links.Length != 0)
				return external.ToArray();
			else return null;
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
				string[] external = findExternalLinks(links);
				if (external != null)
					foreach (string link in external)
						onTarget(strArrayReverse(currentPath.ToArray()),depth);

				if (links != null)
				{
					foreach (string link in links)
					{
						string URI = htmlLinkToURI(link);
						recSearch
							(
								root + cutHttp_s_rootURI(URI),
								maxPages, 
								depth + 1
							);

						currentPath.Pop();

						Console.WriteLine(URI);
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

		//public void writeLinkCsv(string[] str, int depth)
		//{
		//	FileStream file = new FileStream("links.csv", FileMode.Open, FileAccess.Write);
		//	StreamWriter w = new StreamWriter(file);

		//}

		public void writeLinkConsole(string[] str, int depth)
		{
			Console.WriteLine(str[str.Length - 1]);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			//Analyzer a = new Analyzer("https://www.susu.ru/");
			//AnalyzerHandler h = new AnalyzerHandler("links.csv");
			//a.onTarget += h.writeLinkConsole;
			//a.recSearch(a.root);
			//a.fileOutput("links.csv");

			//Stack<string> stack = new Stack<string>();
			//stack.Push("string0");
			//stack.Push("string1");
			//stack.Push("string2");
			//stack.Push("string3");

			//string[] arr = stack.ToArray();
			//string[] arr2 = new string[arr.Length];

			//for (int i = arr.Length - 1, j = 0; i >= 0; i--, j++)
			//{
			//	arr2[j] = arr[i];
			//}

			//AnalyzerHandler h1 = new AnalyzerHandler("links.csv");
			//h1.writeLinkConsole(arr2, 4);

			//Link link = new Link("https://www.susu.ru/");
		}
	}
}