using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Persistencia
{
    public class Arquivo_LD6 : IArquivo
    {
        List<String> listaVerificacoes = new List<string>();
        public Arquivo_LD6()
        {
            InicializaListaVerificacoes();
        }

        private void InicializaListaVerificacoes()
        {
 	        listaVerificacoes.Add("[FileInfo]");
            listaVerificacoes.Add("NomeArquivo");
            listaVerificacoes.Add("[Controlador]");
            listaVerificacoes.Add("[ConfPainel]");
            listaVerificacoes.Add("[Configuracoes]");
            listaVerificacoes.Add("[Roteiros]");
            listaVerificacoes.Add("[Mensagens]");
        }
        /* Método não será implementado para Salvar arquivos LD6 */
        public override void Salvar(string arquivoNome, object controlador)
        {
            throw new NotImplementedException();
        }

        public override object Abrir(string arquivoNome)
        {
            throw new NotImplementedException();
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            TextReader fileInput = new StreamReader(arquivoNome, Encoding.UTF8, true);
            bool passou = false;

            foreach (String atributo in listaVerificacoes)
            {
                string linha = "";

                while (fileInput.Peek() > -1)
                {
                    linha = fileInput.ReadLine();
                    passou = false;
                    if (linha.Contains(atributo))
                    {
                        passou = true;
                        break;
                    }
                }
            }
            fileInput.Close();

            return passou;
        }
    }
}
