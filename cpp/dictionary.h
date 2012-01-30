
#ifndef DICTIONARY_H
#define DICTIONARY_H

#include <map>
#include <string>

/// Basically a map with a few helpers
template <class tKey, class tVal> class Dictionary
{
public:
   /// hash
   std::map<tKey , tVal> DataBase;

   /// returns the number of elements in the dictionary
   size_t size()
   {
      return DataBase.size();
   }

   /// add a key, value tuple
   void Add(tKey k, tVal v)
   {
      DataBase[k] = v;
   }

   /// no idea
   bool Find(tKey k, tVal **vout)
   {
      std::map<tKey , tVal>::iterator i;

      i = DataBase.find(k);

      if (i == DataBase.end())
      {
         vout = NULL;
         return false;
      }
      else
      {
         *vout = &i->second;
         return true;
      }
   }

   /// returns a string with all the keys
   void DumpKeys()
   {
      string keys;
      std::map<tKey , tVal>::iterator i;

      for (i = DataBase.begin(); i != DataBase.end(); i++)
      {
         printf("%s %s", i->first, i->second);
      }
   }
};

#endif // DICTIONARY_H
