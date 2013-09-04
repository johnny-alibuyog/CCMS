﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CCMS.Core.Common
{
    public class RandomCreditCardNumberGenerator
    {
        /*This is a port of the port of of the Javascript credit card number generator now in C#
        * by Kev Hunter http://kevhunter.wordpress.com
        * See the license below. Obviously, this is not a Javascript credit card number
         generator. However, The following class is a port of a Javascript credit card
         number generator.
         @author robweber
         Javascript credit card number generator Copyright (C) 2006 Graham King
         graham@darkcoding.net
         This program is free software; you can redistribute it and/or modify it
         under the terms of the GNU General Public License as published by the
         Free Software Foundation; either version 2 of the License, or (at your
         option) any later version.
         This program is distributed in the hope that it will be useful, but
         WITHOUT ANY WARRANTY; without even the implied warranty of
         MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
         Public License for more details.
 
         You should have received a copy of the GNU General Public License along
         with this program; if not, write to the Free Software Foundation, Inc.,
         51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
         www.darkcoding.net
        */

        public static readonly string[] AMEX_PREFIX_LIST = new[] { "34", "37" };
        public static readonly string[] DINERS_PREFIX_LIST = new[] { "300", "301", "302", "303", "36", "38" };
        public static readonly string[] DISCOVER_PREFIX_LIST = new[] { "6011" };
        public static readonly string[] ENROUTE_PREFIX_LIST = new[] { "2014", "2149" };
        public static readonly string[] JCB_15_PREFIX_LIST = new[] { "2100", "1800" };
        public static readonly string[] JCB_16_PREFIX_LIST = new[] { "3088", "3096", "3112", "3158", "3337", "3528" };
        public static readonly string[] MASTERCARD_PREFIX_LIST = new[] { "51", "52", "53", "54", "55" };
        public static readonly string[] VISA_PREFIX_LIST = new[] { "4539", "4556", "4916", "4532", "4929", "40240071", "4485", "4716", "4" };
        public static readonly string[] VOYAGER_PREFIX_LIST = new[] { "8699" };

        /* 
         * 'prefix' is the start of the  CC number as a string, any number
         *  private of digits 'length' is the length of the CC number to generate.
         *  Typically 13 or  16
         */
        private static string CreateFakeCreditCardNumber(string prefix, int length)
        {
            var ccnumber = prefix;

            while (ccnumber.Length < (length - 1))
            {
                var random = (new Random().NextDouble() * 1.0f - 0f);
                ccnumber += Math.Floor(random * 10);

                //sleep so we get a different seed
                Thread.Sleep(20);
            }


            // reverse number and convert to int
            var reversedCCnumberList = ccnumber.Reverse()
                .Select(x => Convert.ToInt32(x));

            // calculate sum
            var sum = 0;
            var pos = 0;
            var reversedCCnumber = reversedCCnumberList.ToArray();

            while (pos < length - 1)
            {
                var odd = reversedCCnumber[pos] * 2;

                if (odd > 9)
                    odd -= 9;

                sum += odd;

                if (pos != (length - 2))
                    sum += reversedCCnumber[pos + 1];

                pos += 2;
            }

            // calculate check digit
            var checkdigit = Convert.ToInt32((Math.Floor((decimal)sum / 10) + 1) * 10 - sum) % 10;
            ccnumber += checkdigit;

            return ccnumber;
        }


        public static IEnumerable<string> GetCreditCardNumbers(string[] prefixList, int length, int howMany)
        {
            var result = new Stack<string>();
            for (int i = 0; i < howMany; i++)
            {
                var randomPrefix = new Random().Next(0, prefixList.Length - 1);
                if (randomPrefix > 1)
                    randomPrefix--;

                var ccnumber = prefixList[randomPrefix];
                result.Push(CreateFakeCreditCardNumber(ccnumber, length));
            }
            return result;
        }


        public static IEnumerable<string> GenerateMasterCardNumbers(int howMany)
        {
            return GetCreditCardNumbers(MASTERCARD_PREFIX_LIST, 16, howMany);
        }


        public static string GenerateMasterCardNumber()
        {
            return GetCreditCardNumbers(MASTERCARD_PREFIX_LIST, 16, 1).First();
        }

        public static bool IsValidCreditCardNumber(string creditCardNumber)
        {
            try
            {
                var reversedNumber = creditCardNumber.ToCharArray().Reverse();

                var mod10Count = 0;
                for (int i = 0; i < reversedNumber.Count(); i++)
                {
                    var augend = Convert.ToInt32(reversedNumber.ElementAt(i).ToString());
                    if (((i + 1) % 2) == 0)
                    {
                        var productstring = (augend * 2).ToString();
                        augend = 0;
                        for (int j = 0; j < productstring.Length; j++)
                            augend += Convert.ToInt32(productstring.ElementAt(j).ToString());

                    }
                    mod10Count += augend;
                }

                if ((mod10Count % 10) == 0)
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
