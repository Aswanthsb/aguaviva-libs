// http://codinglab.blogspot.com
//
// Simple LZ-type compression system that requires almost no RAM for decompression, this is ideal for embedded systems.
// This code has been inspired by an article in http://excamera.com/sphinx/article-compression.html 
// The current implementation is faster and achieves a higher compression rate.
//
// 
#include <iostream>
#include <fstream>
using namespace std;

#define uint8_t unsigned __int8
#define uint16_t unsigned __int16

// bitstream object that can output individual bits from a char buffer
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

// bitstream object that can store individual bits into a char buffer
class ibitstream 
{
public:
   void begin(unsigned char *s) 
   {
      src = s;
      mask = 0x01;
      size = 0;
   }

   void set1( unsigned char c) 
   {
      *src &= ~mask;
      *src |= (c & 1 ) * mask;
      mask <<= 1;
      if (!mask) {
         mask = 1;
         src++;
         size++;
      }
   }

   void setn( unsigned int c, unsigned char n) 
   {
      while (n--) 
      {
         set1( c & 1 );
         c>>=1;
      }
   }

   unsigned int GetSize()
   {
      return size+1;
   }

private:
   unsigned char *src;
   unsigned char mask;
   unsigned int size;
};



class lz 
{

   unsigned int lookback;
   unsigned int len;

public:
   lz(int lb, int ln)
   {
      lookback = lb;
      len = ln;
   }

   size_t compress(uint8_t* src, uint8_t* dst, size_t src_len, size_t d_len)
   {
      unsigned int lenmax = (1<<len);
      unsigned int lenmask = lenmax -1;

      unsigned int lbmax = (1<<lookback);
      unsigned int lbmask = lbmax -1;

      ibitstream fb;
      fb.begin(dst);

      //push decompressed length
      fb.setn(src_len,24);

      //push lookback size 
      fb.setn(lookback,8);

      //push max length size
      fb.setn(len,8);

      for( int i = 0;i< src_len;)
      {
         int maxlen = -1;
         int offset = 0;

         // lookback for string
         for(int s = 2;s < (lbmax + 2); s++)
         {
            if ( (i - s) < 0)
               break;

            // try to find string and get its length
            for(int l=0;l<lenmax+2;l++)
            {
               if ( i + l > src_len )
                  break;

               if ( src[i+l-s] == src[i+l] )
               {
                  if (l>=maxlen)
                  {                     
                     maxlen = l+1;
                     offset = s;
                  }
               }
               else
               {
                  break;
               }
            }
         }

         if ( maxlen <=1  )
         {
            //printf("%c\n", *src);
            fb.set1(0);
            fb.setn(src[i],8);
            i++;
         }
         else
         {
            maxlen-=2;
            if ( maxlen > lenmask)
               maxlen = lenmask;

            //printf("(%i,%i) \n", offset, maxlen+2);
            fb.set1(1);
            fb.setn(offset-2,lookback);

            fb.setn(maxlen ,len);
            i+=maxlen+2;
         }

      }



      return fb.GetSize();
   }

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
};

int main(int argc, char* argv[])
{
   ifstream::pos_type size;
   uint8_t * memblock;
   uint8_t * cmpblock;
   uint8_t * verblock;

   if ( argc != 2)
   {
      printf("Usage:\n%s <infile>\n", argv[0]);
      printf("\nWill compress <infile> into a file called <infile>.Z\n");
      return 0;
   }

   ifstream file (argv[1], ios::in|ios::binary|ios::ate);
   if (file.is_open())
   {
      size = file.tellg();
      memblock = new uint8_t [size];

      file.seekg (0, ios::beg);
      file.read ((char*)memblock, size);
      file.close();

      int minsize = size;

      //buffer where to store compressed data
      cmpblock = new uint8_t [size*2];

      //buffer where to store decompressed data (used to verify that compression and decompression work fine)
      verblock = new uint8_t [size];

      int opt_ln= -1;
      int opt_lb= -1;

      for( int ln = 1; ln < 15; ln++ )
      {
         for( int lb = 1; lb < 15; lb++ )
         {
            lz *c = new lz(lb,ln);
            size_t cmpsize = c->compress(memblock, cmpblock, size, size*2);

            size_t sizedecomp = c->decompress(cmpblock, verblock);
            if ( memcmp(verblock,memblock,size) == 0 )
            {
               if ( cmpsize < minsize )
               {
                  minsize = cmpsize;
                  opt_ln = ln;
                  opt_lb = lb;
                  printf("%i %i => %i  %%%i\n", lb,ln, cmpsize, (100*cmpsize)/sizedecomp);
               }
            }
            else
            {
               printf("compression/decompression failed, check algorithm!\n");
            }

            delete c;
         }
      }

      //save binary data
      char outputfilename[1024];
      strcpy(outputfilename,argv[1]);
      strcat(outputfilename, ".Z");

      printf("Saving %s ... ", outputfilename);

      ofstream ofile (outputfilename, ios::out|ios::binary|ios::ate);
      if (ofile.is_open())
      {
         lz *c = new lz(opt_lb,opt_ln);
         size_t cmpsize = c->compress(memblock, cmpblock, size, size*2);
         delete c;

         ofile.write ((char*)cmpblock, cmpsize);
         ofile.close();
         printf("OK!");
      }
      else
      {
         printf("error!");
      }


      delete[] memblock;
      delete[] verblock;
      delete[] cmpblock;

   }
   else 
   {
      cout << "Unable to open file";
   }

   return 0;
}

