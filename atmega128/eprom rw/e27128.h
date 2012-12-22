unsigned char Read27128(unsigned int a);


class E27128 : public IO
{
public:
    virtual unsigned char Read(unsigned int address) { return Read27128(address); };
};
