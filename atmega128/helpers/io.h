#ifndef _IOSTUFF_
#define _IOSTUFF_

class IO
{
public:
    void Init() { };
    virtual void Write(unsigned int address, unsigned char data) {};
    virtual unsigned char Read(unsigned int address) { return 0; };
};


#endif