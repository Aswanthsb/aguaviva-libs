package com.example.android.guitartuner;

public class BandPass implements Filter 
{
	double a0, a1;
	double gain;
	double xv[] = new double[3];
	double yv[] = new double[3];

	public BandPass(double _a0, double _a1, double _gain)
	{
		a0 = _a0;	
		a1 = _a1;	
		gain = _gain;
		
		xv[0] = 0; xv[1] = 0;
		yv[0] = 0; yv[1] = 0;
	}
	
	
	public double Apply(double v)
	{
		xv[0] = xv[1]; xv[1] = xv[2]; 
		xv[2] = v / gain;
		yv[0] = yv[1]; yv[1] = yv[2]; 
		yv[2] =   (xv[2] - xv[0]) + ( a0 * yv[0]) + (  a1 * yv[1]);

		return yv[2];				
	}
}	
