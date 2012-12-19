#ifndef _SERIAL0_
#define _SERIAL0_

#include <avr/interrupt.h>
#include "helpers.h"
#include "SerialX.h"

class Serial0 : public Serial
{
public:
	void Begin()
	{
		#define BAUD 19200
		#include <util/setbaud.h>
		UBRR0H = UBRRH_VALUE;
		UBRR0L = UBRRL_VALUE;
		#if USE_2X
		UCSR0A |= (1 << U2X);
		#else
		UCSR0A &= ~(1 << U2X);
		#endif
		
		SETBIT(UCSR0B, RXEN0); 	
		SETBIT(UCSR0B, TXEN0); 
		SETBIT(UCSR0B, RXCIE0);	
		sei();
	}
	
	void Write( const unsigned char c )
	{
		/* Wait for empty transmit buffer */
		while ( !( UCSR0A & (1<<UDRE0)) )
			;
		/* Put data into buffer, sends the data */
		UDR0 = c;
	}
};


extern Serial0 Ser0;
	

#endif