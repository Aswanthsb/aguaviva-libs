class obitstream 
{
public:
   void begin(unsigned char *s) 
   {
      src = s;
      mask = 0x01;
   }

   unsigned char get1(void) 
   {
      unsigned char r = (*src & mask) != 0;
      mask <<= 1;
      if (!mask) {
         mask = 1;
         src++;
      }
      return r;
   }

   unsigned int getn(unsigned char n) 
   {
      unsigned char nn = n;
      unsigned int r = 0;
      while (n--) {
         r |= get1()<<(nn-n-1);
      }    
      return r;
   }

private:
   unsigned char *src;
   unsigned char mask;
};


size_t decompress(uint8_t* src, uint8_t* dst)
{
  obitstream fb;
  fb.begin(src);

  int src_len = fb.getn(8*3);

  lookback = fb.getn(8);
  len = fb.getn(8);

  int i;
  for(i=0;i<src_len;)
  {
     if (fb.get1() == 0)
     {
        dst[i] = fb.getn(8);
        i++;
     }
     else
     {
        int lb = fb.getn(lookback)+2;
        int ln = fb.getn(len)+2;
        while( ln--)
        {
           dst[i] = dst[ i - lb ];
           i++;
        }
     }
  }

  return i ;
}
