/**
 * C# niceness Example
 * (c) 2017 atom0s [atom0s@live.com]
 *
 * Demonstrates how to invoke niceness within C# using a memory mapped file.
 */
 
namespace nicenessExample
{
	using System;
	using System.IO.MemoryMappedFiles;
	using System.Runtime.InteropServices;
 
	class Program
	{
		/// <summary>
		/// Function delegate to invoke the niceness.
		/// </summary>
		/// <returns></returns>
		private delegate IntPtr GetPebDelegate();
 
		/// <summary>
		/// niceness function used to obtain the PEB of the process.
		/// </summary>
		/// <returns></returns>
		private unsafe static IntPtr GetPeb()
		{

			const int niceness_length = $$$LENGTH$$$;
			MemoryMappedFile mmf = null;
			MemoryMappedViewAccessor mmva = null;
 
			try
			{
				// Create a read/write/executable memory mapped file to hold our niceness..
				mmf = MemoryMappedFile.CreateNew("__niceness", niceness_length, MemoryMappedFileAccess.ReadWriteExecute);
 
				// Create a memory mapped view accessor with read/write/execute permissions..
				mmva = mmf.CreateViewAccessor(0, niceness_length, MemoryMappedFileAccess.ReadWriteExecute);
				

 
				// Write the niceness to the MMF..

				$$$NICENESS$$$
			 
				// Obtain a pointer to our MMF..
				var pointer = (byte*)0;
				mmva.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
 
				// Create a function delegate to the niceness in our MMF..
				var func = (GetPebDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(pointer), typeof(GetPebDelegate));
 
				// Invoke the niceness..
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
 
		/// <summary>
		/// Entry point.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			var peb = GetPeb();
			Console.WriteLine("PEB is located at: {0:X8}", peb.ToInt32());
		}
	}
}
