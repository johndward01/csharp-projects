﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace CodingWarsSolutions.KYU._5
{
    public class PhoneDir
    {
        public static string Phone(string str, string num)
        {
            var entries = str.Split('\n')
                .Select(x => new {
                    name = Regex.Match(x, @"<(.*)>").Groups[1].Value,
                    phone = Regex.Match(x, @"\+([0-9]{1,2}\-[0-9]{3}\-[0-9]{3}\-[0-9]{4})").Groups[1].Value,
                    raw = x
            });

            var matching = entries.Where(i => i.phone == num);
            if (matching.Count() > 1)
                return "Error => Too many people: " + num;
            else if (matching.Count() == 0)
                return "Error => Not found: " + num;
            
            var first = matching.First();
            var address = first.raw.Replace($"<{first.name}>", "").Replace($"+{first.phone}", "").Replace('_', ' ')
                .Split(' ')
                .Where(x => x.Length > 1)
                .Select(x => Regex.Replace(x.Trim(), @"[^a-zA-Z0-9\-\.]", "").Replace("  ", " "));

            return $"Phone => {first.phone}, Name => {first.name}, Address => {string.Join(" ", address)}";
        }
    }

    [TestFixture]
    [Ignore("not current test")]
    public static class PhoneDirTests
    {

        static string dr = "/+1-541-754-3010 156 Alphand_St. <J Steeve>\n 133, Green, Rd. <E Kustur> NY-56423 ;+1-541-914-3010\n"
        + "+1-541-984-3012 <P Reed> /PO Box 530; Pollocksville, NC-28573\n :+1-321-512-2222 <Paul Dive> Sequoia Alley PQ-67209\n"
        + "+1-741-984-3090 <Peter Reedgrave> _Chicago\n :+1-921-333-2222 <Anna Stevens> Haramburu_Street AA-67209\n"
        + "+1-111-544-8973 <Peter Pan> LA\n +1-921-512-2222 <Wilfrid Stevens> Wild Street AA-67209\n"
        + "<Peter Gone> LA ?+1-121-544-8974 \n <R Steell> Quora Street AB-47209 +1-481-512-2222\n"
        + "<Arthur Clarke> San Antonio $+1-121-504-8974 TT-45120\n <Ray Chandler> Teliman Pk. !+1-681-512-2222! AB-47209,\n"
        + "<Sophia Loren> +1-421-674-8974 Bern TP-46017\n <Peter O'Brien> High Street +1-908-512-2222; CC-47209\n"
        + "<Anastasia> +48-421-674-8974 Via Quirinal Roma\n <P Salinger> Main Street, +1-098-512-2222, Denver\n"
        + "<C Powel> *+19-421-674-8974 Chateau des Fosses Strasbourg F-68000\n <Bernard Deltheil> +1-498-512-2222; Mount Av.  Eldorado\n"
        + "+1-099-500-8000 <Peter Crush> Labrador Bd.\n +1-931-512-4855 <William Saurin> Bison Street CQ-23071\n"
        + "<P Salinge> Main Street, +1-098-512-2222, Denve\n" + "<P Salinge> Main Street, +1-098-512-2222, Denve\n";

        private static void testing(string actual, string expected)
        {
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public static void test1()
        {
            testing(PhoneDir.Phone(dr, "48-421-674-8974"), "Phone => 48-421-674-8974, Name => Anastasia, Address => Via Quirinal Roma");
        }

        [Test]
        public static void test2()
        {
            testing(PhoneDir.Phone(dr, "1-921-512-2222"), "Phone => 1-921-512-2222, Name => Wilfrid Stevens, Address => Wild Street AA-67209");
        }

        [Test]
        public static void test3()
        {
            testing(PhoneDir.Phone(dr, "1-908-512-2222"), "Phone => 1-908-512-2222, Name => Peter O'Brien, Address => High Street CC-47209");
        }

        [Test]
        public static void test4()
        {
            testing(PhoneDir.Phone(dr, "1-541-754-3010"), "Phone => 1-541-754-3010, Name => J Steeve, Address => 156 Alphand St.");
        }

        [Test]
        public static void test5()
        {
            testing(PhoneDir.Phone(dr, "1-121-504-8974"), "Phone => 1-121-504-8974, Name => Arthur Clarke, Address => San Antonio TT-45120");
        }

        [Test]
        public static void test6()
        {
            testing(PhoneDir.Phone(dr, "1-498-512-2222"), "Phone => 1-498-512-2222, Name => Bernard Deltheil, Address => Mount Av. Eldorado");
        }

        [Test]
        public static void test7()
        {
            testing(PhoneDir.Phone(dr, "1-098-512-2222"), "Error => Too many people: 1-098-512-2222");
        }

        [Test]
        public static void test8()
        {
            testing(PhoneDir.Phone(dr, "5-555-555-5555"), "Error => Not found: 5-555-555-5555");
        }
    }
}
