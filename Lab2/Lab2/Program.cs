using System.Text.RegularExpressions;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;

namespace Lab2
{
	class Link
	{
		public bool isHttps { get; set; }   //https
		public bool hasWWW { get; set; }    //www
		public string rootURI { get; set; } // "sitename"
		public string domain { get; set; }  // "com"
		public string bodyURI { get; set; } // "/page_1/page_2/.../page_n"
		int depth;

		public Link()
		{
			isHttps = false;
			hasWWW = true;
			rootURI = "";
			domain = "";
			bodyURI = "";
			depth = 0;
		}

		public Link(string link, int _depth = 0)
		{
			if (link.Contains("https"))
				isHttps = true;
			else
				isHttps = false;

			if (link.Contains("www"))
				hasWWW = true;
			else
				hasWWW = false;

			rootURI = obtainRoot(link);

			domain = obtainDomain(link);

			bodyURI = obtainBody(link);

			depth = _depth;
		}

		public Link(string root, string _bodyURI, int _depth = 0)
		{
			if (root.Contains("https"))
				isHttps = true;
			else
				isHttps = false;

			if (root.Contains("www"))
				hasWWW = true;
			else
				hasWWW = false;

			rootURI = obtainRoot(root);

			domain = obtainDomain(root, true);

			bodyURI = _bodyURI;
		}

		private string obtainRoot(string link)
		{
			int i = link.IndexOf("//");
			if (i >= 0)
			{
				i += 6; //skip "//www."

				List<char> root = new List<char>();
				while (link[i] != '.')
				{
					root.Add(link[i]);
					i++;
				}

				string result = new string(root.ToArray());
				return result;
			}
			else
				return "";
		}

		private string obtainDomain(string link, bool isRoot = false)
		{
			int dotIndex = link.Length - 1;
			if (dotIndex >= 0)
			{
				while (link[dotIndex] != '.')
				{
					dotIndex--;
				}

				List<char> domain = new List<char>();
				if (!isRoot)
				{
					int i = dotIndex + 1;
					while (link[i] != '/')
					{
						domain.Add(link[i]);
						i++;
					}
				}
				else
				{
					int n = link.Length;
					if (link[n - 1] == '/')
						n -= 1;

					for (int i = dotIndex + 1; i < n; i++)
						domain.Add(link[i]);
				}

				string result = new string(domain.ToArray());
				return result;
			}
			else
				return "";
		}

		private string obtainBody(string link)
		{
			int len = link.Length;
			if(domain != "")
			{
				int first = link.IndexOf(domain) + domain.Length;
				List<char> body = new List<char>();
				for (int i = first; i < len; i++)
				{
					body.Add(link[i]);
				}
				if (body.Count != 0)
				{
					string result = new string(body.ToArray());
					return result;
				}
				else
					return "";
			}
			return link;
		}

		public override string ToString()
		{
			string http_s;
			if (isHttps)
				http_s = "https";
			else
				http_s = "http";

			string wwwDot;
			if (hasWWW)
				wwwDot = "www.";
			else
				wwwDot = "";

			return http_s + "://" + wwwDot + rootURI + "." + domain + bodyURI;
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
			//a.fileOutput("visitedLinks.csv");

			Link link = new Link("https://www.susu.ru/en/arts-and-culture/russian-museum-branch");
			Link link1 = new Link("https://www.susu.ru", "/en/athletics");
			Console.WriteLine(link.ToString());
			Console.WriteLine(link1.ToString());
			Console.ReadKey();
		}
	}
}