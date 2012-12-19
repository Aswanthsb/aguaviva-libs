#include "Serial0.h"

Serial0 Ser0;

SIGNAL(USART0_RX_vect)
{
	unsigned char c  =  UDR0;
	Ser0.PutChar(c);
}
