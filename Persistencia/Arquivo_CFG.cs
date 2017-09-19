/* Versao 1 
        - Adição:
            public Byte alternanciaSelecionada;
            public UInt16 mensagem_selecionada;
            public Util.Util.OpcoesApresentacao somenteHora;
            public Util.Util.OpcoesApresentacao dataHora;
            public Util.Util.OpcoesApresentacao horaSaida;
            public Util.Util.OpcoesApresentacao temperatura;
            public Util.Util.OpcoesApresentacao tarifa;
            public Util.Util.OpcoesApresentacao horaTemperatura;
            public Util.Util.OpcoesApresentacao velocidade;
  
  Versão 2
        - Adição:
            public Util.Util.OpcoesApresentacao dataHoraTemp;
            public UInt16 mensagem_Secselecionada;
            public fixed Byte reservado[2];
 Versão 3
        - Adição:
            public Byte BrilhoMax;
            public Byte BrilhoMin;
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_CFG
    {
        //código antigo
        //public unsafe struct FormatoPainelCfg
        //{
        //    public Byte versao;
        //    public Byte alternanciaSelecionada;
        //    public UInt16 mensagem_selecionada; //4
        //    public UInt32 altura; //8
        //    public UInt32 largura; //12
        //    public fixed byte fontePath[50]; //62
        //    public UInt16 crc;
        //    public UInt32 offsetBomDia;
        //    public UInt32 offsetBoaTarde;
        //    public UInt32 offsetBoaNoite;
        //    public byte* videosSaudacao;
        //}

        //código novo
        public unsafe struct FormatoPainelCfg
        {
            public Byte versao;
            public Byte alternanciaSelecionada;
            public UInt16 mensagem_selecionada; //4            
            public UInt32 altura; //8
            public UInt32 largura; //12
            public fixed byte fontePath[50]; //62
            public UInt16 crc;
            public UInt32 offsetBomDia;
            public UInt32 offsetBoaTarde;
            public UInt32 offsetBoaNoite;
            public UInt32 offsetTextoSaida;
            public UInt32 offsetSimboloTarifa;
            public UInt32 offsetUnidadeTemperatura;
            public UInt32 offsetUnidadeVelocidade;
            public UInt32 offsetLabelSegunda;
            public UInt32 offsetLabelTerca;
            public UInt32 offsetLabelQuarta;
            public UInt32 offsetLabelQuinta;
            public UInt32 offsetLabelSexta;
            public UInt32 offsetLabelSabado;
            public UInt32 offsetLabelDomingo;
            public UInt32 offsetExtensaoHoraAM;
            public UInt32 offsetExtensaoHoraPM;
            public Util.Util.OpcoesApresentacao somenteHora;
            public Util.Util.OpcoesApresentacao dataHora;
            public Util.Util.OpcoesApresentacao horaSaida;
            public Util.Util.OpcoesApresentacao temperatura;
            public Util.Util.OpcoesApresentacao tarifa;
            public Util.Util.OpcoesApresentacao horaTemperatura;
            public Util.Util.OpcoesApresentacao velocidade;
            public Util.Util.OpcoesApresentacao dataHoraTemp;
            public UInt16 mensagem_Secselecionada;
            public Byte BrilhoMax;
            public Byte BrilhoMin;
            public byte* videosMensagensEspeciais;
        }

 //       typedef struct {
             //U8 versao;
             //U8 alternanciaSelecionada;
             //U16 mensagem;
             //U32 altura;
             //U32 largura;
             //char fontePath[50];
             //U16 crc;
             //U32 offsetBomDia;
             //U32 offsetBoaTarde;
             //U32 offsetBoaNoite;
             //U32 offsetTextoSaida;
             //U32 offsetSimboloTarifa;
             //U32 offsetUnidadeTemperatura;
             //U32 offsetUnidadeVelocidade;
             //U32 offsetLabelSegunda;
             //U32 offsetLabelTerca;
             //U32 offsetLabelQuarta;
             //U32 offsetLabelQuinta;
             //U32 offsetLabelSexta;
             //U32 offsetLabelSabado;
             //U32 offsetLabelDomingo;
             //U32 offsetExtensaoHoraAM;
             //U32 offsetExtensaoHoraPM;
             //OpcoesApresentacao somenteHora;
             //OpcoesApresentacao dataHora;
             //OpcoesApresentacao horaSaida;
             //OpcoesApresentacao temperatura;
             //OpcoesApresentacao tarifa;
             //OpcoesApresentacao horaTemperatura;
             //OpcoesApresentacao velocidade;
             //U8* videos;
         //} FormatoPainelCfg;


        //public Byte versao { get { return 3; } }
        private Byte versao = 3;

        public Byte alternanciaSelecionada { get; set; }
        public UInt16 mensagemSelecionada { get; set; }
        public UInt16 mensagemSecSelecionada { get; set; }
        public UInt32 altura { get; set; }
        public UInt32 largura { get; set; }
        public string fontePath { get; set; }
        public UInt16 crc { get; set; }

        public Util.Util.OpcoesApresentacao somenteHora { get; set; }
        public Util.Util.OpcoesApresentacao dataHora { get; set; }
        public Util.Util.OpcoesApresentacao horaSaida { get; set; }
        public Util.Util.OpcoesApresentacao temperatura { get; set; }
        public Util.Util.OpcoesApresentacao tarifa { get; set; }
        public Util.Util.OpcoesApresentacao horaTemperatura { get; set; }
        public Util.Util.OpcoesApresentacao velocidade { get; set; }
        public Util.Util.OpcoesApresentacao dataHoraTemp { get; set; }


        public Byte BrilhoMax { get; set; }
        public Byte BrilhoMin { get; set; }

        //public List<Videos.Video> VideosSaudacao = new List<Video>();
        public List<Videos.Video> videosMensagensEspeciais = new List<Video>();


        public Arquivo_CFG()
        {
            //versao = 3;
            alternanciaSelecionada = 0;
            altura = 8;
            largura = 80;
            fontePath = string.Empty;
            crc = 0;
            mensagemSelecionada = 0;

            //adicionado na versao 2 do painel CFG
            mensagemSecSelecionada = 0;

            this.somenteHora = new Util.Util.OpcoesApresentacao();
            this.dataHora = new Util.Util.OpcoesApresentacao();
            this.horaSaida = new Util.Util.OpcoesApresentacao();
            this.temperatura = new Util.Util.OpcoesApresentacao();
            this.tarifa = new Util.Util.OpcoesApresentacao();
            this.horaTemperatura = new Util.Util.OpcoesApresentacao();
            this.dataHoraTemp = new Util.Util.OpcoesApresentacao();

            this.BrilhoMax = 100;
            this.BrilhoMin = 10;

        }

        public Arquivo_CFG(Arquivo_CFG oldValue)
        {
            this.versao = oldValue.versao;
            this.alternanciaSelecionada = oldValue.alternanciaSelecionada;
            this.mensagemSelecionada = oldValue.mensagemSelecionada;
            this.altura = oldValue.altura;
            this.largura = oldValue.largura;
            this.fontePath = oldValue.fontePath;
            this.crc = oldValue.crc;

            //this.VideosSaudacao = new List<Video>();
            //this.VideosSaudacao = UtilPersistencia.GravaVideo(this.VideosSaudacao, oldValue.VideosSaudacao);
            this.videosMensagensEspeciais = new List<Video>();
            this.videosMensagensEspeciais = UtilPersistencia.GravaVideo(this.videosMensagensEspeciais, oldValue.videosMensagensEspeciais);

            //Colocando os parametrosFixos para o painel.cfg
            this.somenteHora = new Util.Util.OpcoesApresentacao();
            this.dataHora = new Util.Util.OpcoesApresentacao();
            this.horaSaida = new Util.Util.OpcoesApresentacao();
            this.temperatura = new Util.Util.OpcoesApresentacao();
            this.tarifa = new Util.Util.OpcoesApresentacao();
            this.horaTemperatura = new Util.Util.OpcoesApresentacao();
            this.velocidade = new Util.Util.OpcoesApresentacao();
            this.dataHoraTemp = new Util.Util.OpcoesApresentacao();

            this.somenteHora = oldValue.somenteHora;
            this.dataHora = oldValue.dataHora;
            this.horaSaida = oldValue.horaSaida;
            this.temperatura = oldValue.temperatura;
            this.tarifa = oldValue.tarifa;
            this.horaTemperatura = oldValue.horaTemperatura;
            this.velocidade = oldValue.velocidade;
            this.dataHoraTemp = oldValue.dataHoraTemp;


            this.BrilhoMax = oldValue.BrilhoMax;
            this.BrilhoMin = oldValue.BrilhoMin;

            //adicionado na versao 2 do painel CFG
            this.mensagemSecSelecionada = oldValue.mensagemSecSelecionada;

        }

        
        // public void CriarCFG

        public void CriarConfiguracaoDefault()
        {
            //versao = 3;
            alternanciaSelecionada = 0;
            altura = 8;
            largura = 80;
            fontePath = @"fontes/fonte8.fnt"; //(Defines.OrigemDiretorioFontes.PadRight(50, '\0').ToCharArray());
            crc = 0;
            //this.VideosSaudacao.Add(new VideoV01());
            //this.VideosSaudacao.Add(new VideoV01());
            //this.VideosSaudacao.Add(new VideoV01());
            this.videosMensagensEspeciais.Add(new VideoV01());
            this.videosMensagensEspeciais.Add(new VideoV01());
            this.videosMensagensEspeciais.Add(new VideoV01());
            mensagemSelecionada = 0;
            //adicionado na versao 2 do painel CFG
            mensagemSecSelecionada = 0;

            this.BrilhoMax = 100;
            this.BrilhoMin = 10;

            AtualizarCRC();
        }

        public void Salvar(string arquivoNome) //, object controlador)
        {           
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();

        }

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
                    FormatoPainelCfg* config = (FormatoPainelCfg*)pSrc;

                    this.versao = config->versao;

                    this.alternanciaSelecionada = config->alternanciaSelecionada;

                    this.altura = config->altura;
                    this.largura = config->largura;

                    for (int i = 0; i < 50; i++)
                    {
                        this.fontePath = this.fontePath + (char)config->fontePath[i];
                    }

                    this.crc = config->crc;

                    this.mensagemSelecionada = config->mensagem_selecionada;
                    //adicionado na versao 2 do painel CFG
                    this.mensagemSecSelecionada = config->mensagem_Secselecionada;
                    

                    //adição dos parametrosfixos no painel.cfg
                    this.somenteHora = config->somenteHora;
                    this.dataHora = config->dataHora;
                    this.horaSaida = config->horaSaida;
                    this.temperatura = config->temperatura;
                    this.tarifa = config->tarifa;
                    this.horaTemperatura = config->horaTemperatura;
                    this.velocidade = config->velocidade;
                    this.dataHoraTemp = config->dataHoraTemp;
                    this.BrilhoMax = config->BrilhoMax;
                    this.BrilhoMin = config->BrilhoMin;


                    this.videosMensagensEspeciais.Clear();
                    int indice = (int)(sizeof(FormatoPainelCfg) - 4);

                    for (int i = 0; i < 16; i++)
                    {
                        int indiceInicial = indice;
                        // Pegar o tamanho do Arquivo
                        uint tamanhoArquivo = BitConverter.ToUInt32(dados, indice);
                        byte[] dadosVideo = new byte[tamanhoArquivo];
                        // Atualiza o indice com o tamanho do arquivo
                        indice += sizeof(uint);
                        // Pular a versão
                        indice++;

                        Array.Copy(dados, indiceInicial, dadosVideo, 0, tamanhoArquivo);
                        
                        // Pegar o formato do Arquivo Ex.: V02
                        if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("V01"))
                        {
                            // Instancia o video a partir do formato
                            VideoV01 video = new VideoV01();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            videosMensagensEspeciais.Add(video);
                            indice += dadosVideo.Length;

                        }
                        else if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("V02"))
                        {
                            // Instancia o video a partir do formato
                            VideoV02 video = new VideoV02();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            videosMensagensEspeciais.Add(video);
                            indice += dadosVideo.Length;

                        }

                        //barata, adicionei isso aqui. o índice tava cagado.
                        indice = indice - sizeof(uint);
                        indice = indice - 1;
                    }

                    
                }
            }
        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoPainelCfg* parametros = (FormatoPainelCfg*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }

        private byte[] toByteArray()
        {
            List<byte> dados = new List<byte>();
            List<UInt32> listaOffset = new List<uint>();
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoPainelCfg)];

                FormatoPainelCfg config = new FormatoPainelCfg();
                config.versao = (Byte)this.versao;

                //if (this.VideosSaudacao.Count > 0)
                //{
                    listaOffset.Add((uint) (sizeof (FormatoPainelCfg) - 4));
                    foreach (IVideo video in videosMensagensEspeciais)
                    {
                        if (video is VideoV01)
                        {
                            dados.AddRange((video as VideoV01).ToBytes());
                        }
                        else if (video is VideoV02)
                        {
                            dados.AddRange((video as VideoV02).ToBytes());
                        }
                        listaOffset.Add((uint) (sizeof (FormatoPainelCfg) - 4 + dados.Count));
                    }
                //}
            }
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoPainelCfg) - 4 + dados.Count];

                fixed (byte* pSrc = resultado)
                {
                    FormatoPainelCfg* config = (FormatoPainelCfg*)pSrc;

                    config->versao = (Byte)this.versao;
                    config->alternanciaSelecionada = (Byte)this.alternanciaSelecionada;
                    config->altura = this.altura;
                    config->largura = this.largura;

                    fontePath.PadRight(50, '\0');

                    for (int i = 0; i < fontePath.Length; i++)
                    {
                        config->fontePath[i] = (byte)this.fontePath[i];
                    }

                    config->crc = this.crc;

                    config->mensagem_selecionada = this.mensagemSelecionada;
                    //adicionado na versao 2 do painel CFG
                    config->mensagem_Secselecionada = this.mensagemSecSelecionada;
                    

                    config->offsetBomDia = listaOffset[0];
                    config->offsetBoaTarde = listaOffset[1];
                    config->offsetBoaNoite = listaOffset[2];

                    //adição dos campos
                    config->offsetTextoSaida = listaOffset[3];
                    config->offsetSimboloTarifa = listaOffset[4];
                    config->offsetUnidadeTemperatura = listaOffset[5];
                    config->offsetUnidadeVelocidade = listaOffset[6];
                    config->offsetLabelSegunda = listaOffset[7];
                    config->offsetLabelTerca = listaOffset[8];
                    config->offsetLabelQuarta = listaOffset[9];
                    config->offsetLabelQuinta = listaOffset[10];
                    config->offsetLabelSexta = listaOffset[11];
                    config->offsetLabelSabado = listaOffset[12];
                    config->offsetLabelDomingo = listaOffset[13];
                    config->offsetExtensaoHoraAM = listaOffset[14];
                    config->offsetExtensaoHoraPM = listaOffset[15];
                    
                    //adição dos parametrosfixos em painel.cfg
                    config->somenteHora = this.somenteHora;
                    config->dataHora = this.dataHora;
                    config->horaSaida = this.horaSaida;
                    config->temperatura = this.temperatura;
                    config->tarifa = this.tarifa;
                    config->horaTemperatura = this.horaTemperatura;
                    config->velocidade = this.velocidade;
                    config->dataHoraTemp = this.dataHoraTemp;
                    config->BrilhoMax = this.BrilhoMax;
                    config->BrilhoMin = this.BrilhoMin;


                    byte* temp = (byte*)&config->videosMensagensEspeciais;

                    for (int i = 0; i < dados.Count; i++)
                    {
                        temp[i] = dados[i];
                    }

                    //byte* temp2 = (byte*)&config->mensagem_Secselecionada;
                    //for (int i = 0; i < sizeof(UInt16); i++)
                    //{
                    //    temp2[i] = BitConverter.GetBytes(this.mensagemSecSelecionada)[i];
                    //}
                    //adicionado na versao 2 do painel CFG
                    //config->mensagem_Secselecionada = this.mensagemSecSelecionada;

                }

                return resultado;
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

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoPainelCfg* parametros = (FormatoPainelCfg*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&parametros->crc - (int)pSrc);
                Array.Copy(dados, ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&parametros->crc - (int)pSrc,
                           dados.Length - ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarCRC(byte[] dados)
        {            
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoPainelCfg* config = (FormatoPainelCfg*)pSrc;

                    return (config->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoPainelCfg));
            }

            return resposta;
        }
    }
}




