using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ServidorWebCustomizado
{
	public class ServidorService
	{
		public static void Escutando(string[] prefixes)
		{
			if (prefixes == null || prefixes.Length == 0)
				throw new ArgumentException("prefixes");

			// Essa classe é responsável por ficar escutando os prefixos
			HttpListener listener = new HttpListener();

			// Adiciona os prefixos
			prefixes.ToList().ForEach(prefixo => listener.Prefixes.Add(prefixo));

			//inicia o listener
			listener.Start();
			Utilidades.Escreve("Servidor escutando");

			/*
			Dentro do objeto listener (do tipo HttpListener) iremos obter um objeto do tipo HttpListenerContext através do método GetContext
			Esse objeto do tipo HttpListenerContext é responsável por obter informações da requisição do cliente e responsável por enviar uma resposta
			pra essa implementação
			O método GetContext bloqueia enquanto aguarda pela requisição
			*/
			HttpListenerContext contexto = listener.GetContext();

			//Aqui eu to referenciando a requisição em uma variável
			HttpListenerRequest requisicao = contexto.Request;


			//Essa classe será responsavel procurar pelo endpoint em arquivo fisico
			//Ex: Na rota vou receber localhost:4000/home, eu vou pegar o "home" e vou procurar no diretorio da aplicação por home.html ou home.htm
			//Caso eu encontre eu vou retornar um Stream com os bytes deste arquivo
			//Caso não encontre eu vou retornar null
			ManipulaRequisicao manipularRequisicao = new ManipulaRequisicao(requisicao);

			//TODO
			//Aqui eu criei uma variável para o retorno no Browser quando o endereço for chamado
			//string mensagemParaExibirNoBrowser =  Utilidades.BemVindoHtml();

			//Aqui eu to referenciado a resposta em uma variável
			HttpListenerResponse resposta = contexto.Response;


			//Aqui eu preciso converte-lá (string) para um array de byte pois ele é um dos tipos de dados que eu preciso pra escrever em um stream
			//byte[] buffer = Encoding.UTF8.GetBytes(mensagemParaExibirNoBrowser);
			byte[] buffer = manipularRequisicao.ObtemInfoRequisicao();

			if (manipularRequisicao.PrimeiroEndpoint != null)
			{
				ObtemArquivoHtm arquivoHtm = new ObtemArquivoHtm(manipularRequisicao.PrimeiroEndpoint);

				buffer = arquivoHtm.ObterArrayBytes();

				if (buffer == null)
				{
					resposta.StatusCode = 404;

				}
			}

			if (buffer != null && buffer.Length > 0)
			{
				//Obtendo a referencia do stream da respota
				resposta.ContentLength64 = buffer.Length;
				Stream stream = resposta.OutputStream;

				//Escrevendo no stream
				stream.Write(buffer, 0, buffer.Length);

				//Aqui eu preciso fechar a conexão com o stream caso contrário ele ficar referenciando um endereço na memória desnecessária.
				stream.Close();
			}

			//para o listner
			listener.Stop();
		}
	}
}
