#include "serialX.h"
#include "io.h"

static unsigned char GetCharFromSerial(Serial *ser)
{
	while( ser->Available() == 0 );
	return ser->Read();
}


static void rmReceive(Serial *ser, unsigned int address, IO* io )
{
	while ( 1 ) 
	{	
		unsigned char c = GetCharFromSerial(ser);
		if ( c == 0 )
			break;
			
		unsigned char checksum = 0;
		
		unsigned char back = c;
		
		while( c-- )
		{
			unsigned char data = GetCharFromSerial(ser);
			checksum += data;
			io->Write( address++, data );
		}
		
		if ( GetCharFromSerial(ser) != checksum)			
		{
			address -= back;
			ser->Write("b");
		}
		else
		{
			ser->Write("g");
		}		
	}
}


static void rmSend(Serial *ser, unsigned int address, unsigned int size, IO* io )
{
	for(;;)
	{	
        unsigned char block = 255;    
        
        if ( size < block )
            block = size;
    
        ser->Write(block);
        
        if ( block == 0 )
            break;
        
        unsigned char data;
        unsigned char checksum = 0;
		while( block-- )
		{
            data = io->Read(address++);
            checksum += data;
            ser->Write( data );
            size--;
        }
        ser->Write( checksum );
        
        data = GetCharFromSerial(ser);
        
        if ( data != 'g')
            break;
	}
}


void rmModem(Serial *ser, IO *io)
{
    for(;;)
    {       
        unsigned char data = GetCharFromSerial(ser);
        if ( data == 'r' )
        {
            unsigned int size = GetCharFromSerial(ser)*256 + GetCharFromSerial(ser);
            rmReceive( ser, size, io );
        }
        else if ( data == 's' )
        {
            unsigned int addr = GetCharFromSerial(ser)*256 + GetCharFromSerial(ser);
            unsigned int size = GetCharFromSerial(ser)*256 + GetCharFromSerial(ser);
            rmSend( ser, addr, size, io );
        }
        else if ( data == '?' )
        {
            ser->Write( ":)" );
        }
        else if ( data == 'x' )
        {
            return;
        }
    }
}
