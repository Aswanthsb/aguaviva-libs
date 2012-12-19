// Martin Thomas 4/2008
#include <stdio.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/wdt.h>
#include <util/delay.h>
#include <avr/pgmspace.h>
#include <util/crc16.h>

#include "serial0.h"
#include "io.h"
#include "dump.h"
#include "rmmodem.h"

//#include "tetris.gb.h"
//#include "gbd.gb.h"

#define LED_PORT   PORTB
#define LED_DDR    DDRB
#define LED_BIT    PB1

#define BT_PORT    PORTC
#define BT_DDR     DDRC
#define BT_PIN     PINC
#define BT_BIT     PC7

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

	stdout = &mystdout;
	Ser0.Begin();

	printf("XChip stuff\n");
	printf(">");
	   
	while ( 1 ) 
	{	
		if ( Ser0.Available() > 0 )
		{
			char c = Ser0.Read();
			
			printf("%c\n", c);

			if ( c == 0 )
			{
				printf("caquitassss\n");
			}
			else if ( c == 't' )
			{
				printf("timers: %u %u %u\n", Count0, Count1, Count2 );
			}
			else if ( c == 'S' )
			{
				DDRC = 0xff;
				PINC = 0x00;
				asm("JMP (0x1F800/2)"); 
			}
			else
			{
				printf("Error\n");
			}
		
            
			printf(">");		
		}
		
	}

	return 0; /* never reached */
}
