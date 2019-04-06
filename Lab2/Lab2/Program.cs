using System.Text.RegularExpressions;
using System.Net;
using System;
//using System.IO;
using System.Collections.Generic;

namespace Lab2
{
	//class ArrayLinks
	//{
	//	Match[] links;
	//	int cursor;

	//	public ArrayLinks(int arrSize)
	//	{
	//		links = new Match[arrSize];
	//		cursor = 0;
	//	}

	//	public void addToEnd(string link)
	//	{
	//		links.SetValue(link, cursor);
	//		cursor++;
	//	}

	//}


	class Analyzer
	{
		WebClient client;
		List<Match> visitedLinks;

		public Analyzer()
		{
			client = new WebClient();
		}

		public MatchCollection findLinksOnPage(string pageURI)
		{
			string page = client.DownloadString(new Uri(pageURI));
			return Regex.Matches(page, @"<a href=[""\/\w-\.:]+>");
		}

		public void recSearch(string thisURI, int depth)
		{
			if (depth == 5)
				return;
			else //if
			{	
				//visitedLinks.Add(thisURI);
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Analyzer a = new Analyzer();

			MatchCollection c;
			c = a.findLinksOnPage("http://www.susu.ru/");
			Match[] arr = new System.Text.RegularExpressions.Match[1024];
			c.CopyTo(arr, 0);
		}
	}
}
