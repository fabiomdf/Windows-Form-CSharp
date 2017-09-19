using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Persistencia
{
    public class Arquivo_LDX : IArquivo
    {
        List<String> listaVerificacoes = new List<string>();
        public Arquivo_LDX()
        {
            InicializaListaVerificacoes();
        }

        private void InicializaListaVerificacoes()
        {
 	        listaVerificacoes.Add("[FileInfo]");
            listaVerificacoes.Add("NomeArquivo");
            listaVerificacoes.Add("[ControladorInfo]");
            listaVerificacoes.Add("[TecladoInfo]");
            listaVerificacoes.Add("[Painel - 0]");
            listaVerificacoes.Add("[Roteiro Numero]");
            listaVerificacoes.Add("[Roteiro Ida]");
            listaVerificacoes.Add("[Roteiro Volta]");
            listaVerificacoes.Add("[Mensagem]");
            listaVerificacoes.Add("[Saudacao]");
            listaVerificacoes.Add("[MensagemEspecial]");
            listaVerificacoes.Add("[Fim de Arquivo]");
        }
        /* Método não será implementado para Salvar arquivos LDX */
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
            TextReader fileInput =  new StreamReader(arquivoNome, Encoding.UTF8, true);
            bool passou = false;

            foreach (String atributo in listaVerificacoes)
            {
                string linha = "";
                
                while (fileInput.Peek() > -1)
                {
                    linha = fileInput.ReadLine();
                    passou = false;
                    if (linha.StartsWith(atributo))
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
