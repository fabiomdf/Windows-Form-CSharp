using System;
using System.Collections.Generic;
using System.Text;
using FRT.Pontos.Arquivos;
using FRT.Pontos.Controlador_90;

namespace ImportacaoLDX
{
    public class ParserLDX
    {
        public Controlador control; 

        public ParserLDX()
        {
            
        }
        public void CarregarControlador(string nomeArquivo)
        {
            FRT.Pontos.Util.Defines.config = @"C:\Program Files\FRT Tecnologia Eletronica Ltda\Pontos 6.15.0\idioma.inf";
            
            //FRT.Pontos.Util.Defines.config = @"C:\SVN\PontosX2\Recursos\Bibliotecas\idioma.inf";
            
            ArquivoFamilia_11_0_0 arquivo = new ArquivoFamilia_11_0_0();
            Controlador _control = new Controlador(arquivo.AbrirArquivo(nomeArquivo));
            ArquivoControlador ac = new ArquivoControlador(_control);
            switch (ac.VerificarFamilia(nomeArquivo))
            {
                case "Pontos 9.0.*":
                    {
                        ArquivoFamilia_9_0_0 arquivoCtrl = new ArquivoFamilia_9_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                        this.control.Versao = 4;
                    }
                    break;
                case "Pontos 9.1.*":
                    {
                        ArquivoFamilia_9_1_0 arquivoCtrl = new ArquivoFamilia_9_1_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.2.*":
                    {
                        ArquivoFamilia_9_2_0 arquivoCtrl = new ArquivoFamilia_9_2_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.3.*":
                    {
                        ArquivoFamilia_9_3_0 arquivoCtrl = new ArquivoFamilia_9_3_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.5.*":
                    {
                        ArquivoFamilia_9_5_0 arquivoCtrl = new ArquivoFamilia_9_5_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.6.*": // A versão 9.6 é a mesma do 9.5, só que roda com outra memória.
                    {
                        ArquivoFamilia_9_5_0 arquivoCtrl = new ArquivoFamilia_9_5_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.7.*": // A versão 9.6 é a mesma do 9.5, só que roda com outra memória.
                    {
                        ArquivoFamilia_9_5_0 arquivoCtrl = new ArquivoFamilia_9_5_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.8.*": // A versão 9.8 é a mesma do 9.5, só que roda com outra memória.
                    {
                        ArquivoFamilia_9_5_0 arquivoCtrl = new ArquivoFamilia_9_5_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.9.*": // A versão 9.9 é a mesma do 9.5, só que roda com outra memória.
                    {
                        ArquivoFamilia_9_5_0 arquivoCtrl = new ArquivoFamilia_9_5_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.10.*": // Esta versão possui a implementação de senha no controlador
                    {
                        ArquivoFamilia_9_10_0 arquivoCtrl = new ArquivoFamilia_9_10_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 9.11.*": // Esta versão possui a implementação de senha no controlador
                    {
                        ArquivoFamilia_9_11_0 arquivoCtrl = new ArquivoFamilia_9_11_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));

                        this.control.PainelRadioExistente = arquivoCtrl.ObterPainelRadioExistente(nomeArquivo);
                    }
                    break;
                case "Pontos 9.12.*": // Esta versão possui a implementação de senha no controlador
                    {
                        ArquivoFamilia_9_11_0 arquivoCtrl = new ArquivoFamilia_9_11_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                        this.control.PainelRadioExistente = arquivoCtrl.ObterPainelRadioExistente(nomeArquivo);
                    }
                    break;
                case "Pontos 10.0.*":
                    {
                        ArquivoFamilia_10_0_0 arquivoCtrl = new ArquivoFamilia_10_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 10.1.*":
                    {
                        ArquivoFamilia_10_1_0 arquivoCtrl = new ArquivoFamilia_10_1_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 10.2.*": //A versão 10.2 é a mesma da 10.1, só que roda com outra memória.
                    {
                        ArquivoFamilia_10_1_0 arquivoCtrl = new ArquivoFamilia_10_1_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 10.3.*": //A versão 10.3 é a mesma da 10.1, só que roda com outra memória.
                    {
                        ArquivoFamilia_10_1_0 arquivoCtrl = new ArquivoFamilia_10_1_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 10.4.*": //A versão 10.3 é a mesma da 10.1, só que roda com outra memória.
                    {
                        ArquivoFamilia_10_1_0 arquivoCtrl = new ArquivoFamilia_10_1_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Pontos 11.0.*":
                    {
                        ArquivoFamilia_11_0_0 arquivoCtrl = new ArquivoFamilia_11_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                        this.control.PainelRadioExistente = arquivoCtrl.ObterPainelRadioExistente(nomeArquivo);
                        this.control.VelocidadeInicial = arquivoCtrl.ObterVelocidadeInicial(nomeArquivo);
                    }
                    break;
                case "Pontos 11.*.*":
                    {
                        ArquivoFamilia_11_0_0 arquivoCtrl = new ArquivoFamilia_11_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));                        
                        this.control.VelocidadeInicial = arquivoCtrl.ObterVelocidadeInicial(nomeArquivo);
                        ushort[] mensagemSecundaria = arquivoCtrl.ObterMensagensSecundarias(nomeArquivo);
                        for (int i = 0; i < 6; i++)
                        {
                            this.control.Painel[i].MensagemSelecionada2 = mensagemSecundaria[i];
                        }
                        this.control.Tempo_USB_Off = arquivoCtrl.ObterTempoUSBOff(nomeArquivo);                        
                        this.control.Tempo_InversaoBits = arquivoCtrl.ObterVariavel(nomeArquivo, "TempoInversaoBits");
                    }
                    break;
                case "Pontos G 1.0.0":
                    {
                        ArquivoPainelMensagem_1_0_0 arquivoCtrl = new ArquivoPainelMensagem_1_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
                case "Painel Mensg 1.2":
                    {
                        ArquivoPainelMensagem_2_0_0 arquivoCtrl = new ArquivoPainelMensagem_2_0_0();
                        this.control = new Controlador(arquivoCtrl.AbrirArquivo(nomeArquivo));
                    }
                    break;
            }

            //ArquivoFamilia_11_0_0 arquivo = new ArquivoFamilia_11_0_0();
            //control = new Controlador(arquivo.AbrirArquivo(nomeArquivo));

            //Senhas[0] = arquivo.ObterSenha(nomeArquivo);
            //control.PainelRadioExistente = arquivo.ObterPainelRadioExistente(nomeArquivo);
            //control.VelocidadeInicial = arquivo.ObterVelocidadeInicial(nomeArquivo);            
            
            //control.Tempo_USB_Off = arquivo.ObterTempoUSBOff(nomeArquivo);
            //control.Tempo_InversaoBits = arquivo.ObterVariavel(nomeArquivo, "TempoInversaoBits");            

        }
        // Propriedades para retornar as informações
        public int quantidadePaineis
        {
            get { return control.QtdPaineis; }
        }

        public string RetornaLabelMensagem(int indicePainel, int indiceMensagem, int indiceFrase, int indiceImagem)
        {
            return control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceFrase].Imagem[indiceImagem].Rotulo;
        }

        public int RetornaRolagemMensagem(int indicePainel, int indiceMensagem, int indiceFrase)
        {
            return RetornarRolagem(control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceFrase]);
        }
        public int RetornaRolagemRoteiro(int indicePainel, int indiceRoteiro, int indiceFrase, bool ida)
        {
            if (ida)
                return RetornarRolagem(control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceFrase]);
            else
            {
                return RetornarRolagem(control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceFrase]);
            }
        }
        public int RetornaRolagemNumeroRoteiro(int indicePainel, int indiceRoteiro)
        {
            return RetornarRolagem(control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero);
        }

        private int RetornarRolagem(Frase f)
        {
            int resposta = 0;

            switch (f.RolagemTipo)
            {
                case RolagemTipo.Nenhuma:
                    resposta = 0;
                    break;
                case RolagemTipo.Continua:
                    resposta = 1;
                    break;
                case RolagemTipo.Paginada:
                    resposta = 2;
                    break;
                case RolagemTipo.Continua2:
                    resposta = 3;
                    break;
                case RolagemTipo.Continua3:
                    resposta = 3; //TODO: FIXME Verificar com o Pessoal de Firmware esta implementação.
                    break;
            }
            return resposta;
        }
        public string RetornarLabelNumero(int indicePainel, int indiceRoteiro, int indiceImagem)
        {
            return control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Rotulo;
        }
        public string RetornarLabelRoteiro(int indicePainel, int indiceRoteiro, int indiceFrase, int indiceImagem, bool ida)
        {
            if (ida)
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceFrase].Imagem[indiceImagem].Rotulo;
            else
            {
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceFrase].Imagem[indiceImagem].Rotulo;
            }
        }
        public int RetornarQuantidadeFrases(int indicePainel, int indiceRoteiro, bool ida)
        {
            if (ida)
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida.Length;
            else
            {
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta.Length;
            }
        }
        public int RetornarQuantidadeFrases(int indicePainel, int indiceMensagem)
        {
            return control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Length;
        }

        public byte[] RetornarPixelBytesRoteiros(int indicePainel, int indiceRoteiro, int indiceFrase, bool ida)
        {
            if (ida)
                return RetornarPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceFrase], control.Painel[indicePainel].Altura, control.Painel[indicePainel].Largura);
            else
            {
                return RetornarPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceFrase], control.Painel[indicePainel].Altura, control.Painel[indicePainel].Largura);
            }
        }
        public byte[] RetornarPixelBytesNumeroRoteiro(int indicePainel, int indiceRoteiro)
        {
            return RetornarPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero, control.Painel[indicePainel].Altura, control.Painel[indicePainel].Largura);
        }

        public byte[] RetornarPixelBytesMensagens(int indicePainel, int indiceMensagem, int indiceFrase)
        {
            return RetornarPixelBytes(control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceFrase], control.Painel[indicePainel].Altura, control.Painel[indicePainel].Largura);
        }

        public int RetornarLarguraPixelBytes(Frase frase)
        {
            List<int> colunas = new List<int>();

            for (int i = 0; i < frase.Imagem.Length; i++)
            {
                colunas.AddRange(frase.Imagem[i].Colunas);
            }
            return colunas.Count;
        }

        public int RetornarLarguraPixelBytesRoteiros(int indicePainel, int indiceRoteiro, int indiceFrase, bool ida)
        {
            if (ida)
                return RetornarLarguraPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceFrase]);
            else
            {
                return RetornarLarguraPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceFrase]);
            }
        }
        public int RetornarLarguraPixelBytesNumeroRoteiro(int indicePainel, int indiceRoteiro)
        {
            return RetornarLarguraPixelBytes(control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero);
        }

        public int RetornarLarguraPixelBytesMensagens(int indicePainel, int indiceMensagem, int indiceFrase)
        {
            return RetornarLarguraPixelBytes(control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceFrase]);
        }
        public byte[] RetornarPixelBytes(Frase imagem, int altura, int largura)
        {
            List<int> colunas = new List<int>();

            for (int i= 0; i< imagem.Imagem.Length; i++)
            {
                colunas.AddRange(imagem.Imagem[i].Colunas);
            }
            

            long qntBytesImagem = altura * colunas.Count;
            qntBytesImagem = (qntBytesImagem / 8) + (qntBytesImagem % 8 == 0 ? 0 : 1);
            byte[] bytes = new byte[qntBytesImagem];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = this.GetBitmapByte(i, colunas.ToArray(), altura);
            }

            return bytes;
        }

        // Método copiado do APP 
        private byte GetBitmapByte(int indiceByte, int[] colunas, int altura)
        {
            byte retorno = 0;
            int colunaInicial, colunaFinal;
            int bitColunaInicial, bitColunaFinal;
            int i, j;
            Int32 bitmap;

            colunaInicial = (int) ((indiceByte*8)/altura);
            colunaFinal = (int) (((indiceByte + 1)*8)/altura);

            bitColunaInicial = (int) ((indiceByte*8)%altura);
            bitColunaFinal = (int) (((indiceByte + 1)*8)%altura);

            i = colunaInicial;
            j = bitColunaInicial;

            bitmap = colunas[i];

            while (!((i == colunaFinal) && (j == bitColunaFinal)))
            {
                retorno = (byte) ((retorno >> 1) & ~(0x80));
                retorno |= (byte) (((bitmap >> j) & 0x01) << 7);

                j++;
                if (j == altura)
                {
                    j = 0;
                    i++;

                    if (i < colunas.Length)
                    {
                        bitmap = colunas[i];
                    }
                    else
                    {
                        bitmap = 0;
                    }
                }
            }

            return retorno;
        }


        public int GetTempoMensagem()
        {
            return control.TempoMensagem;
        }
        public int GetTempoRolagem()
        {
            return control.TempoRolagem;
        }
        public int GetTempoRoteiro()
        {
            return control.TempoRoteiro[0];
        }

        public int GetHoraInicioDia()
        {
            return control.HoraInicioDia;
        }

        public int GetHoraInicioTarde()
        {
            return control.HoraInicioTarde;
        }

        public int GetHoraInicioNoite()
        {
            return control.HoraInicioNoite;
        }

        public int RetornarQuantidadeRoteiros(int indicePainel)
        {
            return control.Painel[indicePainel].Roteiro.Length;
        }
        public int RetornarQuantidadeFrasesIdaRoteiros(int indicePainel, int indiceRoteiro, bool ida)
        {
            if (ida)
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida.Length;
            else // Volta
                return control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta.Length;
        }
        public int RetornarQuantidadeFrasesMensagem(int indicePainel, int indiceMensagem)
        {
            return control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Length;
        }

        public int retornarQuantidadeMensagens(int indicePainel)
        {
            return control.Painel[indicePainel].Mensagem.Length;
        }

        public int RetornarAlternancia(int indicePainel)
        {
            return (int)control.Painel[indicePainel].Apresentacao;
        }

        public int RetornarAlturaPainel(int indicePainel)
        {
            return control.Painel[indicePainel].Altura;
        }
        public int RetornarLarguraPainel(int indicePainel)
        {
            return control.Painel[indicePainel].Largura;
        }

        public int RetornarMensagemSelecionada(int indicePainel)
        {
            // O menos um é por causa de Mensagem de Emergência que antes ficava em mensagens
            int resposta = (control.Painel[indicePainel].MensagemSelecionada <= 0)? 0:(control.Painel[indicePainel].MensagemSelecionada - 1);
            return resposta;
        }
        public int RetornarMensagemSelecionada2(int indicePainel)
        {
            // O menos um é por causa de Mensagem de Emergência que antes ficava em mensagens
            int resposta = (control.Painel[indicePainel].MensagemSelecionada2 <= 0) ? 0 : (control.Painel[indicePainel].MensagemSelecionada2 - 1);
            return (control.Painel[indicePainel].MensagemSelecionada2 - 1);
        }

        public uint RetornarRoteiroSelecionado()
        {
            return control.Painel[0].RoteiroSelecionado;
        }
        public int RetornarFormatacaoTipoFrasesRoteiro(int indicePainel, int indiceRoteiro, int indiceFrase, bool ida)
        {
            if (ida)
            {
                return (int)control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceFrase].FormatacaoTipo;
            }
            else
            {
                return (int)control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceFrase].FormatacaoTipo;
            }
        }
        public int RetornarFormatacaoTipoFrasesMensagem(int indicePainel, int indiceMensagem, int indiceFrase)
        {
            return (int)control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceFrase].FormatacaoTipo;
        }

        public string RetornaLabelSaudacao(int indicePainel, int indiceSaudacao)
        {
            return control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[0].Imagem[0].Rotulo;            
        }

        public string RetornaLabelDiaSemana(int indicePainel, int indiceDiasSemana)
        {
            return control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDiasSemana].Imagem[0].Rotulo;
        }

        public string RetornaLabelSaida(int indicePainel)
        {
            return control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[13].Imagem[0].Rotulo;
        }
    }
}
