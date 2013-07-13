package com.example.android.guitartuner;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.util.Log;
import android.view.View;

public class DrawView extends View 
{
    Paint paint = new Paint();
    Paint bold = new Paint();

    Paint red = new Paint();
    Paint green = new Paint();
    Paint blue = new Paint();
    Paint yellow = new Paint();
    Paint white = new Paint();

    Paint [] pens = new Paint[] { blue, red, green, white, green, red, blue}; 
    
    float [] freqs = new float[] {82.4f, 110f,146.8f, 196, 246.9f, 329.6f} ;
    String [] freqnames = new String[] {"E", "A", "d", "g", "b", "e"} ;

    final int subfreqs = 7;
    
    float [][] power = new float[freqs.length][subfreqs];
    float [] maxpower = new float[freqs.length];
    
    
    ToneDetector [][] tone = new ToneDetector[freqs.length][5]; 
   
    
    private static final String TAG = "tuner";
    
    short[] m_buffer = null;
    short [] tmp; 
    
    int corrLength;
    int[] m_corrVals;
    
    FFT f;

    float samplingRate = 0;
    
    ToneDetector lp;
    
    public DrawView(Context context) 
    {
        super(context);
              
        for(int i = 0;i<freqs.length;i++)
        {        	
        	float f = freqs[i];

        	for(int j=0;j<5;j++)
        	{
        		float f1 = (f+2*j-5);
        		float f2 = (f+2*j-3);
        		tone[i][j] = new ToneDetector("mkfilter -Bu -Bp -o 1 -a " + (f1/samplingRate) + " " + (f2/samplingRate) );       		        		
        	}
        }        
        
        red.setColor(Color.RED);
        green.setColor(Color.GREEN);
        blue.setColor(Color.rgb(128, 128, 255));
        yellow.setColor(Color.YELLOW);   
        white.setColor(Color.WHITE);   
        
        paint.setColor(Color.WHITE);        
        bold.setStrokeWidth(4);
        bold.setColor(Color.WHITE);
    }

    public void SetData(short[] buffer, float smprate)
    {
    	samplingRate = smprate/2;
    	m_buffer = buffer;
    	
    	if ( m_corrVals == null  )
    	{
    		m_corrVals = new int[m_buffer.length];
    		
    		tmp = new short[m_buffer.length];
    		lp = new ToneDetector("mkfilter -Bu -Bp -o 1 -a " + (77.4f/samplingRate) + " " + (336.4f/samplingRate) );
    		
            f = new FFT(m_buffer.length);
            f.makeWindow();

    	}    	    	
    }   



    
    public float mapvalue(float min0, float max0, float min1, float max1, float val1)
    {
    	final float t = (val1-min1)/(max1-min1);
    	
    	return min0 + (max0-min0)*t;
    }
    
    int lastNote;
    
    public void DrawScale(Canvas canvas, int length, float x1,float x2,float y)
    {
        int ii = 4;
        int size = 5;
        for(int i=0;i<=length;i++)
        {
        	float x = mapvalue(x1, x2, 0, length, i);
        	
        	ii++;
        	if (ii==5)
        	{
        		size = 10;
        		ii=0;
        	}
        	else
        	{
        		size = 5;
        	}
        	
        	canvas.drawLine(x, y-size, x, y+size, bold);
        }
    }

    public void DrawThumb(Canvas canvas, float x, float y)
    {
    	canvas.drawLine(x, y-10, x+10, y-15, bold);
    	canvas.drawLine(x, y-10, x-10, y-15, bold);

    	canvas.drawLine(x+10, y-15, x-10, y-15, bold);
    	
    	canvas.drawLine(x, y+10, x+10, y+15, bold);
    	canvas.drawLine(x, y+10, x-10, y+15, bold);
    	canvas.drawLine(x+10, y+15, x-10, y+15, bold);
    }
    
    
    public void DrawWave(Canvas canvas, short [] buf, float x,float y)
    {
        int old = buf[0]/256;
        for(int i=1;i<buf.length;i++)
        {
        	int nue = buf[i]/256;
        	canvas.drawLine(x+i-1, y-old, x+i, y-nue, paint);
        	old = nue;
        }
    }

    public void DrawFFT(Canvas canvas, float x,float y)
    {
        for(int i=0;i<m_buffer.length/2;i++)
        {
        	int nue = m_buffer[i];
        	canvas.drawLine(x+2*i, y, x+2*i, y-nue, white);
        }
    }
    
    
    //2756.25
    public void DrawFilteredWave(Canvas canvas, float x,float y, ToneDetector td, Paint p)
    {
    	final int scale = 32;
    	
        int old=0;
        
        int nue;
        int j = 0;
        for(int i=1;i<m_buffer.length;i++)
        {
        	nue = (int)(td.GetTone( m_buffer[i] )/scale);
        	canvas.drawLine(x+i-1, y-old, x+i, y-nue, p);
        	old = nue;        	
        }
    }
   
    public void FilterWave(short [] in, short [] out, ToneDetector td)
    {
        for(int i=0;i<in.length;i++)
        {
        	out[i] = (short)(td.GetTone( in[i] ));
        }    	
    }

    public int GetWaveMax(short [] in,int ini,int fin)
    {
    	short max = 0;
    	int idx = -1;
    	if (fin==0)
    		fin = in.length;
    	
        for(int i=ini;i<fin;i++)
        {
        	if (in[i]>max)
        	{
        		max = in[i];
        		idx = i;        				
        	}
        }    	
        return idx;
    }

    public float GoetzelPower(short [] in, float target_frequency)
    {
    	float s_prev = 0;
        float s_prev2 = 0;
		float normalized_frequency = target_frequency / samplingRate;
		float coeff = (float)(2.0f*Math.cos(2.0f*Math.PI*normalized_frequency));
		for(int i=0;i<in.length;i++)
		{
		  float s = in[i] + coeff*s_prev - s_prev2;
		  s_prev2 = s_prev;
		  s_prev = s;
		}
		float power =  s_prev2*s_prev2 + s_prev*s_prev - coeff*s_prev*s_prev2;
		
		return (float)Math.sqrt(power/in.length);
    }

    
    float time = 0;
    
    void GenerateSignal(short [] buf, float freq, float sr)
    {    	
    	float t = (1.0f/sr);
    	
        for(int i=0;i<buf.length;i++)
        {
        	m_buffer[i] = (short)(3*8192*Math.cos(2*Math.PI * time * freq));
        	
        	time += t;
        }    	
    }
    
    float ComputePower(short [] buf)
    {
    	float power = 0;
    	
        for(int i=0;i<buf.length;i++)
        {
        	power += buf[i]*buf[i];
        }
    	return (float)Math.sqrt(power/buf.length);
    }

    public float ComputePower(short [] buf, ToneDetector td)
    {
    	double power = 0;
        for(int i=0;i<buf.length;i++)
        {
        	double v = td.GetTone((double)buf[i]);
        	power += v*v;
        }
    	return (float)Math.sqrt(power/buf.length);
    }
    
    int freq = 1;
        
    short [][] bode = new short[6][50];   
    
    @Override
    public void onDraw(Canvas canvas) 
    {       
    	//Log.v(TAG, "Begin Drawing--------------");

    	int pad400 = (canvas.getWidth() - m_buffer.length)/2;
    	
        final long t0 = System.currentTimeMillis();
        
        if (m_buffer==null)
        	return;

        canvas.drawLine(pad400, 200, pad400+m_buffer.length, 200, paint);
        
        
        //GenerateSignal(m_buffer, freq, sr);        
        FilterWave(m_buffer, m_buffer, lp);
        //DrawWave(canvas, tmp, 0, 400);
        
        
        /*
        DrawWave(canvas, m_buffer, pad400, 200);
        float v = ComputePower(m_buffer);
        canvas.drawLine(pad400, 200, pad400, 200-v/256, red);
        */

    	int pad = (canvas.getWidth() - (75 * (freqs.length-1) + 6*10 ) )/2;        
        
    	float min = 1000000;
    	float max = 0;
    	float avg = 0;
    	float std = 0;

    	for(int i=0;i<freqs.length;i++)
    	{
    		maxpower[i]=0;
        	for(int j=0;j<subfreqs;j++)
        	{
        		float vv = GoetzelPower(m_buffer, freqs[i]+(j-(subfreqs-1)/2)*4);
        		
        		if (i==0)
        		{
        			vv *=2;
        		}
        		
        		if (vv<min)
        			min= vv;

        		if (vv>max)
        			max=vv;
        		
        		power[i][j] = vv;
        		maxpower[i] +=vv;
        		avg += vv;
        	}
        	maxpower[i] /= subfreqs;
    	}    	
    	avg /= freqs.length*subfreqs;
/*    	
    	for(int i=0;i<freqs.length;i++)
    	{
        	for(int j=0;j<subfreqs;j++)
        	{
        		float vv = (power[i][j]-avg);
        		std += vv*vv;
        	}
    	}
    	std /= freqs.length*subfreqs;
*/
    	
        paint.setTextSize(40);
   	
    	for(int i=0;i<freqs.length;i++)
    	{
        	float vv;
        	for(int j=0;j<subfreqs;j++)
        	{
        		float m = max;       		
        		
        		if ( m<2000)
        			m=2000;
        		int x = pad + 75*i+j*10;
        		vv = 300*power[i][j]/max;
        		//vv= (float)Math.log(power[i][j]);      		
        		canvas.drawRect(x, 600, x + 10, 600-vv, pens[j]);
        	}
        	canvas.drawText( freqnames[i], pad+5+75*i+2*10, 640, paint);
    	}

    	
        
    	
    	f.cepstrum(m_buffer);
    	int bin = GetWaveMax(m_buffer,50,80);
    	canvas.drawText( "Fq =" + (samplingRate/bin)+" "+bin, 10, 300, paint);
    	//DrawWave(canvas, m_buffer, 0, 200);
    	DrawFFT(canvas, 0, 200);
        //f.fft(m_buffer);
    	//DrawFFT(canvas, 0, 200); 
        
        
        /*        
        int max = GetWaveMax(bode);
    	paint.setTextSize(50);
        canvas.drawText( "Fag ="+max, 10, 100, paint);
        */


        /*        
        f.fft(m_buffer);
    	DrawFFT(canvas, pad400, 100); 
    	paint.setTextSize(50);
    	double freq = m_buffer[m_buffer.length/2]*(sr)/128;      	
        canvas.drawText(String.format("%f", freq), 10, 100, paint);
        */
    	   	
    	
    	pad400 = (canvas.getWidth() - 400)/2;
    	
/*        
        int note = FindClosestNote(0, notesFreqs);

        {
        	float notepos = (note -1)*400/5 + pad400;
        	float prevpos = notepos-(400/12);
        	float nextpos = notepos+(400/12);
        	        	       	
        	float f = mapvalue(prevpos,nextpos, notesFreqs[note]-15, notesFreqs[note]+15, freq);

        	DrawThumb(canvas, f, 300);
        }
        
        paint.setTextSize(50);
        canvas.drawText(notesNames[note], pad400+200, 480, paint);        
        
        {
        	float f = mapvalue(pad400,pad400+ 400, notesFreqs[note]-15, notesFreqs[note]+15, freq);    	
        	DrawThumb(canvas, f, 500);        	
        }        

        paint.setTextSize(50);
        canvas.drawText(String.format("%d %d %d %d", mins[0], mins[1], mins[2], mins[3]), 10, 600, paint);
        
        
        paint.setTextSize(50);
        canvas.drawText(String.format("%s %4.1f %d %d", notesNames[note], freq, minsnum, exitpoint), 10, 700, paint);
*/		
		final long t1 = System.currentTimeMillis() -t0;
		paint.setTextSize(20);
        canvas.drawText(String.format("%d", t1), 400, 20, paint);
       // Log.v(TAG, "End Drawing--------------");
    }

}