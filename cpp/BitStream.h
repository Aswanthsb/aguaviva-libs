#ifndef BITSTREAM
#define BITSTREAM

class BitStream
{
   unsigned char *pData;
   int size;
   int bitCount;

public:
   BitStream(unsigned char *_pData, int _size)
   {
      size = _size;
      pData = _pData;
      bitCount = 0;
   }

   char GetBit()
   {
      char b = pData[ bitCount>>3 ] >> (7-(bitCount&7));
      bitCount++;
      return b & 1;
   }

   void PutBit(unsigned char b)
   {
      if ((bitCount & 7) == 0)
      {
         pData[ bitCount>>3 ] = 0;
      }

      pData[ bitCount>>3 ] |= b<<(7-(bitCount&7));
      bitCount++;
   }

   void PutBitN(unsigned char b, char bits)
   {
      while(bits--)
      {
         PutBit((b>>bits)&1);
      }     
   }

   unsigned char GetBitN(char bits)
   {
      unsigned char b = 0;
      while(bits--)
      {
         b<<=1;
         b |= GetBit();
      }     

      return b;
   }

   size_t GetPos()
   {
      return bitCount;
   }
};

#endif