using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Erros;
using Util;

namespace Persistencia
{
    public class Arquivo_MSG
    {
        public unsafe struct FormatoMensagem
        {
            public Byte versao;
            public Byte reservado;
            public UInt16 id;
            public fixed byte labelMensagem [20];
            public fixed Byte reservado2 [38];
            public UInt16 crc;
        }
        //public Byte versao { get; set; }
        private Byte versao = 1;
        public Byte reservado { get; set; }
        public UInt16 id { get; set; }
        public string labelMensagem { get; set; }
        public Byte[] reservado2{ get; set; }
        public UInt16 crc { get; set; }
        
        public Arquivo_MSG()
        {
            //this.versao = 1;
            this.reservado = 0;
            this.id = 0;
            this.labelMensagem = "*FRT*";
            this.reservado2 = new byte[38];
            this.crc = 0;

        }
        
        public Arquivo_MSG(Arquivo_MSG oldValue)
        {
            this.versao = oldValue.versao;
            this.reservado = oldValue.reservado;
            this.id = oldValue.id;
            this.labelMensagem = oldValue.labelMensagem;
            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);
            this.crc = oldValue.crc;
        }

        public void Salvar(string arquivoNome)
        {
            if (String.IsNullOrEmpty(this.labelMensagem))
            {
                throw new FormatException();
            }
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
            
        }
        /*
        public void Salvar(string arquivoNome, ControladorPontos.Controlador Ctrdl)
        {
            for(int painel = 0; painel < Ctrdl.QuantidadePaineis; painel++)
            {
                for(int msg = 0; msg < Ctrdl.Paineis[painel].Mensagens.Count; msg++)
                {
                    Arquivo_MSG amsg = new Arquivo_MSG();

                    amsg.id = (ushort)Ctrdl.Paineis[painel].Mensagens[msg].Id;
                    amsg.labelMensagem = Ctrdl.Paineis[painel].Mensagens[msg].LabelMensagem;
                    amsg.versao = 1;
                    amsg.Salvar(arquivoNome+ @"\" + msg.ToString("d3") + ".msg");
                }
            }
        }
        */
        public object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {

                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    FromBytesToFormatoPainelCfg(dados);
                    return this;
                }
            }
            return null;
        }
        private void FromBytesToFormatoPainelCfg(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagem* mensagem = (FormatoMensagem*)pSrc;

                    this.versao = mensagem->versao;
                    this.reservado = mensagem->reservado;
                    this.id = mensagem->id;
                    this.labelMensagem = ArrayLDX2.ByteArrayToString(mensagem->labelMensagem, 20);

                    for (int i = 0; i < 38; i++)
                    {
                        this.reservado2[i] = mensagem->reservado2[i];
                    }

                    this.crc = mensagem->crc;                    
                }
            }
        }
        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoMensagem)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoMensagem* mensagem = (FormatoMensagem*)pSrc;


                    mensagem->versao = this.versao;
                    mensagem->reservado = this.reservado;
                    mensagem->id = this.id;
                    ArrayLDX2.StringToByteArray(mensagem->labelMensagem, this.labelMensagem, 20);

                    for (int i = 0; i < 38; i++)
                    {
                        mensagem->reservado2[i] = this.reservado2[i];
                    }

                    mensagem->crc = this.crc;   

                    return resultado;
                }
            }
        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoMensagem* regiao = (FormatoMensagem*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }
        public bool VerificarIntegridade(string arquivoNome)
        {
            bool resposta = false;
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            resposta = VerificarCRC(dados);
            if (!resposta)
            {
                throw new CRCFileException("CRC file error.");
            }
            resposta = VerificarTamanhoArquivo(fs);
            if (!resposta)
            {
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return resposta;
        }
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;

            Byte[] dadosCRC = new byte[sizeof(FormatoMensagem) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoMensagem* regiao = (FormatoMensagem*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&regiao->crc - (int)pSrc);
                Array.Copy(dados, ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&regiao->crc - (int)pSrc,
                           sizeof(FormatoMensagem) - ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
        public void CriarMensagemPadrao()
        {
            //this.versao = 1;
            this.reservado = 0xFF;
            this.id = 00;
            this.labelMensagem = "Emergência";
            this.reservado2 = new byte[38];
            for (int i = 0; i < 38; i++)
            {
                this.reservado2[i] = 0x00;
            }
            
            this.crc = 0;
        }

        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagem* parametros = (FormatoMensagem*) pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length == sizeof(FormatoMensagem));
            }
            return resposta;
        }
    }
}
