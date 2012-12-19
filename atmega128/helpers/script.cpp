#include "script.h"
#include "parser.h"
#include "expression.h"

#define NULL 0

static bool (*pfnProcessStatements)( char **p ) = NULL;
static Variables *var;

static bool ProcessVariableAssignment( char **p )
{
	ParseSpaces(p);
	int l = GetKeyWord( p );
	int *v = var->FindOrCreate(*p, l);
	(*p)+=l;

	if ( ParseToken(p,"++") )
	{
		(*v)++;
	}
	else if ( ParseToken(p,"--") )
	{
		(*v)--;
	}
	else if ( ParseToken(p,"+=") )
	{		
		int vv;
		if ( parseExpression( p, var, &vv ) == false)
			return false;
		(*v)+=vv;
	}
	else if ( ParseToken(p,"-=") )
	{		
		int vv;
		if ( parseExpression( p, var, &vv ) == false)
			return false;
		(*v)-=vv;
	}
	else
	{
		if (ExpectNoSpace(p,'=')==false)
			return false;

		if ( parseExpression( p, var, v ) == false)
			return false;
	}
	return true;
}

static bool Process( char **p )
{
	ParseSpaces(p);

	if ( Expect(p,'{') )
	{
		while( ExpectNoSpace(p, '}') == false) 
		{
			if ( Process( p ) == false)
				return false;			
		};				
		
		return true;
	}
	else if ( ParseToken(p, "for") )
	{
		int l = 0;

		char *cond; 
		char *inc; 
		char *clause=0; 
		if ( ExpectNoSpace(p,'(') )
		{
			// get var assignment
			ProcessVariableAssignment( p );
			ExpectNoSpace(p,';');

			// get condition
			ParseSpaces(p);
			cond = *p;
			int condition;
			if ( parseExpression( p, var, &condition ) == false)
				return false;
			ExpectNoSpace(p,';');

			// get increment
			ParseSpaces(p);
			inc = *p;
			while(**p!=')') (*p)++;
			ExpectNoSpace(p,')');

			//loop!
			while(condition)
			{
				clause = *p;
				if ( Process( &clause ) == false )
				{
					*p = clause;
					return false;
				}

				char *_inc = inc;
				if ( *inc != ')')
				{
					ProcessVariableAssignment( &_inc );
				}

				char *_cond = cond; 
				if ( parseExpression( &_cond, var, &condition ) == false)
				{
					*p = _cond;
					return false;
				}
			}

			FindClosingBracket( p );
		}
	}
	else if ( ParseToken(p, "if") )
	{
		ParseSpaces(p);
		int condition = 0;

		if ( ExpectNoSpace(p,'(') == false)
			return false;

		if ( parseExpression( p, var, &condition ) == false)
			return false;

		if ( ExpectNoSpace(p,')') == false)
			return false;

		if ( condition )
		{
			if ( Process( p ) == false)
				return false;
		}
		else
		{
			FindClosingBracket( p );
		}
		
		ParseSpaces(p);

		if ( ParseToken(p, "else") )
		{
			if (  !condition )
			{
				if ( Process( p ) == false)
					return false;
			}
			else
			{
				FindClosingBracket( p );
			}
		}
	}
	else 
	{
		if ( (*pfnProcessStatements)(p) ==false)
		{
			if ( ProcessVariableAssignment( p )== false)
				return false;
		}

		if ( ExpectNoSpace(p,';')== false)
			return false;
	}

	return true;
}

bool ProcessScript( char **p, bool (*fn)( char **p ), Variables *v )
{
	var = v;
	pfnProcessStatements  = fn;

	return Process(p);
}