using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle
{
    public static class OpenCLImageAnalyseDLL
    {
        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "OCL")]
        public static extern void OCL();

        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "MatMulV1Call", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMulV1Call();

        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "MatMulV2Call", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMulV2Call();

        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "MatMulV3Call", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMulV3Call();

        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "BitmapAnalyse", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 BitmapAnalyse(byte* ptr0, int width, int heigth,
            void* refPtrAr, int arraySize, int* retValues);

        [DllImport("../../../Debug/OpenCLDLL.dll", EntryPoint = "BitmapAnalyseV2", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 BitmapAnalyseV2(void** samples, int samplesSize, void** references,
            int referencesSize, int imgWidth, int imgHeigth, int** retValues);
    }

}
