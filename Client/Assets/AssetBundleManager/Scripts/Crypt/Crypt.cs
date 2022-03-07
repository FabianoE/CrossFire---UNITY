using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

	public class Crypt
	{
		// Token: 0x0600A38B RID: 41867 RVA: 0x002FBB44 File Offset: 0x002F9D44
		public static string Encrypt(string plainText)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(Crypt.PasswordHash, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(Crypt.VIKey));
			byte[] inArray;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					inArray = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x0600A38C RID: 41868 RVA: 0x002FBC34 File Offset: 0x002F9E34
		public static string Decrypt(string encryptedText)
		{
			byte[] array = Convert.FromBase64String(encryptedText);
			byte[] bytes = new Rfc2898DeriveBytes(Crypt.PasswordHash, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.None
			};
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(Crypt.VIKey));
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[array.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array2, 0, count).TrimEnd("\0".ToCharArray());
		}

		// Token: 0x0600A38D RID: 41869 RVA: 0x002FBCF4 File Offset: 0x002F9EF4
		public static byte[] Encrypt(byte[] plainBuffer)
		{
			if (plainBuffer == null || plainBuffer.Length <= 0)
			{
				return null;
			}
			byte[] bytes = new Rfc2898DeriveBytes(Crypt.PasswordHash, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, Encoding.ASCII.GetBytes(Crypt.VIKey));
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainBuffer, 0, plainBuffer.Length);
					cryptoStream.FlushFinalBlock();
					result = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return result;
		}

		// Token: 0x0600A38E RID: 41870 RVA: 0x002FBDE4 File Offset: 0x002F9FE4
		public static byte[] Decrypt(byte[] encryptedBuffer)
		{
			byte[] bytes = new Rfc2898DeriveBytes(Crypt.PasswordHash, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.None
			};
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(Crypt.VIKey));
			MemoryStream memoryStream = new MemoryStream(encryptedBuffer);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array = new byte[encryptedBuffer.Length];
			int num = cryptoStream.Read(array, 0, array.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return array;
		}

		// Token: 0x0600A38F RID: 41871 RVA: 0x002FBE7C File Offset: 0x002FA07C
		public static byte[] Encrypt(string key, byte[] plainBuffer)
		{
			if (plainBuffer == null || plainBuffer.Length <= 0)
			{
				return null;
			}
			byte[] bytes = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, Encoding.ASCII.GetBytes(Crypt.VIKey));
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainBuffer, 0, plainBuffer.Length);
					cryptoStream.FlushFinalBlock();
					result = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return result;
		}

		// Token: 0x0600A390 RID: 41872 RVA: 0x002FBF68 File Offset: 0x002FA168
		public static byte[] Decrypt(string key, byte[] encryptedBuffer)
		{
			byte[] bytes = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(Crypt.SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.None
			};
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(Crypt.VIKey));
			MemoryStream memoryStream = new MemoryStream(encryptedBuffer);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array = new byte[encryptedBuffer.Length];
			int num = cryptoStream.Read(array, 0, array.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return array;
		}

		public static string MD5Calc(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}

			return sb.ToString();
		}
	// Token: 0x04007586 RID: 30086
	private static readonly string PasswordHash = "P@@Sw0rd";

		// Token: 0x04007587 RID: 30087
		private static readonly string SaltKey = "S@LT&KEY";

		// Token: 0x04007588 RID: 30088
		private static readonly string VIKey = "@1B2c3D4e5F6g7H8";
	}
