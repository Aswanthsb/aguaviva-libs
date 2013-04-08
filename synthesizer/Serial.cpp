#define RX_BUFFER_SIZE 32

class FIFO
{
	unsigned char buffer[RX_BUFFER_SIZE];
	unsigned char ini;
	unsigned char len;
	
	FIFO()
	{
		ini = 0;
		len = 0;
	}

	void PutChar(char c)
	{
		if (len < RX_BUFFER_SIZE )
		{
			buf->buffer[ (ini + len) & (RX_BUFFER_SIZE-1) ] = c;
			buf->len++;
		}
	}

	char GetChar()
	{
		if (len > 0 )
		{
			char c = buffer[ ini ];
			ini = (ini + 1) & (RX_BUFFER_SIZE-1);
			len--;
			return c;
		}
		
		return 0;
	}

	unsigned char GetLen()
	{
		return len;
	}

	void Flush()
	{
		len = 0;
	}
	
};

class Serial : public FIFO
{
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
	}
	
	unsigned char Available()
	{
		return GetLen();
	}
	
	unsigned char Read()
	{
		/* Wait for empty transmit buffer */
		while ( !( UCSR0A & (1<<UDRE0)) )
			;
		/* Put data into buffer, sends the data */
		UDR0 = data;
	}

	void Write( unsigned char c )
	{
		PutChar( c );
	}

};

Serial Ser0;

SIGNAL(USART0_RX_vect)
{
	unsigned char c  =  UDR0;
	Ser0.PutChar(c);
}



