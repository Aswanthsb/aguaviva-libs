void Init28256();
unsigned char Read28256(unsigned int a);
unsigned char Write28256(unsigned int a, unsigned char val);
void SetDelay(int _d1, int _d2);

class EE28256 : public IO
{
public:
    EE28256() { Init28256(); }
    unsigned char Read(unsigned int address) { return Read28256(address); };
    void Write(unsigned int address, unsigned char val ) { Write28256(address, val); };
};
