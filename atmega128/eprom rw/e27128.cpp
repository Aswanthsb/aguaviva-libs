#include <avr/io.h>
#include <util/delay.h>

//             _________
// VPP   A2  -|   |_|   |- +5  VCC
// A12   A3  -|         |- b0  /P
// A7    A4  -|         |- B1  A13
// A6    A5  -|         |- B2  A8
// A5    A6  -|         |- B3  A9
// A4    A7  -|         |- B4  A11
// A3    C7  -|         |- B5  /G
// A2    C6  -|         |- B6  A10
// A1    C5  -|         |- b7  /E
// A0    C4  -|         |- D0  Q7
// Q0    C3  -|         |- D1  Q6
// Q1    C2  -|         |- D2  Q5
// Q2    C1  -|         |- D3  Q4
// GND   GND -|_________|- D4  Q3


#define aa(p,nn, n) DDR ## p |= (1<<nn); PORT##p &= ~(1<<nn); PORT##p |= (((a>>n)&1)<<nn);
#define dd(p,nn, n) DDR ## p &= ~(1<<nn); PORT ## p &= ~(1<<nn); d|= ((PIN ## p >>nn)&1)<<n;

#define E_LOW {DDRB |= 1<<7;	PORTB &= ~(1<<7);}
#define E_HIGH {DDRB |= 1<<7;	PORTB |= (1<<7);}

#define G_LOW {DDRB |= 1<<5;	PORTB &= ~(1<<5);}
#define G_HIGH {DDRB |= 1<<5;	PORTB |= (1<<5);}


void SetAddress27128(unsigned int a)
{
	//aa(A,2, 15);
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

unsigned char GetData27128()
{
	unsigned char d= 0;
	
	G_LOW;
	E_LOW;	

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


unsigned char Read27128(unsigned int a)
{
	SetAddress27128(a);
	
	return GetData27128();
}