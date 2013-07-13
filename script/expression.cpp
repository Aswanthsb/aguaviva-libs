#include "parser.h"
#include "variables.h"

static bool error;
static char *p;
static Variables *pVar;

int parseExpression() 
{
	int v = 0;

	while (1) 
	{
		if ( error )
			return 0;

		ParseSpaces(&p);

		char tok = *p;

		if (Expect(&p, '(')) 
		{
			v = parseExpression();

			if ( ExpectNoSpace(&p, ')') == false) 
			{
				error = true;
				return 0;
			}
		}
		else 
		{			
			if ( ParseInt(&p, &v ) == false )
			{
				int l = GetKeyWord( &p );

				int *pv = pVar->Find(p, l);
				if ( pv == 0)
				{
					error = true;
					return 0;
				}
				p+=l;
				v = *pv;
			}
		}

		ParseSpaces(&p);
		while(1)
		{
			switch (*p) 
			{
				case '*':
					p++;
					v *= parseExpression();
					break;
				case '/':
					p++;
					v /= parseExpression();
					break;
				case '<':
					p++;
					v = v < parseExpression();
					break;
				case '>':
					p++;
					v = v > parseExpression();
					break;
				case '+':
					p++;
					v += parseExpression();
					break;
				case '-':
					p++;
					v -= parseExpression();
					break;
				default:
					return v;
					break;
			}
		}
	}

	//never reached
	return 0;
}

bool parseExpression(char **ptr, Variables *var, int *out) 
{
	error = false;
	pVar = var;
	p = *ptr;

	int v = parseExpression();
	*ptr = p;

	if ( error == false)
		*out = v;

	return !error;
}
