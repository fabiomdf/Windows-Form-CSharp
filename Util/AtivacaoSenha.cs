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

namespace Util
{
    public static class AtivacaoSenha
    {

        private const string ENDERECO = "sopa.frt.com.br";
        private const string URL_API = "api/sopa_api/ativaProduto/";
        private const string URL_API_DOWNLOAD = "api/sopa_api/download/softwareFiles/";
        private const string ACK = "\"tudo beleza\"";  // Resposta esperada pela API implementada por Vinicius;

        // OBS.: A classe abaixo deverá ser igual ao da API Rest
        public class DadosDownloadProduto
        {
            public string checkVersion { get; set; }
            public string produto { get; set; }
            public string versao { get; set; }
            public string sw_fw { get; set; } // SW ou FW
        }
        public class DadosAtivaProduto
        {
            public string empresa { get; set; }
            public string versao { get; set; }
            public string produto { get; set; }
            public string nome { get; set; }
            public string telefone { get; set; }
            public string email { get; set; }

        }
        public static byte[] ConverterSenhaNumero(String senha)
        {
            return GerarHash(senha, true);
        }

        public static byte[] ConverterSenhaTexto(String senha)
        {
            return GerarHash(senha);
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

        /// Ativar Produto serve para ativar o produto através da API Rest.
        public static void AtivarProduto(DadosAtivaProduto dados)
        {
            try
            {
                HttpClient client;

                client = inicializaHttpClient(ENDERECO);

                string DATA = JsonConvert.SerializeObject(dados, Formatting.Indented);

                System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

                //chamando a api pela url            
                HttpResponseMessage response = client.PostAsync(URL_API, content).Result;


                //se retornar com sucesso busca os dados
                if (response.IsSuccessStatusCode)
                {
                    string resposta = response.Content.ReadAsStringAsync().Result;

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

        public static void ExecutarDownload(string url, string caminhoDestino)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, caminhoDestino);
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

            if (dados.checkVersion.ToLower() != "true")
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

        private static byte[] GerarHash(String senha, bool isNumber = false)
        {
            SHA256 hash = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();

            // Sem senha
            if (String.IsNullOrEmpty(senha.Trim()))
            {
                byte[] resposta = new byte[32];
                for (int i = 0; i < resposta.Length; i++)
                {
                    resposta[i] = byte.MaxValue;
                }
                return resposta;
            }

            // Com senha
            byte[] sourcebytes = (isNumber) ? BitConverter.GetBytes(Convert.ToUInt32(senha)) : encoding.GetBytes(senha);
            byte[] keyBytes = encoding.GetBytes("321894231456156732135765123186423168465105615641");

            hash.Initialize();
            hash.TransformBlock(sourcebytes, 0, sourcebytes.Length, null, 0);
            hash.TransformFinalBlock(keyBytes, 0, keyBytes.Length);

            return hash.Hash;
        }
    }
}

