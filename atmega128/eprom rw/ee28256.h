unsigned char Read28256(unsigned int a);

class EE28256 : public IO
{
public:
    virtual unsigned char Read(unsigned int address) { return Read28256(address); };
};
