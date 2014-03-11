using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Commons.Extensions.Assembly {
	/// <summary>
	/// バイト配列を処理する上でよく使う関数
	/// </summary>
	public　static class BytesControlExtensions {
		/// <summary>
		/// バイト配列を1024Byte刻みで分割
		/// </summary>
		/// <param name="datas"></param>
		/// <returns></returns>
		static IEnumerable<byte[]> cutByates(byte[] datas) {
			var data = new byte[1024];
			var count = 0;
			for(var x = 0; x < datas.Count(); x++) {
				data[count] = datas[x];
				if(count == 1023) {
					yield return data;
					data = new byte[1024];
					count = 0;
				} else
					count++;
			}
			yield return data;
		}

		/// <summary>
		/// バイト配列を1024Byte刻みで分割(拡張メソッド)
		/// </summary>
		/// <param name="datas">byte[]</param>
		/// <returns>cutByatesにパイプするだけ</returns>
		public static IEnumerable<byte[]> Cut1024Bytes(this byte[] datas) {
			return cutByates(datas);
		}

		/// <summary>
		/// Uintをbyte配列に変換
		/// </summary>
		/// <param name="i">Uint</param>
		/// <returns>変換されたbyte[]</returns>
		public static byte[] ConvertBytes(this UInt32 i) {
			return BitConverter.GetBytes(i).Reverse().ToArray();
		}
        public static byte ConvertByte(this UInt32 i)
        {
            return BitConverter.GetBytes(i).Reverse().ToArray().Last();
        }
        public static byte[] ConvertBytes(this UInt16 i)
        {
            UInt32 a = (UInt32)i;
            return a.ConvertBytes().Skip(2).ToArray();
        }
        public static byte ConvertByte(this UInt16 i)
        {
            UInt32 a = (UInt32)i;
            return a.ConvertByte();
        }
        public static byte[] ConvertBytes(this int i)
        {
            UInt32 a = (UInt32) i;
            return a.ConvertBytes();
        }
        public static byte ConvertByte(this int i)
        {
            UInt32 a = (UInt32)i;
            return a.ConvertByte();
        }

		/// <summary>
		/// Uint変換
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
        public static UInt32 ConvertUint(this byte[] b)
        {
            if (b.Length > 4) throw new ArgumentException();
            UInt32 ret = 0;
            for (var cout = 0; cout < b.Length; cout++)
            {
                ret += b[cout] * (ret + 1) * 0x100;
            }
            return ret;
        }

		/// <summary>
		/// byte[]から特定の範囲を取得する
		/// </summary>
		/// <param name="b"></param>
		/// <param name="offset">オフセット</param>
		/// <param name="range">レンジ</param>
		/// <returns>byte[]</returns>
		public static byte[] GetRange(this byte[] b, int offset, int range) {
			if(offset <= 0 || range <= 0)
				throw new ArgumentException("argument is under 0");
			if(b.Length > offset)
				throw new ArgumentException("offset is too large");
			return b.Skip(offset - 1).Take(range).ToArray();
		}

		/// <summary>
		/// 0か1が8文字の文字列をbyteに変換
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte ConvertByte(this string s) {
			if(Regex.IsMatch(s, @"^[01]{8}$")) {
				return Convert.ToByte(s, 2);
			} else
				throw new ArgumentException(@"有効な文字列は""^[01]{8}$""を満たすものだけです");
		}

		/// <summary>
		/// byte[]同士の比較を行う
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>等しければ0</returns>
		public static int Compare(this byte[] a, byte[] b) {
			return string.Compare(a.ToString(), b.ToString());
		}
		public static bool IsSame(this byte[] a, byte[] b) {
			return string.Compare(a.ToString(), b.ToString()) == 0;
		}
        public static bool IsSame(this byte[] a, UInt32 b)
        {
            var bb = b.ConvertBytes();
            return string.Compare(a.ToString(), bb.ToString()) == 0;
        }
	}


	public partial class Sumple {

		public void run() {
			var header = new HeaderData() {
				Status = HeaderStatus.Ste
			};
			UInt32 status = 891;

			var sendData = sendMessage(header, status);
            byte data1 = 0x1;
			var a = new byte[3] { data1, 0x22, 0x33 };
			var b = new byte[3] { 0x11, 0x22, 0x33 };
			if(a.IsSame(b)) {
			}

            if (a.IsSame(0x112233))
            {
            }
		}

        /// <summary>
        /// 手作りパケット
        /// </summary>
        class HundMadePacket
        {
            /// <summary>
            /// Enumのヘッダ
            /// </summary>
            public Status Head1 { get; set; }

            /// <summary>
            /// shortのヘッダ
            /// </summary>
            public UInt16 Head2
            {
                get;
                set;
            }
            /// <summary>
            /// 可変長データ部
            /// </summary>
            public Dataa Data { get; set; }

            /// <summary>
            /// パケット
            /// </summary>
            public byte[] Packet
            {
                get
                {
                    var p = new List<byte>();
                    if (Head1 == Status.Stereo)
                    {
                        p.Add((0x00).ConvertByte());
                    }
                    else if (Head1 == Status.Mono)
                    {
                        p.Add((0x01).ConvertByte());
                    }

                    p.AddRange(Head2.ConvertBytes());
                    p.AddRange(Data.Data);
                    return p.ToArray();
                }
                set
                {
                    Head1 = value[0] == 0x1 ? Status.Mono : Status.Stereo;
                    Head2 = (UInt16)(value[1] << 8 + value[2]);
                    Data = new Dataa() { Data = new List<byte>(value.Skip(3)) };
                }
            }

            /// <summary>
            /// 可変長データ
            /// </summary>
            public class Dataa
            {
                public List<byte> Data { get; set; }
            }

            /// <summary>
            /// ステータス地
            /// </summary>
            public enum Status
            {
                Stereo,
                Mono
            }
        }

		/// <summary>
		/// メッセージ送信
		/// </summary>
		/// <param name="header"></param>
        public byte[][] sendMessage(HeaderData header, UInt32 status)
        {

            //送信データ作成
            var sendData = new HundMadePacket[10000];
            var ret = new List<byte[]>();
            var sendBytes = new byte[sendData.Sum(x => x.Packet.Length)];
            
            var i = 0;
            sendData.TakeWhile(x =>
            {
                var j = 0;
                x.Packet.TakeWhile(y =>
                {
                    sendBytes[i * 8 + j] = y;
                    j++;
                    return true;
                });
                i++;
                return true;
            });
            //1024byte刻みでデータ送信
            foreach (var data in sendBytes.Cut1024Bytes())
            {
                //ヘッダ作成
                var h = new byte[3] { 0x11, 0x00, 0x00 };
                //ヘッダーをいただく
                h[1] = header.HeaderByte;
                //ステータス値(uint)の後ろ1Byte分をいただく
                h[2] = status.ConvertBytes().Last();
                //送信
                var send = h.Concat(data).ToArray();
                ret.Add(send);
            }
            return ret.ToArray();

        }

		/// <summary>
		/// ユーザーたちがやりとりするステータス
		/// </summary>
		public enum HeaderStatus { Mono, Ste }

		public class HeaderData {
			public HeaderStatus Status { get; set; }
			public byte HeaderByte {
				get {
					switch(Status) {
						case HeaderStatus.Mono:
							return "00000001".ConvertByte();
						case HeaderStatus.Ste:
							return "00000010".ConvertByte();
						default:
							return "00000001".ConvertByte();
					}
				}
			}
		}

	}
}