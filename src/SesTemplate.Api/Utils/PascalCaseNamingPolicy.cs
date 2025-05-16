﻿using System.Text.Json;

namespace SesTemplate.Api.Utils;

public class PascalCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;
        return char.ToUpper(name[0]) + name.Substring(1);
    }
}