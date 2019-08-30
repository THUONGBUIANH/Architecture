using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Architecture_BE.Models.Validations
{
    public static class ValidationDto
    {
        public static object GetMessagesByErrors(IList<ValidationFailure> Errors)
        {

            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;
            var keys = Errors.GroupBy(x => x.PropertyName).Select(x => x.Key);

            foreach (string key in keys)
            {
                string errorMessage = string.Join(",", Errors
                    .Where(x => x.PropertyName.Equals(key))
                    .Select(x => x.ErrorMessage).ToArray());

                expandoObjCollection.Add(new KeyValuePair<string, object>(FirstCharToLower(key), errorMessage));
            }

            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }

        private static string FirstCharToLower(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToLower() + input.Substring(1);
            }
        }
    }
}
