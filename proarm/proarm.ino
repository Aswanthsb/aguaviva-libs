// BSY ERR --- --- GND
// ACK -7- -6- -5- -4- -3- -2- -1- -0- -S-

boolean SendData( char b )
{
  //wait for busy
  for(int i=0;digitalRead(11)==HIGH;i++);

  if ( b=='.')
    b = 10;

  for(int i=0;i<7;i++)
  {
    digitalWrite(i+3, ((b&1)==1)?HIGH:LOW);
    b>>=1;
  }  
  
  digitalWrite(2, HIGH);     
  //delay(1);
  digitalWrite(2, LOW);      
  //delay(1);
  
  return (digitalRead(12) == HIGH);
}

void SendString( char *str )
{
  for(int i=0;i<str[i]!=0;i++)
  {  
    char c = str[i];

    //Serial.print(c); 

    if ( SendData( c ) )  
    {
      Serial.println( "<ERR>");
      return;
    }
  }
}

char cmd_buf[256];

// calibrar robot en L

float ff1=0;      // +3
float ff2=90;      // +3
float ff3=90;      // 0->45  , 0->24

void forwardK(float f1, float f2, float f3)
{
  float d1 = f1-ff1;
  float d2 = f2-ff2;  
  float d3 = -(f3-ff3);  
  
  int a1 = map(d1,0,90,0,-750);
  int a2 = map(d2,0,90,0,750);  
  int a3 = map(d3 + map(d2,0,45,0,24),0,90,0,900) ;
  
  sprintf(cmd_buf,"M%i,%i,%i,0,0,0.",a1,a2,a3);
  SendString(cmd_buf);
  //Serial.println( cmd_buf );  
  
  ff1+=d1;
  ff2+=d2;  
  ff3+=-d3;  
}

void InverseK(float x, float y, float z)
{
  float L1 = 21;
  float L2 = 19.8;
  float L3 = 15;
  float L4 = 10;
  
  float f1 = atan2(y,x);
  float c3 = (x*x + y*y + (z-L1)* (z-L1) - L2*L2 - L3*L3) / (2.0*L2*L3);
  float s3 = sqrt(1.0-c3*c3);
  float f3 = atan2(s3,c3);

  float f2 = -(atan2( -z+L1,sqrt(x*x+y*y) ) - atan2(L3*s3,L2+L3*c3)); 
  
  //forwardK(f1, f2, f3);
  Serial.print( c3 );  
  Serial.print( ' ' );    
  Serial.print( s3 );  
  Serial.print( "--->" );    
  
  f1 = f1*180.0/3.1415;
  f2 = f2*180.0/3.1415;
  f3 = f3*180.0/3.1415;
  
  Serial.print( f1 );  
  Serial.print( ' ' );    
  Serial.print( f2 );  
  Serial.print( ' ' );    
  Serial.println( f3 );     
  
  forwardK(f1, f2, f3);  
}


void PrintAngles()
{
  Serial.print( ff1 );  
  Serial.print( ' ' );    
  Serial.print( ff2 );  
  Serial.print( ' ' );    
  Serial.println( ff3 );    
}

void setup()
{
  // initialize the serial communication:
  Serial.begin(19600);
  
  for(int i=0;i<9;i++)
  {
    pinMode(i+2, OUTPUT);
    digitalWrite(i+2, LOW);
  }
  
  // 8th bit
  pinMode(13, OUTPUT);
  digitalWrite( 13, LOW);
  
  for(int i=0;i<3;i++)
  {
    pinMode(10+i, INPUT);
    digitalWrite(10+i, HIGH);
  }
}

char cmd_Wp[]="M 0,0,0,500,-500,0."; // 90 grados up
char cmd_Wn[]="M 0,0,0,-500,500,0."; // 90 grados up

char cmd[]="M 0,0,0,500,-500,0."; // 90 grados up
char cmd_N[]="N.";

void loop() 
{

    while( true)
    {
      if ( Serial.available() )
      {
        char c = (char)Serial.read();        
        SendData(c);
      }
    }
  

  if ( Serial.available() )
  {
    char c = (char)Serial.read();    
    //SendData( 1<<(inChar-'0') );
    if ( c == 'n')
    { 
      SendString( cmd_N );
    } 
    else if ( c == '+')
    { 
      SendString( cmd_Wp );
    } 
    else if ( c == '-')
    { 
      SendString( cmd_Wn );
    } 
    else if ( c == 'O')
    { 
      SendString( "O." );
    } 
    else if ( c == 'C')
    { 
      SendString( "C." );
    } 
    else if ( c == 'R')
    { 
      SendString( "M 0,0,0,500,500,0." );
    } 
    else if ( c == '3')
    {   
      InverseK(20,0,21);
    }
    else if ( c == '4')
    {
      InverseK(20,0,21);
      InverseK(20+10,0,21);
      InverseK(20+10,0,21+10);
      InverseK(20,0,21+10);
      InverseK(20,0,21);
      forwardK(0,90,90);      
    }
    else if ( c == '5')
    {
      InverseK(20,5,21);
      InverseK(20+10,5,21);
      InverseK(20+10,-5,21);
      InverseK(20,-5,21);
      forwardK(0,90,90);      
    }
    else if ( c == 'l')
    {
      while( true)
      {
        if ( Serial.available() )
        {
          char c = (char)Serial.read();        
          SendData(c);
        }
      }
    }
    else if ( c == 'q')
    {
      char table[] = {0x5,0x6,0xa,0x9};

      for(int i=0;i<100;i++)
      {
        sprintf(cmd_buf,"Q%i.Q%i.",64+table[i&3], 32+table[i&3]);        
        SendString(cmd_buf);
        delay(10);
      }
      
      for(int i=0;i<6;i++)
      {
        sprintf(cmd_buf,"Q%i.", i<<4);  
        SendString(cmd_buf);      
      }

    }
    else
    {    
      //SendString( cmd );
      /*
      forwardK(0,45,90);
      forwardK(0,90,90);
      forwardK(0,45,45);
      forwardK(0,90,45);
      */
      forwardK(90,45,135);
      forwardK(90,-10,25);
      forwardK(90,45,135);
      
      forwardK(0,90,90);      
    }
    
    
    // Serial.print( digitalRead(10) );    // ack
    // Serial.print( digitalRead(11) );    // busy
    // Serial.println( digitalRead(12) );    //err

  }
}


