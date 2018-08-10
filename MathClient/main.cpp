// MathClient.cpp : Defines the entry point for the console application.  
// Compile by using: cl /EHsc /link MathLibrary.lib MathClient.cpp  

#include "stdafx.h"  
#include <iostream>  
#include "OpenCLImageAnalyse.h"  
#include <time.h>
#include <chrono>
#include <thread>

using namespace std;


void testMatMul()
{
	clock_t tStart = clock();
	OpenCLImageAnalyse::MatMulV1Call();
	printf("Time taken: %.2fs\n", (double)(clock() - tStart) / CLOCKS_PER_SEC);
	std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	tStart = clock();
	OpenCLImageAnalyse::MatMulV2Call();
	printf("Time taken: %.2fs\n", (double)(clock() - tStart) / CLOCKS_PER_SEC);
	std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	tStart = clock();
	OpenCLImageAnalyse::MatMulV3Call();
	printf("Time taken: %.2fs\n", (double)(clock() - tStart) / CLOCKS_PER_SEC);
}

void testBitmapAnalyse()
{
	int width = 20;
	int height = 20;
	char* ptr0 = (char*)malloc(sizeof(char) * width * height * 3);
	int arSize = 5;
	char** ar = (char**)malloc(sizeof(char*) * arSize);
	for (size_t i = 0; i < arSize; i++)
	{
		ar[i] = (char*)malloc(sizeof(char) * width * height * 3);
	}
	int* asd = (int*)malloc(sizeof(int) * arSize);
	OpenCLImageAnalyse::BitmapAnalyse((unsigned char*)ptr0, width, height, (void**)ar, arSize, asd);
	for (size_t i = 0; i < arSize; i++)
	{
		int zxc = *(asd + i);
		int ghj = 2;
	}
}

void testBitmapAnalyseV2()
{
	const int width = 20;
	const int height = 20;
	const int samplesSize = 2;
	const int referencesSize = 6;

	char** samples = (char**)malloc(sizeof(char*) * samplesSize);
	char** references = (char**)malloc(sizeof(char*) * referencesSize);
	int** retValues = (int**)malloc(sizeof(int) * samplesSize);

	for (size_t i = 0; i < samplesSize; i++)
	{
		*(samples + i) = (char*)malloc(width * 3 * height * sizeof(char));
		*(retValues + i) = (int*)malloc(referencesSize * sizeof(int));
	}
	for (size_t i = 0; i < referencesSize; i++)
	{
		*(references + i) = (char*)malloc(width * 3 * height * sizeof(char));
	}

	for (size_t k = 0; k < samplesSize; k++)
	{
		for (size_t i = 0; i < width * 3; i++)
		{
			for (size_t j = 0; j < height; j++)
			{
				*(((char*)(*(((char**)samples) + k))) + i*height + j) = k + 3;
			}
		}
	}
	for (size_t k = 0; k < referencesSize; k++)
	{
		for (size_t i = 0; i < width * 3; i++)
		{
			for (size_t j = 0; j < height; j++)
			{
				*(((char*)(*(((char**)references) + k))) + i*height + j) = k + 1;
			}
		}
	}
	for (size_t k = 0; k < samplesSize; k++)
	{
		for (size_t i = 0; i < referencesSize; i++)
		{
			*(((int*)(*(((int**)retValues) + k))) + i) = 0;
		}
	}

	OpenCLImageAnalyse::BitmapAnalyseV2((void**)samples, samplesSize, (void**)references, referencesSize, width, height, retValues);
	int qwe[samplesSize][referencesSize];
	for (size_t k = 0; k < samplesSize; k++)
	{
		for (size_t i = 0; i < referencesSize; i++)
		{
			qwe[k][i] = *(((int*)(*(((int**)retValues) + k))) + i);
		}
	}

	for (size_t i = 0; i < samplesSize; i++)
	{
		free(*(samples + i));
		free(*(retValues + i));
	}
	for (size_t i = 0; i < referencesSize; i++)
	{
		free(*(references + i));
	}

	free(samples);
	free(references);
	free(retValues);
}

int main()
{
	testBitmapAnalyseV2();
	return 0;
}
