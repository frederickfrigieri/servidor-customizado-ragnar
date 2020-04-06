using System;
namespace ServidorWebCustomizado
{
	public class Utilidades
	{
		public static void Escreve(string s, bool comQuebrea = true)
		{
			Console.Write(s);

			if (comQuebrea) Console.WriteLine();
		}

		public static void Quebra()
		{
			Console.WriteLine();
		}


		public static string BemVindoHtml()
		{
			string html = "<HTML>";
			html += "<TITLE>Ragnar Server</TITLE>";
			html += "<BODY>";
			html += @"
			<h3>Bem vindo ao Servidor Ragnar</h3>
			@autor: Frederick Frigieri";
			html += "</BODY></HTML>";

			return html;
		}
	}
}
