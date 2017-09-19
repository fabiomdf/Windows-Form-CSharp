using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia;
using Persistencia.Videos;

namespace Nucleo
{
    public class TabelaEnderecoPath
    {
        public class Registro
        {
            public UInt32 offset;
            public string path;
            public UInt32 size;

            public Registro(UInt32 offset, string path, UInt32 size)
            {
                this.offset = offset;
                this.path = path;
                this.size = size;
            }
        }
        public List<Registro> lista;
        public TabelaEnderecoPath()
        {
            inicializaLista();
        }
        public void inicializaLista()
        {
            lista = new List<Registro>();
        }        
    }

    public class GeradorArquivo
    {
        public const string DIRETORIO_PAI = @"\..";

        public const int PATH_SIZE_V01 = 42;
        public const int PATH_SIZE_CFG = 50;
        public const int PATH_SIZE_DEFAULT = 64;

        private TabelaEnderecoPath tabelinha = new TabelaEnderecoPath();
        private List<EstruturaArquivo> arquivo;

        private string diretorioRaiz = string.Empty;

        public GeradorArquivo()
        {
            this.arquivo = new List<EstruturaArquivo>();
        }
       
        public void GerarArquivo(String diretorioRaiz, String nomeArquivoDestino)
        {
            this.diretorioRaiz = diretorioRaiz;
            String[] listaArquivos = System.IO.Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories);
            FileStream fs = File.Open(nomeArquivoDestino, FileMode.Create);
            
            for (int i = 0; i < listaArquivos.Length; i++)
            {
                EstruturaArquivo estrutura = getEstrutura(diretorioRaiz, listaArquivos[i]);
                //estrutura.conteudo[0] = 0;
                fs.Write(estrutura.ToArray(), 0, estrutura.Count);
                tabelinha.lista.Add(new TabelaEnderecoPath.Registro(Util.Util.OffSetArquivo,
                                                                    listaArquivos[i].Replace(diretorioRaiz, String.Empty).PadRight(32, '\0').Replace("\\", "/"),
                                                                    (UInt32) estrutura.conteudo.Length));
                Util.Util.OffSetArquivo += (UInt32)((2 * sizeof(UInt32)) + estrutura.conteudo.Length);
            }
            for (int i = 0; i < listaArquivos.Length; i++)            
            {
                ReplacePathToPointer(listaArquivos[i].Replace(diretorioRaiz, String.Empty).Replace("\\", "/"), fs, i);
            }
            
           
            fs.Close();
            GerarArquivoTabela(diretorioRaiz + "tabela.inf");
        }

        private void GerarArquivoTabela(String nomeArquivoDestino)
        {
            FileStream fs = File.Open(nomeArquivoDestino, FileMode.Create);

            for (int i = 0; i < tabelinha.lista.Count; i++)
            {
                if (ContemAlgumArquivo(tabelinha.lista[i].path))
                {
                    byte[] buffer = BitConverter.GetBytes(tabelinha.lista[i].offset);
                    fs.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(tabelinha.lista[i].path.Replace("\\", "/").PadRight(32, '\0'));
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            fs.Close();
        }

        private bool ContemAlgumArquivo(string path)
        {
            bool resposta = !((path.Contains(".rot")) || path.Contains(".rpt") || path.Contains(".msg") || path.Contains(".mpt") || path.Contains(".v01") || path.Contains(".v02") || path.Contains(".pls"));

            return resposta;
        }

        private void ReplacePathToPointer(string nomeArquivo, FileStream fs, int indiceArquivo)
        {
            uint enderecoBase = tabelinha.lista[indiceArquivo].offset;
            nomeArquivo = nomeArquivo.ToLower();

            if (nomeArquivo.EndsWith(".fix"))
            {
                // Bom Dia, Boa Tarde e Boa Noite
                for (uint i = 1; i <= 3; i++)
                {
                    EscreveOffSetArquivo(fs, (64 * i), PATH_SIZE_DEFAULT, enderecoBase);
                } 
            }
            else if (nomeArquivo.EndsWith(".v01"))
            {                
                EscreveOffSetArquivo(fs, 20, PATH_SIZE_V01, enderecoBase);
            }
            else if (nomeArquivo.EndsWith(".mpt"))
            {
                EscreveOffSetArquivo(fs, 64, PATH_SIZE_DEFAULT, enderecoBase);
            }
            else if (nomeArquivo.EndsWith(".rgn"))
            {
                EscreveOffSetArquivo(fs, 64, PATH_SIZE_DEFAULT, enderecoBase);
            }
            else if (nomeArquivo.EndsWith(".cfg"))
            {
                EscreveOffSetArquivo(fs, 12, PATH_SIZE_CFG, enderecoBase);
            }
            else if (nomeArquivo.EndsWith(".rpt"))
            {
                // Ida, Volta e Número
                for (uint i = 1; i <= 3; i++)
                {
                    EscreveOffSetArquivo(fs, (64*i), PATH_SIZE_DEFAULT, enderecoBase);
                } 
            }
            else if (nomeArquivo.EndsWith(".lst"))
            {
                Arquivo_LST arquivo = new Arquivo_LST();
                arquivo.Abrir(this.diretorioRaiz + nomeArquivo);
                uint quantidadeArquivos = arquivo.qtdArquivos;
                // Pegar a quantidade de arquivos
                for (uint i = 1; i <= quantidadeArquivos; i++)
                {
                    EscreveOffSetArquivo(fs, (64*i), PATH_SIZE_DEFAULT, enderecoBase);
                }                
            }
            else if (nomeArquivo.EndsWith(".pls"))
            {                              
                Arquivo_PLS arquivo = new Arquivo_PLS(diretorioRaiz);
                arquivo.Abrir(this.diretorioRaiz + nomeArquivo);
                uint quantidadeArquivos = arquivo.qtdArquivos;
                
                // Pegar a quantidade de arquivos
                for (uint i = 1; i <= quantidadeArquivos; i++)
                {
                    EscreveOffSetArquivo(fs, (64 * i), PATH_SIZE_DEFAULT, enderecoBase);
                }   
            }            
        }
        
        private void EscreveOffSetArquivo(FileStream fs, uint indiceInicial, int pathSize, uint enderecoBase)
        {
            byte[] buffer = new byte[64];
            string texto = string.Empty;

            // Ler o Path
            fs.Seek(enderecoBase + (2 * sizeof(UInt32)) + indiceInicial, SeekOrigin.Begin);
            fs.Read(buffer, 0, pathSize);
            texto = Encoding.ASCII.GetString(buffer).PadRight(32, '\0').Replace("\\", "/");

            // Pega o offset
            buffer = getOffSet(texto, pathSize);
            // Escreve o offset
            fs.Seek(enderecoBase + (2*sizeof(UInt32)) + indiceInicial, SeekOrigin.Begin);
            fs.Write(buffer, 0, pathSize);            
        }
        private byte[] getOffSet(string texto, int tamanhoResposta)
        {
            texto = texto.Replace("\0", string.Empty);

            // Há alguns paths gravados com a barra "/"
            if (texto.StartsWith("/"))
                texto = texto.Substring(1).Replace("\0", string.Empty);

            byte[] resposta = new byte[tamanhoResposta];
            for (int i = 0; i < resposta.Length; i++)
            {
                resposta[i] = 0xff;
            }

            for (int i=0;i<tabelinha.lista.Count;i++)
            {
                if (tabelinha.lista[i].path.Replace("\0", string.Empty).Equals(texto))
                {
                    BitConverter.GetBytes(tabelinha.lista[i].offset).CopyTo(resposta, 0);
                    break;
                }
            }
            return resposta;
        }
        private EstruturaArquivo getEstrutura(String diretorioRaiz, string nomeArquivo)
        {
            EstruturaArquivo estruturaTemporaria = new EstruturaArquivo();
            estruturaTemporaria.tamanho = (UInt32)File.OpenRead(nomeArquivo).Length;
            estruturaTemporaria.conteudo = File.ReadAllBytes(nomeArquivo);
            estruturaTemporaria.path = Encoding.ASCII.GetBytes(nomeArquivo.Replace(diretorioRaiz, String.Empty).PadRight(32, '\0').Replace("\\","/"));

            return estruturaTemporaria;
        }
    }
}
