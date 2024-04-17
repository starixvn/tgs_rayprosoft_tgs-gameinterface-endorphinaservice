using System.Security.Cryptography;
using System.Text;

namespace Tgs.GameInterface.EndorphinaService.Core;

public class EndorphinaHasher
{
	private static readonly MD5 _Md5;

	static EndorphinaHasher()
	{
		_Md5 = MD5.Create();
	}

	public static string ComputeHash(string data)
	{
		var dataByte = Encoding.UTF8.GetBytes(data);

		var hash = _Md5.ComputeHash(dataByte);

		return Convert.ToHexString(hash).ToUpper();
	}
}
