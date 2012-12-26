import serial
import md5
import msvcrt

def LoadFile( filename ):
    f = open(filename, "rb")
    data = f.read()
    f.close()
    
    return data

def SaveFile( filename, data ):
    f = open(filename, "wb")
    f.write(data)
    f.close()
    
def ProgressBar( i, total ):

    th = i*60/total;
    
    o= ""
    for i in range(0,60):
        if ( i<th):
            o+="#"
        else:
            o+="."
    print o,"\r",

def UploadData( data, address):
      
    ser.write( "r" )
    ser.write( "%c" % ((address>>8) & 0xff) )
    ser.write( "%c" % (address & 0xff) )

    size = len(data)     

    print "Writing %i bytes @ address %i" % ( size, address)

    errors = 0
    p = 0;
    while( p < size ):

        remaining = size - p

        block = 16;#255;
        if ( block > remaining):
            block = remaining
                
        ser.write( "%c" % block )
            
        checksum = 0
        str = ""
        for i in range( p, p + block):
            checksum += ord(data[i])
            ser.write( data[i] )
            
            #str += "%02X " % ord(b[i])
            
        ser.write( "%c" % ( checksum & 0xff ) )
        
        #print(str, checksum & 0xff)
        r = ser.read(1)
        if ( r == "g"):	
            p += block
            ProgressBar(p,size)
        else:
            print
            print remaining, block, 
            if ( r!= 'b'):
                for i in range(0,255):
                    ser.write( "%c" % 0 )
                print "sync:", ser.read(1)
                return p
            print "bad", r
            #break
            
    print "\nok!"		
    ser.write( "%c" % 0 )		
    return p
    
def UploadFile( filename, address ):
    data = LoadFile(filename)
    return  UploadData( data, address )
 
def DownloadData( address, size ):
       
    out = '';
    ser.write( "s" )
    ser.write( "%c" % ((address>>8) & 0xff) )
    ser.write( "%c" % (address & 0xff) )

    ser.write( "%c" % ((size>>8) & 0xff) )
    ser.write( "%c" % (size & 0xff) )
    
    print "Reading %i bytes @ address %i" % ( size, address)

    while(True):
        l = ord(ser.read(1))
        if l==0:
            break            
        checksum = 0
        for i in range(0,l):
            c = ser.read(1)
            out += c
            checksum += ord(c)

        if ( ord(ser.read(1)) == ( checksum & 0xff ) ):
            ser.write( "g" )            
            ProgressBar(len(out),size)
        else:
            print "Error"
            
    print "\nok"
    return out


def DownloadFile( filename, address, size ):   
    SaveFile( filename, DownloadData(address, size ) )
    

def CompareMD5( a,b):
    m1 = md5.new(a).digest()
    m2 = md5.new(b).digest()
    return ( m1 == m2 )

def VerifyData(address, data):
    ver = DownloadData( address, len(data)) 

    print "Data verified"
    
    for i in range(0,len(data)):
        if ( data[i]!= ver[i]):
            print "Error @ %x expected %x found %x" % ( i, ord(data[i]), ord(ver[i]) )
            
    if CompareMD5(data,ver):
        print "OK!"
    else:
        print "error!!"

def Test():
    ser.write('t')
    s = ser.read(255)
    print "Got %i bytes" % len(s)
    print s

def IsReady():
    init = False
    for i in range(0,4):
        ser.flush()
        ser.write( "?")
        tmp = ser.read(2) 
        if tmp == ":)":
            init = True
            break
    
    if init == False:
        print "cant initialize!"
        print "received '%s'" % tmp

    return init


def main():   
    from optparse import OptionParser
    
    parser = OptionParser("%prog [ -u | -d ] <file> -a <address> -s <size>")

    parser.add_option("-a", type=int, default=0, dest="address", help="lookback field size in bits")
    parser.add_option("-s", type=int, dest="size", help="length field size in bits")
    parser.add_option("-d", type=str, dest="download", help="name for generated C array")
    parser.add_option("-u", type=str, dest="upload", help="name for generated C array")    
    parser.add_option("-v", type=str, dest="verify", help="name for generated C array")    
    options, args = parser.parse_args()

    if IsReady()==False:
        return;
        
    print "avr ready"     
        
    if options.upload != None:
        data = LoadFile(options.upload)
     
        if options.size !=None:
            data = data[0:options.size]
    
        p = 0;
        while( p < len(data) ):
            if IsReady()==False:
                return;
            p += UploadData( data[p:], options.address + p )
        
        VerifyData( options.address, data)
               
        return
        
    elif options.verify != None:
        data = LoadFile(options.verify)
     
        if options.size !=None:
            data = data[0:options.size]
            
        VerifyData( options.address, data)
        return
    
    elif options.download != None:
    
        data = DownloadData( options.address, options.size) 
        SaveFile(options.download, data)
        return
    
    print "need an input or output file!"

    return
    
    
ser = serial.Serial("COM4", 19200, bytesize=8, parity='N', stopbits=1,  timeout=1)

print "press a key to download"
msvcrt.getch()

main()

#ser.write( "x" )		

ser.close()
