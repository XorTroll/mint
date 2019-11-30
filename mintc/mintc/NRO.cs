using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using LibHac.FsSystem;
using LibHac.FsSystem.RomFs;

namespace mintc
{
    public static class NRO
    {
        public struct NROStruct
        {
            public string Name { get; set; }

            public string Author { get; set; }

            public string Version { get; set; }

            public Bitmap Icon { get; set; }

            public void ApplySave(string InNRO, string RomFsPath, string OutNRO)
            {
                var nro = File.ReadAllBytes(InNRO);
                uint nrosize = BitConverter.ToUInt32(nro, 0x18);
                if ((char)nro[nrosize] == 'A')
                {
                    if ((char)nro[nrosize + 1] == 'S')
                    {
                        if ((char)nro[nrosize + 2] == 'E')
                        {
                            if ((char)nro[nrosize + 3] == 'T')
                            {
                                byte[] icon;
                                if(Icon == null)
                                {
                                    ulong oldioffset = BitConverter.ToUInt64(nro, (int)nrosize + 8);
                                    ulong oldisize = BitConverter.ToUInt64(nro, (int)nrosize + 16);
                                    icon = new byte[oldisize];
                                    Array.Copy(nro, (int)oldioffset + nrosize, icon, 0, (int)oldisize);
                                }
                                else icon = Icon.ToByteArray(ImageFormat.Jpeg);

                                byte[] nacp;
                                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Version))
                                {
                                    ulong oldnoffset = BitConverter.ToUInt64(nro, (int)nrosize + 24);
                                    ulong oldnsize = BitConverter.ToUInt64(nro, (int)nrosize + 32);
                                    nacp = new byte[0x4000];
                                    Array.Copy(nro, (int)oldnoffset + nrosize, nacp, 0, (int)oldnsize);
                                }
                                else nacp = Utils.GenerateNACPFrom(Name, Author, Version);
                                
                                var romfs = new RomFsBuilder(new LocalFileSystem(RomFsPath)).Build();
                                romfs.GetSize(out long romfssz);
                                byte[] romfsbuf = new byte[romfssz];
                                romfs.Read(0, romfsbuf);
                                romfs.Dispose();
                                byte[] nrodata = new byte[nrosize];
                                Array.Copy(nro, nrodata, nrosize);
                                var newnro = new List<byte>();
                                newnro.AddRange(nrodata);
                                newnro.AddRange(Encoding.ASCII.GetBytes("ASET"));
                                uint asetversion = 0;
                                newnro.AddRange(BitConverter.GetBytes(asetversion));
                                ulong iconoffset = 56;
                                newnro.AddRange(BitConverter.GetBytes(iconoffset));
                                ulong iconsize = (ulong)icon.Length;
                                newnro.AddRange(BitConverter.GetBytes(iconsize));
                                ulong nacpoffset = iconoffset + iconsize;
                                newnro.AddRange(BitConverter.GetBytes(nacpoffset));
                                ulong nacpsize = (ulong)nacp.Length;
                                newnro.AddRange(BitConverter.GetBytes(nacpsize));
                                ulong romfsoffset = iconoffset + iconsize + nacpsize;
                                newnro.AddRange(BitConverter.GetBytes(romfsoffset));
                                newnro.AddRange(BitConverter.GetBytes(romfssz));
                                newnro.AddRange(icon);
                                newnro.AddRange(nacp);
                                newnro.AddRange(romfsbuf);
                                File.WriteAllBytes(OutNRO, newnro.ToArray());
                            }
                        }
                    }
                }
            }
        }
    }
}
