import serial
import md5



def GetFile( filename ):
    f = open(filename, "rb")
    data = f.read()
    f.close()
    
    return data


def UploadData( data, address ):
    
    ser.write( "r" )
    ser.write( "%c" % ((address>>8) & 0xff) )
    ser.write( "%c" % (address & 0xff) )

    errors = 0
    p = 0;
    print p, len(data)
    while( p < len(data) ):

        remaining = len(data) - p

        block = 255;
        if ( block > remaining):
            block = remaining
            
        print remaining, block, 
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
            p+= block
            print r
        else:
            print "bad", r
            #break
            
    print "done!"		
    ser.write( "%c" % 0 )		
    return p
    
def UploadFile( filename, address ):
    data = GetFile(filename)
    return  UploadData( data, address )
 
def DownloadData( address, size ):
       
    out = '';
    ser.write( "s" )
    ser.write( "%c" % ((address>>8) & 0xff) )
    ser.write( "%c" % (address & 0xff) )

    ser.write( "%c" % ((size>>8) & 0xff) )
    ser.write( "%c" % (size & 0xff) )

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
            print i, "g"
        else:
            print "Error"
            
    print "done"
    return out


def DownloadFile( filename, address, size ):   
    f = open(filename, "wb")
    f.write( DownloadData(address, size ) )
    f.close()
    

def main():   

    ser.write( "?")
    if ser.read(2) == ":)":
        print "avr ready"     
        
        data = GetFile("tetris.gb")
        p = UploadData( data,0 )
        print p
        ver = DownloadData( 0, p) 
        

        m1 = md5.new(data).digest()
        m2 = md5.new(ver).digest()

        if (m1 == m2 ):
            print "transfer OK!"
        else:
            print "error!!"
        
               
    else:
        print "cant initialize"

ser = serial.Serial("COM18", 19200,  timeout=100)

while(True):
    c = ser.read(1)
    
    print ord(c), c,
    
    if ( ord(c)==0x1b):
        print "esc",
    elif ( c=="S"):
        ser.write("AVRBOOT")
    elif ( c=="V"):
        ser.write("08")
    elif ( c=="a"):
        ser.write("Y")
    else:
        print ord(c),
    print


ser.close()
     