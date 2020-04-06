using System;
using System.IO;

namespace ServidorWebCustomizado
{
	public class ObtemArquivoHtm
	{
		private readonly string _referencia;

		public ObtemArquivoHtm(string referencia)
		{
			_referencia = referencia;
		}

		private string BuscarArquivoPorReferencia(string referencia)
		{
			return $"{System.Environment.CurrentDirectory}/wwwroot/{referencia}.htm";
		}

		public byte[] ObterArrayBytes()
		{
			var path = BuscarArquivoPorReferencia(_referencia);

			if (File.Exists(path))
				return File.ReadAllBytes(path);
			else
				return null;
		}

	}
}
