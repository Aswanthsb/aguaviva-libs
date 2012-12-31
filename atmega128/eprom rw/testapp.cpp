#include <stdio.h>
#include <string.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/wdt.h>
#include <util/delay.h>
#include <avr/pgmspace.h>
#include <util/crc16.h>

#include "prompt.h"

#include "serial0.h"
#include "io.h"
#include "dump.h"
#include "rmmodem.h"
#include "parser.h"

#include "e27128.h"
#include "e27256.h"
#include "e27512.h"
#include "RAM62256.h"
#include "ee28256.h"

#include "msx.h"

#define LED_PORT   PORTB
#define LED_DDR    DDRB
#define LED_BIT    PB1

#define BT_PORT    PORTC
#define BT_DDR     DDRC
#define BT_PIN     PINC
#define BT_BIT     PC7

#include <stdio.h>

#undef FDEV_SETUP_STREAM 
#define FDEV_SETUP_STREAM(p, g, f) { 0, 0, f, 0, 0, p, g, 0 }

static int uart_putchar(char c, FILE *stream)
{
	if (c == '\n')
		uart_putchar('\r', stream);

	Ser0.Write( c );
	
	return 0;
}

static FILE mystdout = FDEV_SETUP_STREAM(uart_putchar, NULL, _FDEV_SETUP_WRITE);
volatile unsigned char Count0 = 0;

// pin 25
ISR(INT0_vect)
{
	Count0++;
}

volatile unsigned char Count1 = 0;

ISR(TIMER0_OVF_vect)
{
	Count1++;
}

volatile unsigned char Count2 = 0;

ISR(TIMER1_OVF_vect)
{
	Count2++;
}

char wait[] = "|/-\\";
char w = 0;

void SetDelay(int _d1, int _d2);

char msg[] = "The AT28C256 is a high-performance electrically erasable and programmable readonly memory. Its 256K of memory is organized as 32,768 words by 8 bits. Manufactured with Atmel's advanced nonvolatile CMOS technology, the device offers access times to 150 ns with power dissipation of just 440 mW. When the device is deselected, the CMOS standby current is less than 200 µA.";

unsigned DumpAddress = 0;
unsigned DumpLength = 256;

E27512 e27512;
E27256 e27256;
E27128 e27128;
RAM62256 ram62256;
EE28256 ee28256;


IO *pIO = &ee28256;


bool ProcessCommand( char **p )
{

	if ( ParseToken(p, "dump") )
	{
		ParseSpaces(p);
		if ( ParseUInt(p, &DumpAddress) )
		{
			ParseSpaces(p);
			ParseUInt(p, &DumpLength);
		}
		
		Dump(DumpAddress, DumpLength, pIO);

		DumpAddress += DumpLength;
	}
	else if ( ParseToken(p, "download") )
	{
		rmModem(&Ser0, pIO);
	}
	else if ( ParseToken(p, "crc") )
	{
		ParseSpaces(p);
		
		unsigned int start;
		unsigned int length;
		if ( ParseUInt(p, &start) && ParseUInt(p, &length) )
		{
			uint16_t crc = 0;
			for(unsigned int i=0;i<length;i++)
			{
				crc = _crc16_update(crc, pIO->Read(i));
			}
			printf("crc %x\n", crc);
		}
	}
	else if ( ParseToken(p, "reboot") )
	{
		DDRC = 0xff;
		PINC = 0x00;
		asm("JMP (0x1F800/2)"); 
	}		
	else if ( ParseToken(p, "port") )
	{
		char port = GetChar(p);
		
		int val;
		
		if ( ParseInt(p, &val) )
		{		
			switch(port)
			{
				case 'a': DDRA = val; PORTA = val; break;
				case 'b': DDRB = val; PORTB = val; break;
				case 'c': DDRC = val; PORTC = val; break;
				case 'd': DDRD = val; PORTD = val; break;
				case 'e': DDRE = val; PORTE = val; break;
				case 'f': DDRF = val; PORTF = val; break;
				case 'g': DDRG = val; PORTG = val; break;
			}
		}
	}		
	else if ( ParseToken(p, "timer") )
	{
		printf("timers: %u %u %u\n", Count0, Count1, Count2 );
	}		
	else
	{
		return false;
	}
	
	return true;	
}


int main( void ) 
{

/*
	TCCR0 = 1<<CS02;                      //divide by 256 * 256 
	TIMSK = 1<<TOIE0;                     //enable timer interrupt 

	TCCR1B = 1<<CS12;                      //divide by 256 * 256 
	TIMSK |= 1<<TOIE1;                     //enable timer interrupt 
*/
	char command[256];
	command[0]=0;

	stdout = &mystdout;
	Ser0.Begin();
	
	printf("XChip stuff\n");
	   
	pIO->Init();
	
	msxInit();
	   
	while ( 1 ) 
	{	
		printf ( ">");
		GetCommand(command);
		printf ( "\n");
		
		char *pp = command;
		char **p = (char **)(&pp);		
		
		ProcessCommand(p);
		
		ParseSpaces(p);
		
		if ( (**p)==0 )
		{
			printf ( "ok\n" );
			continue;
		}
		/*
		else
		{
			printf ( "error from: '%s'\n", pp );
		}				
		*/
		char c = **p;
		
		if ( c == 'x' )
		{
			int l = strlen(msg);
			for(unsigned int i=0;i<l;)
			{
				msxEnableWait();
				msxWaitForIO();
				if (msxReading())
				{
					msxWrite(msg[i]);
					printf("%c", msg[i]);								
					i++;
				}
				msxDisableWait();

			}
			//printf("\n");
		}
		else if ( c == 'X' )
		{
			for(;;)
			{
				msxEnableWait();
				msxWaitForIO();
				unsigned char c = msxRead();
				
				printf("%x,", c );
				msxDisableWait();
			}
			//printf("\n");
		}
		else if ( c == 'e' )
		{
			printf("waiting r/w...");
			msxEnableWait();
			msxWaitForIO();
			if (msxReading())
			{
				printf("wrote 65 to MSX\n");								
				msxWrite(65);						
			}
			else
			{
				printf("read fom MSX: %x\n", msxRead() );				
			}
			
			msxDisableWait();
		}
		else if ( c == '1' )
		{
			pIO = &e27128;            
			printf("Read27128\n");
		}
		else if ( c == '2' )
		{
			pIO = &e27256;
			printf("Read27256\n");
		}
		else if ( c == '5' )
		{
			pIO = &e27512;
			printf("Read27512\n");
		}
		else if ( c == 'd')
		{		
            Dump(0, 256, pIO);
		}
		else if ( c == 'm')
		{
			rmModem(&Ser0, pIO);		
		}
		else if ( c == 'o')
		{		
			for(unsigned int i=0;i<strlen(msg);i++)
				pIO->Write(i, msg[i]);
		}
		else if ( c == 'O')
		{
			for(unsigned int i=0;i<strlen(msg);i++)
				pIO->Write(i, 0);
		}
		else if ( c == 'p')
		{
            char ttt[] = "raul";
			for(unsigned int i=0;i<strlen(ttt);i++)
				pIO->Write(i, ttt[i]);
		}
		else
		{
			printf ( "error from: '%s'\n", pp );
		}				
		
	}

	return 0; /* never reached */
}
