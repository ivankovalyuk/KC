using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;


namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.InputEncoding = System.Text.Encoding.GetEncoding("Cyrillic");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            FileInfo f1 = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part1\Shevchenko.txt");
            FileInfo f2 = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part1\Podrevlanskiy.txt");
            FileInfo f3 = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part1\PCI.txt");
            DepthFileAnalyser Repka = new DepthFileAnalyser(f2.FullName);
            DepthFileAnalyser Virsh = new DepthFileAnalyser(f1.FullName);
            DepthFileAnalyser Pci = new DepthFileAnalyser(f3.FullName);
            FileInfo ShevaCompressedToGz = CompressGz(f1);
            FileInfo ShevaCompressedToZip = CompressZip(f1);
            FileInfo ShevaCompressedToRar = CompressRar(f1);
            FileInfo ShevaCompressedToBZip2 = CompressBZip2(f1);
            FileInfo ShevaCompressedToTar = CompressTar(f1);
            FileInfo PodrevCompressedToGz = CompressGz(f2);
            FileInfo PodrevCompressedToZip = CompressZip(f2);
            FileInfo PodrevCompressedToRar = CompressRar(f2);
            FileInfo PodrevCompressedToBZip2 = CompressBZip2(f2);
            FileInfo PodrevCompressedToTar = CompressTar(f2);
            FileInfo PCICompressedToGz = CompressGz(f3);
            FileInfo PCICompressedToZip = CompressZip(f3);
            FileInfo PCICompressedToRar = CompressRar(f3);
            FileInfo PCICompressedToBZip2 = CompressBZip2(f3);
            FileInfo PCICompressedToTar = CompressTar(f3);
            Console.WriteLine("Мені 13-й минало: " + Virsh.ToString() + "\nАрхів Gzip: " + ShevaCompressedToGz.Length + "\nZip:  "
                + ShevaCompressedToZip.Length + "\nRar:  " + ShevaCompressedToRar.Length + "\nBzip2:  "
                + ShevaCompressedToBZip2.Length + "\nTar:   " + ShevaCompressedToTar.Length + "\nРозмір .txt файлу:  " + f1.Length+'\n');
            Console.WriteLine("Казка про рєпку: " + Repka.ToString() + "\nАрхів Gzip: " + PodrevCompressedToGz.Length + "\nZip:  "
                + PodrevCompressedToZip.Length + "\nRar:  " + PodrevCompressedToRar.Length + "\nBzip2:  "
                + PodrevCompressedToBZip2.Length + "\nTar:   " + PodrevCompressedToTar.Length + "\nРозмір .txt файлу:  " + f2.Length+'\n');
            Console.WriteLine("Специфікація PCI: " + Pci.ToString() + "\nАрхів Gzip: " + PCICompressedToGz.Length + "\nZip:  "
                + PCICompressedToZip.Length + "\nRar:  " + PCICompressedToRar.Length + "\nBzip2:  "
                + PCICompressedToBZip2.Length + "\nTar:   " + PCICompressedToTar.Length + "\nРозмір .txt файлу:  " + f3.Length);
            
            Console.ReadLine();
        }
        public static FileInfo CompressGz(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".gz"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            return new FileInfo(outFile.Name);
                        }
                    }
                }
            }

            return null;

        }
        public static FileInfo CompressZip(FileInfo fi)
        {
            if (!File.Exists(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".zip"))
            {
                using (ZipArchive newFile = ZipFile.Open(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".zip", ZipArchiveMode.Create))
                {
                    newFile.CreateEntryFromFile(fi.FullName, fi.Name, CompressionLevel.Optimal);

                }
            }
            return new FileInfo(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".zip");
        }
        public static FileInfo CompressRar(FileInfo fi)

        {
            string intputPath = fi.FullName;
            string outputPath = fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".rar";
            string outputFileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".rar";
            string rarCmd;
            ProcessStartInfo processStartInfo;
            Process process;
            rarCmd = " a " + outputPath + " " + intputPath + " -r -ep1";
            string rarFile = @"C:\Program Files\WinRAR\WinRAR.exe";
            if (outputPath.IndexOf(' ') > 0 || intputPath.IndexOf(' ') > 0)
            {
                rarCmd = " a " + outputPath + " \"" + intputPath + "\" -r -ep1";
            }

            if (!File.Exists(outputPath))
            {
                try
                {
                    if (!Directory.Exists(intputPath.Substring(0, intputPath.Length - fi.Name.Length)))
                    {

                        throw new ArgumentException("CompressRar'arge : inputPath isn't exsit.");

                    }
                    processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = rarFile;
                    processStartInfo.Arguments = rarCmd;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.WorkingDirectory = outputPath;
                    process = new Process();
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return new FileInfo(outputPath);
        }
        public static FileInfo CompressBZip2(FileInfo fi)
        {
            FileInfo zipFileName = new FileInfo(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".bz2");
            if (!File.Exists(zipFileName.FullName))
            {
                using (FileStream fileToBeZippedAsStream = fi.OpenRead())
                {

                    using (FileStream zipTargetAsStream = zipFileName.Create())
                    {
                        try
                        {
                            BZip2.Compress(fileToBeZippedAsStream, zipTargetAsStream, true, 4096);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return zipFileName;
        }
        public static FileInfo CompressTar(FileInfo fi)
        {

            FileInfo zipFileName = new FileInfo(fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length) + ".tar.gz");
            if (!File.Exists(zipFileName.FullName))
            {
                using (Stream targetStream = new GZipOutputStream(File.Create(zipFileName.FullName)))
                {
                    using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(targetStream))
                    {
                        
                            TarEntry entry = TarEntry.CreateEntryFromFile(fi.FullName);
                            tarArchive.WriteEntry(entry,false);
                        
                    }
                }
            }
            return zipFileName;
        }
      

        }
    }
   

