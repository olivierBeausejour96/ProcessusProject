// MathLibrary.h - Contains declaration of Function class  
#pragma once  

#ifdef MYEXECREFSDll_EXPORTS
#define MYEXECREFSDll_API __declspec(dllexport) 
#else
#define MYEXECREFSDll_API __declspec(dllimport) 
#endif

namespace OpenCLImageAnalyse
{  
	//test functions
	extern "C" MYEXECREFSDll_API int OCL();
	extern "C" MYEXECREFSDll_API void MatMulV1Call();
	extern "C" MYEXECREFSDll_API void MatMulV2Call();
	extern "C" MYEXECREFSDll_API void MatMulV3Call();

	//ImageAnalyse
	extern "C" MYEXECREFSDll_API int BitmapAnalyse(unsigned char* ptr0, int width, int heigth, void** ar, int arSize, int* retValues);
	extern "C" MYEXECREFSDll_API int BitmapAnalyseV2(void** samples, int samplesSize, void** references, int referencesSize, int imgWidth, int imgHeigth, int** retValues);
}  
