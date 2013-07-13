package com.example.android.guitartuner;

public class LowPass implements Filter
{
	double a1;
	double gain;
	double xv[] = new double[2];
	double yv[] = new double[2];

	public LowPass(double _a1, double _gain)
	{
		a1 = _a1;
		
		gain = _gain;
	}
	
	
	public double Apply(double v)
	{
		xv[0] = xv[1]; 
        xv[1] = v / gain;
        
        yv[0] = yv[1]; 		
		
		yv[1] = (  xv[0] +xv[1] ) + (  a1 * yv[0]);
		
		return yv[1];
	}
	
}
