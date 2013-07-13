package com.example.android.guitartuner;

import android.app.Activity;
import android.os.Bundle;
import android.media.AudioFormat; 
import android.media.AudioRecord;
import android.media.MediaRecorder.AudioSource;
import android.util.Log;
import android.view.MotionEvent; 
import android.widget.TextView; 
import android.graphics.Color; 

public class GuitarTunerActivity extends Activity {

	DrawView drawView;
	
	public AudioRecord audioRecord; 
	public int mSamplesRead; //how many samples read 
	public int buffersizebytes; 
	public int buflen; 
	public int channelConfiguration = AudioFormat.CHANNEL_CONFIGURATION_MONO; 
	public int audioEncoding = AudioFormat.ENCODING_PCM_16BIT; 
	public static short[] buffer; //+-32767 
	public static final int SAMPPERSEC = 44100; //samp per sec 8000, 11025, 22050 44100 or 48000 	
	private static final String TAG = "tuner";
	
	public static short[] downsampledBuffer1 = null; //+-32767 
	public static short[] downsampledBuffer2 = null; //+-32767
	public static short[] downsampledBuffer3 = null; //+-32767
	public static short[] downsampledBuffer4 = null; //+-32767
	
	/** Called when the activity is first created. */ 
	@Override 
	public void onCreate(Bundle savedInstanceState)
	{ 
		super.onCreate(savedInstanceState);
		
		drawView = new DrawView(this);
		
		// setContentView(R.layout.main); 
		
		audioRecord = null;
		
		acquire();
	    
		//setContentView(R.layout.main);
        drawView.setBackgroundColor(Color.DKGRAY);
		setContentView(drawView);
    }
    
  //-------------------------------------- 
	void downSample(short [] in, short [] out)
	{
    	int j=0;
    	for(int i=0;i<out.length;i++)
    	{
    		out[i] = (short)((in[j++]+in[j++])>>1);
    	}
	}
	
    public void acquire()
    { 
    	if ( audioRecord == null)
    	{
    		int buffersizebytes = AudioRecord.getMinBufferSize(SAMPPERSEC,channelConfiguration,audioEncoding); //4096 on ion
    		
    		if ( buffer == null )
    		{
    			buffer = new short[buffersizebytes/2];
    			
    			downsampledBuffer1 = new short[buffer.length/2];
    			downsampledBuffer2 = new short[buffer.length/4];
    			downsampledBuffer3 = new short[buffer.length/8];
    			//downsampledBuffer4 = new short[buffer.length/16];
    		}
    		if (downsampledBuffer4!=null)
    			drawView.SetData(downsampledBuffer4, 44100.0f/16.0f);
    		if (downsampledBuffer3!=null)
    			drawView.SetData(downsampledBuffer3, 44100.0f/8.0f);
    		if (downsampledBuffer2!=null)
    			drawView.SetData(downsampledBuffer3, 44100.0f/4.0f);

    		audioRecord = new AudioRecord(android.media.MediaRecorder.AudioSource.MIC,SAMPPERSEC, channelConfiguration,audioEncoding,buffersizebytes); //constructor    	
    	}
    	
    	
    	try 
    	{ 
    		audioRecord.setPositionNotificationPeriod(buffer.length);

    		audioRecord.setRecordPositionUpdateListener( new AudioRecord.OnRecordPositionUpdateListener() 
    		{
    			//@Override
    		    public void onMarkerReached(AudioRecord recorder) 
    			{
    		        Log.v("MicInfoService", "onMarkedReached CALL");
    		    }
    			
    			//@Override
    		    public void onPeriodicNotification(AudioRecord recorder) 
    			{
    		    	mSamplesRead = audioRecord.read(buffer, 0, buffer.length);
    		    	
    		    	downSample(buffer, downsampledBuffer1);
    		    	downSample(downsampledBuffer1, downsampledBuffer2);
    		    	if (downsampledBuffer3!=null)
    		    		downSample(downsampledBuffer2, downsampledBuffer3);
    		    	if (downsampledBuffer4!=null)
    		    		downSample(downsampledBuffer3, downsampledBuffer4);
    		    	
    		    	drawView.invalidate();
    		    	
    		    	Log.v("MicInfoService", "onPeriodicNotification CALL");
    		    }
    		});
    		
    		audioRecord.startRecording(); 
    		
    		mSamplesRead = audioRecord.read(buffer, 0, buffer.length);
    		drawView.invalidate();
    	} 
    	catch (Throwable t) 
    	{ 
    	 Log.e("AudioRecord", "Recording Failed"); 
    	} 
    }     
    
    
    //-------lifecycle callbacks------------------- 
    @Override 
    public void onResume()
    { 
    	Log.v(TAG, "* Resume");    	
    	super.onResume(); 
    	acquire(); 
    }//onresume 

    @Override 
    public void onPause()
    { 
    	Log.v(TAG, "* Pause");
    	super.onPause(); 
    	audioRecord.stop();
    	
    	
    }//onpause 

    @Override 
    public void onStop()
    { 
    	Log.v(TAG, "* Stop");    	
    	super.onStop(); 
    	audioRecord.release();
    	audioRecord = null;
    }//onpause 
    
    @Override 
    public boolean onTouchEvent(MotionEvent motionevent)
    { 
	    if(motionevent.getAction()==MotionEvent.ACTION_DOWN)
	    { 
	    	//if ( audioRecord.)
	    	//trigger(); //acquire buffer full of samples 
	    } 
	    return true; 
    }     
    
}