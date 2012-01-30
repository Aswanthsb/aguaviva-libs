// Wire Master Writer
// by Nicholas Zambetti <http://www.zambetti.com>

// Demonstrates use of the Wire library
// Writes data to an I2C/TWI slave device
// Refer to the "Wire Slave Receiver" example for use with this

// Created 29 March 2006

#include <Wire.h>

byte motor[4] = { 
  2, 3, 4, 5 };
byte motor2[4] = { 
  1, 2, 4, 8 };

unsigned char IsNumber( char c )
{
  return ( c >= '0' ) && ( c <= '9' );
}

unsigned char ParseInt( char *s, unsigned int *out)
{
  *out = 0;

  char c = 0;

  for(;;)
  {
    if (IsNumber(*s))
    {
      *out = ( *out * 10 ) + (*s-'0');
    }
    else
    {
      return c;               
    }
    s++;
    c++;
  }
}


void setup()
{
  Serial.begin(19200) ;  
  Wire.begin(); // join i2c bus (address optional for master)
/*
  for( byte i=0;i<4;i++)
  {
    pinMode( motor[i], OUTPUT);       
  }
*/
}

byte x = 0;
unsigned char v = 0;

void loop()
{
  if (Serial.available() > 0) 
  {
      unsigned char in = Serial.read();
      
      if ( IsNumber(in) )
      {
        v = ( v * 10 ) + ( in - '0' );
      }
      else if ( in == 'i' )
      {
        Wire.beginTransmission(0x60); // transmit to device #4
        Wire.send( v );              // sends one byte  
        Wire.endTransmission();    // stop transmitting
        v = 0;
      }
      else if ( in == 'I' )
      {
        Wire.beginTransmission(0x61); // transmit to device #4
        Wire.send( v );              // sends one byte  
        Wire.endTransmission();    // stop transmitting
        v = 0;
      }
      else
      {
        Serial.println("Err");
      }
  }
}


