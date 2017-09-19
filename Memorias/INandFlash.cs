using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memorias
{
    public interface INandFlash
    {
        void Inicializar();
	    void Abrir();
	    void Fechar();

	    UInt32 GetNumBlocks();
        UInt32 GetPagesPerBlock();
	    UInt32 GetPageDataSize();
	    UInt32 GetPageSpareSize();
	    byte EraseBlock(UInt32 blockNumber);
        
	    byte PageWrite(UInt32 address, byte[] buffer, UInt32 length);
	    byte PageRead(UInt32 address, byte[] buffer, UInt32 length);
	    byte SpareWrite(UInt32 address, byte[] buffer, UInt32 length);
	    byte SpareRead(UInt32 address, byte[] buffer, UInt32 length);
    }
}
