
unsigned char GetAddress2732(unsigned int a)
{
	unsigned char d= 0;

	DDRC |= 1<<2;	PORTC &= ~(1<<2);  //negative
	DDRB |= 1<<3;	PORTB &= ~(1<<3);  // /G
	DDRB |= 1<<5;	PORTB &= ~(1<<5);  // /E
	
	aa(A,2, 7);
	aa(A,3, 6);
	aa(A,4, 5);
	aa(A,5, 4);
	aa(A,6, 3);
	aa(A,7, 2);
	aa(C,7, 1);
	aa(C,6, 0);
	aa(B,0, 8)
	aa(B,1, 9)
	aa(B,2, 11)
	//DDRB |= 1<<3;	PORTB &= ~(1<<3);
	aa(B,4, 10)

	dd(C,5, 0);
	dd(C,4, 1);
	dd(C,3, 2);
	//DDRC |= 1<<2;	PORTC &= ~(1<<2);  //negative

	//DDRB |= 1<<5;	PORTB &= ~(1<<5);
	dd(B,6, 7);
	dd(B,7, 6);
	dd(D,0, 5);
	dd(D,1, 4);
	dd(D,2, 3);
	
	return d;
}

unsigned char program2732(unsigned int a, unsigned char v)
{
	unsigned char d= 0;

	DDRC |= 1<<2;	PORTC &= ~(1<<2);  //negative

	DDRB |= 1<<5;	PORTB |= (1<<5);  // E hight
	
	//set address
	aa(A,2, 7);
	aa(A,3, 6);
	aa(A,4, 5);
	aa(A,5, 4);
	aa(A,6, 3);
	aa(A,7, 2);
	aa(C,7, 1);
	aa(C,6, 0);
	aa(B,0, 8)
	aa(B,1, 9)
	aa(B,2, 11)
	aa(B,4, 10)

	//set data
	a = v;
	aa(C,5, 0);
	aa(C,4, 1);
	aa(C,3, 2);
	aa(B,6, 7);
	aa(B,7, 6);
	aa(D,0, 5);
	aa(D,1, 4);
	aa(D,2, 3);
	

	// pulse E
	DDRB |= 1<<5;	PORTB &= ~(1<<5);  // E low
	_delay_ms(1);	
	DDRB |= 1<<5;	PORTB |= (1<<5);  //  E high
	
	return d;
}
