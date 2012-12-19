#ifndef BINARYFILE
#define BINARYFILE

bool ReadFile(char *fileName, void **pOut, unsigned int *pSize);
bool WriteFile(char *fileName, unsigned char *pOut, unsigned int fileLen);

#endif