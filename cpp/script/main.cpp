// parser.cpp : Defines the entry point for the console application.
//
#include <stdio.h>
#include <tchar.h>

#include <string.h>
#include "c:\WinAVR-20100110\projects\libs\script.h"
#include "c:\WinAVR-20100110\projects\libs\parser.h"
#include "c:\WinAVR-20100110\projects\libs\variables.h"
#include "c:\WinAVR-20100110\projects\libs\expression.h"

Variables var;

bool ProcessStatements( char **p )
{
	if ( ParseToken(p, "hi") )
	{
		printf("hi\n");
	}
	else if ( ParseToken(p, "bye") )
	{
		printf("bye\n");
	}
	else if ( ParseToken(p, "print") )
	{
		do 
		{
			int v;
			if ( parseExpression( p, &var, &v ) == false)
				return false;
			printf("%i ",v);
		} while( ExpectNoSpace(p, ',') );
		printf("\n");
	}
	else
	{
		return false;
	}

	return true;
}

int _tmain(int argc, _TCHAR* argv[])
{
	//char command[] = "for 1 { hi hi } bye";
	//char command[] = " mm=(1+3) * 4; if (2>4)  hi;  else  bye;   caca=4*mm;";
	//char command[] = " mm=(1+3) * 0 if mm>4 { hi } else { bye }  caca=4*mm";
	char command[] = " for(i=0;i<10;i++){  for(j=0;j<10;) { print j+(i*10), i, 2*i, 3*i; j++;} } bye;";
	char *pp = command;
	do
	{
		if ( ProcessScript(&pp, ProcessStatements, &var) == false)
		{
			printf ( "error from: '%s'\n", *pp );
			return 0;
		}
	}while(*pp!=0);	

	printf ( "ok\n");
	var.List();

	return 0;
}

