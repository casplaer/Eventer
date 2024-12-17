using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Domain.Utilities
{
    public static class EmailNormalizer
    {
        public static string Normalize(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Поле Email не может быть пустым.");
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            var atIndex = normalizedEmail.IndexOf('@');
            if (atIndex > 0)
            {
                var localPart = normalizedEmail.Substring(0, atIndex);
                var domainPart = normalizedEmail.Substring(atIndex + 1);

                if (localPart.Contains('+'))
                {
                    localPart = localPart.Substring(0, localPart.IndexOf('+'));
                }

                localPart = localPart.Replace(".", "");

                normalizedEmail = $"{localPart}@{domainPart}";
            }

            return normalizedEmail;
        }
    }

}
