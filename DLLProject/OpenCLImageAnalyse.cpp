// MathLibrary.cpp : Defines the exported functions for the DLL application.
// Compile by using: cl /EHsc /DMATHLIBRARY_EXPORTS /LD MathLibrary.cpp

#include "stdafx.h"
#include "OpenCLImageAnalyse.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h> 

#ifdef __APPLE__
#include <OpenCL/opencl.h>
#else
#include <CL/cl.h>
#endif

#define MAX_SOURCE_SIZE (0x100000)

namespace OpenCLImageAnalyse
{
	void CLPrintDevInfo(cl_device_id device) {
		char device_string[1024];

		// CL_DEVICE_NAME
		clGetDeviceInfo(device, CL_DEVICE_NAME, sizeof(device_string), &device_string, NULL);
		printf("  CL_DEVICE_NAME: \t\t\t%s\n", device_string);

		// CL_DEVICE_VENDOR
		clGetDeviceInfo(device, CL_DEVICE_VENDOR, sizeof(device_string), &device_string, NULL);
		printf("  CL_DEVICE_VENDOR: \t\t\t%s\n", device_string);

		// CL_DRIVER_VERSION
		clGetDeviceInfo(device, CL_DRIVER_VERSION, sizeof(device_string), &device_string, NULL);
		printf("  CL_DRIVER_VERSION: \t\t\t%s\n", device_string);

		// CL_DEVICE_INFO
		cl_device_type type;
		clGetDeviceInfo(device, CL_DEVICE_TYPE, sizeof(type), &type, NULL);
		if (type & CL_DEVICE_TYPE_CPU)
			printf("  CL_DEVICE_TYPE:\t\t\t%s\n", "CL_DEVICE_TYPE_CPU");
		if (type & CL_DEVICE_TYPE_GPU)
			printf("  CL_DEVICE_TYPE:\t\t\t%s\n", "CL_DEVICE_TYPE_GPU");
		if (type & CL_DEVICE_TYPE_ACCELERATOR)
			printf("  CL_DEVICE_TYPE:\t\t\t%s\n", "CL_DEVICE_TYPE_ACCELERATOR");
		if (type & CL_DEVICE_TYPE_DEFAULT)
			printf("  CL_DEVICE_TYPE:\t\t\t%s\n", "CL_DEVICE_TYPE_DEFAULT");

		// CL_DEVICE_MAX_COMPUTE_UNITS
		cl_uint compute_units;
		clGetDeviceInfo(device, CL_DEVICE_MAX_COMPUTE_UNITS, sizeof(compute_units), &compute_units, NULL);
		printf("  CL_DEVICE_MAX_COMPUTE_UNITS:\t\t%u\n", compute_units);

		size_t preferred_work_group_size = 0;
		auto ret = clGetDeviceInfo(device, CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE, sizeof(preferred_work_group_size), &preferred_work_group_size, NULL);
		printf("  CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE:\t\t%u\n", preferred_work_group_size);

		// CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS
		size_t workitem_dims;
		clGetDeviceInfo(device, CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS, sizeof(workitem_dims), &workitem_dims, NULL);
		printf("  CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS:\t%u\n", workitem_dims);

		// CL_DEVICE_MAX_WORK_ITEM_SIZES
		size_t workitem_size[3];
		clGetDeviceInfo(device, CL_DEVICE_MAX_WORK_ITEM_SIZES, sizeof(workitem_size), &workitem_size, NULL);
		printf("  CL_DEVICE_MAX_WORK_ITEM_SIZES:\t%u / %u / %u \n", workitem_size[0], workitem_size[1], workitem_size[2]);

		// CL_DEVICE_MAX_WORK_GROUP_SIZE
		size_t workgroup_size;
		clGetDeviceInfo(device, CL_DEVICE_MAX_WORK_GROUP_SIZE, sizeof(workgroup_size), &workgroup_size, NULL);
		printf("  CL_DEVICE_MAX_WORK_GROUP_SIZE:\t%u\n", workgroup_size);

		// CL_DEVICE_MAX_CLOCK_FREQUENCY
		cl_uint clock_frequency;
		clGetDeviceInfo(device, CL_DEVICE_MAX_CLOCK_FREQUENCY, sizeof(clock_frequency), &clock_frequency, NULL);
		printf("  CL_DEVICE_MAX_CLOCK_FREQUENCY:\t%u MHz\n", clock_frequency);

		// CL_DEVICE_ADDRESS_BITS
		cl_uint addr_bits;
		clGetDeviceInfo(device, CL_DEVICE_ADDRESS_BITS, sizeof(addr_bits), &addr_bits, NULL);
		printf("  CL_DEVICE_ADDRESS_BITS:\t\t%u\n", addr_bits);

		// CL_DEVICE_MAX_MEM_ALLOC_SIZE
		cl_ulong max_mem_alloc_size;
		clGetDeviceInfo(device, CL_DEVICE_MAX_MEM_ALLOC_SIZE, sizeof(max_mem_alloc_size), &max_mem_alloc_size, NULL);
		printf("  CL_DEVICE_MAX_MEM_ALLOC_SIZE:\t\t%u MByte\n", (unsigned int)(max_mem_alloc_size / (1024 * 1024)));

		// CL_DEVICE_GLOBAL_MEM_SIZE
		cl_ulong mem_size;
		clGetDeviceInfo(device, CL_DEVICE_GLOBAL_MEM_SIZE, sizeof(mem_size), &mem_size, NULL);
		printf("  CL_DEVICE_GLOBAL_MEM_SIZE:\t\t%u MByte\n", (unsigned int)(mem_size / (1024 * 1024)));

		// CL_DEVICE_ERROR_CORRECTION_SUPPORT
		cl_bool error_correction_support;
		clGetDeviceInfo(device, CL_DEVICE_ERROR_CORRECTION_SUPPORT, sizeof(error_correction_support), &error_correction_support, NULL);
		printf("  CL_DEVICE_ERROR_CORRECTION_SUPPORT:\t%s\n", error_correction_support == CL_TRUE ? "yes" : "no");

		// CL_DEVICE_LOCAL_MEM_TYPE
		cl_device_local_mem_type local_mem_type;
		clGetDeviceInfo(device, CL_DEVICE_LOCAL_MEM_TYPE, sizeof(local_mem_type), &local_mem_type, NULL);
		printf("  CL_DEVICE_LOCAL_MEM_TYPE:\t\t%s\n", local_mem_type == 1 ? "local" : "global");

		// CL_DEVICE_LOCAL_MEM_SIZE
		clGetDeviceInfo(device, CL_DEVICE_LOCAL_MEM_SIZE, sizeof(mem_size), &mem_size, NULL);
		printf("  CL_DEVICE_LOCAL_MEM_SIZE:\t\t%u KByte\n", (unsigned int)(mem_size / 1024));

		// CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE
		clGetDeviceInfo(device, CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE, sizeof(mem_size), &mem_size, NULL);
		printf("  CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE:\t%u KByte\n", (unsigned int)(mem_size / 1024));

		// CL_DEVICE_QUEUE_PROPERTIES
		cl_command_queue_properties queue_properties;
		clGetDeviceInfo(device, CL_DEVICE_QUEUE_PROPERTIES, sizeof(queue_properties), &queue_properties, NULL);
		if (queue_properties & CL_QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE)
			printf("  CL_DEVICE_QUEUE_PROPERTIES:\t\t%s\n", "CL_QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE");
		if (queue_properties & CL_QUEUE_PROFILING_ENABLE)
			printf("  CL_DEVICE_QUEUE_PROPERTIES:\t\t%s\n", "CL_QUEUE_PROFILING_ENABLE");

		// CL_DEVICE_IMAGE_SUPPORT
		cl_bool image_support;
		clGetDeviceInfo(device, CL_DEVICE_IMAGE_SUPPORT, sizeof(image_support), &image_support, NULL);
		printf("  CL_DEVICE_IMAGE_SUPPORT:\t\t%u\n", image_support);

		// CL_DEVICE_MAX_READ_IMAGE_ARGS
		cl_uint max_read_image_args;
		clGetDeviceInfo(device, CL_DEVICE_MAX_READ_IMAGE_ARGS, sizeof(max_read_image_args), &max_read_image_args, NULL);
		printf("  CL_DEVICE_MAX_READ_IMAGE_ARGS:\t%u\n", max_read_image_args);

		// CL_DEVICE_MAX_WRITE_IMAGE_ARGS
		cl_uint max_write_image_args;
		clGetDeviceInfo(device, CL_DEVICE_MAX_WRITE_IMAGE_ARGS, sizeof(max_write_image_args), &max_write_image_args, NULL);
		printf("  CL_DEVICE_MAX_WRITE_IMAGE_ARGS:\t%u\n", max_write_image_args);

		// CL_DEVICE_IMAGE2D_MAX_WIDTH, CL_DEVICE_IMAGE2D_MAX_HEIGHT, CL_DEVICE_IMAGE3D_MAX_WIDTH, CL_DEVICE_IMAGE3D_MAX_HEIGHT, CL_DEVICE_IMAGE3D_MAX_DEPTH
		size_t szMaxDims[5];
		printf("\n  CL_DEVICE_IMAGE <dim>");
		clGetDeviceInfo(device, CL_DEVICE_IMAGE2D_MAX_WIDTH, sizeof(size_t), &szMaxDims[0], NULL);
		printf("\t\t\t2D_MAX_WIDTH\t %u\n", szMaxDims[0]);
		clGetDeviceInfo(device, CL_DEVICE_IMAGE2D_MAX_HEIGHT, sizeof(size_t), &szMaxDims[1], NULL);
		printf("\t\t\t\t\t2D_MAX_HEIGHT\t %u\n", szMaxDims[1]);
		clGetDeviceInfo(device, CL_DEVICE_IMAGE3D_MAX_WIDTH, sizeof(size_t), &szMaxDims[2], NULL);
		printf("\t\t\t\t\t3D_MAX_WIDTH\t %u\n", szMaxDims[2]);
		clGetDeviceInfo(device, CL_DEVICE_IMAGE3D_MAX_HEIGHT, sizeof(size_t), &szMaxDims[3], NULL);
		printf("\t\t\t\t\t3D_MAX_HEIGHT\t %u\n", szMaxDims[3]);
		clGetDeviceInfo(device, CL_DEVICE_IMAGE3D_MAX_DEPTH, sizeof(size_t), &szMaxDims[4], NULL);
		printf("\t\t\t\t\t3D_MAX_DEPTH\t %u\n", szMaxDims[4]);

		// CL_DEVICE_PREFERRED_VECTOR_WIDTH_<type>
		printf("  CL_DEVICE_PREFERRED_VECTOR_WIDTH_<t>\t");
		cl_uint vec_width[6];
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_CHAR, sizeof(cl_uint), &vec_width[0], NULL);
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_SHORT, sizeof(cl_uint), &vec_width[1], NULL);
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_INT, sizeof(cl_uint), &vec_width[2], NULL);
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_LONG, sizeof(cl_uint), &vec_width[3], NULL);
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_FLOAT, sizeof(cl_uint), &vec_width[4], NULL);
		clGetDeviceInfo(device, CL_DEVICE_PREFERRED_VECTOR_WIDTH_DOUBLE, sizeof(cl_uint), &vec_width[5], NULL);
		printf("CHAR %u, SHORT %u, INT %u, FLOAT %u, DOUBLE %u\n\n\n",
			vec_width[0], vec_width[1], vec_width[2], vec_width[3], vec_width[4]);
	}

#pragma warning(disable : 4996)
	int OCL()
	{
		printf("started running\n");

		// Create the two input vectors
		int i;
		const int LIST_SIZE = 1024;
		int *A = (int*)malloc(sizeof(int)*LIST_SIZE);
		int *B = (int*)malloc(sizeof(int)*LIST_SIZE);
		for (i = 0; i < LIST_SIZE; i++) {
			A[i] = i;
			B[i] = LIST_SIZE - i;
		}

		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		fp = fopen("C:\\Users\\beao3002\\Desktop\\DLLOpenCLTestProject\\DLLProject\\DLLProject\\Kernel.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		printf("kernel loading done\n");
		// Get platform and device information
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;


		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));

		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);
		printf("ret at %d is %d\n", __LINE__, ret);

		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);

		cl_uint retInfo;
		size_t sizeRet;
		printf("ret at %d is %d\n", __LINE__, ret);
		// Create an OpenCL context
		cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret);
		printf("ret at %d is %d\n", __LINE__, ret);

		// Create a command queue
		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);
		printf("ret at %d is %d\n", __LINE__, ret);

		// Create memory buffers on the device for each vector
		cl_mem a_mem_obj = clCreateBuffer(context, CL_MEM_READ_ONLY,
			LIST_SIZE * sizeof(int), NULL, &ret);
		cl_mem b_mem_obj = clCreateBuffer(context, CL_MEM_READ_ONLY,
			LIST_SIZE * sizeof(int), NULL, &ret);
		cl_mem c_mem_obj = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			LIST_SIZE * sizeof(int), NULL, &ret);

		// Copy the lists A and B to their respective memory buffers
		ret = clEnqueueWriteBuffer(command_queue, a_mem_obj, CL_TRUE, 0,
			LIST_SIZE * sizeof(int), A, 0, NULL, NULL);
		printf("ret at %d is %d\n", __LINE__, ret);

		ret = clEnqueueWriteBuffer(command_queue, b_mem_obj, CL_TRUE, 0,
			LIST_SIZE * sizeof(int), B, 0, NULL, NULL);
		printf("ret at %d is %d\n", __LINE__, ret);

		printf("before building\n");
		// Create a program from the kernel source
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);
		printf("ret at %d is %d\n", __LINE__, ret);

		// Build the program
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);
		printf("ret at %d is %d\n", __LINE__, ret);

		printf("after building\n");
		// Create the OpenCL kernel
		cl_kernel kernel = clCreateKernel(program, "vector_add", &ret);
		printf("ret at %d is %d\n", __LINE__, ret);

		// Set the arguments of the kernel
		ret = clSetKernelArg(kernel, 0, sizeof(cl_mem), (void *)&a_mem_obj);
		printf("ret at %d is %d\n", __LINE__, ret);

		ret = clSetKernelArg(kernel, 1, sizeof(cl_mem), (void *)&b_mem_obj);
		printf("ret at %d is %d\n", __LINE__, ret);

		ret = clSetKernelArg(kernel, 2, sizeof(cl_mem), (void *)&c_mem_obj);
		printf("ret at %d is %d\n", __LINE__, ret);

		//added this to fix garbage output problem
		//ret = clSetKernelArg(kernel, 3, sizeof(int), &LIST_SIZE);

		printf("before execution\n");
		// Execute the OpenCL kernel on the list
		size_t global_item_size = LIST_SIZE; // Process the entire lists
		size_t local_item_size = 64; // Divide work items into groups of 64
		ret = clEnqueueNDRangeKernel(command_queue, kernel, 1, NULL,
			&global_item_size, &local_item_size, 0, NULL, NULL);
		printf("after execution\n");
		// Read the memory buffer C on the device to the local variable C
		int *C = (int*)malloc(sizeof(int)*LIST_SIZE);
		ret = clEnqueueReadBuffer(command_queue, c_mem_obj, CL_TRUE, 0,
			LIST_SIZE * sizeof(int), C, 0, NULL, NULL);
		printf("after copying\n");
		// Display the result to the screen
		for (i = 0; i < LIST_SIZE; i++)
			printf("%d + %d = %d\n", A[i], B[i], C[i]);

		// Clean up
		ret = clFlush(command_queue);
		ret = clFinish(command_queue);
		ret = clReleaseKernel(kernel);
		ret = clReleaseProgram(program);
		ret = clReleaseMemObject(a_mem_obj);
		ret = clReleaseMemObject(b_mem_obj);
		ret = clReleaseMemObject(c_mem_obj);
		ret = clReleaseCommandQueue(command_queue);
		ret = clReleaseContext(context);
		free(A);
		free(B);
		free(C);
		return 0;
	}


	typedef struct {
		int width;
		int height;
		cl_mem elements;
	} MatrixV1;

	// Host code
	// Matrices are stored in row-major order:
	// M(row, col) = *(M.elements + row * M.stride + col)
	typedef struct {
		int width;
		int height;
		int stride;
		cl_mem elements;
	} MatrixV2;


#define BLOCK_SIZE 16
#define BLOCK_SIZE_V3 32
#define MATRIX_DIM 2048

	// Host code
	// Matrices are stored in row-major order:
	// M(row, col) = *(M.elements + row * M.width + col)

	// Thread block size
	// Matrix multiplication - Host code
	// Matrix dimensions are assumed to be multiples of BLOCK_SIZE
	void MatMulHostV1(const MatrixV1 A, const MatrixV1 B, MatrixV1 C,
		const cl_context context,
		const cl_kernel matMulKernel,
		const cl_command_queue queue)
	{
		// Invoke kernel
		cl_uint i = 0;
		clSetKernelArg(matMulKernel, i++,
			sizeof(A.width), (void*)&A.width);
		clSetKernelArg(matMulKernel, i++,
			sizeof(A.height), (void*)&A.height);
		clSetKernelArg(matMulKernel, i++,
			sizeof(A.elements), (void*)&A.elements);
		clSetKernelArg(matMulKernel, i++,
			sizeof(B.width), (void*)&B.width);
		clSetKernelArg(matMulKernel, i++,
			sizeof(B.height), (void*)&B.height);
		clSetKernelArg(matMulKernel, i++,
			sizeof(B.elements), (void*)&B.elements);
		clSetKernelArg(matMulKernel, i++,
			sizeof(C.width), (void*)&C.width);
		clSetKernelArg(matMulKernel, i++,
			sizeof(C.height), (void*)&C.height);
		clSetKernelArg(matMulKernel, i++,
			sizeof(C.elements), (void*)&C.elements);
		size_t localWorkSize[] = { BLOCK_SIZE, BLOCK_SIZE };
		size_t globalWorkSize[] = { A.width, A.width };
		auto ret = clEnqueueNDRangeKernel(queue, matMulKernel, 2, 0,
			globalWorkSize, localWorkSize,
			0, 0, 0);
		auto size = C.width*C.height*sizeof(float);
		float *Cret = (float*)malloc(size);
		ret = clEnqueueReadBuffer(queue, C.elements, CL_TRUE, 0,
			size, Cret, 0, NULL, NULL);
		for (size_t i = 0; i < size / sizeof(float); i++)
		{
			float qwe = *(Cret + i);
			if ((int)qwe != A.width)
			{
				int asd = 2;
			}
		}
		int asd = 2;
	}

	void MatMulV1Call()
	{

		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		fp = fopen("C:\\Users\\beao3002\\Desktop\\DLLOpenCLTestProject\\DLLProject\\DLLProject\\MatrixMulV1.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		// Get platform and device information
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;


		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));

		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);

		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);

		cl_uint retInfo;
		size_t sizeRet;
		// Create an OpenCL context
		cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret);

		// Create a command queue
		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);

		MatrixV1 A, B, C;
		int dim = MATRIX_DIM;
		A.width = C.width = dim;
		A.height = B.width = dim;
		B.height = C.height = dim;
		size_t LIST_SIZE = dim*dim;
		float *floatArrayA = (float*)malloc(sizeof(float) * LIST_SIZE);
		float *floatArrayB = (float*)malloc(sizeof(float) * LIST_SIZE);
		for (size_t i = 0; i < dim * dim; i++)
		{
			*(floatArrayA + i) = 1.0f;
			*(floatArrayB + i) = 1.0f;
		}

		// Create memory buffers on the device for each vector
		A.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			A.width * A.height * sizeof(float), NULL, &ret);

		B.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			B.width * B.height * sizeof(float), NULL, &ret);

		// Copy the lists A and B to their respective memory buffers
		ret = clEnqueueWriteBuffer(command_queue, A.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayA, 0, NULL, NULL);

		ret = clEnqueueWriteBuffer(command_queue, B.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayB, 0, NULL, NULL);


		C.elements = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			C.width * C.height * sizeof(float), NULL, &ret);


		// Create a program from the kernel source
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);

		// Build the program
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);

		// Create the OpenCL kernel
		cl_kernel kernel = clCreateKernel(program, "MatMulKernel", &ret);


		MatMulHostV1(A, B, C, context, kernel, command_queue);

		clReleaseMemObject(A.elements);
		clReleaseMemObject(C.elements);
		clReleaseMemObject(B.elements);
	}

	// Matrix multiplication - Host code
	// Matrix dimensions are assumed to be multiples of BLOCK_SIZE
	void MatMulHostV2(const MatrixV2 A, const MatrixV2 B, MatrixV2 C,
		const cl_context context,
		const cl_kernel matMulKernel,
		const cl_command_queue queue)
	{
		cl_int ret;
		// Invoke kernel
		cl_uint i = 0;
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.width), (void*)&A.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.height), (void*)&A.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.stride), (void*)&A.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.elements), (void*)&A.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.width), (void*)&B.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.height), (void*)&B.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.stride), (void*)&B.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.elements), (void*)&B.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.width), (void*)&C.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.height), (void*)&C.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.stride), (void*)&C.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.elements), (void*)&C.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			BLOCK_SIZE * BLOCK_SIZE * sizeof(float), NULL);
		ret = clSetKernelArg(matMulKernel, i++,
			BLOCK_SIZE * BLOCK_SIZE * sizeof(float), NULL);
		size_t localWorkSize[] = { BLOCK_SIZE, BLOCK_SIZE };
		size_t globalWorkSize[] = { A.width, A.width };
		ret = clEnqueueNDRangeKernel(queue, matMulKernel, 2, 0,
			globalWorkSize, localWorkSize,
			0, 0, 0);
		auto size = C.width*C.height*sizeof(float);
		float *Cret = (float*)malloc(size);
		ret = clEnqueueReadBuffer(queue, C.elements, CL_TRUE, 0,
			size, Cret, 0, NULL, NULL);
		for (size_t i = 0; i < size / sizeof(float); i++)
		{
			float qwe = *(Cret + i);
			if ((int)qwe != A.width)
			{
				int asd = 2;
			}
		}
		int asd = 2;
		//CL_INVALID_PROGRAM_EXECUTABLE
	}

	void MatMulV2Call()
	{

		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		fp = fopen("C:\\Users\\beao3002\\Desktop\\DLLOpenCLTestProject\\DLLProject\\DLLProject\\MatrixMulV2.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		// Get platform and device information
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;


		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));

		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);

		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);

		cl_uint retInfo;
		size_t sizeRet;
		// Create an OpenCL context
		cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret);

		// Create a command queue
		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);

		MatrixV2 A, B, C;
		int dim = MATRIX_DIM;
		A.width = C.width = A.stride = C.stride = dim;
		A.height = B.width = B.stride = dim;
		B.height = C.height = dim;
		size_t LIST_SIZE = dim*dim;
		float *floatArrayA = (float*)malloc(sizeof(float) * LIST_SIZE);
		float *floatArrayB = (float*)malloc(sizeof(float) * LIST_SIZE);
		for (size_t i = 0; i < dim * dim; i++)
		{
			*(floatArrayA + i) = 1.0f;
			*(floatArrayB + i) = 1.0f;
		}

		// Create memory buffers on the device for each vector
		A.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			A.width * A.height * sizeof(float), NULL, &ret);

		B.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			B.width * B.height * sizeof(float), NULL, &ret);

		// Copy the lists A and B to their respective memory buffers
		ret = clEnqueueWriteBuffer(command_queue, A.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayA, 0, NULL, NULL);

		ret = clEnqueueWriteBuffer(command_queue, B.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayB, 0, NULL, NULL);


		C.elements = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			C.width * C.height * sizeof(float), NULL, &ret);


		// Create a program from the kernel source
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);

		// Build the program
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);

		// Create the OpenCL kernel
		cl_kernel kernel = clCreateKernel(program, "MatMulKernel", &ret);

		size_t qweRet;
		size_t qweRetSize;
		ret = clGetKernelWorkGroupInfo(kernel, device_id, CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE, sizeof(qweRet), &qweRet, &qweRetSize);

		MatMulHostV2(A, B, C, context, kernel, command_queue);

		clReleaseMemObject(A.elements);
		clReleaseMemObject(C.elements);
		clReleaseMemObject(B.elements);
	}

	// Matrix multiplication - Host code
	// Matrix dimensions are assumed to be multiples of BLOCK_SIZE
	void MatMulHostV3(const MatrixV2 A, const MatrixV2 B, MatrixV2 C,
		const cl_context context,
		const cl_kernel matMulKernel,
		const cl_command_queue queue)
	{
		cl_int ret;
		// Invoke kernel
		cl_uint i = 0;
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.width), (void*)&A.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.height), (void*)&A.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.stride), (void*)&A.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(A.elements), (void*)&A.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.width), (void*)&B.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.height), (void*)&B.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.stride), (void*)&B.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(B.elements), (void*)&B.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.width), (void*)&C.width);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.height), (void*)&C.height);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.stride), (void*)&C.stride);
		ret = clSetKernelArg(matMulKernel, i++,
			sizeof(C.elements), (void*)&C.elements);
		ret = clSetKernelArg(matMulKernel, i++,
			BLOCK_SIZE_V3 * BLOCK_SIZE_V3 * sizeof(float), NULL);
		ret = clSetKernelArg(matMulKernel, i++,
			BLOCK_SIZE_V3 * BLOCK_SIZE_V3 * sizeof(float), NULL);
		size_t localWorkSize[] = { BLOCK_SIZE_V3, BLOCK_SIZE_V3 };
		size_t globalWorkSize[] = { A.width, A.width };
		ret = clEnqueueNDRangeKernel(queue, matMulKernel, 2, 0,
			globalWorkSize, localWorkSize,
			0, 0, 0);
		auto size = C.width*C.height*sizeof(float);
		float *Cret = (float*)malloc(size);
		ret = clEnqueueReadBuffer(queue, C.elements, CL_TRUE, 0,
			size, Cret, 0, NULL, NULL);
		for (size_t i = 0; i < size / sizeof(float); i++)
		{
			float qwe = *(Cret + i);
			if ((int)qwe != A.width)
			{
				int asd = 2;
			}
		}
		int asd = 2;
		//CL_INVALID_PROGRAM_EXECUTABLE
	}

	void MatMulV3Call()
	{
		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		//TODO: change this path
		fp = fopen("C:\\Users\\beao3002\\Desktop\\DLLOpenCLTestProject\\DLLProject\\DLLProject\\MatrixMulV3.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		// Get platform and device information
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;


		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));

		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);

		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);

		cl_uint retInfo;
		size_t sizeRet;
		// Create an OpenCL context
		cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret);

		// Create a command queue
		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);

		MatrixV2 A, B, C;
		int dim = MATRIX_DIM;
		A.width = C.width = A.stride = C.stride = dim;
		A.height = B.width = B.stride = dim;
		B.height = C.height = dim;
		size_t LIST_SIZE = dim*dim;
		float *floatArrayA = (float*)malloc(sizeof(float) * LIST_SIZE);
		float *floatArrayB = (float*)malloc(sizeof(float) * LIST_SIZE);
		for (size_t i = 0; i < dim * dim; i++)
		{
			*(floatArrayA + i) = 1.0f;
			*(floatArrayB + i) = 1.0f;
		}

		// Create memory buffers on the device for each vector
		A.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			A.width * A.height * sizeof(float), NULL, &ret);

		B.elements = clCreateBuffer(context, CL_MEM_READ_ONLY,
			B.width * B.height * sizeof(float), NULL, &ret);

		// Copy the lists A and B to their respective memory buffers
		ret = clEnqueueWriteBuffer(command_queue, A.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayA, 0, NULL, NULL);

		ret = clEnqueueWriteBuffer(command_queue, B.elements, CL_TRUE, 0,
			LIST_SIZE * sizeof(float), floatArrayB, 0, NULL, NULL);


		C.elements = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			C.width * C.height * sizeof(float), NULL, &ret);


		// Create a program from the kernel source
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);

		// Build the program
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);

		// Create the OpenCL kernel
		cl_kernel kernel = clCreateKernel(program, "MatMulKernel", &ret);

		size_t qweRet;
		size_t qweRetSize;
		ret = clGetKernelWorkGroupInfo(kernel, device_id, CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE, sizeof(qweRet), &qweRet, &qweRetSize);

		MatMulHostV3(A, B, C, context, kernel, command_queue);

		clReleaseMemObject(A.elements);
		clReleaseMemObject(C.elements);
		clReleaseMemObject(B.elements);
	}

	//for imgs of same sizes
	int BitmapAnalyse(unsigned char* ptr0, int width, int heigth, void** ar, int arSize, int* retValues)
	{

		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		fp = fopen("C:\\Users\\beao3002\\Desktop\\DLLOpenCLTestProject\\DLLProject\\DLLProject\\BitmapDistances.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;

		for (size_t i = 0; i < width * 3; i++)
		{
			for (size_t j = 0; j < heigth; j++)
			{
				*(ptr0 + i*heigth + j) = 3;
			}
		}
		int qwe = 0;
		for (size_t k = 0; k < arSize; k++)
		{
			for (size_t i = 0; i < width * 3; i++)
			{
				for (size_t j = 0; j < heigth; j++)
				{
					qwe++;
					*(((char*)(*(((char**)ar) + k))) + i*heigth + j) = k + 2;
				}
			}
		}
		//(unsigned char)*(((char*)(*(((char**)ar) + 0))) + 0 * heigth + 0);
		//retVal = (unsigned char)*(ptr0 + 0 + 0);
		int retVal = 0;
		/*for (size_t k = 0; k < arSize; k++)
		{
			for (size_t i = 0; i < width * 3; i++)
			{
				for (size_t j = 0; j < heigth; j++)
				{
					int diff = 0;
					diff = (unsigned char)*(((char*)(*(((char**)ar) + k))) + i*heigth + j);
					diff -= (unsigned char)*(ptr0 + i*heigth + j);
					diff *= diff;
					retVal += diff;
				}
			}
		}*/
		retVal = 0;

		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));

		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);

		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);

		static cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret); // this is the only line that is time consuming

		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);

		size_t mem_img_size = width * width * 3 * sizeof(char); //3 == assumed 24bpp
		int mem_retDiffs_size = width * heigth * 3 * sizeof(int) * arSize;

		cl_mem mem_sample = clCreateBuffer(context, CL_MEM_READ_ONLY,
			mem_img_size, NULL, &ret);
		cl_mem mem_refs = clCreateBuffer(context, CL_MEM_READ_ONLY,
			mem_img_size * arSize, NULL, &ret);
		cl_mem mem_retDiffs = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			mem_retDiffs_size, NULL, &ret);

		ret = clEnqueueWriteBuffer(command_queue, mem_sample, CL_TRUE, 0,
			mem_img_size, (char*)ptr0, 0, NULL, NULL);

		char* bigAssArray = (char*)malloc(arSize * mem_img_size);

		for (size_t i = 0; i < arSize; i++)
		{
			memcpy((bigAssArray + i*mem_img_size), ((char*)(*(((char**)ar) + i))), mem_img_size);
		}
		ret = clEnqueueWriteBuffer(command_queue, mem_refs, CL_TRUE, 0,
			mem_img_size * arSize, bigAssArray, 0, NULL, NULL);
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);
		cl_kernel kernel = clCreateKernel(program, "BitmapDistances", &ret);

		size_t localPrefSize;
		size_t qweRetSize;
		ret = clGetKernelWorkGroupInfo(kernel, device_id, CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE, sizeof(localPrefSize), &localPrefSize, &qweRetSize);
		int stride = sqrt(arSize * mem_img_size / sizeof(int));
		stride /= localPrefSize;
		stride *= localPrefSize;
		stride += localPrefSize;
		ret = clSetKernelArg(kernel, 0, sizeof(mem_sample), (void *)&mem_sample);
		ret = clSetKernelArg(kernel, 1, sizeof(mem_refs), (void *)&mem_refs);
		ret = clSetKernelArg(kernel, 2, sizeof(mem_retDiffs), (void *)&mem_retDiffs);
		ret = clSetKernelArg(kernel, 3, sizeof(mem_retDiffs_size), (void *)&mem_retDiffs_size);
		ret = clSetKernelArg(kernel, 4, sizeof(stride), (void *)&stride);
		ret = clSetKernelArg(kernel, 5, sizeof(mem_img_size), (void *)&mem_img_size);

		size_t localWorkSize[] = { localPrefSize, localPrefSize };
		size_t globalWorkSize[] = { stride, stride };

		ret = clEnqueueNDRangeKernel(command_queue, kernel, 2, 0,
			globalWorkSize, localWorkSize, 0, 0, 0);
		int *diff = (int*)malloc(mem_retDiffs_size);
		ret = clEnqueueReadBuffer(command_queue, mem_retDiffs, CL_TRUE, 0,
			mem_retDiffs_size, diff, 0, NULL, NULL);

		retVal = 0;
		for (size_t i = 0; i < arSize; i++)
		{
			*(retValues + i) = 0;
		}
		int ind = 0;
		for (size_t i = 0; i < mem_retDiffs_size / sizeof(int); i++)
		{
			*(retValues + (i / mem_img_size)) += *(diff + i);
			retVal += *(diff + i);
		}

		// Clean up
		ret = clFlush(command_queue);
		ret = clFinish(command_queue);
		ret = clReleaseKernel(kernel);
		ret = clReleaseProgram(program);
		ret = clReleaseMemObject(mem_sample);
		ret = clReleaseMemObject(mem_refs);
		ret = clReleaseMemObject(mem_retDiffs);
		ret = clReleaseCommandQueue(command_queue);
		//ret = clReleaseContext(context); now static
		free(diff);
		free(platforms);
		free(bigAssArray);

		free(source_str);

		return retVal;
	}

	int BitmapAnalyseV2(void** samples, int samplesSize, void** references, int referencesSize, int imgWidth, int imgHeigth, int** retValues)
	{

		// Load the kernel source code into the array source_str
		FILE *fp;
		char *source_str;
		size_t source_size;

		fp = fopen("..\\DLLProject\\BitmapDistancesV2.cl", "r");
		if (!fp) {
			fprintf(stderr, "Failed to load kernel.\n");
			exit(1);
		}
		source_str = (char*)malloc(MAX_SOURCE_SIZE);
		source_size = fread(source_str, 1, MAX_SOURCE_SIZE, fp);
		fclose(fp);
		cl_device_id device_id = NULL;
		cl_uint ret_num_devices;
		cl_uint ret_num_platforms;
		cl_int ret = clGetPlatformIDs(0, NULL, &ret_num_platforms);
		cl_platform_id *platforms = NULL;
		platforms = (cl_platform_id*)malloc(ret_num_platforms*sizeof(cl_platform_id));
		ret = clGetPlatformIDs(ret_num_platforms, platforms, NULL);
		ret = clGetDeviceIDs(platforms[0], CL_DEVICE_TYPE_ALL, 1,
			&device_id, &ret_num_devices);
		static cl_context context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret); // this is the only line that is time consuming
		cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);
		cl_program program = clCreateProgramWithSource(context, 1,
			(const char **)&source_str, (const size_t *)&source_size, &ret);
		ret = clBuildProgram(program, 1, &device_id, NULL, NULL, NULL);
		cl_kernel kernel = clCreateKernel(program, "BitmapDistancesV2", &ret);

		size_t mem_img_size = imgWidth * imgWidth * 3 * sizeof(char); //3 == assumed 24bpp
		size_t mem_retDiffs_size = imgWidth * imgHeigth * 3 * sizeof(int) * referencesSize * samplesSize;

		cl_mem mem_sample = clCreateBuffer(context, CL_MEM_READ_ONLY,
			mem_img_size * samplesSize, NULL, &ret);
		cl_mem mem_refs = clCreateBuffer(context, CL_MEM_READ_ONLY,
			mem_img_size * referencesSize, NULL, &ret);
		cl_mem mem_retDiffs = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
			mem_retDiffs_size, NULL, &ret);

		char* referencesData = (char*)malloc(referencesSize * mem_img_size);
		for (size_t i = 0; i < referencesSize; i++)
		{
			memcpy((referencesData + i*mem_img_size), ((char*)(*(((char**)references) + i))), mem_img_size);
		}
		ret = clEnqueueWriteBuffer(command_queue, mem_refs, CL_TRUE, 0,
			mem_img_size * referencesSize, referencesData, 0, NULL, NULL);

		char* samplesData = (char*)malloc(samplesSize * mem_img_size);
		for (size_t i = 0; i < samplesSize; i++)
		{
			memcpy((samplesData + i*mem_img_size), ((char*)(*(((char**)samples) + i))), mem_img_size);
		}
		ret = clEnqueueWriteBuffer(command_queue, mem_sample, CL_TRUE, 0,
			mem_img_size * samplesSize, samplesData, 0, NULL, NULL);
		size_t localPrefSize;
		ret = clGetKernelWorkGroupInfo(kernel, device_id, CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE, sizeof(localPrefSize), &localPrefSize, 0);

		int stride = sqrt(referencesSize * mem_img_size / sizeof(int));
		stride /= localPrefSize;
		stride *= localPrefSize;
		stride += localPrefSize;
		int argInd = 0;
		ret = clSetKernelArg(kernel, argInd++, sizeof(mem_sample), (void *)&mem_sample);
		ret = clSetKernelArg(kernel, argInd++, sizeof(mem_refs), (void *)&mem_refs);
		ret = clSetKernelArg(kernel, argInd++, sizeof(mem_retDiffs), (void *)&mem_retDiffs);
		ret = clSetKernelArg(kernel, argInd++, sizeof(mem_retDiffs_size), (void *)&mem_retDiffs_size);
		ret = clSetKernelArg(kernel, argInd++, sizeof(samplesSize), (void *)&samplesSize);
		ret = clSetKernelArg(kernel, argInd++, sizeof(stride), (void *)&stride);
		ret = clSetKernelArg(kernel, argInd++, sizeof(mem_img_size), (void *)&mem_img_size);

		size_t localWorkSize[] = { localPrefSize, localPrefSize };
		size_t globalWorkSize[] = { stride, stride };
		ret = clEnqueueNDRangeKernel(command_queue, kernel, 2, 0,
			globalWorkSize, localWorkSize, 0, 0, 0);

		int *diff = (int*)malloc(mem_retDiffs_size);
		ret = clEnqueueReadBuffer(command_queue, mem_retDiffs, CL_TRUE, 0,
			mem_retDiffs_size, diff, 0, NULL, NULL);
		for (size_t k = 0; k < samplesSize; k++)
		{
			for (size_t i = 0; i < referencesSize; i++)
			{
				*(((int*)(*(((int**)retValues) + k))) + i) = 0;
			}
		}
		for (size_t i = 0; i < mem_retDiffs_size / sizeof(int); i++)
		{
			//set the returning values 
			*(((int*)(*(((int**)retValues) + i / (mem_retDiffs_size / samplesSize / sizeof(int))))) + ((i / mem_img_size) % referencesSize)) += *(diff + i);
			int asd = 3;
		}

		// Clean up
		ret = clFlush(command_queue);
		ret = clFinish(command_queue);
		ret = clReleaseKernel(kernel);
		ret = clReleaseProgram(program);
		ret = clReleaseMemObject(mem_sample);
		ret = clReleaseMemObject(mem_refs);
		ret = clReleaseMemObject(mem_retDiffs);
		ret = clReleaseCommandQueue(command_queue);
		//ret = clReleaseContext(context); now static
		free(diff);
		free(platforms);
		free(referencesData);
		free(samplesData);
		free(source_str);

		return ret;
	}

}