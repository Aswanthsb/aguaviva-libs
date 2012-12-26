#include <avr/io.h>
#include <util/delay.h>


//             RAM28256
//            _________
// A14  A2  -|   |_|   |- +5  VCC
// A12  A3  -|         |- b0  /WE
// A7   A4  -|         |- B1  A13
// A6   A5  -|         |- B2  A8
// A5   A6  -|         |- B3  A9
// A4   A7  -|         |- B4  A11
// A3   C7  -|         |- B5  /OE
// A2   C6  -|         |- B6  A10
// A1   C5  -|         |- b7  /CS
// A0   C4  -|         |- D0  Q7
// Q0   C3  -|         |- D1  Q6  
// Q1   C2  -|         |- D2  Q5
// Q2   C1  -|         |- D3  Q4
//      GND -|_________|- D4  Q3
//

#define hiz(p,nn) DDR##p &= ~(1<<nn); PORT##p &= ~(1<<nn);  

#define port(p,nn, n) DDR##p |=  (1<<nn); PORT##p &= ~(1<<nn); PORT##p |= (((a>>n)&1)<<nn);
#define inp(p,nn, n) d|= ((PIN##p >>nn)&1)<<n;


#define WR_LOW  {DDRB |= 1<<0;  PORTB &= ~(1<<0);}
#define WR_HIGH {DDRB |= 1<<0;  PORTB |=  (1<<0);}

#define OE_LOW  {DDRB |= 1<<5;  PORTB &= ~(1<<5);}
#define OE_HIGH {DDRB |= 1<<5;  PORTB |=  (1<<5);}

#define CS_LOW  {DDRB |= 1<<7;  PORTB &= ~(1<<7);}
#define CS_HIGH {DDRB |= 1<<7;  PORTB |=  (1<<7);}

static int d1 = 0;
static int d2 = 0;

void SetDelay(int _d1, int _d2)
{
	d1 = _d1;
	d2 = _d2;
}

void Init28256()
{
	WR_HIGH;    
	OE_HIGH;

	CS_LOW;
}

void SetAddress28256(unsigned int a)
{
	//set address
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

unsigned char GetData28256()
{
	unsigned char d= 0;

    hiz(C,3)
    hiz(C,2)
    hiz(C,1)

    hiz(D,0)
    hiz(D,1)
    hiz(D,2)
    hiz(D,3)
    hiz(D,4)    

	WR_HIGH;    

	OE_LOW;

    // from datasheet
    _delay_us(200);
	
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

void SetData28256(unsigned char a)
{
	OE_HIGH;

	port(C,3, 0);
	port(C,2, 1);
	port(C,1, 2);

	port(D,0, 7);
	port(D,1, 6);
	port(D,2, 5);
	port(D,3, 4);
	port(D,4, 3);

    //write pulse
	WR_LOW;	
	_delay_us(100);
	WR_HIGH;
    
    while ( GetData28256() != a );
}

unsigned char Read28256(unsigned int a)
{
	SetAddress28256(a);
	
	return GetData28256();
}

void Write28256(unsigned int a, unsigned char v)
{
	SetAddress28256(a);
    SetData28256(v);
}
