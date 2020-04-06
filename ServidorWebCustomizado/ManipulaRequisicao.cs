using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServidorWebCustomizado
{
	public class ManipulaRequisicao
	{
		private readonly HttpListenerRequest _requisicao;

		private readonly string _localPath;
		private readonly string _pathAndQuery;
		private readonly string _absolutePath;
		private readonly string _query;
		public readonly string _host;
		private readonly string _absoluteUri;


		public string PrimeiroEndpoint;

		public ManipulaRequisicao(HttpListenerRequest requisicao)
		{
			_requisicao = requisicao;
			_localPath = _requisicao.Url.LocalPath;
			_pathAndQuery = _requisicao.Url.PathAndQuery;
			_absolutePath = _requisicao.Url.AbsolutePath;

			//Essa propriedade retorna os dados da QuerString (ex: localhot/fred/?id=1&name=teste) vai retornar p ?id=1&name=teste
			_query = _requisicao.Url.Query;

			_host = _requisicao.Url.Host;
			_absoluteUri = _requisicao.Url.AbsoluteUri;

			SetarPrimeiroEndpoint();
		}

		public byte[] ObtemInfoRequisicao()
		{
			return Utilidades.TransformStringToStream(InfoTable());
		}

		public string Queries
		{
			get
			{
				var parametros = ObterParametros();

				if (parametros != null && parametros.Length > 0)
					return string.Join(',', parametros);
				else
					return null;
			}
		}

		//Parametros de query é tudo que vem depois do ?
		public string[] ObterParametros()
		{
			List<string> lista = null;

			if (_query.Contains("?"))
			{
				SetarParametrosQuery(_query, "&", ref lista);

				return lista.ToArray();
			}

			return null;
		}

		public string[] ObterEndpoints()
		{
			var partes = _absolutePath.Split("/");

			return partes;
		}

		#region Privates
		private string InfoTable()
		{
			return @$"<table>
						<tr>
						<td>{nameof(_localPath)}</td>
						<td>{_localPath}</td>
						</tr>
						<tr>
						<td>{nameof(_pathAndQuery)}</td>
						<td>{_pathAndQuery}</td>
						</tr>
						<tr>
						<td>{nameof(_absolutePath)}</td>
						<td>{_absolutePath}</td>
						</tr>
						<tr>
						<td>{nameof(_query)}</td>
						<td>{_query}</td>
						</tr>
						<tr>
						<td>{nameof(_host)}</td>
						<td>{_host}</td>
						</tr>
						<tr>
						<td>{nameof(_absoluteUri)}</td>
						<td>{_absoluteUri}</td>
						</tr>
						<tr>
						<td>{nameof(PrimeiroEndpoint)}</td>
						<td>{PrimeiroEndpoint}</td>
						</tr>
						<tr>
						<td>{nameof(Queries)}</td>
						<td>{Queries}</td>
						</tr>
					</table>";
		}

		private void SetarParametrosQuery(string valores, string delimitador, ref List<string> lista)
		{
			if (lista == null) lista = new List<string>();

			foreach (var valor in valores.Split(delimitador, System.StringSplitOptions.RemoveEmptyEntries))
				lista.Add(valor.Replace("?", ""));
		}

		private void SetarPrimeiroEndpoint()
		{
			var partes = _absolutePath.Split("/", System.StringSplitOptions.RemoveEmptyEntries);

			if (partes.Length == 0)
				PrimeiroEndpoint = null;
			else
				PrimeiroEndpoint = partes[0];
		}
		#endregion
	}
}
