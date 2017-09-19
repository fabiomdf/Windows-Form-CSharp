using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Recursos;
using System.Net.Sockets;
using Comunicacao.Erros;
using System.Threading;
using System.IO;
using System.Net;

namespace Comunicacao
{
    public class DispositivoWifi : IDispositivo
    {
        private const int QTDRETRY = 3;

        // Socket de Envio
        private TcpClient tcpClient = new TcpClient();
        public String EnderecoIP { get; set; }
        public int Porta { get; set; }
        public int SendTimeout = 2000;
        public int ReceiveTimeout = 2000;
        private bool isOnline = false;
        private Byte[] ack = new byte[1] {ACK}; //Ack = "*";

        public String NumeroRoteiroSelecionado { get; set; }
        public Boolean ForcarEnvioProgramacao { get; set; }

        public override void Conectar()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(this.EnderecoIP), Porta);
                tcpClient.SendTimeout = this.SendTimeout;
                tcpClient.ReceiveTimeout = this.ReceiveTimeout;
                while (!tcpClient.Connected) ;
            }
            catch (SocketException)
            {
                isOnline = false;
                throw new FalhaComunicacaoException("Não foi possível conectar com o host " + this.EnderecoIP);
            }
            catch (FormatException)
            {
                isOnline = false;
                throw new FalhaComunicacaoException("Endereço IP inválido.[" + this.EnderecoIP + "]");
            }
            catch (ArgumentNullException)
            {
                isOnline = false;
                throw new FalhaComunicacaoException("Endereço IP inválido. [Nulo]");
            }
            catch (ArgumentException)
            {
                isOnline = false;
                throw new FalhaComunicacaoException("Endereço IP inválido. ");
            }
        }

        public override void Desconectar()
        {
            isOnline = false;

            try
            {
                tcpClient.Client.Disconnect(true);
            }
            catch
            {
            }
            finally
            {
                tcpClient.Client.Close();
            }
        }

        
        private bool VerificarCompatibilidadeRoteiro(System.ComponentModel.BackgroundWorker worker)
        {
            worker.ReportProgress(this.Status.percentualProgresso++);
            if (!ForcarEnvioProgramacao)
            {
                this.Status.label = "Verificando Compatibilidade de Roteiros... ";

                String rotulo = LerRoteiroSelecionado().Trim();

                if (rotulo.Equals(NumeroRoteiroSelecionado.Trim()))
                {
                    this.Status.label = "Roteiro Verificado... ";
                    return true;
                }
                else
                {
                    this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                    this.Status.label = "Roteiro não compativel";
                    return false;
                }
            }
            worker.ReportProgress(this.Status.percentualProgresso++);
            return true;
        }

        private bool VerificarCompatibilidadeFamilias(System.ComponentModel.BackgroundWorker worker, String Familia)
        {
            string familiaArquivo = String.Empty;
            string familiaControlador = String.Empty;
            String[] versao = new string[1];
            char[] separador = new char[1] {' '};

            this.Status.label = "Verificando Compatibilidade de Famílias... ";
            worker.ReportProgress(this.Status.percentualProgresso++);

            string idProduto = IdentificarControlador();
            //ControladorPontos = idProduto;

            if (idProduto.Length != 16)
            {
                this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                this.Status.label = "Não foi possível identificar a Família do Controlador.";
                worker.ReportProgress(this.Status.percentualProgresso++);
                return false;
            }

            // Separar elementos do idProduto
            versao = idProduto.Trim().Split(separador, 2);
            familiaControlador = versao[1].Substring(0, 2);

            // Separar elementos do controlador.Familia
            versao = Familia.Trim().Split(separador, 2);
            familiaArquivo = versao[1].Substring(0, 2);
            // Comparar se compatíveis
            if (!familiaArquivo.Equals(familiaControlador))
            {
                this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                this.Status.label = "Família não compatível.";
                worker.ReportProgress(this.Status.percentualProgresso++);
                return (familiaArquivo.Equals(familiaControlador));
            }
            this.Status.label = "Verificada Compatibilidade de Famílias... ";
            worker.ReportProgress(this.Status.percentualProgresso++);
            return (familiaArquivo.Equals(familiaControlador));
        }

        private string IdentificarControlador()
        {
            byte[] bytesRecebidos = new byte[16];
            byte[] comando = new byte[4] {47, 47, 63, 63};
            string resposta = "";
            Flush();

            for (int retryDados = 0; retryDados < 3; retryDados++)
            {
                EnviarParaControlador(comando, 0, 4);
                try
                {
                    for (int i = 0; i < 16; i++)
                    {
                        bytesRecebidos[i] = (byte) (ReceberByte());
                    }
                    ushort crc16 = CRC16CCITT.Calcular(bytesRecebidos);

                    int hicrc = ReceberByte();
                    int locrc = ReceberByte();

                    if (crc16 == (hicrc*256) + locrc)
                    {
                        Enviar(Encoding.UTF8.GetBytes("*"), 0, 1);
                        break;
                    }
                    else
                    {
                        Enviar(Encoding.UTF8.GetBytes("#"), 0, 1);
                        if (retryDados == 2)
                        {
                            resposta = "Nack - Não foi possível Identificar o Controlador.";
                        }
                    }
                }
                catch
                {
                    if (retryDados == 2)
                    {
                        resposta = "TimeOutError - Não foi possível Identificar o Controlador. ";
                    }
                }
            }
            resposta = ASCIIEncoding.UTF8.GetString(bytesRecebidos);
            return resposta;
        }

        
        public override void Flush()
        {
            if (null != tcpClient.Client)
            {
                if (tcpClient.Client.Connected)
                {
                    byte[] buffer = new byte[1];

                    while (tcpClient.Client.Available > 0)
                    {
                        int teste = tcpClient.Client.Receive(buffer);
                    }

                }
            }
        }

        
        public override void EnviarSenha(byte[] arrayEntrada)
        {
            // Comando do Protocolo "//SS"
            Byte[] parametroProtocolo = new byte[] {47, 47, 83, 83}; //ComandosProtocolos.ProtocoloInserirSenha();

            EnviarDados(arrayEntrada, parametroProtocolo);
        }

        public override bool EnviarDados(byte[] arrayEntrada, byte[] parametrosProtocolo)
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
            this.EnviarParaControlador(parametrosProtocolo, 0, parametrosProtocolo.Length);

            // PASSO 2 - RECEBER CONFIRMAÇÃO
            resposta = (byte) this.ReceberByte();

            if (resposta != ACK)
                throw new FalhaComunicacaoException("NACK");

            // PASSO 3 - ENVIAR DADOS
            ushort crc16 = CRC16CCITT.Calcular(arrayEntrada);

            arraySaida[tamanhoArrayEntrada + 0] = (byte) (crc16%256);
            arraySaida[tamanhoArrayEntrada + 1] = (byte) (crc16/256);

            this.Enviar(arraySaida, 0, arraySaida.Length);

            // PASSO 4 - RECEBER CONFIRMAÇÃO
            resposta = (byte) this.ReceberByte();

            if (resposta != ACK)
            {
                //throw new SenhaNaoConfereException("Senha não confere");
                resultado = false;
            }

            return resultado;
        }

        public override void DesabilitarSenha(byte[] arrayEntrada)
        {
            // Comando do Protocolo "//NN"
            Byte[] parametroProtocolo = new byte[] {47, 47, 78, 78}; //ComandosProtocolos.ProtocoloDesabilitarSenha();

            EnviarDados(arrayEntrada, parametroProtocolo);
        }

        public override bool ChecarSenha(byte[] arrayEntrada)
        {
            Boolean resposta;
            // Comando do Protocolo "//CC"
            Byte[] parametroProtocolo = new byte[] {47, 47, 67, 67}; // ComandosProtocolos.ProtocoloChecarSenha();

            resposta = EnviarDados(arrayEntrada, parametroProtocolo);

            return resposta;
        }

        public bool VerificarUtilizacaoSenha()
        {
            Boolean retorno = true;
            byte resposta = 1;

            byte[] parametroProtocolo = new byte[] {47, 47, 115, 115}; //ComandosProtocolos.ProtocoloVerificarSenha();

            // PASSO 1 - ENVIAR PROTOCOLO
            this.EnviarParaControlador(parametroProtocolo, 0, parametroProtocolo.Length);

            // PASSO 2 - RECEBER CONFIRMAÇÃO
            resposta = (byte) this.ReceberByte();

            if (resposta != ACK)
                retorno = false;

            // PASSO 3 - RETORNAR
            return retorno;
        }


        public bool ApagarPaineis()
        {
            int resposta = -1;

            // Comando "//BB" com "//DD" adicionado no começo para que o wifi transfira para o controlador.
            byte[] comando = new byte[4] {47, 47, 66, 66};

            for (int retry = 0; retry < 2; retry++)
            {
                resposta = -1;
                // com o comando "//DD" adicionado no começo para que o wifi transfira para o controlador.
                EnviarParaControlador(comando, 0, comando.Length);

                try
                {
                    resposta = (ReceberByte());
                    if (resposta == ACK)
                    {
                        break;
                    }
                }
                catch
                {
                }
            }
            return (resposta == ACK);
        }

        public bool EnviarFirmware(byte[] bytesFirmware)
        {
            int deslocamento = 0;
            byte[] temp = new byte[TAMANHOPACOTE];
            int retry = 0;
            byte[] addr = new byte[4];
            byte[] crc = new byte[2];
            byte[] array = new byte[1024];
            /* Comando Enviar Firmware - "//FF" */
            byte[] comando = new byte[] {47, 47, 70, 70};

            if (bytesFirmware.Length > TAMANHOPACOTE)
            {
                deslocamento = TAMANHOPACOTE;
            }

            int qtdIteracoes = bytesFirmware.Length/TAMANHOPACOTE;

            if (bytesFirmware.Length%TAMANHOPACOTE > 0)
            {
                int novoTamanho = bytesFirmware.Length;
                qtdIteracoes++;
                novoTamanho += (TAMANHOPACOTE - (bytesFirmware.Length%TAMANHOPACOTE));
                Array.Resize(ref bytesFirmware, novoTamanho);
            }



            for (int i = 0; i < qtdIteracoes; i++)
            {
                array = new byte[TAMANHOPACOTE];
                addr = BitConverter.GetBytes((Int32) ((TAMANHOPACOTE*i) + deslocamento));

                Array.Copy(bytesFirmware, TAMANHOPACOTE*i, array, 0, TAMANHOPACOTE);

                array = ArrayLDX2.Concat<Byte>(addr, array);
                crc = BitConverter.GetBytes(CRC16CCITT.Calcular(array));

                array = ArrayLDX2.Concat<Byte>(array, crc);
                array = ArrayLDX2.Concat<Byte>(comando, array);

                Enviar(array, 0, array.Length);
                //Flush();
                int resposta = ReceberByte();
                if (resposta != ACK)
                {
                    retry++;
                    i--;
                    if (retry > 4)
                    {
                        throw new Exception("PAGINA SEM ACK... ");
                        return false;
                    }
                    continue;
                }
                int numeroPagina = ((Int32) ((TAMANHOPACOTE*i) + deslocamento)/TAMANHOPACOTE);
                temp = ReceberPaginaFirmware(numeroPagina);

                if (numeroPagina != 0)
                {
                    numeroPagina--;
                }

                for (int j = 0; j < TAMANHOPACOTE; j++)
                {
                    if (temp[j] != bytesFirmware[(numeroPagina*TAMANHOPACOTE) + j])
                    {
                        throw new Exception("PAGINAS NAO CONFEREM");
                    }
                }
            }
            return true;
        }

       
        public void AtualizarFirmwareControlador(System.ComponentModel.BackgroundWorker worker)
        {
            /* Verifica se o usuário deseja atualizar firmware do módulo wifi. */
            worker.ReportProgress(10);
            this.Status.label = "Atualizando o Firmware do Controlador... ";
            worker.ReportProgress(20);
            //String versaoAnterior = IdentificarModuloWifi();
            if (!EnviarArquivoFirmware(Defines.OrigemFirmwareOptControlador))
            {
                this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                this.Status.label = "Firmware do controlador não atualizado";
                return;
            }
            else
            {
                if (!EnviarArquivoFirmware(Defines.OrigemFirmwareControlador))
                {
                    this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                    this.Status.label = "Firmware do controlador não atualizado";
                    return;
                }
                //RestaurarModuloWifi();

                this.Status.tipoStatus = TipoStatusEnvio.SUCESSO;
                this.Status.label = "Firmware do controlador atualizado !!! ";
                worker.ReportProgress(100);
                //this.threadEnvio.Abort();
                Thread.Sleep(5000);
                return;
            }
        }

        public void AtualizarFirmwareWifi(System.ComponentModel.BackgroundWorker worker)
        {
            /* Verifica se o usuário deseja atualizar firmware do módulo wifi. */
            worker.ReportProgress(10);
            this.Status.label = "Atualizando o Firmware do Wifi... ";
            worker.ReportProgress(20);
            //String versaoAnterior = IdentificarModuloWifi();
            if (!EnviarArquivoFirmware(Defines.OrigemFirmwareOptWifi))
            {
                this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                this.Status.label = "Firmware do controlador não atualizado";
                return;
            }
            else
            {
                if (!EnviarArquivoFirmware(Defines.OrigemFirmwareWifi))
                {
                    this.Status.tipoStatus = TipoStatusEnvio.ERRO;
                    this.Status.label = "Firmware do controlador não atualizado";
                    return;
                }
                //RestaurarModuloWifi();

                this.Status.tipoStatus = TipoStatusEnvio.SUCESSO;
                this.Status.label = "Firmware do controlador atualizado !!! ";
                worker.ReportProgress(100);
                //this.threadEnvio.Abort();
                Thread.Sleep(5000);
                return;
            }
        }

        #region Métodos que não são da Interface IDispositivo

        private bool EnviarArquivoFirmware(string nomeArquivo)
        {
            FileStream fs = File.OpenRead(nomeArquivo);
            Byte[] conteudoArquivo = new Byte[fs.Length];
            fs.Read(conteudoArquivo, 0, conteudoArquivo.Length);
            fs.Close();

            if (!EnviarFirmware(conteudoArquivo))
            {
                if (nomeArquivo.EndsWith(".opt"))
                    throw new FalhaComunicacaoException("Não foi possível atualizar o arquivo OPT do firmware!");
                else
                    throw new FalhaComunicacaoException("Não foi possível atualizar o arquivo FIR do firmware!");
            }
            return true;
        }

        private byte[] ReceberPaginaFirmware(int indicePagina)
        {
            int retry = 0;
            /* Comando para Ler Firmware - "//pp" */
            byte[] comando = new byte[] {47, 47, 102, 102};
            byte[] addr = new byte[4];
            byte[] crc = new byte[2];
            byte[] temp = new byte[TAMANHOPACOTE + 2];
            byte[] resposta = new byte[TAMANHOPACOTE];

            temp = new byte[TAMANHOPACOTE + 2];

            addr = BitConverter.GetBytes((UInt32) ((TAMANHOPACOTE*indicePagina)));

            crc = BitConverter.GetBytes(CRC16CCITT.Calcular(addr));

            byte[] array = ArrayLDX2.Concat<Byte>(ArrayLDX2.Concat<Byte>(comando, addr), crc);

            Enviar(array, 0, array.Length);

            // Ler a resposta ACK
            if (ReceberByte() != ACK)
            {
                throw new FalhaComunicacaoException(" Não recebeu a confirmação de recebimento da página: " +
                                                    indicePagina);
            }

            ReceberArray(temp, 0, TAMANHOPACOTE + 2);

            //     base.ReceberArray(crc, 0, 2);
            crc[0] = temp[temp.Length - 2];
            crc[1] = temp[temp.Length - 1];

            Array.Resize(ref temp, temp.Length - 2);

            if (CRC16CCITT.Calcular(temp).Equals(BitConverter.ToUInt16(crc, 0)))
            {
                Array.Copy(temp, 0, resposta, 0, TAMANHOPACOTE);
            }

            return resposta;
        }

        private void EnviarParaControlador(byte[] buffer, int offSet, int tamanho)
        {
            int indice = 0;
            Byte[] dados = new byte[8];

            Encoding.UTF8.GetBytes("//DD").CopyTo(dados, indice);

            indice += 4;

            BitConverter.GetBytes((UInt32) tamanho).CopyTo(dados, indice);

            indice += 4;

            Array.Resize(ref dados, dados.Length + tamanho - offSet);

            buffer.CopyTo(dados, indice);

            tcpClient.Client.Send(dados);
        }

        public string ReceberNumeroSerieWifi()
        {
            /* Comando para ler o Número de Série - "//ii" */
            byte[] comando = new byte[] {47, 47, 105, 105};

            byte[] crc = new byte[2];
            byte[] qtdBytes = new byte[3];
            Byte[] resposta = new Byte[8];



            Enviar(comando, 0, comando.Length);

            ReceberByte();

            ReceberArray(qtdBytes, 0, 3);

            Enviar(ack, 0, ack.Length);

            Byte[] identificacao = new Byte[8];

            ReceberArray(identificacao, 0, 8);

            //for (int i = 0; i < 8; i++)
            //{
            //    identificacao[i] = (byte)ReceberByte();
            //}

            ReceberArray(crc, 0, 2);
            if (BitConverter.ToUInt16(crc, 0).Equals(CRC16CCITT.Calcular(identificacao)))
            {
                String numeroSerie =
                    ConversorNumeroSerie.MascararNumeroSerie(
                        ConversorNumeroSerie.ConverterBytesParaNumeroSerie(identificacao));
                return numeroSerie;
            }

            return String.Empty;
        }

        #endregion Métodos que não são da Interface IDispositivo

        public string ReceberNumeroSeriePainel(int numeroPainel)
        {
            /* Comando para ler o Número de Série - "//ii" */
            byte[] comando = new byte[] {47, 47, 105, 105};

            byte[] crc = new byte[2];
            byte[] qtdBytes = new byte[3];
            Byte[] resposta = new Byte[8];
            Byte[] ack = new byte[1] {42}; //Ack = "*";

            comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes(numeroPainel));

            EnviarParaControlador(comando, 0, comando.Length);

            ReceberByte();

            ReceberArray(qtdBytes, 0, 3);

            EnviarParaControlador(ack, 0, ack.Length);

            Byte[] identificacao = new Byte[8];

            ReceberArray(identificacao, 0, 8);

            //for (int i = 0; i < 8; i++)
            //{
            //    identificacao[i] = (byte)ReceberByte();
            //}

            ReceberArray(crc, 0, 2);
            if (BitConverter.ToUInt16(crc, 0).Equals(CRC16CCITT.Calcular(identificacao)))
            {
                String numeroSerie =
                    ConversorNumeroSerie.MascararNumeroSerie(
                        ConversorNumeroSerie.ConverterBytesParaNumeroSerie(identificacao));
                return numeroSerie;
            }

            return String.Empty;
        }
        


        private byte[] CarregarConteudoArquivo(String nomeArquivo)
        {
            byte[] conteudo = new byte[1] {0};

            FileStream fs = File.OpenRead(nomeArquivo);
            conteudo = new Byte[fs.Length];
            fs.Read(conteudo, 0, conteudo.Length);
            fs.Close();

            return conteudo;
        }

        public void AtualizarFirmwarePaineis()
        {
        }

        #region Comandos Gerais da FRT
        /// <summary>
        /// Método RestaurarProduto
        /// </summary>
        /// <returns> Retorna se o comando foi executado com sucesso. </returns>
        public bool RestaurarProduto()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//RESET");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte) ReceberByte();
            }
            catch
            {
            }
            return (resposta == 0);
        }
        /// <summary>
        /// Método LerIdentificacaoProduto
        /// </summary>
        /// <returns> Retorna a identificação do Produto </returns>
        public String LerIdentificacaoProduto()
        {
            String textoResposta = String.Empty;
            //byte[] resposta = new byte[1];

            //Byte[] crc = new byte[2];
            //byte[] comando = Encoding.ASCII.GetBytes("//RDPRODUTO");

            //for (int retry = 0; retry < 3; retry++)
            //{
            //    try
            //    {
            //        EnviarParaControlador(comando, 0, comando.Length);

            //        for (int i = 0; i < resposta.Length; i++)
            //        {
            //            resposta[i] = (byte)(ReceberByte());
            //        }
            //        for (int i = 0; i < 2; i++)
            //        {
            //            crc[i] = (byte)(ReceberByte());
            //        }
            //        if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
            //        {
            //            Byte[] dados = new byte[resposta[0]];
            //            for (int i = 0; i < resposta[0]; i++)
            //            {
            //                dados[i] = (byte)(ReceberByte());
            //            }
            //            textoResposta = Encoding.ASCII.GetString(dados);
            //            return textoResposta;
            //        }
            //        else
            //        {
            //            if (retry == 2)
            //                throw new NotImplementedException();
            //        }
            //    }
            //    catch
            //    {
            //        if (retry == 2)
            //            throw new NotImplementedException();
            //    }
            //}

            return textoResposta;
        }

        /// <summary>
        /// Método Ler Versão
        /// </summary>
        /// <returns> Byte[0]: Familia </returns>
        /// <returns> Byte[1]: Major </returns>
        /// <returns> Byte[2]: Minor </returns>
        public byte[] LerVersao()
        {
            byte[] resposta = new byte[3];

            //Byte[] crc = new byte[2];
            //byte[] comando = Encoding.ASCII.GetBytes("//RDVERSAO");

            //for (int retry = 0; retry < 3; retry++)
            //{
            //    try
            //    {
            //        EnviarParaControlador(comando, 0, comando.Length);

            //        for (int i = 0; i < resposta.Length; i++)
            //        {
            //            resposta[i] = (byte)(ReceberByte());
            //        }
            //        for (int i = 0; i < 2; i++)
            //        {
            //            crc[i] = (byte)(ReceberByte());
            //        }
            //        if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            if (retry == 2)
            //                throw new NotImplementedException();
            //        }
            //    }
            //    catch
            //    {
            //        if (retry == 2)
            //            throw new NotImplementedException();
            //    }
            //}

            return resposta;
        }
       
        /// <summary>
        /// Método ReceberNumeroSerieControlador - Lê o número de série do dispositivo
        /// </summary>
        /// <returns> Retorna o número de série já com o formato e a máscara. </returns>
        public string ReceberNumeroSerieControlador()
        {
            //byte[] resposta = new byte[8];

            //Byte[] crc = new byte[2];
            //byte[] comando = Encoding.ASCII.GetBytes("//RDNUMSERIE");


            //for (int retry = 0; retry < 3; retry++)
            //{
            //    try
            //    {
            //        EnviarParaControlador(comando, 0, comando.Length);

            //        for (int i = 0; i < resposta.Length; i++)
            //        {
            //            resposta[i] = (byte)(ReceberByte());
            //        }
            //        for (int i = 0; i < 2; i++)
            //        {
            //            crc[i] = (byte)(ReceberByte());
            //        }
            //        if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
            //        {
            //            String numeroSerie =
            //                ConversorNumeroSerie.MascararNumeroSerie(
            //                    ConversorNumeroSerie.ConverterBytesParaNumeroSerie(resposta));
            //            return numeroSerie;
            //        }
            //        else
            //        {
            //            if (retry == 2)
            //                throw new NotImplementedException();
            //        }
            //    }
            //    catch
            //    {
            //        if (retry == 2)
            //            throw new NotImplementedException();
            //    }
            //}
            return String.Empty;
        }        
        /// <summary>
        ///  Método Enviar Data e Hora - Serve para ajustar o relógio do controlador;
        /// </summary>
        /// <param name="dados">Numero de Série no formato de array de bytes de tamanho 8</param>
        /// <returns> Retorna se o comando foi executado com sucesso. </returns>
        public bool EnviarNumeroSerie(Byte[] dados)
        {
            //if (!dados.Length.Equals(8))
            //    throw new NotImplementedException();

            // Comando "//HH" com "//DD" adicionado no começo para que o wifi transfira para o controlador.

            byte[] comando = Encoding.ASCII.GetBytes("//WRNUMSERIE");

            int resposta = -1;

            comando = ArrayLDX2.Concat(comando, dados);

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);
                    resposta = ReceberByte();
                    if (resposta != 0)
                    {
                        if (retry == 2)
                            throw new FalhaComunicacaoException("Falha na atualização da data e da hora.");
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new FalhaComunicacaoException("Falha na atualização da data e da hora.");
                }
            }
            return (resposta == 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte LerStatusDispositivo()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//RDSTATUS");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] { ACK }; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte)ReceberByte();
            }
            catch
            {
            }
            return resposta;
        }

        /// <summary>
        /// Método RestaurarConfiguracaoFabrica
        /// </summary>
        /// <returns> Retorna se o comando foi executado com sucesso. </returns>	
        public bool RestaurarConfiguracaoFabrica()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FACTORY");
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte) ReceberByte();
            }
            catch
            {
            }
            return (resposta == 0);
        }

        #endregion Comandos Gerais da FRT

        #region Comandos da FAT (Sistema de Arquivo)

        public bool FileExists(String caminhoArquivo)
        {
            int resposta = -1;
            try
            {
                resposta = FileOpen(caminhoArquivo, FileOpenMode.Ler);
            }
            catch
            {
            }
            finally
            {
                FileClose();
            }

            return (resposta == 0);
        }

        /// <summary>
        /// //FOPEN	Abre um arquivo no dispositivo
        /// //→ //FOPEN<pathLength : 1 byte><path : n bytes><mode : 1 byte>	
        /// //← <result : 1 byte>
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        /// <param name="modoAbertura"></param>
        /// <returns></returns>
        public int FileOpen(String caminhoArquivo, FileOpenMode modoAbertura)
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FOPEN");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes((byte) (caminhoArquivo.Length)));
                comando = Recursos.ArrayLDX2.Concat(comando, Encoding.ASCII.GetBytes(caminhoArquivo));
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes((byte) modoAbertura));

                EnviarParaControlador(comando, 0, comando.Length);

                resposta = (byte) ReceberByte();
            }
            catch
            {
            }

            return resposta;
        }

        public int FileClose()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FCLOSE");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte) ReceberByte();
            }
            catch
            {
            }
            return resposta;
        }


        ////FREAD	Lê uma determinada quantidade de bytes de um arquivo
        //→ //FREAD<qntBytes : 4 byte>	
        //← <byteRead : 4 byte><conteudo : n bytes>	
        // TODO: FIXME - Ler de 512 em 512 bytes
        public byte[] FileRead(UInt32 quantidadeBytes)
        {
            byte[] resposta = new byte[quantidadeBytes];
            byte[] comando = Encoding.ASCII.GetBytes("//FWRITE");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes(quantidadeBytes));
                EnviarParaControlador(comando, 0, comando.Length);

                for (int i = 0; i < quantidadeBytes; i++)
                {
                    resposta[i] = (byte) ReceberByte();
                }
            }
            catch
            {

            }

            return resposta;
        }

        ////FWRITE	Escreve uma determinada quantidade de bytes em um arquivo
        //→ //FWRITE<qntBytes : 4 byte><conteudo : n bytes>	
        //← <byteWritten : 4 byte>

        // TODO: FIXME - Escrever de 512 em 512 bytes
        public int FileWrite(byte[] conteudo)
        {
            int resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FWRITE");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes(conteudo.Length));
                comando = Recursos.ArrayLDX2.Concat(comando, conteudo);

                EnviarParaControlador(comando, 0, comando.Length);

                resposta = (byte) ReceberByte();
            }
            catch
            {
            }

            return resposta;
        }

        ////FSEEK	Seta  posição do ponteiro do arquivo
        //→ //FSEEK<posicao : 4 byte>	
        //← <result : 1 byte>	
        public int FileSeek(UInt32 posicao)
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FSEEK");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes(posicao));
                EnviarParaControlador(comando, 0, comando.Length);

                resposta = (byte) ReceberByte();
            }
            catch
            {
            }

            return resposta;
        }

        ////FSFORMAT	Formata a memória do dispositivo
        //→ //FSFORMAT	
        //← <result : 1 byte>	
        public int FileSystemFormat()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FSFORMAT");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte) ReceberByte();
            }
            catch
            {
            }
            return resposta;
        }

        ////FRENAME	Renomeia um arquivo ou diretório
        //→ //FRENAME<oldPathLength : 1 byte><oldPath : n bytes><newPathLength : 1 byte><newPath : n bytes>	
        //← <result : 1 byte>	

        public int FileRename(String caminhoArquivoAnterior, String caminhoArquivoNovo)
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FRENAME");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando,
                                                    BitConverter.GetBytes((byte) (caminhoArquivoAnterior.Length)));
                comando = Recursos.ArrayLDX2.Concat(comando, Encoding.ASCII.GetBytes(caminhoArquivoAnterior));
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes((byte) (caminhoArquivoNovo.Length)));
                comando = Recursos.ArrayLDX2.Concat(comando, Encoding.ASCII.GetBytes(caminhoArquivoNovo));

                EnviarParaControlador(comando, 0, comando.Length);

                resposta = (byte) ReceberByte();
            }
            catch
            {
            }

            return resposta;
        }



        //        //FDELETE	Apaga um arquivo ou diretório
        //→ //FDELETE<pathLength : 1 byte><path : n bytes>	
        //← <result : 1 byte>	

        public int FileDelete(String caminhoArquivo)
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FDELETE");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                comando = Recursos.ArrayLDX2.Concat(comando, BitConverter.GetBytes((byte) (caminhoArquivo.Length)));
                comando = Recursos.ArrayLDX2.Concat(comando, Encoding.ASCII.GetBytes(caminhoArquivo));

                EnviarParaControlador(comando, 0, comando.Length);

                resposta = (byte) ReceberByte();
            }
            catch
            {
            }

            return resposta;
        }



        // //FFLUSH	Descarrega o buffer do arquivo na memória
        //→ //FFLUSH	
        //← <result : 1 byte>	

        public int FileFlush()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//FFLUSH");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] {ACK}; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte) ReceberByte();
            }
            catch
            {
            }
            return resposta;
        }

        #endregion Comandos da FAT (Sistema de Arquivo)
       
        public string LerRoteiroSelecionado()
        {
            return Encoding.ASCII.GetString(LerRoteiroSelecionado(ModoLeituraRoteiro.Numero));
        }

        #region Comandos Controlador        

        /// <summary>
        /// Método Ler roteiro selecionado
        /// </summary>
        /// <param name="modo"> modo poderá ser Id, Indice e Numero </param>
        /// <returns> - Para o modo Id : Retorno de 2 bytes </returns>
        /// <returns> - Para o modo Indice : Retorno de 4 bytes </returns>
        /// <returns> - Para o modo Numero : Retorno de 20 bytes </returns>
        public Byte[] LerRoteiroSelecionado(ModoLeituraRoteiro modo)
        {
            Byte[] resposta = new byte[20];
            //Byte[] crc = new byte[2];
            //byte[] comando = Encoding.ASCII.GetBytes("//RDROTEIRO");
            //Array.Resize(ref comando, comando.Length + 1);
            //comando[comando.Length - 1] = (byte) (modo);

            //for (int retry = 0; retry < QTDRETRY; retry++)
            //{
            //    try
            //    {
            //        Flush();
            //        EnviarParaControlador(comando, 0, comando.Length);
            //        switch (modo)
            //        {
            //            case ModoLeituraRoteiro.Id:
            //                resposta = new byte[2];
            //                break;
            //            case ModoLeituraRoteiro.Indice:
            //                resposta = new byte[4];
            //                break;
            //            case ModoLeituraRoteiro.Numero:
            //                resposta = new byte[20];
            //                break;
            //        }

            //        for (int i = 0; i < resposta.Length; i++)
            //        {
            //            resposta[i] = (byte)(ReceberByte());
            //        }
            //        for (int i = 0; i < 2; i++)
            //        {
            //            crc[i] = (byte)(ReceberByte());
            //        }
            //        if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            if (retry == QTDRETRY - 1)
            //                throw new NotImplementedException();
            //        }
            //    }
            //    catch
            //    {
            //        if (retry == QTDRETRY - 1)
            //            throw new NotImplementedException();
            //    }
            //}
            return resposta;
        }

        /// <summary>
        /// Método selecionar roteiro
        /// </summary>
        /// <param name="modo"> modo poderá ser Id, Indice e Numero - Para o modo Id : Retorno de 2 bytes - Para o modo Indice : Retorno de 4 bytes - Para o modo Numero : Retorno de 20 bytes</param>
        /// <param name="dados"> dados a serem enviados </param>
        /// <returns> Se o result == 0, então a função retorna true e o comando enviado com sucesso </returns>
        public bool SelecionarRoteiro(ModoLeituraRoteiro modo, Byte[] dados)
        {
            Byte[] resposta = {0xFF};
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//WRROTEIRO");
            Array.Resize(ref comando, comando.Length + 1);
            comando[comando.Length - 1] = (byte) (modo);

            comando = ArrayLDX2.Concat(comando, dados);

            for (int retry = 0; retry < QTDRETRY; retry++)
            {
                try
                {
                    Flush();
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == QTDRETRY - 1)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == QTDRETRY - 1)
                        throw new NotImplementedException();
                }
            }
            return (resposta[0] == 0);
        }

        /// <summary>
        /// Método Ler mensagem selecionada
        /// </summary>
        /// <param name="modo"> modo poderá ser Id e Indice</param>
        /// <param name="indicePainel"> Indice do Painel a ser selecionada a mensagem. </param>
        /// <returns> - Para o modo Id : Retorno de 2 bytes </returns>
        /// <returns> - Para o modo Indice : Retorno de 4 bytes </returns>
        public Byte[] LerMensagemSelecionada(ModoLeituraRoteiro modo, int indicePainel)
        {
            if (modo == ModoLeituraRoteiro.Numero)
                throw new NotImplementedException();

            Byte[] resposta = new byte[20];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDMENSAGEM");
            Array.Resize(ref comando, comando.Length + 2);
            comando[comando.Length - 1] = (byte) (modo);
            comando[comando.Length - 2] = (byte) (indicePainel);

            for (int retry = 0; retry < QTDRETRY; retry++)
            {
                try
                {
                    Flush();
                    EnviarParaControlador(comando, 0, comando.Length);
                    switch (modo)
                    {
                        case ModoLeituraRoteiro.Id:
                            resposta = new byte[2];
                            break;
                        case ModoLeituraRoteiro.Indice:
                            resposta = new byte[4];
                            break;
                    }

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == QTDRETRY - 1)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == QTDRETRY - 1)
                        throw new NotImplementedException();
                }
            }
            return resposta;
        }

        /// <summary>
        /// Método selecionar mensagem
        /// </summary>
        /// <param name="modo"> modo poderá ser Id e Indice - Para o modo Id : Retorno de 2 bytes - Para o modo Indice : Retorno de 4 bytes </param>
        /// <param name="dados"> dados a serem enviados </param>
        /// <param name="indicePainel"> Indice do Painel a ser selecionada a mensagem. </param>
        /// <returns> Se o result == 0, então a função retorna true e o comando enviado com sucesso </returns>
        public bool SelecionarMensagem(ModoLeituraRoteiro modo, Byte[] dados, int indicePainel)
        {
            if (modo == ModoLeituraRoteiro.Numero)
                throw new NotImplementedException();

            if (dados.Length > 4) // Se for maior que quatro, existe algum problema pois o length deve ser 2 ou 4;
                throw new NotImplementedException();

            Byte[] resposta = {0xFF};
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//WRMENSAGEM");
            Array.Resize(ref comando, comando.Length + 2);
            comando[comando.Length - 1] = (byte) (modo);
            comando[comando.Length - 2] = (byte) (indicePainel);

            comando = ArrayLDX2.Concat(comando, dados);

            for (int retry = 0; retry < QTDRETRY; retry++)
            {
                try
                {
                    Flush();
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte)(ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte)(ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == QTDRETRY - 1)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == QTDRETRY - 1)
                        throw new NotImplementedException();
                }
            }
            return (resposta[0] == 0);
        }

        /// <summary>
        /// Método Ler painel selecionado
        /// </summary>        
        /// <returns> Retorna o Indice do Painel Selecionado </returns>
        public int LerPainelSelecionado()
        {
            Byte[] resposta = new byte[1];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDPAINEL");

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return resposta[0];
        }

        /// <summary>
        /// Método selecionar painel
        /// </summary>
        /// <param name="indicePainel"> Indice do painel que será escolhido </param>
        /// <returns> Se o result == 0, então a função retorna true e o comando enviado com sucesso </returns>
        public bool SelecionarPainel(int indicePainel)
        {
            Byte[] resposta = {0xFF};
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//WRPAINEL");
            Array.Resize(ref comando, comando.Length + 1);
            comando[comando.Length - 1] = (byte) (indicePainel);

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return (resposta[0] == 0);
        }

        /// <summary>
        /// Método Ler sentido selecionado
        /// </summary>        
        /// <returns> Sentido escolhido 0 = Ida e 1 = Volta </returns>
        public int LerSentidoSelecionado()
        {
            Byte[] resposta = new byte[1];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDSENTIDO");

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return resposta[0];
        }

        /// <summary>
        /// Método selecionar Sentido
        /// </summary>
        /// <param name="sentido"> Sentido que será escolhido 0 = Ida e 1 = Volta </param>
        /// <returns> Se o result == 0, então a função retorna true e o comando enviado com sucesso </returns>
        public bool SelecionarSentido(int sentido)
        {
            Byte[] resposta = {0xFF};
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//WRSENTIDO");
            Array.Resize(ref comando, comando.Length + 1);
            comando[comando.Length - 1] = (byte) (sentido);

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return (resposta[0] == 0);
        }

        /// <summary>
        /// Método Ler Hora de Saída selecionada
        /// </summary>
        /// <returns> Retorna a Hora de Saída no formato "HH:MM" </returns>        
        public string LerHoraSaidaSelecionada()
        {
            Byte[] resposta = new byte[2];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDHORASAIDA");

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return resposta[0] + ":" + resposta[1];
        }

        /// <summary>
        /// Método selecionar Hora de Saída
        /// </summary>
        /// <param name="hora"> Hora da Saída </param>
        /// <param name="minuto"> Minuto da Saída </param>
        /// <returns> Se o result == 0, então a função retorna true e o comando enviado com sucesso </returns>
        public bool SelecionarHoraSaida(int hora, int minuto)
        {
            Byte[] resposta = {0xFF};
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//WRHORASAIDA");
            Array.Resize(ref comando, comando.Length + 2);
            comando[comando.Length - 1] = (byte) (minuto);
            comando[comando.Length - 2] = (byte) (hora);

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return (resposta[0] == 0);
        }

        /// <summary>
        /// Método Ler quantidade de Painéis
        /// </summary>
        /// <returns> Retorna a quantidade de Painéis </returns>
        public int LerQtdPaineis()
        {
            Byte[] resposta = new byte[4];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDQNTPAINEIS");

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return BitConverter.ToInt32(resposta, 0);
        }

        /// <summary>
        /// Método Ler quantidade de Roteiros
        /// </summary>        
        /// <returns> Quantidade de Roteiros </returns>
        public int LerQtdRoteiros()
        {
            Byte[] resposta = new byte[2];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDQNTROTS");

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return BitConverter.ToInt32(resposta, 0);
        }

        /// <summary>
        /// Método Ler quantidade de mensagens
        /// </summary>
        /// <param name="indicePainel"> índice do painel do qual se deseja saber a quantidade de mensagens </param>
        /// <returns> Quantidade de Mensagens </returns>        
        public int LerQtdMensagens( /*int indicePainel*/)
        {
            Byte[] resposta = new byte[2];
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDQNTMSGS");

            // TODO: FIXME Definir se a quantidade de mensagens será por painel ou não.
            //Array.Resize(ref comando, comando.Length + 1);
            //comando[comando.Length - 1] = (byte)(indicePainel);

            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            return BitConverter.ToInt32(resposta, 0);
        }

        /// <summary>
        /// Método Ler dimensoes do Painel
        /// </summary>
        /// <param name="indicePainel"> índice do painel do qual se deseja saber as dimensões. </param>
        /// <returns> int[0]: Altura </returns>
        /// <returns> int[1]: Largura </returns>
        public int[] LerDimensoesPainel(int indicePainel)
        {
            byte[] resposta = new byte[8];

            int[] dimensao = new int[2]; // Altura x Largura 
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDDIMENSOES");
            Array.Resize(ref comando, comando.Length + 1);
            comando[comando.Length - 1] = (byte) (indicePainel);


            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte) (ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte) (ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }
            dimensao[0] = BitConverter.ToInt32(resposta, 0);
            dimensao[1] = BitConverter.ToInt32(resposta, 4);
            return dimensao;
        }

        /// <summary>
        /// Método Sincronizar
        /// </summary>
        /// <returns>Retorna se o comando foi enviado. </returns>
        public bool Sincronizar()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//SYNC");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] { ACK }; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte)ReceberByte();
            }
            catch
            {
            }
            return (resposta == 0);
        }
        
        /// <summary>
        /// Método Ler Data Hora
        /// </summary>
        /// <returns> Retorna a data hora do Controlador. </returns>
        public DateTime LerDataHora()
        {
            byte[] resposta = new byte[8];

            int[] dimensao = new int[2]; // Altura x Largura 
            Byte[] crc = new byte[2];
            byte[] comando = Encoding.ASCII.GetBytes("//RDDATAHORA");


            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);

                    for (int i = 0; i < resposta.Length; i++)
                    {
                        resposta[i] = (byte)(ReceberByte());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        crc[i] = (byte)(ReceberByte());
                    }
                    if (CRC16CCITT.Calcular(resposta).Equals(BitConverter.ToUInt16(crc, 0)))
                    {
                        break;
                    }
                    else
                    {
                        if (retry == 2)
                            throw new NotImplementedException();
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new NotImplementedException();
                }
            }

            DateTime dt = new DateTime(BitConverter.ToInt16(resposta, 6), resposta[4], resposta[3], resposta[2], resposta[1], resposta[0]);
            return dt;

        }

        /// <summary>
        /// Método Enviar Data e Hora - Serve para ajustar o relógio do controlador;
        /// </summary>
        public void EnviarDataHora()
        {
            byte[] dados = new byte[8];
            // Comando "//HH" com "//DD" adicionado no começo para que o wifi transfira para o controlador.

            byte[] comando = Encoding.ASCII.GetBytes("//WRDATAHORA");
            int resposta = -1;

            DateTime dt = DateTime.Now;

            // Segundos
            dados[0] = ((byte)(dt.Second));
            // Minutos
            dados[1] = ((byte)(dt.Minute));
            // Hora
            dados[2] = ((byte)(dt.Hour));
            // Dia 
            dados[3] = (byte)(dt.Day);
            // Mês
            dados[4] = (byte)(dt.Month);
            // Dia da Semana
            dados[5] = ((byte)(dt.DayOfWeek));
            // Ano
            dados[6] = (byte)(dt.Year);
            dados[7] = (byte)(dt.Year >> 8);

            ushort crc16 = CRC16CCITT.Calcular(dados);
            Array.Resize(ref dados, 10);
            dados[8] = (byte)(crc16 / 256);
            dados[9] = (byte)(crc16 % 256);
            comando = ArrayLDX2.Concat(comando, dados);


            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnviarParaControlador(comando, 0, comando.Length);
                    resposta = ReceberByte();
                    if (resposta != 0)
                    {
                        if (retry == 2)
                            throw new FalhaComunicacaoException("Falha na atualização da data e da hora.");
                    }
                }
                catch
                {
                    if (retry == 2)
                        throw new FalhaComunicacaoException("Falha na atualização da data e da hora.");
                }
            }
        }
        /// <summary>
        /// Método ClearCache - Apenas usado para 
        /// </summary>
        /// <returns> Retorna o se o comando foi executado. </returns>
        public bool ClearCache()
        {
            byte resposta = 0;
            byte[] comando = Encoding.ASCII.GetBytes("//CLEARCACHE");
            byte[] crc = new byte[2];
            Byte[] ack = new byte[1] { ACK }; //Ack = "*";
            try
            {
                EnviarParaControlador(comando, 0, comando.Length);
                resposta = (byte)ReceberByte();
            }
            catch
            {
            }
            return (resposta == 0);
        }

        #endregion Comandos Controlador                     
    }
}
