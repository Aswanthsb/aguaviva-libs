void Init62256();
unsigned char Read62256(unsigned int a);
void Write62256(unsigned int address, unsigned char val);

class RAM62256 : public IO
{
public:
    void init() { Init62256(); }
    unsigned char Read(unsigned int address) { return Read62256(address); };
    void Write(unsigned int address, unsigned char val) { return Write62256(address, val); };
};