#include "serialX.h"
#include "io.h"
#include <stdio.h>

static unsigned char GetByteFromSerial(Serial *ser)
{
	while( ser->Available() == 0 );
	return ser->Read();
}


static unsigned int GetUintFromSerial(Serial *ser)
{	
    unsigned int n = GetByteFromSerial(ser);    
	return (n<<8) + GetByteFromSerial(ser);
}


static void rmReceive(Serial *ser, unsigned int address, IO* io )
{
	while ( 1 ) 
	{	
		unsigned char c = GetByteFromSerial(ser);
		if ( c == 0 )
			break;
			
		unsigned char checksum = 0;
		
		unsigned char back = c;
		
		while( c-- )
		{
			unsigned char data = GetByteFromSerial(ser);
			checksum += data;
			io->Write( address++, data );
		}
		
		if ( GetByteFromSerial(ser) != checksum)			
		{
			address -= back;
			ser->Write('b');
		}
		else
		{
			ser->Write('g');
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
        
        data = GetByteFromSerial(ser);
        
        if ( data != 'g')
            break;
	}
}

#include <avr/interrupt.h>
#include "helpers.h"


static void test(Serial *ser, IO* io )
{
    unsigned char block = 255;    
    while( block-- )
    {
//        ser->Write( block );
		/* Wait for empty transmit buffer */
		while ( !( UCSR0A & (1<<UDRE0)) )
			;
		/* Put data into buffer, sends the data */
		UDR0 = block;


    }
}

void rmModem(Serial *ser, IO *io)
{
    unsigned int addr = 0;
    unsigned int size = 0;
    for(;;)
    {       
        unsigned char data = GetByteFromSerial(ser);
        if ( data == 'r' )
        {
            addr = GetUintFromSerial(ser);
            rmReceive( ser, addr, io );
        }
        else if ( data == 's' )
        {
            addr = GetUintFromSerial(ser);            
            size = GetUintFromSerial(ser);
            rmSend( ser, addr, size, io );
        }
        else if ( data == '?' )
        {
            ser->Write( ":)" );
        }
        else if ( data == 'i' )
        {
            printf("addr:%u size:%u\n", addr, size) ;
        }
        else if ( data == 't' )
        {
            test(ser, io );
        }        
        else if ( data == 'x' )
        {
            return;
        }
    }
}
