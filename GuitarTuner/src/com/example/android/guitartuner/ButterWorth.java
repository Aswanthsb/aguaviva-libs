package com.example.android.guitartuner;

public class ButterWorth implements Filter
{
	double a1;
	double gain;
	double xv[] = new double[3];
	double yv[] = new double[3];

	public ButterWorth(float _a1, float _gain)
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
