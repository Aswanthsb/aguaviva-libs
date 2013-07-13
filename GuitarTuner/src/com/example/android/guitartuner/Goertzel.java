package com.example.android.guitartuner;

public class Goertzel implements Filter
{
	double s_prev = 0;
	double s_prev2 = 0;
	double coeff;
	int N;
	
	public Goertzel(float normalized_frequency)
	{
		coeff = 2*(float)Math.cos(2*Math.PI*normalized_frequency);
	}
	
	public double Apply(double v)
	{
		double s = v + coeff*s_prev - s_prev2;
		s_prev2 = s_prev;
		s_prev = s;
		
		N++;

		double power = s_prev2*s_prev2 + s_prev*s_prev - coeff*s_prev*s_prev2;	
		double totalpower = v*v;
		if ( totalpower == 0)
			totalpower = 1;
		//return yv[1];
		return power/totalpower/N;
	}
}
