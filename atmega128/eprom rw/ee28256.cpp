#include <avr/io.h>
#include <util/delay.h>


//          _________
//    A2  -|   |_|   |- +5
//    A3  -|         |- b0
//    A4  -|         |- B1
//    A5  -|         |- B2
//    A6  -|         |- B3
//    A7  -|         |- B4
//    C7  -|         |- B5
//    C6  -|         |- B6
//    C5  -|         |- b7
//    C4  -|         |- D0
//    C3  -|         |- D1
//    C2  -|         |- D2
//    C1  -|         |- D3
//    GND -|_________|- D4

#define aa(p,nn, n) DDR ## p |= (1<<nn); PORT##p &= ~(1<<nn); PORT##p |= (((a>>n)&1)<<nn);
#define dd(p,nn, n) DDR ## p &= ~(1<<nn); PORT ## p &= ~(1<<nn); d|= ((PIN ## p >>nn)&1)<<n;


#define CE_LOW  {DDRB |= 1<<7;  PORTB &= ~(1<<7);}
#define CE_HIGH {DDRB |= 1<<7;  PORTB |=  (1<<7);}

#define OE_LOW  {DDRB |= 1<<5;  PORTB &= ~(1<<5);}
#define OE_HIGH {DDRB |= 1<<5;  PORTB |=  (1<<5);}

#define WE_LOW  {DDRB |= 1<<0;  PORTB &= ~(1<<0);}
#define WE_HIGH {DDRB |= 1<<0;  PORTB |=  (1<<0);}

void SetAddress28256(unsigned int a)
{
	//set address
	aa(A,2, 14);	
	aa(A,3, 12);
	aa(A,4, 7);
	aa(A,5, 6);
	aa(A,6, 5);
	aa(A,7, 4);
	aa(C,7, 3);
	aa(C,6, 2);
	aa(C,5, 1);
	aa(C,4, 0);
	
	aa(B,1, 13)
	aa(B,2, 8)
	aa(B,3, 9)
	aa(B,4, 11)
	aa(B,6, 10)	
}
/*
void SetData28256(unsigned char a)
{
	aa(C,3, 0);
	aa(C,2, 1);
	aa(C,1, 2);

	aa(D,0, 7);
	aa(D,1, 6);
	aa(D,2, 5);
	aa(D,3, 4);
	aa(D,4, 3);
}

unsigned char GetData28256()
{
	unsigned char d= 0;

	dd(C,3, 0);
	dd(C,2, 1);
	dd(C,1, 2);

	dd(D,0, 7);
	dd(D,1, 6);
	dd(D,2, 5);
	dd(D,3, 4);
	dd(D,4, 3);

	return d;
}
*/

unsigned char GetData28256()
{
	unsigned char d= 0;

	WE_HIGH;
	
	CE_LOW;
	OE_LOW;
	
	_delay_ms(1);
	
	dd(C,3, 0);
	dd(C,2, 1);
	dd(C,1, 2);

	dd(D,0, 7);
	dd(D,1, 6);
	dd(D,2, 5);
	dd(D,3, 4);
	dd(D,4, 3);

	//OE_HIGH;
	//CE_HIGH;

	return d;
}

void SetData28256(unsigned char a)
{
	OE_HIGH;
	CE_LOW;
	WE_LOW;
	
	aa(C,3, 0);
	aa(C,2, 1);
	aa(C,1, 2);

	aa(D,0, 7);
	aa(D,1, 6);
	aa(D,2, 5);
	aa(D,3, 4);
	aa(D,4, 3);

	WE_HIGH;
	CE_HIGH;
	OE_LOW;
}


unsigned char Read28256(unsigned int a)
{
	SetAddress28256(a);
	
	return GetData28256();
}

void DataPolling()
{
	WE_HIGH;	//WE high
	CE_LOW;	//CE
	OE_HIGH;	//OE	
	
	//
	
	OE_LOW;	//OE

	unsigned char d= 0;

	while(d==0)
	{
		OE_HIGH;	//OE
		CE_HIGH;	//CE

		d=0;

		OE_LOW;	//OE
		CE_LOW;	//CE
		
		
		dd(D,0, 7);			
	}
	
}


int d1 = 0;
int d2 = 0;
void SetDelay(int _d1, int _d2)
{
	d1 = _d1;
	d2 = _d2;
}

void Write28256(unsigned int a, unsigned char v)
{
	SetAddress28256(a);
	
	a = v;

	aa(C,3, 0);
	aa(C,2, 1);
	aa(C,1, 2);

	aa(D,0, 7);
	aa(D,1, 6);
	aa(D,2, 5);
	aa(D,3, 4);
	aa(D,4, 3);
	
	//WE controlled
	
	OE_HIGH;
	
	CE_LOW;
	
	WE_LOW;

	//_delay_ms(1);

	WE_HIGH;
	_delay_ms(d1);
	
	CE_HIGH;
	
	OE_LOW;
	
	_delay_ms(d2);
}


unsigned char program28256(unsigned int a, unsigned char v)
{

	unsigned char d= 0;
/*
	//set data
	a = v;

	aa(C,3, 0);
	aa(C,2, 1);
	aa(C,1, 2);

	aa(D,0, 7);
	aa(D,1, 6);
	aa(D,2, 5);
	aa(D,3, 4);
	aa(D,4, 3);


	//pulse E 1ms
	CE_LOW;	//E
	_delay_ms(1);	
	CE_HIGH;	//E

	_delay_ms(2);	

	OE_LOW;	//G
	_delay_ms	(3);	

	//read data
	dd(C,3, 0);
	dd(C,2, 1);
	dd(C,1, 2);

	dd(D,0, 7);
	dd(D,1, 6);
	dd(D,2, 5);
	dd(D,3, 4);
	dd(D,4, 3);

	OE_HIGH;	//G

*/	
	return d;

}
