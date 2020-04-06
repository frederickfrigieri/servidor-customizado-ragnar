using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ServidorWebCustomizado
{
	class Program
	{
		static void Main(string[] args)
		{

			string[] url = { "http://localhost:1234/" };

			while (true)
				ServidorService.Escutando(url);
		}
	}
}
