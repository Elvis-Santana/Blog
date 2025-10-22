using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.Extensions;

public static class FakerExtensions
{
    public static string Text(this Faker faker,  int Min)
        => faker.Lorem.Paragraph(2001)
        .Substring(0, Math.Min(Min, 10000))
        .ToString();
    
}
 