using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class stringExtenstion
{
	public static bool Contains(this string str, string cmp, bool ignoreCase = false)
	{
		string main = ignoreCase ? str.ToLower() : str;

		return main.Contains(ignoreCase ? cmp.ToLower() : cmp);
	}
}

public static class HelperFunctions
{
	
}
