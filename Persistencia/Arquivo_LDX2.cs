using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Ionic.Zip;

namespace Persistencia
{
    public class Arquivo_LDX2 : IArquivo
    {
        //definiçães de diretórios;
        private string diretorio_raiz = @"c:\teste";//Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public override void Salvar(string arquivoNome, object controlador )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// salva um arquivo LDX2 a partir de um objeto controlador.
        /// </summary>
        /// <param name="arquivoNome">diretorio com nome do arquivo LDX2 a ser salvo.</param>
        /// <param name="diretorio_raiz">diretorio raiz com os arquivos que formam o controlador, 
        /// que serão usados como fonte para compactação.</param>
        public void Salvar(string arquivoNome, string diretorio_raiz)
        {
            ZipFile zip = new ZipFile();
            // Apagar este comentário
            zip.AddDirectory(diretorio_raiz);
            zip.Save(arquivoNome);
        }
        

        public override object Abrir(string arquivoNome)
        {
            ZipFile zip = new ZipFile(arquivoNome);
            
            //zip.ExtractAll(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.DoNotVerify));
            zip.ExtractAll(Path.GetDirectoryName(arquivoNome), ExtractExistingFileAction.OverwriteSilently);

            return null;
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            throw new NotImplementedException();
        }

    }
}
