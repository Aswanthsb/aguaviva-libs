#ifndef PARSER
#define PARSER

char GetChar( char **ps);
bool ParseSpaces( char **ps);
bool ParseToken( char **ps, char * token );
unsigned char IsNumber( char c );
bool ParseInt( char **ps, int *out);
bool Expect(char **pc, char c);
bool ExpectNoSpace(char **ps, char c);
void FindClosingBracket( char **pp );
int GetKeyWord( char **ps );

#endif