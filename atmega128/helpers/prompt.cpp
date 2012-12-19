#include "serial0.h"

int GetCommand(char *command)
{
	int i = 0;
	
	while ( 1 ) 
	{	
		if ( Ser0.Available() > 0 )
		{
			char c = Ser0.Read();
			
			if ( c == 8 || c == 127)
			{			
				if ( i>0)
				{
					i--;
					c = 127;
				}
				else
				{
					continue;
				}				
			}
			else
			{
				if ( c == 13 )
				{
					command[i] = 0;
					return i;
				}
				
				if (c>=32)				
					command[i++] = c;
				else
				{
					Ser0.Write( 7 );
					continue;
				}
			}			
			
			Ser0.Write( c );
		}
	}
}
