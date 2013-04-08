

  

void Mixer( c, d )
{
	//Mixer:   ld     a,c        
	//7B22:   cp     #05        
	//7B24:   ret    nz 
    if ( c == 5 )
	{        
		//7B25:   dec    d          
		//7B26:   jr     z,#7b2c    
		//7B28:   ld     a,#9c      
		//7B2A:   jr     #7b2e      
		//7B2C:   ld     a,#b8      
		//7B2E:   ld     (#e03a),a  ;   set mixer control
		//7B31:   ld     e,a        
		//7B32:   ld     a,#07      
		//7B34:   jp     #_WRTPSG     

		if ( d == 0 )
		{
			//mute tone just noise
		    WrtPsg( 7, 10111000 ); // 0xb8
		}
		else
		{
			// 
		   WrtPsg( 7, 10011100); // 0x9c 
		}
 
	}	
}

7B37:   ld     a,(#e03a)  
7B3A:   call   #7b2e      


byte channels[] 
{ 
	00, 01, 00, 7d3a, 00, 00, 00, 00, 00, 00, 00, 00, 00,
	00, 01, 00, 7d3a, 00, 00, 00, 00, 00, 00, 00, 00, 00,
	00, 01, 00, 7d3a, 00, 00, 00, 00, 00, 00, 00, 00, 00,
}

void UpdateMusic()
{
	//7B3D:   ld     c,#01  
	byte reg = 1;
    
	7B3F:   ld     ix,#e010  ; music channel 1
	7B43:   exx               
	//7B44:   ld     b,#03      
	7B46:   ld     de,#000e   

	for (byte b = 0;b<3;b++)
	{
		//7B49:   exx               
		//7B4A:   ld     a,(ix+#02) 
		
		byte a = (ix+#02);
		
		//7B4D:   push   af         
		//7B4E:   dec    a          
		//7B4F:   call   z,#7b5f    
		//7B52:   pop    af         
		
		if ( a == 1 )
		{
			RoutineA();
		}

		//7B53:   or     a          
		//7B54:   call   nz,#7b92

		if ( a != 0 )
		{
			RoutineB( reg );
		}
		
		
		//7B57:   inc    c          
		//7B58:   inc    c          

		reg +=2;

		//7B59:   exx               
		//7B5A:   add    ix,de      
		
		ix += 14;
	}
	//7B5C:   djnz   #7b49      
	//7B5E:   ret               
}


void RoutineA( reg )
{
	//7B5F:   ld     a,c        
	//7B60:   cp     #05        
	//7B62:   ret    c

	if ( reg < 5 )
	{
		return;
	}
          
	//7B63:   ld     hl,#e03e   
	7B66:   ld     de,#7b91   

	byte *ptrA = 0xe03e;

	//7B69:   ld     a,(#e03b)  
	//7B6C:   cp     #01    
	//7B6E:   jr     c,#7b80    

	if ( (#e03b) < 1 )
	{
		//7B80:   push   bc         
		//7B81:   ex     de,hl      
		//7B82:   ld     bc,#0004   
		//7B85:   lddr              
		//7B87:   ex     de,hl      
		//7B88:   pop    bc         
		//7B89:   inc    hl         
		//7B8A:   inc    hl         
		//7B8B:   inc    hl         
		
		memcpy (7b91->ptrA, 4); 
		ptrA--;
	}
	else
	{
		//7B70:   ld     a,#08      
		//7B72:   add    a,(hl)     
		//7B73:   ld     (hl),a     
		
		*ptrA += 8;

		//7B74:   dec    hl         
		
		ptrA--;
		
		7B75:   jr     nc,#7b78   
		7B77:   inc    (hl)       
		
	}
	//7B78:   dec    hl         
	
	ptrA--;
	
	7B79:   ld     (ix+#03),l 
	7B7C:   ld     (ix+#04),h 
	7B7F:   ret               
}
    
7B8E:   .db 01,21, b0

7B91:   .db 61       

void RoutineB( reg )
{

init:
	//7B92:   bit    6,a        
	//7B94:   ld     d,#01      
	//7B96:   call   z,#Mixer 

	if ( a & 1<<6 == 0)
	{
		Mixer( reg, 1 )
	}
   
	7B99:   ld     a,(ix+#02) 
	7B9C:   or     a          
	7B9D:   jp     m,#7c2d 

//--------------------------------------------------   
	if ( (ix+#02) >= 0 )
	{
		7BA0:   dec    (ix+#00)   
		7BA3:   ret    nz         
			
		*(ix+#00)--;
	    if ( *(ix+#00) != 0)
		{
			return;
		}

		//7BA4:   ld     l,(ix+#03) 
		//7BA7:   ld     h,(ix+#04) 
		//7BAA:   ld     a,(hl)     
		
		byte *ptr = ( ix + 03);
		
		byte a = *ptr;  // music command

		//7BAB:   cp     #fe        
		//7BAD:   jp     z,#7af7    
     	if ( a == 0xfe )
		{
			//7AF7:   inc    hl         
			
			ptr++;
			
			//7AF8:   ld     a,(ix+#09) 
			//7AFB:   inc    a          
			//7AFC:   cp     (hl)       
			//7AFD:   jr     z,#7b12 

			a = (ix+#09)+1;
  
			if ( a == *ptr )
			{
			
				//7B12:   inc    hl         
				//7B13:   inc    hl         
				
				ptr+=2;
				
				//7B14:   xor    a          
				//7B15:   ld     (ix+#09),a 

				*(ix+#09) = 0
				
				//7B18:   call   #7ce2      
				
				IncrementPos( ptr );
			}
			else
			{
				7AFF:   jp     m,#7b03    
				7B02:   dec    a          
				7B03:   ld     (ix+#09),a 
				
				//7B06:   inc    hl         
				//7B07:   ld     a,(hl)     
				//7B08:   ld     (ix+#03),a 
				ptr++;			
				*(ix+3) = *ptr;
				
				7B0B:   inc    hl         
				7B0C:   ld     a,(hl)     
				7B0D:   ld     (ix+#04),a 

				ptr++;			
				*(ix+4) = *ptr;
			}

			//7B10:   jr     #7b1b      
			//7B1B:   inc    (ix+#00)   
			
			*(ix+0)++;
			
			//7B1E:   jp     #7b92    
			goto init;
		}


		//7BB0:   jr     nc,#7c1a
		if ( a == 0xff )
		{
			//7C1A:   xor    a          
			//7C1B:   ld     (ix+#09),a 
			//7C1E:   ld     (ix+#0b),a 
			
			*(ix+#09) = 0;
			*(ix+#0b) = 0;
			
			//7C21:   ld     d,#01      
			//7C23:   call   #Mixer    


			Mixer( reg, 1 );
  
			//7C26:   xor    a          
			//7C27:   ld     (ix+#02),a 
			//7C2A:   ld     h,a        
			//7C2B:   jr     #7c53      

			*(ix+#02) = 0;

			//7C53:   ld     a,c        
			//7C54:   rrca              
			//7C55:   add    a,#88      
			//7C57:   ld     e,h        
			//7C58:   jp     #_WRTPSG     
 
			write_PSG_reg_val( (reg>>1) + 0x88, 0 );

			return;
		}
		   
		7BB2:   bit    7,(ix+#02) 
		7BB6:   jp     nz,#7c5b   
		if ( *(ix+#02) & 1<<7 == 0 )
		{
			//7BB9:   and    #f0        
			//7BBB:   cp     #20        
			//7BBD:   ld     a,(hl)     
			//7BBE:   jr     nz,#7bc7   
			if ( (*ptr & 0xf0) & 0x20 == 0 )
			{
				7BC0:   and    #0f        
				7BC2:   ld     (ix+#01),a 
				7BC5:   inc    hl         
				7BC6:   ld     a,(hl)     
			}
			7BC7:   ld     b,a        
			//7BC8:   and    #f0        
			//7BCA:   cp     #10        
			//7BCC:   jr     nz,#7bea   
			if ( ( *ptr & 0xf ) & 0x10 == 0)
			{
				7BCE:   ld     a,(hl)     
				7BCF:   and    #1f        
				7BD1:   ld     e,a        
				7BD2:   inc    hl         
				7BD3:   bit    4,(hl)     
				7BD5:   ld     b,(hl)     
				//7BD6:   jr     nz,#7bdc   
				if ( z )
				{
					//7BD8:   ld     a,e        
					//7BD9:   sub    #10        
					//7BDB:   ld     e,a 
					e-=10;
       
				}
				7BDC:   res    4,b        
				//7BDE:   dec    hl         
				//7BDF:   ld     a,#06      
				//7BE1:   call   #_WRTPSG 
				
				write_PSG_reg_val( 6, e );  //noise generrator

				
				//7BE4:   ld     d,#00      
				//7BE6:   call   #Mixer      
				//7BE9:   inc    hl

				Mixer( reg, 0 );
				
			}

			//7BEA:   bit    6,(ix+#02) 
			//7BEE:   jr     z,#7bf7    
			if ( (ix+#02) & (1<<6) !=0 )
			{
				7BF0:   ld     a,(hl)     
				7BF1:   call   #7ce2      
				7BF4:   ld     a,b        
				7BF5:   jr     #7c0a      
			}
			7BF7:   and    #f0        
			7BF9:   ld     b,a        
			7BFA:   xor    (hl)       
			7BFB:   ld     d,a        
			7BFC:   inc    hl         
			7BFD:   ld     e,(hl)     
			7BFE:   call   #7ce2      
			7C01:   ex     de,hl      
			7C02:   call   #7cd7      
			7C05:   ld     a,b        
			7C06:   rrca              
			7C07:   rrca              
			7C08:   rrca              
			7C09:   rrca              
			7C0A:   ld     h,a        
			//7C0B:   ld     e,(ix+#01) 
			//7C0E:   ld     (ix+#00),e 
			
			*(ix+#00) = *(ix+#01);
			
			//7C11:   ld     a,(ix+#0c) 
			//7C14:   add    a,e        
			//7C15:   ld     (ix+#08),a 
			
			*(ix+#08) = (ix+#0c) + (ix+#01);
			
			7C18:   jr     #7c53      
	}
	else //--------------------------------------------------   else
	{
		7C2D:   dec    (ix+#00)   
		7C30:   jp     z,#7ba4    
		7C33:   dec    (ix+#08)   

		//7C36:   ld     a,(ix+#08) 
		//7C39:   cp     (ix+#00)   
		//7C3C:   jr     nz,#7c47 
	  
		if ( (ix+#08) == (ix+#00) )
		{
			//7C3E:   ld     e,a        
			//7C3F:   ld     a,(ix+#0d) 
			//7C42:   cp     e          
			//7C43:   ld     a,e        
			//7C44:   jr     nc,#7c4a   
			//7C46:   ret               
			
			if ( (ix+#08)  >= (ix+#0d) )
			{
				return;
			}
		}

		7C47:   dec    (ix+#08)   
		7C4A:   ld     a,(ix+#07) 
		7C4D:   dec    a          
		7C4E:   ret    m          


		7C4F:   ld     (ix+#07),a 
		7C52:   ld     h,a        
		
//--------------------------------------------------   		
		7C53:   ld     a,c        
		7C54:   rrca              
		7C55:   add    a,#88      
		7C57:   ld     e,h        
		7C58:   jp     #_WRTPSG      
		return;
	}
	
	
	
	
bucle2:	
	//7C5B:   ld     a,(hl)     
	//7C5C:   and    #f0        
	//7C5E:   cp     #d0        
	//7C60:   ld     a,(hl)     
	//7C61:   jr     nz,#7c6a   
	if ( (*ptr & 0xf) == 0xd0 )
	{
		//7C63:   and    #0f        
		//7C65:   ld     (ix+#0a),a 
		
		*(ix+#0a) = a & 0xf;
		
		//7C68:   inc    hl         
		//7C69:   ld     a,(hl)     
		ptr++;
	}
	//7C6A:   cp     #f0        
	//7C6C:   jr     c,#7c7f    
	if ( *ptr < 0xf0  )
	{
	    
		//7C6E:   and    #0f        
		//7C70:   ld     (ix+#06),a
		//7C73:   inc    hl         
		
		*(ix+#06) = *ptr & 0xf;
		ptr ++;
 
		//7C74:   ld     a,(hl)     		
		//7C75:   ld     (ix+#0c),a 
		//7C78:   inc    hl         

		*(ix+#0c) = *ptr;
		ptr ++;


		//7C79:   ld     a,(hl)     		
		//7C7A:   ld     (ix+#0d),a 
		//7C7D:   inc    hl         
		
		*(ix+#0d) = *ptr;
		ptr++;
		
		//7C7E:   ld     a,(hl)     
	}
	//7C7F:   cp     #e0        
	//7C81:   jr     c,#7c94    
	if ( *ptr < 0xe0 )
	{
		//7C83:   and    #0f        
		//7C85:   bit    3,a        
		//7C87:   jr     z,#7c8f   
		if ( (*ptr & 0xf) & (1<<3) != 0 )
		{ 
			7C89:   ld     (ix+#0b),a 
			//7C8C:   inc    hl         
			
			*(ix+#0b) = *ptr & 0xf;
			ptr++;			
			
			//7C8D:   jr     #7c5b      
			goto bucle2;
		}

		//7C8F:   ld     (ix+#05),a 
		//7C92:   inc    hl         
		//7C93:   ld     a,(hl)     
		*(ix+#05) = *ptr;
		ptr++;
	}


	//7C94:   and    #0f        
	//7C96:   ld     b,a        
	//7C97:   ld     a,(ix+#0a) 
	//7C9A:   jr     z,#7ca1    
	//7C9C:   add    a,(ix+#0a) 
	//7C9F:   djnz   #7c9c      
	//7CA1:   ld     (ix+#01),a 

	(ix+#01) = (ix+#0a) * ( (ptr & 0xf) + 1 );
	
	//7CA4:   ld     a,(hl)     
	//7CA5:   call   #7ce2 

	a = (*ptr & 0xf0)  >> 4;
	
	IncrementPos( ptr );
	
	//7CA8:   and    #f0        
	//7CAA:   rrca              
	//7CAB:   rrca              
	//7CAC:   rrca              
	//7CAD:   rrca              
	//7CAE:   ld     b,a        

	//7CAF:   sub    #0c        	
	//7CB1:   jr     z,#7cb6    
	if ( a != 0Xc )
	{
		//7CB3:   ld     a,(ix+#06) 
		//7CB6:   ld     (ix+#07),a 
		*(ix+#07) = *(ix+#06);
	}
	else
	{
		//7CB6:   ld     (ix+#07),a 
		*(ix+#07) = a-0xc;
	}
	
	7CB9:   call   #7c0a      

	//7CBC:   ld     a,b        
	//7CBD:   ld     hl,#7cea   
	//7CC0:   call   #4010	
	//7CC3:   ld     l,(hl)     
	//7CC4:   ld     h,#00      

	hl = byte prt (#7cea + a);


	//7CC6:   ld     a,(ix+#05) 
	//7CC9:   or     a          
	//7CCA:   jr     z,#7cd0    
	if ( (ix+#05) != 0 )
	{
		//7CCC:   ld     b,a        
		//7CCD:   add    hl,hl      
		//7CCE:   djnz   #7ccd    
	    ptr <<= (ix+#05);
	}
	
	//7CD0:   ld     a,(ix+#0b) 
	//7CD3:   or     a          
	//7CD4:   jr     z,#7cd7    
	//7CD6:   inc    hl         
	if ( (ix + 0b) != 0)
	{
		hl++;
	}
	
	
	//7CD7:   ld     a,c        
	//7CD8:   ld     e,h        
	//7CD9:   call   #_WRTPSG      
	
	write_PSG_reg_val( reg, h );
	
	//7CDC:   ld     a,c        
	//7CDD:   dec    a          
	//7CDE:   ld     e,l        
	//7CDF:   jp     #_WRTPSG      
	
	write_PSG_reg_val( reg-1, l );
}

void IncrementPos( ptrA )
{
	//7CE2:   inc    hl         
	//7CE3:   ld     (ix+#03),l 
	//7CE6:   ld     (ix+#04),h 
	//7CE9:   ret               	
}
