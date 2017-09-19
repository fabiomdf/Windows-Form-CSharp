using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Comunicacao.Erros;
using Recursos;

namespace Comunicacao
{
    public abstract class IDispositivo
    {
        internal const int ACK = 42;
        // Tamanho do Pacote de Envio do Firmware
        internal const int TAMANHOPACOTE = 1024;

        public StatusTransmissao Status;
        public virtual void Conectar() { }
        public virtual void Desconectar() { }
        public virtual string RealizarTransmissao(BackgroundWorker worker) { return String.Empty; }
        public virtual int ReceberDensidadeMemoria() { return -1; }
        public virtual bool RestaurarProduto() { return false; }
        public virtual void EnviarDataHora() { }
        public virtual void Flush() { }
        public virtual void Enviar(byte dado) { }
        public virtual void Enviar(string texto) { }
        public virtual void Enviar(byte[] buffer, int offSet, int count) { }
        public virtual int ReceberArray(byte[] buffer, int offSet, int count) { return -1; }
        public virtual int ReceberByte() { return -1; }
        public virtual String ReceberTexto(int tamanhoString) { return String.Empty; }
        /////////////////////////////////////////////////////////////////                                
        public virtual void EnviarSenha(Byte[] arrayEntrada)
        {
            // Comando do Protocolo "//SS"
            Byte[] parametroProtocolo = new byte[] { 47, 47, 83, 83 }; 

            EnviarDados(arrayEntrada, parametroProtocolo);
        } 
        public virtual void DesabilitarSenha(byte[] arrayEntrada)
        {
            // Comando do Protocolo "//NN"
            Byte[] parametroProtocolo = new byte[] { 47, 47, 78, 78 }; //ComandosProtocolos.ProtocoloDesabilitarSenha();

            EnviarDados(arrayEntrada, parametroProtocolo);
        }
        public virtual Boolean ChecarSenha(Byte[] arrayEntrada)
        {
            Boolean resposta;
            // Comando do Protocolo "//CC"
            Byte[] parametroProtocolo = new byte[] { 47, 47, 67, 67 };

            resposta = EnviarDados(arrayEntrada, parametroProtocolo);

            return resposta;
        }
        public virtual bool VerificarUtilizacaoSenha()
        {
            Boolean retorno = true;
            byte resposta = 1;

            byte[] parametroProtocolo = new byte[] { 47, 47, 115, 115 }; //ComandosProtocolos.ProtocoloVerificarSenha();

            // PASSO 1 - ENVIAR PROTOCOLO
            this.Enviar(parametroProtocolo, 0, parametroProtocolo.Length);

            // PASSO 2 - RECEBER CONFIRMAÇÃO
            resposta = (byte)this.ReceberByte();

            if (resposta != ACK)
                retorno = false;

            // PASSO 3 - RETORNAR
            return retorno;
        }
        public virtual bool EnviarDados(byte[] arrayEntrada, byte[] parametrosProtocolo)
        {
            Byte[] arraySaida = new Byte[arrayEntrada.Length + 2];
            Int32 tamanhoArrayEntrada = arrayEntrada.Length;
            Boolean resultado = true;
            Byte resposta;

            // PREPARAR ARRAY A SER ENVIADO
            for (int i = 0; i < arrayEntrada.Length; i++)
            {
                arraySaida[i] = arrayEntrada[i];
            }

            // PASSO 1 - ENVIAR PROTOCOLO
            this.Enviar(parametrosProtocolo, 0, parametrosProtocolo.Length);

            // PASSO 2 - RECEBER CONFIRMAÇÃO
            resposta = (byte)this.ReceberByte();

            if (resposta != ACK)
                throw new FalhaComunicacaoException("NACK");

            // PASSO 3 - ENVIAR DADOS
            ushort crc16 = CRC16CCITT.Calcular(arrayEntrada);

            arraySaida[tamanhoArrayEntrada + 0] = (byte)(crc16 % 256);
            arraySaida[tamanhoArrayEntrada + 1] = (byte)(crc16 / 256);

            this.Enviar(arraySaida, 0, arraySaida.Length);

            // PASSO 4 - RECEBER CONFIRMAÇÃO
            resposta = (byte)this.ReceberByte();

            if (resposta != ACK)
            {
                //throw new SenhaNaoConfereException("Senha não confere");
                resultado = false;
            }

            return resultado;
        }
        public virtual String LerRoteiroSelecionado(ModoLeituraRoteiro modo) { return String.Empty; }
        public virtual Boolean ApagarPaineis() { return false; }
        public virtual Boolean EnviarFirmware(Byte[] bytesFirmware) { return false; }
        public virtual int CalcularPassosParametrizados(int passo, int qtdTotalPaginas) { return -1; }
        public virtual String ReceberNumeroSerieControlador() { return String.Empty; }
    }
}
