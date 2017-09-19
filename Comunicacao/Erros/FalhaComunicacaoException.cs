using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacao.Erros
{
    public class FalhaComunicacaoException : ApplicationException
    {
        public FalhaComunicacaoException(String mensagem)
            : base(mensagem)
        { }

        public FalhaComunicacaoException(String mensagem, Exception excecao)
            : base(mensagem, excecao)
        { }
    }
}
