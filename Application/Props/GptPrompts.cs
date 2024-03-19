using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Props
{
	public static class GptPrompts
	{
		public static string CreateQuizPrompt(string technologyName, int advanceNumber)
		{
			return $"Stworz quiz z {technologyName} na poziomie {advanceNumber} na 10 (gdzie 1 to proste podstawy a 10 to bardzo zaawansowany poziom)" +
				" ma zawierac 6 pytan a , b , c ,d  wraz z odpowiedziami . Odpowiedz w formacie json według schematu Pytanie - mozliwe odpowiedzi poprawna odpowiedz" +
				" \r\n\r\nCzyli ma to byc tablica 6 obiektow tego typu w formacie json :" +
				"\r\n\r\nclass Question\r\n{\r\n    public string QuestionContent { get; set; }\r\n    public List<string> Answers { get; set; }\r\n" +
				"    public char CharOfProperAnswer { get; set; }\r\n}\r\n\r\nCharOfProperAnswer ma byc 'a', 'b', 'c', 'd'\r\n" +
				"Twoja odpowiedzi powinna zawierac tylko odpowiedz w formacie json . Zadnych innych dodatkowych opisow \r\n\r\n" +
				"Oto przykładowa odpowiedz z dwoma obiektami typu Pytanie : " +
				"\r\n\r\n[\r\n  {\r\n    \"QuestionContent\": \"Jakie jest zastosowanie słowa kluczowego 'var' w C#?\",\r\n    \"Answers\": [\"a. Definiowanie zmiennych o stałej wartości\", \"b. Określanie typu zmiennej na podstawie przypisanej wartości\", \"c. Tworzenie zmiennych globalnych\", \"d. Oznaczanie zmiennych jako prywatne\"],\r\n    \"CharOfProperAnswer\": \"b\"\r\n  },\r\n  {\r\n    \"QuestionContent\": \"Co to jest właściwość (property) w C#?\",\r\n    \"Answers\": [\"a. Metoda, która zwraca wartość\", \"b. Element tablicy\", \"c. Mechanizm dostępu do pól klasy z kontrolą dostępu\", \"d. Zmienna o stałej wartości\"],\r\n    \"CharOfProperAnswer\": \"c\"\r\n  }\r\n]";
		}

		public static string CreateAlgorithmPrompt(int advanceNumber, string? specialTopics)
		{
			var specialTopicPromptElement = 
				specialTopics != null ? $"Zadanie powinno zachaczac o zagadnienia takie jak : {specialTopics}" : "";

			return $"Stworz zadanie algorytmiczne o poziomie trundości 6/10." +
				$"{specialTopicPromptElement}. Dostarcz rowniez odpowiedz do zadania w jezykach" +
				$" C# , Java , JavaScrpit , PHP , C++ , Python." +
				$" Udziel odpowiedzi w formacie JSON według schematu " +
				$" {{\r\n   \"QuestionContent\" : \"Tutaj wymyslone przez ciebie zadania algorytmiczne\" , \r\n   " +
				$" Answers : " +
				$"[\r\n\t{{\"LanguageName\" : \"C#\" , \"Content\" : \"Tresc rozwiazania dla c# tutaj\"}} ," +
				$" \r\n\t{{\"LanguageName : \"Java\" , \"Content\" : \"Tresc rozwiazania dla Java tutaj}}," +
				$"\r\n\t{{\"LanguageName : \"Python\" , \"Content\" : \"Tresc rozwiazania dla Python tutaj}}," +
				$"\r\n\t{{\"LanguageName : \"JavaScript\" , \"Content\" : \"Tresc rozwiazania dla JavaScript tutaj}}," +
				$"\r\n\t{{\"LanguageName : \"Php\" , \"Content\" : \"Tresc rozwiazania dla Php tutaj}}," +
				$"\r\n\t{{\"LanguageName : \"Cpp\" , \"Content\" : \"Tresc rozwiazania dla Cpp tutaj}},\r\n\r\n   ]\r\n}} " +
				$"- Rozwiazania w posczegolnych jezykach powinny byc w html z uwzglednieniem nowych linii np :" +
				$" \r\n\"Content:\" :  \"using system <br /> public class Osoba() <br />\" , Wciecia powinny byc zrobione za pomoca <blockquote />\r\n";
		}
	}
}
	
