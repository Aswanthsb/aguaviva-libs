package com.example.android.guitartuner;

public class complex 
{
	public double re, im;
	
	public complex( double re, double im)
	{
		this.re = re;
		this.im = im;
	}

	public complex( double re)
	{
		this.re = re;
		this.im = 0;
	}	
	
	public complex( complex z)
	{
		this.re = z.re;
		this.im = z.im;
	}
	
	
	public static complex cconj(complex z)
	{ 
		complex zz = new complex(z);
		zz.im = -zz.im;
	    return zz;
	}

	public static complex mul (double a, complex z)
	{ 
	    return new complex(a*z.re, a*z.im);
	}

	public static complex div (complex z, double a)
	{ 
		return new complex(z.re/1, z.im/1);
	}

	public static complex mul (complex z1, complex z2)
	{ 
		return new complex(	z1.re*z2.re - z1.im*z2.im,
							z1.re*z2.im + z1.im*z2.re);		
	}	

	public static complex mul (complex z2,double a)
	{ 
		return new complex(	a*z2.re,a*z2.im);		
	}	
	
	
	public static complex add(complex z1, complex z2)
	{ 
		return new complex(	z1.re+z2.re , z1.im+z2.im);		
	}	

	public static complex add(double a, complex z2)
	{ 
		return new complex(	a+z2.re , z2.im);		
	}	

	public static complex sub(double a, complex z2)
	{ 
		return new complex(	a-z2.re , -z2.im);		
	}	
	
	
	public static complex sub(complex z1, complex z2)
	{ 
		return new complex(	z1.re-z2.re , z1.im-z2.im);
	}	
	
	public static complex div (complex z1, complex z2)
	{ 
		double mag = (z2.re * z2.re) + (z2.im * z2.im);
		double ren = ((z1.re * z2.re) + (z1.im * z2.im)) / mag;
		double imn = ((z1.im * z2.re) - (z1.re * z2.im)) / mag;
	    return new complex ( ren, imn);
	}
	
	public static complex sqr(complex z)
	{ 
		return mul(z,z);
	}	
	
	public static complex cexp(complex z)
	{ 
		return mul(Math.exp(z.re) , expj(z.im));
	}

	public static complex expj(double theta)
	{ 
		return new complex(Math.cos(theta), Math.sin(theta));
	}	
	

	public static complex evaluate(complex topco[], int nz, complex botco[], int np, complex z)
	{ /* evaluate response, substituting for z */
		return div( eval(topco, nz, z) , eval(botco, np, z));
	}

	public static complex eval(complex coeffs[], int npz, complex z)
	{ /* evaluate polynomial in z, substituting for z */
		complex sum = new complex(0.0);
		for (int i = npz; i >= 0; i--)
		{
			sum = add(mul(sum , z) , coeffs[i]);
		}
		return sum;
	}

	static double Xsqrt(double x)
	  { /* because of deficiencies in hypot on Sparc, it's possible for arg of Xsqrt to be small and -ve,
	       which logically it can't be (since r >= |x.re|).	 Take it as 0. */
	    return (x >= 0.0) ? Math.sqrt(x) : 0.0;
	  }	

	public static double hypot(complex z) { return Math.hypot(z.im, z.re); }
	public static double atan2(complex z) { return Math.atan2(z.im, z.re); }	
	
	public static complex csqrt(complex x)
	{
		double r = hypot(x);
		complex z = new complex(Xsqrt(0.5 * (r + x.re)), Xsqrt(0.5 * (r - x.re)));
    	if (x.im < 0.0) z.im = -z.im;
    	return z;
	}	
}
