using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

namespace Util
{
    public static class AtualizacaoSoftware
    {

        private const string ENDERECO = "sopa.frt.com.br";
        private const string URL_API = "api/sopa_api/ativaProduto/";
        private const string URL_API_DOWNLOAD = "api/sopa_api/download/softwareFiles/";
        private const string ACK = "\"tudo beleza\"";  // Resposta esperada pela API implementada por Vinicius;

        // OBS.: A classe abaixo deverá ser igual ao da API Rest
        public class DadosDownloadProduto
        {            
            public string produto { get; set; }
            public string versao { get; set; }
            public string sw_fw { get; set; } // SW ou FW
            public string getversion { get; set; }
        }
     
        

        private static HttpClient inicializaHttpClient(string enderecoIP)
        {
            HttpClient client;            

            client = new HttpClient();

            client.BaseAddress = new Uri("http://" + enderecoIP );
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static void ExecutarDownloadSincrono(string url, string caminhoDestino)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, caminhoDestino);
        }
        public static void ExecutarDownloadAssincrono(string url, string caminhoDestino, AsyncCompletedEventHandler completo, DownloadProgressChangedEventHandler progressoFeito)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(completo);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressoFeito);
            webClient.DownloadFileAsync(new Uri(url), caminhoDestino);
        }
        /// Ativar Produto serve para ativar o produto através da API Rest.
        public static string VerificarDownloadProduto(DadosDownloadProduto dados)
        {
            string resposta = string.Empty;

            try
            {
                HttpClient client;

                client = inicializaHttpClient(ENDERECO);

                string DATA = JsonConvert.SerializeObject(dados, Formatting.Indented);

                System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

                //chamando a api pela url            
                HttpResponseMessage response = client.PostAsync(URL_API_DOWNLOAD, content).Result;


                //se retornar com sucesso busca os dados
                if (response.IsSuccessStatusCode)
                {
                     resposta = response.Content.ReadAsStringAsync().Result;
                    // Retirar o \ do inicio e Retirar o \ do final
                    resposta = resposta.Substring(1).Substring(0, resposta.Length - 2);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return resposta;
                    }
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                        {
                            throw new EntryPointNotFoundException("Erro na Ativação, Não encontrou o cadastro da empresa!!!");
                        }
                        throw new HttpRequestException("Erro na Ativação!!!");
                    }
                }

                ////Se der erro na chamada, mostra o status do código de erro.
                else
                {
                    if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                    {
                        throw new EntryPointNotFoundException("Erro na Ativação, Não encontrou o cadastro da empresa!!!");
                    }
                    throw new HttpRequestException(response.StatusCode.ToString() + " - " + response.ReasonPhrase);
                }
                return resposta;
            }
            catch (EntryPointNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw new HttpRequestException("Erro na Ativação! Tente novamente ou entre em contato com a FRT.");
            }
        }
        /// Ativar Produto serve para ativar o produto através da API Rest.
        public static string VerificarVersaoDownloadProduto(DadosDownloadProduto dados)
        {
            string resposta = string.Empty;

            if (dados.getversion.ToLower() != "true")
            {
                return "0.0.0";
            }

            try
            {
                HttpClient client;

                client = inicializaHttpClient(ENDERECO);

                string DATA = JsonConvert.SerializeObject(dados, Formatting.Indented);

                System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

                //chamando a api pela url            
                HttpResponseMessage response = client.PostAsync(URL_API_DOWNLOAD, content).Result;

                
                //se retornar com sucesso busca os dados
                if (response.IsSuccessStatusCode)
                {
                    resposta = response.Content.ReadAsStringAsync().Result;
                    // Retirar o \ do inicio e Retirar o \ do final
                    resposta = resposta.Substring(1).Substring(0, resposta.Length - 2);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return resposta;
                    }

                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                        {
                            throw new EntryPointNotFoundException("Erro na Ativação, Não encontrou o cadastro da empresa!!!");
                        }
                        throw new HttpRequestException("Erro na Ativação!!!");
                    }
                }

                ////Se der erro na chamada, mostra o status do código de erro.
                else
                {
                    if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                    {
                        throw new EntryPointNotFoundException("Erro na Ativação, Não encontrou o cadastro da empresa!!!");
                    }
                    throw new HttpRequestException(response.StatusCode.ToString() + " - " + response.ReasonPhrase);
                }
                return resposta;
            }
            catch (EntryPointNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw new HttpRequestException("Erro na Ativação! Tente novamente ou entre em contato com a FRT.");
            }
        }


    }
}

