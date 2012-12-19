#include <avr/io.h>
#include <util/delay.h>

// +5 b0 b1 b2 b3 b4 b5 b6 b7 d0 d1 d2 d3 d4
// |                                        |
// |                                        |
//  )                                       |
// |                                        |
// |________________________________________|
//  A2 A3 A4 A5 A6 A7 c7 c6 c5 c4 c3 c2 c1 gnd


#define aa(p,nn, n) DDR ## p |= (1<<nn); PORT##p &= ~(1<<nn); PORT##p |= (((a>>n)&1)<<nn);
#define dd(p,nn, n) DDR ## p &= ~(1<<nn); PORT ## p &= ~(1<<nn); d|= ((PIN ## p >>nn)&1)<<n;

void SetAddress27512(unsigned int a)
{
	aa(A,2, 15);
	aa(A,3, 12);
	aa(A,4, 7);
	aa(A,5, 6);
	aa(A,6, 5);
	aa(A,7, 4);
	aa(C,7, 3);
	aa(C,6, 2);
	aa(C,5, 1);
	aa(C,4, 0);
	
	aa(B,0, 14)
	aa(B,1, 13)
	aa(B,2, 8)
	aa(B,3, 9)
	aa(B,4, 11)

	//   CE
	DDRB |= 1<<5;	PORTB &= ~(1<<5);
	
	aa(B,6, 10)

	//   OE
	DDRB |= 1<<7;	PORTB &= ~(1<<7);
}

unsigned char GetData27512()
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


unsigned char Read27512(unsigned int a)
{
	SetAddress27512(a);
	
	return GetData27512();
}