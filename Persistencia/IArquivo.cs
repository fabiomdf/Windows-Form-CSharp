using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Persistencia
{
    public abstract class IArquivo
    {
        public string ArquivoNome; 
        /// <param name="arquivoNome">Passar a estrutura de diretório do arquivo</param>
        public abstract void Salvar(string arquivoNome, Object controlador);

        /// <param name="arquivoNome">Passar a estrutura de diretório do arquivo</param>
        public abstract Object Abrir(string arquivoNome);

        protected abstract Boolean VerificarIntegridade(string arquivoNome);
         
        /// <param name="arquivoNome">Passar a estrutura de diretório do arquivo</param>
        protected void CopiarArquivo(string arquivoNome)
        {
            string _arquivoDestino = arquivoNome.Substring(0, arquivoNome.Length - 4) + ".BKP";
            File.Copy(arquivoNome, _arquivoDestino, true);
            File.SetAttributes(_arquivoDestino, FileAttributes.Hidden);  
        }
        // Deve passar o nome do arquivo que originou a cópia.
        protected void RestaurarCopia(string arquivoNome)
        {
            string _arquivoDestino = arquivoNome.Substring(0, arquivoNome.Length - 4) + ".BKP";
            File.Copy(_arquivoDestino, arquivoNome, true);
            if (File.Exists(arquivoNome))
            {
                File.Delete(_arquivoDestino);
            }
            File.SetAttributes(arquivoNome, FileAttributes.Normal);  
        }
        public Byte[] ToBytes()
        {
            if (String.IsNullOrEmpty(ArquivoNome))
                throw new FileNotFoundException();
            if (!File.Exists(ArquivoNome))
                throw new FileNotFoundException();

            FileStream fs = File.OpenRead(ArquivoNome);
            Byte[] dados = new byte[fs.Length];
            for (int i = 0; i < fs.Length; i++)
            {
                dados[i] = (byte) fs.ReadByte();
            }
            fs.Close();
            return dados;

        }
    }
}
