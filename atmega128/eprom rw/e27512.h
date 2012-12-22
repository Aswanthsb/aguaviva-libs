void SetAddress27512(unsigned int a);
unsigned char GetData27512();
unsigned char Read27512(unsigned int a);

class E27512 : public IO
{
public:
    virtual unsigned char Read(unsigned int address) { return Read27512(address); };
};