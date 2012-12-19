filename = "mytest.gb"
data = open(filename, "rb").read()

out = ""

index = 0;
        
for i in range(0, len(data), 16):

    if (i & 0x3fff) == 0:        
        if ( index != 0 ):
            out+="};"
        out+="\n"
        out += "static PROGMEM prog_uchar data%i[] = {" % index
        index+=1

    if (i & 0xff) == 0:
        out+="\n"
        
    for c in data[i:i+16]:
        out += ("0x%02x, " % ord(c))
    out+="\n"
out+="};"
        
open(filename+".h", "wb").write(out)
