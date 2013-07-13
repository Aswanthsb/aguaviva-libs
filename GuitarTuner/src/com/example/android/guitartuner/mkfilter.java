package com.example.android.guitartuner;

public class mkfilter 
{
	
	final int MAXORDER = 10;
	final int MAXPZ = 10;
	final double EPS = 1e-10;
	private final String TAG = "filter";
	
	class pzrep
	{ 
	    complex [] poles= new complex[MAXPZ];
	    complex [] zeros= new complex[MAXPZ];
	    int numpoles, numzeros;
	};

	pzrep splane = new pzrep();
	pzrep zplane = new pzrep();
	int order;
	double raw_alpha1, raw_alpha2, raw_alphaz;
	complex dc_gain, fc_gain, hf_gain;
	int options;
	double warped_alpha1, warped_alpha2, chebrip, qfactor;
	boolean infq;
	int polemask;
	double [] xcoeffs = new double[MAXPZ+1];
	double [] ycoeffs = new double[MAXPZ+1];
	
	final int opt_be = 0x00001;	/* -Be		Bessel characteristic	       */
	final int opt_bu = 0x00002;	/* -Bu		Butterworth characteristic     */
	final int opt_ch = 0x00004;	/* -Ch		Chebyshev characteristic       */
	final int opt_re = 0x00008;	/* -Re		Resonator		       */
	final int opt_pi = 0x00010;	/* -Pi		proportional-integral	       */

	final int opt_lp = 0x00020;	/* -Lp		lowpass			       */
	final int opt_hp = 0x00040;	/* -Hp		highpass		       */
	final int opt_bp = 0x00080;	/* -Bp		bandpass		       */
	final int opt_bs = 0x00100;	/* -Bs		bandstop		       */
	final int opt_ap = 0x00200;	/* -Ap		allpass			       */

	final int opt_a = 0x00400;	/* -a		alpha value		       */
	final int opt_l = 0x00800;	/* -l		just list filter parameters    */
	final int opt_o = 0x01000;	/* -o		order of filter		       */
	final int opt_p = 0x02000;	/* -p		specified poles only	       */
	final int opt_w = 0x04000;	/* -w		don't pre-warp		       */
	final int opt_z = 0x08000;	/* -z		use matched z-transform	       */
	final int opt_Z = 0x10000;	/* -Z		additional zero		       */	
/*
	static complex bessel_poles[] =
	  { // table produced by /usr/fisher/bessel --	N.B. only one member of each C.Conj. pair is listed 
	    { -1.00000000000e+00, 0.00000000000e+00}, { -1.10160133059e+00, 6.36009824757e-01},
	    { -1.32267579991e+00, 0.00000000000e+00}, { -1.04740916101e+00, 9.99264436281e-01},
	    { -1.37006783055e+00, 4.10249717494e-01}, { -9.95208764350e-01, 1.25710573945e+00},
	    { -1.50231627145e+00, 0.00000000000e+00}, { -1.38087732586e+00, 7.17909587627e-01},
	    { -9.57676548563e-01, 1.47112432073e+00}, { -1.57149040362e+00, 3.20896374221e-01},
	    { -1.38185809760e+00, 9.71471890712e-01}, { -9.30656522947e-01, 1.66186326894e+00},
	    { -1.68436817927e+00, 0.00000000000e+00}, { -1.61203876622e+00, 5.89244506931e-01},
	    { -1.37890321680e+00, 1.19156677780e+00}, { -9.09867780623e-01, 1.83645135304e+00},
	    { -1.75740840040e+00, 2.72867575103e-01}, { -1.63693941813e+00, 8.22795625139e-01},
	    { -1.37384121764e+00, 1.38835657588e+00}, { -8.92869718847e-01, 1.99832584364e+00},
	    { -1.85660050123e+00, 0.00000000000e+00}, { -1.80717053496e+00, 5.12383730575e-01},
	    { -1.65239648458e+00, 1.03138956698e+00}, { -1.36758830979e+00, 1.56773371224e+00},
	    { -8.78399276161e-01, 2.14980052431e+00}, { -1.92761969145e+00, 2.41623471082e-01},
	    { -1.84219624443e+00, 7.27257597722e-01}, { -1.66181024140e+00, 1.22110021857e+00},
	    { -1.36069227838e+00, 1.73350574267e+00}, { -8.65756901707e-01, 2.29260483098e+00},
	  };
*/
	
	boolean optsok;

	void setdefaults()
	{
		if (flag(options , opt_p)==false )
			polemask = ~0; /* use all poles */

		if (flag(options , (opt_bp | opt_bs))==false)
			raw_alpha2 = raw_alpha1;
	}	
	
	void main(String s)
	{ 
		String [] argv = s.split(" ");
		int argc = argv.length;
		readcmdline(argv);
		checkoptions();
		setdefaults();
		if (flag(options , opt_re))
		{
			if (flag(options , opt_bp)) 
				compute_bpres();	   /* bandpass resonator	 */
			if (flag(options , opt_bs)) 
				compute_notch();	   /* bandstop resonator (notch) */
			if (flag(options , opt_ap)) 
				compute_apres();	   /* allpass resonator		 */
		}
		else
		{ 
			if (flag(options , opt_pi))
			{ 
				prewarp();
				splane.poles[0] = new complex(0.0);
				splane.zeros[0] = new complex(-Math.PI * 2.0f * warped_alpha1);
				splane.numpoles = splane.numzeros = 1;
			}
			else
			{
				compute_s();
				prewarp();
				normalize();
			}
		
			if (flag(options , opt_z)) 
				compute_z_mzt(); 
			else 
				compute_z_blt();
		}
			
		if (flag(options, opt_Z)) 
			add_extra_zero();

		expandpoly();
		printresults(argv);
	}	
	
	void readcmdline(String [] argv)
	{ 
		options = order = polemask = 0;
	    int ap = 0;
	    if (argv[ap] != null) ap++; /* skip program name */
	    while(ap < argv.length)
	    { 
	    	int m = decodeoptions(argv[ap++]);
			if (flag(m , opt_ch)) chebrip = getfarg(argv[ap++]);
			if (flag(m , opt_a))
			{
				raw_alpha1 = getfarg(argv[ap++]);
			    raw_alpha2 = ((ap < argv.length) && (argv[ap].substring(0, 1) != "-")) ? getfarg(argv[ap++]) : raw_alpha1;
			}
			if (flag(m , opt_Z)) raw_alphaz = getfarg(argv[ap++]);
			if (flag(m , opt_o)) order = getiarg(argv[ap++]);
			if (flag(m , opt_p))
			{ 
				int p = (int)Double.parseDouble(argv[ap++]);
				if (p < 0 || p > 31) p = 31; /* out-of-range value will be picked up later */
				polemask |= (1 << p);
			}
					
			if (flag(m , opt_re))
			{ 
				String s = argv[ap++];
				if ((ap < argv.length) && seq(argv[ap++],"Inf")) 
				{
					infq = true;
				}
				else 
				{ 
					qfactor = getfarg(s); infq = false; 
				}
			}
			options |= m;
	    }
	}	

	boolean flag(int v,int opt)
	{
		int r = (v & opt); 
		return  (r > 0);
	}
	
	
	boolean seq(String s1,String s2)
	{
		return s1.compareTo(s2)==0;
	}
	
	int decodeoptions(String s)
	{ 
		int m = 0;
		
		if (seq(s,"-Be")) m |= opt_be;
		else if (seq(s, "-Bu")) m |= opt_bu;
		else if (seq(s, "-Ch")) m |= opt_ch;
		else if (seq(s, "-Re")) m |= opt_re;
		else if (seq(s, "-Pi")) m |= opt_pi;
		else if (seq(s, "-Lp")) m |= opt_lp;
		else if (seq(s, "-Hp")) m |= opt_hp;
		else if (seq(s, "-Bp")) m |= opt_bp;
		else if (seq(s, "-Bs")) m |= opt_bs;
		else if (seq(s, "-Ap")) m |= opt_ap;
		else
		{ 		
		  for(int i=0;i<s.length();i++)
		  { 
			int bit = optbit(s.charAt(i));
		    //if (bit == 0) usage();
		    m |= bit;
		  }
		  
		  }
		return m;
	}
	
	int optbit(char c)
	{ 
		switch (c)
	    {
			default:    return 0;
			case 'a':   return opt_a;
			case 'l':   return opt_l;
			case 'o':   return opt_o;
			case 'p':   return opt_p;
			case 'w':   return opt_w;
			case 'z':   return opt_z;
			case 'Z':   return opt_Z;
	    }
	}

	double getfarg(String s)
	{
		return Double.parseDouble(s);
	}

	int getiarg(String s)
	{ 
    	return (int)Double.parseDouble(s);
	}	
	
	boolean onebit(int m)	    { return (m != 0) && ((m & m-1) == 0);     }	
	
	void opterror(String s)
	{
	
	}
	
	void checkoptions()
	{ 
	    optsok = true;
	    if (onebit(options & (opt_be | opt_bu | opt_ch | opt_re | opt_pi))==false)
	      opterror("must specify exactly one of -Be, -Bu, -Ch, -Re, -Pi");

	    if ( (options & opt_re) >0)
	    { 
	        if (onebit(options & (opt_bp | opt_bs | opt_ap))==false)
	        opterror("must specify exactly one of -Bp, -Bs, -Ap with -Re");
	        if ( (options & (opt_lp | opt_hp | opt_o | opt_p | opt_w | opt_z)) >0)
	            opterror("can't use -Lp, -Hp, -o, -p, -w, -z with -Re");
	    }
	    else if ((options & opt_pi)>0)
	    { 
	        if ((options & (opt_lp | opt_hp | opt_bp | opt_bs | opt_ap))==0)
	            opterror("-Lp, -Hp, -Bp, -Bs, -Ap illegal in conjunction with -Pi");
            if ((((options & opt_o)>0) && (order == 1))==false)
            	opterror("-Pi implies -o 1");
	    }
	    else
	    { 
	        if(onebit(options & (opt_lp | opt_hp | opt_bp | opt_bs))==false)
	        {
	        	opterror("must specify exactly one of -Lp, -Hp, -Bp, -Bs");
	        }
	        if ((options & opt_ap)>0)
	        {
	        	opterror("-Ap implies -Re");
	        }
	        
	        if ((options & opt_o) >0)
	        { 
	        	/*
	            if ( (order >= 1 && order <= MAXORDER)==false) 
	            	opterror("order must be in range 1 .. %d", MAXORDER);
	            if ((options & opt_p) >0)
	            { 
	                int m = (1 << order) - 1; // "order" bits set 
	                if ((polemask & ~m) != 0)
	                	opterror("order=%d, so args to -p must be in range 0 .. %d", order, order-1);
	            }
	        	*/
	        }
	        else
	        {
	        	opterror("must specify -o");
	        }
	    }
	    if ((options & opt_a) >0)
	    {
	    	opterror("must specify -a");
	    }
	    if (optsok==false) 
	    	return;
	}	
	
	double asinh(double x)
	{ /* Microsoft C++ does not define */
	    return Math.log(x + Math.sqrt(1.0 + x*x));
	}
	
	void compute_s() /* compute S-plane poles for prototype LP filter */
	{
	    splane.numpoles = 0;
	    /*
	    // Bessel filter 
	    if (options & opt_be)
	    { 
	        int p = (order*order)/4; // ptr into table
	        if (order & 1) choosepole(bessel_poles[p++]);
	        for (int i = 0; i < order/2; i++)
	        { 
	            choosepole(bessel_poles[p]);
	            choosepole(cconj(bessel_poles[p]));
	            p++;
	        }
	    }
		*/
	    if (flag(options , (opt_bu | opt_ch)))
	    { /* Butterworth filter */
	        for (int i = 0; i < 2*order; i++)
	        { 
	            double theta = ((order & 1)==1) ? (i*Math.PI) / order : ((i+0.5)*Math.PI) / order;
	            choosepole(complex.expj(theta));
	        }
	    }
	    
	    if (flag(options , opt_ch))
	    { // modify for Chebyshev (p. 136 DeFatta et al.) 
	        if (chebrip >= 0.0)
	        {
	            //fprintf(stderr, "mkfilter: Chebyshev ripple is %g dB; must be .lt. 0.0\n", chebrip);
	            //exit(1);
	        }
	        double rip = Math.pow(10.0, -chebrip / 10.0);
	        double eps = Math.sqrt(rip - 1.0);
	        double y = asinh(1.0 / eps) / (double) order;
	        if (y <= 0.0)
	        { 
	            //fprintf(stderr, "mkfilter: bug: Chebyshev y=%g; must be .gt. 0.0\n", y);
	            //exit(1);
	        }
	        for (int i = 0; i < splane.numpoles; i++)
	        {
	            splane.poles[i].re *= Math.sinh(y);
	            splane.poles[i].im *= Math.cosh(y);
	        }
	    }
	    
	}

	void choosepole(complex z)
	{ 
	    if (z.re < 0.0)
	    { 
	        if ((polemask & 1)==1)
	        {
	        	splane.poles[splane.numpoles++] = z;
	        }
	        polemask >>= 1;
	    }
	}

	void prewarp()
	{ 
	    /* for bilinear transform, perform pre-warp on alpha values */
	    if (flag(options ,(opt_w | opt_z)))
	    {
	        warped_alpha1 = raw_alpha1;
	        warped_alpha2 = raw_alpha2;
	    }
	    else
	    { 
	        warped_alpha1 = Math.tan(Math.PI * raw_alpha1) / Math.PI;
	        warped_alpha2 = Math.tan(Math.PI * raw_alpha2) / Math.PI;
	    }
	}

	void normalize()		/* called for trad, not for -Re or -Pi */
	{
	    double w1 = (2.0 * Math.PI) * warped_alpha1;
	    double w2 = (2.0 * Math.PI) * warped_alpha2;
	    /* transform prototype into appropriate filter type (lp/hp/bp/bs) */
	    switch (options & (opt_lp | opt_hp | opt_bp| opt_bs))
	    { 
	        case opt_lp:
	        {
	            for (int i = 0; i < splane.numpoles; i++) splane.poles[i] = complex.mul( w1,splane.poles[i]);
	            splane.numzeros = 0;
	            break;
	        }

	        case opt_hp:
	        {
	            int i;
	            for (i=0; i < splane.numpoles; i++) splane.poles[i] = complex.div( new complex(w1) , splane.poles[i]);
	            for (i=0; i < splane.numpoles; i++) splane.zeros[i] = new complex(0.0);	 /* also N zeros at (0,0) */
	            splane.numzeros = splane.numpoles;
	            break;
	        }

	        case opt_bp:
	        {
	        	complex w0 = new complex(Math.sqrt(w1*w2)); 
	            complex bw = new complex(w2-w1); 
	            int i;
	            for (i=0; i < splane.numpoles; i++)
	            {
	                complex hba = complex.mul(0.5 , complex.mul(splane.poles[i] , bw));
	                complex temp = complex.csqrt(complex.sub(new complex(1.0) ,complex.sqr(complex.div(w0 , hba))));
	                splane.poles[i] = complex.mul(hba , complex.add(1.0 , temp));
	                splane.poles[splane.numpoles+i] = complex.mul(hba , complex.sub(new complex(1.0) , temp));
	            }
	            for (i=0; i < splane.numpoles; i++) splane.zeros[i] = new complex(0.0);	 /* also N zeros at (0,0) */
	            splane.numzeros = splane.numpoles;
	            splane.numpoles *= 2;
	            break;
	        }

	        case opt_bs:
	        { 
	            complex w0 = new complex(Math.sqrt(w1*w2)); 
	            complex bw = new complex(w2-w1); 
	            int i;
	            for (i=0; i < splane.numpoles; i++)
	            {
	                complex hba = complex.mul(0.5 , complex.div(bw , splane.poles[i]));
	                complex temp = complex.csqrt(complex.sub(new complex(1.0) , complex.sqr(complex.div(w0 , hba))));
	                splane.poles[i] = complex.mul(hba , complex.add(1.0 , temp));
	                splane.poles[splane.numpoles+i] = complex.mul(hba , complex.sub(new complex(1.0) , temp));
	            }
	            
	            for (i=0; i < splane.numpoles; i++)	   /* also 2N zeros at (0, +-w0) */
	            {
	                splane.zeros[i] = new complex(0.0, +0);
	                splane.zeros[splane.numpoles+i] = new complex(0.0, -0);
	            }
	            splane.numpoles *= 2;
	            splane.numzeros = splane.numpoles;
	            break;
	        }
	    }
	}

	void compute_z_blt() /* given S-plane poles & zeros, compute Z-plane poles & zeros, by bilinear transform */
	{
	    int i;
	    zplane.numpoles = splane.numpoles;
	    zplane.numzeros = splane.numzeros;
	    for (i=0; i < zplane.numpoles; i++) 
	    	zplane.poles[i] = blt(splane.poles[i]);
	    for (i=0; i < zplane.numzeros; i++) 
	    	zplane.zeros[i] = blt(splane.zeros[i]);
	    while (zplane.numzeros < zplane.numpoles) 
	        zplane.zeros[zplane.numzeros++] = new complex(-1.0);
	}

	static complex blt(complex pz)
	{
		complex c1 = complex.add(2.0 , pz);
		complex c2 = complex.sub(2.0 , pz);
		
		complex res = complex.div( c1, c2); 
		
	    return res;
	}

	void compute_z_mzt() /* given S-plane poles & zeros, compute Z-plane poles & zeros, by matched z-transform */
	{   
	    int i;
	    zplane.numpoles = splane.numpoles;
	    zplane.numzeros = splane.numzeros;
	    for (i=0; i < zplane.numpoles; i++) zplane.poles[i] = complex.cexp(splane.poles[i]);
	    for (i=0; i < zplane.numzeros; i++) zplane.zeros[i] = complex.cexp(splane.zeros[i]);
	}

	void compute_notch()
	{ /* compute Z-plane pole & zero positions for bandstop resonator (notch filter) */
	    compute_bpres();		/* iterate to place poles */
	    double theta = (2.0 * Math.PI) * raw_alpha1;
	    complex zz = complex.expj(theta);	/* place zeros exactly */
	    zplane.zeros[0] = zz; zplane.zeros[1] = complex.cconj(zz);
	}

	void compute_apres()
	{ /* compute Z-plane pole & zero positions for allpass resonator */
	    compute_bpres();		/* iterate to place poles */
	    zplane.zeros[0] = reflect(zplane.poles[0]);
	    zplane.zeros[1] = reflect(zplane.poles[1]);
	}

	complex reflect(complex z)
	{ 
	    double r = complex.hypot(z);
	    return complex.div(z , r*r);
	}

	void compute_bpres()
	{ 
	    /* compute Z-plane pole & zero positions for bandpass resonator */
	    zplane.numpoles = zplane.numzeros = 2;
	    zplane.zeros[0] = new complex(1.0); 
	    zplane.zeros[1] = new complex(-1.0);
	    double theta = (2.0 * Math.PI) * raw_alpha1; /* where we want the peak to be */
	    if (infq)
	    { /* oscillator */
	        complex zp = complex.expj(theta);
	        zplane.poles[0] = zp; 
	        zplane.poles[1] = complex.cconj(zp);
	    }
	    else
	    { /* must iterate to find exact pole positions */
	        complex [] topcoeffs = new complex[MAXPZ+1]; 
	        expand(zplane.zeros, zplane.numzeros, topcoeffs);
	        double r = Math.exp(-theta / (2.0 * qfactor));
	        double thm = theta, th1 = 0.0, th2 = Math.PI;
	        boolean cvg = false;
	        for (int i=0; i < 50 && !cvg; i++)
	        {
	            complex zp = complex.mul(r , complex.expj(thm));
	            zplane.poles[0] = zp; zplane.poles[1] = complex.cconj(zp);
	            complex [] botcoeffs = new complex[MAXPZ+1]; 
	            expand(zplane.poles, zplane.numpoles, botcoeffs);
	            complex g = complex.evaluate(topcoeffs, zplane.numzeros, botcoeffs, zplane.numpoles, complex.expj(theta));
	            double phi = g.im / g.re; /* approx to atan2 */
	            if (phi > 0.0) th2 = thm; else th1 = thm;
	            if (Math.abs(phi) < EPS) cvg = true;
	            thm = 0.5 * (th1+th2);
	        }
	        //unless (cvg) fprintf(stderr, "mkfilter: warning: failed to converge\n");
	    }
	}

	void add_extra_zero()
	{ 
	    if (zplane.numzeros+2 > MAXPZ)
	    { 
	        //fprintf(stderr, "mkfilter: too many zeros; can't do -Z\n");
	        //exit(1);
	    }
	    double theta = (2.0 * Math.PI) * raw_alphaz;
	    complex zz = complex.expj(theta);
	    zplane.zeros[zplane.numzeros++] = zz;
	    zplane.zeros[zplane.numzeros++] = complex.cconj(zz);
	    while (zplane.numpoles < zplane.numzeros) zplane.poles[zplane.numpoles++] = new complex(0.0);	 /* ensure causality */
	}

	void expandpoly() /* given Z-plane poles & zeros, compute top & bot polynomials in Z, and then recurrence relation */
	{
	    complex [] topcoeffs = new complex [MAXPZ+1];
	    complex [] botcoeffs = new complex [MAXPZ+1];
	    int i;
	    expand(zplane.zeros, zplane.numzeros, topcoeffs);
	    expand(zplane.poles, zplane.numpoles, botcoeffs);
	    dc_gain = complex.evaluate(topcoeffs, zplane.numzeros, botcoeffs, zplane.numpoles, new complex(1.0));
	    double theta = (2.0 * Math.PI) * 0.5 * (raw_alpha1 + raw_alpha2); /* "jwT" for centre freq. */
	    fc_gain = complex.evaluate(topcoeffs, zplane.numzeros, botcoeffs, zplane.numpoles, complex.expj(theta));
	    hf_gain = complex.evaluate(topcoeffs, zplane.numzeros, botcoeffs, zplane.numpoles, new complex(-1.0));
	    for (i = 0; i <= zplane.numzeros; i++) xcoeffs[i] = +(topcoeffs[i].re / botcoeffs[zplane.numpoles].re);
	    for (i = 0; i <= zplane.numpoles; i++) ycoeffs[i] = -(botcoeffs[i].re / botcoeffs[zplane.numpoles].re);
	}

	void expand(complex pz[], int npz, complex coeffs[])
	{ /* compute product of poles or zeros as a polynomial of z */
	    int i;
	    coeffs[0] = new complex(1.0);
	    for (i=0; i < npz; i++) coeffs[i+1] = new complex(0.0);
	    for (i=0; i < npz; i++) multin(pz[i], npz, coeffs);
	    /* check computed coeffs of z^k are all real */
	    for (i=0; i < npz+1; i++)
	    { 
	        if (Math.abs(coeffs[i].im) > EPS)
	        { 
	            //fprintf(stderr, "mkfilter: coeff of z^%d is not real; poles/zeros are not complex conjugates\n", i);
	            //exit(1);
	        }
	    }
	}

	void multin(complex w, int npz, complex coeffs[])
	{ /* multiply factor (z-w) into coeffs */
	    complex nw = complex.sub(new complex(0), w);
	    for (int i = npz; i >= 1; i--) 
	    {
	        coeffs[i] = complex.add(complex.mul(nw , coeffs[i]) , coeffs[i-1]);
	    }
	    coeffs[0] = complex.mul(nw , coeffs[0]);
	}
	
	void printresults(String argv[])
	{ 
		/*
	    if (options & opt_l)
	    { // just list parameters 
	        printcmdline(argv);
	        complex gain = (options & opt_pi) ? hf_gain :
	                (options & opt_lp) ? dc_gain :
	                (options & opt_hp) ? hf_gain :
	                (options & (opt_bp | opt_ap)) ? fc_gain :
	                (options & opt_bs) ? csqrt(dc_gain * hf_gain) : complex(1.0);
	        printf("G  = %.10e\n", hypot(gain));
	        printcoeffs("NZ", zplane.numzeros, xcoeffs);
	        printcoeffs("NP", zplane.numpoles, ycoeffs);
	    }
	    else
	    { 
	        printf("Command line: ");
	        printcmdline(argv);
	        printfilter();
	    }
	    */
	}

	void printcmdline(char argv[])
	{ 
		/*
	    int k = 0;
	    until (argv[k] == NULL)
	    { 
	        if (k > 0) putchar(' ');
	        fputs(argv[k++], stdout);
	    }
	    putchar('\n');
	    */
	}

	void printcoeffs(String pz, int npz, double coeffs[])
	{ 
		/*
	    printf("%s = %d\n", pz, npz);
	    for (int i = 0; i <= npz; i++) printf("%18.10e\n", coeffs[i]);
	    */
	}

	void printfilter()
	{ 
		/*
	    printf("raw alpha1    = %14.10f\n", raw_alpha1);
	    printf("raw alpha2    = %14.10f\n", raw_alpha2);
	    unless (options & (opt_re | opt_w | opt_z))
	    { 
	        printf("warped alpha1 = %14.10f\n", warped_alpha1);
	        printf("warped alpha2 = %14.10f\n", warped_alpha2);
	    }
	    printgain("dc    ", dc_gain);
	    printgain("centre", fc_gain);
	    printgain("hf    ", hf_gain);
	    putchar('\n');
	    unless (options & opt_re) printrat_s();
	    printrat_z();
	    printrecurrence();
	    */
	}

	void printgain(String str, complex gain)
	{ 
		/*
	    double r = hypot(gain);
	    printf("gain at %s:   mag = %15.9e", str, r);
	    if (r > EPS) printf("   phase = %14.10f pi", atan2(gain) / Math.PI);
	    putchar('\n');
	    */
	}

	void printrat_s()	/* print S-plane poles and zeros */
	{ 
		/*
	    printf("S-plane zeros:\n");
	    printpz(splane.zeros, splane.numzeros);
	    printf("S-plane poles:\n");
	    printpz(splane.poles, splane.numpoles);
	    */
	}

	void printrat_z()	/* print Z-plane poles and zeros */
	{ 
		/*
	    printf("Z-plane zeros:\n");
	    printpz(zplane.zeros, zplane.numzeros);
	    printf("Z-plane poles:\n");
	    printpz(zplane.poles, zplane.numpoles);
	    */
	 }

	void printpz(complex pzvec[], int num)
	{ 
		/*
	    int n1 = 0;
	    while (n1 < num)
	    { 
	        putchar('\t'); 
	        prcomplex(pzvec[n1]);
	        int n2 = n1+1;
	        while (n2 < num && pzvec[n2] == pzvec[n1]) n2++;
	        if (n2-n1 > 1) printf("\t%d times", n2-n1);
	        putchar('\n');
	        n1 = n2;
	    }
	    putchar('\n');
	    */
	}

	void printrecurrence() /* given (real) Z-plane poles & zeros, compute & print recurrence relation */
	{ 
		/*
	    printf("Recurrence relation:\n");
	    printf("y[n] = ");
	    int i;
	    for (i = 0; i < zplane.numzeros+1; i++)
	    { 
	        if (i > 0) printf("     + ");
	        double x = xcoeffs[i];
	        double f = fmod(fabs(x), 1.0);
	        char *fmt = (f < EPS || f > 1.0-EPS) ? "%3g" : "%14.10f";
	        putchar('('); printf(fmt, x); printf(" * x[n-%2d])\n", zplane.numzeros-i);
	    }
	    putchar('\n');
	    for (i = 0; i < zplane.numpoles; i++)
	    { 
	        printf("     + (%14.10f * y[n-%2d])\n", ycoeffs[i], zplane.numpoles-i);
	    }
	    putchar('\n');
	    */
	}

	BandPass GetBandPass()
	{
		
		return new BandPass((float)ycoeffs[0],(float)ycoeffs[1],(float)complex.hypot(fc_gain));		
	}
	
	
	void prcomplex(complex z)
	{ 
		/*
	    printf("%14.10f + j %14.10f", z.re, z.im);
	    */
	}	
}
