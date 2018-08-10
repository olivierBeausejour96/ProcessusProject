  __kernel void BitmapDistances(
    __global int* sample, __global int* refImgs,
    __global int* retDif, const int retDifSize, const int stride, const int refImgSize)
    {
	     int maxValue = retDifSize/sizeof(int)/sizeof(int);
       int globalId0 = get_global_id(0);
       int globalId1 = get_global_id(1);
	     int globalInd = globalId0 * stride + globalId1;

	     if (globalInd >= maxValue)
		       return;
       globalInd *= 4;

       int sampleInd = globalInd % (refImgSize);
       unsigned int val1 = (unsigned int)*(sample + (sampleInd/4));
	     unsigned int val2 = (unsigned int)*(refImgs + (globalInd/4));

       int mask = 0xFF;
       *(retDif + globalInd + 0) = ((val1 & mask) - (val2 & mask)) * ((val1 & mask) - (val2 & mask));

       mask = mask << 8;
       *(retDif + globalInd + 1) = (((val1 & mask) >> 8) - ((val2 & mask) >> 8)) * (((val1 & mask) >> 8) - ((val2 & mask) >> 8));

       mask = mask << 8;
       *(retDif + globalInd + 2) = (((val1 & mask) >> 16) - ((val2 & mask) >> 16)) * (((val1 & mask) >> 16) - ((val2 & mask) >> 16));

       mask = mask << 8;
       *(retDif + globalInd + 3) = (((val1 & mask) >> 24) - ((val2 & mask) >> 24)) * (((val1 & mask) >> 24) - ((val2 & mask) >> 24));
    }
