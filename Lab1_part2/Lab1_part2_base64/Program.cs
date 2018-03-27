using System;
using System.IO;
using System.Text;
using Lab1;
using ICSharpCode.SharpZipLib.BZip2;

namespace Lab1_part2_base64
{

    class Program
    {

       static string _alphabet = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.GetEncoding("Cyrillic");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (File.Exists(@"C:\Users\Ivan\Desktop\Lab1_part1\Shevchenko.txt") && File.Exists(@"C:\Users\Ivan\Desktop\Lab1_part1\Podrevlanskiy.txt") && File.Exists(@"C:\Users\Ivan\Desktop\Lab1_part1\PCI.txt"))
            {
                Base64Encode(@"C:\Users\Ivan\Desktop\Lab1_part1\Shevchenko.txt", @"C:\Users\Ivan\Desktop\Lab1_part2_base64\ShevaBase64.txt");
                Base64Encode(@"C:\Users\Ivan\Desktop\Lab1_part1\Podrevlanskiy.txt", @"C:\Users\Ivan\Desktop\Lab1_part2_base64\PodrevlanBase64.txt");
                Base64Encode(@"C:\Users\Ivan\Desktop\Lab1_part1\PCI.txt", @"C:\Users\Ivan\Desktop\Lab1_part2_base64\PCIBase64.txt");
            }
            Console.WriteLine(File.ReadAllText(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PCIBase64.txt") == File.ReadAllText(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PCIBase64Online.txt"));
            FileInfo f1Base64Compress = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\ShevaBase64.txt");
            FileInfo f2Base64Compress = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PodrevlanBase64.txt");
            FileInfo f3Base64Compress = new FileInfo(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PCIBase64.txt");
            f1Base64Compress = CompressBZip2(f1Base64Compress);
            f2Base64Compress = CompressBZip2(f2Base64Compress);
            f3Base64Compress = CompressBZip2(f3Base64Compress);
            DepthFileAnalyser f1Base64 = new DepthFileAnalyser(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\ShevaBase64.txt");
            DepthFileAnalyser f2Base64 = new DepthFileAnalyser(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PodrevlanBase64.txt");
            DepthFileAnalyser f3Base64 = new DepthFileAnalyser(@"C:\Users\Ivan\Desktop\Lab1_part2_base64\PCIBase64.txt");

            DepthFileAnalyser f1Base64Bz2 = new DepthFileAnalyser(f1Base64Compress.FullName);
            DepthFileAnalyser f2Base64Bz2 = new DepthFileAnalyser(f2Base64Compress.FullName);
            DepthFileAnalyser f3Base64Bz2 = new DepthFileAnalyser(f3Base64Compress.FullName);
            Console.WriteLine("Кількість інформації в закодованому варіанті || закодованому і стисненму\nШевченко:  "+
                f1Base64.InformationCount.ToString()+" || "+f1Base64Bz2.InformationCount.ToString()+"\nПодрев'янський: "+
                f2Base64.InformationCount.ToString()+ " || " + f2Base64Bz2.InformationCount.ToString()+"\nPCI: "+
                f3Base64.InformationCount.ToString() + " || " + f3Base64Bz2.InformationCount.ToString());
            Console.ReadLine();
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
        public static void Base64Encode(string pathFrom, string pathTo)
        {
            int val;
            using (BinaryReader br = new BinaryReader(File.Open(pathFrom, FileMode.Open), Encoding.UTF8))
            {
                using (StreamWriter sr = new StreamWriter(File.OpenWrite(pathTo)))
                {
                    int mod = (int)(br.BaseStream.Length % 3);
                    for (int i = 0; i < br.BaseStream.Length - mod; i += 3)
                    {
                        val = (((br.ReadByte() << 8) + br.ReadByte() << 8) + br.ReadByte());
                        sr.Write(String.Format($"{ _alphabet[(val >> 18) & 0x3F]}{_alphabet[(val >> 12) & 0x3F]}{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}"));
                    }
                    if (mod == 2)
                    {
                        val = ((br.ReadByte() << 8) + br.ReadByte() << 2);
                        sr.Write(String.Format($"{_alphabet[(val >> 12) & 0x3F]}{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}="));
                    }
                    if (mod == 1)
                    {
                        val = br.ReadByte() << 4;
                        sr.Write(String.Format($"{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}=="));
                    }
                }
            }
        }
    }
}

