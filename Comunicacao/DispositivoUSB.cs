using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Recursos;

namespace Comunicacao
{
    public class DispositivoUSB : IDispositivo
    {        
        public String DriveDestino = @"c:\teste";
        public String nomeArquivo = @"c:\teste\arquivo.ldx2";

        public void Conectar()
        {
            throw new NotImplementedException();
        }

        public void Desconectar()
        {
            throw new NotImplementedException();
        }

        public string RealizarTransmissao(System.ComponentModel.BackgroundWorker worker)
        {
            //ZipFile.ExtractToDirectory(nomeArquivo, DriveDestino);
            throw new NotImplementedException();
        }

        public int ReceberDensidadeMemoria()
        {
            throw new NotImplementedException();
        }

        public bool RestaurarProduto()
        {
            throw new NotImplementedException();
        }

        public void EnviarDataHora()
        {
            /* Não faz nada, pois o relógio ficaria desatualizado se fosse atualizado pelo PENDRIVE. */
        }

        public void Flush()
        {
            /* Não executa nada, pois o PENDRIVE não realiza Flush. */
        }

        public void Enviar(byte dado)
        {
            throw new NotImplementedException();
        }

        public void Enviar(string texto)
        {
            throw new NotImplementedException();
        }

        public void Enviar(byte[] buffer, int offSet, int count)
        {
            throw new NotImplementedException();
        }

        public int ReceberArray(byte[] buffer, int offSet, int count)
        {
            throw new NotImplementedException();
        }

        public int ReceberByte()
        {
            throw new NotImplementedException();
        }

        public string ReceberTexto(int tamanhoString)
        {
            throw new NotImplementedException();
        }

        public void EnviarSenha(byte[] arrayEntrada)
        {
            /* Não executa nada, pois o PENDRIVE não envia senha. */
        }

        public override void DesabilitarSenha(byte[] arrayEntrada)
        {
            /* Não executa nada, pois o PENDRIVE não desabilita a utilização de senha. */
        }

        public override bool ChecarSenha(byte[] arrayEntrada)
        {
            /* Não executa nada, pois o PENDRIVE não checa senha. */
            return true;
        }

        public bool VerificarUtilizacaoSenha()
        {            
            /* Não executa nada, pois o PENDRIVE não verifica utilização de senha. */
            return false;
        }

        public string LerRoteiroSelecionado()
        {
            throw new NotImplementedException();
        }

        public bool ApagarPaineis()
        {
            return true;
        }

        public bool EnviarFirmware(byte[] bytesFirmware)
        {
            
            FileInfo fi = new FileInfo(Defines.OrigemFirmwareWifi);

            File.Copy(Defines.OrigemFirmwareOptWifi, DriveDestino + fi.Name);
            File.Copy(Defines.OrigemFirmwareWifi, DriveDestino + fi.Name);
            File.Copy(Defines.OrigemFirmwareOptControlador, DriveDestino + fi.Name);
            File.Copy(Defines.OrigemFirmwareControlador, DriveDestino + fi.Name);
            
            return true;
        }

        public int CalcularPassosParametrizados(int passo, int qtdTotalPaginas)
        {
            throw new NotImplementedException();
        }


        public string ReceberNumeroSerieControlador()
        {
            /* Não faz nada, pois o PENDRIVE não tem como coletar o número de série. */
            return String.Empty;
        }
    }
}
