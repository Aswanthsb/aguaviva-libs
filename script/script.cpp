#include "script.h"
#include "parser.h"
#include "expression.h"

#define NULL 0

static bool (*pfnProcessStatements)( char **p ) = NULL;
static Variables *var;
static Variables funcs;
static Variables *functions = &funcs;

static bool Process( char **p, int *res );
static bool ProcessLoop( char **p, int *res );

static bool ProcessVariableAssignment( char **p )
{
	ParseSpaces(p);
	int l = GetKeyWord( p );
	int *v = var->Find(*p, l);
	if ( v != NULL )
	{
		(*p)+=l;

		if ( ParseToken(p,"++") )
		{
			(*v)++;
			return true;
		}
		else if ( ParseToken(p,"--") )
		{
			(*v)--;
			return true;
		}
		else if ( ParseToken(p,"+=") )
		{		
			int vv;
			if ( parseExpression( p, var, &vv ) == false)
				return false;
			(*v)+=vv;
			return true;
		}
		else if ( ParseToken(p,"-=") )
		{		
			int vv;
			if ( parseExpression( p, var, &vv ) == false)
				return false;
			(*v)-=vv;
			return true;
		}
	}
	else
	{
		v = var->Create(*p, l);
		(*p)+=l;
	}

	if (ExpectNoSpace(p,'=')==false)
		return false;

	if ( parseExpression( p, var, v ) == false)
		return false;

	return true;
}

static bool ProcessFunction( char **p, int *res )
{
	ParseSpaces(p);
	int l = GetKeyWord( p );

	char *pp = (*p)+l;
	if ( *pp !='(' )
	{
		return false;
	}

	int *v = functions->Find(*p, l);
	if ( v!= NULL)
	{
		(*p)+=l;
		Expect(p,'(');

		void *pre = var->GetPreamble();

		char *fp = (char *)(*v);

		for(;;)
		{
			int vv;
			if ( parseExpression( p, var, &vv ) == false)
				return false;

			int fl = GetKeyWord( &fp );
			*(var->FindOrCreate( fp, fl)) = vv;
			fp += fl;

			if ( ExpectNoSpace(&fp,')') )
			{
				if ( ExpectNoSpace(p,')') )
				{
					break;
				}
				return false;
			}

			if ( Expect(&fp,',') )
			{
				if ( Expect(p,',') == false )
					return false;
			}
		}

		if ( ProcessLoop( &fp, res ) == false )
			return false;

		if ( ExpectNoSpace(p,';')== false)
			return false;

		var->SetPreamble(pre);
		
		return true;
	}
	else
	{
		v = functions->Create(*p, l);
		(*p)+=l;		

		Expect(p,'(');

		*v = (int)(*p);		
		for(;;)
		{
			int l = GetKeyWord( p );
			(*p)+=l;

			if ( ExpectNoSpace(p,',') )
				continue;

			if ( ExpectNoSpace(p,')') )
			{
				return FindClosingBracket( p );
			}
		}
	}

	return false;
}

static bool ProcessLoop( char **p, int *res )
{
	int c = 0;

	for(;;)
	{
		if ( ExpectNoSpace( p,'{') )
		{
			c++;
		}

		if ( ExpectNoSpace( p,'}') )
		{
			c--;

			if ( c==0 )
				return true;
		}

		ParseSpaces(p);
		if ( ParseToken(p, "return") )
		{
			if (res!= NULL)
			{
				*res = 0;
			}
			return true;
		}

		if ( Process( p, res ) == false)
			return false;			

		if ( c==0 )
			return true;
	}
}

static bool Process( char **p, int *res )
{
	ParseSpaces(p);
	
	if ( ParseToken(p, "for") )
	{
		int l = 0;

		char *cond; 
		char *inc; 

		if ( ExpectNoSpace(p,'(') == false)
			return false;

		// get var assignment
		ProcessVariableAssignment( p );
		if ( ExpectNoSpace(p,';') == false)
			return false;

		// get condition
		ParseSpaces(p);
		cond = *p;
		int condition;
		if ( parseExpression( p, var, &condition ) == false)
			return false;

		if ( ExpectNoSpace(p,';') == false)
			return false;

		// get increment
		ParseSpaces(p);
		inc = *p;
		while(**p!=')') (*p)++;
		ExpectNoSpace(p,')');

		if ( condition == false)
		{
			return FindClosingBracket( p );
		}

		//loop!
		for(;;)
		{
			char *clause = *p;
			if ( ProcessLoop( &clause, NULL ) == false )
			{
				*p = clause;
				return false;
			}

			if ( *inc != ')')
			{
				char *_inc = inc;
				if ( ProcessVariableAssignment( &_inc ) == false)
				{
					*p = _inc;
					return false;
				}
			}

			char *_cond = cond; 
			if ( parseExpression( &_cond, var, &condition ) == false)
			{
				*p = _cond;
				return false;
			}

			if ( condition == false)
			{
				*p = clause;
				return true;
			}
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
			if ( ProcessLoop( p, NULL ) == false)
				return false;
		}
		else
		{
			if ( FindClosingBracket( p ) == false )
			{
				return false;
			}
		}
		
		ParseSpaces(p);

		if ( ParseToken(p, "else") )
		{
			if (  !condition )
			{
				return ProcessLoop( p, res );
			}
			else
			{
				return FindClosingBracket( p );
			}
		}
	}
	else 
	{
		if ( (*pfnProcessStatements)( p ) == true)
		{
			if ( ExpectNoSpace(p,';')== false)
				return false;
		}
		else
		{
			if ( ProcessFunction( p, res ) == false)
			{
				if ( ProcessVariableAssignment( p )  == false )
					return false;

				if ( ExpectNoSpace(p,';') == false )
					return false;
			}
			
		}
	}

	return true;
}

bool ProcessScript( char **p, bool (*fn)( char **p ), Variables *v )
{
	var = v;
	pfnProcessStatements  = fn;

	int res;
	return ProcessLoop(p, &res);
}