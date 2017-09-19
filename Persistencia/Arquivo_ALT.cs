using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using System.Resources;
using System.Reflection;
using Globalization;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_ALT : IArquivo
    {
        ResourceManager rm;

        public unsafe struct FormatoAlternancia
        {
            public byte versao;
            public byte QtdAlternancias;
            public fixed byte reservado [60];
            public UInt16 crc;

        }
        
        private Byte versao = 3;

        public Byte QuantidadeAlternancias
        {
            get { return (Byte) (listaAlternancias.Count); }
        }

        public Byte[] reservado = new byte[60];
        public UInt16 crc;
        public List<ItemAlternancia> listaAlternancias = new List<ItemAlternancia>();

        public Arquivo_ALT()
        {
            crc = 0;
            listaAlternancias = new List<ItemAlternancia>();
            //rm = carregaIdioma();
        }

        public Arquivo_ALT(ResourceManager rm)
        {
            crc = 0;
            listaAlternancias = new List<ItemAlternancia>();
            this.rm = rm;
        }

        public Arquivo_ALT(Arquivo_ALT oldValue)
        {
            this.versao = oldValue.versao;
            oldValue.reservado.CopyTo(this.reservado, 0);
            this.crc = oldValue.crc;


            foreach (ItemAlternancia ia in oldValue.listaAlternancias)
            {
                this.listaAlternancias.Add(new ItemAlternancia(ia));
            }
        }

        public int VerificarVersaoUsuario(string arquivoNome)
        {
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[1];
            fs.Read(dados, 0, 1);
            fs.Close();

            return dados[0];
        }

        public bool RestaurarVersao(string arquivoNome)
        {
            bool restaurou = false;
            int versaoArquivoUsuario = VerificarVersaoUsuario(arquivoNome);

            //Comparando a versão do arquivo com a versão do arquivo do usuário
            if (this.versao == versaoArquivoUsuario)
                return restaurou;

            //Se a versão do arquivo for anterior a versão inicial, no caso 2. Criar um arquivo da versão mais nova do zero.
            if (versaoArquivoUsuario < 3)
            { 
                CriarAlternanciasDefault();
                restaurou = true;
            }

            return restaurou;
            //Parte do código onde irá restaurar os arquivos das versões anteriores, foi testado e comentado porque não há versões novas de arquivo_alt

            //// Recuperando o ultimo LPK
            //Arquivo_LPK_V02 LPKRestaurado = new Arquivo_LPK_V02(this.rm);
            //LPKRestaurado.nome = arquivoNome;
            //LPKRestaurado.RestaurarVersao();

            ////Adicionando as alterações da versão 3 ao lpk v2 recuperado

            ////copiando as informações do arquivo do usuário para a nova versão
            //this.rm = LPKRestaurado.rm;
            //this.nome = LPKRestaurado.nome;
            //this.texto = LPKRestaurado.texto;

            ////adicionando as novas informações
            //this.texto.Add(rm.GetString("TEXTO_104_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));


        }

        public override void Salvar(string arquivoNome, object controlador)
        {            
            Byte[] dados = (controlador as Byte[]);
            /* Criar arquivo */
            FileStream fs = File.Create(arquivoNome);

            /* Alimentar o arquivo*/
            fs.Write(dados, 0, dados.Length);

            /* Fechar arquivo */
            fs.Close();

            AtualizarCRC();
        }

        public void Salvar(string arquivoNome)
        {
            FileStream fs;
            
            const int posCRC1 = 62;
            const int posCRC2 = 63;

            this.AtualizarCRC();
            byte[] arquivo_alt = this.AlternanciasToByteArray();
            
            ushort crc16 = CalcularCRC(arquivo_alt);

            arquivo_alt[posCRC2] = (byte)(crc16 >> 8);
            arquivo_alt[posCRC1] = (byte)(crc16);
                        


            fs = File.Create(arquivoNome);                     

            /* Alimentar o arquivo*/
            fs.Write(arquivo_alt, 0, arquivo_alt.Length);

            /* Fechar arquivo */
            fs.Close();

        }

        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoAlternancia)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoAlternancia* fAlternancia = (FormatoAlternancia*)pSrc;

                    fAlternancia->versao = this.versao;
                    fAlternancia->QtdAlternancias = this.QuantidadeAlternancias;

                    for (int i = 0; i < 60; i++)
                    {
                        fAlternancia->reservado[i] = this.reservado[i];
                    }

                    fAlternancia->crc = this.crc;
                    
                    return resultado;
                }
            }
        }

        /// <summary>
        /// Gera o objeto de Arquivo_ALT em array de bytes, incluindo itens de alternancias.
        /// </summary>
        /// <returns>Array de bytes contendo o conteudo do Arquivo_ALT.</returns>
        private byte[] AlternanciasToByteArray()
        {
            byte[] dados = this.toByteArray();
            List<byte> itens_alternancias = new List<byte>();

            foreach (ItemAlternancia ia in this.listaAlternancias)
            {
                itens_alternancias.AddRange(ia.toByteArray());
            }

            byte[] dadosAlternacia = new byte[itens_alternancias.Count];

            dadosAlternacia = itens_alternancias.ToArray();

            Byte[] alt = new byte[dados.Length + dadosAlternacia.Length];

            dados.CopyTo(alt,0);
            dadosAlternacia.ToArray().CopyTo(alt, dados.Length);
            
            return alt;
        }

        public override object Abrir(string arquivoNome)
        {
            byte[] fileBytes = null;

            this.ArquivoNome = arquivoNome;

            FormatoAlternancia fa = new FormatoAlternancia();         

            unsafe
            {
                //fileBytes = new byte[sizeof (FormatoAlternancia)];

                if (!VerificarIntegridade(arquivoNome))
                    throw new Exception(rm.GetString("ARQUIVO_ALT_ERRO_ARQUIVO_CORROMPIDO")); //"Arquivo Corrompido!!!");
                else
                {

                    FileStream fs = File.OpenRead(arquivoNome);
                    fileBytes = new byte[(int)fs.Length];
                    fs.Read(fileBytes, 0, fileBytes.Length);
                    fs.Close();   

                    fa = ArquivoALTAssembly(fileBytes);

                    this.versao = fa.versao;
                    for (int i = 0; i < 60; i++)
                    {
                        this.reservado[i] = fa.reservado[i];
                    }

                    this.crc = fa.crc;
                }
            }

            return fa;
        }

        private FormatoAlternancia ArquivoALTAssembly(Byte[] file)
        {
            FormatoAlternancia fa = new FormatoAlternancia();

            if (file != null)
            {
                fa.versao = file[0];
                fa.QtdAlternancias = file[1];

                unsafe
                {
                    for (int i = 0; i < 60; i++)
                    {
                        fa.reservado[i] = file[i + 2];
                    }
                }

                fa.crc = (UInt16)(file[62] << 8);
                fa.crc = (UInt16)(fa.crc + ((byte) (file[63])));

                //montagem dos itens de alternancia.
                for(int i = 0; i < fa.QtdAlternancias; i++)
                {
                    //1 7 32
                    ItemAlternancia item = new ItemAlternancia();

                    byte[] dados = new byte[40];
                    Array.Copy(file,64+(i*40),dados,0,40);
                    item.fromByteArray(dados);
                    //item.qntExibicoes = ((byte)file[i + 64]); //no setter;
                    ////qtd 40 bytes
                    //for(int exibicoes = 0; exibicoes < 7; exibicoes++)
                    //{
                    //    item.Exibicoes.Add((TipoExibicao) file[exibicoes + 65]);
                    //}

                    //Byte[] nomeTemp = new byte[32];
                    //for (int nome = 0; nome < 32; nome++)
                    //     nomeTemp[nome] = file[nome + 72];

                    //item.NomeAlternancia = System.Text.Encoding.Default.GetString(nomeTemp);

                    this.listaAlternancias.Add(item);
                }

            }
            else
            {
                throw new Exception(rm.GetString("ARQUIVO_ALT_ERRO_NULO_ARQUIVO")); //"Arquivo de Alternância nulo em ArquivoALTAssembly - Arquivo_ALT.cs");
            }
            
            return fa;
        }

        public void AdicionarItemAlternancia(List<TipoExibicao> tipos, String linhaSuperior, String linhaInferior)
        {
            ItemAlternancia ia = new ItemAlternancia();

            if (linhaSuperior.Length > 16)
                throw new Exception(rm.GetString("ARQUIVO_ALT_ERRO_TAMANHO_LINHA_SUPERIOR")); //"O número de caracteres excede na linha superior!");

            if (linhaInferior.Length > 16)
                throw new Exception(rm.GetString("ARQUIVO_ALT_ERRO_TAMANHO_LINHA_INFERIOR")); //"O número de caracteres excede na linha inferior!");

            // Completar a string com espaços até completar 16 caracteres.
            linhaSuperior = linhaSuperior.PadRight(16, ' ');
            linhaInferior = linhaInferior.PadRight(16, ' ');
            
            ia.Exibicoes.AddRange(tipos.ToArray());
            ia.NomeAlternancia = linhaSuperior + linhaInferior;
            this.listaAlternancias.Add(ia);

            //AtualizarCRC();
        }

        public void RemoverItemAlternancia(ItemAlternancia item)
        {
            listaAlternancias.Remove(item);
            AtualizarCRC();
        }

        public void RemoverItemAlternancia(int indice)
        {
            listaAlternancias.RemoveAt(indice);
            AtualizarCRC();
        }

        public void AtualizarCRC()
        {
            const int posCRC1 = 62;
            const int posCRC2 = 63;

            Byte[] dados = this.AlternanciasToByteArray();

            List<byte> dadosValidos = new List<byte>();

            for (int i = 0; i < dados.Length; i++)
            {
                if ((i == posCRC1) || (i == posCRC2))
                {
                    continue;
                }
                else
                {
                    dadosValidos.Add(dados[i]);
                }

            }
            ushort crc16 = CalcularCRC(dadosValidos.ToArray());
            
            dados[posCRC1] = (byte)(crc16 >> 8);
            dados[posCRC2] = (byte)(crc16);
            
            

            this.crc = crc16;
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            bool resposta = false;
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            resposta = VerificarCRC(dados);
            if (!resposta)
            {
                fs.Close();
                throw new CRCFileException("CRC file error.");
            }
            resposta = VerificarTamanhoArquivo(fs);
            if (!resposta)
            {
                fs.Close();
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return resposta;
        }

        public void CriarAlternanciasDefault()
        {
            String linhaSuperior = rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO").PadRight(16, ' ');//"ALTERNA ROTEIRO ";
                                             
            List<TipoExibicao> tipos = new List<TipoExibicao>();

            //this.ArquivoNome = Defines.OrigemArquivoAlternancias;
            /* Preencher as propriedades */
            //this.versao = 3;

            /* Apresentar somente o roteiro */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_ROTEIRO").PadRight(16, ' ')); //"SO ROTEIRO      ");
            
            /* Apresentar somente número do roteiro */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_NUMERO);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_NUM_ROTEIRO").PadRight(16, ' ')); //"SO NUM ROTEIRO  ");

            /* Apresentar somente mensagem */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_MENSAGEM").PadRight(16, ' ')); // "SO MENSAGEM     ");

            /* Apresentar somente hora */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_HORA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_HORA").PadRight(16, ' ')); // "SOMENTE HORA    ");

            /* Apresentar o roteiro alternando com Mensagem */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MENSAGEM").PadRight(16, ' ')); // "MENSAGEM        ");

            /* Apresentar o roteiro alternando com Saudação */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_SAUDACAO);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SAUDACAO").PadRight(16, ' ')); // "SAUDACAO        ");

            /* Apresentar o roteiro alternando com Data Hora */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_DATA_HORA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_DATA_E_HORA").PadRight(16,' ')); // "DATA E HORA     ");

            /* Apresentar o roteiro alternando com Hora Partida */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_HORA_SAIDA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_HORA_DE_SAIDA").PadRight(16, ' ')); // "HORA DE SAIDA   ");

            /* Apresentar o roteiro alternando com Mensagem e Saudação */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_SAUDACAO);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_MSG_SAUDACAO").PadRight(16, ' ')); // "ROT MSG SAUDACAO");

            /* Apresentar o roteiro alternando com Mensagem e Hora Partida */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_HORA_SAIDA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_MSG_PARTIDA").PadRight(16, ' ')); // "ROT MSG PARTIDA ");

            /* Apresentar o roteiro alternando com Mensagem e Data Hora */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_DATA_HORA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_MSG_DATAHORA").PadRight(16, ' ')); // "ROT MSG DATAHORA");

            /* Apresentar o roteiro alternando com Tarifa */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_TARIFA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_TARIFA").PadRight(16, ' ')); // "TARIFA          ");

            /* Apresentar a mensagem alternando com hora */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_HORA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MENSAGEM_HORA").PadRight(16, ' ')); // "MENSAGEM HORA   ");

            /* Apresentar a mensagem alternando com hora e temperatura */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_HORA_E_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_HORA_TEMP").PadRight(16, ' ')); // "MSG HORA TEMP   ");

            /* Apenas Temperatura */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_TEMPERATURA").PadRight(16, ' ')); // "SO TEMPERATURA  ");

            /* Apenas Velocidade */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_VELOCIDADE);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_SO_VELOCIDADE").PadRight(16, ' ')); // "SO VELOCIDADE   ");

            /* Alterna roteiro com velocidade */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_VELOCIDADE);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_COM_VEL").PadRight(16, ' ')); // "ROT COM VEL     ");

            /* Alterna mensagem com velocidade */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_VELOCIDADE);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_COM_VEL").PadRight(16, ' ')); // "MSG COM VEL     ");

            /* Alterna roteiro com temperatura */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_COM_TEMP").PadRight(16, ' ')); // "ROT COM TEMP    ");

            /* Alterna mensagem com temperatura */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_COM_TEMP").PadRight(16, ' ')); // "MSG COM TEMP    ");

            /* Alterna roteiro com hora e temperatura (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_HORA);
            tipos.Add(TipoExibicao.EXIBICAO_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_HORA_TEMP").PadRight(16, ' ')); //"ROT+ HORA+ TEMP ");

            /* Alterna mensagem com hora e temperatura (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_HORA);
            tipos.Add(TipoExibicao.EXIBICAO_TEMPERATURA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_HORA_TEMP2").PadRight(16, ' ')); // "MSG+ HORA+ TEMP ");

            /* Alterna mensagem com hora e temperatura (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_ROTEIRO);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM_SECUNDARIA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_ROT_MSG_SECMSG").PadRight(16, ' ')); // "ROU + MSG + SEC MSG ");

            /* Alterna mensagem com ID do Motorista (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_ID_MOTORISTA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_ID_MOTORISTA").PadRight(16, ' ')); // "MSG + ID ");

            /* Alterna mensagem com Nome do Motorista (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_NOME_MOTORISTA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_NOME_MOTORISTA").PadRight(16, ' ')); // "MSG + NOME ");

            /* Alterna mensagem com ID e Nome do Motorista (Separados) */
            tipos.Clear();
            tipos.Add(TipoExibicao.EXIBICAO_MENSAGEM);
            tipos.Add(TipoExibicao.EXIBICAO_ID_MOTORISTA);
            tipos.Add(TipoExibicao.EXIBICAO_NOME_MOTORISTA);
            AdicionarItemAlternancia(tipos, linhaSuperior, rm.GetString("ARQUIVO_ALT_MSG_ID_NOME_MOTORISTA").PadRight(16, ' ')); // "MSG + ID + NOME ");

            this.Salvar(this.ArquivoNome);
        }


        public bool VerificarCRC(byte[] dados)
        {
            //const int posCRC1 = 62;
            //const int posCRC2 = 63;
            
            //Byte[] dados2 = this.ToBytes();
            //FormatoAlternancia fa = ArquivoALTAssembly(dados);

            //List<byte> dadosValidos = new List<byte>();
            //for (int i = 0; i < dados.Length; i++)
            //{
            //    if ((i == posCRC1) || (i == posCRC2))
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        dadosValidos.Add(dados[i]);
            //    }
            //}
            //ushort crc16 = (ushort)(dados[posCRC1] << 8);
            //crc16 += (ushort)(dados[posCRC2]);
            //ushort crc16_2 = CRC16CCITT.Calcular(dadosValidos.ToArray());
            //ushort crc16_3 = CRC16CCITT.Calcular(dados);
            //return (CRC16CCITT.Calcular(dadosValidos.ToArray()) == crc16);
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoAlternancia* alternancia = (FormatoAlternancia*)pSrc;

                    return (alternancia->crc == CalcularCRC(dados));
                }
            }
        }

        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoAlternancia* alternancia = (FormatoAlternancia*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&alternancia->crc - (int)pSrc);
                Array.Copy(dados, ((int)&alternancia->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&alternancia->crc - (int)pSrc,
                           dados.Length - ((int)&alternancia->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoAlternancia));
            }
            return resposta;
        }

        
    }
}
