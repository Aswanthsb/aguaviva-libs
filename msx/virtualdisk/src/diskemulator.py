import sys
import serial
import md5

sys.stdout.softspace=False

def LoadFile( filename ):
    f = open(filename, "rb")
    data = f.read()
    f.close()
    return data

def SaveFile( filename, data ):
    f = open(filename, "wb")
    f.write(data)
    f.close()
    
def WriteByte( b ):
	ser.write("%c" % b);
	
def ComputeCRC(data):
	crc = len(data)
	for i in data:
		crc ^= ord(i)
	return crc

def ReceivePacket():
	while (True ):
		c = ser.read(1)
		#print c
		if (c == 'w'):
			c = ser.read(1)
			#print c
			if (c== 'r'):
				break

	length = ord(ser.read(1))
	data =  ser.read(length)
	crc= ComputeCRC(data)

	if ( ord(ser.read(1))==crc):
		ser.write(":)")
		return data
	print "*"
	return "error"

def SendSubPacket(ser, data):
	for i in range(3):
		ser.write("rd")
		WriteByte(len(data))
		ser.write(data)
		
		crc= ComputeCRC(data)
		WriteByte(crc)

		res = ser.read(2)
		if  (res==":)"):
			return True
		else:
			sys.stdout.write("*")
			# forces syncing
			ser.write("***********************************************************")

	return False
	
def SendPacket(ser, data):
	chunksize = 32

	off = 0
	while( off< len(data) ):
		rest = len(data)-off
		if (  rest > chunksize):
			rest = chunksize
		block = data[off:off+rest]
		if ( SendSubPacket(ser, block) == True):
			off += rest
			sys.stdout.write("#")
			sys.stdout.flush()
		else:
			print "cant recover!"

	ser.write("rd")
	WriteByte(0)
	WriteByte(0)

	
def CommandLoop(ser,dsk):
	print "Starting disk drive..."
	cmdCnt = 0
	while(True):
		print "%3i: " %cmdCnt,
		data = ReceivePacket()

		if data =="INIHRD":
			print data," ",
			SendPacket(ser, "files\r")
			print
		elif data =="DSKCHG":
			print data
			WriteByte(0xff)
		elif data[0:2] =="io":
			print "DSKIO",
			numsectors = ord(data[5])
			sectorini = ord(data[6]) + ord(data[7])*256
			mem = ord(data[8]) + ord(data[9])*256
			print  "%i %i %04X, " % (sectorini, numsectors,mem),

			print " ", numsectors/2.0, "Kb.. ",	

			data = dsk[sectorini*512:(sectorini+numsectors)*512]
			SendPacket(ser, data)
			print
		else:
			print 
		cmdCnt=cmdCnt+1


if (sys.argv[1] ==""):
	print "usage %s <image.dsk>" % sys.argv[0]
dsk = LoadFile(sys.argv[1])

ser = serial.Serial("/dev/ttyUSB0", 115200, bytesize=8, parity='N', stopbits=1,  timeout=3)

print ser

CommandLoop(ser,dsk)
ser.close()


