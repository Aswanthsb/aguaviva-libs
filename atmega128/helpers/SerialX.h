#ifndef _SERIALX_
#define _SERIALX_


#define RX_BUFFER_SIZE 32

class FIFO
{
	volatile unsigned char buffer[RX_BUFFER_SIZE];
	volatile unsigned char ini;
	volatile unsigned char len;
public:	
	FIFO()
	{
		ini = 0;
		len = 0;
	}

	void PutChar(const unsigned char c)
	{
		if (len < (RX_BUFFER_SIZE-1)  )
		{
			buffer[ (ini + len) & (RX_BUFFER_SIZE-1) ] = c;
			len++;
		}
	}

	unsigned char GetChar()
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
public:
	void Begin() {};
	
	unsigned char Available()
	{
		return GetLen();
	}
	
	unsigned char Read()
	{	
		return GetChar();
	}

	virtual void Write( const unsigned char c ) {};

	void Write( const char *str )
	{
		while( *str != 0 )
		{
			Write( *str++ );
		}
	}

};


#endif