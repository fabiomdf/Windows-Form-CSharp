using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FRT.Pontos.Controlador_90;
using System.Collections;

namespace ImportacaoLDX
{
    public static class arquivoLDX
    {      
        public static int _contadorLinhas = 0;
        public static String diretorioFontes = @"C:\Projetos\Pontos 6\Pontos 6\Pontos 6\Fontes XLCS";

        public static string retornarAtributoLinha(TextReader fileInput, string atributo)
        {
            string linha = "";
            string strPalavra = "";
            string resultado = "";
            if (fileInput.Peek().Equals(-1))
            {
                for (int i = 0; i < _contadorLinhas; i++)
                {
                    fileInput.ReadLine();
                }
            }
            while (fileInput.Peek() > -1)
            {
                linha = fileInput.ReadLine();
                _contadorLinhas++;
                for (int i = atributo.Length; i < linha.Length; i++)
                {
                    strPalavra = linha.Substring(0, i);
                    if (strPalavra.Equals(atributo + "="))
                    {
                        resultado = linha.Substring(i, linha.Length - i);
                        return resultado.TrimStart();
                    }
                }
            }

            return resultado;
        }
        public static bool VerificarExistenciaVelocidade(string arquivoNome)
        {
            TextReader input = new StreamReader(arquivoNome, Encoding.UTF8, true);
            bool resposta = false;
            try
            {
                if (!retornarAtributoLinha(input, "FrasesFixasLCD[70]").Equals(String.Empty))
                {
                    resposta = true;
                }
                else
                {
                    resposta = false;
                }
            }
            catch
            {
                resposta = false;
            }

            input.Close();
            return resposta;
        }
        public static Controlador AbrirArquivoLDX(string arquivoNome)
        {
            bool existeVelocidade = VerificarExistenciaVelocidade(arquivoNome);
            // Recuperando informações dentro do arquivo			
            TextReader input = new StreamReader(arquivoNome, Encoding.UTF8, true);
            Controlador Ctrl = new Controlador();
            Controlador Control;
            // Informações do arquivo
            int QtdPaineis = 0;
            int QtdRoteiros = 0;
            int QtdMensagem = 0;
            byte[] Altura;
            byte[] Largura;
            string NomeArquivo = "";

            NomeArquivo = retornarAtributoLinha(input, "NomeArquivo");
            // Informações do Controlador
            #region Informações do controlador
            Ctrl.Familia = retornarAtributoLinha(input, "Familia");
            Ctrl.Versao = Convert.ToByte(retornarAtributoLinha(input, "Versao"));
            Ctrl.TempoMensagem = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "TempoMensagem"));
            Ctrl.TempoRolagem = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "TempoRolagem"));
            Ctrl.TempoRoteiro[0] = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "TempoRoteiro"));
            string aux = input.ReadLine();
            if (aux.StartsWith("TempoRoteiro"))
            {
                for (int i = 1; i < 8; i++)
                {
                    Ctrl.TempoRoteiro[i] = (ushort)Convert.ToInt16(aux.Substring(14, aux.Length - 14));
                    aux = input.ReadLine();

                    if (aux.StartsWith("QtdPaineis"))
                    {
                        QtdPaineis = (ushort)Convert.ToInt16(aux.Substring(11, aux.Length - 11));
                    }
                }
            }
            else if (aux.StartsWith("QtdPaineis"))
            {
                QtdPaineis = (ushort)Convert.ToInt16(aux.Substring(11, aux.Length - 11));
            }


            QtdRoteiros = Convert.ToInt16(retornarAtributoLinha(input, "QtdRoteiros"));
            QtdMensagem = Convert.ToInt16(retornarAtributoLinha(input, "QtdMensagens"));

            Ctrl.HoraInicioDia = Convert.ToByte(retornarAtributoLinha(input, "HoraInicioDia"));
            Ctrl.HoraInicioTarde = Convert.ToByte(retornarAtributoLinha(input, "HoraInicioTarde"));
            Ctrl.HoraInicioNoite = Convert.ToByte(retornarAtributoLinha(input, "HoraInicioNoite"));
            switch (retornarAtributoLinha(input, "Idioma"))
            {
                case "Portugues":
                    Ctrl.Lingua = Lingua.Portugues;
                    break;
                case "Espanhol":
                    Ctrl.Lingua = Lingua.Espanhol;
                    break;
                case "Ingles":
                    Ctrl.Lingua = Lingua.Ingles;
                    break;
            }
            #region Informações do Teclado
            Ctrl.Teclado.AjusteDireito = Convert.ToBoolean(retornarAtributoLinha(input, "AjusteDireito"));
            Ctrl.Teclado.AjusteEsquerdo = Convert.ToBoolean(retornarAtributoLinha(input, "AjusteEsquerdo"));
            Ctrl.Teclado.AlternaRoteiroCom = Convert.ToBoolean(retornarAtributoLinha(input, "AlternaRoteiroCom"));
            Ctrl.Teclado.IdaVolta = Convert.ToBoolean(retornarAtributoLinha(input, "IdaVolta"));
            Ctrl.Teclado.MensagemDireito = Convert.ToBoolean(retornarAtributoLinha(input, "MensagemDireito"));
            Ctrl.Teclado.MensagemEsquerdo = Convert.ToBoolean(retornarAtributoLinha(input, "MensagemEsquerdo"));
            Ctrl.Teclado.Ok = Convert.ToBoolean(retornarAtributoLinha(input, "OK"));
            Ctrl.Teclado.RoteiroDireito = Convert.ToBoolean(retornarAtributoLinha(input, "RoteiroDireito"));
            Ctrl.Teclado.RoteiroEsquerdo = Convert.ToBoolean(retornarAtributoLinha(input, "RoteiroEsquerdo"));
            Ctrl.Teclado.SelecionaPainel = Convert.ToBoolean(retornarAtributoLinha(input, "SelecionaPainel"));
            #endregion
            Altura = new byte[6];
            Altura[0] = 16;
            Altura[1] = 16;
            Altura[2] = 16;
            Altura[3] = 16;
            Altura[4] = 16;
            Altura[5] = 16;
            Largura = new byte[6];
            Largura[0] = 128;
            Largura[1] = 128;
            Largura[2] = 128;
            Largura[3] = 128;
            Largura[4] = 128;
            Largura[5] = 128;
            bool[] Ativado = new bool[6];
            Ativado[0] = false;
            Ativado[1] = false;
            Ativado[2] = false;
            Ativado[3] = false;
            Ativado[4] = false;
            Ativado[5] = false;

            #region Informações dos Painéis
            for (int indicePainel = 0; indicePainel < QtdPaineis; indicePainel++)
            {
                //input = new StreamReader(FileName);
                Ativado[indicePainel] = Convert.ToBoolean(retornarAtributoLinha(input, "Ativado"));
                Altura[indicePainel] = Convert.ToByte(retornarAtributoLinha(input, "Altura"));
                Largura[indicePainel] = Convert.ToByte(retornarAtributoLinha(input, "Largura"));
            }
            Control = new Controlador(Ctrl.Familia, Ativado, Altura, Largura, QtdPaineis);
            input.Close();
            input = new StreamReader(arquivoNome);
            string s = input.ReadLine();
            Control.Familia = Ctrl.Familia;
            Control.Versao = Ctrl.Versao;
            Control.TempoMensagem = Ctrl.TempoMensagem;
            Control.TempoRolagem = Ctrl.TempoRolagem;
            Control.TempoRoteiro = Ctrl.TempoRoteiro;
            Control.HoraInicioDia = Ctrl.HoraInicioDia;
            Control.HoraInicioTarde = Ctrl.HoraInicioTarde;
            Control.HoraInicioNoite = Ctrl.HoraInicioNoite;
            Control.Lingua = Ctrl.Lingua;
            Control.Teclado.AjusteDireito = Ctrl.Teclado.AjusteDireito;
            Control.Teclado.AjusteEsquerdo = Ctrl.Teclado.AjusteEsquerdo;
            Control.Teclado.AlternaRoteiroCom = Ctrl.Teclado.AlternaRoteiroCom;
            Control.Teclado.IdaVolta = Ctrl.Teclado.IdaVolta;
            Control.Teclado.MensagemDireito = Ctrl.Teclado.MensagemDireito;
            Control.Teclado.MensagemEsquerdo = Ctrl.Teclado.MensagemEsquerdo;
            Control.Teclado.Ok = Ctrl.Teclado.Ok;
            Control.Teclado.RoteiroDireito = Ctrl.Teclado.RoteiroDireito;
            Control.Teclado.RoteiroEsquerdo = Ctrl.Teclado.RoteiroEsquerdo;
            Control.Teclado.SelecionaPainel = Ctrl.Teclado.SelecionaPainel;

            for (int indicePainel = 0; indicePainel < QtdPaineis; indicePainel++)
            {
                if (Ativado[indicePainel])
                {
                    Control.Painel[indicePainel].Ativado = Ativado[indicePainel];
                    Control.Painel[indicePainel].Altura = Altura[indicePainel];
                    Control.Painel[indicePainel].Largura = Largura[indicePainel];
                    Control.Painel[indicePainel].Roteiro.Remover(0);
                    Control.Painel[indicePainel].Mensagem.Remover(0);
                    Control.Painel[indicePainel].Mensagem.Remover(0);
                    Control.Painel[indicePainel].Apresentacao = RetornarTipoApresentacao(retornarAtributoLinha(input, "Apresentacao"));

                    // string s1 = (indicePainel > 0)?input.ReadLine():"";
                    Control.Painel[indicePainel].RoteiroSelecionado = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "RoteiroSelecionado"));
                    Control.Painel[indicePainel].MensagemSelecionada = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "MensagemSelecionada"));
                    for (int i = 0; i < QtdRoteiros; i++)
                    {
                        Roteiro route = new Roteiro();
                        route.Numero = new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura);
                        route.Numero.DiretorioFontes = diretorioFontes;
                        route.Ida.Adicionar(new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura));
                        route.Ida[0].DiretorioFontes = diretorioFontes;
                        route.Volta = new FRT.Pontos.Util.ConjuntoDoTipo<Frase>();
                        route.Volta.Adicionar(new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura));
                        route.Volta[0].DiretorioFontes = diretorioFontes;
                        Control.Painel[indicePainel].Roteiro.Adicionar(route);
                    }

                    #region Roteiros
                    for (int indiceRoteiro = 0; indiceRoteiro < QtdRoteiros; indiceRoteiro++)
                    {
                        Roteiro route = new Roteiro();
                        #region Roteiro Numero
                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].RolagemTipo"));

                        int QtdFrasesNumero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdFrases"));
                        int QtdTotalImagens = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdTotalDeImagens"));
                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].IdaVoltaIguais = Convert.ToBoolean(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].IdaVoltaIguais"));
                        int QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.QtdImagens"));
                        for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                        {
                            Imagem teste = new Imagem();
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem.Adicionar(teste);
                        }

                        // Propriedades abaixo são READ ONLY
                        //Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.AlturaPainel = Convert.ToBoolean(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlturaPainel"));
                        //Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.LarguraPainel = Convert.ToBoolean(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.LarguraPainel"));
                        //Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.LarguraReal = Convert.ToBoolean(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.LarguraReal"));
                        route.Numero = new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura);
                        #region Formatação Tipo
                        switch (retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.FormatacaoTipo"))
                        {
                            case "FormatacaoTipo1":
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo1.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto");
                                    break;
                                }
                            case "FormatacaoTipo2":
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Largura_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto");
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto_Numero");
                                    break;
                                }
                            case "FormatacaoTipo3":
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Superior"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Superior"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Tamanho_Inferior"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto");
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto_Superior");
                                    break;
                                }
                            case "FormatacaoTipo4":
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Superior"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Superior"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Largura_Numero"));
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto");
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto_Numero");
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Texto_Superior");
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Tamanho_Inferior"));
                                    break;
                                }
                        }
                        #endregion Formatação Tipo
                        #region Imagens

                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.RolagemTipo =
                            retornarRolagem(retornarAtributoLinha(input,
                                                                  "Roteiro[" + indiceRoteiro + "].Numero.RolagemTipo"));
                        route.Numero.RolagemTipo = Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.RolagemTipo;

                        for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                        {
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Imagem[" + indiceImagem + "].Rotulo");
                            char[] espaco = new char[1];
                            espaco[0] = ' ';
                            string[] Colunas = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                            if (Colunas[0] != "Agora")
                            {
                                while (Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Coluna.Length > 0)
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Coluna.Remover(0);
                                }
                                for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                }
                            }
                            if (QtdImagens == 1)
                            {
                                bool ColunasIguaisZero = true;

                                for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                {
                                    if (!Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[indiceImagem].Coluna[indiceColunas].Equals(0))
                                    {
                                        ColunasIguaisZero = false;
                                        break;
                                    }
                                }
                                if (ColunasIguaisZero)
                                {
                                    Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.ConstruirImagens(
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo1.Texto
                                            .Substring(6));
                                }
                            }
                        }
                        if (QtdImagens == 0)
                        {
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.ConstruirImagens(Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.FormatacaoTipo1.Texto.Substring(6));
                        }
                        //                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.ConstruirImagens(Control.Painel[indicePainel].Roteiro[indiceRoteiro].Numero.Imagem[0].Rotulo);
                        #endregion Imagens
                        #endregion Roteiro Numero
                        #region Roteiros Ida
                        int QtdFrasesIda = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdFrases"));
                        if (!Familias_Rolagem_Individual.Contains(Control.Familia))
                        {
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].RolagemTipo"));
                        }

                        while (Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida.Length > 0)
                        {
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida.Remover(0);
                        }
                        for (int indiceIda = 0; indiceIda < QtdFrasesIda; indiceIda++)
                        {
                            Frase teste = new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura);
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida.Adicionar(teste);
                        }

                        for (int indiceIda = 0; indiceIda < QtdFrasesIda; indiceIda++)
                        {

                            // As Propriedades abaixo são READ ONLY
                            //            "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlturaPainel= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].AlturaPainel);
                            //            "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].LarguraPainel= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].LarguraPainel);
                            //            "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].LarguraReal= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].LarguraReal);
                            QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            if (Familias_Rolagem_Individual.Contains(Control.Familia))
                            {
                                Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].RolagemTipo"));
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem.Adicionar(teste);
                            }

                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }
                            #endregion Imagens
                            //Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].ConstruirImagens(Control.Painel[indicePainel].Roteiro[indiceRoteiro].Ida[indiceIda].Imagem[0].Rotulo);
                        }
                        #endregion Roteiros Ida
                        #region Roteiros Volta
                        // Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].RolagemTipo"));
                        int QtdFrasesVolta = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdFrases"));
                        int QtdTotalImagensRoteiro = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdTotalDeImagens"));
                        while (Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta.Length > 0)
                        {
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta.Remover(0);
                        }
                        for (int indiceVolta = 0; indiceVolta < QtdFrasesVolta; indiceVolta++)
                        {
                            Frase teste = new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura);
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta.Adicionar(teste);
                        }

                        for (int indiceVolta = 0; indiceVolta < QtdFrasesVolta; indiceVolta++)
                        {


                            // Propriedades abaixo são READ ONLY
                            //            "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlturaPainel= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].AlturaPainel);
                            //            "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].LarguraPainel= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].LarguraPainel);
                            //            "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].LarguraReal= " + Ctrl.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].LarguraReal);
                            QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto_Numero");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Texto_Superior");
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem.Adicionar(teste);
                            }
                            Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].RolagemTipo = retornarRolagem((retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].RolagemTipo")));

                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }
                            //Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].ConstruirImagens(Control.Painel[indicePainel].Roteiro[indiceRoteiro].Volta[indiceVolta].Imagem[0].Rotulo);
                            #endregion Imagens

                        }
                        #endregion Roteiros Volta
                    }
                    #endregion Roteiros
                    #region Mensagem
                    while (Control.Painel[indicePainel].Mensagem.Length > 0)
                    {
                        Control.Painel[indicePainel].Mensagem.Remover(0);
                    }


                    for (int i = 0; i < QtdMensagem; i++)
                    {
                        Mensagem msg = new Mensagem();
                        while (msg.Ida.Length > 0)
                        {
                            msg.Ida.Remover(0);
                        }

                        msg.Ida.Adicionar(new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura));
                        msg.Ida[0].DiretorioFontes = diretorioFontes;

                        Control.Painel[indicePainel].Mensagem.Adicionar(msg);
                    }
                    for (int indiceMensagem = 0; indiceMensagem < QtdMensagem; indiceMensagem++)
                    {
                        Control.Painel[indicePainel].Mensagem[indiceMensagem].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].RolagemTipo"));
                        int QtdFrasesMensagem = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].QtdFrases"));
                        int QtdTotalImagensMensagem = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].QtdTotalDeImagens"));
                        if (QtdFrasesMensagem == 0)
                        {
                            while (Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Length > 0)
                            {
                                Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Remover(0);
                            }
                        }
                        while (Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Length < QtdFrasesMensagem)
                        {
                            Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Adicionar(new Frase(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura));
                        }
                        for (int indiceIda = 0; indiceIda < Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida.Length; indiceIda++)
                        {

                            //////// READ ONLY - Abaixo/////////
                            //            "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlturaPainel= " + Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].AlturaPainel);
                            //            "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].LarguraPainel= " + Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].LarguraPainel);
                            //            "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].LarguraReal= " + Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].LarguraReal);
                            //////// READ ONLY - Acima/////////
                            int QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem.Adicionar(teste);
                            }
                            if (Familias_Rolagem_Individual.Contains(Control.Familia))
                            {
                                Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda.ToString() + "].RolagemTipo"));
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }
                            //for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            //{
                            //    Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Rotulo");
                            //    char[] espaco = new char[1];
                            //    espaco[0] = ' ';
                            //    string[] Colunas = retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                            //    if (Colunas[0] != "Agora")
                            //    {
                            //        while (Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Length > 0)
                            //        {
                            //            Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Remover(0);
                            //        }
                            //        for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                            //        {
                            //            Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                            //        }
                            //    }
                            //}
                            //Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].ConstruirImagens(Control.Painel[indicePainel].Mensagem[indiceMensagem].Ida[indiceIda].Imagem[0].Rotulo);
                            #endregion Imagens
                        }
                    }
                    #endregion Mensagens
                    #region Saudação
                    for (int indiceSaudacao = 0; indiceSaudacao < 3; indiceSaudacao++)
                    {

                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].RolagemTipo"));
                        int QtdFrasesSaudacao = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].QtdFrases"));
                        int QtdTotalImagensSaudacao = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].QtdTotalDeImagens"));

                        for (int indiceIda = 0; indiceIda < Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida.Length; indiceIda++)
                        {
                            //////// READ ONLY - Abaixo/////////
                            //            "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlturaPainel= " + Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].AlturaPainel);
                            //            "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].LarguraPainel= " + Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].LarguraPainel);
                            //            "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].LarguraReal= " + Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].LarguraReal);
                            //////// READ ONLY - Acima/////////

                            int QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].QtdImagens"));
                            //string teste = input.ReadLine();
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Largura_Numero"));
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto");
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto_Numero");
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Texto_Superior");
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda.ToString() + "].Tamanho_Inferior"));
                                        break;
                                    }


                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            while (Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem.Length > 0)
                            {
                                Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem.Remover(0);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem.Adicionar(teste);
                            }

                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                //input = new StreamReader(FileName);

                                Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }
                            // Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].ConstruirImagens(Control.Painel[indicePainel].Saudacao[indiceSaudacao].Ida[indiceIda].Imagem[0].Rotulo);
                            #endregion Imagens
                        }
                    }
                    #endregion Saudação
                    #region Mensagens Especiais
                    Control.Painel[indicePainel].MensagemEspecial = new MensagemEspecial(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura, diretorioFontes);
                    Control.Painel[indicePainel].MensagemEspecial.Customizada = Convert.ToBoolean(retornarAtributoLinha(input, "MensagemEspecial.Customizada"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoBarra.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoBarra.Deslocamento"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoBarra.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoBarra.PosicaoInicial"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoDoisPontos.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoDoisPontos.Deslocamento"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoDoisPontos.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoDoisPontos.PosicaoInicial"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoEspaco.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoEspaco.Deslocamento"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoEspaco.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoEspaco.PosicaoInicial"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoSaida.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoSaida.Deslocamento"));
                    Control.Painel[indicePainel].MensagemEspecial.PosicaoSaida.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoSaida.PosicaoInicial"));
                    if (Familias_Tarifa.Contains(Control.Familia))
                    {
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoMoeda.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoMoeda.Deslocamento"));
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoMoeda.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoMoeda.PosicaoInicial"));
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoVirgula.Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoVirgula.Deslocamento"));
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoVirgula.PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoVirgula.PosicaoInicial"));
                    }


                    for (int indiceDia = 0; indiceDia < 7; indiceDia++)
                    {
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoDiasSemana[indiceDia].Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoDiasSemana[" + indiceDia.ToString() + "].Deslocamento"));
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoDiasSemana[indiceDia].PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoDiasSemana[" + indiceDia.ToString() + "].PosicaoInicial"));
                    }
                    for (int indiceNumero = 0; indiceNumero < 10; indiceNumero++)
                    {
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoNumeros[indiceNumero].Deslocamento = Convert.ToByte(retornarAtributoLinha(input, "PosicaoNumeros[" + indiceNumero.ToString() + "].Deslocamento"));
                        Control.Painel[indicePainel].MensagemEspecial.PosicaoNumeros[indiceNumero].PosicaoInicial = (ushort)Convert.ToInt16(retornarAtributoLinha(input, "PosicaoNumeros[" + indiceNumero.ToString() + "].PosicaoInicial"));
                    }



                    if (!Control.Painel[indicePainel].MensagemEspecial.Customizada)
                    {
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipoHoraSaida = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipoHoraSaida"));
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipoDataHora = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipoDataHora"));
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipo"));
                        #region Mensagem Especial Padrão
                        for (int indiceIda = 0; indiceIda < Control.Painel[indicePainel].MensagemEspecial.Ida.Length; indiceIda++)
                        {
                            //////// READ ONLY - Abaixo/////////
                            //Ctrl.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].AlturaPainel = ;
                            //Ctrl.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].LarguraPainel = 128;
                            //Ctrl.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].LarguraReal = 128;                    
                            //////// READ ONLY - Acima/////////
                            int QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda.ToString() + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto_Numero");
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Texto_Superior");
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            while (Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem.Length > 0)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem.Remover(0);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem.Adicionar(teste);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagens; indiceImagem++)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "MensagemEspecial.Ida[" + indiceIda + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }
                            //Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].ConstruirImagens(Control.Painel[indicePainel].MensagemEspecial.Ida[indiceIda].Imagem[0].Rotulo);
                            #endregion Imagens
                        }
                        #endregion Mensagem Especial Padrão
                    }

                    else
                    {
                        #region Mensagem Especial Customizada
                        Control.Painel[indicePainel].MensagemEspecial = new MensagemEspecial(Control.Painel[indicePainel].Altura, Control.Painel[indicePainel].Largura);
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipoHoraSaida = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipoHoraSaida"));
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipoDataHora = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipoDataHora"));
                        Control.Painel[indicePainel].MensagemEspecial.RolagemTipo = retornarRolagem(retornarAtributoLinha(input, "MensagemEspecial.RolagemTipo"));
                        #region Dias da Semana
                        for (int indiceDia = 0; indiceDia < 7; indiceDia++)
                        {

                            //Ctrl.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].AlturaPainel = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlturaPainel= "));
                            // Ctrl.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].LarguraPainel = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].LarguraPainel= "));
                            //Ctrl.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].LarguraReal = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].LarguraReal= "));
                            int QtdImagensDiasDaSemana = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].QtdImagens"));
                            //Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto_Numero");
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Texto_Superior");
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            while (Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem.Length > 0)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem.Remover(0);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagensDiasDaSemana; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem.Adicionar(teste);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagensDiasDaSemana; indiceImagem++)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDia + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }

                            //Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].ConstruirImagens(Control.Painel[indicePainel].MensagemEspecial.DiasDaSemana[indiceDia].Imagem[0].Rotulo);
                            #endregion Imagens
                        }
                        #endregion Dias da Semana
                        #region Numeros e Caracteres Especiais
                        int _maxNumero = (Familias_Tarifa.Contains(Control.Familia)) ? 16 : 14;

                        _maxNumero = (Familias_Temperatura.Contains(Control.Familia) && existeVelocidade) ? 18 : _maxNumero;

                        _maxNumero = (Familias_Velocidade.Contains(Control.Familia) && existeVelocidade) ? 19 : _maxNumero;

                        for (int indiceNumero = 0; indiceNumero < _maxNumero; indiceNumero++)
                        {
                            // Ctrl.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].AlturaPainel = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].AlturaPainel= "));
                            //Ctrl.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].LarguraPainel = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].LarguraPainel= "));
                            // Ctrl.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].LarguraReal = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].LarguraReal= "));
                            int QtdImagensNumerosECaracteresEspeciais = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].QtdImagens"));
                            //Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].QtdImagens = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].QtdImagens"));
                            #region Formatação Tipo
                            switch (retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].FormatacaoTipo"))
                            {
                                case "FormatacaoTipo1":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo1.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo1.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo1.Texto = retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].Texto");
                                        break;
                                    }
                                case "FormatacaoTipo2":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo2.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto_Numero");
                                        break;
                                    }
                                case "FormatacaoTipo3":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Tamanho_Inferior"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo3.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto_Superior");
                                        break;
                                    }
                                case "FormatacaoTipo4":
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoHorizontal = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoHorizontal"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoHorizontal_Numero = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoHorizontal_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoHorizontal_Superior = AlinharHotizontal(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoHorizontal_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoVertical = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoVertical_Numero = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.AlinhamentoVertical_Superior = AlinharVertical(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].AlinhamentoVertical_Superior"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.Largura_Numero = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Largura_Numero"));
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.Texto = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto");
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.Texto_Numero = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto_Numero");
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.Texto_Superior = retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Texto_Superior");
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].FormatacaoTipo4.Tamanho_Inferior = Convert.ToInt16(retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceNumero + "].Tamanho_Inferior"));
                                        break;
                                    }
                            }
                            #endregion Formatação Tipo
                            #region Imagens
                            while (Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem.Length > 0)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem.Remover(0);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagensNumerosECaracteresEspeciais; indiceImagem++)
                            {
                                Imagem teste = new Imagem();
                                Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem.Adicionar(teste);
                            }
                            for (int indiceImagem = 0; indiceImagem < QtdImagensNumerosECaracteresEspeciais; indiceImagem++)
                            {
                                Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem[indiceImagem].Rotulo = retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].Imagem[" + indiceImagem + "].Rotulo");
                                char[] espaco = new char[1];
                                espaco[0] = ' ';
                                string[] Colunas = retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[" + indiceNumero + "].Imagem[" + indiceImagem + "].Colunas").Trim().Split(espaco);
                                if (Colunas[0] != "Agora")
                                {
                                    while (Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem[indiceImagem].Coluna.Length > 0)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem[indiceImagem].Coluna.Remover(0);
                                    }
                                    for (int indiceColunas = 0; indiceColunas < Colunas.Length; indiceColunas++)
                                    {
                                        Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem[indiceImagem].Coluna.Adicionar(Convert.ToInt32(Colunas[indiceColunas]));
                                    }
                                }
                            }

                            //Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].ConstruirImagens(Control.Painel[indicePainel].MensagemEspecial.NumerosECaracteresEspeciais[indiceNumero].Imagem[0].Rotulo);
                            #endregion Imagens

                        }
                        #endregion Numeros e Caracteres Especiais

                        #endregion Mensagem Especial Customizada
                    }
                    #endregion Mensagens Especiais
                }
            }
            #endregion Informações dos Painéis
            #region Frases Fixas do LCD
            for (int i = 0; i < 69; i++)
            {
                Control.FrasesFixasLCD[i] = retornarAtributoLinha(input, "FrasesFixasLCD[" + i.ToString() + "]");
            }
            #endregion Frases Fixas do LCD
            input.Close();
            AbrirAtributosColunasFixas(arquivoNome, Control);

            return Control;
            #endregion Informações do controlador
        }
        // Método para carregar o tipo de apresentação a partir da linha do arquivo.
        public  static Apresentacao RetornarTipoApresentacao(String apresentacao)
        {
            switch (apresentacao)
            {
                case "ApresentaRoteiro":
                    return Apresentacao.ApresentaRoteiro;
                case "ApresentaMensagem":
                    return Apresentacao.ApresentaMensagem;
                case "AlternaRoteiro_Saudacao":
                    return Apresentacao.AlternaRoteiro_Saudacao;
                case "AlternaRoteiro_DataHora":
                    return Apresentacao.AlternaRoteiro_DataHora;
                case "AlternaRoteiro_HoraPartida":
                    return Apresentacao.AlternaRoteiro_HoraPartida;
                case "AlternaRoteiro_Mensagem":
                    return Apresentacao.AlternaRoteiro_Mensagem;
                case "ApresentaNumeroRoteiro":
                    return Apresentacao.ApresentaNumeroRoteiro;
                case "AlternaRoteiro_MensagemSaudacao":
                    return Apresentacao.AlternaRoteiro_MensagemSaudacao;
                case "AlternaRoteiro_MensagemHoraPartida":
                    return Apresentacao.AlternaRoteiro_MensagemHoraPartida;
                case "AlternaRoteiro_MensagemDataHora":
                    return Apresentacao.AlternaRoteiro_MensagemDataHora;
                case "AlternaRoteiro_Tarifa":
                    return Apresentacao.AlternaRoteiro_Tarifa;
                case "ApresentaHora":
                    return Apresentacao.ApresentaHora;
                case "AlternaMensagem_Hora":
                    return Apresentacao.AlternaMensagem_Hora;
                case "AlternaMensagem_Hora_Temp":
                    return Apresentacao.AlternaMensagem_Hora_Temp;
                case "Apenas_Temperatura":
                    return Apresentacao.Apenas_Temperatura;
                case "Apenas_Velocidade":
                    return Apresentacao.Apenas_Velocidade;
                case "AlternaRoteiro_Velocidade":
                    return Apresentacao.AlternaRoteiro_Velocidade;
                case "AlternaMensagem_Velocidade":
                    return Apresentacao.AlternaMensagem_Velocidade;
                case "AlternaRoteiro_Temperatura":
                    return Apresentacao.AlternaRoteiro_Temperatura;
                case "AlternaMensagem_Temperatura":
                    return Apresentacao.AlternaMensagem_Temperatura;
                case "AlternaRoteiro_Hora_Temp":
                    return Apresentacao.AlternaRoteiro_Hora_Temp;
                case "AlternaMensagem_Hora_Temp2":
                    return Apresentacao.AlternaMensagem_Hora_Temp2;
                case "AlternaNumeroVelocidade":
                    return Apresentacao.AlternaNumeroVelocidade;
                case "RoteiroDataHoraMsg1Msg2":
                    return Apresentacao.RoteiroDataHoraMsg1Msg2;
                case "RoteiroSaudacaoDataHora":
                    return Apresentacao.RoteiroSaudacaoDataHora;
                case "NumeroHora":
                    return Apresentacao.NumeroHora;
                default:
                    return Apresentacao.ApresentaRoteiro;
            }
        }
        public static FRT.Pontos.Controlador_90.RolagemTipo retornarRolagem(string rolagem)
        {
            FRT.Pontos.Controlador_90.RolagemTipo temp = RolagemTipo.Nenhuma;
            switch (rolagem)
            {
                case "Nenhuma": temp = RolagemTipo.Nenhuma;
                    break;
                case "Continua": temp = RolagemTipo.Continua;
                    break;
                case "Continua2": temp = RolagemTipo.Continua2;
                    break;
                case "Paginada": temp = RolagemTipo.Paginada;
                    break;
                case "Continua3": temp = RolagemTipo.Continua3;
                    break;
                case "Multipla": temp = RolagemTipo.Multipla;
                    break;
                case "Invertida": temp = RolagemTipo.Invertida;
                    break;

            }
            return temp;
        }
        public static FRT.Pontos.Controlador_90.Alinhamento_Horizontal AlinharHotizontal(string alinhamento)
        {
            FRT.Pontos.Controlador_90.Alinhamento_Horizontal temp = Alinhamento_Horizontal.Centralizado;
            switch (alinhamento)
            {
                case "^C": break;
                case "^E": temp = Alinhamento_Horizontal.Esquerda;
                    break;
                case "^D": temp = Alinhamento_Horizontal.Direita;
                    break;
                case "Direita": temp = Alinhamento_Horizontal.Direita;
                    break;
                case "Esquerda": temp = Alinhamento_Horizontal.Esquerda;
                    break;
            }
            return temp;
        }
        /// <summary>
        /// * M: texto centralizado verticalmente<br>
        /// * S: alinhar a cima<br>
        /// * I: alinhar abaixo<br>
        /// </summary>
        public static FRT.Pontos.Controlador_90.Alinhamento_Vertical AlinharVertical(string alinhamento)
        {
            FRT.Pontos.Controlador_90.Alinhamento_Vertical temp = Alinhamento_Vertical.Centralizado;
            switch (alinhamento)
            {
                case "": break;
                case "M^": temp = Alinhamento_Vertical.Centralizado;
                    break;
                case "S^": temp = Alinhamento_Vertical.Acima;
                    break;
                case "I^": temp = Alinhamento_Vertical.Abaixo;
                    break;
                case "Centralizado": temp = Alinhamento_Vertical.Centralizado;
                    break;
                case "Acima": temp = Alinhamento_Vertical.Acima;
                    break;
                case "Abaixo": temp = Alinhamento_Vertical.Abaixo;
                    break;


            }
            return temp;
        }
        public static void AbrirAtributosColunasFixas(string arquivoNome, Controlador controlador)
        {
            // Recuperando informações dentro do arquivo			
            TextReader input = new StreamReader(arquivoNome, Encoding.UTF8, true);
            for (int i = 0; i < controlador.Painel.Length; i++)
            {                
                try
                {
                    controlador.Painel[i].ColunasFixas = Convert.ToInt16(retornarAtributoLinha(input, "ColunasFixas"));
                    controlador.Painel[i].NumeroFixo = Convert.ToBoolean(retornarAtributoLinha(input, "NumeroFixo"));
                }
                catch
                {
                    controlador.Painel[i].ColunasFixas = 0;
                    controlador.Painel[i].NumeroFixo = false;
                }
            }
            input.Close();
        }
        public static List<string> TodasFamiliasPontos = new List<string>(16)
        {
            "Pontos 9.0.*", 
            "Pontos 9.1.*",
            "Pontos 9.2.*",
            "Pontos 9.3.*",             
            "Pontos 9.5.*", 
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 10.0.*", 
            "Pontos 10.1.*", 
            "Pontos 10.2.*", 
            "Pontos 10.3.*",
            "Pontos 10.4.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Continua2 = new List<string>(11)
        {
            "Pontos 9.2.*",
            "Pontos 9.3.*",
            "Pontos 9.5.*",
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 10.0.*", 
            "Pontos 10.1.*",
            "Pontos 10.2.*",
            "Pontos 10.3.*", 
            "Pontos 10.4.*",   
            "Pontos 11.0.*",
            "Pontos 11.*.*",
            "Pontos G 1.0.0",
            "Painel Mensg 1.2",
            "Painel Mensg 1.3"
        };
        public static List<string> Familias_Continua3 = new List<string>(11)
        {
           "Pontos 11.0.*",
           "Pontos 11.*.*"
        };
        public static List<string> Familias_Invertida = new List<string>(11)
        {
           "Pontos 11.*.*"
        };
        public static List<string> Familias_Multiplas_Imagens = new List<string>(2)
        {            
            "Pontos 11.0.*",
            "Pontos 11.*.*"         
        };
        public static List<string> Familias_Rolagem_Individual = new List<string>(7)
        {
            "Pontos 9.5.*", 
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 10.1.*",
            "Pontos 10.2.*", 
            "Pontos 10.3.*",  
            "Pontos 10.4.*",   
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Tarifa = new List<string>(4)
        {
            "Pontos 9.3.*", 
            "Pontos 9.5.*",
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Temperatura = new List<string>(4)
        {           
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Velocidade = new List<string>(4)
        {           
            "Pontos 11.*.*"
        };

        public static List<string> Familia_9 = new List<string>(2)
        {
            "Pontos 9.1.*", 
            "Pontos 9.2.*", 
            "Pontos 9.3.*", 
            "Pontos 9.5.*",
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*"
        };
        public static List<string> Familia_10 = new List<string>(2)
        {
            "Pontos 10.0.*", 
            "Pontos 10.1.*", 
            "Pontos 10.2.*",
            "Pontos 10.3.*",            
            "Pontos 10.4.*",   
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familia_11 = new List<string>(2)
        {
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familia_9_Senha = new List<string>(2)
        {
            "Pontos 9.10.*",
            "Pontos 9.11.*",
            "Pontos 9.12.*"
        };

        public static List<string> Familias_Radio_Rolagem = new List<string>(2)
        {
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> TodasFamiliasPainelMensagem = new List<string>(2)
        {
            "Pontos G 1.0.0",
            "Painel Mensg 1.2",
            "Painel Mensg 1.3"            
        };

        public static List<string> FamiliasPainelMensagem_Alterna_Msg_Hora = new List<string>(2)
        {
            "Painel Mensg 1.2",
            "Painel Mensg 1.3"            
        };
        public static List<string> Familias_Alterna_Msg_Hora = new List<string>(2)
        {
            "Painel Mensg 1.2",
            "Painel Mensg 1.3",
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Alterna_So_Hora = new List<string>(2)
        {
            "Pontos G 1.0.0",
            "Painel Mensg 1.2",
            "Painel Mensg 1.3",
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_Radio = new List<string>(2)
        {
            "Pontos 9.1.*",
            "Pontos 9.2.*",
            "Pontos 9.3.*",             
            "Pontos 9.5.*", 
            "Pontos 9.6.*",
            "Pontos 9.7.*",
            "Pontos 9.8.*",
            "Pontos 9.9.*",
            "Pontos 9.10.*",            
            "Pontos 9.11.*",    
            "Pontos 9.12.*",
            "Pontos 11.0.*",
            "Pontos 11.*.*",
        };

        public static List<string> Familias_Controlador_Senha = new List<string>(2)
        {
            "Pontos 9.10.*",            
            "Pontos 9.11.*",  
            "Pontos 9.12.*",
            "Pontos 11.0.*",
            "Pontos 11.*.*"
        };
        public static List<string> Familias_USB = new List<string>(2)
        {
            "Pontos 11.0.*",
            "Pontos 11.*.*" 
        };

        public static List<string> Familias_TCP = new List<string>(2)
        {
            "Pontos 9.11.*",
            "Pontos 9.12.*",
            "Pontos 11.0.*", 
            "Pontos 11.*.*" 
        };
    }
}
