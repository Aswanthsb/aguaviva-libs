#include <avr/io.h>
#include <util/delay.h>

//             RAM 62256
//            _________
// A14  A2  -|   |_|   |- +5  VCC
// A12  A3  -|         |- b0  /WE
// A7   A4  -|         |- B1  A13
// A6   A5  -|         |- B2  A8
// A5   A6  -|         |- B3  A9
// A4   A7  -|         |- B4  A11
// A3   C7  -|         |- B5  /OE
// A2   C6  -|         |- B6  A10
// A1   C5  -|         |- b7  /CE
// A0   C4  -|         |- D0  Q7
// Q0   C3  -|         |- D1  Q6  
// Q1   C2  -|         |- D2  Q5
// Q2   C1  -|         |- D3  Q4
//      GND -|_________|- D4  Q3
//

#define port(p,nn, n) DDR##p |=  (1<<nn); PORT##p &= ~(1<<nn); PORT##p |= (((a>>n)&1)<<nn);
#define inp(p,nn, n) DDR##p &= ~(1<<nn); PORT##p &= ~(1<<nn); d|= ((PIN##p >>nn)&1)<<n;

#define hiz(p,nn) DDR##p &= ~(1<<nn); PORT##p &= ~(1<<nn); 

#define CS_LOW {DDRB |= 1<<7;	PORTB &= ~(1<<7);}
#define CS_HIGH {DDRB |= 1<<7;	PORTB |= (1<<7);}

#define OE_LOW {DDRB |= 1<<5;	PORTB &= ~(1<<5);}
#define OE_HIGH {DDRB |= 1<<5;	PORTB |= (1<<5);}

#define WR_LOW {DDRB |= 0<<5;	PORTB &= ~(0<<5);}
#define WR_HIGH {DDRB |= 0<<5;	PORTB |= (0<<5);}


void Init62256()
{
	CS_LOW;
	WR_HIGH;    
}

void SetAddress62256(unsigned int a)
{
	//set aDDRess
	port(A,2, 14);
	port(A,3, 12);
	port(A,4, 7);
	port(A,5, 6);
	port(A,6, 5);
	port(A,7, 4);
	port(C,7, 3);
	port(C,6, 2);
	port(C,5, 1);
	port(C,4, 0);
	
	port(B,1, 13)
	port(B,2, 8)
	port(B,3, 9)
	port(B,4, 11)
	port(B,6, 10)	
}

unsigned char GetData62256()
{
	unsigned char d= 0;

	WR_HIGH;    

	OE_LOW;

	_delay_ms(1);	
	
	inp(C,3, 0);
	inp(C,2, 1);
	inp(C,1, 2);

	inp(D,0, 7);
	inp(D,1, 6);
	inp(D,2, 5);
	inp(D,3, 4);
	inp(D,4, 3);

	OE_HIGH;

	return d;
}

void SetData62256(unsigned char a)
{
	port(C,3, 0);
	port(C,2, 1);
	port(C,1, 2);

	port(D,0, 7);
	port(D,1, 6);
	port(D,2, 5);
	port(D,3, 4);
	port(D,4, 3);

	WR_LOW;
    
	_delay_ms(1);

	WR_HIGH;

	_delay_ms(1);

    
    hiz(C,3)
    hiz(C,2)
    hiz(C,1)

    hiz(D,0)
    hiz(D,1)
    hiz(D,2)
    hiz(D,3)
    hiz(D,4)    
}

unsigned char Read62256(unsigned int address)
{
	SetAddress62256(address);
	
	return GetData62256();
}

void Write62256(unsigned int address, unsigned char val)
{
	SetAddress62256(address);
	
	SetData62256(val);
}
