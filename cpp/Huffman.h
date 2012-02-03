#ifndef HUFFMAN
#define HUFFMAN

#include <vector>
#include <list>

#include "BitStream.h"

template< class T>
class Symbol
{
public:
   int weight;
   T value;

   Symbol *child[2]; 

   Symbol(int _weight, int _value)
   {
      weight = _weight;
      value = _value;

      child[0] = NULL;
      child[1] = NULL;
   }

   bool isLeaf()
   {
      return (child[0] == NULL) && (child[1] == NULL);
   }
};

template< class T>
class CreateHuffmanTree
{
   std::list<Symbol<T> *> list;

   std::vector<T> symbols[16];

public:
   CreateHuffmanTree()
   {
   }

   int LoadDictionary(void *data)
   {
      unsigned int size[16];
      unsigned int *pSize = (unsigned int *)data;
      for (unsigned int i=0;i<16;i++)
      {
         size[i] = *pSize++;
      }

      unsigned int totalsize = 0;
      T *sym = (T *)pSize;
      for (unsigned int i=0;i<16;i++)
      {
         for (unsigned int j=0;j<size[i];j++)
         {
            symbols[i].push_back( *sym++ );
         }
         totalsize += size[i];
      }

      return ( 16 * sizeof(int)) + (totalsize * sizeof(T));
   }

   void SaveDictionary(unsigned char **data, unsigned int *pSize)
   {
      //compute dicts size
      int totalsize = 0;
      for (unsigned int i=0;i<16;i++)
      {
         totalsize += symbols[i].size();
      }

      *pSize = ( 16 * sizeof(int)) + (totalsize * sizeof(T));

      *data = (unsigned char *)malloc( *pSize );
      int *pDest = (int*)*data;

      for (unsigned int i=0;i<16;i++)
      {
         *pDest++ = symbols[i].size();
      }

      T *pSym = (T *)pDest;
      for (unsigned int i=0;i<16;i++)
      {
         for (unsigned int j=0;j<symbols[i].size();j++)
         {
            *pSym++ = symbols[i][j];
         }
      }     
   }


   void AddElement(Symbol<T> *pSym)
   {
      if ( list.size() == 0)
      {
         list.push_back(pSym);
         return;
      }

      for(std::list<Symbol<T> *>::iterator i=list.begin(); i != list.end(); ++i)
      {
         if ( (*i)->weight > pSym->weight )
         {
            list.insert(i, pSym);
            return;
         }
      }

      list.push_back(pSym);
   }

   void AddElement(int weight, int value)
   {
      AddElement( new Symbol<T>( weight, value) );
   }

   void Build()
   {
      while( list.size()>=2 )
      {
         std::list<Symbol<T> *>::iterator i=list.begin();
         Symbol<T> *p0 = *(i);
         Symbol<T> *p1 = *(++i);
         list.remove(*list.begin());
         list.remove(*list.begin());

         Symbol<T> *pSym = new Symbol<T>( p0->weight + p1->weight, 0);

         pSym->child[0] = p0;
         pSym->child[1] = p1;

         AddElement(pSym);
      }      

      GenerateBins(*list.begin(),-1);
   }

   void GenerateBins(Symbol<T> *curr, int level)
   {
      if (curr->isLeaf())
      {
         symbols[level].push_back(curr->value);
      }
      if ( curr->child[0] )
      {
         GenerateBins(curr->child[0],level+1);
      }
      if ( curr->child[1] )
      {
         GenerateBins(curr->child[1],level+1);
      }
   }

   void Encode( T value, BitStream *pBS)
   {
      unsigned int code = 0;
      for(unsigned int i=0;i<16;i++)
      {
         for(unsigned int j=0;j<symbols[i].size();j++)
         {
            if (symbols[i][j] == value)
            {
               pBS->PutBitN( code, i+1);
               return;
            }
            code++;
         }
         code<<=1;
      }
   }

   T Decode( BitStream *pBS)
   {
      unsigned int val = 0;
      unsigned int ini = 0;
		
      for (unsigned int i=0;i<16;i++)
      {         
	      val = val*2 + pBS->GetBit();

	      if (symbols[i].size()>0)
         {
            unsigned int pos = val-ini;
		      if (pos<symbols[i].size())
            {
			      return symbols[i][pos];
            }
		      ini += symbols[i].size();
         }
	      ini <<= 2;
      }

      //handle error
      return  (T)(0xff);
   }
};




void huffmanTest()
{
   char text[]= "In computer science, the longest common substring problem is to find the longest string (or strings) that is a substring (or are substrings) of two or more strings. It should not be confused with the longest common subsequence problem. (For an explanation of the difference between a substring and a subsequence, see Substring vs. subsequence).";
 
   int histo[255];

   for(int i=0;i<255;i++)
      histo[i] = 0;

   for(unsigned int i=0;i<strlen(text);i++)
   {
      histo[text[i]]++;
   }

   char out[1024];

   BitStream bso((unsigned char *)out, 1024);
   BitStream bsi((unsigned char *)out, 1024);

   CreateHuffmanTree<char> ht;

   for(unsigned int i=0;i<255;i++)
   {
      if ( histo[i]>0)
      {
         ht.AddElement( histo[i],i);
      }
   }

   ht.Build();

   for(unsigned int i=0;i<strlen(text);i++)
   {
      ht.Encode(text[i],&bso);
   }

   for(unsigned int i=0;i<strlen(text);i++)
   {
      char c = ht.Decode(&bsi);
      printf("%c", c);
   }

   printf("\nCompressed size: %%%2.1f", ((float)bso.GetPos()/8.0)*100.0 / strlen(text) );

}

#endif