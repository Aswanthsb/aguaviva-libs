// written by aguaviva@gmail.com
// no comments, sorry :)

const int EnableWait = 13;       // pin that the LED is attached to
const int IsWaiting = 12;       // PB4
const int IsWriting = 11;       // PB3
const int IsReading = 10;       // PB2

void PreRead()
{
  for(int i=0;i<8;i++)
  { 
    pinMode(i+2, INPUT);
    digitalWrite(i+2, LOW);
  }
}

void PreWrite()
{
  for(int i=0;i<8;i++)
  { 
    pinMode(i+2, OUTPUT);
  }
}
 
void LoopWhileNotWaiting()
{
  while( digitalRead(IsWaiting)==HIGH );
}

void WaitOnIO()
{
  //digitalWrite(EnableWait, HIGH);
  PORTB |= (1<<5);
}

void Release()
{
    PORTB &= ~(1<<5);  
}

void Write(byte b)
{
  WaitOnIO();
  LoopWhileNotWaiting();
  
  PreWrite();
  for(int i=0;i<8;i++)
  { 
    digitalWrite(i+2, (((b>>i)&1)==0)?LOW:HIGH);
  } 
  
  Release();  
  
  //loop wile z80 is reading
  while( (PINB & (1<<2)) == 0 );
    
  
  WaitOnIO();
}

byte Read()
{
  WaitOnIO();
  LoopWhileNotWaiting();

  PreRead();
  
  byte b=0;
  for(int i=0;i<8;i++)
  { 
    b >>=1;
    b |= (digitalRead(i+2)==HIGH)?128:0;
  } 
  
  Release();

  //loop wile z80 is writing
  while( (PINB & (1<<3)) == 0 );

  WaitOnIO();
  
  return b;
}

boolean IsMSXReading()
{
  return digitalRead(IsReading)==0;
}

boolean IsMSXWriting()
{
  return digitalRead(IsWriting)==0;
}

void setup() 
{
  // initialize serial communications:
  Serial.begin(115200);
  //Serial.begin(250000*4);

  for(int i=0;i<8;i++)
  {  
    pinMode(i+2, INPUT);
    digitalWrite(i+2, LOW);
  } 

  pinMode(EnableWait, OUTPUT);
  digitalWrite(EnableWait, LOW);

  pinMode(IsWaiting, INPUT);
  digitalWrite(IsWaiting, LOW);

  pinMode(IsReading, INPUT);
  digitalWrite(IsReading, LOW);

  pinMode(IsWriting, INPUT);
  digitalWrite(IsWriting, LOW);

  WaitOnIO();
}


void loop()
{
  LoopWhileNotWaiting();
  
  if ( IsMSXReading() && Serial.available()!=0)
  {
    Write( Serial.read() );
  }    
  else if ( IsMSXWriting() )
  {
    Serial.write(Read() );     
  }
}

