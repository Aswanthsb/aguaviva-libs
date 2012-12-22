
unsigned char Read27256(unsigned int a);

class E27256 : public IO
{
public:
    virtual unsigned char Read(unsigned int address) { return Read27256(address); };
};