//#include <iostream>
#include <stdio.h>
#include <string.h>
#include "variables.h"
//using namespace std;




Variables::Variables()
{
	pLastVar = variables;
}

void *Variables::GetPreamble()
{
	return pLastVar;
}

void Variables::SetPreamble(void *pr)
{
	pLastVar = (char *)pr;
}

int *Variables::Find(char *name, byte len)
{
	for(char *vtable = variables;vtable!=pLastVar;) {
		char l = *vtable;
		vtable++;
		if ( l==len ) {
			if (strncmp(name,vtable,len)==0)	{
				return (int*)(vtable+len);
			}
		}
		vtable+=l+sizeof(int);
	}

	return NULL;
}

int *Variables::FindOrCreate(char *name, byte len)
{
	int *v = Find(name, len);
	if ( v==NULL ) {
		//add it
		*pLastVar=len;
		strcpy(pLastVar+1,name);
		pLastVar += *pLastVar +1;
		v = (int*)(pLastVar);
		pLastVar+=sizeof(int);
	}

	return v;
}

void Variables::Set(char *name, byte len, int value)
{
	int *v = FindOrCreate(name, len);
	*v = value;
}

void Variables::Set(char *name, int value)
{
	Set(name, strlen(name), value);
}



void Variables::List()
{
	for(char *vtable = variables;vtable!=pLastVar;) {
		char l = *vtable;
		vtable++;
		for(;l>0;l--)
		{
			printf("%c",*vtable++);
		}

		printf(": %i\n",*((int*)vtable));

		vtable+=l+sizeof(int);
	}
}
