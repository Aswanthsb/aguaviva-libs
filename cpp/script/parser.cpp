#include <string.h>

bool Expect(char **ps, char c)
{
	if ( **ps == c )
	{
		(*ps)++;
		return true;
	}
	return false;
}

char GetChar( char **ps)
{
	char c = **ps;
	
	if ( c != 0 )
		(*ps)++;
	
	return c;
}

bool ParseSpaces( char **ps)
{
	char *s = *ps;
	int i=0;
	for(;s[i]==' ';i++);
	
	(*ps)+= i;
	return i>0;
}

bool ExpectNoSpace(char **ps, char c)
{
	ParseSpaces(ps);
	return Expect(ps, c);
}


bool ParseToken( char **ps, char * token )
{
	char *s = *ps;
	
	int i=0;
	
	for(;token[i]!=0;i++)
	{
		if (s[i]!=token[i])
		return false;
	}
	
	(*ps) += i;
	
	return true;
}

int GetKeyWord( char **ps )
{
	int i=0;
	char *p = *ps;
	while( 1)
	{
		
		if ((*p>='a' && *p<='z') || (*p>='A' && *p<='Z'))
		{
			i++;
			p++;
		}
		else
			break;
	}
	return i;
}

unsigned char IsNumber( char c )
{
	return ( c >= '0' ) && ( c <= '9' );
}

bool ParseInt( char **ps, int *out)
{
	int val = 0;

	bool sign = Expect(ps, '-');

	char *s = *ps;

	int i = 0;
	for(;IsNumber(*s);i++)
	{
		val = ( val * 10 ) + (*s-'0');
		s++;
	}
		
	if (i>0)
	{
		(*ps) +=i;
		if (sign) 
			val = -val;
			
		*out = val;
		return true;
	}			
	
	return false;	
}

bool FindClosingBracket( char **pp )
{
	int c = 0;
	char *p = *pp;

	for(int i=0;;i++)
	{
		if (*p==0)
		{
			return false;
		}
		else if (*p=='{')
		{
			c++;
		}
		else if (*p=='}')
		{
			c--;
			if ( c==0)
			{
				(*pp)+=i+1;
				return true;
			}
		}
		else if (*p==';')
		{
			if ( c==0)
			{
				(*pp)+=i+1;
				return true;
			}
		}
		p++;
	}
}