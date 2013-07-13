package com.example.android.guitartuner;

public class ToneDetector {

	LowPass lp;
	BandPass bp;
	Goertzel g;
	
	ToneDetector(float a0, float a1, float gain)
	{
		lp = new LowPass((float)0.9886663993, (float)1.764664251e+02);  //5hz
		bp = new BandPass(a0,a1,gain);
	}

	ToneDetector(String s)
	{		
		lp = new LowPass((float)0.9886663993, (float)1.764664251e+02);  //5hz

		mkfilter mk = new mkfilter();
        mk.main(s);
		bp = mk.GetBandPass();
	}
	
	ToneDetector(float f)
	{
		g = new Goertzel(f);
	}
	
	
	double GetTone(double v)
	{
		//final float f = g.Apply(v);
    	final double f = bp.Apply(v);
		
    	//return (float)Math.sqrt(lp.Apply(f*f));    		
    	//return Math.abs(lp.Apply(f*f));
    	return (double)f;
	}		
}
