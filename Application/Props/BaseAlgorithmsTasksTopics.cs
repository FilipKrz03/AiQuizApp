using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Props
{
	public static class BaseAlgorithmsTasksTopics
	{
		public static IEnumerable<string> Get()
		{
			yield return "Sortowanie";
			yield return "Ciagi liczbowe";
			yield return "Binary search";
			yield return "Manipulacja stringami";
			yield return "Szukanie optymalnego rozwiazania";
		}
	}
}
