using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memorias
{
    public unsafe class ChunkTags
    {
        public TipoObjeto objectType;
        public UInt32 objectId; //: 24;
        public UInt32 parentId; // : 24;
        public Byte depth;
        public ushort chunkLength;
        public readonly int tamanho;

        public ChunkTags()
        {
            unsafe
            {
                tamanho = sizeof(FormatoChunkTags);   
            }             
        }
        protected unsafe struct FormatoChunkTags
        {
            public byte objectType;
            public fixed byte objectId[3]; //: 24;
            public fixed byte parentId[3]; // : 24;
            public Byte depth;
            public UInt16 chunkLength;
            public fixed byte reservado[6]; // : 24;
        }
        public Byte[] toBytes()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoChunkTags)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoChunkTags* formatoChunk = (FormatoChunkTags*)pSrc;

                    formatoChunk->objectType = (byte) this.objectType;
                    for (int i = 0; i < 3; i++)
                    {
                        formatoChunk->objectId[i] = BitConverter.GetBytes(this.objectId)[i];
                        formatoChunk->parentId[i] = BitConverter.GetBytes(this.parentId)[i];
                    }
                    formatoChunk->depth = this.depth;
                    formatoChunk->chunkLength = this.chunkLength;
                }

                return resultado;
            }
        }
        public void LoadFromBytes(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    ChunkTags.FormatoChunkTags* formatoChunk = (ChunkTags.FormatoChunkTags*)pSrc;

                    this.objectType = (TipoObjeto) formatoChunk->objectType;
                    byte[] objectIdLocal = new byte[sizeof(UInt32)];
                    byte[] parentIdLocal = new byte[sizeof(UInt32)];

                    for (int i = 0; i < 3;i++ )
                    {
                        objectIdLocal[i] = formatoChunk->objectId[i];
                        parentIdLocal[i] = formatoChunk->parentId[i];
                    }
                    this.objectId = BitConverter.ToUInt32(objectIdLocal, 0);
                    this.parentId = BitConverter.ToUInt32(parentIdLocal, 0);
                    this.depth = formatoChunk->depth;
                    this.chunkLength = formatoChunk->chunkLength;
                }
            }
        }

        
    }
}
