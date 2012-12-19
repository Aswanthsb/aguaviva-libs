/*
 * variables.h
 *
 *  Created on: Apr 8, 2012
 *      Author: raguaviv
 */

#ifndef VARIABLES_H_
#define VARIABLES_H_
//#include "common.h"
#define boolean bool
#define byte unsigned char

class Variables
{
	char variables[255];
	char *pLastVar;
public:
	Variables();
	int *Find(char *name, byte len);
	int *FindOrCreate(char *name, byte len);
	void Set(char *name, byte len, int value);
	void Set(char *name, int value);
	void List();

	void SetPreamble(void *pr);
	void *GetPreamble();
};

#endif /* VARIABLES_H_ */
