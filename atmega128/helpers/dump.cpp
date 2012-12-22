#include <stdio.h>

#include "dump.h"

void Dump( unsigned int address, unsigned int len, IO *io)
{
	while( address < len )
	{
		unsigned int l = len;
		
		if (l>16)
		{
			l = 16;
		}
		
		printf("%04X:  ", address);	
			
		for( char i =0;i<l;i++)
		{
			unsigned char data = io->Read( address + i );
			printf("%02X ", data );
		}
		
		printf("  ");	

		for( char i =0;i<l;i++)
		{
			unsigned char data = io->Read( address + i );
			if ( data < 32 || data > 128) 
			{
				data = '.';
			}
				
			printf("%c", data );
		}
		
		address += l;
		printf("\n");	
	}
}
