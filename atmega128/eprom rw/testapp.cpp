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

#include "e27256.h"
#include "e27512.h"


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

void Dump( unsigned int address, unsigned char (*pPeek)(unsigned int) )
{
	unsigned char buf[16];
	
	for(int j=0;j<16;j++)
	{
		printf("%04x ", address );
	
		for(int i=0;i<16;i++)
		{
			buf[i] = (*pPeek)(address);
			printf("%02x ", buf[i] );
			address++;
		}

		for(int i=0;i<16;i++)
		{
			unsigned char c = buf[i];
			if ( c<32 || c > 127) 
				c = '.';
			printf("%c", c );
		}
		
		printf("\n");
	}
}

void SetDelay(int _d1, int _d2);

char msg[] = "The AT28C256 is a high-performance electrically erasable and programmable readonly memory. Its 256K of memory is organized as 32,768 words by 8 bits. Manufactured with Atmel's advanced nonvolatile CMOS technology, the device offers access times to 150 ns with power dissipation of just 440 mW. When the device is deselected, the CMOS standby current is less than 200 µA.";

bool ProcessBasicCommand( char **p )
{
	if (**p == '{')
	{
		*p++;
		ProcessBasicCommand( p );
	}
	else if (**p == '}')
	{
		*p++;
		return true;
	}
	else if ( ParseToken(p, "for") )
	{
		ParseSpaces(p);
		int l = 0;
		
		ParseInt(p, &l);		
		
		for(int i=0;i<l;i++)
		{
			char **fp = p
			if ( Expect(fp, "{") )
			{
				do 
				{
					ProcessBasicCommand( fp );
					ParseSpaces(fp);		
				}while( Expect(fp,';') )
				
				Expect(fp, "}");
			}
		}
	}
}

bool ProcessCommand( char **p )
{
	if ( ParseToken(p, "delay") )
	{
		ParseSpaces(p);
		int d1=1982;
		int d2=1982;
		if ( ParseInt(p, &d1) )
		{
			ParseSpaces(p);
			if ( ParseInt(p, &d2) )
			{
				SetDelay(d1,d2);
			}
		}
	}
	else if ( ParseToken(p, "dump") )
	{
		ParseSpaces(p);
		int addr=0;
		if ( ParseInt(p, &addr) )
		{
			address = addr;
		}
		
		Dump(address, pPeekVar);
	}
	else if ( ParseToken(p, "test") )
	{
		ParseSpaces(p);
		int d1=0;
		int d2=0;
		if ( ParseInt(p, &d1) )
		{
			ParseSpaces(p);
			if ( ParseInt(p, &d2) )
			{
				SetDelay(5,5);

				for(int i=0;i<strlen(msg);i++)
					Write27256(i, 0);

				SetDelay(d1,d2);

				for(int i=0;i<strlen(msg);i++)
					Write27256(i, msg[i]);					
			}
		}		
	}
	else if ( ParseToken(p, "crc") )
	{
		uint16_t crc = 0;
		for(int i=0;i<strlen(msg);i++)
		{
			crc = _crc16_update(crc, pPeekVar(i));
		}
		printf("crc %x\n", crc);
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
	DDRA = 0x00;
	PORTA = 0x00;
	DDRB = 0x00;
	PORTB = 0x00;
	DDRC = 0x00;
	PORTC = 0x00;
	DDRD = 0x00;
	PORTD = 0x00;
	DDRF = 0x00;
	PORTF = 0x00;

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
	   
	unsigned address = 0;
	   
	unsigned char (*pPeekVar)(unsigned int) = Read27256;	   
	   
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
		
		if ( c == '2' )
		{
			pPeekVar = Read27256;
			printf("Read27256\n");
		}
		else if ( c == '5' )
		{
			pPeekVar = Read27512;
			printf("Read27512\n");
		}
		else if ( c == 'o')
		{
			for(int i=0;i<strlen(msg);i++)
				Write27256(i, msg[i]);
		}
		else if ( c == 'O')
		{
			for(int i=0;i<strlen(msg);i++)
				Write27256(i, 0);
		}
		else if ( c == 'p')
		{
			SetAddress27256(0);
			SetData27256('r');
			Dump(0, pPeekVar);
			SetAddress27256(1);
			SetData27256('a');
			SetAddress27256(2);
			SetData27256('u');
			SetAddress27256(3);
			SetData27256('l');
		}
		else
		{
			printf ( "error from: '%s'\n", pp );
		}				
		
	}

	return 0; /* never reached */
}
